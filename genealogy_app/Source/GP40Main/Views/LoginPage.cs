using System;
using GP40Main.Core;
using GP40Main.Models;
using GP40Main.Services.Navigation;
using GP40Main.Utility;
using static GP40Main.Core.AppConst;

namespace GP40Main.Views
{
    /// <summary>
    /// Meno        : Login Page
    /// Create by   : AKB Nguyen Thanh Tung
    /// </summary>
    public partial class LoginPage : BaseUserControl
    {
        public LoginPage(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            this.SetDefaultUI();

            // Generate Data Base
            GenerateData.CreateDataMTypeName();
            GenerateData.CreateDataMRelation();
            GenerateData.CreateDataMReligion();
            GenerateData.CreateDataMNational();
            GenerateData.CreateDataMSocialNetwork();
            GenerateData.CreateDefaultUserLogin();
            GenerateData.CreateThemeTree();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            txtUserName.Text = txtUserName.Text.Trim();
            txtPassword.Text = txtPassword.Text.Trim();

            var emptyUser = string.IsNullOrEmpty(txtUserName.Text);
            var emptyPass = string.IsNullOrEmpty(txtPassword.Text);

            if (!emptyUser && emptyPass && txtUserName.Focused)
            {
                txtPassword.Focus();
                return;
            }

            if (emptyUser || emptyPass)
            {
                AppManager.Dialog.Warning("Vui lòng không để trống thông tin tên đăng nhập!");
                var objFocus = emptyUser ? txtUserName : txtPassword;
                objFocus.Focus();
                return;
            }

            var user = AppManager.DBManager.GetTable<MUser>().CreateQuery(i => i.UserName == txtUserName.Text && i.Password == txtPassword.Text.Sha512()).FirstOrDefault();

            if (user == null)
            {
                AppManager.Dialog.Warning("Sai thông tin tài khoản đăng nhập. Vui lòng kiểm tra lại!");
                txtUserName.Focus();
                return;
            }

            this.Close(user);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
