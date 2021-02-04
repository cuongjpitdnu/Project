using Dapper;
using GPCommon;
using GPConst;
using GPMain.Common;
using GPMain.Common.Database;
using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using GPMain.Core;
using GPMain.Views.Controls;
using GPMain.Views.Member;
using GPModels;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GPMain.Views.Config
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
            SetEventDataGridView();

            TabMain_SelectedIndexChanged(tabMain, new EventArgs());
        }

        #region Event Form

        private void SetEventDataGridView()
        {
            // Nationality - edit
            UIHelper.SetColumnEditAction<MNationality>(gridNational, rowSelected =>
            {
                var param = new NavigationParameters()
                {
                    {popupNRT.KEY_TAB, tabMain.SelectedTab.Name},
                    {popupNRT.VALUE_OBJ, rowSelected }
                };

                if (AppManager.Navigation.ShowDialogWithParam<popupNRT>(param, ModeForm.Edit).Result == DialogResult.OK)
                {
                    LoadDataToGrid<MNationality>(gridNational);
                }
            });

            // Nationality - delete
            UIHelper.SetColumnDeleteAction<MNationality>(gridNational, rowSelected =>
            {
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }

                var mNationality = AppManager.DBManager.GetTable<MNationality>();
                var objData = mNationality.FirstOrDefault(i => i.Id == rowSelected.Id);

                if (objData == null)
                {
                    return;
                }

                // set date delete
                objData.DeleteDate = DateTime.Now;

                if (!mNationality.UpdateOne(i => i.Id == objData.Id, objData))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }

                LoadDataToGrid<MNationality>(gridNational);
            });

            // Religion - edit
            UIHelper.SetColumnEditAction<MReligion>(gridReligion, rowSelected =>
            {
                var param = new NavigationParameters()
                {
                    {popupNRT.KEY_TAB, tabMain.SelectedTab.Name},
                    {popupNRT.VALUE_OBJ, rowSelected}
                };

                if (AppManager.Navigation.ShowDialogWithParam<popupNRT>(param, ModeForm.Edit).Result == DialogResult.OK)
                {
                    LoadDataToGrid<MReligion>(gridReligion);
                }
            });

            // Religion - delete
            UIHelper.SetColumnDeleteAction<MReligion>(gridReligion, rowSelected =>
            {
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }

                var mReligion = AppManager.DBManager.GetTable<MReligion>();
                var objData = mReligion.FirstOrDefault(i => i.Id == rowSelected.Id);

                if (objData == null)
                {
                    return;
                }

                // set date delete
                objData.DeleteDate = DateTime.Now;

                if (!mReligion.UpdateOne(i => i.Id == objData.Id, objData))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }

                LoadDataToGrid<MReligion>(gridReligion);
            });

            // TypeName - edit
            UIHelper.SetColumnEditAction<MTypeName>(gridTypeName, rowSelected =>
            {
                var param = new NavigationParameters()
                {
                    {popupNRT.KEY_TAB, tabMain.SelectedTab.Name},
                    {popupNRT.VALUE_OBJ, rowSelected }
                };

                if (AppManager.Navigation.ShowDialogWithParam<popupNRT>(param, ModeForm.Edit).Result == DialogResult.OK)
                {
                    LoadDataToGrid<MTypeName>(gridTypeName);
                }
            });

            // TypeName - delete
            UIHelper.SetColumnDeleteAction<MTypeName>(gridTypeName, rowSelected =>
            {
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }

                var mTypeName = AppManager.DBManager.GetTable<MTypeName>();
                var objData = mTypeName.FirstOrDefault(i => i.Id == rowSelected.Id);

                if (objData == null)
                {
                    return;
                }

                // set date delete
                objData.DeleteDate = DateTime.Now;

                if (!mTypeName.UpdateOne(i => i.Id == objData.Id, objData))
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }
                LoadDataToGrid<MTypeName>(gridTypeName);
            });

            // Relation - edit
            UIHelper.SetColumnEditAction<MRelation>(gridRelation, rowSelected =>
            {
                if (AppManager.Navigation.ShowDialogWithParam<popupAddNewRelation, MRelation>(rowSelected, ModeForm.Edit).Result == DialogResult.OK)
                {
                    LoadDataMRelation();
                }
            });

            // Relation - delete
            UIHelper.SetColumnDeleteAction<MRelation>(gridRelation, rowSelected =>
            {
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }

                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                var objMRelation = tblMRelation.FirstOrDefault(i => i.Id == rowSelected.Id);
                if (objMRelation != null)
                {
                    objMRelation.DeleteDate = DateTime.Now;
                    var objRelatedMRelation = tblMRelation.FirstOrDefault(i => i.MainRelation == objMRelation.RelatedRelation);
                    if (objRelatedMRelation != null)
                    {
                        objRelatedMRelation.DeleteDate = DateTime.Now;
                    }

                    // find in TMemberRelation
                    var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>();

                    var lstTMemberRelationMain = tblTMemberRelation.ToList(i => i.relType == objMRelation.MainRelation);
                    if (lstTMemberRelationMain != null)
                    {
                        foreach (var objTMemberRelation in lstTMemberRelationMain)
                        {
                            objTMemberRelation.DeleteDate = DateTime.Now;

                            // find in TMember
                            var tblTMember = AppManager.DBManager.GetTable<TMember>();
                            // main member
                            var objTMemberMain = tblTMember.FirstOrDefault(i => i.Id == objTMemberRelation.memberId);
                            if (objTMemberMain != null)
                            {
                                // listPARENT - SPOUSE - CHILD
                                if (objTMemberRelation.relType.Contains(Relation.PREFIX_DAD) || objTMemberRelation.relType.Contains(Relation.PREFIX_MOM))
                                {
                                    objTMemberMain.ListPARENT.Remove(objTMemberRelation.relMemberId);
                                }
                                else if (objTMemberRelation.relType.Contains(Relation.PREFIX_HUSBAND) || objTMemberRelation.relType.Contains(Relation.PREFIX_WIFE))
                                {
                                    objTMemberMain.ListSPOUSE.Remove(objTMemberRelation.relMemberId);
                                }
                                else if (objTMemberRelation.relType.Contains(Relation.PREFIX_CHILD))
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
                                if (objTMemberRelation.relType.Contains(Relation.PREFIX_DAD) || objTMemberRelation.relType.Contains(Relation.PREFIX_MOM))
                                {
                                    objTMemberRelated.ListPARENT.Remove(objTMemberRelation.memberId);
                                }
                                else if (objTMemberRelation.relType.Contains(Relation.PREFIX_HUSBAND) || objTMemberRelation.relType.Contains(Relation.PREFIX_WIFE))
                                {
                                    objTMemberRelated.ListSPOUSE.Remove(objTMemberRelation.memberId);
                                }
                                else if (objTMemberRelation.relType.Contains(Relation.PREFIX_CHILD))
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

                    var lstTMemberRelationRelated = tblTMemberRelation.ToList(i => i.relType == objMRelation.RelatedRelation);
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

                LoadDataMRelation();
            });

            UIHelper.SetColumnDownloadAction<Version>(dgvListVersion, rowSelected =>
            {
                var strBackupFolder = Directory.GetCurrentDirectory() + AppConst.BackupDBPath;
                var fileBackup = strBackupFolder + "\\" + rowSelected.VersionName;
                var strPath = AppManager.Dialog.SaveFile("", "Gia pha new File (*.gp4b)|*.gp4b");
                if (!string.IsNullOrEmpty(strPath))
                {
                    File.Copy(fileBackup, strPath, true);
                    var file = new FileInfo(strPath);
                    if (file.Exists)
                    {
                        Process.Start(file.DirectoryName);
                    }
                }
            });

            UIHelper.SetColumnRestoreAction<Version>(dgvListVersion, rowSelected =>
            {


                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }
                var strBackupFolder = Directory.GetCurrentDirectory() + AppConst.BackupDBPath;
                var fileBackup = strBackupFolder + "\\" + rowSelected.VersionName;
                RestoreDataBase(fileBackup, true);
            });

            UIHelper.SetColumnDeleteAction<Version>(dgvListVersion, rowSelected =>
            {
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }
                var strBackupFolder = Directory.GetCurrentDirectory() + AppConst.BackupDBPath;
                var fileBackup = strBackupFolder + "\\" + rowSelected.VersionName;
                if (!File.Exists(fileBackup))
                {
                    return;
                }

                File.Delete(fileBackup);
                AppManager.Dialog.Ok("Xóa thành công");
                LoadListVersionBackup();
            });
        }

        private void TabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedTab = tabMain.SelectedTab;

            if (selectedTab.Name == tabNational.Name)
            {
                LoadDataToGrid<MNationality>(gridNational);
                btnAdd.Show();
            }
            else if (selectedTab.Name == tabReligion.Name)
            {
                LoadDataToGrid<MReligion>(gridReligion);
                btnAdd.Show();
            }
            else if (selectedTab.Name == tabTypeName.Name)
            {
                LoadDataToGrid<MTypeName>(gridTypeName);
                btnAdd.Show();
            }
            else if (selectedTab.Name == tabRelation.Name)
            {
                LoadDataMRelation();
                btnAdd.Show();
            }
            else if (selectedTab.Name == tabUpdateRestoreVersion.Name)
            {
                LoadDataInfoVersion();
                btnAdd.Hide();
            }
            else if (selectedTab.Name == tabSystem.Name)
            {
                LoadListVersionBackup();
                btnAdd.Hide();
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var selectedTabName = tabMain.SelectedTab?.Name + "";

            if (string.IsNullOrEmpty(selectedTabName))
            {
                return;
            }

            if (selectedTabName == tabRelation.Name)
            {
                if (AppManager.Navigation.ShowDialogWithParam<popupAddNewRelation>(new NavigationParameters(), ModeForm.New).Result != DialogResult.OK)
                {
                    return;
                }
            }
            else
            {
                if (AppManager.Navigation.ShowDialogWithParam<popupNRT, string>(selectedTabName, ModeForm.New).Result != DialogResult.OK)
                {
                    return;
                }
            }

            if (selectedTabName == tabNational.Name)
            {
                LoadDataToGrid<MNationality>(gridNational);
            }

            if (selectedTabName == tabReligion.Name)
            {
                LoadDataToGrid<MReligion>(gridReligion);
            }

            if (selectedTabName == tabTypeName.Name)
            {
                LoadDataToGrid<MTypeName>(gridTypeName);
            }

            if (selectedTabName == tabRelation.Name)
            {
                LoadDataMRelation();
            }
        }

        private void GridNational_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                gridNational.ClearSelection();

                ContextMenuStrip m = new ContextMenuStrip();

                int currentMouseOverRow = gridNational.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    gridNational.Rows[currentMouseOverRow].Selected = true;
                    if (gridNational.SelectedRows[0].DataBoundItem is MNationality objRowSelected)
                    {

                        //}
                        //var objRowSelected = gridNational.SelectedRows[0].DataBoundItem as MNationality;
                        //if (objRowSelected != null)
                        //{
                        var objData = AppManager.DBManager.GetTable<MNationality>().FirstOrDefault(i => i.Id == objRowSelected.Id);

                        if (objData != null)
                        {
                            // check isDefault
                            m.Items.Add(objData.IsDefault ? cstrSetNotDefaultText : cstrSetDefaultText);
                            m.Show(gridNational, new System.Drawing.Point(e.X, e.Y));
                            m.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenu_ItemClicked);
                        }
                    }
                }
            }
        }

        private void GridReligion_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                gridReligion.ClearSelection();

                ContextMenuStrip m = new ContextMenuStrip();

                int currentMouseOverRow = gridReligion.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    gridReligion.Rows[currentMouseOverRow].Selected = true;

                    if (gridReligion.SelectedRows[0].DataBoundItem is MReligion objRowSelected)
                    {
                        var objData = AppManager.DBManager.GetTable<MReligion>().FirstOrDefault(i => i.Id == objRowSelected.Id);

                        if (objData != null)
                        {
                            // check isDefault
                            m.Items.Add(objData.IsDefault ? cstrSetNotDefaultText : cstrSetDefaultText);
                            m.Show(gridReligion, new System.Drawing.Point(e.X, e.Y));
                            m.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenu_ItemClicked);
                        }
                    }
                }
            }
        }

        private void GridTypeName_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                gridTypeName.ClearSelection();

                ContextMenuStrip m = new ContextMenuStrip();

                int currentMouseOverRow = gridTypeName.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    gridTypeName.Rows[currentMouseOverRow].Selected = true;

                    if (gridTypeName.SelectedRows[0].DataBoundItem is MTypeName objRowSelected)
                    {
                        var objData = AppManager.DBManager.GetTable<MTypeName>().FirstOrDefault(i => i.Id == objRowSelected.Id);

                        if (objData != null)
                        {
                            // check isDefault
                            m.Items.Add(objData.IsDefault ? cstrSetNotDefaultText : cstrSetDefaultText);
                            m.Show(gridTypeName, new System.Drawing.Point(e.X, e.Y));
                            m.ItemClicked += new ToolStripItemClickedEventHandler(ContextMenu_ItemClicked);
                        }
                    }
                }
            }
        }

        private void BtnRestoreData_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = AppManager.Dialog.Warning("Khi dữ liệu được khôi phục thành công thì dữ liệu hiện tại sẽ bị mất hoàn toàn. Hãy chú ý trước khi khôi phục dữ liệu");
                if (dialogResult != DialogResult.OK)
                {
                    return;
                }
                var strPath = AppManager.Dialog.OpenFile("Gia pha File (*.gpb)|*.gpb");
                RestoreDataBase(strPath, false);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }

        private void DgvListVersion_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (dgvListVersion.Rows[e.RowIndex].DataBoundItem is Version rowSelected)
            {
                var strBackupFolder = Directory.GetCurrentDirectory() + AppConst.BackupDBPath;
                var fileBackup = strBackupFolder + "\\" + rowSelected.VersionName;
                if (e.ColumnIndex == colActionDownload.Index)
                {
                    var strPath = AppManager.Dialog.SaveFile("", "Gia pha new File (*.gp4b)|*.gp4b");
                    if (!string.IsNullOrEmpty(strPath))
                    {
                        File.Copy(fileBackup, strPath, true);
                        var file = new FileInfo(strPath);
                        if (file.Exists)
                        {
                            Process.Start(file.DirectoryName);
                        }
                    }
                }

                if (e.ColumnIndex == colActionRestore.Index)
                {
                    DialogResult dialogResult = AppManager.Dialog.Warning("Khi dữ liệu được khôi phục thành công thì dữ liệu hiện tại sẽ bị mất hoàn toàn. Hãy chú ý trước khi khôi phục dữ liệu");
                    if (dialogResult != DialogResult.OK)
                    {
                        return;
                    }
                    var strPath = AppManager.Dialog.OpenFile("Gia pha new File (*.gp4b)|*.gp4b");
                    RestoreDataBase(strPath, true);
                }

                if (e.ColumnIndex == colActionDelete.Index)
                {
                    //if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                    //{
                    //    return;
                    //}

                    //if (!File.Exists(fileBackup))
                    //{
                    //    return;
                    //}

                    //File.Delete(fileBackup);
                    //AppManager.Dialog.Ok("Xóa thành công");
                    //LoadListVersionBackup();
                }
            }
        }

        private void BtnRestoreNew_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = AppManager.Dialog.Warning("Khi dữ liệu được khôi phục thành công thì dữ liệu hiện tại sẽ bị mất hoàn toàn. Hãy chú ý trước khi khôi phục dữ liệu");
            if (dialogResult != DialogResult.OK)
            {
                return;
            }
            var strPath = AppManager.Dialog.OpenFile("Gia pha new File (*.gp4b)|*.gp4b");
            RestoreDataBase(strPath, true);
        }
        private void RestoreDataBase(string strPath, bool newDB = true)
        {
            string fileBackup = "";
            var flag = false;
            bool flagBackup = false;
            try
            {
                if (!string.IsNullOrEmpty(strPath))
                {
                    AppManager.Dialog.ShowProgressBar((progressBar) =>
                    {
                        progressBar.total = 2;
                        progressBar.count = 0;

                        this.Invoke(new Action(() =>
                        {
                            progressBar.Title = "Đamg sao lưu dữ liệu hiện tại...";
                        }));

                        flagBackup = FncBackupDBv2(null, out fileBackup);
                        progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);

                        this.Invoke(new Action(() =>
                        {
                            progressBar.Title = "Vui lòng đợi trong quá trình khôi phục...";
                        }));

                        DeleteAllAlbum();

                        DeleteAllDataInFolder(Directory.GetCurrentDirectory() + "\\Temp");

                        flag = newDB ? FncRestoreNewDBBackup(strPath, progressBar) : FncRestoreOldDBBackup(strPath, progressBar);

                        if (flag)
                        {
                            ResetBufferData();
                        }
                    }, "Vui lòng đợi trong quá trình khôi phục...", $"Khôi phục dữ liệu {(newDB ? "mới" : "cũ")}");

                    FinishRestoreData(flag, flagBackup, fileBackup);

                    AppManager.LoginUser = AppManager.DBManager.GetTable<MUser>().FirstOrDefault(i => i.UserName == AppManager.LoginUser.UserName && i.Password == AppManager.LoginUser.Password);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.WaitCursor;
                FncRestoreNewDBBackup(fileBackup, null);
                ResetBufferData();
                this.Cursor = Cursors.Default;

                AppManager.Dialog.Error("Khôi phục dữ liệu thất bại");

                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }

        private void FinishRestoreData(bool flag, bool flagBackup, string fileBackup)
        {

            if (flag)
            {
                if (AppManager.Dialog.Confirm("Bạn có muốn cập nhật lại thứ bậc?"))
                {
                    using MemberHelper memberHelper = new MemberHelper();

                    if (memberHelper.RootTMember != null)
                    {
                        AppManager.Dialog.ShowProgressBar(progressBar =>
                        {
                            memberHelper.UpdateLevelInFamily(progressBar);
                        }, "Đang cập nhật thứ bậc...", "Cập nhật thứ bậc");
                        AppManager.Dialog.Ok("Khôi phục dữ liệu thành công!");
                    }
                    else
                    {
                        AppManager.Dialog.Ok("Khôi phục dữ liệu thành công!\nXin vui lòng đặt tổ phụ để cập nhật thứ bậc!");
                    }

                }
                else
                {
                    AppManager.Dialog.Ok("Khôi phục dữ liệu thành công!");
                }
            }
            else
            {
                if (flagBackup)
                {
                    this.Cursor = Cursors.WaitCursor;
                    FncRestoreNewDBBackup(fileBackup, null);
                    ResetBufferData();
                    this.Cursor = Cursors.Default;
                }
                AppManager.Dialog.Error("Khôi phục dữ liệu thất bại");
            }
            if (File.Exists(fileBackup))
            {
                File.Delete(fileBackup);
            }
        }

        private void ResetBufferData()
        {
            AppManager.MenuMemberBuffer.ListAllMember.Clear();
            AppManager.MenuMemberBuffer.ListMember.Clear();
            AppManager.MenuMemberBuffer.MemberCurrent = null;

            var familyInfo = AppManager.DBManager.GetTable<MFamilyInfo>().FirstOrDefault();
            AppManager.LoginUser.FamilyId = familyInfo.Id;
            var mUser = AppManager.DBManager.GetTable<MUser>().UpdateOne(i => i.Id == AppManager.LoginUser.Id, AppManager.LoginUser);
        }

        private void DeleteAllAlbum()
        {
            string pathAlbum = Application.StartupPath + AppConst.AlbumImageFolderPath;
            if (!Directory.Exists(pathAlbum))
            {
                return;
            }
            DeleteAllDataInFolder(pathAlbum);
            string pathThumbnail = Application.StartupPath + AppConst.AlbumThumbnailFolderPath;
            DeleteAllDataInFolder(pathThumbnail);
        }

        private void BtnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                // version 1
                //var strPath = AppManager.Dialog.SaveFile("", "Gia pha new File (*.gp4b)|*.gp4b");

                //if (!string.IsNullOrWhiteSpace(strPath))
                //{
                //    if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                //    {
                //        return;
                //    }

                //    AppManager.Dialog.ShowProgressBar((obj) =>
                //    {
                //        var flag = fncBackupDB(strPath, obj);

                //        this.SafeInvoke(() =>
                //        {
                //            var rst = !flag ? AppManager.Dialog.Error("Sao lưu dữ liệu thất bại")
                //                            : AppManager.Dialog.Ok("Sao lưu dữ liệu thành công");
                //        });
                //    });
                //}

                // version 2
                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }

                AppManager.Dialog.ShowProgressBar((obj) =>
                {
                    var flag = FncBackupDBv2(obj, out string fileBackup);

                    this.SafeInvoke(() =>
                    {
                        var rst = !flag ? AppManager.Dialog.Error("Sao lưu dữ liệu thất bại")
                                        : AppManager.Dialog.Ok("Sao lưu dữ liệu thành công");
                        LoadListVersionBackup();
                    });
                }, "Vui lòng đợi trong quá trình sao lưu...", $"Đang sao lưu dữ liệu");
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }
        #endregion Event Form

        #region Private Function
        private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
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
                                var oldObjDataa = tblMNational.FirstOrDefault(i => i.IsDefault == true);
                                if (oldObjDataa != null)
                                {
                                    oldObjDataa.IsDefault = false;
                                    if (!tblMNational.UpdateOne(i => i.Id == oldObjDataa.Id, oldObjDataa))
                                    {
                                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                                        return;
                                    }
                                }

                                var objData = tblMNational.FirstOrDefault(i => i.Id == objRowSelected.Id);
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
                                var oldObjDataa = tblMReligion.FirstOrDefault(i => i.IsDefault == true);
                                if (oldObjDataa != null)
                                {
                                    oldObjDataa.IsDefault = false;
                                    if (!tblMReligion.UpdateOne(i => i.Id == oldObjDataa.Id, oldObjDataa))
                                    {
                                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                                        return;
                                    }
                                }

                                var objData = tblMReligion.FirstOrDefault(i => i.Id == objRowSelected.Id);
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
                                var oldObjDataa = tblTypeName.FirstOrDefault(i => i.IsDefault == true);
                                if (oldObjDataa != null)
                                {
                                    oldObjDataa.IsDefault = false;
                                    if (!tblTypeName.UpdateOne(i => i.Id == oldObjDataa.Id, oldObjDataa))
                                    {
                                        AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                                        return;
                                    }
                                }

                                var objData = tblTypeName.FirstOrDefault(i => i.Id == objRowSelected.Id);
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
                                var objData = tblMNational.FirstOrDefault(i => i.Id == objRowSelected.Id);
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
                                var objData = tblMReligion.FirstOrDefault(i => i.Id == objRowSelected.Id);
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
                                var objData = tblTypeName.FirstOrDefault(i => i.Id == objRowSelected.Id);
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
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }

        private void LoadDataToGrid<T>(DataGridTemplate gridTemplate) where T : BaseModel
        {
            var data = AppManager.DBManager.GetTable<T>().ToList();
            BindingHelper.BindingDataGrid(gridTemplate, data);
        }

        private void LoadDataMRelation()
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }

        private void LoadDataInfoVersion()
        {
            lblAppName.Text = string.Format("Tên phần mềm: {0}", AppManager.AppName);
            lblAppVersion.Text = string.Format("Version: {0} ({1})", AppManager.AppVersion, AppManager.AppCreateDate.ToString());
        }
        #endregion Private Function
        private void AddDir(ZipFile objZip, string strDir)
        {
            try
            {
                var objdir = new DirectoryInfo(strDir);
                AddRecusiveDir(objZip, objdir, "");
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }

        private void AddRecusiveDir(ZipFile objZip, DirectoryInfo objDir, string strFolderName)
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
                foreach (DirectoryInfo dir in objDir.GetDirectories())
                {
                    AddRecusiveDir(objZip, dir, strFolderName + "\\" + strSubFolderName + "\\");
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
            }
        }

        private bool FncRestoreOldDBBackup(string fileName, ProgressBarManager progressBar)
        {
            try
            {
                AppManager.isExecuting = true;

                if (string.IsNullOrEmpty(fileName))
                {
                    return false;
                }

                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Temp"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Temp");
                }

                string pathBackupCurrentData = Directory.GetCurrentDirectory() + "\\Temp\\Backup";
                if (!Directory.Exists(pathBackupCurrentData))
                {
                    Directory.CreateDirectory(pathBackupCurrentData);
                }

                string timeBackup = DateTime.Now.ToString("HHmmssddmmyyyy");

                var strExtractFolder = Directory.GetCurrentDirectory() + $"\\Temp\\Restore{timeBackup}";

                if (!Directory.Exists(strExtractFolder))
                {
                    Directory.CreateDirectory(strExtractFolder);
                }

                FastZip objZip = new FastZip()
                {
                    Password = "giaphabackup"
                };

                objZip.ExtractZip(fileName, strExtractFolder, "");

                var fileRestore = strExtractFolder + "\\Data\\giaphadb.mdb";
                var folderImageRestore = strExtractFolder + "\\images";
                var folderAvatarRawRestore = folderImageRestore + "\\avatar";
                var folderAvatarThumbnailRestore = folderAvatarRawRestore + "\\thumbnail";
                var folderAlbumRestore = folderImageRestore + "\\Album\\album";

                if (!Directory.Exists(folderImageRestore)) { Directory.CreateDirectory(folderImageRestore); }
                if (!Directory.Exists(folderAvatarRawRestore)) { Directory.CreateDirectory(folderAvatarRawRestore); }
                if (!Directory.Exists(folderAvatarThumbnailRestore)) { Directory.CreateDirectory(folderAvatarThumbnailRestore); }
                if (!Directory.Exists(folderAlbumRestore)) { Directory.CreateDirectory(folderAlbumRestore); }

                var sizeAvatarImage = new Size();
                using (var usrAddMember = new addMember(null, ModeForm.None))
                {
                    sizeAvatarImage = usrAddMember.avatarImage.Image.Size;
                }

                if (!File.Exists(fileRestore))
                {
                    return false;
                }

                using (var conection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source='" + fileRestore + "';Jet OLEDB:Database Password=giapha1712012"))
                {
                    conection.Open();

                    var data = conection.Query<TFMemberMain>("SELECT T_FMEMBER_MAIN.* FROM T_FMEMBER_MAIN").ToList();

                    progressBar.total += data.Count;

                    var mapId = new Dictionary<int, TMember>();

                    AppManager.LoggerApp.Debug(typeof(ConfigCommon), "thông tin thành viên!" + data.Count);

                    if (data != null)
                    {
                        var lstMember = new List<TMember>();
                        var lstMemberRelation = new List<TMemberRelation>();
                        var cnn = 0;

                        // save avatar image
                        var strAvatarFolder = Directory.GetCurrentDirectory() + AppConst.AvatarFolderPath;
                        var strAvatarFolderRaw = Directory.GetCurrentDirectory() + AppConst.AvatarRawPath;
                        var strAvatarFolderThumbnail = Directory.GetCurrentDirectory() + AppConst.AvatarThumbnailPath;

                        var strAlbumFolder = Directory.GetCurrentDirectory() + AppConst.AlbumFolderPath;
                        string dateBackup = DateTime.Now.ToString("ddMMyyy_hhmm");
                        string albumName = $"AlbumBackup{dateBackup}";
                        var strfolderImageAlbum = Directory.GetCurrentDirectory() + AppConst.AlbumImageFolderPath;
                        var strDetailAlbum = strfolderImageAlbum + albumName;
                        var strfolderThumbnailAlbum = Directory.GetCurrentDirectory() + AppConst.AlbumThumbnailFolderPath;

                        var strDocument = Path.Combine(Directory.GetCurrentDirectory(), AppConst.DocumentFilesPath);

                        // check if diẻctory folder backup empty
                        if (!Directory.Exists(strAvatarFolder))
                        {
                            Directory.CreateDirectory(strAvatarFolder);
                        }

                        if (!Directory.Exists(strAvatarFolderRaw))
                        {
                            Directory.CreateDirectory(strAvatarFolderRaw);
                        }

                        if (!Directory.Exists(strAvatarFolderThumbnail))
                        {
                            Directory.CreateDirectory(strAvatarFolderThumbnail);
                        }

                        if (!Directory.Exists(strAlbumFolder))
                        {
                            Directory.CreateDirectory(strAlbumFolder);
                        }

                        if (!Directory.Exists(strDetailAlbum))
                        {
                            Directory.CreateDirectory(strDetailAlbum);
                        }

                        if (!Directory.Exists(strfolderThumbnailAlbum))
                        {
                            Directory.CreateDirectory(strfolderThumbnailAlbum);
                        }

                        if (!Directory.Exists(strfolderImageAlbum))
                        {
                            Directory.CreateDirectory(strfolderImageAlbum);
                        }

                        // clear file in folder before restore new file
                        // raw
                        string[] fileRawArray = Directory.GetFiles(strAvatarFolderRaw);
                        if (fileRawArray.Length > 0)
                        {
                            foreach (var imgPath in fileRawArray)
                            {
                                File.Delete(imgPath);
                            }
                        }

                        // thumbnail
                        string[] fileThumbnailArray = Directory.GetFiles(strAvatarFolderThumbnail);
                        if (fileThumbnailArray.Length > 0)
                        {
                            foreach (var imgPath in fileThumbnailArray)
                            {
                                using (var fileStream = File.OpenRead(imgPath))
                                {
                                    Image image = Image.FromStream(fileStream);
                                    image.Dispose();
                                    fileStream.Close();
                                    fileStream.Dispose();
                                }
                                File.Delete(imgPath);
                            }
                        }

                        #region Copy Album Image
                        DeleteAllFile(strDetailAlbum);
                        DeleteAllFile(strfolderImageAlbum);
                        DeleteAllFile(strfolderThumbnailAlbum);

                        CopyAllFile(folderAlbumRestore, strDetailAlbum);
                        string[] arrTemp = Directory.GetFiles(strDetailAlbum);
                        if (arrTemp.Length > 0)
                        {
                            CreateAlbumInfo(strfolderImageAlbum, strfolderThumbnailAlbum, arrTemp[0], albumName);
                        }
                        #endregion

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
                            var emGender = (item.GENDER == 1) ? GPConst.EmGender.Male : ((item.GENDER == 2) ? GPConst.EmGender.FeMale : GPConst.EmGender.Unknown);
                            member.Gender = (int)emGender;
                            // HOMETOWN
                            member.BirthPlace = item.BIRTH_PLACE?.Trim();
                            member.National = item.NATIONALITY?.Trim();
                            member.Religion = item.RELIGION?.Trim();
                            if (int.TryParse(item.FAMILY_ORDER, out int childlevelInFamily))
                            {
                                member.ChildLevelInFamily = childlevelInFamily;
                            }
                            member.LevelInFamily = item.LEVEL;
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
                                        var objTMemberJob = new TMemberJob()
                                        {
                                            Id = LiteDBManager.CreateNewId(),
                                            CompanyName = objCareer.OFFICE_NAME?.Trim(),
                                            CompanyAddress = objCareer.OFFICE_PLACE?.Trim(),
                                            Job = objCareer.OCCUPATION?.Trim(),
                                            Position = objCareer.POSITION?.Trim()
                                        };
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
                                        var objTMemberSchool = new TMemberSchool()
                                        {
                                            Id = LiteDBManager.CreateNewId(),
                                            SchoolName = objCareer.OFFICE_NAME?.Trim(),
                                            Description = objCareer.REMARK?.Trim()
                                        };
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
                                    var objTMemberEvent = new TMemberEvent()
                                    {
                                        Id = LiteDBManager.CreateNewId(),
                                        EventName = objFact.FACT_NAME?.Trim(),
                                        Location = objFact.FACT_PLACE?.Trim(),
                                        Description = objFact.DESCRIPTION?.Trim()
                                    };
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
                                lstFMemberRelation = lstFMemberRelation.OrderBy(x => x.ROLE_ORDER).ToList();
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
                                        genderOfMEMBER_ID = (objMEMBER_ID.GENDER == 1) ? (int)GPConst.EmGender.Male : ((objMEMBER_ID.GENDER == 2) ? (int)GPConst.EmGender.FeMale : (int)GPConst.EmGender.Unknown);
                                    }
                                    int genderOfREL_FMEMBER_ID = 0;
                                    var objREL_FMEMBER_ID = conection.Query<TFMemberMain>("SELECT T_FMEMBER_MAIN.GENDER FROM T_FMEMBER_MAIN WHERE T_FMEMBER_MAIN.MEMBER_ID = " + obj.REL_FMEMBER_ID).FirstOrDefault();
                                    if (objREL_FMEMBER_ID != null)
                                    {
                                        genderOfREL_FMEMBER_ID = (objREL_FMEMBER_ID.GENDER == 1) ? (int)GPConst.EmGender.Male : ((objREL_FMEMBER_ID.GENDER == 2) ? (int)GPConst.EmGender.FeMale : (int)GPConst.EmGender.Unknown);
                                    }

                                    // vợ - chồng - 1 - 1
                                    if (obj.RELID == 1)
                                    {
                                        var newObjWH = new TMemberRelation()
                                        {
                                            Id = LiteDBManager.CreateNewId(),
                                            memberId = newMEMBER_ID,
                                            relMemberId = newREL_FMEMBER_ID
                                        };
                                        if (genderOfMEMBER_ID == (int)GPConst.EmGender.Male)
                                        {
                                            // vợ chồng
                                            newObjWH.relType = "WIF01";
                                        }
                                        if (genderOfMEMBER_ID == (int)GPConst.EmGender.FeMale)
                                        {
                                            // chồng vợ
                                            newObjWH.relType = "HUS01";
                                        }
                                        if (genderOfMEMBER_ID == (int)GPConst.EmGender.Unknown)
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
                                        var newObjRelationCP = new TMemberRelation()
                                        {
                                            Id = LiteDBManager.CreateNewId(),
                                            memberId = newMEMBER_ID,
                                            relMemberId = newREL_FMEMBER_ID
                                        };
                                        if (genderOfREL_FMEMBER_ID == (int)GPConst.EmGender.Male)
                                        {
                                            newObjRelationCP.relType = (obj.RELID == 4) ? "DAD02" : "DAD01";
                                        }
                                        if (genderOfREL_FMEMBER_ID == (int)GPConst.EmGender.FeMale)
                                        {
                                            newObjRelationCP.relType = (obj.RELID == 4) ? "MOM02" : "MOM01";
                                        }
                                        if (genderOfREL_FMEMBER_ID == (int)GPConst.EmGender.Unknown)
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
                                        var newObjRelationPC = new TMemberRelation()
                                        {
                                            Id = LiteDBManager.CreateNewId(),
                                            memberId = newREL_FMEMBER_ID,
                                            relMemberId = newMEMBER_ID
                                        };
                                        if (genderOfMEMBER_ID == (int)GPConst.EmGender.Male)
                                        {
                                            newObjRelationPC.relType = (obj.RELID == 4) ? "CHI02" : "CHI01";
                                        }
                                        if (genderOfMEMBER_ID == (int)GPConst.EmGender.FeMale)
                                        {
                                            newObjRelationPC.relType = (obj.RELID == 4) ? "CHI04" : "CHI03";
                                        }
                                        if (genderOfMEMBER_ID == (int)GPConst.EmGender.Unknown)
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

                            // avatar
                            //item.AVATAR_PATH
                            var oldImage = folderAvatarRawRestore + "\\" + item.AVATAR_PATH;
                            var objFile = new FileInfo(oldImage);
                            if (objFile.Exists)
                            {
                                // save raw image
                                var fileName = member.Id + objFile.Extension;
                                var destinationRaw = strAvatarFolderRaw + "\\" + fileName;
                                File.Copy(oldImage, destinationRaw, true);

                                // save thumbnail image
                                using (var thumbnailImg = FileHepler.ResizeImage(new Bitmap(destinationRaw), sizeAvatarImage.Width, sizeAvatarImage.Height))
                                {
                                    var destinationThumbnail = strAvatarFolderThumbnail + fileName;
                                    FileHepler.SaveImage(thumbnailImg, destinationThumbnail);
                                }

                                // make thumbnail for tree
                                using (var imgThumbTree = FileHepler.ResizeImage(new Bitmap(destinationRaw), 512))
                                {
                                    var destinationThumbnailTree = strAvatarFolderThumbnail + String.Format(AppConst.FormatNameThumbnailTree, fileName);
                                    FileHepler.SaveImage(imgThumbTree, destinationThumbnailTree);
                                }

                                member.AvatarImg = member.Id + objFile.Extension;
                            }

                            if (progressBar != null)
                            {
                                progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
                            }
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

                        // clear folder image temp
                        // raw TEMP
                        //string[] fileRawTempArray = Directory.GetFiles(folderAvatarRawRestore);
                        //if (fileRawTempArray.Length > 0)
                        //{
                        //    foreach (var imgPath in fileRawTempArray)
                        //    {
                        //        File.Delete(imgPath);
                        //    }
                        //}
                        //// thumbnail TEMP
                        //if (Directory.Exists(folderAvatarThumbnailRestore))
                        //{
                        //    string[] fileThumbnailTempArray = Directory.GetFiles(folderAvatarThumbnailRestore);
                        //    if (fileThumbnailTempArray.Length > 0)
                        //    {
                        //        foreach (var imgPath in fileThumbnailTempArray)
                        //        {
                        //            File.Delete(imgPath);
                        //        }
                        //    }
                        //}
                        DeleteAllDocument(strDocument);
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

        private bool CopyAllFile(string sourceFolder, string desFolder)
        {
            try
            {
                if (!Directory.Exists(sourceFolder))
                {
                    Directory.CreateDirectory(sourceFolder);
                }
                if (!Directory.Exists(desFolder))
                {
                    Directory.CreateDirectory(desFolder);
                }
                string[] sourceFilePath = Directory.GetFiles(sourceFolder);
                if (sourceFilePath.Length > 0)
                {
                    sourceFilePath.ToList().ForEach(x =>
                    {
                        string[] arrTemp = x.Split('\\');
                        File.Copy(x, Path.Combine(desFolder, arrTemp[arrTemp.Length - 1]), true);
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
                return false;
            }
        }

        private bool DeleteAllFile(string sourceFolder)
        {
            try
            {
                if (!Directory.Exists(sourceFolder))
                {
                    Directory.CreateDirectory(sourceFolder);
                    return true;
                }
                string[] allFile = Directory.GetFiles(sourceFolder);
                if (allFile.Length == 0) return true;
                allFile.ToList().ForEach(x => { File.Delete(x); });

                var lstAlbum = Directory.GetDirectories(sourceFolder);
                if (lstAlbum.Length > 0)
                {
                    lstAlbum.ToList().ForEach(x => { DeleteAllFile(x); });
                }
                return true;
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
                return false;
            }
        }

        private bool CreateAlbumInfo(string avartaPath, string avartaThumbPath, string filePath, string albumName)
        {
            try
            {
                FileInfo objFile = new FileInfo(filePath);
                TFamilyAlbum objAlbum = new TFamilyAlbum()
                {
                    Id = LiteDBManager.CreateNewId()
                };

                var fileName = objAlbum.Id + objFile.Extension;
                var destinationRaw = Path.Combine(avartaPath, fileName);
                File.Copy(filePath, destinationRaw, true);

                // save thumbnail image
                using (var stream = File.OpenRead(filePath))
                {
                    var thumbnailImg = Image.FromStream(stream);
                    var destinationThumbnail = Path.Combine(avartaThumbPath, fileName);
                    FileHepler.SaveImage(thumbnailImg, destinationThumbnail);
                }
                objAlbum.Thumbnail = fileName;
                objAlbum.AlbumName = albumName;
                objAlbum.AlbumDescriptrion = "Album ảnh được restore từ bản backup từ phần mềm cũ";
                objAlbum.CreateUser = AppManager.LoginUser.Id;
                if (AppManager.DBManager.DropTable<TFamilyAlbum>())
                {
                    var tblTFamilyAlbum = AppManager.DBManager.GetTable<TFamilyAlbum>();
                    var rst = tblTFamilyAlbum.InsertOne(objAlbum);
                    return rst;
                }
                return true;
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
                return false;
            }
        }

        private bool DeleteAllDocument(string sourcePath)
        {
            try
            {
                if (!Directory.Exists(sourcePath))
                {
                    Directory.CreateDirectory(sourcePath);
                    return true;
                }
                string[] allFiles = Directory.GetFiles(sourcePath);
                allFiles.ToList().ForEach(x =>
                {
                    File.Delete(x);
                });
                if (AppManager.DBManager.DropTable<TDocumentFile>())
                {
                    var objTDocumentFile = AppManager.DBManager.GetTable<TDocumentFile>();
                }
                return true;
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
                return false;
            }
        }

        private bool FncRestoreNewDBBackup(string fileName, ProgressBarManager progressBar)
        {
            try
            {
                AppManager.isExecuting = true;
                var cnn = 0;

                if (string.IsNullOrEmpty(fileName))
                {
                    return false;
                }

                FastZip objZip = null;

                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Temp"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Temp");
                }

                var strExtractFolder = Directory.GetCurrentDirectory() + $"\\Temp\\Restore{DateTime.Now.ToString("HHmmddmmyyyy")}";

                var strData = Directory.GetCurrentDirectory() + "\\Data\\giapha.db";
                var strAlbum = Directory.GetCurrentDirectory() + AppConst.AlbumFolderPath;
                var strImageAlbum = Directory.GetCurrentDirectory() + AppConst.AlbumImageFolderPath;
                var strThumbnailAlbum = Directory.GetCurrentDirectory() + AppConst.AlbumThumbnailFolderPath;

                if (!Directory.Exists(strExtractFolder))
                {
                    Directory.CreateDirectory(strExtractFolder);
                }

                var fileRestore = strExtractFolder + "\\Data\\giapha.db";

                var folderAvatar = strExtractFolder + "\\Data\\Avatar";
                var folderAvatarRaw = folderAvatar + "\\raw";
                var folderAvatarThumbnail = folderAvatar + "\\thumbnail";

                string folderImageAlbum = strExtractFolder + AppConst.AlbumImageFolderPath;
                string folderThumbnailAlbum = strExtractFolder + AppConst.AlbumThumbnailFolderPath;

                objZip = new FastZip()
                {
                    Password = "giaphabackup"
                };
                objZip.ExtractZip(fileName, strExtractFolder, "");

                var newAvatarPath = Directory.GetCurrentDirectory() + "\\Data\\Avatar";
                var newRawPath = newAvatarPath + "\\raw";
                var newThumbnailPath = newAvatarPath + "\\thumbnail";

                if (!File.Exists(fileRestore)) { return false; }
                if (!Directory.Exists(folderAvatar)) { Directory.CreateDirectory(folderAvatar); }
                if (!Directory.Exists(folderAvatarRaw)) { Directory.CreateDirectory(folderAvatarRaw); }
                if (!Directory.Exists(folderAvatarThumbnail)) { Directory.CreateDirectory(folderAvatarThumbnail); }
                if (!Directory.Exists(newAvatarPath)) { Directory.CreateDirectory(newAvatarPath); }
                if (!Directory.Exists(newRawPath)) { Directory.CreateDirectory(newRawPath); }
                if (!Directory.Exists(newThumbnailPath)) { Directory.CreateDirectory(newThumbnailPath); }

                if (!Directory.Exists(strAlbum)) { Directory.CreateDirectory(strAlbum); }
                if (!Directory.Exists(strImageAlbum)) { Directory.CreateDirectory(strImageAlbum); }
                if (!Directory.Exists(strThumbnailAlbum)) { Directory.CreateDirectory(strThumbnailAlbum); }

                // disconnect db
                AppManager.DBManager.Dispose();

                // copy & replace db
                File.Copy(fileRestore, strData, true);

                //copy album
                if (Directory.Exists(folderImageAlbum))
                {
                    string[] folders = Directory.GetDirectories(folderImageAlbum);
                    foreach (string folder in folders)
                    {
                        string[] temp = folder.Split('\\');
                        Directory.Move(folder, strImageAlbum + temp[temp.Length - 1]);
                    }

                    string[] files = Directory.GetFiles(folderImageAlbum);
                    foreach (string file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        File.Copy(file, strImageAlbum + fileInfo.Name, true);
                    }
                }

                if (Directory.Exists(folderThumbnailAlbum))
                {
                    string[] files = Directory.GetFiles(folderThumbnailAlbum);
                    foreach (string file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        File.Copy(file, strThumbnailAlbum + fileInfo.Name, true);
                    }
                }

                int countFile = 0;
                // clear file in folder avatar before restore NEW FILE
                // raw
                string[] fileRawArray = Directory.GetFiles(newRawPath);
                if (fileRawArray.Length > 0)
                {
                    countFile += fileRawArray.Length;
                }
                // thumbnail
                string[] fileThumbnailArray = Directory.GetFiles(newThumbnailPath);
                if (fileThumbnailArray.Length > 0)
                {
                    countFile += fileThumbnailArray.Length;
                }

                // clear old folder avatar in TEMP
                // raw
                string[] fileRawTempArray = Directory.GetFiles(folderAvatarRaw);
                if (fileRawTempArray.Length > 0)
                {
                    countFile += fileRawTempArray.Length;
                }
                // thumbnail
                string[] fileThumbnailTempArray = Directory.GetFiles(folderAvatarThumbnail);
                if (fileThumbnailTempArray.Length > 0)
                {
                    countFile += fileThumbnailTempArray.Length;
                }

                // copy & replace folder img
                string[] filesRaw = null;
                if (Directory.Exists(folderAvatarRaw))
                {
                    filesRaw = Directory.GetFiles(folderAvatarRaw);
                    if (filesRaw != null)
                    {
                        countFile += filesRaw.Length;
                    }
                }

                string[] filesThumbnail = null;
                if (Directory.Exists(folderAvatarThumbnail))
                {
                    filesThumbnail = Directory.GetFiles(folderAvatarThumbnail);
                    if (filesThumbnail != null)
                    {
                        countFile += filesThumbnail.Length;
                    }
                }

                int totalLoading = countFile + 1;

                // root raw folder
                if (fileRawArray.Length > 0)
                {
                    foreach (var imgPath in fileRawArray)
                    {
                        File.Delete(imgPath);
                    }
                }
                // root thumnail folder
                if (fileThumbnailArray.Length > 0)
                {
                    foreach (var imgPath in fileThumbnailArray)
                    {
                        File.Delete(imgPath);
                    }
                }

                if (filesRaw != null)
                {
                    foreach (var path in filesRaw)
                    {
                        var file = Regex.Match(path, @"([^\\]+$)");
                        File.Copy(path, newRawPath + "\\" + file, true);

                        if (progressBar != null)
                        {
                            progressBar.Percent = progressBar.fncCalculatePercent2(++cnn, totalLoading);
                        }
                    }
                }

                if (filesThumbnail != null)
                {
                    foreach (var path in filesThumbnail)
                    {
                        var file = Regex.Match(path, @"([^\\]+$)");
                        File.Copy(path, newThumbnailPath + "\\" + file, true);

                        if (progressBar != null)
                        {
                            progressBar.Percent = progressBar.fncCalculatePercent2(++cnn, totalLoading);
                        }
                    }
                }

                //// root temp raw folder
                //if (fileRawTempArray.Length > 0)
                //{
                //    foreach (var imgPath in fileRawTempArray)
                //    {
                //        File.Delete(imgPath);
                //    }
                //}
                //// root temp thumbnail folder
                //if (fileThumbnailTempArray.Length > 0)
                //{
                //    foreach (var imgPath in fileThumbnailTempArray)
                //    {
                //        File.Delete(imgPath);
                //    }
                //}

                if (progressBar != null)
                {
                    progressBar.Percent = progressBar.fncCalculatePercent2(++cnn, totalLoading);
                }

                return true;
            }
            catch (Exception ex)
            {
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
                return false;
            }
            finally
            {
                AppManager.isExecuting = false;
            }
        }

        private void DeleteAllDataInFolder(string folder)
        {
            try
            {
                string[] allfiles = Directory.GetFiles(folder);
                foreach (string file in allfiles)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
                string[] allFolder = Directory.GetDirectories(folder);
                foreach (string fol in allFolder)
                {
                    DeleteAllDataInFolder(fol);
                    Directory.Delete(fol);
                }
            }
            catch
            {
            }
        }

        private bool FncBackupDB(string fileName, ProgressBarManager progressBar)
        {
            try
            {
                AppManager.isExecuting = true;
                var cnn = 0;

                if (progressBar != null)
                {
                    progressBar.Percent = progressBar.fncCalculatePercent(cnn, 5);
                }
                var file = new FileInfo(fileName);
                cnn++;

                if (progressBar != null)
                {
                    progressBar.Percent = progressBar.fncCalculatePercent(cnn, 5);
                }
                // disconnect db
                AppManager.DBManager.Dispose();
                cnn++;

                if (progressBar != null)
                {
                    progressBar.Percent = progressBar.fncCalculatePercent(cnn, 5);
                }

                var strData = Directory.GetCurrentDirectory() + "\\Data\\";

                using (var objZip = ZipFile.Create(file.FullName))
                {
                    cnn++;

                    if (progressBar != null)
                    {
                        progressBar.Percent = progressBar.fncCalculatePercent(cnn, 5);
                    }
                    objZip.BeginUpdate();
                    objZip.Password = "giaphabackup";
                    AddDir(objZip, strData);
                    cnn++;

                    if (progressBar != null)
                    {
                        progressBar.Percent = progressBar.fncCalculatePercent(cnn, 5);
                    }

                    objZip.CommitUpdate();
                    cnn++;

                    if (progressBar != null)
                    {
                        progressBar.Percent = progressBar.fncCalculatePercent(cnn, 5);
                    }
                }

                Process.Start(file.DirectoryName);
                return true;
            }
            catch (Exception ex)
            {
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
                return false;
            }
            finally
            {
                AppManager.isExecuting = false;
            }
        }

        private bool FncBackupDBv2(ProgressBarManager progressBar, out string fileBackup)
        {
            try
            {
                AppManager.isExecuting = true;
                if (progressBar != null)
                {
                    progressBar.total = 6;
                    progressBar.count = 0;
                }

                bool backupOK = false;

                if (progressBar != null)
                {
                    progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
                }

                var strBackupFolder = Directory.GetCurrentDirectory() + AppConst.BackupDBPath;

                // check if diẻctory folder backup empty
                if (!Directory.Exists(strBackupFolder))
                {
                    Directory.CreateDirectory(strBackupFolder);
                }

                if (progressBar != null)
                {
                    progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
                }

                var newDateTime = DateTime.Now.ToString("yyyyMMddHHmmssffffff");
                var fileName = strBackupFolder + "\\" + newDateTime + ".gp4b";
                fileBackup = fileName;

                if (progressBar != null)
                {
                    progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
                }

                AppManager.isExecuting = true;

                AppManager.DBManager.Dispose();

                var strData = Directory.GetCurrentDirectory() + "\\Data\\";

                using (var objZip = ZipFile.Create(fileName))
                {
                    if (progressBar != null)
                    {
                        progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
                    }

                    objZip.BeginUpdate();
                    objZip.Password = "giaphabackup";
                    AddDir(objZip, strData);

                    if (progressBar != null)
                    {
                        progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
                    }

                    objZip.CommitUpdate();
                    backupOK = true;
                    if (progressBar != null)
                    {
                        progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
                    }
                }
                return backupOK;
            }
            catch (Exception ex)
            {
                AppManager.LoggerApp.Error(typeof(ConfigCommon), ex);
                fileBackup = "";
                return false;
            }
            finally
            {
                AppManager.isExecuting = false;
            }
        }
        private void LoadListVersionBackup()
        {
            var strBackupFolder = Directory.GetCurrentDirectory() + AppConst.BackupDBPath;
            var files = Directory.GetFiles(strBackupFolder);
            if (files != null)
            {
                var lstVersion = new List<Version>();
                foreach (var file in files)
                {
                    var objFile = new FileInfo(file);
                    lstVersion.Add(new Version
                    {
                        VersionName = objFile.Name,
                        TimeCreate = objFile.CreationTime,
                        Size = FileHepler.SizeSuffix(objFile.Length)
                    });
                }
                BindingHelper.BindingDataGrid(dgvListVersion, lstVersion);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (AppManager.isHasVersion())
            {
                if (!AppManager.Dialog.Confirm("Phần mềm có phiên bản cập nhật mới.\nBạn có muốn cập nhập phần mềm?"))
                {
                    return;
                }
                AppManager.UpdateApp();
            }
            else
            {
                AppManager.Dialog.Ok("Phần mềm không có phiên bản cập nhật mới.");
            }
        }

        private void BtnRestore_Click(object sender, EventArgs e)
        {
            AppManager.RestoreVersion();
        }

        private class Version
        {
            public string VersionName { get; set; }
            public DateTime TimeCreate { get; set; }
            public string Size { get; set; }
        }
    }
}