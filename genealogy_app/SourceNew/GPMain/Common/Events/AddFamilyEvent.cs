using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPMain.Common.Helper;
using GPModels;
using GPMain.Common.Database;
using GPMain.Common.Navigation;
using GPMain.Common.Events;
using GPCommon;
using GPMain.Views.Tree.Build;
using MaterialSkin;
using GPConst;

namespace GPMain.Common.FamilyEvent
{
    public partial class AddFamilyEvent : BaseUserControl
    {
        TEvent tEvent;
        bool bInsert = false;
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        public AddFamilyEvent(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            TitleBar = "Thêm sự kiện";
            if (this.Params != null)
            {
                tEvent = Params.GetValue<TEvent>("TEvent", new TEvent());
            }
            else
            {
                tEvent = new TEvent();
            }
            bInsert = mode == ModeForm.New;
            //AppManager.DBManager.DropTable<TEvent>();
            //AppManager.DBManager.DropTable<TEventType>();
            InitFontButton(MaterialSkinManager.fontType.Button);
            InitFontCheckbox(MaterialSkinManager.fontType.Body1);
            InitFontCombobox(MaterialSkinManager.fontType.Body1);
            InitFontLabel(MaterialSkinManager.fontType.Body1);
            InitFontTextbox(MaterialSkinManager.fontType.Body1);
        }

        private void InitFontLabel(MaterialSkinManager.fontType fontType)
        {
            Font font = SkinManager.getFontByType(fontType);
            foreach (Control ctl in this.Controls)
            {
                if (ctl is Label)
                {
                    ctl.Font = font;
                }
            };
        }
        private void InitFontTextbox(MaterialSkinManager.fontType fontType)
        {
            Font font = SkinManager.getFontByType(fontType);
            foreach (Control ctl in this.Controls)
            {
                if (ctl is TextBox)
                {
                    ctl.Font = font;
                }
            };
        }
        private void InitFontCombobox(MaterialSkinManager.fontType fontType)
        {
            Font font = SkinManager.getFontByType(fontType);
            foreach (Control ctl in this.Controls)
            {
                if (ctl is ComboBox)
                {
                    ctl.Font = font;
                }
            };
        }
        private void InitFontCheckbox(MaterialSkinManager.fontType fontType)
        {
            Font font = SkinManager.getFontByType(fontType);
            foreach (Control ctl in this.Controls)
            {
                if (ctl is CheckBox)
                {
                    ctl.Font = font;
                }
            };
        }
        private void InitFontButton(MaterialSkinManager.fontType fontType)
        {
            Font font = SkinManager.getFontByType(fontType);
            foreach(Control ctl in this.Controls)
            {
                if (ctl is Button)
                {
                    ctl.Font = font;
                }
            };
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void materialDropDownColorPicker1_Load(object sender, EventArgs e)
        {

        }

        private void cblevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (char)Keys.None;
        }

