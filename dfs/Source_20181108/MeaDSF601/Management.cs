namespace MeaDSF601
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class Management : BaseForm
    {
        private const string ADMIN = "Admin";
        private const string USER = "User";
        private const string DEVICE1 = "Device 1";
        private const string DEVICE2 = "Device 2";

        private const string MSG_COMFIRM_DELETE = "Are you sure you want to delete user?";
        private const string MSG_COMFIRM_RESET = "Are you sure you want to reset device current?";

        private const string MSG_INSERT_ERR = "Insert Fail";
        private const string MSG_UPDATE_ERR = "Update Fail";
        private const string MSG_CHANGE_PASS_ERR = "Change password fail";
        private const string MSG_CHANGE_PASS_SUCCESS = "Change password success";
        private const string MSG_PASS_NOT_SAME = "Password does not match the confirm password";
        private const string MSG_SAVE_SETTING_SUCCESS = "Save setting success";
        private const string MSG_SAVE_SETTING_ERR = "Save setting fail";
        private const string MSG_RESET_SETTING_ERR = "Reset setting fail";

        private const string FOMAT_MSG_INPUT_NUMBER = "Please input {0} is a number than 0";
        private const string FOMAT_MSG_BLANK = "Please input {0} password";
        private const string FOMAT_MSG_BLANK_USER = "Please input {0}";
        
        private enum emModeForm
        {
            CreateNewUser,
            UpdateUser
        }

        private emModeForm _mode = emModeForm.CreateNewUser;
        private int _userid;
        private int _deviceid;
        private int _devicetype;
        private bool _changeSetting;

        public bool ChangeSetting
        {
            get { return _changeSetting; }
        }

        public Management()
        {
            InitializeComponent();
            IntForm();
        }

        private void IntForm()
        {
            // Limit resize form
            this.MinimumSize = this.Size;
            this.MaximumSize = Screen.PrimaryScreen.Bounds.Size;

            grvUser.AutoGenerateColumns = false;
            grvDevice.AutoGenerateColumns = false;
            txtPort.KeyPress += onlyInputNumber;
            txtAlarmValue.KeyPress += onlyInputNumber;
            txtPeriod.KeyPress += onlyInputNumber;
            txtFailLevel.KeyPress += onlyInputNumber;
            txtSamples.KeyPress += onlyInputNumber;

            tabMngt.TabPages.Remove(tabUsers);

            if (Common.ModeApp != Common.emModeApp.Admin)
            {
                tabMngt.TabPages.Remove(tabSetting);
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
            } else
            {
                tabMngt.TabPages.Remove(tabChangePassword);
            }

            using (var objDb = new clsDBUltity())
            {
                grvDevice.DataSource = objDb.GetDeviceList();
                grvDevice.ClearSelection();
                if (grvDevice.Rows.Count > 0)
                {
                    grvDevice.Rows[0].Selected = true;
                    grvDevice_CellClick(grvDevice, new DataGridViewCellEventArgs(0, 0));
                }
            }
        }

        private void tabMngt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabMngt.SelectedTab == tabUsers)
            {
                using (var objDb = new clsDBUltity())
                {
                    grvUser.DataSource = objDb.GetMUser();
                    grvUser.ClearSelection();
                }

                if (cboRole.Items.Count == 0)
                {
                    cboRole.Items.Add(ADMIN);
                    cboRole.Items.Add(USER);
                    cboRole.SelectedIndex = 0;
                }
            } else if (tabMngt.SelectedTab == tabChangePassword)
            {
                xResetModeChangePassword();
            }
        }

        #region Tab Setting

        private void grvDevice_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            if (e.ColumnIndex == colDevice.Index)
            {
                var deviceType = Common.CnvNullToInt(e.Value);
                e.Value = xGetDeviceShow(deviceType);
            }
        }

        private void grvDevice_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            var deviceType = Common.CnvNullToInt(grvDevice.Rows[e.RowIndex].Cells[colDevice.Index].Value);
            lblDevice.Text = (xGetDeviceShow(deviceType) + " Setting").Trim();
            txtDeviceName.Text = Common.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colDeviceName.Index].Value);
            txtIpAddress.Text = Common.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colIpAdress.Index].Value);
            txtPort.Text = Common.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colPort.Index].Value);
            txtAlarmValue.Text = Common.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colAlarmValue.Index].Value);
            txtPeriod.Text = Common.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colPeriod.Index].Value);
            txtFailLevel.Text = Common.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colFailLevel.Index].Value);
            txtSamples.Text = Common.CnvNullToString(grvDevice.Rows[e.RowIndex].Cells[colSamples.Index].Value);
            chkActive.Checked = Common.CnvNullToInt(grvDevice.Rows[e.RowIndex].Cells[colActive.Index].Value) > 0;
            _deviceid = Common.CnvNullToInt(grvDevice.Rows[e.RowIndex].Cells[colDeviceId.Index].Value);
            _devicetype = Common.CnvNullToInt(grvDevice.Rows[e.RowIndex].Cells[colDevice.Index].Value);
        }
        
        private void txtIpAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.Split('.').Length - 1 >= 3))
            {
                e.Handled = true;
            }
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            if (grvDevice.CurrentCell != null)
            {
                grvDevice_CellClick(grvDevice, new DataGridViewCellEventArgs(grvDevice.CurrentCell.ColumnIndex, grvDevice.CurrentCell.RowIndex));
            }
        }

        private void btSaveSetting_Click(object sender, EventArgs e)
        {
            if (!xValidateInputDevice())
            {
                return;
            }

            using (var objDb = new clsDBUltity())
            {
                var emDeviceType = clsDBUltity.emDeviceType.None;

                if (_devicetype == (int)clsDBUltity.emDeviceType.Device1)
                {
                    emDeviceType = clsDBUltity.emDeviceType.Device1;
                } else if(_devicetype == (int)clsDBUltity.emDeviceType.Device2) {
                    emDeviceType = clsDBUltity.emDeviceType.Device2;
                }

                if (objDb.SaveDevice(_deviceid, 
                                    txtDeviceName.Text, 
                                    txtIpAddress.Text, 
                                    Common.CnvNullToInt(txtPort.Text),
                                    Common.CnvNullToInt(txtAlarmValue.Text),
                                    Common.CnvNullToInt(txtPeriod.Text),
                                    Common.CnvNullToInt(txtFailLevel.Text),
                                    Common.CnvNullToInt(txtSamples.Text),
                                    chkActive.Checked,
                                    emDeviceType))
                {
                    var intColumnIndexCurrent = grvDevice.CurrentCell.ColumnIndex;
                    var intRowIndexCurrent = grvDevice.CurrentCell.RowIndex;
                    grvDevice.DataSource = objDb.GetDeviceList();
                    grvDevice_CellClick(grvDevice, new DataGridViewCellEventArgs(intColumnIndexCurrent, intRowIndexCurrent));
                    _changeSetting = true;
                    Common.ShowMsg(MessageBoxIcon.Information, MSG_SAVE_SETTING_SUCCESS);
                } else
                {
                    Common.ShowMsg(MessageBoxIcon.Error, MSG_SAVE_SETTING_ERR);
                }
            }
        }

        private void btResetDefault_Click(object sender, EventArgs e)
        {
            if (Common.ComfirmMsg(MSG_COMFIRM_RESET))
            {
                using (var objDb = new clsDBUltity())
                {
                    if (!objDb.ResetDefauleDevice(_deviceid, _devicetype))
                    {
                        Common.ShowMsg(MessageBoxIcon.Error, MSG_RESET_SETTING_ERR);
                        return;
                    }
                    var intColumnIndexCurrent = grvDevice.CurrentCell.ColumnIndex;
                    var intRowIndexCurrent = grvDevice.CurrentCell.RowIndex;
                    grvDevice.DataSource = objDb.GetDeviceList();
                    grvDevice_CellClick(grvDevice, new DataGridViewCellEventArgs(intColumnIndexCurrent, intRowIndexCurrent));
                    _changeSetting = true;
                }
            }
        }

        private bool xValidateInputDevice()
        {
            txtDeviceName.Text = txtDeviceName.Text.Trim();
            txtIpAddress.Text = txtIpAddress.Text.Trim();
            txtPort.Text = txtPort.Text.Trim();
            txtAlarmValue.Text = txtAlarmValue.Text.Trim();
            txtPeriod.Text = txtPeriod.Text.Trim();
            txtFailLevel.Text = txtFailLevel.Text.Trim();
            txtSamples.Text = txtSamples.Text.Trim();
            
            if (string.IsNullOrEmpty(txtIpAddress.Text))
            {
                txtIpAddress.Focus();
                return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK_USER, "Ip Address"));
            }

            if (string.IsNullOrEmpty(txtPort.Text))
            {
                txtPort.Focus();
                return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK_USER, "Port"));
            }

            if (!Common.IsNumberAndThanZero(txtAlarmValue.Text))
            {
                txtAlarmValue.Focus();
                return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Alarm Value"));
            }

            if (!Common.IsNumberAndThanZero(txtPeriod.Text))
            {
                txtPeriod.Focus();
                return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Period"));
            }

            if (!Common.IsNumberAndThanZero(txtFailLevel.Text))
            {
                txtFailLevel.Focus();
                return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Fail Level"));
            }

            if (!Common.IsNumberAndThanZero(txtSamples.Text))
            {
                txtSamples.Focus();
                return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Samples"));
            }
            
            return true;
        }
        
        private string xGetDeviceShow(int deviceType)
        {
            if (deviceType == (int)clsDBUltity.emDeviceType.Device1)
            {
                return DEVICE1;
            }
            else if (deviceType == (int)clsDBUltity.emDeviceType.Device2)
            {
                return DEVICE2;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion Tab Setting

        #region Tab User

        private void grvUser_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (e.ColumnIndex == colRole.Index)
            {
                var role = Common.CnvNullToInt(e.Value);
                if (role == (int)Common.emModeApp.Admin)
                {
                    e.Value = ADMIN;
                }
                else if (role == (int)Common.emModeApp.User)
                {
                    e.Value = USER;
                }
                else
                {
                    e.Value = string.Empty;
                }
            }

            if (e.ColumnIndex == colDeleteUser.Index)
            {
                var userId = Common.CnvNullToInt(grvUser.Rows[e.RowIndex].Cells[colUserId.Index].Value);
                if (userId == Common.UserLoginId)
                {
                    e.Value = string.Empty;
                }
            }
        }

        private void grvUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            xModeUser(emModeForm.UpdateUser);

            _userid = Common.CnvNullToInt(grvUser.Rows[e.RowIndex].Cells[colUserId.Index].Value);
            txtUserName.Text = Common.CnvNullToString(grvUser.Rows[e.RowIndex].Cells[colUserName.Index].Value);
            txtPassword.Text = Common.CnvNullToString(grvUser.Rows[e.RowIndex].Cells[colUserName.Index].Value);
            txtFullName.Text = Common.CnvNullToString(grvUser.Rows[e.RowIndex].Cells[colFullName.Index].Value);
            txtEmail.Text = Common.CnvNullToString(grvUser.Rows[e.RowIndex].Cells[colEmail.Index].Value);
            cboRole.SelectedIndex = Common.CnvNullToInt(grvUser.Rows[e.RowIndex].Cells[colRole.Index].Value);
            cboRole.Enabled = _userid != Common.UserLoginId;
            
            if (e.ColumnIndex == colDeleteUser.Index)
            {
                if (_userid == Common.UserLoginId)
                {
                    return;
                }

                if (Common.ComfirmMsg(MSG_COMFIRM_DELETE))
                {
                    using (var objDb = new clsDBUltity())
                    {
                        objDb.DeleteUserById(_userid);
                        xModeUser(emModeForm.CreateNewUser);
                        grvUser.DataSource = objDb.GetMUser();
                        grvUser.ClearSelection();
                    }
                }
            }
        }

        private void btCreateUser_Click(object sender, EventArgs e)
        {
            xModeUser(emModeForm.CreateNewUser);
        }

        private void btSaveUser_Click(object sender, EventArgs e)
        {
            if (!xValidateInputUser())
            {
                return;
            }

            using (var objDb = new clsDBUltity())
            {
                if (_mode == emModeForm.CreateNewUser)
                {
                    if (!objDb.InsertUser(txtUserName.Text, txtPassword.Text, txtFullName.Text, txtEmail.Text, cboRole.SelectedIndex))
                    {
                        Common.ShowMsg(MessageBoxIcon.Error, MSG_INSERT_ERR);
                        return;
                    }
                }
                else
                {
                    if (!objDb.UpdateUser(_userid, txtUserName.Text, txtPassword.Text, txtFullName.Text, txtEmail.Text, cboRole.SelectedIndex))
                    {
                        Common.ShowMsg(MessageBoxIcon.Error, MSG_UPDATE_ERR);
                        return;
                    }
                }

                xModeUser(emModeForm.CreateNewUser);
                grvUser.DataSource = objDb.GetMUser();
                grvUser.ClearSelection();
            }
        }

        private void xModeUser(emModeForm mode)
        {
            _mode = mode;

            if (_mode == emModeForm.CreateNewUser)
            {
                txtUserName.Text = string.Empty;
                txtPassword.Text = string.Empty;
                txtFullName.Text = string.Empty;
                txtEmail.Text = string.Empty;
                cboRole.SelectedIndex = 0;
                txtUserName.ReadOnly = false;
                cboRole.Enabled = true;
            }
            else
            {
                txtUserName.ReadOnly = true;
            }
        }

        private bool xValidateInputUser()
        {
            txtUserName.Text = txtUserName.Text.Trim();
            txtFullName.Text = txtFullName.Text.Trim();

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                txtUserName.Focus();
                return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK_USER, "username"));
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                txtPassword.Focus();
                return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK_USER, "password"));
            }

            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                txtFullName.Focus();
                return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK_USER, "full name"));
            }
            
            return true;
        }

        #endregion Tab User

        #region Tab Change Password

        private void btSaveChange_Click(object sender, EventArgs e)
        {
            if (!xValidateChangePassword())
            {
                return;
            }

            using (var objDb = new clsDBUltity())
            {
                if (objDb.ChangePasswordUser(Common.UserLoginId, txtCurent.Text, txtNew.Text))
                {
                    Common.ShowMsg(MessageBoxIcon.Information, MSG_CHANGE_PASS_SUCCESS);
                    xResetModeChangePassword();
                }
                else
                {
                    Common.ShowMsg(MessageBoxIcon.Error, MSG_CHANGE_PASS_ERR);
                }
            }
        }

        private void xResetModeChangePassword()
        {
            txtCurent.Text = string.Empty;
            txtNew.Text = string.Empty;
            txtRetypeNew.Text = string.Empty;
        }

        private bool xValidateChangePassword()
        {
            if (string.IsNullOrEmpty(txtCurent.Text))
            {
                txtCurent.Focus();
                return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK, "current"));
            }

            if (string.IsNullOrEmpty(txtNew.Text))
            {
                txtNew.Focus();
                return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK, "new"));
            }

            if (string.IsNullOrEmpty(txtRetypeNew.Text))
            {
                txtRetypeNew.Focus();
                return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK, "re-type new"));
            }

            if (txtNew.Text != txtRetypeNew.Text)
            {
                txtRetypeNew.Focus();
                return Common.ShowMsg(MessageBoxIcon.Warning, MSG_PASS_NOT_SAME);
            }

            return true;
        }

        #endregion Tab Change Password

    }
}
