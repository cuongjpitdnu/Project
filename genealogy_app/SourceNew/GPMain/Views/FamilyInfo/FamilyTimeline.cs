using GPMain.Common;
using GPMain.Common.Navigation;
using GPModels;
using System;
using System.Windows.Forms;

namespace GPMain.Views.FamilyInfo
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

            if (this.Mode == ModeForm.Edit)
            {
                var objParam = this.Params.GetValue<MFamilyTimeline>();
                txtStartDate.Text = objParam.StartDate;
                txtEndDate.Text = objParam.EndDate;
                txtContent.Text = objParam.Content;
                TitleBar = "Chỉnh sửa lịch sử dòng họ";
            }
            else
            {
                TitleBar = "Thêm lịch sử dòng họ";
            }

            this.BackColor = AppConst.PopupBackColor;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                txtStartDate.Text = txtStartDate.Text.Trim();
                txtEndDate.Text = txtEndDate.Text.Trim();
                txtContent.Text = txtContent.Text.Trim();

                string messValidate = ValidateTimeLine();

                if (!string.IsNullOrEmpty(messValidate))
                {
                    AppManager.Dialog.Error(messValidate);
                    return;
                }

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
                var objParam = this.Params.GetValue<MFamilyTimeline>();
                MFamilyTimeline objFamilyTimeline = null;
                if (objParam != null)
                {
                    objFamilyTimeline = mFamilyTimeLine.FirstOrDefault(i => i.Id == objParam.Id);
                }
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

                this.Close(System.Windows.Forms.DialogResult.OK);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(FamilyTimeline), ex);
            }
        }

        private string ValidateTimeLine()
        {
            string dateStart = txtStartDate.Text;
            string dateEnd = txtEndDate.Text;
            string content = txtContent.Text;

            if (string.IsNullOrEmpty(dateStart))
            {
                txtStartDate.Focus();
                return "Thời gian bắt đầu không được để trống.\nXin vui lòng nhập thời gian bắt đầu!";
            }

            if (string.IsNullOrEmpty(dateEnd))
            {
                txtEndDate.Focus();
                return "Thời gian kết thúc không được để trống.\nXin vui lòng nhập thời gian kết thúc!";
            }

            if (string.IsNullOrEmpty(content))
            {
                txtContent.Focus();
                return "Nội dung lịch sử dòng họ không được để trống.\nXin vui lòng nhập nội dung lịch sử dòng họ!";
            }

            if (!DateTime.TryParse(dateStart, out DateTime date))
            {
                txtStartDate.SelectAll();
                txtStartDate.Focus();
                return "Thời gian bắt đầu không đúng định dạng.\nXin vui lòng nhập thời gian bắt đầu!";
            }

            if (!DateTime.TryParse(dateEnd, out DateTime date2))
            {
                txtEndDate.SelectAll();
                txtEndDate.Focus();
                return "Thời gian kết thúc không đúng định dạng.\nXin vui lòng nhập thời gian kết thúc!";
            }

            return "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(System.Windows.Forms.DialogResult.Cancel);
        }

        private void txtStartDate_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            string filter = "0123456789/";
            if (!filter.Contains(e.KeyChar.ToString()))
            {
                e.KeyChar = (char)Keys.None;
            }
        }
    }
}
