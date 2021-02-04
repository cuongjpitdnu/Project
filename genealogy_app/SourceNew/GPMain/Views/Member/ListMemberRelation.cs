using GPConst;
using GPMain.Common;
using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using GPMain.Views.Tree.Build;
using GPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GPMain.Views.Member
{
    public partial class ListMemberRelation : BaseUserControl
    {
        DialogResult dialogResult = DialogResult.Cancel;
        string gender = "";
        RelationParam relationFilter { get; set; }
        Dictionary<string, ExTMember> DicMember = new Dictionary<string, ExTMember>();

        MemberHelper memberHelper { get; set; } = new MemberHelper();

        public ListMemberRelation(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            TitleBar = "Danh sách quan hệ";

            this.BackColor = AppConst.PopupBackColor;

            var member = this.Params.GetValue<TMember>();
            relationFilter = this.Params.GetValue<RelationParam>();
            if (relationFilter == null)
            {
                relationFilter = new RelationParam() { Member = ConvertExtMemberFromTMember(member), RelationFilter = "" };
                relationFilter.Gender = relationFilter.Member.GenderShow;
            }

            gender = relationFilter.Gender;
            this.HandleDestroyed += (sender, e) =>
            {
                Close(dialogResult);
            };

            UIHelper.SetColumnDeleteAction<ExTMember>(dgvListMemberRelation, rowSelected =>
            {
                try
                {
                    if (rowSelected != null)
                    {
                        if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                        {
                            return;
                        }

                        // TMemberRelation
                        var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
                        // main member
                        var objTMemberRelation = tblTMemberRelation.FirstOrDefault(i => i.Id == rowSelected.IdRelate);
                        if (objTMemberRelation == null)
                        {
                            return;
                        }

                        if (objTMemberRelation.relType.Contains(Relation.PREFIX_DAD) || objTMemberRelation.relType.Contains(Relation.PREFIX_MOM))
                        {
                            var lstParent = relationFilter.Member.ListPARENT;

                            var memberTemp = DicMember[rowSelected.Id];
                            RemoveRelation(relationFilter.Member, memberTemp);
                        }
                        else if (objTMemberRelation.relType.Contains(Relation.PREFIX_CHILD))
                        {
                            if (DicMember.ContainsKey(objTMemberRelation.relMemberId))
                            {
                                var memberTemp = DicMember[objTMemberRelation.relMemberId];

                                RemoveRelation(relationFilter.Member, memberTemp);

                                var lstSpouseMemberTemp = memberTemp.ListPARENT;
                                if (lstSpouseMemberTemp.Count > 0)
                                {
                                    string parentTempID = relationFilter.Member.ListPARENT.FirstOrDefault(i => !i.Equals(memberTemp.Id) && lstSpouseMemberTemp.Contains(i));

                                    if (!string.IsNullOrEmpty(parentTempID) && DicMember.ContainsKey(parentTempID))
                                    {
                                        var parentTemp = DicMember[parentTempID];
                                        RemoveRelation(relationFilter.Member, memberTemp);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (DicMember.ContainsKey(objTMemberRelation.relMemberId))
                            {
                                var memberTemp = DicMember[objTMemberRelation.relMemberId];
                                RemoveRelation(relationFilter.Member, memberTemp);
                            }
                        }
                        LoadDataRelation(relationFilter.Member, relationFilter.RelationFilter);
                        AppManager.MenuMemberBuffer.MemberCurrent = null;
                        dialogResult = DialogResult.OK;
                    }
                }
                catch (Exception ex)
                {
                    AppManager.Dialog.Error(ex.Message);
                    AppManager.LoggerApp.Error(typeof(ListMemberRelation), ex);
                }
            });
            LoadDataRelation(relationFilter.Member, relationFilter.RelationFilter);
        }

        //private void RemoveRelation(ExTMember member)
        //{
        //    // TMemberRelation
        //    var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
        //    // main member
        //    var objTMemberRelation = tblTMemberRelation.FirstOrDefault(i => i.Id == member.IdRelate);
        //    if (objTMemberRelation == null)
        //    {
        //        return;
        //    }
        //    // set date delete relation
        //    objTMemberRelation.DeleteDate = DateTime.Now;

        //    if (tblTMemberRelation.UpdateOne(i => i.Id == objTMemberRelation.Id, objTMemberRelation) == false)
        //    {
        //        return;
        //    }
        //    // find related member of main member
        //    var objRelated = tblTMemberRelation.CreateQuery(i => i.memberId == objTMemberRelation.relMemberId
        //                                                    && i.relMemberId == objTMemberRelation.memberId).FirstOrDefault();
        //    if (objRelated != null)
        //    {
        //        // set date delete
        //        objRelated.DeleteDate = DateTime.Now;

        //        if (tblTMemberRelation.UpdateOne(i => i.Id == objRelated.Id, objRelated) == false)
        //        {
        //            return;
        //        }
        //    }
        //    // TMember
        //    var tblTMember = AppManager.DBManager.GetTable<TMember>();

        //    // Relation
        //    // main member
        //    var objMainMember = tblTMember.FirstOrDefault(i => i.Id == objTMemberRelation.memberId);

        //    TMember memberTemp = objMainMember;

        //    if (objMainMember != null)
        //    {
        //        // listPARENT - SPOUSE - CHILD
        //        if (objTMemberRelation.relType.Contains(Relation.PREFIX_DAD) || objTMemberRelation.relType.Contains(Relation.PREFIX_MOM))
        //        {
        //            objMainMember.ListPARENT.Remove(objTMemberRelation.relMemberId);
        //            objMainMember.RootID = objMainMember.Id;

        //        }
        //        else if (objTMemberRelation.relType.Contains(Relation.PREFIX_HUSBAND) || objTMemberRelation.relType.Contains(Relation.PREFIX_WIFE))
        //        {
        //            objMainMember.ListSPOUSE.Remove(objTMemberRelation.relMemberId);
        //            if (objMainMember.SpouseInRootTree)
        //            {
        //                objMainMember.InRootTree = objMainMember.SpouseInRootTree = false;
        //            }
        //            objMainMember.RootID = objMainMember.Id;
        //        }
        //        else if (objTMemberRelation.relType.Contains(Relation.PREFIX_CHILD))
        //        {
        //            objMainMember.ListCHILDREN.Remove(objTMemberRelation.relMemberId);
        //        }
        //        var findInRelation = objMainMember.Relation.FindAll(i => i.memberId == objTMemberRelation.memberId && i.relMemberId != objTMemberRelation.relMemberId);
        //        objMainMember.Relation = findInRelation;

        //        if (tblTMember.UpdateOne(i => i.Id == objMainMember.Id, objMainMember) == false)
        //        {
        //            return;
        //        }
        //    }
        //    // related member 
        //    var objMemberRelated = tblTMember.CreateQuery(i => i.Id == objTMemberRelation.relMemberId).FirstOrDefault();
        //    if (objMemberRelated != null)
        //    {
        //        // listPARENT - SPOUSE - CHILD
        //        if (objRelated.relType.Contains(Relation.PREFIX_DAD) || objRelated.relType.Contains(Relation.PREFIX_MOM))
        //        {
        //            objMemberRelated.ListPARENT.Remove(objTMemberRelation.memberId);
        //            objMemberRelated.RootID = objMemberRelated.Id;
        //            memberTemp = objMemberRelated;
        //        }
        //        else if (objRelated.relType.Contains(Relation.PREFIX_HUSBAND) || objRelated.relType.Contains(Relation.PREFIX_WIFE))
        //        {
        //            objMemberRelated.ListSPOUSE.Remove(objTMemberRelation.memberId);
        //            if (objMemberRelated.SpouseInRootTree)
        //            {
        //                objMemberRelated.InRootTree = objMemberRelated.SpouseInRootTree = false;
        //                memberTemp = objMemberRelated;
        //            }
        //            objMemberRelated.RootID = objMainMember.Id;
        //        }
        //        else if (objRelated.relType.Contains(Relation.PREFIX_CHILD))
        //        {
        //            objMemberRelated.ListCHILDREN.Remove(objTMemberRelation.memberId);

        //        }
        //        var findInRelation = objMemberRelated.Relation.FindAll(i => i.memberId == objTMemberRelation.relMemberId && i.relMemberId != objTMemberRelation.memberId);
        //        objMemberRelated.Relation = findInRelation;

        //        if (tblTMember.UpdateOne(i => i.Id == objMemberRelated.Id, objMemberRelated) == false)
        //        {
        //            return;
        //        }
        //    }

        //    if (memberTemp != null)
        //    {
        //        if (AppManager.Dialog.Confirm("Bạn có muốn cập nhật lại đời và thông tin thành viên?"))
        //        {
        //            AppManager.Dialog.ShowProgressBar(progressBar =>
        //            {
        //                memberHelper.UpdateLevelInFamily(progressBar, memberTemp);
        //            }, "Đang cập nhật lại đời và thông tin thành viên...", "Cập nhật thông tin thành viên");
        //        }
        //    }
        //}

        private void RemoveRelation(ExTMember mainMember, ExTMember memberDelete)
        {
            RemoveRelate(mainMember, memberDelete);
            mainMember = memberHelper.GetExTMemberByID(mainMember.Id);
            memberDelete = memberHelper.GetExTMemberByID(memberDelete.Id);
            RemoveRelate(memberDelete, mainMember);

            AppManager.MenuMemberBuffer.ListAllMember.Clear();
            AppManager.MenuMemberBuffer.ListMember.Clear();
        }

        private void RemoveRelate(TMember mainMember, TMember memberDelete)
        {
            TMember member = memberHelper.GetTMemberByID(mainMember.Id);
            TMember memberDel = memberHelper.GetTMemberByID(memberDelete.Id);

            var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
            var tblTMember = AppManager.DBManager.GetTable<TMember>();

            var tMemberRelation = tblTMemberRelation.AsEnumerable().FirstOrDefault(x => x.memberId.Equals(member.Id) && x.relMemberId.Equals(memberDel.Id));
            if (tMemberRelation == null) { return; }

            tMemberRelation.DeleteDate = DateTime.Now;
            if (!tblTMemberRelation.UpdateOne(i => i.Id == tMemberRelation.Id, tMemberRelation)) { return; }

            if (member == null || memberDel == null) { return; }

            if (tMemberRelation.relType.Contains(Relation.PREFIX_DAD) || tMemberRelation.relType.Contains(Relation.PREFIX_MOM))
            {
                member.ListPARENT.Remove(memberDel.Id);
                member.RootID = member.Id;

                var lstParent = member.ListPARENT;
                var lstSpouse = memberDel.ListSPOUSE;

                var parentID = lstParent.FirstOrDefault(id => lstSpouse.Contains(id) && !id.Equals(memberDel.Id)) ?? "";

                var parent = memberHelper.GetExTMemberByID(parentID);

                if (parent != null)
                {
                    tblTMember.UpdateOne(mem => mem.Id == member.Id, member);
                    RemoveRelate(member, parent);
                    member = memberHelper.GetExTMemberByID(member.Id);
                    parent = memberHelper.GetExTMemberByID(parent.Id);
                    RemoveRelate(parent, member);
                }
            }
            else if (tMemberRelation.relType.Contains(Relation.PREFIX_HUSBAND) || tMemberRelation.relType.Contains(Relation.PREFIX_WIFE))
            {
                member.ListSPOUSE.Remove(memberDel.Id);
                if (member.SpouseInRootTree) { member.RootID = member.Id; }
                else if (memberDel.SpouseInRootTree) { memberDel.RootID = memberDel.Id; }
            }
            else if (tMemberRelation.relType.Contains(Relation.PREFIX_CHILD))
            {
                member.ListCHILDREN.Remove(memberDel.Id);
                memberDel.RootID = memberDel.Id;
            }

            tblTMember.UpdateOne(mem => mem.Id == member.Id, member);
            tblTMember.UpdateOne(mem => mem.Id == memberDel.Id, memberDel);
        }

        private ExTMember ConvertExtMemberFromTMember(TMember member)
        {
            return new ExTMember()
            {
                Id = member.Id,
                Name = member.Name + "",
                Gender = member.Gender,
                IsDeath = member.IsDeath,
                GenderShow = (member.Gender == (int)EmGender.Male) ? AppConst.Gender.Male : ((member.Gender == (int)EmGender.FeMale) ? AppConst.Gender.Female : AppConst.Gender.Unknow),
                BirthdayShow = member.Birthday != null ? member.Birthday.ToDateSun() : "",
                BirthdayLunarShow = member.Birthday != null ? member.Birthday.ToDateMoon() : "",
                DeadDaySunShow = member.DeadDay != null ? member.DeadDay.ToDateSun() : "",
                DeadDayLunarShow = member.DeadDay != null ? member.DeadDay.ToDateMoon() : "",
                Tel_1 = member.Contact.Tel_1 + "",
                Tel_2 = member.Contact.Tel_2 + "",
                Email_1 = member.Contact.Email_1 + "",
                Email_2 = member.Contact.Email_2 + "",
                Address = member.Contact.Address + "",
                Relation = member.Relation,
                Religion = member.Religion,
                National = member.National,
                TypeName = member.TypeName,
                HomeTown = member.HomeTown,
                BirthPlace = member.BirthPlace,
                DeadPlace = member.DeadPlace,
                ListCHILDREN = member.ListCHILDREN,
                ListPARENT = member.ListPARENT,
                ListSPOUSE = member.ListSPOUSE,
                AvatarImg = member.AvatarImg,
                LevelInFamily = member.LevelInFamily,
                ChildLevelInFamily = member.ChildLevelInFamily
            };
        }

        private void BtnAddRelation_Click(object sender, EventArgs e)
        {
            try
            {
                var dicRelation = new Dictionary<string, string>();
                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                var dataMRelation = tblMRelation.ToList(i => i.DeleteDate == null);

                if (dataMRelation != null)
                {
                    foreach (var item in dataMRelation)
                    {
                        dicRelation[item.Id] = item.MainRelation + " - " + item.RelatedRelation;
                    }
                }

                object relationTypeSelected = null;

                if (relationFilter.RelationFilter == GPConst.Relation.PREFIX_HUSBAND)
                {
                    relationTypeSelected = dicRelation.FirstOrDefault(v => v.Value == $"{Relation.PREFIX_HUSBAND}01 - {Relation.PREFIX_WIFE}01").Key;
                    gender = AppConst.Gender.Male;
                }
                else if (relationFilter.RelationFilter == GPConst.Relation.PREFIX_WIFE)
                {
                    relationTypeSelected = dicRelation.FirstOrDefault(v => v.Value == $"{Relation.PREFIX_WIFE}01 - {Relation.PREFIX_HUSBAND}01").Key;
                    gender = AppConst.Gender.Female;
                }
                else if (relationFilter.RelationFilter == GPConst.Relation.PREFIX_CHILD)
                {
                    string sParent = $"{Relation.PREFIX_DAD}01";
                    string sChild = $"{Relation.PREFIX_CHILD}01";
                    if (relationFilter.Member.GenderShow.Equals(AppConst.Gender.Female))
                    {
                        sParent = $"{Relation.PREFIX_MOM}01";
                        sChild = $"{Relation.PREFIX_CHILD}03";
                    }
                    relationTypeSelected = dicRelation.FirstOrDefault(v => v.Value == $"{sChild} - {sParent}").Key;
                }

                var newParams = new NavigationParameters()
                {
                    { "primary_member", relationFilter.Member},
                    { "relation_type", relationTypeSelected},
                    { "gender", gender}
                };

                var naviResult = AppManager.Navigation.ShowDialogWithParam<popupAddRelation>(newParams, ModeForm.New, AppConst.StatusBarColor);

                //var newMember = naviResult.GetValue<TMember>("newmember");

                //if (newMember != null && relationFilter.Member.ListSPOUSE.Count > 0)
                //{
                //    if (AppManager.Dialog.Confirm($"Bạn có muốn thêm {(relationFilter.Member.Gender == (int)EmGender.Male ? "mẹ" : "cha")} cho thành viên {newMember.Name}"))
                //    {
                //        newParams.Add("newmember", newMember);
                //        newParams.Add("addlistspouseofmember", true);
                //        AppManager.Navigation.ShowDialogWithParam<AddMemberRelationFromList>(newParams);

                //        AppManager.MenuMemberBuffer.ListAllMember.Clear();
                //        AppManager.MenuMemberBuffer.ListMember.Clear();
                //    }
                //}

                dialogResult = naviResult.Result;
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMemberRelation), ex);
            }
        }

        public void LoadDataRelation(TMember objParam = null, string relationFilter = "")
        {
            try
            {
                var tblMember = AppManager.DBManager.GetTable<TMember>();
                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();

                var dataLinQ = (from tMemberRelation in tblTMemberRelation.AsEnumerable()
                                join tMember in tblMember.AsEnumerable()
                                on tMemberRelation.relMemberId equals tMember.Id
                                join mRelation in tblMRelation.AsEnumerable()
                                on tMemberRelation.relType equals mRelation.MainRelation
                                where tMemberRelation.memberId.Equals(objParam.Id) && (string.IsNullOrEmpty(relationFilter) || tMemberRelation.relType.Substring(0, 3).Equals(relationFilter))
                                select new ExTMember
                                {
                                    Id = tMember.Id,
                                    IdRelate = tMemberRelation.Id,
                                    Name = tMember.Name,
                                    GenderShow = (tMember.Gender == (int)EmGender.Male) ? "Nam" : ((tMember.Gender == (int)EmGender.FeMale) ? "Nữ" : "Chưa rõ"),
                                    RelTypeShow = mRelation.NameOfRelation
                                }).ToList();

                DicMember = dataLinQ.ToDictionary(i => i.Id);

                BindingHelper.BindingDataGrid(dgvListMemberRelation, DicMember.Values.ToList());
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMemberRelation), ex);
            }
        }

        private void DeleteRelation_Event(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                if (e.ColumnIndex == colActionDelete.Index)
                {
                    if (dgvListMemberRelation.Rows[e.RowIndex].DataBoundItem is ExTMember rowSelected)
                    {
                        if (AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                        {
                            // TMemberRelation
                            var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
                            // main member
                            var objTMemberRelation = tblTMemberRelation.FirstOrDefault(i => i.Id == rowSelected.Id);
                            if (objTMemberRelation == null)
                            {
                                return;
                            }

                            // set date delete
                            objTMemberRelation.DeleteDate = DateTime.Now;

                            if (tblTMemberRelation.UpdateOne(i => i.Id == objTMemberRelation.Id, objTMemberRelation) == false)
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

                                if (tblTMemberRelation.UpdateOne(i => i.Id == objRelated.Id, objRelated) == false)
                                {
                                    return;
                                }
                            }

                            // TMember
                            var tblTMember = AppManager.DBManager.GetTable<TMember>();
                            // Relation
                            // main member
                            var objMainMember = tblTMember.FirstOrDefault(i => i.Id == objTMemberRelation.memberId);
                            if (objMainMember != null)
                            {
                                // listPARENT - SPOUSE - CHILD
                                if (objTMemberRelation.relType.Contains(Relation.PREFIX_DAD) || objTMemberRelation.relType.Contains(Relation.PREFIX_MOM))
                                {
                                    objMainMember.ListPARENT.Remove(objTMemberRelation.relMemberId);
                                }
                                else if (objTMemberRelation.relType.Contains(Relation.PREFIX_HUSBAND) || objTMemberRelation.relType.Contains(Relation.PREFIX_WIFE))
                                {
                                    objMainMember.ListSPOUSE.Remove(objTMemberRelation.relMemberId);
                                }
                                else if (objTMemberRelation.relType.Contains(Relation.PREFIX_CHILD))
                                {
                                    objMainMember.ListCHILDREN.Remove(objTMemberRelation.relMemberId);
                                }
                                var findInRelation = objMainMember.Relation.FindAll(i => i.memberId == objTMemberRelation.memberId && i.relMemberId != objTMemberRelation.relMemberId);
                                objMainMember.Relation = findInRelation;

                                if (tblTMember.UpdateOne(i => i.Id == objMainMember.Id, objMainMember) == false)
                                {
                                    return;
                                }
                            }

                            // related member
                            var objMemberRelated = tblTMember.CreateQuery(i => i.Id == objTMemberRelation.relMemberId).FirstOrDefault();
                            if (objMemberRelated != null)
                            {
                                // listPARENT - SPOUSE - CHILD
                                if (objRelated.relType.Contains(Relation.PREFIX_DAD) || objRelated.relType.Contains(Relation.PREFIX_MOM))
                                {
                                    objMemberRelated.ListPARENT.Remove(objTMemberRelation.memberId);
                                }
                                else if (objRelated.relType.Contains(Relation.PREFIX_HUSBAND) || objRelated.relType.Contains(Relation.PREFIX_WIFE))
                                {
                                    objMemberRelated.ListSPOUSE.Remove(objTMemberRelation.memberId);
                                }
                                else if (objRelated.relType.Contains(Relation.PREFIX_CHILD))
                                {
                                    objMemberRelated.ListCHILDREN.Remove(objTMemberRelation.memberId);
                                }
                                var findInRelation = objMemberRelated.Relation.FindAll(i => i.memberId == objTMemberRelation.relMemberId && i.relMemberId != objTMemberRelation.memberId);
                                objMemberRelated.Relation = findInRelation;

                                if (tblTMember.UpdateOne(i => i.Id == objMemberRelated.Id, objMemberRelated) == false)
                                {
                                    return;
                                }
                            }
                            LoadDataRelation(this.Params.GetValue<TMember>());
                        }
                    }
                }
                dialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ListMemberRelation), ex);
            }
        }
    }

    public class RelationParam
    {
        public ExTMember Member { get; set; }
        public string RelationFilter { get; set; }
        public string Gender { get; set; }
    }
}