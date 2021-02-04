using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static GP40Main.Core.AppConst;
using GP40Main.Services.Navigation;
using GP40Main.Core;
using GP40Main.Models;
using System.Windows.Forms;

namespace GP40Main.Views.Member
{
    public partial class popupAddRelation : BaseUserControl
    {
        public popupAddRelation(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            var tblMRelation = AppManager.DBManager.GetTable<MRelation>();

            var dataMRelation = tblMRelation.CreateQuery(i => i.DeleteDate == null).ToList();
            if (dataMRelation != null)
            {
                var dicRelation = new Dictionary<string, string>();
                foreach (var item in dataMRelation)
                {
                    // get name of related relation
                    var objRelated = tblMRelation.AsEnumerable().FirstOrDefault(i => i.MainRelation == item.RelatedRelation);
                    if (objRelated != null)
                    {
                        dicRelation[item.Id] = item.NameOfRelation + " - " + objRelated.NameOfRelation;
                    }
                }
                cmbRelationship.DataSource = new BindingSource(dicRelation, null);
                cmbRelationship.DisplayMember = "Value";
                cmbRelationship.ValueMember = "Key";
            }

            //BindingTableToCombo<MRelation>(cmbRelationship, "NameOfRelation");
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try {
                var objParams = this.GetParameters().GetValue<TMember>();
                var relationTypeSelected = cmbRelationship.SelectedValue;
                var newParams = new NavigationParameters();
                newParams.Add("primary_member", objParams);
                newParams.Add("relation_type", relationTypeSelected);
                if (chkboxAddFromList.Checked == true)
                {
                    AppManager.Navigation.ShowDialog<AddMemberRelationFromList>(newParams, AppConst.ModeForm.New);
                }
                else
                {
                    AppManager.Navigation.ShowDialog<addMember>(newParams, AppConst.ModeForm.New);
                }
                this.Close();
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(popupAddRelation), ex);
            }
        }
    }
}
