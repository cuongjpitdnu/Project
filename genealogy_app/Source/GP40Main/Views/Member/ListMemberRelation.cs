using System;
using System.Linq;
using GP40Main.Core;
using GP40Main.Models;
using GP40Main.Services.Navigation;
using GP40Main.Utility;
using static GP40Main.Core.AppConst;

namespace GP40Main.Views.Member
{
    public partial class ListMemberRelation : BaseUserControl
    {
        public ListMemberRelation(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            LoadDataRelation(this.GetParameters().GetValue<TMember>());
        }

        private void btnAddRelation_Click(object sender, EventArgs e)
        {
            try {
                AppManager.Navigation.ShowDialog<popupAddRelation>(new NavigationParameters(this.GetParameters().GetValue<TMember>()), AppConst.ModeForm.New);
                this.Close();
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMemberRelation), ex);
            }
        }

        public void LoadDataRelation(TMember objParam = null)
        {
            try {
                var tblMember = AppManager.DBManager.GetTable<TMember>();
                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();

                var dataLinQ = from tMemberRelation in tblTMemberRelation.AsEnumerable()
                                join tMember in tblMember.AsEnumerable()
                                on tMemberRelation.relMemberId equals tMember.Id
                                join mRelation in tblMRelation.AsEnumerable()
                                on tMemberRelation.relType equals mRelation.MainRelation
                                where tMemberRelation.memberId.Equals(objParam.Id)
                                select new ExTMember
                                {
                                    Id = tMemberRelation.Id,
                                    Name = tMember.Name,
                                    GenderShow = (tMember.Gender == (int)GenderMember.Male) ? "Nam" : ((tMember.Gender == (int)GenderMember.Female) ? "Nữ" : "Chưa rõ"),
                                    RelTypeShow = mRelation.NameOfRelation
                                    //RelTypeShow = tMemberRelation.relType
                                };
                BindingHelper.BindingDataGrid(dgvListMemberRelation, dataLinQ.ToList());
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMemberRelation), ex);
            }
        }

        private async void dgvListMemberRelation_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            try {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                var rowSelected = dgvListMemberRelation.Rows[e.RowIndex].DataBoundItem as ExTMember;

                if (e.ColumnIndex == colActionDelete.Index)
                {
                    if (rowSelected != null)
                    {
                        if (AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                        {
                            // TMemberRelation
                            var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
                            // main member
                            var objTMemberRelation = tblTMemberRelation.CreateQuery(i => i.Id == rowSelected.Id).FirstOrDefault();
                            if (objTMemberRelation == null)
                            {
                                return;
                            }

                            // set date delete
                            objTMemberRelation.DeleteDate = DateTime.Now;

                            if (await tblTMemberRelation.UpdateOneAsync(i => i.Id == objTMemberRelation.Id, objTMemberRelation) == false)
                            {
                                return;
                            }

                            // find related member of main member
                            var objRelated = tblTMemberRelation.CreateQuery(i => i.memberId == objTMemberRelation.relMemberId
                                                                            && i.relMemberId == objTMemberRelation.memberId).FirstOrDefault();

                            if (objRelated != null)
                            {
                                // set date delete
                                objRelated.DeleteDate = DateTime.Now;

                                if (await tblTMemberRelation.UpdateOneAsync(i => i.Id == objRelated.Id, objRelated) == false)
                                {
                                    return;
                                }
                            }

                            // TMember
                            var tblTMember = AppManager.DBManager.GetTable<TMember>();
                            // Relation
                            // main member
                            var objMainMember = tblTMember.CreateQuery(i => i.Id == objTMemberRelation.memberId).FirstOrDefault();
                            if (objMainMember != null)
                            {
                                // listPARENT - SPOUSE - CHILD
                                if (objTMemberRelation.relType.Contains(cstrPreFixDAD) || objTMemberRelation.relType.Contains(cstrPreFixMOM))
                                {
                                    objMainMember.ListPARENT.Remove(objTMemberRelation.relMemberId);
                                }
                                else if (objTMemberRelation.relType.Contains(cstrPreFixHUSBAND) || objTMemberRelation.relType.Contains(cstrPreFixWIFE))
                                {
                                    objMainMember.ListSPOUSE.Remove(objTMemberRelation.relMemberId);
                                }
                                else if (objTMemberRelation.relType.Contains(cstrPreFixCHILD))
                                {
                                    objMainMember.ListCHILDREN.Remove(objTMemberRelation.relMemberId);
                                }
                                var findInRelation = objMainMember.Relation.FindAll(i => i.memberId == objTMemberRelation.memberId && i.relMemberId != objTMemberRelation.relMemberId);
                                objMainMember.Relation = findInRelation;

                                if (await tblTMember.UpdateOneAsync(i => i.Id == objMainMember.Id, objMainMember) == false)
                                {
                                    return;
                                }
                            }

                            // related member
                            var objMemberRelated = tblTMember.CreateQuery(i => i.Id == objTMemberRelation.relMemberId).FirstOrDefault();
                            if (objMemberRelated != null)
                            {
                                // listPARENT - SPOUSE - CHILD
                                if (objRelated.relType.Contains(cstrPreFixDAD) || objRelated.relType.Contains(cstrPreFixMOM))
                                {
                                    objMemberRelated.ListPARENT.Remove(objTMemberRelation.memberId);
                                }
                                else if (objRelated.relType.Contains(cstrPreFixHUSBAND) || objRelated.relType.Contains(cstrPreFixWIFE))
                                {
                                    objMemberRelated.ListSPOUSE.Remove(objTMemberRelation.memberId);
                                }
                                else if (objRelated.relType.Contains(cstrPreFixCHILD))
                                {
                                    objMemberRelated.ListCHILDREN.Remove(objTMemberRelation.memberId);
                                }
                                var findInRelation = objMemberRelated.Relation.FindAll(i => i.memberId == objTMemberRelation.relMemberId && i.relMemberId != objTMemberRelation.memberId);
                                objMemberRelated.Relation = findInRelation;

                                if (await tblTMember.UpdateOneAsync(i => i.Id == objMemberRelated.Id, objMemberRelated) == false)
                                {
                                    return;
                                }
                            }
                            LoadDataRelation(this.GetParameters().GetValue<TMember>());
                        }
                    }
                }
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMemberRelation), ex);
            }
        }
    }
}
