using KeyMgnt.Common;
using KeyMgnt.Models;
using System;
using System.Windows.Forms;

namespace KeyMgnt.Screens
{
    public partial class DeviceForm : BaseForm
    {
        #region variables

        private string ADD_DEVICE_SUCCESS = "Add new device success!";
        private string ADD_DEVICE_FAIL = "Add new device fail!";

        private string UPDATE_DEVICE_SUCCESS = "Update a device success!";
        private string UPDATE_DEVICE_FAIL = "Update a device fail!";

        private string DELETE_DEVICE_SUCCESS = "Delete a device success!";
        private string DELETE_DEVICE_FAIL = "Delete a device fail!";

        public bool isSuccess { get; set; }

        public MDevice paramUpdate { get; set; }

        public enum Mode
        {
            CREATE = 0,
            EDIT = 1
        }

        public Mode ViewMode { get; set; }

        #endregion

        public DeviceForm()
        {
            InitializeComponent();
            txtDeviceName.Focus();
        }

        #region functions

        public void InitForm()
        {
            switch (ViewMode)
            {
                case Mode.CREATE:
                    this.Text = "Add New Device";
                    this.lblCaption.Text = "Add New Device";
                    this.btnAdd.Visible = true;
                    this.btnAdd.Enabled = true;
                    this.btnUpdate.Visible = false;
                    this.btnUpdate.Enabled = false;
                    this.btnDelete.Enabled = false;
                    this.txtDeviceName.Focus();
                    break;

                case Mode.EDIT:
                    this.Text = "Edit A Device";
                    this.lblCaption.Text = "Edit A Device";
                    this.btnAdd.Visible = false;
                    this.btnAdd.Enabled = false;
                    this.btnUpdate.Visible = true;
                    this.btnUpdate.Enabled = true;
                    this.btnDelete.Enabled = true;

                    bindingMDevice();
                    break;

                default:
                    break;
            }
        }


        private void bindingMDevice()
        {
            this.txtMacAddress.Enabled = false;
            this.txtMacAddress.ReadOnly = true;
            this.txtDeviceName.Text = this.paramUpdate.DeviceName;
            this.txtMacAddress.Text = this.paramUpdate.MacAddress;
            this.txtDeviceName.Focus();
        }

        private bool validateDevice(bool isInsert = true)
        {
            var deviceName = this.txtDeviceName.Text.Trim();
            var macAddress = this.txtMacAddress.Text.Trim();

            if (string.IsNullOrEmpty(deviceName))
            {
                ShowMsg(MessageBoxIcon.Error, "Please input DeviceName!");
                this.txtDeviceName.Focus();

                return false;
            }

            if (string.IsNullOrEmpty(macAddress))
            {
                ShowMsg(MessageBoxIcon.Error, "Please input MacAddress!");
                this.txtMacAddress.Focus();

                return false;
            }

            if (isInsert)
            {
                if (isDeviceExist(macAddress))
                {
                    ShowMsg(MessageBoxIcon.Error, "Device is existed! Please input diffirent MacAddress");
                    this.txtMacAddress.Focus();

                    return false;
                }
            }

            return true;
        }

        private bool isDeviceExist(string macAddress)
        {
            bool result = false;

            try
            {
                string sql = string.Format("SELECT * FROM M_DEVICES WHERE LOWER(MACADDRESS) = '{0}'", macAddress.ToLower());
                var res = SQLiteCommon.ExecuteSqlWithResult(sql);

                if (res != null && res.HasRows)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

            return result;
        }

        #endregion

        #region events

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!validateDevice(false))
            {
                return;
            }

            var deviceName = this.txtDeviceName.Text.Trim();
            var macAddress = this.txtMacAddress.Text.Trim();

            SQLiteCommon.BeginTran();

            try
            {
                string sql = "UPDATE M_DEVICES SET DEVICENAME = '{0}', MACADDRESS = '{1}' WHERE ID = {2}";
                sql = string.Format(sql, deviceName, macAddress, paramUpdate.ID);
                SQLiteCommon.ExecuteSqlNonResult(sql);

                SQLiteCommon.CommitTran();
                ShowMsg(MessageBoxIcon.Information, UPDATE_DEVICE_SUCCESS);
                this.isSuccess = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                SQLiteCommon.RollBackTran();
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, UPDATE_DEVICE_FAIL + " : " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var deviceName = this.txtDeviceName.Text.Trim();
            var macAddress = this.txtMacAddress.Text.Trim();

            if (!validateDevice())
            {
                return;
            }

            try
            {
                SQLiteCommon.BeginTran();
                string sql = "INSERT INTO M_DEVICES (DEVICENAME, MACADDRESS, USERID) VALUES ('{0}', '{1}', '{2}')";
                sql = string.Format(sql, deviceName, macAddress, CURRENT_USER);
                SQLiteCommon.ExecuteSqlNonResult(sql);
                SQLiteCommon.CommitTran();
                ShowMsg(MessageBoxIcon.Information, ADD_DEVICE_SUCCESS);
                this.isSuccess = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                SQLiteCommon.RollBackTran();
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, ADD_DEVICE_FAIL + " : " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SQLiteCommon.BeginTran();

            try
            {
                string sql = "DELETE FROM M_DEVICES WHERE ID = '{0}'";
                sql = string.Format(sql, paramUpdate.ID);
                SQLiteCommon.ExecuteSqlNonResult(sql);
                SQLiteCommon.CommitTran();
                ShowMsg(MessageBoxIcon.Information, DELETE_DEVICE_SUCCESS);
                this.isSuccess = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                SQLiteCommon.RollBackTran();
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, DELETE_DEVICE_FAIL + " : " + ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void DeviceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.isSuccess = true;
            this.DialogResult = DialogResult.OK;
        }

        #endregion
    }
}
