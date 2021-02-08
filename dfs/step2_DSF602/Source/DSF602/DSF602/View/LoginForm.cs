using BaseCommon.Core;
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

namespace DSF602.View
{
    public partial class LoginForm : BaseForm
    {
        public LoginForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.login;
            txtUser.Focus();
        }

        protected override void SetLanguageControl()
        {
            LanguageHelper.SetValueOf(this, "MAIN_TITLE");
            LanguageHelper.SetValueOf(lblUserName, "LOGIN_USERNAME");
            LanguageHelper.SetValueOf(lblPassword, "LOGIN_PASSWORD");
            LanguageHelper.SetValueOf(btnLogin, "LOGIN_BTN_LOGIN");
            LanguageHelper.SetValueOf(btnExit, "LOGIN_BTN_EXIT");

            this.Text = clsCommon.AddVersionApp(this.Text);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.ResultData = null;
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!ValidateLogin())
            {
                return;
            }

            var user = txtUser.Text.Trim();
            var pass = txtPass.Text;

            this.SetModeWaiting();
            try
            {
                using (var objDB = AppManager.GetConnection())
                {
                    var usr = objDB.GetUserLoginInfo(user, pass);
                    if (usr == null)
                    {
                        ShowMsg(MessageBoxIcon.Error, LanguageHelper.GetValueOf("MSG_ERR_LOGIN"));
                        return;
                    }
                    this.ResultData = usr;
                }

                this.Close();
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

        private bool ValidateLogin()
        {
            txtUser.Text = txtUser.Text.Trim();

            if (string.IsNullOrEmpty(txtUser.Text))
            {
                txtUser.Focus();
                return ShowMsg(MessageBoxIcon.Warning, LanguageHelper.GetValueOf("MSG_ERR_USER_BLANK"));
            }

            if (string.IsNullOrEmpty(txtPass.Text))
            {
                txtPass.Focus();
                return ShowMsg(MessageBoxIcon.Warning, LanguageHelper.GetValueOf("MSG_ERR_PASS_BLANK"));
            }

            return true;
        }
    }
}
