using KeyMgnt.Common;
using System;
using System.Windows.Forms;

namespace KeyMgnt.Screens
{
    public partial class LoginForm : BaseForm
    {

        public LoginForm()
        {
            InitializeComponent();
            txtUserName.Focus();

            try
            {
                SQLiteCommon.InitDB();
            }
            catch (Exception e)
            {
                log.Error(e);
                ShowMsg(MessageBoxIcon.Error, e.Message);
            }
        }

        #region events

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = this.txtUserName.Text.Trim();
                string passWord = this.txtPassword.Text.Trim();

                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
                {
                    ShowMsg(MessageBoxIcon.Warning, "Please input UserName Or Password!");
                    this.txtUserName.Focus();
                    return;
                }

                string sql = string.Format("SELECT * FROM M_USERS WHERE LOWER(USERID)='{0}' AND PASSWORD='{1}'", userName.ToLower(), passWord);
                var result = SQLiteCommon.ExecuteSqlWithResult(sql);

                if (result != null && result.HasRows)
                {
                    CURRENT_USER = userName;
                    MainForm mainForm = new MainForm();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    ShowMsg(MessageBoxIcon.Warning, "UserName Or Password Wrong! Please input this again!");
                    this.txtUserName.Focus();
                    this.txtPassword.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                SQLiteCommon.CloseDB();
                Application.Exit();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                ShowMsg(MessageBoxIcon.Error, ex.Message);
            }
        }

        #endregion
    }
}
