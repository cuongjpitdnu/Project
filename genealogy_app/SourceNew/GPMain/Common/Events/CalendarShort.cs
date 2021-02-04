using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPCommon;
using GPMain.Common.Navigation;
using MaterialSkin;

namespace GPMain.Common.Events
{
    public partial class CalendarShort : BaseUserControl
    {
        bool CalendarMoon = false;
        bool leapMoon = false;
        public string time;
        public string date;
        public MaterialSkinManager SkinManager => MaterialSkinManager.Instance;
        public CalendarShort(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            TitleBar = "Chọn lịch";
            time = parameters.GetValue<string>("Time");
            date = parameters.GetValue<string>("Date");
            CalendarMoon = parameters.GetValue<bool>("CalendarMoon");
            leapMoon = parameters.GetValue<bool>("LeapMoon");
        }

        private void CalendarShort_Load(object sender, EventArgs e)
        {
            string[] timeTemp = time.Split(':');
            numHour.Text = int.Parse(timeTemp[0]).ToString();
            numMinute.Text = int.Parse(timeTemp[1]).ToString();
            string[] dateTemp = date.Split('/');
            leapMoon = dateTemp[1].Contains("nhuận");
            txtyear.Text = dateTemp[2];
        }

        private void txtyear_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(txtyear.Text, out int y))
            {
                cbmonth.Items.Clear();
                cbmonth.Text = "--";
                return;
            }

            LunarCalendar lunarCalendar = new LunarCalendar();
            int.TryParse(txtyear.Text, out int yy);

            if (yy > 1200)
            {
                int leapMonth = lunarCalendar.GetLeapMonth(yy);
                List<string> months = Enumerable.Range(1, 12).Select(x => x.ToString(AppConst.FormatDate)).ToList();
                if (CalendarMoon)
                {
                    if (months.Contains(leapMonth.ToString(AppConst.FormatDate)))
                    {
                        int idx = months.IndexOf(leapMonth.ToString(AppConst.FormatDate));
                        months.Insert(idx + 1, $"{leapMonth.ToString(AppConst.FormatDate)} nhuận");
                    }
                }
                cbmonth.Items.Clear();
                cbmonth.Items.AddRange(months.ToArray());

                string[] dateTemp = date.Split('/');
                string temp = dateTemp[1].Trim();
                int idxTemp = months.IndexOf(temp);
                cbmonth.SelectedIndex = idxTemp;
            }
        }

        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            //this.Size = new Size(flowLayoutPanel1.Width + 20, this.Height);
        }

        private void cbmonth_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btncancel_Click(object sender, EventArgs e)
        {
            this.Close(DialogResult.No);
        }

        private void btnselect_Click(object sender, EventArgs e)
        {
            CalendarResult calendarResult = new CalendarResult()
            {
                Time = $"{numHour.Text.PadLeft(2, '0')}:{numMinute.Text.PadLeft(2, '0')}",
                Date = $"{cbday.Text.PadLeft(2, '0')}/{cbmonth.Text.PadLeft(2, '0')}/{txtyear.Text}"
            };
            this.Close<CalendarResult>(calendarResult);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cbmonth_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = (Char)Keys.None;
        }

        private void cbmonth_TextChanged(object sender, EventArgs e)
        {
            if (cbmonth.Items.Count > 0)
            {
                string[] dateTemp = date.Split('/');

                if (CalendarMoon)
                {
                    int y = int.Parse(txtyear.Text);
                    int m = int.Parse(cbmonth.Text.Replace(" nhuận", "").Trim());
                    bool leapM = cbmonth.Text.Contains("nhuận");
                    LunarCalendar lunarCalendar = new LunarCalendar();
                    int dayinMonth = lunarCalendar.GetLunarMonthDays(m, y, leapM);
                    string[] days = Enumerable.Range(1, dayinMonth).Select(x => x.ToString(AppConst.FormatDate)).ToArray();
                    cbday.Items.Clear();
                    cbday.Items.AddRange(days);
                    cbday.Text = dateTemp[0];
                }
                else
                {
                    int y = int.Parse(txtyear.Text);
                    int m = int.Parse(cbmonth.Text);
                    int dayInMonth = DateTime.DaysInMonth(y, m);
                    string[] days = Enumerable.Range(1, dayInMonth).Select(x => x.ToString(AppConst.FormatDate)).ToArray();
                    cbday.Items.Clear();
                    cbday.Items.AddRange(days);
                    cbday.Text = dateTemp[0];
                }
            }
        }
    }

    public class CalendarResult
    {
        public string Time { get; set; }
        public string Date { get; set; }
    }
}
