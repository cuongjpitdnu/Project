using GPMain.Common;
using GPMain.Views.Config;
using GPMain.Views.FamilyInfo;
using GPMain.Views.Tree;
using GPModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using GPMain.Views.Tree.Build;

namespace GPMain.Views
{
    public partial class MenuForm : BaseForm
    {
        private Dictionary<string, Type> mappingMenuToView = new Dictionary<string, Type>();
        private bool stageInit = true;

        public MenuForm() : base()
        {
            InitializeComponent();

            //this.SetDefaultUI();
            this.Padding = new Padding(this.Padding.Left, STATUS_BAR_HEIGHT + ACTION_BAR_HEIGHT, this.Padding.Right, this.Padding.Bottom);
            this.MinimumSize = this.Size;
            this.Sizable = false;

            foreach (TabPage tabChild in tabMain.TabPages)
            {
                tabChild.BackColor = BackColor;
            }

            mappingMenuToView.Add(tabTree.Name, typeof(TreeViewer));
            mappingMenuToView.Add(tabFamily.Name, typeof(FamilyInfoFullPage));
            mappingMenuToView.Add(tabMember.Name, typeof(ListMember));
            mappingMenuToView.Add(tabConfig.Name, typeof(ConfigCommon));
            mappingMenuToView.Add(tabInfo.Name, typeof(About));
            mappingMenuToView.Add(tabEvent.Name, typeof(Events));
        }

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stageInit)
            {
                return;
            }

            if (tabMain.SelectedTab != null && mappingMenuToView.ContainsKey(tabMain.SelectedTab.Name))
            {

                var infoFamily = AppManager.DBManager.GetTable<MFamilyInfo>().FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);

                if (tabTree.Text == tabMain.SelectedTab.Text)
                {
                    Text = string.Format("Phả đồ dòng họ {0}", infoFamily?.FamilyName.ToUpper()).Trim();
                }
                else
                {
                    Text = tabMain.SelectedTab.Text;
                }

                this.Invalidate();
                this.TitleBar = $"{AppConst.TitleBarFisrt}{tabMain.SelectedTab.Text}";
                Application.DoEvents();
                AppManager.Navigation.NextMenu(mappingMenuToView[tabMain.SelectedTab.Name], tabMain.SelectedTab);
            }
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            stageInit = false;
            Task.Run(() => this.BeginInvoke(new Action(() => tabMain_SelectedIndexChanged(tabMain, new EventArgs()))));
        }
    }
}
