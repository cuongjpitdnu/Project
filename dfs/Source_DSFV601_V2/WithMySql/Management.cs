using BaseCommon;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WithMySql
{
    public partial class Management : BaseForm
    {
        private const string ADMIN = "Admin";
        private const string USER = "User";

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

        public bool ChangeSetting { get; set; }

        public Management()
        {
            InitializeComponent();
            this.dvcManagement.EventSave += btnSaveSetting_Click;
            IntForm();
        }

        public void InitForm(List<DeviceInfo> lstDeviceInfo)
        {
            this.dvcManagement.SetListDevice(lstDeviceInfo);
        }

        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            var deviceManagement = (DeviceManagement) sender;
            var gridDevice = deviceManagement.GridDeviceSelected;
            var swapDevice = deviceManagement.SwapDeviceSelected;
            var swapOrdinalDevice = deviceManagement.SwapOrdinalDisplayDevice;
            using (var objDb = new clsDBUltity())
            {
                if (objDb.SaveDevice(gridDevice) && objDb.SaveDevice(swapDevice) && objDb.SaveDevice(swapOrdinalDevice))
                {
                    this.dvcManagement.SetListDevice(deviceManagement.ListDevice);
                    this.ChangeSetting = true;
                    ShowMsg(MessageBoxIcon.Information, MSG_SAVE_SETTING_SUCCESS);
                }
                else
                {
                    ShowMsg(MessageBoxIcon.Error, MSG_SAVE_SETTING_ERR);
                }
            }
        }

        private void IntForm()
        {
            // Limit resize form
            this.MinimumSize = this.Size;
            this.MaximumSize = Screen.PrimaryScreen.Bounds.Size;
            grvUser.AutoGenerateColumns = false;

            tabMngt.TabPages.Remove(tabUsers);

            if (clsConfig.ModeApp != clsConfig.emModeApp.Admin)
            {
                tabMngt.TabPages.Remove(tabSetting);
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
            }
            else
            {
                tabMngt.TabPages.Remove(tabChangePassword);
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
            }
            else if (tabMngt.SelectedTab == tabChangePassword)
            {
                xResetModeChangePassword();
            }
        }

        #region Tab User

        private void grvUser_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (e.ColumnIndex == colRole.Index)
            {
                var role = clsCommon.CnvNullToInt(e.Value);
                if (role == (int)clsConfig.emModeApp.Admin)
                {
                    e.Value = ADMIN;
                }
                else if (role == (int)clsConfig.emModeApp.User)
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
                var userId = clsCommon.CnvNullToInt(grvUser.Rows[e.RowIndex].Cells[colUserId.Index].Value);
                if (userId == clsConfig.UserLoginId)
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

            _userid = clsCommon.CnvNullToInt(grvUser.Rows[e.RowIndex].Cells[colUserId.Index].Value);
            txtUserName.Text = clsCommon.CnvNullToString(grvUser.Rows[e.RowIndex].Cells[colUserName.Index].Value);
            txtPassword.Text = clsCommon.CnvNullToString(grvUser.Rows[e.RowIndex].Cells[colUserName.Index].Value);
            txtFullName.Text = clsCommon.CnvNullToString(grvUser.Rows[e.RowIndex].Cells[colFullName.Index].Value);
            txtEmail.Text = clsCommon.CnvNullToString(grvUser.Rows[e.RowIndex].Cells[colEmail.Index].Value);
            cboRole.SelectedIndex = clsCommon.CnvNullToInt(grvUser.Rows[e.RowIndex].Cells[colRole.Index].Value);
            cboRole.Enabled = _userid != clsConfig.UserLoginId;

            if (e.ColumnIndex == colDeleteUser.Index)
            {
                if (_userid == clsConfig.UserLoginId)
                {
                    return;
                }

                if (ComfirmMsg(MSG_COMFIRM_DELETE))
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
                        ShowMsg(MessageBoxIcon.Error, MSG_INSERT_ERR);
                        return;
                    }
                }
                else
                {
                    if (!objDb.UpdateUser(_userid, txtUserName.Text, txtPassword.Text, txtFullName.Text, txtEmail.Text, cboRole.SelectedIndex))
                    {
                        ShowMsg(MessageBoxIcon.Error, MSG_UPDATE_ERR);
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
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK_USER, "username"));
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                txtPassword.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK_USER, "password"));
            }

            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                txtFullName.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK_USER, "full name"));
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
                if (objDb.ChangePasswordUser(clsConfig.UserLoginId, txtCurent.Text, txtNew.Text))
                {
                    ShowMsg(MessageBoxIcon.Information, MSG_CHANGE_PASS_SUCCESS);
                    xResetModeChangePassword();
                }
                else
                {
                    ShowMsg(MessageBoxIcon.Error, MSG_CHANGE_PASS_ERR);
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
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK, "current"));
            }

            if (string.IsNullOrEmpty(txtNew.Text))
            {
                txtNew.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK, "new"));
            }

            if (string.IsNullOrEmpty(txtRetypeNew.Text))
            {
                txtRetypeNew.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_BLANK, "re-type new"));
            }

            if (txtNew.Text != txtRetypeNew.Text)
            {
                txtRetypeNew.Focus();
                return ShowMsg(MessageBoxIcon.Warning, MSG_PASS_NOT_SAME);
            }

            return true;
        }

        #endregion Tab Change Password

    }
}
