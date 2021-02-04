using GPCommon;
using GPMain.Common;
using GPMain.Common.Navigation;
using GPMain.Properties;
using GPModels;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GPMain.Views
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

            pictureLogo.Image = Resources.logo_login;
            TitleBar = "Đăng nhập";

            var infoFamily = AppManager.DBManager.GetTable<MFamilyInfo>().FirstOrDefault();
            var dongho = infoFamily?.FamilyName.ToUpper();
            if (!string.IsNullOrEmpty(dongho))
            {
                lblphadodongho.Text = $"Phả đồ dòng họ {dongho}";
            }
            else
            {
                lblphadodongho.Text = string.Empty;
            }

            //this.SetDefaultUI();

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
                AppManager.Dialog.Warning(Resources.MSG_LOGIN_WARNING);
                var objFocus = emptyUser ? txtUserName : txtPassword;
                objFocus.Focus();
                return;
            }

            var user = AppManager.DBManager.GetTable<MUser>().FirstOrDefault(i => i.UserName == txtUserName.Text && i.Password == txtPassword.Text.Sha512());

            if (user == null)
            {
                AppManager.Dialog.Error(Resources.MSG_LOGIN_FAILD);
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