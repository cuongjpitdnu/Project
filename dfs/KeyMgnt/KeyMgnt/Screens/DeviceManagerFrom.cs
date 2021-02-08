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
    public partial class DeviceManagerForm : BaseForm
    {
        public DeviceManagerForm()
        {
            InitializeComponent();
            loadListDevice();
            txtKeySeach.Focus();
        }

        public void SetFristFocus()
        {
            txtKeySeach.Focus();
        }

        #region functions

        private void loadListDevice()
        {
            try
            {
                List<MDevice> lstDevice = searchListDevice();
                BindingDataGridView(dgvDevices, lstDevice);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, ex.Message);
            }
        }

        private List<MDevice> searchListDevice()
        {
            var key = this.txtKeySeach.Text.Trim()?.ToLower();

            try
            {
                List<MDevice> lstDevice = SQLiteCommon.GetALlDevices().Where(i => string.IsNullOrEmpty(key)
                                                                        || i.DeviceName.ToLower().Contains(key)
                                                                        || i.MacAddress.ToLower().Contains(key)).ToList();
                return lstDevice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void exportDeviceList(List<MDevice> data)
        {
            string pathOutput = "excel";

            try
            {
                if (Directory.Exists(pathOutput))
                {
                    File.Delete(pathOutput + "/Devices.xlsx");
                }
                else
                {
                    Directory.CreateDirectory(pathOutput);
                }

                using (var excel = new ExcelPackage())
                {
                    var workSheet = excel.Workbook.Worksheets.Add("Devices");
                    workSheet.Cells[1, 1].Value = "DeviceName";
                    workSheet.Cells[1, 2].Value = "MacAddress";
                    workSheet.Cells[1, 3].Value = "CreateBy";
                    workSheet.Cells[1, 4].Value = "CreateDate";

                    int rowIndex = 2;
                    foreach (var device in data)
                    {
                        workSheet.Cells[rowIndex, 1].Value = device.DeviceName;
                        workSheet.Cells[rowIndex, 2].Value = device.MacAddress;
                        workSheet.Cells[rowIndex, 3].Value = device.UserId;
                        workSheet.Cells[rowIndex, 4].Value = device.CreateDate?.ToString("dd/MM/yyyy");

                        rowIndex++;
                    }

                    excel.SaveAs(new FileInfo(pathOutput + "/Devices.xlsx"));
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

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                List<MDevice> data = searchListDevice();
                exportDeviceList(data);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, ex.Message);
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            DeviceForm deviceForm = new DeviceForm();
            deviceForm.ViewMode = DeviceForm.Mode.CREATE;
            deviceForm.InitForm();
            DialogResult result = deviceForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (deviceForm.isSuccess)
                {
                    loadListDevice();
                }
            }
        }

        private void dgvDivices_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            DeviceForm deviceForm = new DeviceForm();
            deviceForm.ViewMode = DeviceForm.Mode.EDIT;
            deviceForm.paramUpdate = (MDevice)dgvDevices.CurrentRow.DataBoundItem;
            deviceForm.InitForm();
            DialogResult result = deviceForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (deviceForm.isSuccess)
                {
                    loadListDevice();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            loadListDevice();
        }

        private void txtKeySeach_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                loadListDevice();
            }
        }

        #endregion
    }
}
