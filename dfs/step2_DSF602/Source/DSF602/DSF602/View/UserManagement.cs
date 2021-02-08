using BaseCommon;
using BaseCommon.Core;
using BaseCommon.Utility;
using DSF602.Language;
using DSF602.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace DSF602.View
{
    public partial class UserManagement : BaseForm
    {
        private int _userId;
        public enum emModeApp
        {
            Admin = 1,
            User = 0,
        }

        private enum emModeForm
        {
            CreateNewUser,
            UpdateUser
        }

        private emModeForm _mode = emModeForm.CreateNewUser;

        protected override void SetLanguageControl()
        {
            LanguageHelper.SetValueOf(this, "USERS_TITLE");
            LanguageHelper.SetValueOf(tabUsers, "USERS_TAB_USERS");
            LanguageHelper.SetValueOf(tabChangePassword, "USERS_TAB_CHANGEPASSWORD");
            LanguageHelper.SetValueOf(lblUsername, "USERS_USERNAME");
            LanguageHelper.SetValueOf(lblPassword, "USERS_PASSWORD");
            LanguageHelper.SetValueOf(lblFullName, "USERS_FULLNAME");
            LanguageHelper.SetValueOf(lblEmail, "USERS_EMAIL");
            LanguageHelper.SetValueOf(lblRole, "USERS_ROLE");
            LanguageHelper.SetValueOf(btCreateUser, "USERS_BTN_CREATEUSER");
            LanguageHelper.SetValueOf(btSaveUser, "USERS_BTN_SAVEUSER");
            LanguageHelper.SetValueOf(btSaveChange, "USERS_BTN_SAVECHANGE");
            LanguageHelper.SetValueOf(lblCurrentPass, "USERS_CURRENTPASS");
            LanguageHelper.SetValueOf(lblNewPass, "USERS_NEWPASS");
            LanguageHelper.SetValueOf(lblRetypePass, "USERS_RETYPEPASS");
            LanguageHelper.SetValueOf(colUserName, "USERS_COL_USERNAME");
            LanguageHelper.SetValueOf(colFullName, "USERS_COL_FULLNAME");
            LanguageHelper.SetValueOf(colRole, "USERS_COL_ROLE");

        }
        public UserManagement()
        {
            InitializeComponent();
            IntForm();
            InitDataUserToGrvUser();
        }

        private void IntForm()
        {
            this.Icon = Properties.Resources.user_list;

            // Limit resize form
            this.MinimumSize = this.Size;
            this.MaximumSize = Screen.PrimaryScreen.Bounds.Size;
            grvUser.AutoGenerateColumns = false;

            if (AppManager.UserLogin.Role != (int)emModeApp.Admin)
            {
                tabMngt.TabPages.Remove(tabUsers);
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
            }
        }

        private void InitDataUserToGrvUser()
        {
            using (var objDB = AppManager.GetConnection())
            {
                grvUser.DataSource = objDB.GetMUser();
                grvUser.ClearSelection();
            }
            if (cboRole.Items.Count == 0)
            {
                cboRole.Items.Add(LanguageHelper.GetValueOf("USERS_CBOROLE_ADMIN"));
                cboRole.Items.Add(LanguageHelper.GetValueOf("USERS_CBOROLE_USER"));
                cboRole.SelectedIndex = 0;
            }
        }

        private void tabMngt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabMngt.SelectedTab == tabUsers)
            {
                using (var objDB = AppManager.GetConnection())
                {
                    var listUser = objDB.GetMUser();
                    grvUser.DataSource = listUser;
                    grvUser.ClearSelection();
                }
                if (cboRole.Items.Count == 0)
                {
                    cboRole.Items.Add(LanguageHelper.GetValueOf("USERS_CBOROLE_ADMIN"));
                    cboRole.Items.Add(LanguageHelper.GetValueOf("USERS_CBOROLE_USER"));
                    cboRole.SelectedIndex = 0;
                }
            }
            else if (tabMngt.SelectedTab == tabChangePassword)
            {
                xResetModeChangePassword();
            }
        }

        #region Tab change User
        private void grvUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            xModeUser(emModeForm.UpdateUser);

            _userId = ConvertHelper.CnvNullToInt(grvUser.SelectedCells[0].OwningRow.Cells["colUserId"].Value.ToString());
            txtUserName.Text = ConvertHelper.CnvNullToString(grvUser.Rows[e.RowIndex].Cells[colUserName.Index].Value.ToString());
            txtUserName.ReadOnly = true;

            txtPassword.Text = ConvertHelper.CnvNullToString(grvUser.Rows[e.RowIndex].Cells[colPass.Index].Value.ToString());
            txtFullName.Text = ConvertHelper.CnvNullToString(grvUser.Rows[e.RowIndex].Cells[colFullName.Index].Value.ToString());
            txtEmail.Text = ConvertHelper.CnvNullToString(grvUser.Rows[e.RowIndex].Cells[colEmail.Index].Value.ToString());
            cboRole.SelectedIndex = ConvertHelper.CnvNullToInt(grvUser.Rows[e.RowIndex].Cells[colRole.Index].Value.ToString()) == (int)emModeApp.Admin ? 0 : 1;

            cboRole.Enabled = _userId == AppManager.UserLogin.UserId ? false : true;

            if (e.ColumnIndex == colDeleteUser.Index && _userId != AppManager.UserLogin.UserId)
            {
                int id = ConvertHelper.CnvNullToInt(grvUser.Rows[e.RowIndex].Cells[colUserId.Index].Value.ToString());
                using (var objDB = AppManager.GetConnection())
                {
                    if (clsCommon.ComfirmMsg("MSG_COMFIRM_DELETE"))
                    {
                        if (objDB.DeleteUserById(id))
                        {
                            InitDataUserToGrvUser();
                        }
                    }
                    return;

                }
            }
        }

        private void grvUser_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (e.ColumnIndex == colRole.Index)
            {
                int role = ConvertHelper.CnvNullToInt(e.Value.ToString());
                e.Value = role == 1 ? LanguageHelper.GetValueOf("USERS_CBOROLE_ADMIN") : LanguageHelper.GetValueOf("USERS_CBOROLE_USER");
            }

            if (e.ColumnIndex == colDeleteUser.Index)
            {
                int userId = ConvertHelper.CnvNullToInt(grvUser.Rows[e.RowIndex].Cells[colUserId.Index].Value.ToString());
                e.Value = userId == AppManager.UserLogin.UserId ? string.Empty : LanguageHelper.GetValueOf("USERS_COL_DELETE");
            }
        }

        private void btSaveUser_Click(object sender, EventArgs e)
        {
            if (!xValidateInputUser())
            {
                return;
            }
            var mUser = new MUser();
            mUser.UserName = txtUserName.Text.Trim();
            mUser.Password = txtPassword.Text;
            mUser.FullName = txtFullName.Text;
            mUser.Email = txtEmail.Text;
            mUser.Role = cboRole.SelectedItem.ToString() == LanguageHelper.GetValueOf("USERS_CBOROLE_ADMIN") ? (int)emModeApp.Admin : (int)emModeApp.User;
            mUser.UpdateTime = DateTime.Now;

            this.SetModeWaiting();
            try
            {
                using (var objDB = AppManager.GetConnection())
                {
                    if (_mode == emModeForm.CreateNewUser)
                    {
                        if (!objDB.InserUser(mUser))
                        {
                            ShowMsg(MessageBoxIcon.Error, LanguageHelper.GetValueOf("MSG_INSERT_ERR"));
                            return;
                        }
                    }
                    else
                    {
                        mUser.UserId = _userId;

                        if (!objDB.UpdateUser(mUser))
                        {
                            ShowMsg(MessageBoxIcon.Error, LanguageHelper.GetValueOf("MSG_UPDATE_ERR"));
                            return;
                        }
                    }
                    _mode = emModeForm.UpdateUser;
                    InitDataUserToGrvUser();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                this.SetModeWaiting(false);
            }

        }

        private void btCreateUser_Click(object sender, EventArgs e)
        {
            this.SetModeWaiting();
            try
            {
                xModeUser(emModeForm.CreateNewUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                this.SetModeWaiting(false);
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


            List<string> lstUserName = new List<string>();
            string data = string.Empty;
            foreach (DataGridViewRow row in grvUser.Rows)
            {
                data = row.Cells[colUserName.Index].Value.ToString();
                lstUserName.Add(data);
            }

            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                txtUserName.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_BLANK_USER"), LanguageHelper.GetValueOf("USERS_USERNAME")));
            }

            if (lstUserName.Contains(txtUserName.Text) && _mode != emModeForm.UpdateUser)
            {
                txtUserName.Focus();
                return ShowMsg(MessageBoxIcon.Warning, LanguageHelper.GetValueOf("FOMAT_MSG_EXIST_USER"));
            }


            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                txtUserName.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_BLANK_USER"), LanguageHelper.GetValueOf("USERS_USERNAME")));
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                txtPassword.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_BLANK_USER"), LanguageHelper.GetValueOf("USERS_PASSWORD")));
            }

            if (string.IsNullOrEmpty(txtFullName.Text))
            {
                txtFullName.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_BLANK_USER"), LanguageHelper.GetValueOf("USERS_FULLNAME")));
            }

            return true;
        }
        #endregion

        #region Tab Change Password

        private void btSaveChange_Click(object sender, EventArgs e)
        {
            if (!xValidateChangePassword())
            {
                return;
            }
            var passwordNew = txtNew.Text;
            var passwordCurrent = txtCurent.Text;
            var userId = AppManager.UserLogin.UserId;

            this.SetModeWaiting();
            try
            {
                using (var objDB = AppManager.GetConnection())
                {
                    if (objDB.ChangePassUser(userId, passwordNew))
                    {
                        ShowMsg(MessageBoxIcon.Information, LanguageHelper.GetValueOf("MSG_CHANGE_PASS_SUCCESS"));
                        xResetModeChangePassword();
                    }
                    else
                    {
                        ShowMsg(MessageBoxIcon.Error, LanguageHelper.GetValueOf("MSG_CHANGE_PASS_ERR"));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                this.SetModeWaiting(false);
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
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_BLANK"), LanguageHelper.GetValueOf("MSG_ERR_CURRENT")));
            }

            if (txtCurent.Text != AppManager.UserLogin.Password.ToString())
            {
                {
                    txtCurent.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("MSG_PASS_NOT_SAME"), LanguageHelper.GetValueOf("MSG_ERR_CURRENT")));
                }
            }

            if (string.IsNullOrEmpty(txtNew.Text))
            {
                txtNew.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_BLANK"), LanguageHelper.GetValueOf("MSG_ERR_NEW")));
            }

            if (string.IsNullOrEmpty(txtRetypeNew.Text))
            {
                txtRetypeNew.Focus();
                return ShowMsg(MessageBoxIcon.Warning, string.Format(LanguageHelper.GetValueOf("FOMAT_MSG_BLANK"), LanguageHelper.GetValueOf("MSG_ERR_RETYPENEW")));
            }

            if (txtNew.Text != txtRetypeNew.Text)
            {
                txtRetypeNew.Focus();
                return ShowMsg(MessageBoxIcon.Warning, LanguageHelper.GetValueOf("MSG_PASS_NOT_SAME"));
            }

            return true;
        }

        #endregion Tab Change Password
    }
}
