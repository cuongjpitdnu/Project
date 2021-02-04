using GPConst;
using GPMain.Views.Member;
using GPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMain.Common.Helper
{
    /// <summary>
    /// Meno: Add, remove relation of member
    /// Create by: Nguyễn Văn Hải
    /// </summary>
    public class MemberRelationHelper : IDisposable
    {
        MemberHelper memberHelper = new MemberHelper();
        #region Quan hệ của thành viên
        //Thêm quan hệ thành viên
        public void addMemberRelation(string idTypeRelation, TMember objMainMember, TMember objRelatedMember)
        {
            if (objMainMember.ListCHILDREN.Contains(objRelatedMember.Id) || objMainMember.ListPARENT.Contains(objRelatedMember.Id) || objMainMember.ListSPOUSE.Contains(objRelatedMember.Id))
                return;

            try
            {
                var tblTMember = AppManager.DBManager.GetTable<TMember>();
                var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
                var objMemberRelation = new TMemberRelation();
                var objMemberRelatedRelation = new TMemberRelation();

                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                var objRelationType = tblMRelation.FirstOrDefault(i => i.Id == idTypeRelation.ToString());

                if (objRelationType == null)
                {
                    return;
                }

                var objNewMainMember = tblTMember.FirstOrDefault(i => i.Id == objMainMember.Id);
                if (objNewMainMember == null)
                {
                    return;
                }
                // Main member
                var objUserRelation = tblTMemberRelation.ToList(i => i.memberId == objMainMember.Id && i.relType == objRelationType.MainRelation);
                var countObjUserRelation = objUserRelation.Count;

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
                if ((objRelationType.MainRelation.Contains(Relation.PREFIX_DAD) || objRelationType.MainRelation.Contains(Relation.PREFIX_MOM)) && !objNewMainMember.ListPARENT.Contains(objMemberRelation.relMemberId))
                {
                    objNewMainMember.ListPARENT.Add(objMemberRelation.relMemberId);
                }
                else if ((objRelationType.MainRelation.Contains(Relation.PREFIX_HUSBAND) || objRelationType.MainRelation.Contains(Relation.PREFIX_WIFE)) && !objNewMainMember.ListSPOUSE.Contains(objMemberRelation.relMemberId))
                {
                    objNewMainMember.ListSPOUSE.Add(objMemberRelation.relMemberId);
                }
                else if ((objRelationType.MainRelation.Contains(Relation.PREFIX_CHILD)) && !objNewMainMember.ListCHILDREN.Contains(objMemberRelation.relMemberId))
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
                var objUserRelatedRelation = tblTMemberRelation.ToList(i => i.memberId == objRelatedMember.Id && i.relType == objRelationType.RelatedRelation);
                var countObjUseRelatedrRelation = objUserRelatedRelation.Count;

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
                if (objRelationType.RelatedRelation.Contains(Relation.PREFIX_DAD) || objRelationType.RelatedRelation.Contains(Relation.PREFIX_MOM))
                {
                    objRelatedMember.ListPARENT.Add(objMemberRelatedRelation.relMemberId);
                }
                else if (objRelationType.RelatedRelation.Contains(Relation.PREFIX_HUSBAND) || objRelationType.RelatedRelation.Contains(Relation.PREFIX_WIFE))
                {
                    objRelatedMember.ListSPOUSE.Add(objMemberRelatedRelation.relMemberId);
                }
                else if (objRelationType.RelatedRelation.Contains(Relation.PREFIX_CHILD))
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
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(AddMemberRelationFromList), ex);
            }
        }
        //Lấy ID quan hệ
        public string GetRelationID(TMember mainMember, TMember relateMember, string relate)
        {
            var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
            var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();

            var mRelation = (from tabMRel in tblMRelation.AsEnumerable()
                             join tabTMemberRel in tblTMemberRelation.AsEnumerable()
                             on tabMRel.MainRelation equals tabTMemberRel.relType
                             where tabTMemberRel.memberId == mainMember.Id && tabTMemberRel.relMemberId == relateMember.Id && tabTMemberRel.DeleteDate == null
                             select tabMRel).FirstOrDefault();

            if (mRelation == null) return "";
            return tblMRelation.AsEnumerable().FirstOrDefault(x => x.IsMain == mRelation.IsMain && x.MainRelation.Contains(relate))?.Id ?? "";
        }

        //Xóa quan hệ
        public void DeleteRelation(ref ExTMember mainMember, ExTMember relateMember)
        {
            var tblMember = AppManager.DBManager.GetTable<TMember>();
            var tblMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
            string relate = GetRelation(mainMember, relateMember);
            if (relate.Equals(Relation.PREFIX_CHILD))
            {
                mainMember.ListCHILDREN.Remove(relateMember.Id);
            }
            else if (relate.Equals(Relation.PREFIX_DAD) || relate.Equals(Relation.PREFIX_MOM))
            {
                mainMember.ListPARENT.Remove(relateMember.Id);
            }
            else if (relate.Equals(Relation.PREFIX_HUSBAND) || relate.Equals(Relation.PREFIX_WIFE))
            {
                mainMember.ListSPOUSE.Remove(relateMember.Id);
            }
            string mainMemberID = mainMember.Id;
            var idx = mainMember.Relation.FindIndex(m => m.memberId == mainMemberID && m.relMemberId == relateMember.Id);
            if (idx != -1)
            {
                mainMember.Relation.RemoveAt(idx);
            }

            var relationMember = tblMemberRelation.FirstOrDefault(m => m.memberId == mainMemberID && m.relMemberId == relateMember.Id);
            if (tblMemberRelation.FirstOrDefault(m => m.memberId == mainMemberID && m.relMemberId == relateMember.Id) != null)
            {
                try
                {
                    relationMember.DeleteDate = DateTime.Now;
                    tblMemberRelation.UpdateOne(m => m.memberId == mainMemberID && m.relMemberId == relateMember.Id, relationMember);
                }
                catch (Exception ex) { }
            }
        }

        //Xóa toàn bộ quan hệ của thành viên
        public bool DeleteAllRelation(ExTMember mainMember)
        {
            if (mainMember == null) return false;
            var tblMember = AppManager.DBManager.GetTable<TMember>();

            var listChild = mainMember.ListCHILDREN;
            listChild = listChild == null ? new List<string>() : listChild;
            while (listChild.Count > 0)
            {
                var relationMember = memberHelper.GetExTMemberByID(listChild[0]);
                if (relationMember != null)
                {
                    DeleteRelation(ref mainMember, relationMember);
                    DeleteRelation(ref relationMember, mainMember);
                    tblMember.UpdateOne(m => m.Id == relationMember.Id, relationMember);
                }
                if (listChild.Count == 0) break;
            }

            var listSpouse = mainMember.ListSPOUSE;
            listSpouse = listSpouse == null ? new List<string>() : listSpouse;
            while (listSpouse.Count > 0)
            {
                var relationMember = memberHelper.GetExTMemberByID(listSpouse[0]);
                if (relationMember != null)
                {
                    DeleteRelation(ref mainMember, relationMember);
                    DeleteRelation(ref relationMember, mainMember);
                    tblMember.UpdateOne(m => m.Id == relationMember.Id, relationMember);
                }
                if (listSpouse.Count == 0) break;
            }
            return tblMember.UpdateOne(x => x.Id == mainMember.Id, mainMember);
        }

        //Lấy quan hệ giữa 2 thành viên
        public string GetRelation(ExTMember mainMember, ExTMember relateMember)
        {
            string relation = string.Empty;
            if (mainMember.ListCHILDREN.Contains(relateMember.Id) && relateMember.Id != null)
            {
                return Relation.PREFIX_CHILD;
            }
            if (mainMember.ListPARENT.Contains(relateMember.Id) && relateMember.GenderShow.Equals(AppConst.Gender.Male) && relateMember.Id != null)
            {
                return Relation.PREFIX_DAD;
            }
            if (mainMember.ListPARENT.Contains(relateMember.Id) && relateMember.GenderShow.Equals(AppConst.Gender.Female) && relateMember.Id != null)
            {
                return Relation.PREFIX_MOM;
            }
            if (mainMember.ListSPOUSE.Contains(relateMember.Id) && relateMember.GenderShow.Equals(AppConst.Gender.Male) && relateMember.Id != null)
            {
                return Relation.PREFIX_HUSBAND;
            }
            if (mainMember.ListSPOUSE.Contains(relateMember.Id) && relateMember.GenderShow.Equals(AppConst.Gender.Female) && relateMember.Id != null)
            {
                return Relation.PREFIX_WIFE;
            }
            return relation;
        }
        #endregion
        #region Dispose Object
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MemberRelationHelper()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
