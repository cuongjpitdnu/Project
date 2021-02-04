using GPCommon;
using GPConst;
using GPMain.Common;
using GPMain.Common.Database;
using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using GPMain.Views.Tree.Build;
using GPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace GPMain.Views.FamilyInfo
{
    /// <summary>
    /// Meno        : Display Family Info
    /// Create by   : AKB Bùi Minh Chiến
    /// </summary>
    public partial class FamilyInfoFullPage : BaseUserControl
    {
        public FamilyInfoFullPage(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            var objLstMember = AppManager.DBManager.GetTable<TMember>().ToList();
            LoadFamilyInfo();
            LoadFamilyTimeline();
            LoadStatisticsInfo(objLstMember);

            UIHelper.SetColumnDeleteAction<TMember>(gridFamilyHead, rowSelected =>
            {
                if (rowSelected == null)
                {
                    return;
                }
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }
                var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
                var objMFamilyInfo = tblMFamilyInfo.FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);
                if (objMFamilyInfo != null)
                {
                    if (objMFamilyInfo.ListFamilyHead.Contains(rowSelected.Id))
                    {
                        objMFamilyInfo.ListFamilyHead.Remove(rowSelected.Id);
                        // update new current family head
                        objMFamilyInfo.CurrentFamilyHead = (objMFamilyInfo.ListFamilyHead.HasValue()) ? objMFamilyInfo.ListFamilyHead[objMFamilyInfo.ListFamilyHead.Count - 1] : "";

                        if (!tblMFamilyInfo.UpdateOne(i => i.Id == objMFamilyInfo.Id, objMFamilyInfo))
                        {
                            AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                            return;
                        }
                        LoadFamilyInfo();
                    }
                }
            });

            UIHelper.SetColumnDeleteAction<MFamilyTimeline>(tblEvent, rowselected =>
            {
                var tabmFamilyTimeLine = AppManager.DBManager.GetTable<MFamilyTimeline>();
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }
                rowselected.DeleteDate = DateTime.Now;
                tabmFamilyTimeLine.UpdateOne(m => m.Id == rowselected.Id, rowselected);
                LoadFamilyTimeline();
            });
        }

        #region Event Form

        private void btnEditFamilyInfo_Click(object sender, EventArgs e)
        {

            int levelFamily = 0;
            int.TryParse(txtFamilyLevel.Text, out levelFamily);
            var familyInfo = AppManager.Navigation.ShowDialog<FamilyInfoNewPage, MFamilyInfo>(ModeForm.Edit, AppConst.StatusBarColor);
            LoadFamilyInfo();
            if (familyInfo == null || levelFamily == familyInfo.FamilyLevel)
            {
                return;
            }
            using (MemberHelper memberHelper = new MemberHelper())
            {
                if (memberHelper.RootTMember != null && (AppManager.Dialog.Confirm("Cập nhật lại thứ bậc dòng họ?")))
                {
                    this.Cursor = Cursors.WaitCursor;
                    AppManager.Dialog.ShowProgressBar(progressBar =>
                    {
                        memberHelper.UpdateLevelInFamily(progressBar);
                    }, "Đang cập nhật thứ bậc...", $"{AppConst.TitleBarFisrt}Cập nhật lại thứ bậc");
                    AppManager.Dialog.Ok("Cập nhật thứ bậc kết thúc!");
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void btnEditFamilyTimeline_Click(object sender, EventArgs e)
        {
            AppManager.Navigation.ShowDialog<FamilyTimeline>(ModeForm.New, AppConst.StatusBarColor);
            LoadFamilyTimeline();
        }

        private void tblEvent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var rowSelected = tblEvent.Rows[e.RowIndex].DataBoundItem as MFamilyTimeline;

            if (rowSelected != null
                && AppManager.Navigation.ShowDialogWithParam<FamilyTimeline, MFamilyTimeline>(rowSelected, ModeForm.Edit).Result == DialogResult.OK)
            {
                LoadFamilyTimeline();
            }
        }

        private void gridFamilyHead_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                var rowSelected = gridFamilyHead.Rows[e.RowIndex].DataBoundItem as TMember;

                if (rowSelected == null)
                {
                    return;
                }

                if (e.ColumnIndex == colDelAction.Index)
                {
                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        return;
                    }

                    var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
                    var objMFamilyInfo = tblMFamilyInfo.FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);
                    if (objMFamilyInfo != null)
                    {
                        if (objMFamilyInfo.ListFamilyHead.Contains(rowSelected.Id))
                        {
                            objMFamilyInfo.ListFamilyHead.Remove(rowSelected.Id);
                            // update new current family head
                            objMFamilyInfo.CurrentFamilyHead = (objMFamilyInfo.ListFamilyHead.HasValue()) ? objMFamilyInfo.ListFamilyHead[objMFamilyInfo.ListFamilyHead.Count - 1] : "";

                            if (!tblMFamilyInfo.UpdateOne(i => i.Id == objMFamilyInfo.Id, objMFamilyInfo))
                            {
                                AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                                return;
                            }

                            LoadFamilyInfo();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(FamilyInfoFullPage), ex);
            }
        }

        private void btnAddFamilyHead_Click(object sender, EventArgs e)
        {
            AppManager.Navigation.ShowDialog<AddFamilyHead>(ModeForm.New, AppConst.StatusBarColor);
            LoadFamilyInfo();
        }

        #endregion Event Form

        #region Private Function

        private void LoadFamilyInfo()
        {
            MFamilyInfo familyInfo = AppManager.DBManager.GetTable<MFamilyInfo>().FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);

            if (familyInfo.FamilyAnniversary != null && familyInfo != null)
            {
                LunarCalendar lunarCalendar = new LunarCalendar(familyInfo.FamilyAnniversary.Value);

                txtFamilyAnniversary.Text = lunarCalendar.ToString();
            }
            else
            {
                txtFamilyAnniversary.Text = "";
            }
            txtFamilyName.Text = familyInfo == null ? "" : familyInfo.FamilyName;

            txtFamilyLevel.Text = familyInfo == null ? "" : (familyInfo.FamilyLevel.ToString() ?? null);
            txtFamilyHometown.Text = familyInfo == null ? "" : (familyInfo.FamilyHometown ?? null);
            txtUserCreated.Text = familyInfo == null ? "" : (AppManager.LoginUser.FullName ?? null);

            if (familyInfo != null)
            {
                var lstMember = new List<ExTMember>();
                if (familyInfo.ListFamilyHead.HasValue())
                {
                    foreach (var memberId in familyInfo.ListFamilyHead)
                    {
                        var objMember = AppManager.DBManager.GetTable<TMember>().FirstOrDefault(i => i.Id == memberId);
                        if (objMember != null)
                        {
                            lstMember.Add(new ExTMember()
                            {
                                Id = objMember.Id,
                                Name = objMember.Name + "",
                                BirthdayShow = objMember.Birthday != null ? objMember.Birthday.ToDateSun() : "",
                                Address = objMember.Contact.Address + "",
                            });
                        }
                    }
                }
                BindingHelper.BindingDataGrid(gridFamilyHead, lstMember);
            }
        }

        private void LoadFamilyTimeline()
        {
            var dtaMFamilyTimeline = AppManager.DBManager.GetTable<MFamilyTimeline>().CreateQuery().OrderBy(i => i.StartDate).ToList();
            BindingHelper.BindingDataGrid(tblEvent, dtaMFamilyTimeline);
        }

        private void LoadStatisticsInfo(List<TMember> lstMember)
        {
            using (var staticManager = new Statistics())
            {
                txtTotalMember.Text = string.Format(AppConst.FormatNumber, staticManager.TotalMember.Total);
                txtPercentDear.Text = staticManager.TotalStatusMember(false).ToFomat("{0:#,0} người ({1: 0.00 })");
                txtPercentLiving.Text = staticManager.TotalStatusMember(true).ToFomat("{0:#,0} người ({1: 0.00 })");
                txtPercentMale.Text = staticManager.TotalGenderMember(EmGender.Male).ToFomat("{0:#,0} người ({1: 0.00 })");
                txtPercentFemale.Text = staticManager.TotalGenderMember(EmGender.FeMale).ToFomat("{0:#,0} người ({1: 0.00 })");
                txtPercentUnknown.Text = staticManager.TotalGenderMember(EmGender.Unknown).ToFomat("{0:#,0} người ({1: 0.00 })");
                txtAge05.Text = staticManager.TotalBettweenAge(0, 5).ToFomat("{0:#,0} người ({1: 0.00 })");
                txtAge617.Text = staticManager.TotalBettweenAge(6, 17).ToFomat("{0:#,0} người ({1: 0.00 })");
                txtAge1835.Text = staticManager.TotalBettweenAge(18, 35).ToFomat("{0:#,0} người ({1: 0.00 })");
                txtAge3655.Text = staticManager.TotalBettweenAge(36, 55).ToFomat("{0:#,0} người ({1: 0.00 })");
                txtAge5671.Text = staticManager.TotalBettweenAge(56, 71).ToFomat("{0:#,0} người ({1: 0.00 })");
                txtAgeOver71.Text = staticManager.TotalBettweenAge(72, 0).ToFomat("{0:#,0} người ({1: 0.00 })");

                llb_lstBirthday.Text = $"D.sách sinh nhật tháng {DateTime.Now.Month} dương lịch.";
                llb_lstDeadDay.Text = $"D.sách ngày giỗ tháng {DateTime.Now.Month} dương lịch.";
            }
        }

        #endregion Private Function

        private void llb_lstBirthday_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Expression<Func<TMember, bool>> filterMem = (i =>
               (i.Birthday.MonthSun.Equals(DateTime.Now.Month))
                && i.IsDeath == false
            );
            using (var staticManager = new Statistics(filterMem))
            {
                var param = new NavigationParameters();
                param.Add(FamilyPopup.KEY_DATA, staticManager.getDataByCondition());
                param.Add(FamilyPopup.KEY_TYPE, "Birthday");

                AppManager.Navigation.ShowDialogWithParam<FamilyPopup>(param, ModeForm.None, AppConst.StatusBarColor);
            }
            this.Cursor = Cursors.Default;
        }

        private void llb_lstDeadDay_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            var timeSelect = DateTime.Now;
            DateTime begionMonth = new DateTime(timeSelect.Year, timeSelect.Month, 1);
            DateTime endMonth = new DateTime(timeSelect.Year, timeSelect.Month, DateTime.DaysInMonth(timeSelect.Year, timeSelect.Month));
            LunarCalendar objBeginMonth = new LunarCalendar(begionMonth);
            LunarCalendar objEndMonth = new LunarCalendar(endMonth);

            var lstMember = AppManager.DBManager.GetTable<TMember>().AsEnumerable().Where(i =>
                    (CompareDeadDay(i.DeadDay, begionMonth, endMonth) ||
                    (i.DeadDay.DayMoon == -1 && (i.DeadDay.MonthMoon == objBeginMonth.intLunarMonth || i.DeadDay.MonthMoon == objEndMonth.intLunarMonth)))
                    && i.IsDeath && objBeginMonth.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.NormalMonth
                    ).ToList();

            var param = new NavigationParameters();
            param.Add(FamilyPopup.KEY_DATA, lstMember);
            param.Add(FamilyPopup.KEY_TYPE, "DeadDay");
            AppManager.Navigation.ShowDialogWithParam<FamilyPopup>(param, ModeForm.None, AppConst.StatusBarColor);
            this.Cursor = Cursors.Default;
        }

        private void tabmainfamily_selectedindexchanged(object sender, EventArgs e)
        {
            var selectedtab = tabFamilyAlbum.SelectedTab;

            if (selectedtab.Name == tabAlbum.Name)
            {
                tabAlbum.Controls.Clear();
                try
                {
                    var frmfamilyimage = new FamilyImage();
                    frmfamilyimage.Dock = DockStyle.Fill;
                    tabAlbum.Controls.Add(frmfamilyimage);
                    frmfamilyimage.BringToFront();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            else if (selectedtab.Name == tabDocument.Name)
            {
                try
                {
                    var doc = new FamilyDocumentList();
                    doc.Dock = DockStyle.Fill;
                    tabDocument.Controls.Add(doc);
                    //frmfamilyimage.BringToFront();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void materialTabSelector1_Click(object sender, EventArgs e)
        {

        }

        private bool CompareDeadDay(VNDate deadDay, DateTime begionMonth, DateTime endMonth)
        {
            if (deadDay == null)
            {
                return false;
            }
            if (deadDay.MonthMoon == -1 || deadDay.DayMoon == -1)
            {
                return false;
            }

            LunarCalendar lunarStart = new LunarCalendar(begionMonth);
            LunarCalendar lunarEnd = new LunarCalendar(endMonth);

            if (lunarStart.intLunarYear == lunarEnd.intLunarYear)
            {
                LunarCalendar lunarEvent = new LunarCalendar();
                lunarEvent.intLunarDay = deadDay.DayMoon;
                lunarEvent.intLunarMonth = deadDay.MonthMoon;
                lunarEvent.intLunarYear = lunarStart.intLunarYear;
                int comp1 = CompareDateTime(lunarStart, lunarEvent);
                int comp2 = CompareDateTime(lunarEnd, lunarEvent);
                return comp1 != -1 && comp2 != 1;
            }
            else if (lunarStart.intLunarYear < lunarEnd.intLunarYear)
            {
                if (deadDay.MonthMoon == lunarStart.intLunarMonth)
                {
                    LunarCalendar lunarEvent = new LunarCalendar();
                    lunarEvent.intLunarDay = deadDay.DayMoon;
                    lunarEvent.intLunarMonth = deadDay.MonthMoon;
                    lunarEvent.intLunarYear = lunarStart.intLunarYear;
                    int comp1 = CompareDateTime(lunarStart, lunarEvent);
                    int comp2 = CompareDateTime(lunarEnd, lunarEvent);
                    return comp1 != -1 && comp2 != 1;
                }
                else if (deadDay.MonthMoon == lunarEnd.intLunarMonth)
                {
                    LunarCalendar lunarEvent = new LunarCalendar();
                    lunarEvent.intLunarDay = deadDay.DayMoon;
                    lunarEvent.intLunarMonth = deadDay.MonthMoon;
                    lunarEvent.intLunarYear = lunarEnd.intLunarYear;
                    int comp1 = CompareDateTime(lunarStart, lunarEvent);
                    int comp2 = CompareDateTime(lunarEnd, lunarEvent);
                    return comp1 != -1 && comp2 != 1;
                }
            }
            return false;
        }
        public int CompareDateTime(LunarCalendar time1, LunarCalendar time2)
        {
            if (time2.intLunarYear > time1.intLunarYear) { return 1; }
            else if (time2.intLunarYear == time1.intLunarYear)
            {
                if (time2.intLunarMonth > time1.intLunarMonth) { return 1; }
                else if (time2.intLunarMonth == time1.intLunarMonth)
                {
                    if (!(time1.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth) && (time2.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth)) return 1;
                    else if ((time1.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth) && !(time2.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth)) return -1;
                    else
                    {
                        if (time2.intLunarDay > time1.intLunarDay) { return 1; }
                        else if (time2.intLunarDay == time1.intLunarDay)
                        {
                            return 0;
                        }
                        else { return -1; }
                    }
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        }
    }
}
