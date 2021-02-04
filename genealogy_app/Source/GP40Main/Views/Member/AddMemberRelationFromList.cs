using System;
using System.Collections.Generic;
using System.Linq;
using GP40Main.Core;
using GP40Main.Models;
using GP40Main.Services.Navigation;
using GP40Main.Utility;
using static GP40Main.Core.AppConst;

namespace GP40Main.Views.Member
{
    public partial class AddMemberRelationFromList : BaseUserControl
    {
        public AddMemberRelationFromList(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            LoadListTMember();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void LoadListTMember(string keyword = "", string gender = "")
        {
            try {
                var intGender = ConvertHelper.CnvNullToInt(gender);
                var objMainMemberParam = this.GetParameters().GetValue<TMember>("primary_member");
                keyword = keyword.ToLower();

                var tblTMember = AppManager.DBManager.GetTable<TMember>();
                var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();

                var lstRelMemberRelation = tblTMemberRelation.CreateQuery(i => i.memberId == objMainMemberParam.Id).Select(i => new
                {
                    i.relMemberId
                }).ToList();

                var exceptionList = new List<string> { };
                if (lstRelMemberRelation != null)
                {
                    foreach (var item in lstRelMemberRelation)
                    {
                        exceptionList.Add(item.relMemberId);
                    }
                }

                var dtaTMember = tblTMember.CreateQuery(
                        i => (intGender < 0 || i.Gender == intGender) // gender
                            && i.Id != objMainMemberParam.Id
                            && !exceptionList.Contains(i.Id)
                            && (string.IsNullOrEmpty(keyword) || i.Name.ToLower().Contains(keyword)) // key
                    ).ToList().Select(i => new ExTMember()
                    {
                        Id = i.Id,
                        Name = i.Name + "",
                        Gender = i.Gender,
                        IsDeath = i.IsDeath,
                        GenderShow = (i.Gender == (int)GenderMember.Male) ? "Nam" : ((i.Gender == (int)GenderMember.Female) ? "Nữ" : "Chưa rõ"),
                        BirthdayShow = i.Birthday != null ? i.Birthday.ToDateSun() : "",
                        Tel_1 = i.Contact.Tel_1 + "",
                        Tel_2 = i.Contact.Tel_2 + "",
                        Email_1 = i.Contact.Email_1 + "",
                        Email_2 = i.Contact.Email_2 + "",
                        Address = i.Contact.Address + "",
                        Relation = i.Relation
                    }).ToList();

                BindingHelper.BindingDataGrid(dgvListMember, dtaTMember);
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(AddMemberRelationFromList), ex);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try {
                var strKeyword = txtKeyword.Text.Trim();
                //var genderSelected = cmbGender.SelectedValue.ToString();
                var genderSelected = "";

                LoadListTMember(strKeyword, genderSelected);
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(AddMemberRelationFromList), ex);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try {
                if (dgvListMember.SelectedRows.Count > 0)
                {
                    var selectedRows = dgvListMember.SelectedRows[0].DataBoundItem as TMember;

                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        return;
                    }
                    var primaryMember = this.GetParameters().GetValue<TMember>("primary_member");
                    var relationType = this.GetParameters().GetValue<string>("relation_type");

                    var tblTMember = AppManager.DBManager.GetTable<TMember>();

                    // main member ~ params
                    var mainMember = tblTMember.CreateQuery(i => i.Id == primaryMember.Id).FirstOrDefault();

                    // related member ~ seleted row
                    var relatedMember = tblTMember.CreateQuery(i => i.Id == selectedRows.Id).FirstOrDefault();

                    addMemberRelation(relationType, mainMember, relatedMember);
                    this.Close();
                }
                else
                {
                    AppManager.Dialog.Warning("Vui lòng chọn thành viên!");
                    return;
                }
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(AddMemberRelationFromList), ex);
            }
        }

