using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPMain.Views.Controls;
using GP40DrawTree;

namespace GPMain.Views.Tree.MenuItems
{
    public partial class ShowDeathDayItem : ItemBase
    {
        public ShowDeathDayItem(DrawTreeConfig config = null) : base(config)
        {
            InitializeComponent();
            rdshowdeathdaycalendar.Click += (sender, e) => ChangeData_Event();
            rdshowdeathdaylunarcalendar.Click += (sender, e) => ChangeData_Event();
        }

        private void ShowDeathDay_Load(object sender, EventArgs e)
        {

        }
        protected override void SetConfig()
        {
            base.SetConfig();
            Config.ShowDeathDayLunarCalendar = rdshowdeathdaylunarcalendar.Checked;
        }
        protected override void SetDisplayUI()
        {
            base.SetDisplayUI();
            rdshowdeathdaycalendar.Checked = !Config.ShowDeathDayLunarCalendar;
            rdshowdeathdaylunarcalendar.Checked = Config.ShowDeathDayLunarCalendar;
        }
    }
}
