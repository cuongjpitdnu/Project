using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPModels;
using GPMain.Common.Helper;
using GPMain.Properties;
using System.Diagnostics;
using System.IO;
using GPMain.Common.Navigation;
using GPMain.Common.Dialog;
using GPMain.Core;
using GPMain.Common;
using ICSharpCode.SharpZipLib.Zip;
using GPMain.Views.Config;
using System.IO.Compression;

namespace GPMain.Views.FamilyInfo
{
    public partial class FamilyDocumentList : UserControl
    {
        // private string _fileId = "";
        //  private string _pathDb = "";
        private List<ExTDocumentFile> _lstFileDocument = new List<ExTDocumentFile>();
        public FamilyDocumentList()
        {
            InitializeComponent();
            LoadDataFileDocument();

            dgv_lisDocument.SetColumnDownloadAction<ExTDocumentFile>(ProcessDownload);
            dgv_lisDocument.SetColumnEditAction<ExTDocumentFile>(ProcessEdit);
            dgv_lisDocument.SetColumnDeleteAction<ExTDocumentFile>(ProcessDelete);
        }

        private void LoadDataFileDocument()
        {
            try
            {
                dgv_lisDocument.AllowUserToAddRows = false;
                var dataDoc = AppManager.DBManager.GetTable<TDocumentFile>();

                var cnn = 0;
                var dataShow = dataDoc.AsEnumerable().Select(i => new ExTDocumentFile
                {
                    STT = ++cnn,
                    Id = i.Id,
                    FileName = i.FileName,
                    FileIntroduce = i.FileIntroduce,
                    PathFile = i.PathFile
                }).ToList();
                _lstFileDocument = dataShow;
                BindingHelper.BindingDataGrid(dgv_lisDocument, _lstFileDocument);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(FamilyDocumentList), ex);
            }
        }

        private void ProcessDownload(ExTDocumentFile exTDocumentFile)
        {
            if (exTDocumentFile == null)
            {
                return;
            }

            var pathFileSave = AppManager.Dialog.SaveFile(exTDocumentFile.FileName, DialogManager.DIALOG_FILTER_EXCEL);
            if (pathFileSave != "")
            {
                try
                {
                    File.Copy(exTDocumentFile.PathFile, pathFileSave, true);
                    AppManager.Dialog.Ok(Resources.MSG_DOWNLOAD_SUCCESS);
                }
                catch (Exception ex)
                {
                    AppManager.Dialog.Error(ex.Message);
                    AppManager.LoggerApp.Error(typeof(FamilyDocumentList), ex);
                }

            }
        }
        private void ProcessEdit(ExTDocumentFile rowSelected)
        {
            var param = new NavigationParameters();
            param.Add(FamilyDocument.KEY_PATHFILE, rowSelected.PathFile);
            param.Add(FamilyDocument.KEY_DATA, _lstFileDocument);
            param.Add(FamilyDocument.KEY_ID, rowSelected.Id);


            var result = AppManager.Navigation.ShowDialogWithParam<FamilyDocument>(param, ModeForm.Edit, AppConst.StatusBarColor).GetValue<List<ExTDocumentFile>>();
            if (result != null)
            {
                BindingHelper.BindingDataGrid(dgv_lisDocument, result);
            }
        }

        private void ProcessDelete(ExTDocumentFile rowSelected)
        {
            if (!AppManager.Dialog.Confirm(Resources.MSG_CONFIRM_DELETE_FILE))
            {
                return;
            }

            try
            {
                var dataFile = AppManager.DBManager.GetTable<TDocumentFile>();
                var objFile = dataFile.CreateQuery(i => i.Id == rowSelected.Id).FirstOrDefault();
                if (objFile == null)
                {
                    return;
                }

                // set date delete
                objFile.DeleteDate = DateTime.Now;

                if (dataFile.UpdateOne(i => i.Id == objFile.Id, objFile) == false)
                {
                    return;
                }
                var pathDel = rowSelected.PathFile;
                if (File.Exists(rowSelected.PathFile))
                {
                    File.Delete(rowSelected.PathFile);
                }
                AppManager.Dialog.Ok(Resources.MSG_FILE_UPDATE_SUCCESS);
                var item = _lstFileDocument.Find(i => i.Id == rowSelected.Id);
                _lstFileDocument.Remove(item);
                var cnn = 0;
                var dataShow = _lstFileDocument.Select(i => new ExTDocumentFile
                {
                    STT = ++cnn,
                    Id = i.Id,
                    FileName = i.FileName,
                    FileIntroduce = i.FileIntroduce,
                    PathFile = i.PathFile
                }).ToList();
                _lstFileDocument = dataShow;
                BindingHelper.BindingDataGrid(dgv_lisDocument, _lstFileDocument);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(FamilyDocumentList), ex);
            }
        }

        private void dgv_lisDocument_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            var rowSelected = dgv_lisDocument.GetSelectedGridRowData<ExTDocumentFile>();

            if (rowSelected == null)
            {
                return;
            }

            if (e.ColumnIndex == download.Index && e.RowIndex >= 0)
            {
                var pathFileSave = AppManager.Dialog.SaveFile(rowSelected.FileName, DialogManager.DIALOG_FILTER_EXCEL);
                if (pathFileSave != "")
                {
                    try
                    {
                        File.Copy(rowSelected.PathFile, pathFileSave, true);
                        AppManager.Dialog.Ok(Resources.MSG_DOWNLOAD_SUCCESS);
                    }
                    catch (Exception ex)
                    {
                        AppManager.Dialog.Error(ex.Message);
                        AppManager.LoggerApp.Error(typeof(FamilyDocumentList), ex);
                    }

                }
            }
        }

        private void btnAddDoc_Click(object sender, EventArgs e)
        {
            var param = new NavigationParameters();
            param.Add(FamilyDocument.KEY_DATA, _lstFileDocument);

            var result = AppManager.Navigation
                                   .ShowDialogWithParam<FamilyDocument>(param, ModeForm.New, AppConst.StatusBarColor)
                                   .GetValue<List<ExTDocumentFile>>();
            if (result != null)
            {
                BindingHelper.BindingDataGrid(dgv_lisDocument, result);
            }
        }

        private void btnDownloadAll_Click(object sender, EventArgs e)
        {
            if (!AppManager.Dialog.Confirm(Resources.MSG_CONFIRM_DOWNLOAD_FILE_ALL))
            {
                return;
            }

            try
            {
                string directory = AppConst.DocumentFilesPath;
                string[] files = Directory.GetFiles(directory);
                string zipFile = AppManager.Dialog.SaveFile(lblHeader.Text, DialogManager.DIALOG_FILTER_ZIP);

                using (ZipOutputStream zipOut = new ZipOutputStream(File.Create(zipFile)))
                {
                    foreach (string file in files)
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        ZipEntry entry = new ZipEntry(fileInfo.Name);
                        FileStream fileStream = File.OpenRead(file);
                        entry.IsUnicodeText = true;

                        byte[] buffer = new byte[Convert.ToInt32(fileStream.Length)];
                        fileStream.Read(buffer, 0, (int)fileStream.Length);
                        entry.DateTime = fileInfo.LastWriteTime;
                        entry.Size = fileStream.Length;
                        fileStream.Close();

                        zipOut.PutNextEntry(entry);
                        zipOut.Write(buffer, 0, buffer.Length);
                    }

                    zipOut.Finish();
                    zipOut.Close();
                }

                AppManager.Dialog.Ok(Resources.MSG_DOWNLOAD_SUCCESS);
                Process.Start(zipFile);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(FamilyDocumentList), ex);
            }
        }
    }
}
