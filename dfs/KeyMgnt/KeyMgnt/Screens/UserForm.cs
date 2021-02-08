using KeyMgnt.Common;
using KeyMgnt.Const;
using KeyMgnt.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyMgnt.Screens
{
    public partial class UserForm : BaseForm
    {
        #region variables

        private string ADD_USER_SUCCESS = "Add new user success!";
        private string ADD_USER_FAIL = "Add new user fail!";

        private string UPDATE_USER_SUCCESS = "Update a user success!";
        private string UPDATE_USER_FAIL = "Update a user fail!";

        private string DELETE_USER_SUCCESS = "Delete a user success!";
        private string DELETE_USER_FAIL = "Delete a user fail!";

        public bool isSuccess { get; set; }

        public MUser paramUpdate { get; set; }

        public enum Mode
        {
            CREATE = 0,
            EDIT = 1
        }

        public Mode ViewMode { get; set; }

        #endregion

        public UserForm()
        {
            InitializeComponent();
            bindingCombobox();
            txtUserName.Focus();
        }

        #region functions

        public void InitForm()
        {
            switch (ViewMode)
            {
                case Mode.CREATE:
                    this.Text = "Add New User";
                    this.lblCaption.Text = "Add New User";
                    this.btnAdd.Visible = true;
                    this.btnAdd.Enabled = true;
                    this.btnUpdate.Visible = false;
                    this.btnUpdate.Enabled = false;
                    this.btnDelete.Enabled = false;
                    this.txtUserName.Focus();
                    break;

                case Mode.EDIT:
                    this.Text = "Edit A User";
                    this.lblCaption.Text = "Edit A User";
                    this.btnAdd.Visible = false;
                    this.btnAdd.Enabled = false;
                    this.btnUpdate.Visible = true;
                    this.btnUpdate.Enabled = true;
                    this.btnDelete.Enabled = true;

                    bindingMUser();
                    break;

                default:
                    break;
            }
        }


        private void bindingMUser()
        {
            this.txtUserName.Enabled = false;
            this.txtUserName.ReadOnly = true;
            this.txtUserName.Text = this.paramUpdate.UserId;
            this.txtPassword.Text = this.paramUpdate.Password;
            this.txtEmail.Text = this.paramUpdate.Email;
            this.txtMobile.Text = this.paramUpdate.Mobile;
            this.txtFullName.Text = this.paramUpdate.FullName;

            if (paramUpdate.Role == 0)
            {
                cboRole.SelectedValue = Config.ROLE_ADMIN;
            }
            else
            {
                cboRole.SelectedValue = Config.ROLE_USERS;
            }

            if ("admin".Equals(paramUpdate?.UserId))
            {
                btnDelete.Enabled = false;
            }
            else
            {
                btnDelete.Enabled = true;
            }

            this.txtPassword.Focus();
        }

        private bool validateUser(bool isInsert = true)
        {
            var userName = this.txtUserName.Text.Trim();
            var password = this.txtPassword.Text.Trim();
            var email = this.txtEmail.Text.Trim();
            var mobile = this.txtMobile.Text.Trim();
            var fullName = this.txtFullName.Text.Trim();

            if (string.IsNullOrEmpty(userName))
            {
                ShowMsg(MessageBoxIcon.Error, "Please input UserName!");
                this.txtUserName.Focus();

                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowMsg(MessageBoxIcon.Error, "Please input Password!");
                this.txtPassword.Focus();

                return false;
            }

            if (string.IsNullOrEmpty(fullName))
            {
                ShowMsg(MessageBoxIcon.Error, "Please input FullName!");
                this.txtFullName.Focus();

                return false;
            }

            //if (string.IsNullOrEmpty(email))
            //{
            //    ShowMsg(MessageBoxIcon.Error, "Please input Email!");
            //    this.txtEmail.Focus();

            //    return false;
            //}

            //if (string.IsNullOrEmpty(mobile))
            //{
            //    ShowMsg(MessageBoxIcon.Error, "Please input Mobile!");
            //    this.txtMobile.Focus();

            //    return false;
            //}

            if (isInsert)
            {
                if (isUserExist(userName))
                {
                    ShowMsg(MessageBoxIcon.Error, "Account is existed! Please input diffirent UserName");
                    this.txtUserName.Focus();

                    return false;
                }
            }

            return true;
        }

        private bool isUserExist(string userName)
        {
            bool result = false;

            try
            {
                string sql = string.Format("SELECT * FROM M_USERS WHERE LOWER(USERID) = '{0}'", userName.ToLower());
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

        private void bindingCombobox()
        {
            List<MUser> rolesData = new List<MUser>();
            var userAdmin = new MUser();
            userAdmin.UserId = "ADMIN"; userAdmin.Role = Config.ROLE_ADMIN;
            rolesData.Add(userAdmin);
            var userOthers = new MUser();
            userOthers.UserId = "USERS"; userOthers.Role = Config.ROLE_USERS;
            rolesData.Add(userOthers);

            cboRole.DataSource = rolesData;
            cboRole.DisplayMember = "UserId";
            cboRole.ValueMember = "Role";
            cboRole.SelectedValue = Config.ROLE_USERS;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!validateUser(false))
            {
                return;
            }

            var userName = this.txtUserName.Text.Trim();
            var password = this.txtPassword.Text.Trim();
            var email = this.txtEmail.Text.Trim();
            var mobile = this.txtMobile.Text.Trim();
            var fullName = this.txtFullName.Text.Trim();
            var role = this.cboRole.SelectedValue ?? 1;

            SQLiteCommon.BeginTran();

            try
            {
                string sql = "UPDATE M_USERS SET PASSWORD = '{0}', FULLNAME = '{1}', EMAIL = '{2}', MOBILE = '{3}' , ROLE = {4} WHERE USERID = '{5}'";
                sql = string.Format(sql, password, fullName, email, mobile, role, paramUpdate.UserId);
                SQLiteCommon.ExecuteSqlNonResult(sql);

                SQLiteCommon.CommitTran();
                ShowMsg(MessageBoxIcon.Information, UPDATE_USER_SUCCESS);
                this.isSuccess = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                SQLiteCommon.RollBackTran();
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, UPDATE_USER_FAIL + " : " + ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var userName = this.txtUserName.Text.Trim();
            var password = this.txtPassword.Text.Trim();
            var email = this.txtEmail.Text.Trim();
            var mobile = this.txtMobile.Text.Trim();
            var fullName = this.txtFullName.Text.Trim();
            var role = this.cboRole.SelectedValue;

            if (!validateUser())
            {
                return;
            }

            try
            {
                SQLiteCommon.BeginTran();
                string sql = "INSERT INTO M_USERS (USERID, PASSWORD, FULLNAME, EMAIL, MOBILE, ROLE, CREATEBY) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', {5}, '{6}')";
                sql = string.Format(sql, userName, password, fullName, email, mobile, role, CURRENT_USER);
                SQLiteCommon.ExecuteSqlNonResult(sql);
                SQLiteCommon.CommitTran();
                ShowMsg(MessageBoxIcon.Information, ADD_USER_SUCCESS);
                this.isSuccess = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                SQLiteCommon.RollBackTran();
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, ADD_USER_FAIL + " : " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            SQLiteCommon.BeginTran();

            try
            {
                string sql = "DELETE FROM M_USERS WHERE USERID = '{0}'";
                sql = string.Format(sql, paramUpdate.UserId);
                SQLiteCommon.ExecuteSqlNonResult(sql);
                SQLiteCommon.CommitTran();
                ShowMsg(MessageBoxIcon.Information, DELETE_USER_SUCCESS);
                this.isSuccess = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                SQLiteCommon.RollBackTran();
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, DELETE_USER_FAIL + " : " + ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.isSuccess = true;
            this.DialogResult = DialogResult.OK;
        }

        #endregion

    }
}
