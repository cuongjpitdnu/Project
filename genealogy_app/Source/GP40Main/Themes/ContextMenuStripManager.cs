using GP40Main.Core;
using GP40Main.Models;
using GP40Main.Services.Navigation;
using GP40Main.Views.Member;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static GP40Main.Core.AppConst;

namespace GP40Main.Themes
{

    public class ContextMenuStripManager
    {
        public const string cstrAddRootFamilyMember = "Đặt làm tổ phụ";
        public const string cstrRemoveRootFamilyMember = "Không phải tổ phụ";

        public static ContextMenuStripData<TMember> CreateForMember()
        {
            var contextMember = new ContextMenuStripData<TMember>();

            // Action
            Action<TMember> actionAddRootMember = (member) =>
            {
                if (member == null) return;
                try
                {
                    if (AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();

                        var objUserLogin = AppManager.LoginUser;
                        var objMFamilyInfo = tblMFamilyInfo.CreateQuery(i => i.Id == objUserLogin.FamilyId).FirstOrDefault();
                        // update rootId with memberId
                        objMFamilyInfo.RootId = member.Id;
                        if (!tblMFamilyInfo.UpdateOne(i => i.Id == objMFamilyInfo.Id, objMFamilyInfo))
                        {
                            AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppManager.Dialog.Error(ex.Message);
                    AppManager.LoggerApp.Error(typeof(ContextMenuStripManager), ex);
                }
            };

            Action<TMember> actionRemoveRootMember = (member) =>
            {
                try
                {
                    if (member == null) return;
                    var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();

                    var objUserLogin = AppManager.LoginUser;
                    var objMFamilyInfo = tblMFamilyInfo.CreateQuery(i => i.Id == objUserLogin.FamilyId).FirstOrDefault();
                    // update rootId to empty
                    objMFamilyInfo.RootId = "";
                    if (!tblMFamilyInfo.UpdateOne(i => i.Id == objMFamilyInfo.Id, objMFamilyInfo))
                    {
                        AppManager.Dialog.Error("Cập nhật dữ liệu thất bại!");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    AppManager.Dialog.Error(ex.Message);
                    AppManager.LoggerApp.Error(typeof(ContextMenuStripManager), ex);
                }
            };

            // Fill menu
            contextMember.AddItem("Chỉnh sửa thông tin", (member) =>
            {
                if (member == null) return;
                AppManager.Navigation.ShowDialog<addMember, TMember>(new NavigationParameters(member), ModeForm.Edit);
            });

            var idItemRootFamilyMember = contextMember.AddItem(cstrAddRootFamilyMember, actionAddRootMember);

            contextMember.AddItem("Xem quan hệ hiện tại", (member) =>
            {
                if (member == null) return;
                AppManager.Navigation.ShowDialog<ListMemberRelation, TMember>(new NavigationParameters(member), ModeForm.None);
            });

            contextMember.AddItem("Thêm quan hệ", (member) =>
            {
                if (member == null) return;
                AppManager.Navigation.ShowDialog<popupAddRelation, TMember>(new NavigationParameters(member), ModeForm.New);
            });

            contextMember.AddItem("Xóa", (objMember) =>
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
                var listMemberRelation = tblTMemberRelation.CreateQuery(i => i.memberId == objMember.Id || i.relMemberId == objMember.Id).ToList();
                if (listMemberRelation != null)
                {
                    foreach (var member in listMemberRelation)
                    {
                        member.DeleteDate = DateTime.Now;

                        // find member in TMember related with member want to delete -> remove listPARENT/SPOUSE/CHILDREN or props Relation
                        var objMemberRelated = tblTMember.CreateQuery(i => i.Id == ((objMember.Id == member.memberId) ? member.relMemberId : member.memberId)).FirstOrDefault();
                        if (objMemberRelated != null)
                        {
                            // listPARENT - SPOUSE - CHILD
                            if (member.relType.Contains(cstrPreFixDAD) || member.relType.Contains(cstrPreFixMOM))
                            {
                                objMemberRelated.ListPARENT.Remove(objMember.Id);
                            }
                            else if (member.relType.Contains(cstrPreFixHUSBAND) || member.relType.Contains(cstrPreFixWIFE))
                            {
                                objMemberRelated.ListSPOUSE.Remove(objMember.Id);
                            }
                            else if (member.relType.Contains(cstrPreFixCHILD))
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
            });

            contextMember.ChangeData += (sender, member) => {
                var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
                var objUserLogin = AppManager.LoginUser;
                var objMFamilyInfo = tblMFamilyInfo.CreateQuery(i => i.Id == objUserLogin.FamilyId).FirstOrDefault();

                if (string.IsNullOrEmpty(objMFamilyInfo.RootId))
                {
                    contextMember.UpdateItem(idItemRootFamilyMember, cstrAddRootFamilyMember, actionAddRootMember);
                }
                else
                {
                    // check isRootId of row selected
                    if (member.Id == objMFamilyInfo.RootId)
                    {
                        contextMember.UpdateItem(idItemRootFamilyMember, cstrRemoveRootFamilyMember, actionRemoveRootMember);
                    }
                    else
                    {
                        contextMember.UpdateItem(idItemRootFamilyMember, cstrAddRootFamilyMember, actionAddRootMember);
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
        public event EventHandler<ToolStripItemClickedEventArgs> ItemClickEnd;

        public T Data { 
            get => _data; 
            set {
                
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

            _contentMenu.OnItemClickStart += (sender, e) =>
            {
                if (e.ClickedItem == null || !_mapingEvent.ContainsKey(e.ClickedItem.Name) || _mapingEvent[e.ClickedItem.Name] == null)
                {
                    return;
                }
                _mapingEvent[e.ClickedItem.Name](this.Data);

                if (ItemClickEnd != null)
                {
                    ItemClickEnd(sender, e);
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

        public string AddItem(string display, Action<T> action = null)
        {
            var itemAdd = _contentMenu.Items.Add(display);
            itemAdd.Name = "Item" + Guid.NewGuid().ToString();
            _mapingEvent.Add(itemAdd.Name, action);
            return itemAdd.Name;
        }

        public bool UpdateItem(string id, string display, Action<T> action = null)
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
