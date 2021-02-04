using System;
using System.Linq;
using System.Windows.Forms;
using GP40Main.Core;
using GP40Main.Services.Navigation;
using static GP40Main.Core.AppConst;
using GP40Main.Models;
using GP40Main.Utility;
using GP40Main.Themes.Controls;
using System.Drawing;
using System.Diagnostics;
using System.Data;
using System.Data.OleDb;
using Dapper;
using GP40Main.Services.Dialog;
using System.Collections.Generic;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Threading;

namespace GP40Main.Views.Config
{
    /// <summary>
    /// Meno        : Setting common data
    /// Create by   : AKB Bùi Minh Chiến
    /// </summary>
    public partial class ConfigCommon : BaseUserControl
    {
        public const string cstrSetDefaultText = "Đặt làm mặc định";
        public const string cstrSetNotDefaultText = "Hủy làm mặc định";

        public ConfigCommon(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            tabMain_SelectedIndexChanged(tabMain, new EventArgs());
        }

        #region Event Form

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedTab = tabMain.SelectedTab;

            if (selectedTab.Name == tabNational.Name)
            {
                LoadDataToGrid<MNationality>(gridNational);
            }
            else if (selectedTab.Name == tabReligion.Name)
            {
                LoadDataToGrid<MReligion>(gridReligion);
            }
            else if (selectedTab.Name == tabTypeName.Name)
            {
                LoadDataToGrid<MTypeName>(gridTypeName);
            }
            else if (selectedTab.Name == tabRelation.Name)
            {
                LoadDataMRelation();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var selectedTab = tabMain.SelectedTab;

            if (selectedTab.Name == tabNational.Name)
            {
                AppManager.Navigation.ShowDialog<popupNRT, MNationality>(new NavigationParameters(selectedTab.Name), ModeForm.New);
                LoadDataToGrid<MNationality>(gridNational);
            }
            else if (selectedTab.Name == tabReligion.Name)
            {
                AppManager.Navigation.ShowDialog<popupNRT, MReligion>(new NavigationParameters(selectedTab.Name), ModeForm.New);
                LoadDataToGrid<MReligion>(gridReligion);
            }
            else if (selectedTab.Name == tabTypeName.Name)
            {
                AppManager.Navigation.ShowDialog<popupNRT, MTypeName>(new NavigationParameters(selectedTab.Name), ModeForm.New);
                LoadDataToGrid<MTypeName>(gridTypeName);
            }
            else if (selectedTab.Name == tabRelation.Name)
            {
                AppManager.Navigation.ShowDialog<popupAddNewRelation>(new NavigationParameters(), ModeForm.New);
                LoadDataMRelation();
            }
        }

        private void gridNational_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var rowSelected = gridNational.Rows[e.RowIndex].DataBoundItem as MNationality;

            if (rowSelected == null)
            {
                return;
            }

            if (e.ColumnIndex == ColActionNational.Index)
            {
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }

                var mNationality = AppManager.DBManager.GetTable<MNationality>();
                var objData = mNationality.CreateQuery(i => i.Id == rowSelected.Id).FirstOrDefault();

                if (objData == null)
                {
                    return;
                }

                // set date delete
                objData.DeleteDate = DateTime.Now;

