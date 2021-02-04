using GP40Main.Core;
using GP40Main.Models;
using GP40Main.Views;
using GP40Main.Views.Config;
using GP40Main.Views.FamilyInfo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GP40Main.Themes
{
    public partial class MenuForm : BaseForm
    {
        private Dictionary<string, Type> mappingMenuToView = new Dictionary<string, Type>();

        public MenuForm()
        {
            InitializeComponent();

            this.SetDefaultUI();
            this.Padding = new Padding(this.Padding.Left, STATUS_BAR_HEIGHT + ACTION_BAR_HEIGHT, this.Padding.Right, this.Padding.Bottom);

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor, false);

            foreach (TabPage tabChild in tabMain.TabPages)
            {
                tabChild.BackColor = BackColor;
            }

            mappingMenuToView.Add(tabTree.Name, typeof(TreeViewer));
            mappingMenuToView.Add(tabFamily.Name, typeof(FamilyInfoFullPage));
            mappingMenuToView.Add(tabMember.Name, typeof(ListMember));
            mappingMenuToView.Add(tabConfig.Name, typeof(ConfigCommon));

            tabMain_SelectedIndexChanged(tabMain, new EventArgs());
        }

        private async void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabMain.SelectedTab != null && mappingMenuToView.ContainsKey(tabMain.SelectedTab.Name))
            {

                var infoFamily = AppManager.DBManager.GetTable<MFamilyInfo>().AsEnumerable().FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);

                if (tabTree.Text == tabMain.SelectedTab.Text)
                {
                    Text = string.Format("Phả đồ dòng họ {0}", infoFamily?.FamilyName.ToUpper()).Trim();
                }
                else
                {
                    Text = tabMain.SelectedTab.Text;
                }

                this.Invalidate();
                await AppManager.Navigation.NextMenuAsync(mappingMenuToView[tabMain.SelectedTab.Name], tabMain.SelectedTab);
            }
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }
    }
}
