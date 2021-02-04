using System;
using GP40Main.Core;
using GP40Main.Models;
using GP40Main.Services.Navigation;
using static GP40Main.Core.AppConst;

namespace GP40Main.Views.FamilyInfo
{
    /// <summary>
    /// Meno        : Add/Edit family history
    /// Create by   : AKB Bùi Minh Chiến
    /// </summary>
    public partial class FamilyTimeline : BaseUserControl
    {
        public FamilyTimeline(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            if (this.GetMode() == ModeForm.Edit)
            {
                var objParam = this.GetParameters().GetValue<MFamilyTimeline>();
                txtStartDate.Text = objParam.StartDate;
                txtEndDate.Text = objParam.EndDate;
                txtContent.Text = objParam.Content;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try {
                txtStartDate.Text = txtStartDate.Text.Trim();
                txtEndDate.Text = txtEndDate.Text.Trim();
                txtContent.Text = txtContent.Text.Trim();

                if (string.IsNullOrEmpty(txtStartDate.Text) && string.IsNullOrEmpty(txtEndDate.Text))
                {
                    AppManager.Dialog.Warning("Vui lòng nhập thời gian tương ứng!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtContent.Text))
                {
                    AppManager.Dialog.Warning("Vui lòng nhập nội dung!");
                    return;
                }

                var mFamilyTimeLine = AppManager.DBManager.GetTable<MFamilyTimeline>();
                var objParam = this.GetParameters().GetValue<MFamilyTimeline>();
                var objFamilyTimeline = mFamilyTimeLine.CreateQuery(i => i.Id == objParam.Id).FirstOrDefault();
                var isModeInsert = objFamilyTimeline == null;

                if (isModeInsert)
                {
                    objFamilyTimeline = new MFamilyTimeline();
                }

                objFamilyTimeline.StartDate = txtStartDate.Text.Trim();
                objFamilyTimeline.EndDate = txtEndDate.Text.Trim();
                objFamilyTimeline.Content = txtContent.Text.Trim();

                var flagSuccess = isModeInsert ? mFamilyTimeLine.InsertOne(objFamilyTimeline)
                                               : mFamilyTimeLine.UpdateOne(objFamilyTimeline);

                if (!flagSuccess)
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }

                this.Close();
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(FamilyTimeline), ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
