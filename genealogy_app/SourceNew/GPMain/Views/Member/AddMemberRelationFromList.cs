using GPCommon;
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
    public partial class AddMemberRelationFromList : BaseUserControl
    {
        MemberHelper memberHelper = new MemberHelper();
        MemberRelationHelper memberRelation = new MemberRelationHelper();
        TMember primaryMember;
        string relationType;
        TMember subMember;
        string relationType2;
        bool addListMember;
        string gender;
        TMember newMember;
        string typeMember;
        public AddMemberRelationFromList(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            TitleBar = "Thêm mới quan hệ từ danh sách";

            this.BackColor = AppConst.PopupBackColor;
            GetParameter();
            LoadListTMember();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Lấy giá trị các tham số được truyền vào
        private void GetParameter()
        {
            primaryMember = this.Params.GetValue<TMember>("primary_member");//Thành viên chính
            relationType = this.Params.GetValue<string>("relation_type");// id quan hệ thành viên chính với thành viên thêm quan hệ
            subMember = this.Params.GetValue<TMember>("sub_member");// Thành viên phụ
            relationType2 = this.Params.GetValue<string>("relation_type2");//id quan hệ thành viên phụ với thành viên thêm quan hệ
            gender = this.Params.GetValue<string>("gender");// giới tính
            addListMember = this.Params.GetValue<bool>("addlistmember", false);//bit xác nhận danh sách thành viên lọc để thêm quan hệ là thuộc danh sách cha/mẹ, vợ/chồng hoặc là danh sách con của thành viên chính
            newMember = this.Params.GetValue<TMember>("newmember");//thành viên mới được thêm
            typeMember = this.Params.GetValue<string>("type_member");//kiểu thành viên là cha, mẹ, vợ, chồng, con
        }

        public void LoadListTMember(string keyword = "")
        {
            try
            {
                gender = gender == null ? "" : gender;
                var intGender = gender.Equals(AppConst.Gender.Male) ? 0 : gender.Equals(AppConst.Gender.Female) ? 1 : gender.Equals(AppConst.Gender.Unknow) ? 2 : -1;
                keyword = keyword.ToLower();

                var tblTMember = AppManager.DBManager.GetTable<TMember>();
                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();

                var allTMemberRelation = tblTMemberRelation.AsEnumerable().Select(i => i.memberId);

                var lstRelMemberRelation = tblTMemberRelation.ToList(i => i.memberId == primaryMember.Id).Select(i => i.relMemberId).ToList();

                string relationType = string.Empty;
                var dataMRelation = tblMRelation.ToList(i => i.DeleteDate == null);

                if (!string.IsNullOrEmpty(relationType))
                {
                    var relationTemp = dataMRelation.FirstOrDefault(x => x.Id == relationType).MainRelation;
                    if (relationTemp.Contains(GPConst.Relation.PREFIX_CHILD))
                    {
                        relationType = AppConst.NameDefaul.Child;
                    }
                }

                //var dtaTMember = tblTMember.ToList(
                //    i => ((intGender < 0 || i.Gender == intGender)
                //             && i.Id != objMainMemberParam.Id
                //             && !lstRelMemberRelation.Contains(i.Id)
                //             && (string.IsNullOrEmpty(keyword) || i.Name.ToLower().Contains(keyword))
                //             &&
                //             (
                //              string.IsNullOrEmpty(relationType)
                //              || (relationType.Equals(AppConst.NameDefaul.Child) && (i.ListPARENT == null || i.ListPARENT.Count == 0))
                //             )
                //          )
                List<ExTMember> dtaTMember = new List<ExTMember>();

                if (addListMember)
                {
                    if (typeMember.Equals(AppConst.NameDefaul.Child))
                    {
                        var lstRelNewMember = tblTMemberRelation.ToList(i => i.memberId == newMember.Id).Select(i => i.relMemberId).ToList();

                        dtaTMember = tblTMember.AsEnumerable().Where(mem => primaryMember.ListSPOUSE.Contains(mem.Id) && !lstRelNewMember.Contains(mem.Id) && (string.IsNullOrEmpty(keyword) || mem.Name.ToLower().Contains(keyword)))
                                                              .Select(i => new ExTMember()
                                                              {
                                                                  Id = i.Id,
                                                                  Name = i.Name + "",
                                                                  Gender = i.Gender,
                                                                  IsDeath = i.IsDeath,
                                                                  GenderShow = (i.Gender == (int)EmGender.Male) ? "Nam" : ((i.Gender == (int)EmGender.FeMale) ? "Nữ" : "Chưa rõ"),
                                                                  BirthdayShow = i.Birthday != null ? i.Birthday.ToDateSun() : "",
                                                                  Tel_1 = i.Contact.Tel_1 + "",
                                                                  Tel_2 = i.Contact.Tel_2 + "",
                                                                  Email_1 = i.Contact.Email_1 + "",
                                                                  Email_2 = i.Contact.Email_2 + "",
                                                                  Address = i.Contact.Address + "",
                                                                  Relation = i.Relation
                                                              }).ToList();
                    }
                    else if (typeMember.Equals(AppConst.NameDefaul.Father) || typeMember.Equals(AppConst.NameDefaul.Mother))
                    {
                        dtaTMember = tblTMember.AsEnumerable().Where(mem => newMember.ListSPOUSE.Contains(mem.Id) && !lstRelMemberRelation.Contains(mem.Id) && (string.IsNullOrEmpty(keyword) || mem.Name.ToLower().Contains(keyword)))
                                                              .Select(i => new ExTMember()
                                                              {
                                                                  Id = i.Id,
                                                                  Name = i.Name + "",
                                                                  Gender = i.Gender,
                                                                  IsDeath = i.IsDeath,
                                                                  GenderShow = (i.Gender == (int)EmGender.Male) ? "Nam" : ((i.Gender == (int)EmGender.FeMale) ? "Nữ" : "Chưa rõ"),
                                                                  BirthdayShow = i.Birthday != null ? i.Birthday.ToDateSun() : "",
                                                                  Tel_1 = i.Contact.Tel_1 + "",
                                                                  Tel_2 = i.Contact.Tel_2 + "",
                                                                  Email_1 = i.Contact.Email_1 + "",
                                                                  Email_2 = i.Contact.Email_2 + "",
                                                                  Address = i.Contact.Address + "",
                                                                  Relation = i.Relation
                                                              }).ToList();
                    }
                }
                else
                {
                    dtaTMember = tblTMember.AsEnumerable()
                                           .Where(
                       mem => ((intGender < 0 || mem.Gender == intGender)
                                && mem.Id != primaryMember.Id
                                && !lstRelMemberRelation.Contains(mem.Id)
                                && (string.IsNullOrEmpty(keyword) || mem.Name.ToLower().Contains(keyword))
                                && ((!string.IsNullOrEmpty(primaryMember.RootID) && CheckMember(primaryMember, mem)) ||
                                    (string.IsNullOrEmpty(primaryMember.RootID) && !allTMemberRelation.Contains(mem.Id)))
                             )
                   ).Select(i => new ExTMember()
                   {
                       Id = i.Id,
                       Name = i.Name + "",
                       Gender = i.Gender,
                       IsDeath = i.IsDeath,
                       GenderShow = (i.Gender == (int)EmGender.Male) ? "Nam" : ((i.Gender == (int)EmGender.FeMale) ? "Nữ" : "Chưa rõ"),
                       BirthdayShow = i.Birthday != null ? i.Birthday.ToDateSun() : "",
                       Tel_1 = i.Contact.Tel_1 + "",
                       Tel_2 = i.Contact.Tel_2 + "",
                       Email_1 = i.Contact.Email_1 + "",
                       Email_2 = i.Contact.Email_2 + "",
                       Address = i.Contact.Address + "",
                       Relation = i.Relation
                   }).ToList();
                }
                BindingHelper.BindingDataGrid(dgvListMember, dtaTMember);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(AddMemberRelationFromList), ex);
            }
        }

        private bool CheckMember(TMember mainMember, TMember member)
        {
            //khai báo bảng thành viên
            var tblTMember = AppManager.DBManager.GetTable<TMember>();
            //khai báo bảng danh sách quan hệ
            var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
            //khai báo bảng quan hệ thành viên
            var tblTMemberRealtion = AppManager.DBManager.GetTable<TMemberRelation>();
            //nếu thành viên đang được yêu cầu thêm quan hệ là chồng hoặc vợ thì sẽ chọn những thành viên không cùng trong 1 cây với thành viên chính
            if (typeMember.Equals(AppConst.NameDefaul.Husban) || typeMember.Equals(AppConst.NameDefaul.Wife))
            {
                return mainMember.RootID != member.RootID;
            }
            //nếu thành viên đang được yêu cầu thêm quan hệ là con thì sẽ chọn những thành viên không cùng trong 1 cây và những người con của vợ/ chồng thành viên chính mà không phải con của thành viên chính
            else if (typeMember.Equals(AppConst.NameDefaul.Child))
            {
                var lstSpouse = mainMember.ListSPOUSE;
                List<string> lstAllChild = new List<string>();
                lstSpouse.ForEach(memID =>
                {
                    var mem = memberHelper.GetTMemberByID(memID);
                    lstAllChild.AddRange(mem.ListCHILDREN);
                });
                var lstChildTemp = lstAllChild.Where(x => !mainMember.ListCHILDREN.Contains(x));
                return (mainMember.RootID != member.RootID) || lstChildTemp.Contains(member.Id);
            }
            //nếu thành viên đang được yêu cầu thêm quan hệ là cha/mẹ thì sẽ chọn những thành viên không cùng trong 1 cây với thành viên chính và những thành viên là vợ/ chồng của cha/ mẹ khi thành viên chỉ có mẹ hoặc cha.
            else
            {
                var lstParent = mainMember.ListPARENT;
                if (lstParent.Count == 0) return mainMember.RootID != member.RootID;

                var mRelation = tblMRelation.AsEnumerable().FirstOrDefault(x => x.Id == relationType);

                var lstFather = (from tabTMem in tblTMember.AsEnumerable()
                                 join tabTMemRel in tblTMemberRealtion.AsEnumerable()
                                 on tabTMem.Id equals tabTMemRel.relMemberId
                                 join tabMRel in tblMRelation.AsEnumerable()
                                 on tabTMemRel.relType equals tabMRel.MainRelation
                                 where tabTMemRel.memberId == mainMember.Id
                                       && tabTMemRel.relType.Contains(Relation.PREFIX_DAD)
                                       && tabMRel.IsMain == mRelation.IsMain
                                       && tabTMemRel.DeleteDate == null
                                 select tabTMem).ToList();

                var lstMother = (from tabTMem in tblTMember.AsEnumerable()
                                 join tabTMemRel in tblTMemberRealtion.AsEnumerable()
                                 on tabTMem.Id equals tabTMemRel.relMemberId
                                 join tabMRel in tblMRelation.AsEnumerable()
                                 on tabTMemRel.relType equals tabMRel.MainRelation
                                 where tabTMemRel.memberId == mainMember.Id
                                       && tabTMemRel.relType.Contains(Relation.PREFIX_MOM)
                                       && tabMRel.IsMain == mRelation.IsMain
                                       && tabTMemRel.DeleteDate == null
                                 select tabTMem).ToList();

                if (typeMember.Equals(AppConst.NameDefaul.Mother))
                {
                    if (lstMother.Count > 0) return mainMember.RootID != member.RootID;

                    foreach (var mem in lstFather)
                    {
                        if (mem.ListSPOUSE.Contains(member.Id) && !lstParent.Contains(member.Id))
                        {
                            return true;
                        }
                    };
                }
                else
                {
                    if (lstFather.Count > 0) return mainMember.RootID != member.RootID;

                    foreach (var mem in lstMother)
                    {
                        if (mem.ListSPOUSE.Contains(member.Id) && !lstParent.Contains(member.Id))
                        {
                            return true;
                        }
                    };
                }
                return false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var strKeyword = txtKeyword.Text.Trim();
                //var genderSelected = cmbGender.SelectedValue.ToString();
                LoadListTMember(strKeyword);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(AddMemberRelationFromList), ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvListMember.SelectedRows.Count > 0)
                {
                    var selectedRows = dgvListMember.SelectedRows[0].DataBoundItem as TMember;

                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        return;
                    }

                    var tblTMember = AppManager.DBManager.GetTable<TMember>();
                    var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                    var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
                    // main member ~ params
                    TMember mainMember = new TMember();
                    if (addListMember && newMember != null && primaryMember != null)
                    {
                        if (typeMember.Equals(AppConst.NameDefaul.Child))
                        {
                            mainMember = newMember;

                            MRelation mRel = (from tabTMemberRelation in tblTMemberRelation.AsEnumerable()
                                              join tabMRelation in tblMRelation.AsEnumerable()
                                              on tabTMemberRelation.relType equals tabMRelation.MainRelation
                                              where tabTMemberRelation.memberId == newMember.Id
                                                    && tabTMemberRelation.relMemberId == primaryMember.Id
                                                    && tabTMemberRelation.DeleteDate == null
                                              select tabMRelation).FirstOrDefault();

                            if (primaryMember.Gender == (int)EmGender.Male)
                            {
                                relationType = tblMRelation.AsEnumerable().FirstOrDefault(x => x.MainRelation.Contains(Relation.PREFIX_MOM) && x.IsMain == mRel.IsMain)?.Id ?? "";
                            }
                            else if (primaryMember.Gender == (int)EmGender.FeMale)
                            {
                                relationType = tblMRelation.AsEnumerable().FirstOrDefault(x => x.MainRelation.Contains(Relation.PREFIX_DAD) && x.IsMain == mRel.IsMain)?.Id ?? "";
                            }
                        }
                        else if (typeMember.Equals(AppConst.NameDefaul.Father) || typeMember.Equals(AppConst.NameDefaul.Mother))
                        {
                            mainMember = primaryMember;

                            MRelation mRel = (from tabTMemberRelation in tblTMemberRelation.AsEnumerable()
                                              join tabMRelation in tblMRelation.AsEnumerable()
                                              on tabTMemberRelation.relType equals tabMRelation.MainRelation
                                              where tabTMemberRelation.memberId == primaryMember.Id && tabTMemberRelation.relMemberId == newMember.Id
                                              select tabMRelation).FirstOrDefault();

                            if (newMember.Gender == (int)EmGender.Male)
                            {
                                relationType = tblMRelation.AsEnumerable().FirstOrDefault(x => x.MainRelation.Contains(Relation.PREFIX_MOM) && x.IsMain == mRel.IsMain)?.Id ?? "";
                            }
                            else if (newMember.Gender == (int)EmGender.FeMale)
                            {
                                relationType = tblMRelation.AsEnumerable().FirstOrDefault(x => x.MainRelation.Contains(Relation.PREFIX_DAD) && x.IsMain == mRel.IsMain)?.Id ?? "";
                            }
                        }
                    }
                    else
                    {
                        mainMember = memberHelper.GetTMemberByID(primaryMember.Id);
                    }

                    int _levelInFamily = mainMember.LevelInFamily;
                    // related member ~ seleted row
                    var relatedMember = tblTMember.FirstOrDefault(i => i.Id == selectedRows.Id);
                    var mRelation = AppManager.DBManager.GetTable<MRelation>().FirstOrDefault(i => i.Id == relationType.ToString());
                    if (mRelation.MainRelation.Contains(GPConst.Relation.PREFIX_CHILD))
                    {
                        _levelInFamily++;
                    }
                    relatedMember.LevelInFamily = _levelInFamily;
                    tblTMember.UpdateOne(m => m.Id == relatedMember.Id, relatedMember);
                    memberRelation.addMemberRelation(relationType, mainMember, relatedMember);

                    //if (spouseMember != null && relationType2 != null)
                    //{
                    //    addMemberRelation(relationType2, spouseMember, relatedMember);
                    //}
                    NavigationResult naviResult = new NavigationResult() { Result = DialogResult.OK };
                    naviResult.Add("newmember", relatedMember);

                    this.Close(naviResult);
                }
                else
                {
                    AppManager.Dialog.Warning("Vui lòng chọn thành viên!");
                    return;
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(AddMemberRelationFromList), ex);
            }
        }

        //public void addMemberRelation(string idTypeRelation, TMember objMainMember, TMember objRelatedMember)
        //{
        //    if (objMainMember.ListCHILDREN.Contains(objRelatedMember.Id) || objMainMember.ListPARENT.Contains(objRelatedMember.Id) || objMainMember.ListSPOUSE.Contains(objRelatedMember.Id))
        //        return;

        //    try
        //    {
        //        var tblTMember = AppManager.DBManager.GetTable<TMember>();
        //        var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
        //        var objMemberRelation = new TMemberRelation();
        //        var objMemberRelatedRelation = new TMemberRelation();

        //        var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
        //        var objRelationType = tblMRelation.FirstOrDefault(i => i.Id == idTypeRelation.ToString());

        //        if (objRelationType == null)
        //        {
        //            return;
        //        }

        //        var objNewMainMember = tblTMember.FirstOrDefault(i => i.Id == objMainMember.Id);
        //        if (objNewMainMember == null)
        //        {
        //            return;
        //        }
        //        // Main member
        //        var objUserRelation = tblTMemberRelation.ToList(i => i.memberId == objMainMember.Id && i.relType == objRelationType.MainRelation);
        //        var countObjUserRelation = objUserRelation.Count;

        //        objMemberRelation.memberId = objNewMainMember.Id;
        //        objMemberRelation.relMemberId = objRelatedMember.Id;
        //        objMemberRelation.relType = objRelationType.MainRelation;
        //        objMemberRelation.roleOrder = (countObjUserRelation > 0) ? countObjUserRelation += 1 : 1;

        //        if (!tblTMemberRelation.InsertOne(objMemberRelation))
        //        {
        //            AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
        //            return;
        //        }

        //        // insert relation of tmember
        //        if ((objRelationType.MainRelation.Contains(Relation.PREFIX_DAD) || objRelationType.MainRelation.Contains(Relation.PREFIX_MOM)) && !objNewMainMember.ListPARENT.Contains(objMemberRelation.relMemberId))
        //        {
        //            objNewMainMember.ListPARENT.Add(objMemberRelation.relMemberId);
        //        }
        //        else if ((objRelationType.MainRelation.Contains(Relation.PREFIX_HUSBAND) || objRelationType.MainRelation.Contains(Relation.PREFIX_WIFE)) && !objNewMainMember.ListSPOUSE.Contains(objMemberRelation.relMemberId))
        //        {
        //            objNewMainMember.ListSPOUSE.Add(objMemberRelation.relMemberId);
        //        }
        //        else if ((objRelationType.MainRelation.Contains(Relation.PREFIX_CHILD)) && !objNewMainMember.ListCHILDREN.Contains(objMemberRelation.relMemberId))
        //        {
        //            objNewMainMember.ListCHILDREN.Add(objMemberRelation.relMemberId);
        //        }

        //        objNewMainMember.Relation.Add(objMemberRelation);
        //        // update data relation for main member
        //        if (!tblTMember.UpdateOne(i => i.Id == objNewMainMember.Id, objNewMainMember))
        //        {
        //            AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
        //            return;
        //        }

        //        // Relate member
        //        var objUserRelatedRelation = tblTMemberRelation.ToList(i => i.memberId == objRelatedMember.Id && i.relType == objRelationType.RelatedRelation);
        //        var countObjUseRelatedrRelation = objUserRelatedRelation.Count;

        //        objMemberRelatedRelation.memberId = objRelatedMember.Id;
        //        objMemberRelatedRelation.relMemberId = objNewMainMember.Id;
        //        objMemberRelatedRelation.relType = objRelationType.RelatedRelation;
        //        objMemberRelatedRelation.roleOrder = (countObjUseRelatedrRelation > 0) ? countObjUseRelatedrRelation += 1 : 1;

        //        if (!tblTMemberRelation.InsertOne(objMemberRelatedRelation))
        //        {
        //            AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
        //            return;
        //        }

        //        // insert relation of tmember
        //        if (objRelationType.RelatedRelation.Contains(Relation.PREFIX_DAD) || objRelationType.RelatedRelation.Contains(Relation.PREFIX_MOM))
        //        {
        //            objRelatedMember.ListPARENT.Add(objMemberRelatedRelation.relMemberId);
        //        }
        //        else if (objRelationType.RelatedRelation.Contains(Relation.PREFIX_HUSBAND) || objRelationType.RelatedRelation.Contains(Relation.PREFIX_WIFE))
        //        {
        //            objRelatedMember.ListSPOUSE.Add(objMemberRelatedRelation.relMemberId);
        //        }
        //        else if (objRelationType.RelatedRelation.Contains(Relation.PREFIX_CHILD))
        //        {
        //            objRelatedMember.ListCHILDREN.Add(objMemberRelatedRelation.relMemberId);
        //        }
        //        objRelatedMember.Relation.Add(objMemberRelatedRelation);
        //        // update data relation for main member
        //        if (!tblTMember.UpdateOne(i => i.Id == objRelatedMember.Id, objRelatedMember))
        //        {
        //            AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AppManager.Dialog.Error(ex.Message);
        //        AppManager.LoggerApp.Error(typeof(AddMemberRelationFromList), ex);
        //    }
        //}
    }
}