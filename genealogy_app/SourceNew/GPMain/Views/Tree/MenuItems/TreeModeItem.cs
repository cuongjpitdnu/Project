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
    public partial class TreeModeItem : ItemBase
    {
        public event EventHandler<bool> ChangeMode;
        private void ChangeMode_Event(object sender, EventArgs e)
        {
            if (ChangeMode != null)
            {
                ChangeMode.Invoke(sender, rbexpand.Checked);
            }
        }
        public TreeModeItem(DrawTreeConfig config = null) : base(config)
        {
            InitializeComponent();
            rbexpand.Click += (sender, e) => ChangeMode_Event(sender, e);
            rbNormal.Click += (sender, e) => ChangeMode_Event(sender, e);
        }

        protected override void SetConfig()
        {
            base.SetConfig();
        }

        protected override void SetDisplayUI()
        {
            base.SetDisplayUI();
        }
    }
}
