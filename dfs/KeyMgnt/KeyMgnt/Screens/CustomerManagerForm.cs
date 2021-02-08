using KeyMgnt.Common;
using KeyMgnt.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace KeyMgnt.Screens
{
    public partial class CustomerManagerForm : BaseForm
    {
        public CustomerManagerForm()
        {
            InitializeComponent();
            txtKeySeach.Focus();
            dtpFrom.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day);
            dtpTo.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(1).AddDays(-1);

            loadListCustomer();
        }

        public void SetFristFocus()
        {
            txtKeySeach.Focus();
        }

        #region functions

        private List<MCustomer> searchListCustomer()
        {
            var keySeach = txtKeySeach.Text.Trim().ToLower();
            List<MCustomer> result = new List<MCustomer>();

            try
            {
                List<MCustomer> lstCustomer = SQLiteCommon.GetALlCustomers();
                List<KeyDevice> lstKeyDevice = SQLiteCommon.GetALlKeyDevices();
                List<CustomerKey> lstCusKey = SQLiteCommon.GetALlCustomerKeys();

                result = (from cus in lstCustomer
                          join cusKey in lstCusKey on cus.ID equals cusKey.CusId into grp
                          from g in grp.DefaultIfEmpty()
                          join keyDevice in lstKeyDevice on g?.KeyCode equals keyDevice.KeyCode into grp1
                          from g1 in grp1.DefaultIfEmpty()
                          select new MCustomer
                          {
                              ID = cus.ID,
                              CompanyName = cus.CompanyName,
                              CompanyAddress = cus.CompanyAddress,
                              CompanyMobile = cus.CompanyMobile,
                              CusName = cus.CusName,
                              CusEmail = cus.CusEmail,
                              CusMobile = cus.CusMobile,
                              KeyCode = g != null ? g.KeyCode : "",
                              MachineCode = g1 != null ? g1.MachineCode : "",
                              MacAddress = g1 != null ? g1.MacAddress : "",
                              UserId = g1 != null ? g1.UserId : "",
                              CreateDate = g1?.CreateDate
                          }).Where(i => (string.IsNullOrEmpty(keySeach) ||
                                              i.KeyCode.ToLower().Contains(keySeach) || i.CusName.ToLower().Contains(keySeach) || i.MacAddress.ToLower().Contains(keySeach))).ToList();

                return result;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }
        }

        private void loadListCustomer()
        {
            var keySeach = txtKeySeach.Text.Trim().ToLower();
            DateTime dateFrom = dtpFrom?.Value ?? DateTime.MinValue;
            DateTime dateTo = dtpTo?.Value ?? DateTime.MinValue;
            dateFrom = new DateTime(dateFrom.Year, dateFrom.Month, dateFrom.Day, 0, 0, 0);
            dateTo = new DateTime(dateTo.Year, dateTo.Month, dateTo.Day, 23, 59, 59);

            try
            {
                List<MCustomer> lstCustomer = SQLiteCommon.GetALlCustomers();
                List<KeyDevice> lstKeyDevice = SQLiteCommon.GetALlKeyDevices();
                List<CustomerKey> lstCusKey = SQLiteCommon.GetALlCustomerKeys();

                var lstCusKeyGroup = lstCusKey.GroupBy(i => new { i.CusId, i.KeyCode });

                var data = (from cus in lstCustomer
                            join cusKey in lstCusKeyGroup on cus.ID equals cusKey.Key.CusId into grp
                            from g in grp.DefaultIfEmpty()
                            join keyDevice in lstKeyDevice on g?.Key.KeyCode equals keyDevice.KeyCode into grp1
                            from g1 in grp1.DefaultIfEmpty()
                            select new MCustomer
                            {
                                ID = cus.ID,
                                CompanyName = cus.CompanyName,
                                CompanyAddress = cus.CompanyAddress,
                                CompanyMobile = cus.CompanyMobile,
                                CusName = cus.CusName,
                                CusEmail = cus.CusEmail,
                                CusMobile = cus.CusMobile,
                                KeyCode = g != null ? g.Key.KeyCode : "",
                                MachineCode = g1 != null ? g1.MachineCode : "",
                                MacAddress = g1 != null ? g1.MacAddress : "",
                                UserId = g1 != null ? g1.UserId : "",
                                CreateDate = g1?.CreateDate
                            }).Where(i => (string.IsNullOrEmpty(keySeach) ||
                                              i.KeyCode.ToLower().Contains(keySeach) || i.CusName.ToLower().Contains(keySeach) || i.MacAddress.ToLower().Contains(keySeach)
                                              || i.MachineCode.ToLower().Contains(keySeach))
                                              && (string.IsNullOrEmpty(i.KeyCode) || (i.CreateDate >= dateFrom && i.CreateDate <= dateTo)))
                                .GroupBy(i => new { i.ID, i.CompanyName, i.CompanyAddress, i.CompanyMobile, i.CusName, i.CusMobile, i.CusEmail })
                                .Select(i => new MCustomer
                                {
                                    ID = i.Key.ID,
                                    CompanyName = i.Key.CompanyName,
                                    CompanyAddress = i.Key.CompanyAddress,
                                    CompanyMobile = i.Key.CompanyMobile,
                                    CusName = i.Key.CusName,
                                    CusEmail = i.Key.CusEmail,
                                    CusMobile = i.Key.CusMobile
                                })
                                .ToList();

                BindingDataGridView(dgvCustomerList, data);

                loadCustomerKey();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, ex.Message);
            }
        }

        private void loadCustomerKey()
        {
            try
            {
                List<CustomerKey> lstCusKey = new List<CustomerKey>();
                if (dgvCustomerList.CurrentRow != null)
                {
                    var cusSelected = (MCustomer)dgvCustomerList.CurrentRow.DataBoundItem;
                    lstCusKey = SQLiteCommon.GetALlKeysByCusId(cusSelected.ID);
                }

                BindingDataGridView(dgvCusKeys, lstCusKey);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, ex.Message);
            }
        }

        private void exportCustomerList(List<MCustomer> data)
        {
            string pathOutput = "excel";

            try
            {
                if (Directory.Exists(pathOutput))
                {
                    File.Delete(pathOutput + "/Customers.xlsx");
                }
                else
                {
                    Directory.CreateDirectory(pathOutput);
                }

                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Customers");
                    workSheet.Cells[1, 1].Value = "CompanyName";
                    workSheet.Cells[1, 2].Value = "CompanyAddress";
                    workSheet.Cells[1, 3].Value = "CompanyMobile";
                    workSheet.Cells[1, 4].Value = "CusName";
                    workSheet.Cells[1, 5].Value = "CusEmail";
                    workSheet.Cells[1, 6].Value = "CusMobile";
                    workSheet.Cells[1, 7].Value = "KeyCode";
                    workSheet.Cells[1, 8].Value = "MachineCode";
                    workSheet.Cells[1, 9].Value = "MacAddress";
                    workSheet.Cells[1, 10].Value = "MakeByUser";
                    workSheet.Cells[1, 11].Value = "CreateDate";

                    int rowIndex = 2;
                    foreach (var cus in data)
                    {
                        workSheet.Cells[rowIndex, 1].Value = cus.CompanyName;
                        workSheet.Cells[rowIndex, 2].Value = cus.CompanyAddress;
                        workSheet.Cells[rowIndex, 3].Value = cus.CompanyMobile;
                        workSheet.Cells[rowIndex, 4].Value = cus.CusName;
                        workSheet.Cells[rowIndex, 5].Value = cus.CusEmail;
                        workSheet.Cells[rowIndex, 6].Value = cus.CusMobile;
                        workSheet.Cells[rowIndex, 7].Value = cus.KeyCode;
                        workSheet.Cells[rowIndex, 8].Value = cus.MachineCode;
                        workSheet.Cells[rowIndex, 9].Value = cus.MacAddress;
                        workSheet.Cells[rowIndex, 10].Value = cus.UserId;
                        workSheet.Cells[rowIndex, 11].Value = cus.CreateDate?.ToString("dd/MM/yyyy");

                        rowIndex++;
                    }

                    excel.SaveAs(new FileInfo(pathOutput + "/Customers.xlsx"));
                    Process.Start("explorer.exe", pathOutput);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, "Export excel fail!: " + ex.Message);
            }
        }

        #endregion

        #region events

        private void btnAdd_Click(object sender, EventArgs e)
        {
            CustomerForm cusForm = new CustomerForm();
            cusForm.ViewMode = CustomerForm.Mode.CREATE;
            cusForm.InitForm();
            DialogResult result = cusForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (cusForm.isSuccess)
                {
                    loadListCustomer();
                }
            }
        }

        private void btnSeach_Click(object sender, EventArgs e)
        {
            loadListCustomer();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                List<MCustomer> data = searchListCustomer();
                exportCustomerList(data);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, ex.Message);
            }
        }

        private void btnAddKey_Click(object sender, EventArgs e)
        {
            if (dgvCustomerList.CurrentRow != null)
            {
                var cusSelected = (MCustomer)dgvCustomerList.CurrentRow.DataBoundItem;

                CreateKeyForm createKeyForm = new CreateKeyForm();
                createKeyForm.currentCustomer = cusSelected;
                DialogResult result = createKeyForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (createKeyForm.isSuccess)
                    {
                        loadCustomerKey();
                    }
                }
            }
            else
            {
                ShowMsg(MessageBoxIcon.Warning, "Please select a customer!");
            }
        }

        private void dgvCustomerList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            loadCustomerKey();
        }

        private void txtKeySeach_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char) Keys.Enter)
            {
                loadListCustomer();
            }
        }

        private void dgvCusKeys_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            var cusSelected = (MCustomer)dgvCustomerList.CurrentRow.DataBoundItem;

            CreateKeyForm createKeyForm = new CreateKeyForm();
            createKeyForm.Mode = CreateKeyForm.ViewMode.EDIT;
            createKeyForm.currentCustomer = cusSelected;
            CustomerKey cusKey = null;
            List<string> lstOldMac = new List<string>();

            try
            {
                var x = SQLiteCommon.GetALlKeyDevices();

                cusKey = (CustomerKey)dgvCusKeys.CurrentRow.DataBoundItem;
                if (cusKey == null) return;

                lstOldMac = SQLiteCommon.GetCustomerKeyByKeyCode(cusKey.KeyCode).ListMacAddress;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, ex.Message);
            }

            Params param = new Params
            {
                KeyCode = cusKey.KeyCode,
                MachineCode = cusKey.MachineCode,
                ListMacs = lstOldMac
            };
            createKeyForm.param = param;
            createKeyForm.InitForm();
            DialogResult result = createKeyForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (createKeyForm.isSuccess)
                {
                    loadCustomerKey();
                }
            }
        }

        private void dgvCustomerList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            CustomerForm cusForm = new CustomerForm();
            cusForm.paramUpdate = (MCustomer)dgvCustomerList.CurrentRow.DataBoundItem;
            cusForm.ViewMode = CustomerForm.Mode.EDIT;
            cusForm.InitForm();
            DialogResult result = cusForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (cusForm.isSuccess)
                {
                    loadListCustomer();
                }
            }
        }

        #endregion
    }
}
