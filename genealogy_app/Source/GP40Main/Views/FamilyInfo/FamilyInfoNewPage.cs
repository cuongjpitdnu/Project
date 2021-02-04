using System;
using GP40Main.Core;
using GP40Main.Models;
using GP40Main.Services.Navigation;
using GP40Main.Utility;
using static GP40Main.Core.AppConst;

namespace GP40Main.Views.FamilyInfo
{
    /// <summary>
    /// Meno        : Update Family Info
    /// Create by   :
    /// </summary>
    public partial class FamilyInfoNewPage : BaseUserControl
    {
        public FamilyInfoNewPage(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            if (this.Mode == ModeForm.Edit)
            {
                var dataFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>().CreateQuery(i => i.Id == AppManager.LoginUser.FamilyId).FirstOrDefault();

                txtFamilyName.Text = dataFamilyInfo?.FamilyName + "";
                txtFamilyAnniversary.Text = dataFamilyInfo?.FamilyAnniversary?.ToString("dd/MM/yyyy") + "";
                txtFamilyLevel.Text = dataFamilyInfo?.FamilyLevel.ToString();
                txtFamilyHometown.Text = dataFamilyInfo?.FamilyHometown + "";
                txtUserCreated.Text = AppManager.LoginUser.FullName + "";

                btnContinue.Text = "Lưu";
            }
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            try {
                if (string.IsNullOrWhiteSpace(txtFamilyName.Text))
                {
                    AppManager.Dialog.Warning("Vui lòng nhập tên dòng họ!");
                    txtFamilyName.Focus();
                    return;
                }

                var dataFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>().CreateQuery(i => i.Id == AppManager.LoginUser.FamilyId).FirstOrDefault();
                var isModeInsert = dataFamilyInfo == null;

                if (isModeInsert)
                {
                    dataFamilyInfo = new MFamilyInfo();
                    dataFamilyInfo.CreateUser = AppManager.LoginUser.Id;
                }

                dataFamilyInfo.FamilyName = txtFamilyName.Text.Trim();
                dataFamilyInfo.FamilyHometown = txtFamilyHometown.Text.Trim();
                dataFamilyInfo.FamilyAnniversary = ConvertHelper.CnvStringToDateTimeNull(txtFamilyAnniversary.Text.Trim(), null, "dd/MM/yyyy");
                dataFamilyInfo.FamilyLevel = ConvertHelper.CnvNullToInt(txtFamilyLevel.Text, 1);
                dataFamilyInfo.FamilyLevel = dataFamilyInfo.FamilyLevel > 0 ? dataFamilyInfo.FamilyLevel : 1;
                dataFamilyInfo.UpdateUser = AppManager.LoginUser.Id;

                var flagSuccess = isModeInsert ? AppManager.DBManager.GetTable<MFamilyInfo>().InsertOne(dataFamilyInfo)
                                       : AppManager.DBManager.GetTable<MFamilyInfo>().UpdateOne(dataFamilyInfo);

                flagSuccess = flagSuccess ? AppManager.DBManager.GetTable<MUser>().UpdateOne(i => i.Id == AppManager.LoginUser.Id, (dataUpdate) =>
                {
                    dataUpdate.FamilyId = dataFamilyInfo.Id;
                    dataUpdate.FullName = txtUserCreated.Text.Trim();
                }) : flagSuccess;

                if (!flagSuccess)
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }

                AppManager.LoginUser.FamilyId = dataFamilyInfo.Id;
                AppManager.LoginUser.FullName = txtUserCreated.Text.Trim();
                this.Close(dataFamilyInfo);
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(FamilyInfoNewPage), ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
