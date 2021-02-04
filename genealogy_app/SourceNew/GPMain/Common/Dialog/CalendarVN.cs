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

namespace GPMain.Common.Dialog
{
    public partial class CalendarVN : Form
    {
        LunarCalendar _selectLunarDate;

        public LunarCalendar SelectLunarDate
        {
            get { return _selectLunarDate; }
        }

        public DateTime SelectSolarDate
        {
            get
            {
                return _selectLunarDate.GetSolarDate(_selectLunarDate.intLunarDay, _selectLunarDate.intLunarMonth, _selectLunarDate.intLunarYear, _selectLunarDate.LunarMonthType == LunarCalendar.ENUM_LEAP_MONTH.LeapMonth);
            }
        }
        protected DateTime date = new DateTime();
        public CalendarVN(DateTime dateTime)
        {
            date = dateTime == null ? DateTime.Now : dateTime;
            InitializeComponent();
            // calendarMain.AllowEditingEvents = false;
            calendarMain.CalendarView = CalendarViews.Month;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }
        private void btnSelect_Load(object sender, EventArgs e)
        {

        }

        private void btnSelect_MouseClick(object sender, MouseEventArgs e)
        {
            _selectLunarDate = calendarMain.CalendarLunarDate;

        }

        private void btnExit_Load(object sender, EventArgs e)
        {

        }

        private void btnExit_MouseClick(object sender, MouseEventArgs e)
        {
            this.Dispose();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
