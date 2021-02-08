using BaseCommon;
using System;
using System.Windows.Forms;

namespace WithMySql
{
    public partial class LoginForm : BaseForm
    {

        private const string MSG_ERR_LOGIN = "You have entered incorrect username or password!";
        private const string MSG_ERR_USER_BLANK = "Please input username.";
        private const string MSG_ERR_PASS_BLANK = "Please input password.";

        private bool _blnLogin;

        public bool LoginSusses {
            get
            {
                return _blnLogin;
            }
        }

        public LoginForm()
        {
            InitializeComponent();
            txtUser.Focus();
            _blnLogin = false;
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            if (!ValidateLogin())
            {
                return;
            }

            using (var objDb = new clsDBUltity())
            {
                if (objDb.LoginCheck(txtUser.Text, txtPass.Text))
                {
                    ExitForm(true);
                } else
                {
                    ShowMsg(MessageBoxIcon.Error, MSG_ERR_LOGIN);
                }
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            ExitForm(false);
        }

        private bool ValidateLogin()
        {

            txtUser.Text = txtUser.Text.Trim();

            if (string.IsNullOrEmpty(txtUser.Text))
            {
                txtUser.Focus();
                return ShowMsg(MessageBoxIcon.Warning, MSG_ERR_USER_BLANK);
            }

            if (string.IsNullOrEmpty(txtPass.Text))
            {
                txtPass.Focus();
                return ShowMsg(MessageBoxIcon.Warning, MSG_ERR_PASS_BLANK);
            }

            return true;
        }

        private void ExitForm(bool blnLogin)
        {
            _blnLogin = blnLogin;
            this.Close();
            this.Dispose();
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{tab}");
            }
        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btLogin.PerformClick();
            }
        }
    }
}
