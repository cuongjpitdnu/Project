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
    public partial class OptionDrawTreeItem : ItemBase
    {
        public OptionDrawTreeItem(DrawTreeConfig config = null) : base(config)
        {
            InitializeComponent();
            ckshowfamilylevel.Click += (sender, e) => ChangeData_Event();
            ckshowgender.Click += (sender, e) => ChangeData_Event();
            rdnormal.Click += (sender, e) => ChangeData_Event();
            rdturnright.Click += (sender, e) => ChangeData_Event();
            rdturnleft.Click += (sender, e) => ChangeData_Event();
        }

        protected override void SetConfig()
        {
            base.SetConfig();
            Config.ShowFamilyLevel = ckshowfamilylevel.Checked;
            Config.ShowGender = ckshowgender.Checked;
            Config.TypeTextShow = rdnormal.Checked ? TextShow.Normal.ToString() : rdturnright.Checked ? TextShow.TurnRight.ToString() : TextShow.TurnLeft.ToString();
        }
        protected override void SetDisplayUI()
        {
            base.SetDisplayUI();
            ckshowfamilylevel.Checked = Config.ShowFamilyLevel;
            ckshowgender.Checked = Config.ShowGender;
            rdnormal.Checked = Config.TypeTextShow == TextShow.Normal.ToString();
            rdturnright.Checked = Config.TypeTextShow == TextShow.TurnRight.ToString();
            rdturnleft.Checked = Config.TypeTextShow == TextShow.TurnLeft.ToString();
        }
    }
}
