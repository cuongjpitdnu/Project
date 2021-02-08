using KeyCipher;
using KeyMgnt.Common;
using KeyMgnt.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace KeyMgnt.Screens
{
    public partial class CreateKeyForm : BaseForm
    {
        #region variables

        public enum ViewMode { CREATE, EDIT}

        public bool isSuccess { get; set; }

        public MCustomer currentCustomer { get; set; }

        public Params param { get; set; }

        public ViewMode Mode { get; set; } = ViewMode.CREATE;

        #endregion

        public CreateKeyForm()
        {
            InitializeComponent();

            loadDevices();
            txtMachineCode.Focus();
            btnExport.Enabled = false;
        }

        public void InitForm()
        {
            if (ViewMode.EDIT == Mode)
            {
                txtMachineCode.Text = param.MachineCode;
                txtMac1.Text = param.ListMacs[0];
                txtMac2.Text = param.ListMacs[1];
                txtMac3.Text = param.ListMacs[2];
                txtMac4.Text = param.ListMacs[3];
                txtMac5.Text = param.ListMacs[4];

                btnExport.Enabled = true;
            }
        }

        #region functions

        private void loadDevices()
        {
            try
            {
                List<MDevice> lstDevice = SQLiteCommon.GetALlDevices();
                BindingDataGridView(dgvDevices, lstDevice);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, ex.Message);
            }
        }

        private List<string> getListMacInput()
        {
            List<string> lstMacs = new List<string>();
            var mac1 = this.txtMac1.Text.Trim();
            var mac2 = this.txtMac2.Text.Trim();
            var mac3 = this.txtMac3.Text.Trim();
            var mac4 = this.txtMac4.Text.Trim();
            var mac5 = this.txtMac5.Text.Trim();

            if (!string.IsNullOrEmpty(mac1)) lstMacs.Add(mac1);

            if (!string.IsNullOrEmpty(mac2)) lstMacs.Add(mac2);

            if (!string.IsNullOrEmpty(mac3)) lstMacs.Add(mac3);

            if (!string.IsNullOrEmpty(mac4)) lstMacs.Add(mac4);

            if (!string.IsNullOrEmpty(mac5)) lstMacs.Add(mac5);

            return lstMacs;
        }

        private bool validateCreateKey()
        {
            var txtMachineCode = this.txtMachineCode.Text.Trim();

            if (string.IsNullOrEmpty(txtMachineCode))
            {
                ShowMsg(MessageBoxIcon.Error, "Please input MachineCode!");
                this.txtMachineCode.Focus();

                return false;
            }

            //var lstMacs = getListMacInput();
            //if (lstMacs.Count == 0)
            //{
            //    ShowMsg(MessageBoxIcon.Error, "Please input at least 1 MacAddress!");
            //    this.txtMac1.Focus();

            //    return false;
            //}

            return true;
        }

        #endregion

        #region events

        private void btnCreateKey_Click(object sender, EventArgs e)
        {
            SQLiteCommon.BeginTran();

            try
            {
                string pathOutput = "keys";
                if (!validateCreateKey())
                {
                    return;
                }

                var machineCode = this.txtMachineCode.Text.Trim();
                var mac1 = this.txtMac1.Text.Trim();
                var mac2 = this.txtMac2.Text.Trim();
                var mac3 = this.txtMac3.Text.Trim();
                var mac4 = this.txtMac4.Text.Trim();
                var mac5 = this.txtMac5.Text.Trim();

                //var key = string.Join(",", machineCode, mac1, mac2, mac3, mac4, mac5);
                var key = machineCode;
                string keyEncripted = Cipher.EncryptText(key);

                string fileName = pathOutput + "/" + machineCode.Substring(0, 5) + "_" + DateTime.Now.ToString("yyyyMMdd") + "_keycode.txt";

                if (!Directory.Exists(pathOutput))
                {
                    Directory.CreateDirectory(pathOutput);
                }

                File.WriteAllText(fileName, keyEncripted);

                //INSERT CUSTOMERKEYS
                string sql = "INSERT INTO CUSTOMERKEYS (CUSID, KEYCODE, USERID) VALUES ({0}, '{1}', '{2}')";
                sql = string.Format(sql, currentCustomer.ID, keyEncripted, CURRENT_USER);
                SQLiteCommon.ExecuteSqlNonResult(sql);

                //INSERT KEYDEVICES
                var lstMacs = getListMacInput();
                foreach (var mac in lstMacs)
                {
                    sql = "INSERT INTO KEYDEVICES (KEYCODE, MACHINECODE, MACADDRESS, USERID) VALUES ('{0}','{1}','{2}', '{3}')";
                    sql = string.Format(sql, keyEncripted, machineCode, mac, CURRENT_USER);
                    SQLiteCommon.ExecuteSqlNonResult(sql);
                }

                SQLiteCommon.CommitTran();

                ShowMsg(MessageBoxIcon.Information, "Create Key Success!");
                this.isSuccess = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
                Process.Start("explorer.exe", pathOutput);
            }
            catch (Exception ex)
            {
                SQLiteCommon.RollBackTran();
                ShowMsg(MessageBoxIcon.Error, "Create Key Fail!");
                log.Error(ex);
            }
        }


        private void btnAddDevice_Click(object sender, EventArgs e)
        {
            var lstMacs = getListMacInput();
            MDevice device;
            if (dgvDevices.CurrentRow != null)
            {
                device = (MDevice)dgvDevices.CurrentRow.DataBoundItem;
            }
            else
            {
                ShowMsg(MessageBoxIcon.Warning, "List Device empty!");
                return;
            }

            if (lstMacs.Count == 5)
            {
                ShowMsg(MessageBoxIcon.Warning, "You added max 5 devices!");
                return;
            }

            if (lstMacs.Contains(device.MacAddress))
            {
                ShowMsg(MessageBoxIcon.Warning, "Device is added. Please add a different device!");
                return;
            }

            var mac1 = this.txtMac1.Text.Trim();
            var mac2 = this.txtMac2.Text.Trim();
            var mac3 = this.txtMac3.Text.Trim();
            var mac4 = this.txtMac4.Text.Trim();
            var mac5 = this.txtMac5.Text.Trim();

            if (string.IsNullOrEmpty(mac1)) { this.txtMac1.Text = device.MacAddress; return; }
            if (string.IsNullOrEmpty(mac2)) { this.txtMac2.Text = device.MacAddress; return; }
            if (string.IsNullOrEmpty(mac3)) { this.txtMac3.Text = device.MacAddress; return; }
            if (string.IsNullOrEmpty(mac4)) { this.txtMac4.Text = device.MacAddress; return; }
            if (string.IsNullOrEmpty(mac5)) { this.txtMac5.Text = device.MacAddress; return; }
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Application.StartupPath,
                Title = "Browse Text Files",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "txt",
                Multiselect = false,
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true,
                ReadOnlyChecked = true,
            })
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var fileName = openFileDialog.FileName;
                    var code = File.ReadAllText(fileName);
                    txtMachineCode.Text = code;
                }
            }
        }

        private void CreateKeyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.isSuccess = true;
            this.DialogResult = DialogResult.OK;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var rs = ComfirmMsg("Do you want to export the key?");
            if (rs)
            {
                string pathOutput = "keys";

                string fileName = pathOutput + "/" + param?.MachineCode.Substring(0, 5) + "_" + DateTime.Now.ToString("yyyyMMdd") + "_keycode.txt";

                if (!Directory.Exists(pathOutput))
                {
                    Directory.CreateDirectory(pathOutput);
                }
                File.WriteAllText(fileName, param?.KeyCode);
                Process.Start("explorer.exe", pathOutput);
            }
        }

        #endregion
    }
}
