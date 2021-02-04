using GPConst;
using GPMain.Common;
using GPMain.Common.Navigation;
using GPModels;
using MaterialSkin;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using GPMain.Common.Helper;

namespace GPMain.Views.Member
{
    public partial class popupAddRelation : BaseUserControl
    {
        string typeMember;
        TMember submember;
        TMember primaryMember;
        string relationType;
        string relationType2;
        string gender;
        List<MRelation> dataMRelation;
        MemberHelper memberHelper = new MemberHelper();
        MemberRelationHelper memberRelation = new MemberRelationHelper();
        public popupAddRelation(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            TitleBar = "Thêm quan hệ";

            this.BackColor = AppConst.PopupBackColor;

            GetParameter();

            var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
            var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
            dataMRelation = tblMRelation.ToList(i => i.DeleteDate == null);

            if (dataMRelation != null)
            {
                List<string> lstRelationMainMember = new List<string>();
                if (primaryMember != null)
                {
                    lstRelationMainMember = (from tabRelation in tblMRelation.AsEnumerable()
                                             join tabTMemRel in tblTMemberRelation.AsEnumerable()
                                             on tabRelation.MainRelation equals tabTMemRel.relType
                                             where (tabTMemRel.memberId == primaryMember.Id) 
                                                   && tabRelation.IsMain 
                                                   && (tabTMemRel.relType.Contains(Relation.PREFIX_DAD) || tabTMemRel.relType.Contains(Relation.PREFIX_MOM)) 
                                                   && tabTMemRel.DeleteDate == null
                                             select tabTMemRel.relType).ToList();
                }

                var dicRelation = new Dictionary<string, string>();
                string relName = string.Empty;
                foreach (var item in dataMRelation)
                {
                    // get name of related relation
                    var objRelated = tblMRelation.FirstOrDefault(i => i.MainRelation == item.RelatedRelation);
                    if (objRelated == null) continue;

                    bool bRelationWithGenderMale = gender == AppConst.Gender.Male && (objRelated.MainRelation.Contains(Relation.PREFIX_DAD) || objRelated.MainRelation.Contains(Relation.PREFIX_HUSBAND));
                    bool bRelationWithGenderFeMale = gender == AppConst.Gender.Female && (objRelated.MainRelation.Contains(Relation.PREFIX_MOM) || objRelated.MainRelation.Contains(Relation.PREFIX_WIFE));


                    if (string.IsNullOrEmpty(relationType))
                    {
                        if (string.IsNullOrEmpty(gender))
                        {
                            dicRelation[item.Id] = item.NameOfRelation + " - " + objRelated.NameOfRelation;
                        }
                        else if (bRelationWithGenderMale || bRelationWithGenderFeMale ||
                                ((item.MainRelation.Contains(Relation.PREFIX_DAD) || item.MainRelation.Contains(Relation.PREFIX_MOM)) && !lstRelationMainMember.Contains(item.MainRelation))
                                )
                        {
                            if ((AppManager.ModeDisplay == ModeDisplay.BuildTree) ||
                                (AppManager.ModeDisplay == ModeDisplay.ViewTree && !item.MainRelation.Contains(Relation.PREFIX_DAD) && !item.MainRelation.Contains(Relation.PREFIX_MOM)))
                            {
                                dicRelation[item.Id] = item.NameOfRelation + " - " + objRelated.NameOfRelation;
                            }
                        }
                        continue;
                    }
                    if (item.Id == relationType)
                    {
                        relName = item.MainRelation + " - " + objRelated.MainRelation;
                    }
                    string[] arrRel = relName.Split('-');
                    if (arrRel.Length != 2)
                    {
                        continue;
                    }
                    if (arrRel[0].Contains(item.MainRelation.Substring(0, 3)) && arrRel[1].Contains(objRelated.MainRelation.Substring(0, 3)))
                    {
                        dicRelation[item.Id] = item.NameOfRelation + " - " + objRelated.NameOfRelation;
                    }
                }

                cmbRelationship.DataSource = new BindingSource(dicRelation, null);
                cmbRelationship.DisplayMember = "Value";
                cmbRelationship.ValueMember = "Key";
            }
        }
        private void GetParameter()
        {
            gender = this.Params.GetValue<string>("gender", AppConst.Gender.Male);
            typeMember = this.Params.GetValue<string>("type_member");
            submember = this.Params.GetValue<TMember>("sub_member");
            primaryMember = this.Params.GetValue<TMember>("primary_member");
            if (primaryMember == null)
            {
                primaryMember = this.Params.GetValue<TMember>();
            }
            relationType = this.Params.GetValue<string>("relation_type");
            relationType2 = this.Params.GetValue<string>("relation_type2");
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (primaryMember == null)
                {
                    primaryMember = this.Params.GetValue<TMember>();
                }

                relationType = cmbRelationship.SelectedValue.ToString();
                if (!string.IsNullOrEmpty(relationType2))
                {
                    var tblRelation = AppManager.DBManager.GetTable<MRelation>();
                    var mainRelation = tblRelation.FirstOrDefault(x => x.Id == relationType);
                    if (mainRelation != null)
                    {
                        var strMainRelation = mainRelation.RelatedRelation;
                        if (strMainRelation.Contains(Relation.PREFIX_DAD))
                        {
                            string temp = Relation.PREFIX_MOM + strMainRelation.Substring(Relation.PREFIX_DAD.Length);
                            var spouseRelation = tblRelation.FirstOrDefault(x => x.RelatedRelation == temp);
                            if (spouseRelation != null)
                            {
                                relationType2 = spouseRelation.Id;
                            }
                        }
                        else if (strMainRelation.Contains(Relation.PREFIX_MOM))
                        {
                            string temp = Relation.PREFIX_DAD + strMainRelation.Substring(Relation.PREFIX_MOM.Length);
                            var spouseRelation = tblRelation.FirstOrDefault(x => x.RelatedRelation == temp);
                            if (spouseRelation != null)
                            {
                                relationType2 = spouseRelation.Id;
                            }
                        }
                    }
                }

                MRelation relation = dataMRelation.FirstOrDefault(i => i.Id == relationType);
                if (relation != null)
                {
                    if (relation.MainRelation.Contains(Relation.PREFIX_DAD) || relation.MainRelation.Contains(Relation.PREFIX_HUSBAND))
                    {
                        gender = AppConst.Gender.Male;
                    }
                    else if (relation.MainRelation.Contains(Relation.PREFIX_MOM) || relation.MainRelation.Contains(Relation.PREFIX_WIFE))
                    {
                        gender = AppConst.Gender.Female;
                    }
                }

                var newParams = new NavigationParameters();

                if (string.IsNullOrEmpty(typeMember) && primaryMember != null && relation != null)
                {
                    typeMember = relation.MainRelation.Contains(Relation.PREFIX_DAD) ? AppConst.NameDefaul.Father :
                                (relation.MainRelation.Contains(Relation.PREFIX_CHILD) ? AppConst.NameDefaul.Child : "");
                    switch (relation.MainRelation.Substring(0, 3))
                    {
                        case Relation.PREFIX_DAD: typeMember = AppConst.NameDefaul.Father; break;
                        case Relation.PREFIX_MOM: typeMember = AppConst.NameDefaul.Mother; break;
                        case Relation.PREFIX_CHILD: typeMember = AppConst.NameDefaul.Child; break;
                        case Relation.PREFIX_HUSBAND: typeMember = AppConst.NameDefaul.Husban; break;
                        case Relation.PREFIX_WIFE: typeMember = AppConst.NameDefaul.Wife; break;
                    }
                }

                typeMember = string.IsNullOrEmpty(typeMember) ? "" : typeMember;

                newParams.Add("type_member", typeMember);
                newParams.Add("sub_member", submember);
                newParams.Add("primary_member", primaryMember);
                newParams.Add("relation_type", relationType);
                newParams.Add("relation_type2", relationType2);
                newParams.Add("gender", gender);

                NavigationResult naviResult = new NavigationResult();

                this.Cursor = Cursors.WaitCursor;

                if (chkboxAddFromList.Checked == true)
                {
                    naviResult = AppManager.Navigation.ShowDialogWithParam<AddMemberRelationFromList>(newParams, ModeForm.New, AppConst.StatusBarColor);
                }
                else
                {
                    naviResult = AppManager.Navigation.ShowDialogWithParam<addMember>(newParams, ModeForm.New, AppConst.StatusBarColor);
                }

                var newMember = naviResult.GetValue<TMember>("newmember");

                AddAdditionalMembers(newMember, newParams, naviResult);
                if (naviResult.Result == DialogResult.OK)
                {
                    UpdateLevelInFamily();
                }
                this.Cursor = Cursors.Default;
                this.Close(naviResult);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(popupAddRelation), ex);
            }
        }

        private void UpdateLevelInFamily()
        {
            if (memberHelper.RootTMember != null && AppManager.Dialog.Confirm("Bạn có muốn cập nhật lại thứ bậc?"))
            {
                AppManager.Dialog.ShowProgressBar(progressBar =>
                {
                    memberHelper.UpdateLevelInFamily(progressBar);
                }, "Đang cập nhật lại thứ bậc...", $"Cập nhật thứ bậc");
            }
        }

        //Thêm quan hệ bổ sung cho thành viên
        private void AddAdditionalMembers(TMember newMember, NavigationParameters newParams, NavigationResult naviResult)
        {
            if (newMember == null) return;

            TMember mainMember = memberHelper.GetTMemberByID(primaryMember.Id);
            var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
            //Thành viên vừa được thêm là con
            if (typeMember.Equals(AppConst.NameDefaul.Child) && mainMember.ListSPOUSE.Count > 0)
            {
                var lstParent = newMember.ListPARENT;

                var lstTemp = mainMember.ListSPOUSE.Where(x => lstParent.Contains(x)).ToList();
                if (lstTemp.Count > 0) return;
                // Xác nhận thêm cha/mẹ cho thành viên mới
                if (AppManager.Dialog.Confirm($"Bạn có muốn thêm {(mainMember.Gender == (int)EmGender.Male ? "mẹ" : "cha")} cho thành viên {newMember.Name}"))
                {
                    newParams.Add("newmember", newMember);
                    newParams.Add("addlistmember", true);
                    AppManager.Navigation.ShowDialogWithParam<AddMemberRelationFromList>(newParams);
                }
            }
            //Thành viên vừa được thêm là cha/mẹ và có vợ/chồng
            else if ((typeMember.Equals(AppConst.NameDefaul.Father) || typeMember.Equals(AppConst.NameDefaul.Mother)) && newMember.ListSPOUSE.Count > 0)
            {
                var lstParent = mainMember.ListPARENT;

                var lstTemp = newMember.ListSPOUSE.Where(x => lstParent.Contains(x)).ToList();
                if (lstTemp.Count > 0) return;
                //Xác nhận thêm cha/mẹ cho thành viên chính nếu thành viên mới có vợ/chồng
                if (AppManager.Dialog.Confirm($"Bạn có muốn thêm {(newMember.Gender == (int)EmGender.Male ? "mẹ" : "cha")} cho thành viên {mainMember.Name}"))
                {
                    newParams.Add("newmember", newMember);
                    newParams.Add("addlistmember", true);
                    naviResult = AppManager.Navigation.ShowDialogWithParam<AddMemberRelationFromList>(newParams);

                    var newMember2 = naviResult.GetValue<TMember>("newmember");

                    if (mainMember.ListPARENT.Contains(newMember.Id) && mainMember.ListPARENT.Contains(newMember2.Id) && !newMember.ListSPOUSE.Contains(newMember2.Id))
                    {
                        string relate = newMember.Gender == (int)EmGender.Male ? tblMRelation.AsEnumerable().FirstOrDefault(x => x.MainRelation.Contains(Relation.PREFIX_WIFE))?.Id ?? "" :
                                                                                 tblMRelation.AsEnumerable().FirstOrDefault(x => x.MainRelation.Contains(Relation.PREFIX_HUSBAND))?.Id ?? "";
                        if (!string.IsNullOrEmpty(relate))
                        {
                            memberRelation.addMemberRelation(relate, newMember, newMember2);
                        }
                    }
                }
            }
            //Thành viên vừa được thêm là cha/mẹ và không có vợ/chồng
            else if ((typeMember.Equals(AppConst.NameDefaul.Father) || typeMember.Equals(AppConst.NameDefaul.Mother)) && newMember.ListSPOUSE.Count == 0)
            {
                var member = memberHelper.GetTMemberByID(mainMember.Id);
                var lstParent = member.ListPARENT;
                var parentID = lstParent.FirstOrDefault(x => !x.Equals(newMember.Id)) ?? "";

                var parent = memberHelper.GetTMemberByID(parentID);

                if (lstParent.Count == 2 && parent != null && parent.Gender != newMember.Gender)
                {
                    string relate = newMember.Gender == (int)EmGender.Male ? tblMRelation.AsEnumerable().FirstOrDefault(x => x.MainRelation.Contains(Relation.PREFIX_WIFE))?.Id ?? "" :
                                                                             tblMRelation.AsEnumerable().FirstOrDefault(x => x.MainRelation.Contains(Relation.PREFIX_HUSBAND))?.Id ?? "";
                    if (!string.IsNullOrEmpty(relate))
                    {
                        memberRelation.addMemberRelation(relate, newMember, parent);
                    }
                }
            }
        }
    }
}