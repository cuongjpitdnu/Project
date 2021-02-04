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
    public partial class ShowImageItem : ItemBase
    {
        public ShowImageItem(DrawTreeConfig config = null) : base(config)
        {
            InitializeComponent();
            rdshowimage.Click += (sender, e) => ChangeData_Event(); 
            rdhideimage.Click += (sender, e) => ChangeData_Event();
        }
        protected override void SetConfig()
        {
            base.SetConfig();
            Config.ShowImage = rdshowimage.Checked;
        }
        protected override void SetDisplayUI()
        {
            base.SetDisplayUI();
            rdshowimage.Checked = Config.ShowImage;
            rdhideimage.Checked = !Config.ShowImage;
        }
    }
}