        public void addMemberRelation(string idTypeRelation, TMember objMainMember, TMember objRelatedMember)
        {
            try {
                var tblTMember = AppManager.DBManager.GetTable<TMember>();
                var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
                var objMemberRelation = new TMemberRelation();
                var objMemberRelatedRelation = new TMemberRelation();

                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                var objRelationType = tblMRelation.CreateQuery(i => i.Id == idTypeRelation.ToString()).FirstOrDefault();

                if (objRelationType == null)
                {
                    return;
                }

                var objNewMainMember = tblTMember.CreateQuery(i => i.Id == objMainMember.Id).FirstOrDefault();

                // Main member
                var objUserRelation = tblTMemberRelation.CreateQuery().Where(i => i.memberId == objMainMember.Id && i.relType == objRelationType.MainRelation);
                var countObjUserRelation = objUserRelation.Count();

                objMemberRelation.memberId = objNewMainMember.Id;
                objMemberRelation.relMemberId = objRelatedMember.Id;
                objMemberRelation.relType = objRelationType.MainRelation;
                objMemberRelation.roleOrder = (countObjUserRelation > 0) ? countObjUserRelation += 1 : 1;

                if (!tblTMemberRelation.InsertOne(objMemberRelation))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }

                // insert relation of tmember
                if (objRelationType.MainRelation.Contains(cstrPreFixDAD) || objRelationType.MainRelation.Contains(cstrPreFixMOM))
                {
                    objNewMainMember.ListPARENT.Add(objMemberRelation.relMemberId);
                }
                else if (objRelationType.MainRelation.Contains(cstrPreFixHUSBAND) || objRelationType.MainRelation.Contains(cstrPreFixWIFE))
                {
                    objNewMainMember.ListSPOUSE.Add(objMemberRelation.relMemberId);
                }
                else if (objRelationType.MainRelation.Contains(cstrPreFixCHILD))
                {
                    objNewMainMember.ListCHILDREN.Add(objMemberRelation.relMemberId);
                }
                objNewMainMember.Relation.Add(objMemberRelation);
                // update data relation for main member
                if (!tblTMember.UpdateOne(i => i.Id == objNewMainMember.Id, objNewMainMember))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }

                // Relate member
                var objUserRelatedRelation = tblTMemberRelation.CreateQuery().Where(i => i.memberId == objRelatedMember.Id && i.relType == objRelationType.RelatedRelation);
                var countObjUseRelatedrRelation = objUserRelatedRelation.Count();

                objMemberRelatedRelation.memberId = objRelatedMember.Id;
                objMemberRelatedRelation.relMemberId = objNewMainMember.Id;
                objMemberRelatedRelation.relType = objRelationType.RelatedRelation;
                objMemberRelatedRelation.roleOrder = (countObjUseRelatedrRelation > 0) ? countObjUseRelatedrRelation += 1 : 1;

                if (!tblTMemberRelation.InsertOne(objMemberRelatedRelation))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }

                // insert relation of tmember
                if (objRelationType.RelatedRelation.Contains(cstrPreFixDAD) || objRelationType.RelatedRelation.Contains(cstrPreFixMOM))
                {
                    objRelatedMember.ListPARENT.Add(objMemberRelatedRelation.relMemberId);
                }
                else if (objRelationType.RelatedRelation.Contains(cstrPreFixHUSBAND) || objRelationType.RelatedRelation.Contains(cstrPreFixWIFE))
                {
                    objRelatedMember.ListSPOUSE.Add(objMemberRelatedRelation.relMemberId);
                }
                else if (objRelationType.RelatedRelation.Contains(cstrPreFixCHILD))
                {
                    objRelatedMember.ListCHILDREN.Add(objMemberRelatedRelation.relMemberId);
                }
                objRelatedMember.Relation.Add(objMemberRelatedRelation);
                // update data relation for main member
                if (!tblTMember.UpdateOne(i => i.Id == objRelatedMember.Id, objRelatedMember))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }
            } catch(Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(AddMemberRelationFromList), ex);
            }
        }
    }
}
