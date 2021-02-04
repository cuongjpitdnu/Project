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
    public partial class ShowBirthDateItem : ItemBase
    {
        public ShowBirthDateItem(DrawTreeConfig config = null) : base(config)
        {
            InitializeComponent();
            rdshowdefauld.Click += (sender, e) => ChangeData_Event();
            rdshowunknow.Click += (sender, e) => ChangeData_Event();
        }

        protected override void SetConfig()
        {
            base.SetConfig();
            Config.ShowBirthDayDefaul = rdshowdefauld.Checked;
        }
        protected override void SetDisplayUI()
        {
            base.SetDisplayUI();
            rdshowdefauld.Checked = Config.ShowBirthDayDefaul;
            rdshowunknow.Checked = !Config.ShowBirthDayDefaul;
        }
    }
}