        private void AddMember_Load(object sender, EventArgs e)
        {
            string[] Key = typeof(GPConst.ReplyType).GetFields().ToList().Select(x => x.Name).ToArray();
            string[] Display = typeof(GPConst.ReplyType).GetFields().ToList().Select(x => x.GetValue(x.Name).ToString()).ToArray();
            List<ModelBinding> dataSourceReplyType = new List<ModelBinding>();
            for (int i = 0; i < Key.Length; i++)
            {
                dataSourceReplyType.Add(new ModelBinding() { Key = Key[i], Display = Display[i] });
            }
            cbrepType.DataSource = dataSourceReplyType;
            cbrepType.DisplayMember = "Display";
            cbrepType.ValueMember = "Key";

            ColorConverter colorConverter = new ColorConverter();
            if (!bInsert)
            {
                var even = AppManager.DBManager.GetTable<TEvent>().FirstOrDefault(x => x.Id == tEvent.Id);
                if (even != null)
                {
                    txtEventName.Text = even.name;
                    txtDescription.Text = even.description;
                    rdcalendarS.Checked = even.calendar_type;
                    rdcalendarM.Checked = !even.calendar_type;
                    lbltimestart.Text = even.time_from;
                    lblcalendarSunStart.Text = even.s_date.Value.ToString("dd/MM/yyyy");
                    lblcalendarMoonStart.Text = $"{even.s_moon_day}/{even.s_moon_month} {(even.leapmonthStart ? "nhuận" : "")}/{even.s_moon_year}";
                    lbltimeend.Text = even.time_to;
                    lblcalendarSunEnd.Text = even.e_date.Value.ToString("dd/MM/yyyy");
                    lblcalendarMoonEnd.Text = $"{even.e_moon_day}/{even.e_moon_month} {(even.leapmonthEnd ? "nhuận" : "")}/{even.e_moon_year}";
                    cbrepType.Text = typeof(ReplyType).GetFields().FirstOrDefault(x => x.Name == even.iterate).GetValue(even.iterate).ToString();
                    txtEventPlace.Text = even.place;
                    cbBackColor.Color = (Color)colorConverter.ConvertFromString(string.IsNullOrEmpty(even.background_color) ? "#f5ab2c" : even.background_color);
                    cbForeColor.Color = (Color)colorConverter.ConvertFromString(string.IsNullOrEmpty(even.text_color) ? "#052336" : even.text_color);
                    txtNote.Text = even.note;
                    ckimportant.Checked = even.important;
                }
            }
            else
            {
                var even = this.Params.GetValue<TEvent>("TEvent", new TEvent());
                lbltimestart.Text = even.time_from;
                lblcalendarSunStart.Text = even.s_date.Value.ToString("dd/MM/yyyy");
                lblcalendarMoonStart.Text = $"{even.s_moon_day}/{even.s_moon_month} {(even.leapmonthStart ? "nhuận" : "")}/{even.s_moon_year}";
                lbltimeend.Text = even.time_to;
                lblcalendarSunEnd.Text = even.e_date.Value.ToString("dd/MM/yyyy");
                lblcalendarMoonEnd.Text = $"{even.e_moon_day}/{even.e_moon_month} {(even.leapmonthEnd ? "nhuận" : "")}/{even.e_moon_year}";
                cbBackColor.Color = (Color)colorConverter.ConvertFromString("#f5ab2c");
                cbForeColor.Color = (Color)colorConverter.ConvertFromString("#052336");
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void cbtyperecurring_SelectedIndexChanged(object sender, EventArgs e)
        {



        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string temp = ValidateEvent();
            if (!string.IsNullOrEmpty(temp))
            {
                AppManager.Dialog.Error(temp);
                return;
            }
            if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
            {
                return;
            }
            ColorConverter colorConverter = new ColorConverter();
            tEvent = AppManager.DBManager.GetTable<TEvent>().AsEnumerable().FirstOrDefault(x => x.Id == tEvent.Id);
            if (tEvent == null)
                tEvent = new TEvent();
            tEvent.name = txtEventName.Text;
            tEvent.description = txtDescription.Text;
            tEvent.calendar_type = rdcalendarS.Checked;
            tEvent.time_from = lbltimestart.Text;
            tEvent.time_to = lbltimeend.Text;
            tEvent.s_date = DateTime.Parse(lblcalendarSunStart.Text);
            tEvent.e_date = DateTime.Parse(lblcalendarSunEnd.Text);
            DateTimeVN sdateTimeVN = new DateTimeVN(lblcalendarMoonStart.Text.Replace(" nhuận", "").Trim());
            tEvent.s_moon_day = sdateTimeVN.Day;
            tEvent.s_moon_month = sdateTimeVN.Month;
            tEvent.s_moon_year = sdateTimeVN.Year;
            tEvent.leapmonthStart = lblcalendarMoonStart.Text.Contains("nhuận");
            DateTimeVN edateTimeVN = new DateTimeVN(lblcalendarMoonEnd.Text.Replace(" nhuận", "").Trim());
            tEvent.e_moon_day = edateTimeVN.Day;
            tEvent.e_moon_month = edateTimeVN.Month;
            tEvent.e_moon_year = edateTimeVN.Year;
            tEvent.leapmonthEnd = lblcalendarMoonEnd.Text.Contains("nhuận");
            tEvent.iterate = cbrepType.SelectedValue.ToString();
            tEvent.place = txtEventPlace.Text;
            tEvent.important = ckimportant.Checked;
            tEvent.background_color = colorConverter.ConvertToString(cbBackColor.Color);
            tEvent.text_color = colorConverter.ConvertToString(cbForeColor.Color);
            tEvent.note = txtNote.Text;
            tEvent.activate = ckactiveEvent.Checked;
            if (bInsert)
            {
                tEvent.user_created = AppManager.LoginUser.Id;
                tEvent.created_at = DateTime.Now;
            }
            else
            {
                tEvent.LastUpdate = DateTime.Now;
            }
            var tbEvent = AppManager.DBManager.GetTable<TEvent>();
            bool flag = bInsert ? tbEvent.InsertOne(tEvent) : tbEvent.UpdateOne(c => c.Id == tEvent.Id, tEvent);
            if (flag)
            {
                AppManager.Dialog.Ok(bInsert ? "Thêm mới sự kiện thành công!" : "Cập nhật sự kiện thành công!");
                this.Close(DialogResult.Yes);
            }
            else
            {
                AppManager.Dialog.Error(bInsert ? "Thêm mới sự kiện thất bại!" : "Cập nhật sự kiện thất bại!");
                this.Close(DialogResult.No);
            }
        }

        private string ValidateEvent()
        {
            //errorProvider1.Clear();
            string valRet = string.Empty;
            if (string.IsNullOrEmpty(txtEventName.Text.Trim()))
            {
                //errorProvider1.SetError(txtEventName, "Tên sự kiện không được để trống");
                return "Tên sự kiện không được để trống";
            }
            if (string.IsNullOrEmpty(lbltimestart.Text.Trim()))
            {
                //errorProvider1.SetError(txtTimeStart, "Thời gian bắt đầu sự kiện không được để trống");
                return "Thời gian bắt đầu sự kiện không được để trống";
            }
            if (!CheckTime(lbltimestart.Text.Trim()))
            {
                //errorProvider1.SetError(txtTimeStart, "Thời gian bắt đầu sự kiện sai định dạng. VD: 8:00");
                return "Thời gian bắt đầu sự kiện sai định dạng. VD: 8:00";
            }
            if (string.IsNullOrEmpty(lblcalendarSunStart.Text.Trim()) || string.IsNullOrEmpty(lblcalendarMoonStart.Text.Trim()))
            {
                if (rdcalendarS.Checked)
                {
                    //errorProvider1.SetError(txtcalendarStartS, "Ngày bắt đầu sự kiện không được để trống");
                }
                else
                {
                    // errorProvider1.SetError(txtcalendarStartM, "Ngày bắt đầu sự kiện không được để trống");
                }
                return "Ngày bắt đầu sự kiện không được để trống";
            }
            if (!CheckDate(lblcalendarSunStart.Text.Trim()) || !CheckDate(lblcalendarMoonStart.Text.Trim()))
            {
                if (rdcalendarS.Checked)
                {
                    //errorProvider1.SetError(txtcalendarStartS, "Ngày bắt đầu sự kiện sai định dạng. VD: 1/12/2020");
                }
                else
                {
                    //errorProvider1.SetError(txtcalendarStartM, "Ngày bắt đầu sự kiện sai định dạng. VD: 1/12/2020");
                }
                return "Ngày bắt đầu sự kiện sai định dạng. VD: 1/12/2020";
            }
            if (string.IsNullOrEmpty(lblcalendarSunEnd.Text.Trim()) || string.IsNullOrEmpty(lblcalendarMoonEnd.Text.Trim()))
            {
                if (rdcalendarS.Checked)
                {
                    //errorProvider1.SetError(txtcalendarEndS, "Ngày kết thúc sự kiện không được để trống");
                }
                else
                {
                    //errorProvider1.SetError(txtcalendarEndM, "Ngày kết thúc sự kiện không được để trống");
                }
                return "Ngày kết thúc sự kiện không được để trống";
            }
            if (!CheckDate(lblcalendarSunEnd.Text.Trim()) || !CheckDate(lblcalendarMoonEnd.Text.Trim()))
            {
                if (rdcalendarS.Checked)
                {
                    //errorProvider1.SetError(txtcalendarEndS, "Ngày kết thúc sự kiện sai định dạng. VD: 1/12/2020");
                }
                else
                {
                    // errorProvider1.SetError(txtcalendarEndM, "Ngày kết thúc sự kiện sai định dạng. VD: 1/12/2020");
                }
                return "Ngày kết thúc sự kiện sai định dạng. VD: 1/12/2020";
            }
            if (string.IsNullOrEmpty(cbrepType.Text)) { return "Kiểu lặp lại không được để trống"; }

            if (DateTime.Parse(lblcalendarSunStart.Text) > DateTime.Parse(lblcalendarSunEnd.Text))
            {
                if (rdcalendarS.Checked)
                {
                    //errorProvider1.SetError(txtcalendarEndS, "Ngày kết thúc không thể nhỏ hơn ngày bắt đầu");
                }
                else
                {
                    //errorProvider1.SetError(txtcalendarEndM, "Ngày kết thúc không thể nhỏ hơn ngày bắt đầu");
                }
                return "Ngày kết thúc không thể nhỏ hơn ngày bắt đầu";
            }
            else if (DateTime.Parse(lblcalendarSunStart.Text) == DateTime.Parse(lblcalendarSunEnd.Text))
            {
                string[] temp1 = lbltimestart.Text.Split(':');
                int inttimeS = int.Parse(temp1[0]) * 60 + int.Parse(temp1[1]);
                string[] temp2 = lbltimeend.Text.Split(':');
                int inttimeE = int.Parse(temp2[0]) * 60 + int.Parse(temp2[1]);
                if (inttimeS >= inttimeE)
                {
                    //errorProvider1.SetError(txtTimeEnd, "Thời gian kết thúc không thể nhở hơn hoặc bằng thời gian bắt đầu");
                    return "Thời gian kết thúc không thể nhở hơn hoặc bằng thời gian bắt đầu";
                }
            }
            return valRet;
        }

        private bool CheckTime(string time)
        {
            string[] timeTemp = time.Split(':');
            if (timeTemp.Length != 2) { return false; }
            else if (!int.TryParse(timeTemp[0], out int h) || !int.TryParse(timeTemp[1], out int m))
            {
                return false;
            }
            return true;
        }
        private bool CheckDate(string date)
        {
            string[] dateTemp = date.Replace("nhuận", "").Trim().Split('/');
            if (dateTemp.Length != 3) { return false; }
            else if (!int.TryParse(dateTemp[0], out int d) || !int.TryParse(dateTemp[1], out int M) || !int.TryParse(dateTemp[2], out int y))
            {
                return false;
            }
            return true;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close(DialogResult.No);
        }

        private void btncalendarstartDate_Click(object sender, EventArgs e)
        {
            PopUpCalendarShort(lbltimestart, lblcalendarSunStart, lblcalendarMoonStart);
        }

        private void rdcalendarS_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtcalendarStartS_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtcalendarStartM_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtcalendarEndS_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtcalendarEndM_TextChanged(object sender, EventArgs e)
        {

        }

        private void ConvertCalendarSunToMoon(Label lblBoxSun, Label lblBoxMoon)
        {
            if (DateTime.TryParse(lblBoxSun.Text, out DateTime dt))
            {
                LunarCalendar lunarCalendar = new LunarCalendar(dt);
                int leapMonth = lunarCalendar.GetLeapMonth(lunarCalendar.intLunarYear);
                lblBoxMoon.Text = $"{lunarCalendar.intLunarDay}/{lunarCalendar.intLunarMonth} {(lunarCalendar.intLunarMonth == leapMonth ? " nhuận" : "")}/{lunarCalendar.intLunarYear}";
            }
            else
            {
                lblBoxMoon.Text = "__/__/____";
            }
        }

        private void ConvertCalendarMoonToSun(Label lblBoxMoon, Label lblBoxSun)
        {

            if (DateTime.TryParse(lblBoxMoon.Text.Replace(" nhuận", "").Trim(), out DateTime dt))
            {
                LunarCalendar lunarCalendar = new LunarCalendar();
                DateTime date = lunarCalendar.GetSolarDate(dt.Day, dt.Month, dt.Year, lblBoxMoon.Text.Contains("nhuận"));
                lblBoxSun.Text = date.ToString("dd/MM/yyyy");
            }
            else
            {
                lblBoxSun.Text = "__/__/____";
            }
        }

        private void GetCanChi(Label labelMoon, Label lbldayM, Label lblMonthM, Label lblYearM)
        {
            string[] dateTemp = labelMoon.Text.Split('/');
            LunarCalendar lunarCalendar = new LunarCalendar();
            lbldayM.Text = lunarCalendar.GetDayCanChi(int.Parse(dateTemp[0]));
            bool leapM = dateTemp[1].ToLower().Contains("nhuận");
            lblMonthM.Text = lunarCalendar.GetMonthCanChi(int.Parse(dateTemp[2]), int.Parse(dateTemp[1].Replace(" nhuận", "").Trim()), leapM ? LunarCalendar.ENUM_LEAP_MONTH.LeapMonth : LunarCalendar.ENUM_LEAP_MONTH.NormalMonth);
            lblYearM.Text = lunarCalendar.GetYearCanChi(int.Parse(dateTemp[2]));
        }

        private void PopUpCalendarShort(Label labelTime, Label labelSun, Label labelMoon)
        {
            NavigationParameters param = new NavigationParameters();

            int hour = DateTime.Now.Hour;
            int minute = DateTime.Now.Minute;
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;

            if (rdcalendarM.Checked)
            {
                LunarCalendar lunarCalendar = new LunarCalendar(DateTime.Now);
                year = lunarCalendar.intLunarYear;
                month = lunarCalendar.intLunarMonth;
                day = lunarCalendar.intLunarDay;
            }

            if (!string.IsNullOrEmpty(labelTime.Text))
            {
                string[] times = labelTime.Text.Split(':');
                if (times.Length == 2)
                {
                    int.TryParse(times[0], out hour);
                    int.TryParse(times[1], out minute);
                }
            }
            string sDate = rdcalendarM.Checked ? labelMoon.Text : labelSun.Text;
            if (!string.IsNullOrEmpty(sDate))
            {
                if (DateTime.TryParse(sDate, out DateTime dt))
                {
                    year = dt.Year;
                    month = dt.Month;
                    day = dt.Day;
                }
            }
            param.Add("Time", labelTime.Text);
            if (rdcalendarM.Checked)
            {
                param.Add("Date", labelMoon.Text);
            }
            else
            {
                param.Add("Date", labelSun.Text);
            }
            param.Add("CalendarMoon", rdcalendarM.Checked);
            NavigationResult result = AppManager.Navigation.ShowDialogWithParam<CalendarShort>(param, ModeForm.View, AppConst.StatusBarColor);
            if (result.Result == DialogResult.OK)
            {
                var paramResult = result.GetValue<CalendarResult>();

                labelTime.Text = paramResult.Time;
                if (rdcalendarS.Checked)
                {
                    labelSun.Text = paramResult.Date;
                }
                else
                {
                    labelMoon.Text = paramResult.Date;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PopUpCalendarShort(lbltimeend, lblcalendarSunEnd, lblcalendarMoonEnd);
        }

        private void txtEventName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void lblcalendarMoonStart_TextChanged(object sender, EventArgs e)
        {
            GetCanChi(lblcalendarMoonStart, lbldayStartM, lblmonthStartM, lblyearStartM);
            if (rdcalendarM.Checked)
            {
                ConvertCalendarMoonToSun(lblcalendarMoonStart, lblcalendarSunStart);
            }
        }

        private void lblcalendarSunStart_TextChanged(object sender, EventArgs e)
        {
            if (rdcalendarS.Checked)
            {
                ConvertCalendarSunToMoon(lblcalendarSunStart, lblcalendarMoonStart);
            }
        }

        private void lblcalendarSunEnd_TextChanged(object sender, EventArgs e)
        {
            if (rdcalendarS.Checked)
            {
                ConvertCalendarSunToMoon(lblcalendarSunEnd, lblcalendarMoonEnd);
            }
        }

        private void lblcalendarMoonEnd_TextChanged(object sender, EventArgs e)
        {
            GetCanChi(lblcalendarMoonEnd, lbldayEndM, lblmonthEndM, lblyearEndM);
            if (rdcalendarM.Checked)
            {
                ConvertCalendarMoonToSun(lblcalendarMoonEnd, lblcalendarSunEnd);
            }
        }
    }

    public class ModelBinding
    {
        public string Key { get; set; }
        public string Display { get; set; }
    }
}
