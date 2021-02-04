using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GP40Common
{
    public partial class frmCalendarVN : Form
    {
        clsLunarCalendar _selectLunarDate;

        public clsLunarCalendar SelectLunarDate
        {
            get { return _selectLunarDate; }
        }

        public frmCalendarVN()
        {
            InitializeComponent();
            calendarMain.AllowEditingEvents = true;
            calendarMain.CalendarView = CalendarViews.Month;
            calendarMain.CalendarSolarDate = DateTime.Now;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void btnSelect_Load(object sender, EventArgs e)
        {
           
        }

        private void btnSelect_MouseClick(object sender, MouseEventArgs e)
        {
            _selectLunarDate = calendarMain.CalendarLunarDate;

            //List<clsLunarCalendar> lstLC = obj.GetLunarMonthDays(4, 2020);
           // int x = obj.GetLunarMonthDays(4, 2020);
        }

        private void btnExit_Load(object sender, EventArgs e)
        {
       
        }

        private void btnExit_MouseClick(object sender, MouseEventArgs e)
        {
            this.Dispose();
        }
    }
}
