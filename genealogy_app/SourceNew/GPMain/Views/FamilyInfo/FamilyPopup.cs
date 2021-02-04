using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ClosedXML.Excel;
using GPCommon;
using GPConst;
using GPMain.Common;
using GPMain.Common.Dialog;
using GPMain.Common.Excel;
using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using GPMain.Properties;
using GPModels;

namespace GPMain.Views.FamilyInfo
{
    public partial class FamilyPopup : BaseUserControl
    {
        public const string KEY_DATA = "DATA_MEMBER";
        public const string KEY_TYPE = "TYPE_SHOW";

        private List<TMember> _lstMember = new List<TMember>();
        List<FamilyPopupEntity> lstAllEvent = new List<FamilyPopupEntity>();
        List<FamilyPopupEntity> lstEventSearch = new List<FamilyPopupEntity>();
        private string _typeShow;

        string timeSelect;

        public FamilyPopup(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            TitleBar = "Danh sách thành viên";

            this.BackColor = AppConst.PopupBackColor;

            timeSelect = this.Params.GetValue<string>("Time");

            _lstMember = this.Params.GetValue<List<TMember>>(KEY_DATA, new List<TMember>());
            var cnt = 0;
            _typeShow = this.Params.GetValue<string>(KEY_TYPE);

            InitialDataGridView(_typeShow);

            lstAllEvent = GetListFamilyPopup(_lstMember);

            lb_titlePopup.Text = _typeShow == "Birthday"
                                 ? "Danh sách sinh nhật gần nhất" : "Danh sách ngày giỗ gần nhất";


            BindingHelper.BindingDataGrid(dgv_lstPopMem, lstAllEvent);
        }