                if(!mNationality.UpdateOne(i => i.Id == objData.Id, objData))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }
                LoadDataToGrid<MNationality>(gridNational);
            }
        }

        private void gridReligion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var rowSelected = gridReligion.Rows[e.RowIndex].DataBoundItem as MReligion;

            if (rowSelected == null)
            {
                return;
            }

            if (e.ColumnIndex == colActionReligion.Index)
            {
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }

                var mReligion = AppManager.DBManager.GetTable<MReligion>();
                var objData = mReligion.CreateQuery(i => i.Id == rowSelected.Id).FirstOrDefault();

                if (objData == null)
                {
                    return;
                }

                // set date delete
                objData.DeleteDate = DateTime.Now;

                if(!mReligion.UpdateOne(i => i.Id == objData.Id, objData))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }
                LoadDataToGrid<MReligion>(gridReligion);
            }
        }

        private void gridTypeName_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var rowSelected = gridTypeName.Rows[e.RowIndex].DataBoundItem as MTypeName;

            if (rowSelected == null)
            {
                return;
            }

            if (e.ColumnIndex == colActionTypeName.Index)
            {
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }

                var mTypeName = AppManager.DBManager.GetTable<MTypeName>();
                var objData = mTypeName.CreateQuery(i => i.Id == rowSelected.Id).FirstOrDefault();

                if (objData == null)
                {
                    return;
                }

                // set date delete
                objData.DeleteDate = DateTime.Now;

                if(!mTypeName.UpdateOne(i => i.Id == objData.Id, objData))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }
                LoadDataToGrid<MTypeName>(gridTypeName);
            }
        }

        private void gridRelation_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var rowSelected = gridRelation.Rows[e.RowIndex].DataBoundItem as MRelation;

            if (rowSelected == null)
            {
                return;
            }

            if (e.ColumnIndex == colActionRelation.Index)
            {
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }

                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                var objMRelation = tblMRelation.CreateQuery(i => i.Id == rowSelected.Id).FirstOrDefault();
                if (objMRelation != null)
                {
                    objMRelation.DeleteDate = DateTime.Now;
                    var objRelatedMRelation = tblMRelation.CreateQuery(i => i.MainRelation == objMRelation.RelatedRelation).FirstOrDefault();
                    if (objRelatedMRelation != null)
                    {
                        objRelatedMRelation.DeleteDate = DateTime.Now;
                    }

                    // find in TMemberRelation
                    var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();

                    var lstTMemberRelationMain = tblTMemberRelation.CreateQuery(i => i.relType == objMRelation.MainRelation).ToList();
                    if(lstTMemberRelationMain != null)
                    {
                        foreach(var objTMemberRelation in lstTMemberRelationMain)
                        {
                            objTMemberRelation.DeleteDate = DateTime.Now;

                            // find in TMember
                            var tblTMember = AppManager.DBManager.GetTable<TMember>();
                            // main member
                            var objTMemberMain = tblTMember.CreateQuery(i => i.Id == objTMemberRelation.memberId).FirstOrDefault();
                            if(objTMemberMain != null)
                            {
                                // listPARENT - SPOUSE - CHILD
                                if (objTMemberRelation.relType.Contains(cstrPreFixDAD) || objTMemberRelation.relType.Contains(cstrPreFixMOM))
                                {
                                    objTMemberMain.ListPARENT.Remove(objTMemberRelation.relMemberId);
                                }
                                else if (objTMemberRelation.relType.Contains(cstrPreFixHUSBAND) || objTMemberRelation.relType.Contains(cstrPreFixWIFE))
                                {
                                    objTMemberMain.ListSPOUSE.Remove(objTMemberRelation.relMemberId);
                                }
                                else if (objTMemberRelation.relType.Contains(cstrPreFixCHILD))
                                {
                                    objTMemberMain.ListCHILDREN.Remove(objTMemberRelation.relMemberId);
                                }
                                var findInRelation = objTMemberMain.Relation.FindAll(i => i.memberId == objTMemberRelation.memberId &&
                                                                                          i.relMemberId != objTMemberRelation.relMemberId);
                                objTMemberMain.Relation = findInRelation;

                                if (!tblTMember.UpdateOne(i => i.Id == objTMemberMain.Id, objTMemberMain))
                                {
                                    AppManager.Dialog.Error("Xóa thất bại!");
                                    return;
                                }
                            }
                            // related member
                            var objTMemberRelated = tblTMember.CreateQuery(i => i.Id == objTMemberRelation.relMemberId).FirstOrDefault();
                            if (objTMemberRelated != null)
                            {
                                // listPARENT - SPOUSE - CHILD
                                if (objTMemberRelation.relType.Contains(cstrPreFixDAD) || objTMemberRelation.relType.Contains(cstrPreFixMOM))
                                {
                                    objTMemberRelated.ListPARENT.Remove(objTMemberRelation.memberId);
                                }
                                else if (objTMemberRelation.relType.Contains(cstrPreFixHUSBAND) || objTMemberRelation.relType.Contains(cstrPreFixWIFE))
                                {
                                    objTMemberRelated.ListSPOUSE.Remove(objTMemberRelation.memberId);
                                }
                                else if (objTMemberRelation.relType.Contains(cstrPreFixCHILD))
                                {
                                    objTMemberRelated.ListCHILDREN.Remove(objTMemberRelation.memberId);
                                }
                                var findInRelation = objTMemberRelated.Relation.FindAll(i => i.memberId == objTMemberRelation.relMemberId &&
                                                                                             i.relMemberId != objTMemberRelation.memberId);
                                objTMemberRelated.Relation = findInRelation;

                                if (!tblTMember.UpdateOne(i => i.Id == objTMemberRelated.Id, objTMemberRelated))
                                {
                                    AppManager.Dialog.Error("Xóa thất bại!");
                                    return;
                                }
                            }

                            if (!tblTMemberRelation.UpdateOne(i => i.Id == objTMemberRelation.Id, objTMemberRelation))
                            {
                                AppManager.Dialog.Error("Xóa thất bại!");
                                return;
                            }
                        }
                    }

                    var lstTMemberRelationRelated = tblTMemberRelation.CreateQuery(i => i.relType == objMRelation.RelatedRelation).ToList();
                    if (lstTMemberRelationRelated != null)
                    {
                        foreach (var objTMemberRelation in lstTMemberRelationRelated)
                        {
                            objTMemberRelation.DeleteDate = DateTime.Now;

                            if (!tblTMemberRelation.UpdateOne(i => i.Id == objTMemberRelation.Id, objTMemberRelation))
                            {
                                AppManager.Dialog.Error("Xóa thất bại!");
                                return;
                            }
                        }
                    }

                    // update MRelation
                    if (!tblMRelation.UpdateOne(i => i.Id == objMRelation.Id, objMRelation))
                    {
                        AppManager.Dialog.Error("Xóa thất bại!");
                        return;
                    }
                    if (!tblMRelation.UpdateOne(i => i.Id == objRelatedMRelation.Id, objRelatedMRelation))
                    {
                        AppManager.Dialog.Error("Xóa thất bại!");
                        return;
                    }
                }
            }
            LoadDataMRelation();
        }

        private void gridNational_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var rowSelected = gridNational.Rows[e.RowIndex].DataBoundItem as MNationality;

            if (rowSelected != null)
            {
                var param = new NavigationParameters();
                param.Add(popupNRT.KEY_TAB, tabMain.SelectedTab.Name);
                param.Add(popupNRT.VALUE_OBJ, rowSelected);

                AppManager.Navigation.ShowDialog<popupNRT, MNationality>(param, AppConst.ModeForm.Edit);
                LoadDataToGrid<MNationality>(gridNational);
            }
        }

        private void gridReligion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var rowSelected = gridReligion.Rows[e.RowIndex].DataBoundItem as MReligion;

            if (rowSelected != null)
            {
                var param = new NavigationParameters();
                param.Add(popupNRT.KEY_TAB, tabMain.SelectedTab.Name);
                param.Add(popupNRT.VALUE_OBJ, rowSelected);
                AppManager.Navigation.ShowDialog<popupNRT, MReligion>(param, AppConst.ModeForm.Edit);
                LoadDataToGrid<MReligion>(gridReligion);
            }
        }

        private void gridTypeName_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var rowSelected = gridTypeName.Rows[e.RowIndex].DataBoundItem as MTypeName;

            if (rowSelected != null)
            {
                var param = new NavigationParameters();
                param.Add(popupNRT.KEY_TAB, tabMain.SelectedTab.Name);
                param.Add(popupNRT.VALUE_OBJ, rowSelected);
                AppManager.Navigation.ShowDialog<popupNRT, MTypeName>(param, AppConst.ModeForm.Edit);
                LoadDataToGrid<MTypeName>(gridTypeName);
            }
        }

        private void gridRelation_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var rowSelected = gridRelation.Rows[e.RowIndex].DataBoundItem as MRelation;

            if (rowSelected != null)
            {
                AppManager.Navigation.ShowDialog<popupAddNewRelation>(new NavigationParameters(rowSelected), AppConst.ModeForm.Edit);
                LoadDataMRelation();
            }
        }

        private void gridNational_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                gridNational.ClearSelection();

                ContextMenuStrip m = new ContextMenuStrip();

                int currentMouseOverRow = gridNational.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    gridNational.Rows[currentMouseOverRow].Selected = true;

                    var objRowSelected = gridNational.SelectedRows[0].DataBoundItem as MNationality;
                    if (objRowSelected != null)
                    {
                        var objData = AppManager.DBManager.GetTable<MNationality>().CreateQuery(i => i.Id == objRowSelected.Id).FirstOrDefault();
                        if(objData != null)
                        {
                            // check isDefault
                            if (objData.IsDefault)
                            {
                                m.Items.Add(cstrSetNotDefaultText);
                            }
                            else
                            {
                                m.Items.Add(cstrSetDefaultText);
                            }

                            m.Show(gridNational, new Point(e.X, e.Y));
                            m.ItemClicked += new ToolStripItemClickedEventHandler(contextMenu_ItemClicked);
                        }
                    }
                }
            }
        }

        private void gridReligion_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                gridReligion.ClearSelection();

                ContextMenuStrip m = new ContextMenuStrip();

                int currentMouseOverRow = gridReligion.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    gridReligion.Rows[currentMouseOverRow].Selected = true;

                    var objRowSelected = gridReligion.SelectedRows[0].DataBoundItem as MReligion;
                    if (objRowSelected != null)
                    {
                        var objData = AppManager.DBManager.GetTable<MReligion>().CreateQuery(i => i.Id == objRowSelected.Id).FirstOrDefault();
                        if (objData != null)
                        {
                            // check isDefault
                            if (objData.IsDefault)
                            {
                                m.Items.Add(cstrSetNotDefaultText);
                            }
                            else
                            {
                                m.Items.Add(cstrSetDefaultText);
                            }

                            m.Show(gridReligion, new Point(e.X, e.Y));
                            m.ItemClicked += new ToolStripItemClickedEventHandler(contextMenu_ItemClicked);
                        }
                    }
                }
            }
        }

        private void gridTypeName_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                gridTypeName.ClearSelection();

                ContextMenuStrip m = new ContextMenuStrip();

                int currentMouseOverRow = gridTypeName.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    gridTypeName.Rows[currentMouseOverRow].Selected = true;

                    var objRowSelected = gridTypeName.SelectedRows[0].DataBoundItem as MTypeName;
                    if (objRowSelected != null)
                    {
                        var objData = AppManager.DBManager.GetTable<MTypeName>().CreateQuery(i => i.Id == objRowSelected.Id).FirstOrDefault();
                        if (objData != null)
                        {
                            // check isDefault
                            if (objData.IsDefault)
                            {
                                m.Items.Add(cstrSetNotDefaultText);
                            }
                            else
                            {
                                m.Items.Add(cstrSetDefaultText);
                            }

                            m.Show(gridTypeName, new Point(e.X, e.Y));
                            m.ItemClicked += new ToolStripItemClickedEventHandler(contextMenu_ItemClicked);
                        }
                    }
                }
            }
        }

        private void btnRestoreData_Click(object sender, EventArgs e)
        {
            try
            {
                var strPath = AppManager.Dialog.OpenFile("Gia pha File (*.gpb)|*.gpb");

                if (!string.IsNullOrEmpty(strPath))
                {
                    AppManager.Dialog.ShowProgressBar((obj) =>
                    {
                        var flag = fncRestoreOldDBBackup(strPath, obj);

                        this.SafeInvoke(() =>
                        {
                            var rst = !flag ? AppManager.Dialog.Error("Khôi phục dữ liệu thất bại") 
                                            : AppManager.Dialog.Ok("Khôi phục dữ liệu thành công");
                        });
                    });
                }
            } catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }
        #endregion Event Form

        #region Private Function
        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try {
                ToolStripItem item = e.ClickedItem;
                var selectedTab = tabMain.SelectedTab;

                if (AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    switch (item.Text)
                    {
                        case cstrSetDefaultText:
                            if (selectedTab.Name == tabNational.Name)
                            {
                                var objRowSelected = gridNational.SelectedRows[0].DataBoundItem as MNationality;
                                var tblMNational = AppManager.DBManager.GetTable<MNationality>();
                                // find and reset old default
                                var oldObjDataa = tblMNational.CreateQuery(i => i.IsDefault == true).FirstOrDefault();
                                if (oldObjDataa != null)
                                {
                                    oldObjDataa.IsDefault = false;
                                    if (!tblMNational.UpdateOne(i => i.Id == oldObjDataa.Id, oldObjDataa))
                                    {
                                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                                        return;
                                    }
                                }

                                var objData = tblMNational.CreateQuery(i => i.Id == objRowSelected.Id).FirstOrDefault();
                                if (objData != null)
                                {
                                    // update set to default
                                    objData.IsDefault = true;
                                    if (!tblMNational.UpdateOne(i => i.Id == objData.Id, objData))
                                    {
                                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                                        return;
                                    }
                                }
                            }
                            else if (selectedTab.Name == tabReligion.Name)
                            {
                                var objRowSelected = gridReligion.SelectedRows[0].DataBoundItem as MReligion;
                                var tblMReligion = AppManager.DBManager.GetTable<MReligion>();
                                // find and reset old default
                                var oldObjDataa = tblMReligion.CreateQuery(i => i.IsDefault == true).FirstOrDefault();
                                if (oldObjDataa != null)
                                {
                                    oldObjDataa.IsDefault = false;
                                    if (!tblMReligion.UpdateOne(i => i.Id == oldObjDataa.Id, oldObjDataa))
                                    {
                                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                                        return;
                                    }
                                }

                                var objData = tblMReligion.CreateQuery(i => i.Id == objRowSelected.Id).FirstOrDefault();
                                if (objData != null)
                                {
                                    // update set to default
                                    objData.IsDefault = true;
                                    if (!tblMReligion.UpdateOne(i => i.Id == objData.Id, objData))
                                    {
                                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                                        return;
                                    }
                                }
                            }
                            else if (selectedTab.Name == tabTypeName.Name)
                            {
                                var objRowSelected = gridTypeName.SelectedRows[0].DataBoundItem as MTypeName;
                                var tblTypeName = AppManager.DBManager.GetTable<MTypeName>();
                                // find and reset old default
                                var oldObjDataa = tblTypeName.CreateQuery(i => i.IsDefault == true).FirstOrDefault();
                                if (oldObjDataa != null)
                                {
                                    oldObjDataa.IsDefault = false;
                                    if (!tblTypeName.UpdateOne(i => i.Id == oldObjDataa.Id, oldObjDataa))
                                    {
                                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                                        return;
                                    }
                                }

                                var objData = tblTypeName.CreateQuery(i => i.Id == objRowSelected.Id).FirstOrDefault();
                                if (objData != null)
                                {
                                    // update set to default
                                    objData.IsDefault = true;
                                    if (!tblTypeName.UpdateOne(i => i.Id == objData.Id, objData))
                                    {
                                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                                        return;
                                    }
                                }
                            }
                            break;
                        case cstrSetNotDefaultText:
                            if (selectedTab.Name == tabNational.Name)
                            {
                                var objRowSelected = gridNational.SelectedRows[0].DataBoundItem as MNationality;
                                var tblMNational = AppManager.DBManager.GetTable<MNationality>();
                                var objData = tblMNational.CreateQuery(i => i.Id == objRowSelected.Id).FirstOrDefault();
                                if (objData != null)
                                {
                                    objData.IsDefault = false;
                                    if (!tblMNational.UpdateOne(i => i.Id == objData.Id, objData))
                                    {
                                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                                        return;
                                    }
                                }
                            }
                            else if (selectedTab.Name == tabReligion.Name)
                            {
                                var objRowSelected = gridReligion.SelectedRows[0].DataBoundItem as MReligion;
                                var tblMReligion = AppManager.DBManager.GetTable<MReligion>();
                                var objData = tblMReligion.CreateQuery(i => i.Id == objRowSelected.Id).FirstOrDefault();
                                if (objData != null)
                                {
                                    objData.IsDefault = false;
                                    if (!tblMReligion.UpdateOne(i => i.Id == objData.Id, objData))
                                    {
                                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                                        return;
                                    }
                                }
                            }
                            else if (selectedTab.Name == tabTypeName.Name)
                            {
                                var objRowSelected = gridTypeName.SelectedRows[0].DataBoundItem as MTypeName;
                                var tblTypeName = AppManager.DBManager.GetTable<MTypeName>();
                                var objData = tblTypeName.CreateQuery(i => i.Id == objRowSelected.Id).FirstOrDefault();
                                if (objData != null)
                                {
                                    objData.IsDefault = false;
                                    if (!tblTypeName.UpdateOne(i => i.Id == objData.Id, objData))
                                    {
                                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                                        return;
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }

        private void LoadDataToGrid<T>(DataGridTemplate gridTemplate) where T : BaseModel
        {
            var data = AppManager.DBManager.GetTable<T>().CreateQuery().ToList();
            BindingHelper.BindingDataGrid(gridTemplate, data);
        }

        private void LoadDataMRelation()
        {
            try {
                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();

                var dataLinQ = from mRelationRelated in tblMRelation.AsEnumerable()
                               join mRelationMain in tblMRelation.AsEnumerable()
                               on mRelationRelated.RelatedRelation equals mRelationMain.MainRelation
                               select new ExMRelation
                               {
                                   Id = mRelationRelated.Id,
                                   MainRelationNameShow = mRelationRelated.NameOfRelation,
                                   RelatedRelationNameShow = mRelationMain.NameOfRelation
                               };
                BindingHelper.BindingDataGrid(gridRelation, dataLinQ.ToList());
            } catch (Exception ex) {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }
        #endregion Private Function

        private void btnRestoreNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (!AppManager.Dialog.Confirm("Khi dữ liệu được khôi phục thành công thì dữ liệu hiện tại sẽ bị mất hoàn toàn. Hãy chú ý trước khi khôi phục dữ liệu"))
                {
                    return;
                }

                var strPath = AppManager.Dialog.OpenFile("Gia pha new File (*.gp4b)|*.gp4b");

                if (!string.IsNullOrEmpty(strPath))
                {
                    AppManager.Dialog.ShowProgressBar((obj) =>
                    {
                        var flag = fncRestoreNewDBBackup(strPath, obj);

                        this.SafeInvoke(() =>
                        {
                            var rst = !flag ? AppManager.Dialog.Error("Khôi phục dữ liệu thất bại")
                                            : AppManager.Dialog.Ok("Khôi phục dữ liệu thành công");
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                var strPath = AppManager.Dialog.SaveFile("", "Gia pha new File (*.gp4b)|*.gp4b");

                if (!string.IsNullOrWhiteSpace(strPath))
                {
                    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    {
                        return;
                    }

                    AppManager.Dialog.ShowProgressBar((obj) =>
                    {
                        var flag = fncBackupDB(strPath, obj);

                        this.SafeInvoke(() =>
                        {
                            var rst = !flag ? AppManager.Dialog.Error("Sao lưu dữ liệu thất bại")
                                            : AppManager.Dialog.Ok("Sao lưu dữ liệu thành công");
                        });
                    });
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }

        private void addDir(ZipFile objZip, string strDir)
        {
            try
            {
                var objdir = new DirectoryInfo(strDir);
                addRecusiveDir(objZip, objdir, "");
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }

        private void addRecusiveDir(ZipFile objZip, DirectoryInfo objDir, string strFolderName)
        {
            try
            {
                var strSubFolderName = objDir.Name;

                // add file to zip
                foreach (FileInfo file in objDir.GetFiles())
                {
                    objZip.Add(file.FullName, strFolderName + "\\" + strSubFolderName + "\\" + file.Name);
                }

                // recusive sub-folder
                foreach(DirectoryInfo dir in objDir.GetDirectories())
                {
                    addRecusiveDir(objZip, dir, strFolderName + "\\" + strSubFolderName + "\\");
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }

        private bool fncRestoreOldDBBackup(string fileName, clsProgressBar progressBar)
        {
            try
            {
                AppManager.isExecuting = true;

                if (string.IsNullOrEmpty(fileName))
                {
                    return false;
                }

                FastZip objZip = null;

                var strExtractFolder = Directory.GetCurrentDirectory() + "\\Temp";

                if (!Directory.Exists(strExtractFolder))
                {
                    Directory.CreateDirectory(strExtractFolder);
                }

                objZip = new FastZip();
                objZip.Password = "giaphabackup";
                objZip.ExtractZip(fileName, strExtractFolder, "");

                var fileRestore = strExtractFolder + "\\Data\\giaphadb.mdb";

                if (!File.Exists(fileRestore))
                {
                    return false;
                }

                using (var conection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" + "data source='" + fileRestore + "';Jet OLEDB:Database Password=giapha1712012"))
                {
                    conection.Open();
                    var data = conection.Query<TFMemberMain>("SELECT T_FMEMBER_MAIN.* FROM T_FMEMBER_MAIN").ToList();
                    var mapId = new Dictionary<int, TMember>();

                    if (data != null)
                    {
                        var lstMember = new List<TMember>();
                        var lstMemberRelation = new List<TMemberRelation>();
                        var cnn = 0;

                        data.ForEach(item =>
                        {
                            var member = mapId.ContainsKey(item.MEMBER_ID) ? mapId[item.MEMBER_ID] : new TMember();

                            if (string.IsNullOrEmpty(member.Id))
                            {
                                member.Id = LiteDBManager.CreateNewId();
                            }

                            #region Info Member
                            string strFullname = item.LAST_NAME + ' ' + item.MIDDLE_NAME + ' ' + item.FIRST_NAME;
                            // bảng t_fmember_main
                            // MEMBER_ID
                            member.Name = strFullname;
                            var tblTypeName = AppManager.DBManager.GetTable<MTypeName>();
                            var firstTypeName = tblTypeName.CreateQuery(i => i.DeleteDate == null).FirstOrDefault();
                            member.TypeName[firstTypeName.Id] = item.ALIAS_NAME?.Trim();
                            // BIRTH_DAY
                            var emGender = (item.GENDER == 1) ? GenderMember.Male : ((item.GENDER == 2) ? GenderMember.Female : GenderMember.Unknown);
                            member.Gender = (int)emGender;
                            // HOMETOWN
                            member.BirthPlace = item.BIRTH_PLACE?.Trim();
                            member.National = item.NATIONALITY?.Trim();
                            member.Religion = item.RELIGION?.Trim();
                            // BURY_PLACE
                            // AVATAR_PATH
                            // FAMILY_ORDER
                            // REMARK
                            // DECEASED_DATE
                            // BIR_DAY
                            member.Birthday.DaySun = (item.BIR_DAY != 0) ? item.BIR_DAY : -1;
                            member.Birthday.MonthSun = (item.BIR_MON != 0) ? item.BIR_MON : -1;
                            member.Birthday.YearSun = (item.BIR_YEA != 0) ? item.BIR_YEA : -1;
                            member.Birthday.DayMoon = (item.BIR_DAY_LUNAR != 0) ? item.BIR_DAY_LUNAR : -1;
                            member.Birthday.MonthMoon = (item.BIR_MON_LUNAR != 0) ? item.BIR_MON_LUNAR : -1;
                            member.Birthday.YearMoon = (item.BIR_YEA_LUNAR != 0) ? item.BIR_YEA_LUNAR : -1;

                            if (item.DECEASED == 1)
                            {
                                member.IsDeath = true;
                                member.DeadDay.DaySun = (item.DEA_DAY_SUN != 0) ? item.DEA_DAY_SUN : -1;
                                member.DeadDay.MonthSun = (item.DEA_MON_SUN != 0) ? item.DEA_MON_SUN : -1;
                                member.DeadDay.YearSun = (item.DEA_YEA_SUN != 0) ? item.DEA_YEA_SUN : -1;
                                member.DeadDay.DayMoon = (item.DEA_DAY != 0) ? item.DEA_DAY : -1;
                                member.DeadDay.MonthMoon = (item.DEA_MON != 0) ? item.DEA_MON : -1;
                                member.DeadDay.YearSun = (item.DEA_YEA != 0) ? item.DEA_YEA : -1;
                            }
                            else
                            {
                                member.IsDeath = false;
                            }
                            // CAREER_TYPE
                            // EDUCATION_TYPE
                            // FACT_TYPE
                            // CAREER
                            // EDUCATION
                            // FACT
                            // LEVEL

                            // bảng t_fmember_contact ~ thông tin địa chỉ
                            var lstContactOfMember = conection.Query<TFMemberContact>("SELECT T_FMEMBER_CONTACT.* FROM T_FMEMBER_CONTACT INNER JOIN T_FMEMBER_MAIN ON T_FMEMBER_CONTACT.MEMBER_ID = T_FMEMBER_MAIN.MEMBER_ID WHERE T_FMEMBER_CONTACT.MEMBER_ID = " + item.MEMBER_ID).ToList();
                            if (lstContactOfMember != null)
                            {
                                foreach (var objContact in lstContactOfMember)
                                {
                                    member.HomeTown = objContact.HOMETOWN?.Trim();
                                    member.Contact.Address = objContact.HOME_ADD?.Trim();
                                    member.Contact.Tel_1 = objContact.PHONENUM1?.Trim();
                                    member.Contact.Tel_2 = objContact.PHONENUM2?.Trim();
                                    member.Contact.Email_1 = objContact.MAIL_ADD1?.Trim();
                                    member.Contact.Email_2 = objContact.MAIL_ADD2?.Trim();
                                    member.Contact.Fax = objContact.FAXNUM?.Trim();
                                    member.Contact.Website = objContact.URL?.Trim();
                                    member.Contact.SocialNetwork["IMChat"] = objContact.IMNICK?.Trim();
                                    member.Contact.Note = objContact.REMARK?.Trim();
                                }
                            }

                            // bảng t_fmember_career ~ nghề nghiệp (type = 2) & học hành (type = 1)
                            var lstCareerOfMember = conection.Query<TFMemberCareer>("SELECT T_FMEMBER_CAREER.* FROM T_FMEMBER_CAREER INNER JOIN T_FMEMBER_MAIN ON T_FMEMBER_CAREER.MEMBER_ID = T_FMEMBER_MAIN.MEMBER_ID WHERE T_FMEMBER_CAREER.MEMBER_ID = " + item.MEMBER_ID).ToList();
                            if (lstCareerOfMember != null)
                            {
                                foreach (var objCareer in lstCareerOfMember)
                                {
                                    // job
                                    if (objCareer.CAREER_TYPE == 2)
                                    {
                                        var objTMemberJob = new TMemberJob();
                                        objTMemberJob.Id = LiteDBManager.CreateNewId();
                                        objTMemberJob.CompanyName = objCareer.OFFICE_NAME?.Trim();
                                        objTMemberJob.CompanyAddress = objCareer.OFFICE_PLACE?.Trim();
                                        objTMemberJob.Job = objCareer.OCCUPATION?.Trim();
                                        objTMemberJob.Position = objCareer.POSITION?.Trim();
                                        objTMemberJob.StartDate.DaySun = (objCareer.START_DAY != 0) ? objCareer.START_DAY : -1;
                                        objTMemberJob.StartDate.MonthSun = (objCareer.START_MON != 0) ? objCareer.START_MON : -1;
                                        objTMemberJob.StartDate.YearSun = (objCareer.START_YEA != 0) ? objCareer.START_YEA : -1;
                                        objTMemberJob.EndDate.DaySun = (objCareer.END_DAY != 0) ? objCareer.END_DAY : -1;
                                        objTMemberJob.EndDate.MonthSun = (objCareer.END_MON != 0) ? objCareer.END_MON : -1;
                                        objTMemberJob.EndDate.YearSun = (objCareer.END_YEA != 0) ? objCareer.END_YEA : -1;
                                        member.Job.Add(objTMemberJob);
                                    }
                                    // learn
                                    if (objCareer.CAREER_TYPE == 1)
                                    {
                                        var objTMemberSchool = new TMemberSchool();
                                        objTMemberSchool.Id = LiteDBManager.CreateNewId();
                                        objTMemberSchool.SchoolName = objCareer.OFFICE_NAME?.Trim();
                                        objTMemberSchool.Description = objCareer.REMARK?.Trim();
                                        objTMemberSchool.StartDate.DaySun = (objCareer.START_DAY != 0) ? objCareer.START_DAY : -1;
                                        objTMemberSchool.StartDate.MonthSun = (objCareer.START_MON != 0) ? objCareer.START_MON : -1;
                                        objTMemberSchool.StartDate.YearSun = (objCareer.START_YEA != 0) ? objCareer.START_YEA : -1;
                                        objTMemberSchool.EndDate.DaySun = (objCareer.END_DAY != 0) ? objCareer.END_DAY : -1;
                                        objTMemberSchool.EndDate.MonthSun = (objCareer.END_MON != 0) ? objCareer.END_MON : -1;
                                        objTMemberSchool.EndDate.YearSun = (objCareer.END_YEA != 0) ? objCareer.END_YEA : -1;
                                        member.School.Add(objTMemberSchool);
                                    }
                                }
                            }

                            // bảng t_fmember_fact ~ sự kiện
                            var lstFactOfMember = conection.Query<TFMemberFact>("SELECT T_FMEMBER_FACT.* FROM T_FMEMBER_FACT INNER JOIN T_FMEMBER_MAIN ON T_FMEMBER_FACT.MEMBER_ID = T_FMEMBER_MAIN.MEMBER_ID WHERE T_FMEMBER_FACT.MEMBER_ID = " + item.MEMBER_ID).ToList();
                            if (lstFactOfMember != null)
                            {
                                foreach (var objFact in lstFactOfMember)
                                {
                                    var objTMemberEvent = new TMemberEvent();
                                    objTMemberEvent.Id = LiteDBManager.CreateNewId();
                                    objTMemberEvent.EventName = objFact.FACT_NAME?.Trim();
                                    objTMemberEvent.Location = objFact.FACT_PLACE?.Trim();
                                    objTMemberEvent.Description = objFact.DESCRIPTION?.Trim();
                                    objTMemberEvent.StartDate.DaySun = (objFact.START_DAY != 0) ? objFact.START_DAY : -1;
                                    objTMemberEvent.StartDate.MonthSun = (objFact.START_MON != 0) ? objFact.START_MON : -1;
                                    objTMemberEvent.StartDate.YearSun = (objFact.START_YEA != 0) ? objFact.START_YEA : -1;
                                    objTMemberEvent.EndDate.DaySun = (objFact.END_DAY != 0) ? objFact.END_DAY : -1;
                                    objTMemberEvent.EndDate.MonthSun = (objFact.END_MON != 0) ? objFact.END_MON : -1;
                                    objTMemberEvent.EndDate.YearSun = (objFact.END_YEA != 0) ? objFact.END_YEA : -1;
                                    member.Event.Add(objTMemberEvent);
                                }
                            }

                            lstMember.Add(member);

                            #endregion Info Member

                            if (!mapId.ContainsKey(item.MEMBER_ID))
                            {
                                mapId.Add(item.MEMBER_ID, member);
                            }

                            // t_fmember_relation
                            var lstFMemberRelation = conection.Query<TFMemberRelation>("SELECT T_FMEMBER_RELATION.* FROM T_FMEMBER_RELATION INNER JOIN T_FMEMBER_MAIN ON T_FMEMBER_RELATION.MEMBER_ID = T_FMEMBER_MAIN.MEMBER_ID WHERE T_FMEMBER_RELATION.MEMBER_ID = " + item.MEMBER_ID).ToList();
                            if (lstFMemberRelation != null)
                            {
                                foreach (var obj in lstFMemberRelation)
                                {
                                    if (!mapId.ContainsKey(obj.MEMBER_ID))
                                    {
                                        mapId.Add(obj.MEMBER_ID, new TMember());
                                        mapId[obj.MEMBER_ID].Id = LiteDBManager.CreateNewId();
                                    }
                                    var newMEMBER_ID = mapId[obj.MEMBER_ID].Id;

                                    if (!mapId.ContainsKey(obj.REL_FMEMBER_ID))
                                    {
                                        mapId.Add(obj.REL_FMEMBER_ID, new TMember());
                                        mapId[obj.REL_FMEMBER_ID].Id = LiteDBManager.CreateNewId();
                                    }
                                    var newREL_FMEMBER_ID = mapId[obj.REL_FMEMBER_ID].Id;

                                    // detect gender of rel_fmember_id -> dad or mom
                                    int genderOfMEMBER_ID = 0;
                                    var objMEMBER_ID = conection.Query<TFMemberMain>("SELECT T_FMEMBER_MAIN.GENDER FROM T_FMEMBER_MAIN WHERE T_FMEMBER_MAIN.MEMBER_ID = " + obj.MEMBER_ID).FirstOrDefault();
                                    if (objMEMBER_ID != null)
                                    {
                                        genderOfMEMBER_ID = (objMEMBER_ID.GENDER == 1) ? (int)GenderMember.Male : ((objMEMBER_ID.GENDER == 2) ? (int)GenderMember.Female : (int)GenderMember.Unknown);
                                    }
                                    int genderOfREL_FMEMBER_ID = 0;
                                    var objREL_FMEMBER_ID = conection.Query<TFMemberMain>("SELECT T_FMEMBER_MAIN.GENDER FROM T_FMEMBER_MAIN WHERE T_FMEMBER_MAIN.MEMBER_ID = " + obj.REL_FMEMBER_ID).FirstOrDefault();
                                    if (objREL_FMEMBER_ID != null)
                                    {
                                        genderOfREL_FMEMBER_ID = (objREL_FMEMBER_ID.GENDER == 1) ? (int)GenderMember.Male : ((objREL_FMEMBER_ID.GENDER == 2) ? (int)GenderMember.Female : (int)GenderMember.Unknown);
                                    }

                                    // vợ - chồng - 1 - 1
                                    if (obj.RELID == 1)
                                    {
                                        var newObjWH = new TMemberRelation();
                                        newObjWH.Id = LiteDBManager.CreateNewId();
                                        newObjWH.memberId = newMEMBER_ID;
                                        newObjWH.relMemberId = newREL_FMEMBER_ID;
                                        if (genderOfMEMBER_ID == (int)GenderMember.Male)
                                        {
                                            // vợ chồng
                                            newObjWH.relType = "WIF01";
                                        }
                                        if (genderOfMEMBER_ID == (int)GenderMember.Female)
                                        {
                                            // chồng vợ
                                            newObjWH.relType = "HUS01";
                                        }
                                        if (genderOfMEMBER_ID == (int)GenderMember.Unknown)
                                        {
                                            newObjWH.relType = (obj.MEMBER_ID < obj.REL_FMEMBER_ID) ? "WIF01" : "HUS01";
                                        }
                                        newObjWH.roleOrder = 1;

                                        // add to list TMemberRelation
                                        lstMemberRelation.Add(newObjWH);

                                        member.ListSPOUSE.Add(newObjWH.relMemberId);
                                        member.Relation.Add(newObjWH);
                                    }

                                    // con - cha/mẹ - 2 - 0 or con nuôi - cha/mẹ - 4 - 0
                                    if (obj.RELID == 2 || obj.RELID == 4)
                                    {
                                        // con - cha/mẹ
                                        var newObjRelationCP = new TMemberRelation();
                                        newObjRelationCP.Id = LiteDBManager.CreateNewId();
                                        newObjRelationCP.memberId = newMEMBER_ID;
                                        newObjRelationCP.relMemberId = newREL_FMEMBER_ID;
                                        if (genderOfREL_FMEMBER_ID == (int)GenderMember.Male)
                                        {
                                            newObjRelationCP.relType = (obj.RELID == 4) ? "DAD02" : "DAD01";
                                        }
                                        if (genderOfREL_FMEMBER_ID == (int)GenderMember.Female)
                                        {
                                            newObjRelationCP.relType = (obj.RELID == 4) ? "MOM02" : "MOM01";
                                        }
                                        if (genderOfREL_FMEMBER_ID == (int)GenderMember.Unknown)
                                        {
                                            if (obj.MEMBER_ID < obj.REL_FMEMBER_ID)
                                            {
                                                newObjRelationCP.relType = (obj.RELID == 4) ? "DAD02" : "DAD01";
                                            }
                                            else
                                            {
                                                newObjRelationCP.relType = (obj.RELID == 4) ? "MOM02" : "MOM01";
                                            }
                                        }
                                        newObjRelationCP.roleOrder = 0;

                                        // add to list TMemberRelation
                                        lstMemberRelation.Add(newObjRelationCP);

                                        member.ListPARENT.Add(newObjRelationCP.relMemberId);
                                        member.Relation.Add(newObjRelationCP);

                                        // cha/mẹ - con
                                        var newObjRelationPC = new TMemberRelation();
                                        newObjRelationPC.Id = LiteDBManager.CreateNewId();
                                        newObjRelationPC.memberId = newREL_FMEMBER_ID;
                                        newObjRelationPC.relMemberId = newMEMBER_ID;
                                        if (genderOfMEMBER_ID == (int)GenderMember.Male)
                                        {
                                            newObjRelationPC.relType = (obj.RELID == 4) ? "CHI02" : "CHI01";

                                        }
                                        if (genderOfMEMBER_ID == (int)GenderMember.Female)
                                        {
                                            newObjRelationPC.relType = (obj.RELID == 4) ? "CHI04" : "CHI03";

                                        }
                                        if (genderOfMEMBER_ID == (int)GenderMember.Unknown)
                                        {
                                            if (obj.MEMBER_ID < obj.REL_FMEMBER_ID)
                                            {
                                                newObjRelationPC.relType = (obj.RELID == 4) ? "CHI02" : "CHI01";
                                            }
                                            else
                                            {
                                                newObjRelationPC.relType = (obj.RELID == 4) ? "CHI04" : "CHI03";
                                            }
                                        }
                                        newObjRelationPC.roleOrder = 0;

                                        // add to list TMemberRelation
                                        lstMemberRelation.Add(newObjRelationPC);

                                        mapId[obj.REL_FMEMBER_ID].ListCHILDREN.Add(newObjRelationPC.relMemberId);
                                        mapId[obj.REL_FMEMBER_ID].Relation.Add(newObjRelationPC);
                                    }
                                }
                            }

                            cnn++;
                            progressBar.Percent = progressBar.fncCalculatePercent(cnn, data.Count);
                        });

                        if (AppManager.DBManager.DropTable<MNationality>())
                        {
                            GenerateData.CreateDataMNational();
                        }

                        if (AppManager.DBManager.DropTable<MReligion>())
                        {
                            GenerateData.CreateDataMReligion();
                        }

                        if (AppManager.DBManager.DropTable<MTypeName>())
                        {
                            GenerateData.CreateDataMTypeName();
                        }

                        if (AppManager.DBManager.DropTable<MRelation>())
                        {
                            GenerateData.CreateDataMRelation();
                        }

                        if (AppManager.DBManager.DropTable<TMember>())
                        {
                            var tMember = AppManager.DBManager.GetTable<TMember>();
                            if (!tMember.InsertBulk(lstMember, false))
                            {
                                return false;
                            }
                        }

                        var dataFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>().CreateQuery(i => i.DeleteDate == null).FirstOrDefault();
                        if (dataFamilyInfo != null)
                        {
                            var objMFamilyInfoOld = conection.Query<MFamilyInfoOld>("SELECT M_FAMILY_INFO.* FROM M_FAMILY_INFO").FirstOrDefault();
                            if (objMFamilyInfoOld != null)
                            {
                                dataFamilyInfo.FamilyName = objMFamilyInfoOld.FAMILY_NAME?.Trim();
                                dataFamilyInfo.FamilyHometown = objMFamilyInfoOld.FAMILY_HOMETOWN?.Trim();
                                dataFamilyInfo.FamilyAnniversary = ConvertHelper.CnvStringToDateTimeNull(objMFamilyInfoOld.FAMILY_ANNIVERSARY?.Trim(), null, "dd/MM/yyyy");
                                //dataFamilyInfo.FamilyLevel = ConvertHelper.CnvNullToInt(objMFamilyInfoOld.?.Trim(), 1);
                                //dataFamilyInfo.FamilyLevel = dataFamilyInfo.FamilyLevel > 0 ? dataFamilyInfo.FamilyLevel : 1;
                            }

                            // find rootId
                            var objMROOT = conection.Query<MRoot>("SELECT M_ROOT.* FROM M_ROOT").FirstOrDefault();
                            if (objMROOT != null)
                            {
                                if (mapId.ContainsKey(objMROOT.MEMBER_ID))
                                {
                                    dataFamilyInfo.RootId = mapId[objMROOT.MEMBER_ID].Id;
                                }
                            }

                            if (!AppManager.DBManager.GetTable<MFamilyInfo>().UpdateOne(dataFamilyInfo))
                            {
                                return false;
                            }
                        }

                        if (AppManager.DBManager.DropTable<TMemberRelation>())
                        {
                            var tMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();
                            if (!tMemberRelation.InsertBulk(lstMemberRelation, false))
                            {
                                return false;
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                //AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
                return false;
            }
            finally
            {
                AppManager.isExecuting = false;
            }
        }

        private bool fncRestoreNewDBBackup(string fileName, clsProgressBar progressBar)
        {
            try
            {
                AppManager.isExecuting = true;
                var cnn = 0;
                progressBar.Percent = progressBar.fncCalculatePercent(cnn, 6);

                if (string.IsNullOrEmpty(fileName))
                {
                    return false;
                }

                FastZip objZip = null;

                var strExtractFolder = Directory.GetCurrentDirectory() + "\\Temp";
                var strData = Directory.GetCurrentDirectory() + "\\Data\\giapha.db";
                cnn++;
                progressBar.Percent = progressBar.fncCalculatePercent(cnn, 6);

                if (!Directory.Exists(strExtractFolder))
                {
                    Directory.CreateDirectory(strExtractFolder);
                    cnn++;
                    progressBar.Percent = progressBar.fncCalculatePercent(cnn, 6);
                }

                objZip = new FastZip();
                objZip.Password = "giaphabackup";
                objZip.ExtractZip(fileName, strExtractFolder, "");
                cnn++;
                progressBar.Percent = progressBar.fncCalculatePercent(cnn, 6);

                var fileRestore = strExtractFolder + "\\Data\\giapha.db";

                if (!File.Exists(fileRestore))
                {
                    return false;
                }
                cnn++;

                // disconnect db
                AppManager.DBManager.Dispose();
                cnn++;
                progressBar.Percent = progressBar.fncCalculatePercent(cnn, 6);

                // copy & replace db
                File.Copy(fileRestore, strData, true);
                cnn++;
                progressBar.Percent = progressBar.fncCalculatePercent(cnn, 6);
                return true;
            } catch (Exception ex)
            {
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
                return false;
            } finally
            {
                AppManager.isExecuting = false;
            }
        }

        private bool fncBackupDB(string fileName, clsProgressBar progressBar)
        {
            try
            {
                AppManager.isExecuting = true;
                var cnn = 0;
                progressBar.Percent = progressBar.fncCalculatePercent(cnn, 5);

                var file = new FileInfo(fileName);
                cnn++;
                progressBar.Percent = progressBar.fncCalculatePercent(cnn, 5);

                // disconnect db
                AppManager.DBManager.Dispose();
                cnn++;
                progressBar.Percent = progressBar.fncCalculatePercent(cnn, 5);

                ZipFile objZip = null;

                var strData = Directory.GetCurrentDirectory() + "\\Data\\";

                objZip = ZipFile.Create(file.FullName);
                cnn++;
                progressBar.Percent = progressBar.fncCalculatePercent(cnn, 5);

                objZip.BeginUpdate();
                objZip.Password = "giaphabackup";
                addDir(objZip, strData);
                cnn++;
                progressBar.Percent = progressBar.fncCalculatePercent(cnn, 5);

                objZip.CommitUpdate();
                cnn++;
                progressBar.Percent = progressBar.fncCalculatePercent(cnn, 5);

                Process.Start(file.DirectoryName);
                return true;
            } catch (Exception ex)
            {
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
                return false;
            } finally
            {
                AppManager.isExecuting = false;
            }
        }
    }
}
