using GPCommon;
using GPConst;
using GPMain.Common;
using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using GPMain.Properties;
using GPMain.Views.Member;
using GPModels;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GPMain.Views.Controls
{
    public class ContextMenuStripManager
    {
        public const string cstrAddRootFamilyMember = "Đặt làm tổ phụ";
        public const string cstrRemoveRootFamilyMember = "Không phải tổ phụ";

        public const string cstrAddFamilyHead = "Đặt làm tộc trưởng";
        public const string cstrRemoveFamilyHead = "Không phải tộc trưởng";

        public const string cstrUpdateFamilyLevel = "Cập nhật lại thứ bậc";
        public const string cstrEditInfoMember = "Chỉnh sửa thông tin";
        public const string cstrShowCurrentRelation = "Xem quan hệ hiện tại";
        public const string cstrAddRelation = "Thêm quan hệ";
        public const string cstrDeleteMember = "Xóa";

        public static string TypeTree = string.Empty;
        MemberHelper memberHelper = new MemberHelper();
        public static ContextMenuStripData<TMember> CreateForMember()
        {
            MemberHelper memberHelper = new MemberHelper();
            var contextMember = new ContextMenuStripData<TMember>();
            // Action
            Action<TMember> actionAddRootMember = (member) =>
            {
                if (member == null) return;
                try
                {
                    if (AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        if (!memberHelper.AddRootMember(member.Id))
                        {
                            AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                            contextMember.Member = null;
                            return;
                        }
                        else
                        {
                            contextMember.Member = member;
                        }
                    }
                }
                catch (Exception ex)
                {
                    contextMember.Member = null;
                    AppManager.Dialog.Error(ex.Message);
                    AppManager.LoggerApp.Error(typeof(ContextMenuStripManager), ex);
                }
            };

            Action<TMember> actionRemoveRootMember = (member) =>
            {
                try
                {
                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        return;
                    }
                    if (member == null) return;



                    if (!memberHelper.RemoveRootMember(member.Id))
                    {
                        contextMember.Member = null;
                        AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                        return;
                    }
                    else
                    {
                        contextMember.Member = member;
                    }
                }
                catch (Exception ex)
                {
                    contextMember.Member = null;
                    AppManager.Dialog.Error(ex.Message);
                    AppManager.LoggerApp.Error(typeof(ContextMenuStripManager), ex);
                }
            };

            Action<TMember> actionAddFamilyHead = (member) =>
            {
                if (member == null) return;
                try
                {
                    if (AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
                        var objUserLogin = AppManager.LoginUser;
                        var objMFamilyInfo = tblMFamilyInfo.CreateQuery(i => i.Id == objUserLogin.FamilyId).FirstOrDefault();
                        // add new member to list family head
                        if (!objMFamilyInfo.ListFamilyHead.HasValue())
                        {
                            objMFamilyInfo.ListFamilyHead = new List<string>();
                        }
                        if (objMFamilyInfo.ListFamilyHead.Contains(member.Id))
                        {
                            AppManager.Dialog.Error("Thành viên đã có trong danh sách tộc trưởng!");
                            return;
                        }
                        objMFamilyInfo.ListFamilyHead.Add(member.Id);
                        objMFamilyInfo.CurrentFamilyHead = member.Id;
                        if (!tblMFamilyInfo.UpdateOne(i => i.Id == objMFamilyInfo.Id, objMFamilyInfo))
                        {
                            contextMember.Member = null;
                            AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                            return;
                        }
                        else
                        {
                            contextMember.Member = member;
                        }
                    }
                }
                catch (Exception ex)
                {
                    contextMember.Member = null;
                    AppManager.Dialog.Error(ex.Message);
                    AppManager.LoggerApp.Error(typeof(ContextMenuStripManager), ex);
                }
            };

            Action<TMember> actionRemoveMemberFamilyHead = (member) =>
            {
                try
                {
                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        return;
                    }
                    if (member == null) return;
                    var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();

                    var objUserLogin = AppManager.LoginUser;
                    var objMFamilyInfo = tblMFamilyInfo.CreateQuery(i => i.Id == objUserLogin.FamilyId).FirstOrDefault();
                    // remove member to list family head
                    if (objMFamilyInfo.ListFamilyHead.Contains(member.Id))
                    {
                        objMFamilyInfo.ListFamilyHead.Remove(member.Id);
                        // update new current family head
                        objMFamilyInfo.CurrentFamilyHead = (objMFamilyInfo.ListFamilyHead.HasValue()) ? objMFamilyInfo.ListFamilyHead[objMFamilyInfo.ListFamilyHead.Count - 1] : "";
                    }
                    if (!tblMFamilyInfo.UpdateOne(i => i.Id == objMFamilyInfo.Id, objMFamilyInfo))
                    {
                        contextMember.Member = null;
                        AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                        return;
                    }
                    else
                    {
                        contextMember.Member = member;
                    }
                }
                catch (Exception ex)
                {
                    contextMember.Member = null;
                    AppManager.Dialog.Error(ex.Message);
                    AppManager.LoggerApp.Error(typeof(ContextMenuStripManager), ex);
                }
            };

            // Fill menu
            contextMember.AddItem(cstrEditInfoMember, (member) =>
                {
                    if (member == null) return;
                    NavigationParameters param = new NavigationParameters();
                    param.Add("Default", member);
                    param.Add("TypeTree", TypeTree);

                    if (AppManager.Navigation.ShowDialogWithParam<addMember>(param, ModeForm.Edit, AppConst.StatusBarColor).Result == DialogResult.OK)
                    {
                        contextMember.Member = member;
                    }
                    else
                    {
                        contextMember.Member = null;
                    }
                    TypeTree = string.Empty;
                }, Resources.userinfo);

            contextMember.AddItem(cstrUpdateFamilyLevel, (member) =>
            {
                if (memberHelper.RootTMember == null)
                {
                    AppManager.Dialog.Error("Chưa đặt thành viên nào là tổ phụ.\n Hãy đặt tổ phụ trước khi cập nhật thứ bậc.");
                    return;
                }
                AppManager.Dialog.ShowProgressBar(progressBar =>
                {
                    memberHelper.UpdateLevelInFamily(progressBar);
                }, "Đang cập nhật thứ bậc...", $"{AppConst.TitleBarFisrt}Cập nhật lại thứ bậc");
                AppManager.Dialog.Ok("Cập nhật thứ bậc kết thúc!");
                contextMember.Member = member;
            }, Resources.refresh);

            var idItemRootFamilyMember = contextMember.AddItem(cstrAddRootFamilyMember, actionAddRootMember, Resources.starplus);
            var idItemFamilyHead = contextMember.AddItem(cstrAddFamilyHead, actionAddFamilyHead, Resources.medal_plus);

            contextMember.AddItem(cstrShowCurrentRelation, (member) =>
                    {
                        if (member == null) return;
                        DialogResult dialogResult = AppManager.Navigation.ShowDialogWithParam<ListMemberRelation, TMember>(member, ModeForm.None, AppConst.StatusBarColor).Result;
                        if (dialogResult == DialogResult.OK)
                        {
                            contextMember.Member = member;
                        }
                        else
                        {
                            contextMember.Member = null;
                        }
                    }, Resources.infoRelate);

            contextMember.AddItem(cstrAddRelation, (member) =>
            {
                if (member == null) return;
                var newParams = new NavigationParameters();
                string gender = member.Gender == 0 ? AppConst.Gender.Male : (member.Gender == 1 ? AppConst.Gender.Female : (member.Gender == 2 ? AppConst.Gender.Unknow : AppConst.Gender.Male));
                newParams.Add("primary_member", member);
                newParams.Add("gender", gender);
                var naviResult = AppManager.Navigation.ShowDialogWithParam<popupAddRelation>(newParams, ModeForm.New, AppConst.StatusBarColor);
                if (naviResult.Result == DialogResult.OK)
                {
                    contextMember.Member = member;
                }
                else
                {
                    contextMember.Member = null;
                }
            }, Resources.addrelate);

            contextMember.AddItem(cstrDeleteMember, (objMember) =>
            {
                if (objMember == null) return;
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }
                var tblTMember = AppManager.DBManager.GetTable<TMember>();

                // set date delete
                objMember.DeleteDate = DateTime.Now;

                // check related member relation to delete (TMemberRelation)
                var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
                // find all in TMemberRelation
                var listMemberRelation = tblTMemberRelation.ToList(i => i.memberId == objMember.Id || i.relMemberId == objMember.Id);
                if (listMemberRelation != null)
                {
                    foreach (var member in listMemberRelation)
                    {
                        member.DeleteDate = DateTime.Now;

                        // find member in TMember related with member want to delete -> remove listPARENT/SPOUSE/CHILDREN or props Relation
                        var objMemberRelated = tblTMember.FirstOrDefault(i => i.Id == ((objMember.Id == member.memberId) ? member.relMemberId : member.memberId));

                        if (objMemberRelated != null)
                        {
                            // listPARENT - SPOUSE - CHILD
                            if (member.relType.Contains(Relation.PREFIX_DAD) || member.relType.Contains(Relation.PREFIX_MOM))
                            {
                                objMemberRelated.ListPARENT.Remove(objMember.Id);
                            }
                            else if (member.relType.Contains(Relation.PREFIX_HUSBAND) || member.relType.Contains(Relation.PREFIX_WIFE))
                            {
                                objMemberRelated.ListSPOUSE.Remove(objMember.Id);
                            }
                            else if (member.relType.Contains(Relation.PREFIX_CHILD))
                            {
                                objMemberRelated.ListCHILDREN.Remove(objMember.Id);
                            }
                            var findInRelation = objMemberRelated.Relation.FindAll(i => i.memberId == member.memberId && i.relMemberId != member.relMemberId);
                            objMemberRelated.Relation = findInRelation;

                            if (!tblTMember.UpdateOne(i => i.Id == objMemberRelated.Id, objMemberRelated))
                            {
                                AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                                return;
                            }
                        }

                        if (!tblTMemberRelation.UpdateOne(i => i.Id == member.Id, member))
                        {
                            AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                            return;
                        }
                    }
                }

                if (!tblTMember.UpdateOne(i => i.Id == objMember.Id, objMember))
                {
                    AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                    return;
                }

                // remove member if exist in list family head
                var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
                var objMFamilyInfo = tblMFamilyInfo.FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);
                if (objMFamilyInfo != null && objMFamilyInfo.ListFamilyHead != null)
                {
                    if (objMFamilyInfo.ListFamilyHead.Contains(objMember.Id))
                    {
                        objMFamilyInfo.ListFamilyHead.Remove(objMember.Id);
                        // update new current family head
                        objMFamilyInfo.CurrentFamilyHead = (objMFamilyInfo.ListFamilyHead.HasValue()) ? objMFamilyInfo.ListFamilyHead[objMFamilyInfo.ListFamilyHead.Count - 1] : "";

                        if (!tblMFamilyInfo.UpdateOne(i => i.Id == objMFamilyInfo.Id, objMFamilyInfo))
                        {
                            AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                            return;
                        }
                    }
                }
                contextMember.Member = memberHelper.RootTMember;
            }, Resources.delete);

            contextMember.ChangeData += (sender, member) =>
            {
                var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
                var objUserLogin = AppManager.LoginUser;
                var objMFamilyInfo = tblMFamilyInfo.FirstOrDefault(i => i.Id == objUserLogin.FamilyId);
                if (objMFamilyInfo == null)
                {
                    objMFamilyInfo = new MFamilyInfo();
                }

                contextMember.Member = member;

                if (string.IsNullOrEmpty(objMFamilyInfo.RootId))
                {
                    contextMember.UpdateItem(idItemRootFamilyMember, cstrAddRootFamilyMember, actionAddRootMember, Resources.starplus);
                }
                else
                {
                    // check isRootId of row selected
                    if (member.Id == objMFamilyInfo.RootId)
                    {
                        contextMember.UpdateItem(idItemRootFamilyMember, cstrRemoveRootFamilyMember, actionRemoveRootMember, Resources.starminus);
                    }
                    else
                    {
                        contextMember.UpdateItem(idItemRootFamilyMember, cstrAddRootFamilyMember, actionAddRootMember, Resources.starplus);
                    }
                }

                if (!objMFamilyInfo.ListFamilyHead.HasValue())
                {
                    contextMember.UpdateItem(idItemFamilyHead, cstrAddFamilyHead, actionAddFamilyHead, Resources.medal_plus);
                }
                else
                {
                    // in list
                    if (objMFamilyInfo.ListFamilyHead.Contains(member.Id))
                    {
                        contextMember.UpdateItem(idItemFamilyHead, cstrRemoveFamilyHead, actionRemoveMemberFamilyHead, Resources.medalminus);
                    }
                    else
                    {
                        contextMember.UpdateItem(idItemFamilyHead, cstrAddFamilyHead, actionAddFamilyHead, Resources.medal_plus);
                    }
                }
            };
            return contextMember;
        }
    }

    public class ContextMenuStripData<T> : IDisposable
    {
        private MaterialContextMenuStrip _contentMenu;
        private Dictionary<string, Action<T>> _mapingEvent;
        private T _data;
        private bool disposedValue;

        public event EventHandler<T> ChangeData;
        public event EventHandler<TMember> ItemClickEnd;
        public TMember Member = null;

        public T Data
        {
            get => _data;
            set
            {
                _data = value;

                if (ChangeData != null)
                {
                    ChangeData(this, _data);
                }
            }
        }

        public ContextMenuStripData()
        {
            _contentMenu = new MaterialContextMenuStrip();
            _mapingEvent = new Dictionary<string, Action<T>>();
            _contentMenu.Cursor = Cursors.Hand;
            _contentMenu.OnItemClickStart += (sender, e) =>
            {
                if (e.ClickedItem == null || !_mapingEvent.ContainsKey(e.ClickedItem.Name) || _mapingEvent[e.ClickedItem.Name] == null)
                {
                    return;
                }
                _mapingEvent[e.ClickedItem.Name](this.Data);

                if (ItemClickEnd != null)
                {
                    ItemClickEnd(e, Member);
                }
            };
        }

        public void Show(Point point, T param)
        {
            Data = param;
            _contentMenu.Show(point);
        }

        public void Show(Point point)
        {
            _contentMenu.Show(point);
        }

        public string AddItem(string display, Action<T> action = null, Bitmap icon = null)
        {
            var itemAdd = _contentMenu.Items.Add(display);
            itemAdd.Name = "Item" + Guid.NewGuid().ToString();
            if (icon != null)
            {
                _contentMenu.Items[itemAdd.Name].Image = icon;
                _contentMenu.Items[itemAdd.Name].ImageAlign = ContentAlignment.MiddleLeft;
            }
            _mapingEvent.Add(itemAdd.Name, action);
            return itemAdd.Name;
        }

        public void EnaleItem(string display, bool enable)
        {
            foreach (ToolStripItem item in _contentMenu.Items)
            {
                if (item.Text.Equals(display))
                {
                    item.Enabled = enable;
                    return;
                }
            }
        }

        public bool UpdateItem(string id, string display, Action<T> action = null, Bitmap icon = null)
        {
            if (!_mapingEvent.ContainsKey(id) || !_contentMenu.Items.ContainsKey(id))
            {
                return false;
            }

            display = (display + "").Trim();

            if (string.IsNullOrEmpty(display))
            {
                return false;
            }

            _contentMenu.Items[id].Text = display;
            _contentMenu.Items[id].Image = icon;
            _contentMenu.Items[id].ImageAlign = ContentAlignment.MiddleLeft;

            if (action != null)
            {
                _mapingEvent[id] = action;
            }

            return true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    if (_contentMenu != null)
                    {
                        _contentMenu.Dispose();
                    }

                    if (_mapingEvent != null)
                    {
                        _mapingEvent.Clear();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _contentMenu = null;
                _mapingEvent = null;
                _data = default(T);
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ContextMenuStripData()
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
    }
}