        private void InitialDataGridView(string _typeShow)
        {

            Dictionary<string, string> dicInfoColumn = _typeShow.Equals("Birthday") ? FamilyPopupExcelEntity.NameColumnsTableBirthDay : FamilyPopupExcelEntity.NameColumnsTableDeadDay;
            string[] columns = _typeShow.Equals("Birthday") ? Enum.GetNames(typeof(FamilyPopupExcelEntity.ColumnsTableBirthDay)) : Enum.GetNames(typeof(FamilyPopupExcelEntity.ColumnsTableDeadDay));

            dgv_lstPopMem.Columns.Clear();

            foreach (string col in columns)
            {
                string textColumn = dicInfoColumn[col];
                DataGridViewColumn column = new DataGridViewTextBoxColumn()
                {
                    Name = col,
                    DataPropertyName = col,
                    HeaderText = textColumn
                };
                dgv_lstPopMem.Columns.Add(column);

            }
            if (_typeShow.Equals("Birthday"))
            {
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableBirthDay.Gender.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableBirthDay.Gender.ToString()].Width = 80;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableBirthDay.STT.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableBirthDay.Name.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableBirthDay.BirthDay.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableBirthDay.BirthDay.ToString()].Width = 90;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableBirthDay.Age.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableBirthDay.Date.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableBirthDay.Date.ToString()].Width = 250;
            }
            else
            {
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableDeadDay.Gender.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableDeadDay.Gender.ToString()].Width = 80;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableDeadDay.STT.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableDeadDay.Name.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableDeadDay.DeadDayMoon.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableDeadDay.DeadDayMoon.ToString()].Width = 90;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableDeadDay.DeadDaySun.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableDeadDay.DeadDaySun.ToString()].Width = 90;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableDeadDay.Date.ToString()].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dgv_lstPopMem.Columns[FamilyPopupExcelEntity.ColumnsTableDeadDay.Date.ToString()].Width = 350;
            }
            dgv_lstPopMem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgv_lstPopMem.ColumnHeadersHeight = 50;
        }

        DateTime CreateBirthDaySort(VNDate date)
        {
            int year = DateTime.Now.Year;
            if (!string.IsNullOrEmpty(timeSelect))
            {
                var dateTime = DateTime.Parse(timeSelect);
                year = dateTime.Year;
            }
            return new DateTime(year, date.MonthSun, (date.DaySun == -1 ? DateTime.DaysInMonth(year, date.MonthSun) : date.DaySun));
        }

        string CreateBirthDayString(VNDate date)
        {
            int year = DateTime.Now.Year;
            if (!string.IsNullOrEmpty(timeSelect))
            {
                var dateTime = DateTime.Parse(timeSelect);
                year = dateTime.Year;
            }

            string sday = date.DaySun.ToDayFormat();
            string smonth = date.MonthSun.ToDayFormat();
            string syear = year.ToString();

            if (date.DaySun >= 1 && date.MonthSun >= 1)
            {
                int daysInMonth = DateTime.DaysInMonth(year, date.MonthMoon);
                sday = date.DaySun > daysInMonth ? daysInMonth.ToDayFormat() : sday;
            }

            return $"Dương lịch: {sday}/{smonth}/{syear}";
        }

        DateTime CreateDeadDaySort(VNDate date)
        {
            DateTime dateTime = DateTime.Now;
            if (!string.IsNullOrEmpty(timeSelect))
            {
                dateTime = DateTime.Parse(timeSelect);
            }

            LunarCalendar luna = new LunarCalendar(new DateTime(dateTime.Year, dateTime.Month, 1));
            LunarCalendar lunaEnd = new LunarCalendar(new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month)));

            int year = luna.intLunarMonth == date.MonthMoon ? luna.intLunarYear : lunaEnd.intLunarYear;

            if (date.DayMoon == -1)
            {
                int daysInMonth = luna.GetLunarMonthDays(date.MonthMoon, year, luna.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);
                return luna.GetSolarDate(daysInMonth, date.MonthMoon, year, luna.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);
            }
            else
            {
                return luna.GetSolarDate(date.DayMoon, date.MonthMoon, year, luna.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);
            }
        }

        string CreateDeadDayString(VNDate date)
        {
            DateTime dateTime = DateTime.Now;
            if (!string.IsNullOrEmpty(timeSelect))
            {
                dateTime = DateTime.Parse(timeSelect);
            }

            LunarCalendar luna = new LunarCalendar(new DateTime(dateTime.Year, dateTime.Month, 1));
            LunarCalendar lunaEnd = new LunarCalendar(new DateTime(dateTime.Year, dateTime.Month, DateTime.DaysInMonth(dateTime.Year, dateTime.Month)));

            int year = luna.intLunarMonth == date.MonthMoon ? luna.intLunarYear : lunaEnd.intLunarYear;

            string dayM = date.DayMoon.ToDayFormat();
            string monthM = date.MonthMoon.ToDayFormat();
            string yearM = year.ToString();

            string dayS = date.DaySun.ToDayFormat();
            string monthS = date.MonthSun.ToDayFormat();
            string yearS = DateTime.Now.Year.ToString();

            if (date.DayMoon >= 1 && date.MonthMoon >= 1)
            {
                int daysInMonth = luna.GetLunarMonthDays(date.MonthMoon, year, date.LeapMonth);
                dayM = date.DayMoon > daysInMonth ? daysInMonth.ToDayFormat() : dayM;

                var dateSun = luna.GetSolarDate(date.DayMoon > daysInMonth ? daysInMonth : date.DayMoon, date.MonthMoon, year, date.LeapMonth);
                dayS = dateSun.Day.ToDayFormat();
                monthS = dateSun.Month.ToDayFormat();
                yearS = dateSun.Year.ToYearFormat();
            }
            else
            {
                dayS = 0.ToDayFormat();
                monthS = 0.ToDayFormat();
                yearS = 1.ToYearFormat();
            }

            return $"Âm lịch: {dayM}/{monthM}/{yearM} - {luna.GetYearCanChi(year)}     Dương lịch: {dayS}/{monthS}/{yearS}";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var txtKey = txtKeyword.Text;
            var lstSearch = _lstMember.Where(i => !string.IsNullOrEmpty(txtKey) ? i.Name.ToLower().Contains(txtKey.ToLower()) : true).ToList();

            lstEventSearch = GetListFamilyPopup(lstSearch);

            BindingHelper.BindingDataGrid(dgv_lstPopMem, lstEventSearch);
        }

        private List<FamilyPopupEntity> GetListFamilyPopup(List<TMember> lstMember)
        {
            List<FamilyPopupEntity> lstRet = new List<FamilyPopupEntity>();
            if (_typeShow.Equals("Birthday"))
            {
                lstRet = lstMember.Select(i => new FamilyPopupEntity()
                {
                    Name = i.Name,
                    Gender = (i.Gender == (int)EmGender.Male) ? AppConst.Gender.Male : ((i.Gender == (int)EmGender.FeMale) ? AppConst.Gender.Female : AppConst.Gender.Unknow),
                    Age = (DateTime.Now.Year - i.Birthday.YearSun).ToString(),
                    BirthDay = i.Birthday.ToDateSun(),
                    Date = CreateBirthDayString(i.Birthday),
                    DaySort = CreateBirthDaySort(i.Birthday)
                }).OrderBy(x => x.DaySort).ToList();
            }
            else
            {
                lstRet = lstMember.Select(i => new FamilyPopupEntity()
                {
                    Name = i.Name,
                    Gender = (i.Gender == (int)EmGender.Male) ? AppConst.Gender.Male : ((i.Gender == (int)EmGender.FeMale) ? AppConst.Gender.Female : AppConst.Gender.Unknow),
                    DeadDayMoon = i.DeadDay.ToDateMoon(),
                    DeadDaySun = i.DeadDay.ToDateSun(),
                    Date = CreateDeadDayString(i.DeadDay),
                    DaySort = CreateDeadDaySort(i.DeadDay)
                }).OrderBy(x => x.DaySort).ToList();
            }

            //if (_typeShow == "DeadDay" && (lstRet.FirstOrDefault(x => x.Date.Split('/')[1].Equals("01")) != null) && (lstRet.FirstOrDefault(x => x.Date.Split('/')[1].Equals("12")) != null))
            //{
            //    var lstTemp1 = lstRet.Where(x => x.Date.Split('/')[1].Equals("01")).ToList();
            //    var lstTemp2 = lstRet.Where(x => x.Date.Split('/')[1].Equals("12")).ToList();
            //    lstTemp2.AddRange(lstTemp1);
            //    lstRet = lstTemp2;
            //}

            int cnt = 0;
            lstRet.ForEach(mem => { mem.STT = ++cnt; });
            return lstRet;
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            string pathFile = "";
            bool exportComplete = false;

            lstEventSearch = lstEventSearch.Count == 0 ? lstAllEvent : lstEventSearch;

            if (!lstEventSearch.HasValue())
            {
                AppManager.Dialog.Warning("Không có dữ liệu để in!");
                return;
            }

            try
            {
                var nameSheet = "Danh sách " + ((_typeShow == "Birthday") ? "sinh nhật" : "ngày giỗ") + " tháng " + DateTime.Now.Month;

            ShowDialog:
                pathFile = AppManager.Dialog.SaveFile(nameSheet, DialogManager.DIALOG_FILTER_EXCEL);
                if (string.IsNullOrEmpty(pathFile)) return;

                if (File.Exists(pathFile))
                {
                    try
                    {
                        File.Delete(pathFile);
                    }
                    catch
                    {
                        AppManager.Dialog.Warning("File đã tồn tại và đang được mở");
                        goto ShowDialog;
                    }
                }
                string templatePath = "./Data/Excel/MemberEventTemplate.xltx";

                if (!Directory.Exists("./Temp"))
                {
                    Directory.CreateDirectory("./Temp");
                }
                string tempPath = "./Temp/" + Guid.NewGuid() + ".xlsx";

                File.Copy(templatePath, tempPath);


                using (var exportExcel = new ExportExcelForFamilyPopup())
                {

                    if (_typeShow.Equals("Birthday"))
                    {
                        var lstShow = lstEventSearch.Select(i => new InfoBirthDayFamilyForExportExcel()
                        {
                            STT = i.STT,
                            Name = i.Name,
                            Gender = i.Gender,
                            BirthDay = i.BirthDay,
                            Age = i.Age,
                            Date = i.Date
                        }).ToList();

                        this.Cursor = Cursors.WaitCursor;
                        exportComplete = exportExcel.ExportBirthDayFamily(lstShow, tempPath, pathFile, nameSheet);
                        this.Cursor = Cursors.Default;
                    }
                    else
                    {
                        var lstShow = lstEventSearch.Select(i => new InfoDeadDayFamilyForExportExcel()
                        {
                            STT = i.STT,
                            Name = i.Name,
                            Gender = i.Gender,
                            DeadDaySun = i.DeadDaySun,
                            DeadDayMoon = i.DeadDayMoon,
                            Date = i.Date
                        }).ToList();

                        this.Cursor = Cursors.WaitCursor;
                        exportComplete = exportExcel.ExportDeadDayFamily(lstShow, tempPath, pathFile, nameSheet);
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(AddFamilyHead), ex);
            }
            finally
            {
                if (File.Exists(pathFile) && AppManager.Dialog.Confirm($"Xuất danh sách { ((_typeShow == "Birthday") ? "sinh nhật" : "ngày giỗ") } tháng {DateTime.Now.Month} thành công!\n{Resources.MSG_CONFIRM_OPEN_FILE}") && exportComplete)
                {
                    Process.Start(pathFile);
                }
            }
        }
    }

    public class FamilyPopupEntity
    {
        public int STT { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string BirthDay { get; set; }
        public string DeadDaySun { get; set; }
        public string DeadDayMoon { get; set; }
        public string Age { get; set; }
        public string Date { get; set; }
        public DateTime DaySort { get; set; }
    }
}
