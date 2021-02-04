using GPCommon;
using GPMain.Common;
using GPMain.Common.Dialog;
using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using GPMain.Views.Tree.Build;
using GPModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GPMain.Views.FamilyInfo
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
            TitleBar = "Thông tin dòng họ";

            this.BackColor = AppConst.PopupBackColor;

            if (this.Mode == ModeForm.Edit)
            {
                var dataFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>().FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);

                txtFamilyName.Text = dataFamilyInfo?.FamilyName + "";
                if (dataFamilyInfo.FamilyAnniversary != null)
                {
                    LunarCalendar lunarCalendar = new LunarCalendar(dataFamilyInfo.FamilyAnniversary.Value);
                    txtFamilyAnniversary.Text = lunarCalendar.ToString();
                }
                else
                {
                    txtFamilyAnniversary.Text = "";
                }

                txtFamilyLevel.Text = dataFamilyInfo?.FamilyLevel.ToString();
                txtFamilyHometown.Text = dataFamilyInfo?.FamilyHometown + "";
                txtUserCreated.Text = AppManager.LoginUser.FullName + "";

                btnContinue.Text = "Lưu";
            }
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFamilyName.Text))
                {
                    AppManager.Dialog.Warning("Vui lòng nhập tên dòng họ!");
                    txtFamilyName.Focus();
                    return;
                }

                var dataFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>().FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);
                var isModeInsert = dataFamilyInfo == null;

                if (isModeInsert)
                {
                    dataFamilyInfo = new MFamilyInfo();
                    dataFamilyInfo.CreateUser = AppManager.LoginUser.Id;
                    dataFamilyInfo.RegistrationDate = DateTime.Now;
                }

                dataFamilyInfo.FamilyName = txtFamilyName.Text.Trim();
                dataFamilyInfo.FamilyHometown = txtFamilyHometown.Text.Trim();

                if (!string.IsNullOrEmpty(txtFamilyAnniversary.Text))
                {

                    bool leapMoon = txtFamilyAnniversary.Text.Contains("nhuận");
                    var arrTemp = (txtFamilyAnniversary.Text.Replace("nhuận", "")).Trim().Split('/');

                    int day = int.Parse(arrTemp[0]);
                    int month = int.Parse(arrTemp[1]);
                    int year = int.Parse(arrTemp[2]);

                    LunarCalendar lunarCalendar = new LunarCalendar();
                    dataFamilyInfo.FamilyAnniversary = lunarCalendar.GetSolarDate(day, month, year, leapMoon);
                }

                int familyLevelNew = ConvertHelper.CnvNullToInt(txtFamilyLevel.Text, 1);
                dataFamilyInfo.FamilyLevel = familyLevelNew > 0 ? familyLevelNew : 1;
                dataFamilyInfo.UpdateUser = AppManager.LoginUser.Id;

                var flagSuccess = isModeInsert ? AppManager.DBManager.GetTable<MFamilyInfo>().InsertOne(dataFamilyInfo)
                                       : AppManager.DBManager.GetTable<MFamilyInfo>().UpdateOne(dataFamilyInfo);


                MUser mUser = AppManager.LoginUser;

                mUser.FamilyId = dataFamilyInfo.Id;
                mUser.FullName = txtUserCreated.Text.Trim();

                flagSuccess = flagSuccess ? AppManager.DBManager.GetTable<MUser>().UpdateOne(i => i.Id == AppManager.LoginUser.Id, mUser) : flagSuccess;

                if (!flagSuccess)
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }

                var tabTMember = AppManager.DBManager.GetTable<TMember>();
                MemberHelper memberHelper = new MemberHelper();
                TMember member = memberHelper.RootTMember;
                if (member != null)
                {
                    member.LevelInFamily = dataFamilyInfo.FamilyLevel;
                    tabTMember.UpdateOne(i => i.Id == member.Id, member);

                    AppManager.MenuMemberBuffer.ListAllMember.Clear();
                    AppManager.MenuMemberBuffer.ListMember.Clear();
                }

                AppManager.LoginUser.FamilyId = dataFamilyInfo.Id;
                AppManager.LoginUser.FullName = txtUserCreated.Text.Trim();

                if (!isModeInsert)
                {
                    AppManager.Dialog.Ok("Cập nhập thông tin thành công!");
                }

                this.Close(dataFamilyInfo);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(FamilyInfoNewPage), ex);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFamilyAnniversary_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)'/' && !int.TryParse(e.KeyChar.ToString(), out int num))
            {
                e.KeyChar = (char)Keys.None;
            }
        }

        private void btncalender_Click(object sender, EventArgs e)
        {
            DateTime dateTemp = DateTime.Now;

            if (!string.IsNullOrEmpty(txtFamilyAnniversary.Text))
            {
                bool leapMoon = txtFamilyAnniversary.Text.Contains("nhuận");

                var arrTemp = (txtFamilyAnniversary.Text.Replace("nhuận", "")).Trim().Split('/');

                int day = int.Parse(arrTemp[0]);
                int month = int.Parse(arrTemp[1]);
                int year = int.Parse(arrTemp[2]);

                LunarCalendar lunarCalendar = new LunarCalendar();
                dateTemp = lunarCalendar.GetSolarDate(day, month, year, leapMoon);
            }

            CalendarVN calendarVN = new CalendarVN(dateTemp);

            if (calendarVN.ShowDialog() == DialogResult.OK)
            {
                var calendar = calendarVN.SelectLunarDate;
                txtFamilyAnniversary.Text = calendar.ToString();
            }
        }
    }
}
