using GPCommon;
using GPConst;
using GPMain.Common;
using GPMain.Common.Database;
using GPMain.Common.Dialog;
using GPMain.Common.Events;
using GPMain.Common.Excel;
using GPMain.Common.FamilyEvent;
using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using GPMain.Properties;
using GPMain.Views.FamilyInfo;
using GPModels;
using MaterialSkin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace GPMain.Views
{
    public partial class Events : BaseUserControl
    {
        public Events(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            for (int col = 0; col < dgvEvent.Columns.Count; col++)
            {
                dgvEvent.Columns[col].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            string[] Key = typeof(GPConst.ReplyType).GetFields().ToList().Select(x => x.Name).ToArray();
            string[] Display = typeof(GPConst.ReplyType).GetFields().ToList().Select(x => x.GetValue(x.Name).ToString()).ToArray();
            List<ModelBinding> dataSourceReplyType = new List<ModelBinding>();
            for (int i = 0; i < Key.Length; i++)
            {
                dataSourceReplyType.Add(new ModelBinding() { Key = Key[i], Display = Display[i] });
            }
            dataSourceReplyType.Insert(0, new ModelBinding() { Key = EnumReplyType.ALL.ToString(), Display = "Tất cả" });
            cbrepType.DataSource = dataSourceReplyType;
            cbrepType.DisplayMember = "Display";
            cbrepType.ValueMember = "Key";
            cbrepType.SelectedIndex = 0;

            List<ModelBinding> dataSourceEventStatus = new List<ModelBinding>();
            dataSourceEventStatus.Add(new ModelBinding() { Key = EnumEventStatus.ALL.ToString(), Display = "Tất cả" });
            dataSourceEventStatus.Add(new ModelBinding() { Key = EnumEventStatus.OPEN.ToString(), Display = "Chưa kết thúc" });
            dataSourceEventStatus.Add(new ModelBinding() { Key = EnumEventStatus.CLOSE.ToString(), Display = "Đã kết thúc" });
            cbeventStatus.DataSource = dataSourceEventStatus;
            cbeventStatus.DisplayMember = "Display";
            cbeventStatus.ValueMember = "Key";
            cbeventStatus.SelectedIndex = 0;

            List<ModelBinding> dataSourceCalendarType = new List<ModelBinding>();
            dataSourceCalendarType.Add(new ModelBinding() { Key = EnumCalendarType.ALL.ToString(), Display = "Tất cả" });
            dataSourceCalendarType.Add(new ModelBinding() { Key = EnumCalendarType.MOON.ToString(), Display = "Âm" });
            dataSourceCalendarType.Add(new ModelBinding() { Key = EnumCalendarType.SUN.ToString(), Display = "Dương" });
            cbcalendarType.DataSource = dataSourceCalendarType;
            cbcalendarType.DisplayMember = "Display";
            cbcalendarType.ValueMember = "Key";
            cbcalendarType.SelectedIndex = 0;
        }
        Common.Database.ReposityLiteTable<TEvent> tbEvent;
        DateTime timeSelect;
        DateTime monthSelect;
        List<EventForShowGridView> dataShow = new List<EventForShowGridView>();
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        private void ListFamilyEvent_Load(object sender, EventArgs e)
        {
            UIHelper.SetColumnEditAction<EventForShowGridView>(dgvEvent, selected =>
            {
                NavigationParameters param = new NavigationParameters();
                param.Add("TEvent", selected);
                AppManager.Navigation.ShowDialogWithParam<AddFamilyEvent>(param, ModeForm.Edit, AppConst.StatusBarColor);
                LoadEventToDataGridView(timeSelect);
                calendar1.Refresh();
            });
            UIHelper.SetColumnDeleteAction<EventForShowGridView>(dgvEvent, selected =>
            {
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }
                var even = tbEvent.FirstOrDefault(x => x.Id == selected.Id);
                even.DeleteDate = DateTime.Now;
                even.activate = false;
                tbEvent.UpdateOne(x => x.Id == selected.Id, even);
                Enum.TryParse<EnumReplyType>(cbrepType.SelectedValue?.ToString(), out EnumReplyType replyType);
                Enum.TryParse<EnumEventStatus>(cbeventStatus.SelectedValue?.ToString(), out EnumEventStatus eventStatus);
                Enum.TryParse<EnumCalendarType>(cbcalendarType.SelectedValue?.ToString(), out EnumCalendarType calendarType);
                LoadEventToDataGridView(timeSelect, txtsearchEvent.Text, replyType, eventStatus, calendarType);
                calendar1.Refresh();
            });
            tbEvent = AppManager.DBManager.GetTable<TEvent>();

            timeSelect = calendar1.CalendarSolarDate;

            calendar1.SelectDay += (sender, day) =>
            {
                lbldayinfo.Text = GetDayInfo(day);
                timeSelect = day;
                if (dataShow == null || dataShow.Count == 0) return;
                LunarCalendar lunarCalendar = new LunarCalendar(day);
                List<string> lstSTT = new List<string>();
                dataShow.ForEach(x =>
                {
                    string sDateStart = x.TimeEventStart;
                    string sDateEnd = x.TimeEventEnd;
                    string[] arrTimeStart = sDateStart.Split('-');
                    string[] arrTimeEnd = sDateEnd.Split('-');
                    if (x.calendar_type)
                    {
                        DateTime start = DateTime.Parse(arrTimeStart[0]);
                        DateTime end = DateTime.Parse(arrTimeEnd[0]);
                        if (day >= start && day <= end)
                        {
                            lstSTT.Add(x.STT.ToString());
                        }
                    }
                    else
                    {
                        string[] arrDateStart = arrTimeStart[0].Split('/');
                        LunarCalendar lunaStart = new LunarCalendar();
                        lunaStart.intLunarDay = int.Parse(arrDateStart[0]);
                        lunaStart.intLunarMonth = int.Parse(arrDateStart[1].Replace("nhuận", "").Trim());
                        lunaStart.intLunarYear = int.Parse(arrDateStart[2]);
                        lunaStart.LunarMonthType = arrDateStart[1].Contains("nhuận") ? LunarCalendar.ENUM_LEAP_MONTH.LeapMonth : LunarCalendar.ENUM_LEAP_MONTH.NormalMonth;

                        string[] arrDateEnd = arrTimeEnd[0].Split('/');
                        LunarCalendar lunaEnd = new LunarCalendar();
                        lunaEnd.intLunarDay = int.Parse(arrDateEnd[0]);
                        lunaEnd.intLunarMonth = int.Parse(arrDateEnd[1].Replace("nhuận", "").Trim());
                        lunaEnd.intLunarYear = int.Parse(arrDateEnd[2]);
                        lunaEnd.LunarMonthType = arrDateEnd[1].Contains("nhuận") ? LunarCalendar.ENUM_LEAP_MONTH.LeapMonth : LunarCalendar.ENUM_LEAP_MONTH.NormalMonth;

                        int comp1 = calendar1.CompareDateTime(lunaStart, lunarCalendar);
                        int comp2 = calendar1.CompareDateTime(lunaEnd, lunarCalendar);
                        if (comp1 >= 0 && comp2 <= 0)
                        {
                            lstSTT.Add(x.STT.ToString());
                        }
                    }
                });

                for (int i = 0; i < dgvEvent.RowCount; i++)
                {
                    if (lstSTT.Contains(dgvEvent.Rows[i].Cells["STT"].Value.ToString()))
                    {
                        dgvEvent.Rows[i].DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 238, 245);
                        dgvEvent.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(219, 238, 245);
                    }
                    else
                    {
                        dgvEvent.Rows[i].DefaultCellStyle.SelectionBackColor = Color.White;
                        dgvEvent.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    }
                }

            };
            calendar1.SelectMonth += (sender, month) =>
            {
                if (monthSelect.Month != month.Month || monthSelect.Year != month.Year)
                {
                    Enum.TryParse<EnumReplyType>(cbrepType.SelectedValue?.ToString(), out EnumReplyType replyType);
                    Enum.TryParse<EnumEventStatus>(cbeventStatus.SelectedValue?.ToString(), out EnumEventStatus eventStatus);
                    Enum.TryParse<EnumCalendarType>(cbcalendarType.SelectedValue?.ToString(), out EnumCalendarType calendarType);
                    LoadEventToDataGridView(month, txtsearchEvent.Text, replyType, eventStatus, calendarType);
                    monthSelect = month;
                    llb_lstBirthday.Text = "Danh sách sinh nhật tháng " + month.Month;
                    llb_lstDeadDay.Text = "Danh sách ngày giỗ tháng " + month.Month;
                }
            };

            calendar1.AddNewEvent += (sender, day) =>
            {
                DateTime dt = day;
                NavigationParameters param = new NavigationParameters();
                var tEven = new TEvent();
                tEven.time_from = tEven.time_to = dt.ToString("hh:mm");
                tEven.s_date = tEven.e_date = dt;
                LunarCalendar lunarCalendar = new LunarCalendar(dt);
                tEven.s_moon_day = tEven.e_moon_day = lunarCalendar.intLunarDay;
                tEven.s_moon_month = tEven.e_moon_month = lunarCalendar.intLunarMonth;
                tEven.s_moon_year = tEven.e_moon_year = lunarCalendar.intLunarYear;
                param.Add("TEvent", tEven);
                AppManager.Navigation.ShowDialogWithParam<AddFamilyEvent>(param, ModeForm.New, AppConst.StatusBarColor);
                calendar1.Refresh();
                Enum.TryParse<EnumReplyType>(cbrepType.SelectedValue?.ToString(), out EnumReplyType replyType);
                Enum.TryParse<EnumEventStatus>(cbeventStatus.SelectedValue?.ToString(), out EnumEventStatus eventStatus);
                Enum.TryParse<EnumCalendarType>(cbcalendarType.SelectedValue?.ToString(), out EnumCalendarType calendarType);
                LoadEventToDataGridView(calendar1.CalendarSolarDate);
            };

            LoadEventToDataGridView(timeSelect);
            llb_lstBirthday.Text = $"D.sách sinh nhật tháng {DateTime.Now.Month} dương lịch.";
            llb_lstDeadDay.Text = $"D.sách ngày giỗ tháng {DateTime.Now.Month} dương lịch";
            lbldayinfo.Text = GetDayInfo(DateTime.Now);
            InitFontLabel(MaterialSkinManager.fontType.Body1);
            InitFontCombobox(MaterialSkinManager.fontType.Body1);
            InitFontButton(MaterialSkinManager.fontType.Button);
        }

        private void InitFontLabel(MaterialSkinManager.fontType fontType)
        {
            Font font = SkinManager.getFontByType(fontType);
            lbldayinfo.Font = font;
            lblsearch.Font = font;
            llb_lstBirthday.Font = font;
            llb_lstDeadDay.Font = font;
            lblcalendarType.Font = font;
            lblreplyType.Font = font;
            lblEvent.Font = font;
            txtsearchEvent.Font = font;
        }
        private void InitFontCombobox(MaterialSkinManager.fontType fontType)
        {
            Font font = SkinManager.getFontByType(fontType);
            cbcalendarType.Font = font;
            cbrepType.Font = font;
            cbeventStatus.Font = font;
        }
        private void InitFontButton(MaterialSkinManager.fontType fontType)
        {
            Font font = SkinManager.getFontByType(fontType);
            btnaddEvent.Font = font;
            btnexportExcel.Font = font;
        }

        private int CompareDateTime(int d1, int m1, int y1, int d2, int m2, int y2)
        {
            if (y2 > y1) { return 1; }
            else if (y2 == y1)
            {
                if (m2 > m1) { return 1; }
                else if (m2 == m1)
                {
                    if (d2 > d1) { return 1; }
                    else if (d2 == d1) { return 0; }
                    else { return -1; }
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
        private void tblListEvent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void calendar1_Load(object sender, EventArgs e)
        {

        }

        private string GetDayInfo(DateTime dateTime)
        {
            string info = string.Empty;
            LunarCalendar lunarCalendar = new LunarCalendar(dateTime);
            string dayInfo = lunarCalendar.DayInfo;
            string[] dayInfoSplit = dayInfo.Split('\n');
            string[] dateInfo = dayInfoSplit[0].Split(',');
            info = $"Dương lịch: {dateInfo[0]}\n";
            info += $"{dateInfo[1].Trim()}\n";
            info += $"{dateInfo[2].TrimStart()}\n";
            info += $"Giờ hoàng đạo (Giờ tốt): {lunarCalendar.GetGioHoangDao()}";
            return info;
        }


        private void LoadEventToDataGridView(DateTime dt, string keyWord = "", EnumReplyType replyType = EnumReplyType.ALL, EnumEventStatus eventStatus = EnumEventStatus.ALL, EnumCalendarType calendarType = EnumCalendarType.ALL)
        {
            try
            {
                DateTime beginMonth = new DateTime(dt.Year, dt.Month, 1);
                DateTime DateTimeFilter = beginMonth;

                dgvEvent.AllowUserToAddRows = false;
                var dataEvent = AppManager.DBManager.GetTable<TEvent>();
                //var temp = dataEvent.ToList();
                var lstEvent = dataEvent.ToList(x => x.activate && x.DeleteDate == null && (string.IsNullOrEmpty(keyWord) || x.name.ToLower().Contains(keyWord.ToLower())) &&
                                                    (replyType == EnumReplyType.ALL || x.iterate == replyType.ToString()) &&
                                                    (calendarType == EnumCalendarType.ALL || x.calendar_type == (calendarType == EnumCalendarType.SUN)));

                dataShow = FilterEvent(lstEvent, DateTimeFilter, eventStatus);
                DateTime endMonth = new DateTime(dt.Year, dt.Month, DateTime.DaysInMonth(dt.Year, dt.Month));
                DateTimeFilter = endMonth;
                var lstTemp = FilterEvent(lstEvent, DateTimeFilter, eventStatus).Where(x => !dataShow.Select(x => x.TimeEventStart).Contains(x.TimeEventStart)).ToList();
                lstTemp = lstTemp.Union(dataShow).ToList();
                var cnt = 0;
                lstTemp.ForEach(x => { x.STT = ++cnt; });
                dataShow = lstTemp;
                BindingHelper.BindingDataGrid(dgvEvent, dataShow);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(Events), ex);
            }
        }

        private List<EventForShowGridView> FilterEvent(List<TEvent> tabEvent, DateTime dateTime, EnumEventStatus eventStatus)
        {
            List<EventForShowGridView> lstEvent = new List<EventForShowGridView>();
            tabEvent.ForEach(even =>
            {
                var replyType = Enum.Parse(typeof(GPConst.EnumReplyType), even.iterate);
                string[] arrTimeS = even.time_from.Split(':');
                string[] arrTimeE = even.time_to.Split(':');
                int.TryParse(arrTimeS[0], out int hourStart);
                int.TryParse(arrTimeS[1], out int minuteStart);
                int.TryParse(arrTimeE[0], out int hourEnd);
                int.TryParse(arrTimeE[1], out int minuteEnd);
                int numDayOfEvent = (even.e_date.Value - even.s_date.Value).Days;
                if (even.calendar_type)
                {
                    int dayInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
                    DateTime timeStart = new DateTime();
                    DateTime timeEnd = new DateTime();
                    DateTime dateStartMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
                    DateTime dateEndMonth = new DateTime(dateTime.Year, dateTime.Month, dayInMonth);

                    bool eventInMonth = false;
                    switch (replyType)
                    {
                        case GPConst.EnumReplyType.NONE:
                            if ((even.s_date >= dateStartMonth && even.s_date <= dateEndMonth) ||
                                (even.e_date >= dateStartMonth && even.e_date <= dateEndMonth) ||
                                (even.s_date < dateStartMonth && even.e_date > dateEndMonth))
                            {
                                timeStart = even.s_date.Value;
                                timeEnd = even.e_date.Value;
                                eventInMonth = true;
                            }
                            break;
                        case GPConst.EnumReplyType.MOTH:
                            if (even.s_date.Value >= even.e_date.Value)
                            {
                                timeEnd = new DateTime(dateTime.Year, dateTime.Month, even.e_date.Value.Day > dayInMonth ? dayInMonth : even.e_date.Value.Day);
                                timeStart = timeEnd.AddDays(-numDayOfEvent);
                            }
                            else
                            {
                                timeStart = new DateTime(dateTime.Year, dateTime.Month, even.s_date.Value.Day > dayInMonth ? dayInMonth : even.s_date.Value.Day);
                                timeEnd = timeStart.AddDays(numDayOfEvent);
                            }
                            eventInMonth = true;
                            break;
                        case GPConst.EnumReplyType.YEAR:
                            dayInMonth = DateTime.DaysInMonth(dateTime.Year, even.s_date.Value.Month);
                            timeStart = new DateTime(dateTime.Year, even.s_date.Value.Month, even.s_date.Value.Day > dayInMonth ? dayInMonth : even.s_date.Value.Day);
                            timeEnd = timeStart.AddDays(numDayOfEvent);
                            if ((timeStart >= dateStartMonth && timeStart <= dateEndMonth) ||
                               (timeEnd >= dateStartMonth && timeEnd <= dateEndMonth) ||
                               (timeStart < dateStartMonth && timeEnd > dateEndMonth))
                            {
                                eventInMonth = true;
                            }
                            break;
                    }

                    bool eventInStatus = false;
                    if (eventInMonth)
                    {
                        if (eventStatus == EnumEventStatus.ALL) eventInStatus = true;
                        else if (eventStatus == EnumEventStatus.OPEN)
                        {
                            DateTime dateTemp = new DateTime(timeEnd.Year, timeEnd.Month, timeEnd.Day, hourEnd, minuteEnd, 0);
                            eventInStatus = dateTemp > DateTime.Now;
                        }
                        else if (eventStatus == EnumEventStatus.CLOSE)
                        {
                            DateTime dateTemp = new DateTime(timeEnd.Year, timeEnd.Month, timeEnd.Day, hourEnd, minuteEnd, 0);
                            eventInStatus = dateTemp <= DateTime.Now;
                        }
                    }
                    if (eventInStatus)
                    {
                        lstEvent.Add(new EventForShowGridView()
                        {
                            s_date = even.s_date,
                            e_date = even.e_date,
                            s_moon_day = even.s_moon_day,
                            s_moon_month = even.s_moon_month,
                            s_moon_year = even.s_moon_year,
                            e_moon_day = even.e_moon_day,
                            e_moon_month = even.e_moon_month,
                            e_moon_year = even.e_moon_year,
                            time_from = even.time_from,
                            time_to = even.time_to,
                            leapmonthStart = even.leapmonthStart,
                            leapmonthEnd = even.leapmonthEnd,
                            calendar_type = even.calendar_type,
                            Image = even.important ? Resources.star : new Bitmap(16, 16),
                            Important = even.important,
                            Id = even.Id,
                            EventName = even.name,
                            Desciption = even.description,
                            CalendarType = even.calendar_type ? "Dương lịch" : "Âm lịch",
                            TimeEventStart = $"{timeStart.ToString("dd/MM/yyyy")} - {even.time_from}",
                            TimeEventEnd = $"{timeEnd.ToString("dd/MM/yyyy")} - { even.time_to }",
                            ReplyType = typeof(GPConst.ReplyType).GetFields().FirstOrDefault(x => x.Name == even.iterate).GetValue(even.iterate).ToString()
                        });
                    }
                }
                else
                {
                    int dayInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
                    DateTime dateStartMonth = new DateTime(dateTime.Year, dateTime.Month, 1);
                    DateTime dateEndMonth = new DateTime(dateTime.Year, dateTime.Month, dayInMonth);
                    LunarCalendar lunarCalendar = new LunarCalendar(dateTime);
                    LunarCalendar lunaStart = new LunarCalendar(dateStartMonth);
                    LunarCalendar lunaEnd = new LunarCalendar(dateEndMonth);
                    LunarCalendar lunaEventStart = new LunarCalendar();
                    LunarCalendar lunaEventEnd = new LunarCalendar();
                    bool eventInMonth = false;
                    switch (replyType)
                    {
                        case GPConst.EnumReplyType.NONE:
                            lunaEventStart.intLunarDay = even.s_moon_day;
                            lunaEventStart.intLunarMonth = even.s_moon_month;
                            lunaEventStart.intLunarYear = even.s_moon_year;
                            lunaEventStart.LunarMonthType = even.leapmonthStart ? LunarCalendar.ENUM_LEAP_MONTH.LeapMonth : LunarCalendar.ENUM_LEAP_MONTH.NormalMonth;

                            lunaEventEnd.intLunarDay = even.e_moon_day;
                            lunaEventEnd.intLunarMonth = even.e_moon_month;
                            lunaEventEnd.intLunarYear = even.e_moon_year;
                            lunaEventEnd.LunarMonthType = even.leapmonthEnd ? LunarCalendar.ENUM_LEAP_MONTH.LeapMonth : LunarCalendar.ENUM_LEAP_MONTH.NormalMonth;

                            if ((DateGreatOrEqual(lunaEventStart, lunaStart) && DateGreatOrEqual(lunaEnd, lunaEventStart)) ||
                               (DateGreatOrEqual(lunaEventEnd, lunaStart) && DateGreatOrEqual(lunaEnd, lunaEventEnd)) ||
                               (DateGreatOrEqual(lunaStart, lunaEventStart) && DateGreatOrEqual(lunaEventEnd, lunaEnd)))
                            {
                                eventInMonth = true;
                            }
                            break;
                        case GPConst.EnumReplyType.MOTH:
                            if (dateTime.Day == 1)
                            {
                                dayInMonth = lunaEventStart.GetLunarMonthDays(lunaStart.intLunarMonth, lunaStart.intLunarYear, lunaStart.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);
                                if (even.s_moon_day >= lunaStart.intLunarDay)
                                {
                                    lunaEventStart.intLunarDay = even.s_moon_day > dayInMonth ? dayInMonth : even.s_moon_day;
                                    lunaEventStart.intLunarMonth = lunarCalendar.intLunarMonth;
                                    lunaEventStart.intLunarYear = lunarCalendar.intLunarYear;
                                    lunaEventStart.LunarMonthType = lunarCalendar.LunarMonthType;
                                    lunaEventEnd = calendar1.AddDays(lunaEventStart, numDayOfEvent);
                                }
                                else
                                {
                                    lunaEventEnd.intLunarDay = even.e_moon_day > dayInMonth ? dayInMonth : even.e_moon_day;
                                    lunaEventEnd.intLunarMonth = lunarCalendar.intLunarMonth;
                                    lunaEventEnd.intLunarYear = lunarCalendar.intLunarYear;
                                    lunaEventEnd.LunarMonthType = lunarCalendar.LunarMonthType;
                                    lunaEventStart = calendar1.AddDays(lunaEventEnd, -numDayOfEvent);
                                }
                                if ((DateGreatOrEqual(lunaEventStart, lunaStart) && DateGreatOrEqual(lunaEnd, lunaEventStart)) ||
                                   (DateGreatOrEqual(lunaEventEnd, lunaStart) && DateGreatOrEqual(lunaEnd, lunaEventEnd)) ||
                                   (DateGreatOrEqual(lunaStart, lunaEventStart) && DateGreatOrEqual(lunaEventEnd, lunaEnd)))
                                {
                                    eventInMonth = true;
                                }
                            }
                            else
                            {
                                dayInMonth = lunaEventStart.GetLunarMonthDays(lunaEnd.intLunarMonth, lunaEnd.intLunarYear, lunaEnd.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);
                                if (even.s_moon_day <= lunaEnd.intLunarDay)
                                {
                                    lunaEventStart.intLunarDay = even.s_moon_day > dayInMonth ? dayInMonth : even.s_moon_day;
                                    lunaEventStart.intLunarMonth = lunarCalendar.intLunarMonth;
                                    lunaEventStart.intLunarYear = lunarCalendar.intLunarYear;
                                    lunaEventStart.LunarMonthType = lunarCalendar.LunarMonthType;
                                    lunaEventEnd = calendar1.AddDays(lunaEventStart, numDayOfEvent);
                                }
                                else
                                {
                                    lunaEventEnd.intLunarDay = even.e_moon_day > dayInMonth ? dayInMonth : even.e_moon_day;
                                    lunaEventEnd.intLunarMonth = lunarCalendar.intLunarMonth;
                                    lunaEventEnd.intLunarYear = lunarCalendar.intLunarYear;
                                    lunaEventEnd.LunarMonthType = lunarCalendar.LunarMonthType;
                                    lunaEventStart = calendar1.AddDays(lunaEventEnd, -numDayOfEvent);
                                }
                                if ((DateGreatOrEqual(lunaEventStart, lunaStart) && DateGreatOrEqual(lunaEnd, lunaEventStart)) ||
                                  (DateGreatOrEqual(lunaEventEnd, lunaStart) && DateGreatOrEqual(lunaEnd, lunaEventEnd)) ||
                                  (DateGreatOrEqual(lunaStart, lunaEventStart) && DateGreatOrEqual(lunaEventEnd, lunaEnd)))
                                {
                                    eventInMonth = true;
                                }
                            }
                            break;
                        case GPConst.EnumReplyType.YEAR:
                            dayInMonth = lunaEventStart.GetLunarMonthDays(even.s_moon_month, lunarCalendar.intLunarYear, even.leapmonthStart);
                            lunaEventStart.intLunarDay = even.s_moon_day > dayInMonth ? dayInMonth : even.s_moon_day;
                            lunaEventStart.intLunarMonth = even.s_moon_month;
                            lunaEventStart.intLunarYear = lunarCalendar.intLunarYear;
                            lunaEventStart.LunarMonthType = even.leapmonthStart && (lunarCalendar.GetLeapMonth(lunarCalendar.intLunarYear) > 0) ? LunarCalendar.ENUM_LEAP_MONTH.LeapMonth : LunarCalendar.ENUM_LEAP_MONTH.NormalMonth;

                            lunaEventEnd = calendar1.AddDays(lunaEventStart, numDayOfEvent);
                            if ((DateGreatOrEqual(lunaEventStart, lunaStart) && DateGreatOrEqual(lunaEnd, lunaEventStart)) ||
                                  (DateGreatOrEqual(lunaEventEnd, lunaStart) && DateGreatOrEqual(lunaEnd, lunaEventEnd)) ||
                                  (DateGreatOrEqual(lunaStart, lunaEventStart) && DateGreatOrEqual(lunaEventEnd, lunaEnd)))
                            {
                                eventInMonth = true;
                            }
                            break;
                    }
                    bool eventInStatus = false;
                    if (eventInMonth)
                    {
                        if (eventStatus == EnumEventStatus.ALL) eventInStatus = true;
                        else if (eventStatus == EnumEventStatus.OPEN)
                        {
                            LunarCalendar lunaDateNow = new LunarCalendar(DateTime.Now);
                            int comp = calendar1.CompareDateTime(lunaDateNow, lunaEventEnd);
                            if (comp > 0) eventInStatus = true;
                            else if (comp == 0)
                            {
                                eventInStatus = (hourEnd > DateTime.Now.Hour) || (hourEnd == DateTime.Now.Hour && minuteEnd > DateTime.Now.Minute);
                            }
                        }
                        else if (eventStatus == EnumEventStatus.CLOSE)
                        {
                            LunarCalendar lunaDateNow = new LunarCalendar(DateTime.Now);
                            int comp = calendar1.CompareDateTime(lunaDateNow, lunaEventEnd);
                            if (comp < 0) eventInStatus = true;
                            else if (comp == 0)
                            {
                                eventInStatus = (hourEnd < DateTime.Now.Hour) || (hourEnd == DateTime.Now.Hour && minuteEnd < DateTime.Now.Minute);
                            }
                        }
                    }
                    if (eventInStatus)
                    {
                        lstEvent.Add(new EventForShowGridView()
                        {
                            s_date = even.s_date,
                            e_date = even.e_date,
                            s_moon_day = even.s_moon_day,
                            s_moon_month = even.s_moon_month,
                            s_moon_year = even.s_moon_year,
                            e_moon_day = even.e_moon_day,
                            e_moon_month = even.e_moon_month,
                            e_moon_year = even.e_moon_year,
                            time_from = even.time_from,
                            time_to = even.time_to,
                            leapmonthStart = even.leapmonthStart,
                            leapmonthEnd = even.leapmonthEnd,
                            calendar_type = even.calendar_type,
                            Image = even.important ? Resources.star : new Bitmap(16, 16),
                            Important = even.important,
                            Id = even.Id,
                            EventName = even.name,
                            Desciption = even.description,
                            CalendarType = even.calendar_type ? "Dương lịch" : "Âm lịch",
                            TimeEventStart = $"{lunaEventStart.ToString()} - {even.time_from}",
                            TimeEventEnd = $"{lunaEventEnd.ToString()} - { even.time_to }",
                            ReplyType = typeof(GPConst.ReplyType).GetFields().FirstOrDefault(x => x.Name == even.iterate).GetValue(even.iterate).ToString()
                        });
                    }
                }
            });
            return lstEvent;
        }

        private bool DateGreatOrEqual(LunarCalendar time1, LunarCalendar time2)
        {
            int comp = calendar1.CompareDateTime(time2, time1);
            return comp >= 0;
        }

        private string GetDateEvent(TEvent even, DateTime dateTime, bool eventStart)
        {
            if (even.calendar_type)
            {
                int dayInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
                int numDayOfEvent = (even.e_date.Value - even.s_date.Value).Days;
                string dateReturn = dateTime.ToString("dd/MM/yyyy");
                switch (Enum.Parse(typeof(EnumReplyType), even.iterate))
                {
                    case EnumReplyType.NONE:
                        if (eventStart)
                        {
                            dateReturn = even.s_date.Value.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            dateReturn = even.e_date.Value.ToString("dd/MM/yyyy");
                        }
                        break;
                    case EnumReplyType.MOTH:

                        if (even.e_date.Value.Day < even.s_date.Value.Day)
                        {
                            if (dateTime.Day == 1)
                            {
                                dayInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month - 1 == 0 ? 12 : dateTime.Month - 1);
                                DateTime dateStart = new DateTime(dateTime.Month - 1 == 0 ? dateTime.Year - 1 : dateTime.Year, dateTime.Month - 1 == 0 ? 12 : dateTime.Month - 1, even.s_date.Value.Day > dayInMonth ? dayInMonth : even.s_date.Value.Day);
                                DateTime dateEnd = dateStart.AddDays(numDayOfEvent);
                                if (eventStart)
                                {
                                    dateReturn = dateStart.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    dateReturn = dateEnd.ToString("dd/MM/yyyy");
                                }
                            }
                            else
                            {
                                DateTime dateStart = new DateTime(dateTime.Year, dateTime.Month, even.s_date.Value.Day > dayInMonth ? dayInMonth : even.s_date.Value.Day);
                                DateTime dateEnd = dateStart.AddDays(numDayOfEvent);
                                if (eventStart)
                                {
                                    dateReturn = dateStart.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    dateReturn = dateEnd.ToString("dd/MM/yyyy");
                                }
                            }
                        }
                        else
                        {
                            DateTime dateStart = new DateTime(dateTime.Year, dateTime.Month, even.s_date.Value.Day > dayInMonth ? dayInMonth : even.s_date.Value.Day);
                            DateTime dateEnd = dateStart.AddDays(numDayOfEvent);
                            if (eventStart)
                            {
                                dateReturn = dateStart.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                dateReturn = dateEnd.ToString("dd/MM/yyyy");
                            }
                        }
                        break;
                    case EnumReplyType.YEAR:
                        if (dateTime.Month == even.s_date.Value.Month)
                        {
                            DateTime dateInMonth = new DateTime(dateTime.Year, dateTime.Month, even.s_date.Value.Day > dayInMonth ? dayInMonth : even.s_date.Value.Day);
                            DateTime dateEventEnd = dateInMonth.AddDays(numDayOfEvent);
                            if (eventStart)
                            {
                                dateReturn = dateInMonth.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                dateReturn = dateEventEnd.ToString("dd/MM/yyyy");
                            }
                        }
                        else
                        {
                            DateTime dateEventEnd = new DateTime(dateTime.Year, dateTime.Month, even.e_date.Value.Day > dayInMonth ? dayInMonth : even.s_date.Value.Day);
                            DateTime dateInMonth = dateEventEnd.AddDays(-numDayOfEvent);
                            if (eventStart)
                            {
                                dateReturn = dateInMonth.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                dateReturn = dateEventEnd.ToString("dd/MM/yyyy");
                            }
                        }
                        break;
                }
                return dateReturn;
            }
            else
            {
                LunarCalendar lunarCalendar = new LunarCalendar(dateTime);
                int dayInMonthSun = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
                LunarCalendar lunaStart = new LunarCalendar(new DateTime(dateTime.Year, dateTime.Month, 1));
                LunarCalendar lunaEnd = new LunarCalendar(new DateTime(dateTime.Year, dateTime.Month, dayInMonthSun));
                int sdayInMonthMoon = lunarCalendar.GetLunarMonthDays(lunarCalendar.intLunarMonth, lunarCalendar.intLunarYear, lunarCalendar.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);

                DateTime dateStart = lunarCalendar.GetSolarDate(even.s_moon_day, even.s_moon_month, even.s_moon_year, even.leapmonthStart);
                DateTime dateEnd = lunarCalendar.GetSolarDate(even.e_moon_day, even.e_moon_month, even.e_moon_year, even.leapmonthEnd);

                int dayOfEvent = (dateEnd - dateStart).Days;

                LunarCalendar lunaEvent = new LunarCalendar();
                lunaEvent.intLunarDay = even.s_moon_day > sdayInMonthMoon ? sdayInMonthMoon : even.s_moon_day;
                lunaEvent.intLunarMonth = lunarCalendar.intLunarMonth;
                lunaEvent.intLunarYear = lunarCalendar.intLunarYear;
                lunaEvent.LunarMonthType = lunarCalendar.LunarMonthType;

                int comp11 = calendar1.CompareDateTime(lunaStart, lunaEvent);
                int comp12 = calendar1.CompareDateTime(lunaEnd, lunaEvent);

                if (comp11 >= 0 && comp12 <= 0)
                {
                    DateTime dateEventStart = lunaEvent.GetSolarDate(lunaEvent.intLunarDay, lunaEvent.intLunarMonth, lunaEvent.intLunarYear, lunaEvent.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);
                    DateTime dateEventEnds = dateEventStart.AddDays(dayOfEvent);
                    LunarCalendar lunaEventEnd = new LunarCalendar(dateEventEnds);
                    if (eventStart)
                    {
                        return VNDate.ToDateMoon(lunaEvent.intLunarDay, lunaEvent.intLunarMonth, lunaEvent.intLunarYear, lunaEvent.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);
                    }
                    else
                    {
                        return VNDate.ToDateMoon(lunaEventEnd.intLunarDay, lunaEventEnd.intLunarMonth, lunaEventEnd.intLunarYear, lunaEventEnd.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);
                    }
                }

                lunaEvent.intLunarDay = even.e_moon_day;
                lunaEvent.intLunarMonth = lunarCalendar.intLunarMonth;
                lunaEvent.intLunarYear = lunarCalendar.intLunarYear;

                int comp21 = calendar1.CompareDateTime(lunaStart, lunaEvent);
                int comp22 = calendar1.CompareDateTime(lunaEnd, lunaEvent);

                if (comp21 >= 0 && comp22 <= 0)
                {
                    DateTime dateEventEnds = lunaEvent.GetSolarDate(lunaEvent.intLunarDay, lunaEvent.intLunarMonth, lunaEvent.intLunarYear, lunaEvent.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);
                    DateTime dateEventStart = dateEventEnds.AddDays(-dayOfEvent);
                    LunarCalendar lunaEventStart = new LunarCalendar(dateEventStart);
                    if (!eventStart)
                    {
                        return VNDate.ToDateMoon(lunaEvent.intLunarDay, lunaEvent.intLunarMonth, lunaEvent.intLunarYear, lunaEvent.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);
                    }
                    else
                    {
                        return VNDate.ToDateMoon(lunaEventStart.intLunarDay, lunaEventStart.intLunarMonth, lunaEventStart.intLunarYear, lunaEventStart.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);
                    }
                }

                return dateTime.ToString("dd/MM/yyyy");
            }
        }

        private bool CheckEventStatus(TEvent even, EnumEventStatus eventStatus, DateTime dateTime)
        {
            bool result = false;
            DateTime dtNow = DateTime.Now;
            switch (eventStatus)
            {
                case EnumEventStatus.OPEN:
                    if (even.calendar_type)
                    {
                        string[] temp = even.time_to.Split(':');
                        DateTime dateTemp = new DateTime(dateTime.Year, dateTime.Month, even.e_date.Value.Day, int.Parse(temp[0]), int.Parse(temp[1]), 0);
                        result = dateTemp > dtNow;
                    }
                    else
                    {
                        LunarCalendar lunarCalendardt = new LunarCalendar(dateTime);
                        LunarCalendar lunarCalendarNow = new LunarCalendar(dtNow);
                        string[] temp = even.time_to.Split(':');
                        int timeEvent = int.Parse(temp[0]) * 60 + int.Parse(temp[1]);
                        int timeNow = dtNow.Hour * 60 + dtNow.Minute;
                        if (lunarCalendardt.intLunarYear > lunarCalendarNow.intLunarYear)
                        {
                            return true;
                        }
                        else if (lunarCalendardt.intLunarYear == lunarCalendarNow.intLunarYear)
                        {
                            if (lunarCalendardt.intLunarMonth == lunarCalendarNow.intLunarMonth)
                            {
                                result = (even.e_moon_day > lunarCalendarNow.intLunarDay) || (even.e_moon_day == lunarCalendarNow.intLunarDay && timeEvent < timeNow);
                            }
                            else if (lunarCalendardt.intLunarMonth > lunarCalendarNow.intLunarMonth)
                            {
                                result = true;
                            }
                        }
                    }
                    break;
                case EnumEventStatus.CLOSE:
                    if (even.calendar_type)
                    {
                        string[] temp = even.time_to.Split(':');
                        DateTime dateTemp = new DateTime(dateTime.Year, dateTime.Month, even.e_date.Value.Day, int.Parse(temp[0]), int.Parse(temp[1]), 0);
                        result = dateTemp < dtNow;
                    }
                    else
                    {
                        LunarCalendar lunaNow = new LunarCalendar(dtNow);
                        string[] temp = even.time_to.Split(':');
                        int timeEvent = int.Parse(temp[0]) * 60 + int.Parse(temp[1]);
                        int timeNow = dtNow.Hour * 60 + dtNow.Minute;
                        LunarCalendar lunaEvent = new LunarCalendar();
                        lunaEvent.intLunarDay = even.e_moon_day;
                        lunaEvent.intLunarMonth = lunaNow.intLunarMonth;
                        lunaEvent.intLunarYear = lunaNow.intLunarYear;
                        lunaEvent.LunarMonthType = lunaNow.LunarMonthType;

                        result = calendar1.CompareDateTime(lunaNow, lunaEvent) >= 0;
                    }
                    break;
                case EnumEventStatus.ALL:
                    result = true;
                    break;
            }
            return result;
        }
        public bool CheckEvent(TEvent even, string replyType, DateTime dateTime)
        {
            try
            {
                if (even.calendar_type)
                {
                    switch (Enum.Parse(typeof(GPConst.EnumReplyType), replyType))
                    {
                        case GPConst.EnumReplyType.NONE:
                            return (dateTime.Ticks >= even.s_date.Value.Ticks && dateTime.Ticks <= even.e_date.Value.Ticks) ||
                                   (dateTime.Month == even.s_date.Value.Month) ||
                                   (dateTime.Month == even.e_date.Value.Month);
                        case GPConst.EnumReplyType.MOTH:
                            int dayInMonth = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
                            return even.s_date.Value.Day <= dayInMonth || even.e_date.Value.Day < dayInMonth;
                        case GPConst.EnumReplyType.YEAR: return even.s_date.Value.Month == dateTime.Month || even.e_date.Value.Month == dateTime.Month;
                        default: return false;
                    }
                }
                else
                {
                    LunarCalendar lunarCalendar = new LunarCalendar(dateTime);
                    switch (Enum.Parse(typeof(GPConst.EnumReplyType), replyType))
                    {
                        case GPConst.EnumReplyType.NONE: return (even.s_moon_month == lunarCalendar.intLunarMonth && even.e_moon_year == lunarCalendar.intLunarYear);
                        case GPConst.EnumReplyType.MOTH:
                            int dayInMonthSun = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
                            LunarCalendar lunaStart = new LunarCalendar(new DateTime(dateTime.Year, dateTime.Month, 1));
                            LunarCalendar lunaEnd = new LunarCalendar(new DateTime(dateTime.Year, dateTime.Month, dayInMonthSun));


                            LunarCalendar lunaEvent = new LunarCalendar();
                            lunaEvent.intLunarDay = even.s_moon_day;
                            lunaEvent.intLunarMonth = lunarCalendar.intLunarMonth;
                            lunaEvent.intLunarYear = lunarCalendar.intLunarYear;
                            lunaEvent.LunarMonthType = lunarCalendar.LunarMonthType;

                            int comp11 = calendar1.CompareDateTime(lunaStart, lunaEvent);
                            int comp12 = calendar1.CompareDateTime(lunaEnd, lunaEvent);

                            lunaEvent.intLunarDay = even.e_moon_day;
                            lunaEvent.intLunarMonth = lunarCalendar.intLunarMonth;
                            lunaEvent.intLunarYear = lunarCalendar.intLunarYear;

                            int comp21 = calendar1.CompareDateTime(lunaStart, lunaEvent);
                            int comp22 = calendar1.CompareDateTime(lunaEnd, lunaEvent);

                            return (comp11 >= 0 && comp12 <= 0) || (comp21 >= 0 && comp22 <= 0);

                        //if (dateTime.Day == 1)
                        //{
                        //    return (even.s_moon_day >= lunarCalendar.intLunarDay && even.s_moon_day < lunarCalendar.GetLunarMonthDays()) ||
                        //           (even.e_moon_day >= lunarCalendar.intLunarDay);
                        //}
                        //else
                        //{
                        //    return even.s_moon_day <= lunarCalendar.intLunarDay;
                        //}
                        case GPConst.EnumReplyType.YEAR:
                            int numDayOfEvent = 0;
                            if (even.e_moon_month == even.s_moon_month)
                            {
                                numDayOfEvent = even.e_moon_day - even.s_moon_day;
                            }
                            else
                            {
                                numDayOfEvent = even.e_moon_day + lunarCalendar.GetLunarMonthDays(even.s_moon_month, even.s_moon_year, even.leapmonthStart) - even.s_moon_day;
                            }
                            if (dateTime.Day == 1)
                            {
                                if (lunarCalendar.intLunarDay >= even.s_moon_day && lunarCalendar.intLunarMonth == even.s_moon_month)
                                {
                                    int dayEnd = lunarCalendar.GetLunarMonthDays(lunarCalendar.intLunarYear, lunarCalendar.intLunarMonth, lunarCalendar.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth) - (even.s_moon_day + numDayOfEvent);
                                    if (dayEnd <= 0)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        return lunarCalendar.intLunarDay <= even.s_moon_day + numDayOfEvent;
                                    }
                                }
                                if (lunarCalendar.intLunarDay < even.s_moon_day && lunarCalendar.intLunarMonth == even.s_moon_month)
                                {
                                    return even.s_moon_day <= lunarCalendar.GetLunarMonthDays(lunarCalendar.intLunarMonth, lunarCalendar.intLunarYear, lunarCalendar.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);
                                }
                                return false;
                            }
                            else
                            {
                                if (lunarCalendar.intLunarDay >= even.s_moon_day && lunarCalendar.intLunarMonth == even.s_moon_month)
                                {
                                    return true;
                                }
                                return false;
                            }
                        default: return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void btnaddEvent_Click(object sender, EventArgs e)
        {
            DateTime dt = calendar1.CalendarSolarDate;
            NavigationParameters param = new NavigationParameters();
            var tEven = new TEvent();
            tEven.time_from = tEven.time_to = dt.ToString("HH:mm");
            tEven.s_date = tEven.e_date = dt;
            LunarCalendar lunarCalendar = new LunarCalendar(dt);
            tEven.s_moon_day = tEven.e_moon_day = lunarCalendar.intLunarDay;
            tEven.s_moon_month = tEven.e_moon_month = lunarCalendar.intLunarMonth;
            tEven.s_moon_year = tEven.e_moon_year = lunarCalendar.intLunarYear;
            param.Add("TEvent", tEven);
            AppManager.Navigation.ShowDialogWithParam<AddFamilyEvent>(param, ModeForm.New, AppConst.StatusBarColor);
            LoadEventToDataGridView(timeSelect);
            calendar1.Refresh();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

        }

        private void llb_lstBirthday_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Expression<Func<TMember, bool>> filterMem = (i =>
                i.Birthday.MonthSun == timeSelect.Month
                && !i.IsDeath
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
            DateTime begionMonth = new DateTime(timeSelect.Year, timeSelect.Month, 1);
            DateTime endMonth = new DateTime(timeSelect.Year, timeSelect.Month, DateTime.DaysInMonth(timeSelect.Year, timeSelect.Month));
            LunarCalendar objBeginMonth = new LunarCalendar(begionMonth);
            LunarCalendar objEndMonth = new LunarCalendar(endMonth);
            var lstMember = AppManager.DBManager.GetTable<TMember>().AsEnumerable().Where(i =>
                    (CompareDeadDay(i.DeadDay, objBeginMonth, objEndMonth) ||
                    (i.DeadDay.DayMoon == -1 && (i.DeadDay.MonthMoon == objBeginMonth.intLunarMonth || i.DeadDay.MonthMoon == objEndMonth.intLunarMonth)))
                    && i.IsDeath && objBeginMonth.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.NormalMonth
                    ).ToList();
            var param = new NavigationParameters();
            param.Add(FamilyPopup.KEY_DATA, lstMember);
            param.Add(FamilyPopup.KEY_TYPE, "DeadDay");
            param.Add("Time", timeSelect.ToString("dd/MM/yyyy"));
            AppManager.Navigation.ShowDialogWithParam<FamilyPopup>(param, ModeForm.None, AppConst.StatusBarColor);
            this.Cursor = Cursors.Default;
        }

        private bool CompareDeadDay(VNDate deadDay, LunarCalendar lunarStart, LunarCalendar lunarEnd)
        {
            if (deadDay == null)
            {
                return false;
            }
            if (deadDay.MonthMoon == -1 || deadDay.DayMoon == -1)
            {
                return false;
            }
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

        private void btnexportExcel_Click(object sender, EventArgs e)
        {
            string pathFile = "";
            if (dataShow == null || dataShow.Count == 0)
            {
                AppManager.Dialog.Warning("Không có dữ liệu để in!");
                return;
            }
        ShowDialog:
            pathFile = AppManager.Dialog.SaveFile($"Danh sách sự kiện tháng {calendar1.CalendarSolarDate.Month} năm {calendar1.CalendarSolarDate.Year}.xlsx", DialogManager.DIALOG_FILTER_EXCEL);
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
            string templatePath = "./Data/Excel/EventsTemplate.xltx";

            if (!Directory.Exists("./Temp"))
            {
                Directory.CreateDirectory("./Temp");
            }

            string tempPath = "./Temp/" + Guid.NewGuid() + ".xlsx";
            File.Copy(templatePath, tempPath);
            bool exportComplete = false;
            try
            {
                using (ExportExcelForEvent exportExcelForEvent = new ExportExcelForEvent())
                {
                    this.Cursor = Cursors.WaitCursor;
                    exportComplete = exportExcelForEvent.Export(dataShow, $"Danh sách sự kiện tháng {calendar1.CalendarSolarDate.Month} năm {calendar1.CalendarSolarDate.Year}", tempPath, pathFile);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(Events), ex);
            }
            finally
            {
                if (File.Exists(pathFile) && AppManager.Dialog.Confirm($"Xuất danh sách sự kiện thành công!\n{Resources.MSG_CONFIRM_OPEN_FILE}") && exportComplete)
                {
                    Process.Start(pathFile);
                }
            }
        }

        private void txtsearchEvent_TextChanged(object sender, EventArgs e)
        {
            if (timeSelect.Year < 1000) return;
            EnumReplyType replyType = EnumReplyType.ALL;
            EnumEventStatus eventStatus = EnumEventStatus.ALL;
            EnumCalendarType calendarType = EnumCalendarType.ALL;
            Enum.TryParse<EnumReplyType>(cbrepType.SelectedValue?.ToString(), out replyType);
            Enum.TryParse<EnumEventStatus>(cbeventStatus.SelectedValue?.ToString(), out eventStatus);
            Enum.TryParse<EnumCalendarType>(cbcalendarType.SelectedValue?.ToString(), out calendarType);
            LoadEventToDataGridView(timeSelect, txtsearchEvent.Text, replyType, eventStatus, calendarType);
        }

        private void cbrepType_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (Char)Keys.None;
        }
    }
}
