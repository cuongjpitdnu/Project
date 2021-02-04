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
    public partial class ShowLevelInFamilyItem : ItemBase
    {
        public ShowLevelInFamilyItem(DrawTreeConfig config = null) : base(config)
        {
            InitializeComponent();
            cblevelInFamily.TextChanged += (sender, e) => ChangeData_Event();
        }
        protected override void SetConfig()
        {
            base.SetConfig();
            Config.MaxLevelInFamily = int.Parse(cblevelInFamily.Text);
        }

        protected override void SetDisplayUI()
        {
            base.SetDisplayUI();
            int maxLevelInFamily = Config.MaxLevelInFamily;
            maxLevelInFamily = maxLevelInFamily == 0 ? 1 : maxLevelInFamily;
            var arrTemp = Enumerable.Range(1, 20).Select(x => (object)x).ToArray();
            cblevelInFamily.Items.Clear();
            cblevelInFamily.Items.AddRange(arrTemp);
            cblevelInFamily.SelectedIndex = Config.MaxLevelInFamily - 1;
        }
    }
}
