using System;
using System.Linq;
using System.Windows.Forms;
using GP40Main.Core;
using GP40Main.Models;
using GP40Main.Services.Navigation;
using GP40Main.Utility;
using static GP40Main.Core.AppConst;

namespace GP40Main.Views.FamilyInfo
{
    /// <summary>
    /// Meno        : Display Family Info
    /// Create by   : AKB Bùi Minh Chiến
    /// </summary>
    public partial class FamilyInfoFullPage : BaseUserControl
    {
        public FamilyInfoFullPage(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            //Define
            var data = new[]
            {
                new []{"1", "Tộc trưởng 1", "(không rõ)", "(không rõ)"},
                new []{"2", "Tộc trưởng 2", "23/09/1987", "(không rõ)"},
                new []{"3", "Tộc trưởng 3", "(không rõ)", "(không rõ)"},
                new []{"4", "Tộc trưởng 4", "(không rõ)", "(không rõ)"},
            };

            //Add
            foreach (string[] version in data)
            {
                var item = new ListViewItem(version);
                materialListView1.Items.Add(item);
            }

            var objFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>().CreateQuery(i => i.Id == AppManager.LoginUser.FamilyId).FirstOrDefault();

            LoadFamilyInfo(objFamilyInfo);
            LoadFamilyTimeline();
        }

        #region Event Form

        private void btnEditFamilyInfo_Click(object sender, EventArgs e)
        {
            var objFamilyInfo = AppManager.Navigation.ShowDialog<FamilyInfoNewPage, MFamilyInfo>(ModeForm.Edit);
            LoadFamilyInfo(objFamilyInfo);
        }

        private void btnEditFamilyTimeline_Click(object sender, EventArgs e)
        {
            AppManager.Navigation.ShowDialog<FamilyTimeline>(ModeForm.New);
            LoadFamilyTimeline();
        }

        private void tblEvent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var rowSelected = tblEvent.Rows[e.RowIndex].DataBoundItem as MFamilyTimeline;

            if (rowSelected != null)
            {
                AppManager.Navigation.ShowDialog<FamilyTimeline, MFamilyTimeline>(new NavigationParameters(rowSelected), ModeForm.Edit);
                LoadFamilyTimeline();
            }
        }

        #endregion Event Form

        #region Private Function

        private void LoadFamilyInfo(MFamilyInfo familyInfo)
        {
            if (familyInfo != null)
            {
                txtFamilyName.Text = familyInfo.FamilyName;
                txtFamilyAnniversary.Text = familyInfo.FamilyAnniversary?.ToString("dd/MM/yyyy") ?? null;
                txtFamilyLevel.Text = familyInfo.FamilyLevel.ToString() ?? null;
                txtFamilyHometown.Text = familyInfo.FamilyHometown ?? null;
                txtUserCreated.Text = AppManager.LoginUser.FullName ?? null;
            }
        }

        private void LoadFamilyTimeline()
        {
            var dtaMFamilyTimeline = AppManager.DBManager.GetTable<MFamilyTimeline>().CreateQuery().OrderBy(i => i.StartDate).ToList();
            BindingHelper.BindingDataGrid(tblEvent, dtaMFamilyTimeline);
        }

        #endregion Private Function
    }
}
