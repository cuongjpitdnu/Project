using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GPMain.Common;
using GPMain.Common.Navigation;
using System.IO;
using GPMain.Common.Dialog;
using GPMain.Properties;
using GPModels;
using GPMain.Common.Database;
using GPMain.Common.Helper;
using System.Diagnostics;

namespace GPMain.Views.FamilyInfo
{
    public partial class FamilyDocument : BaseUserControl
    {
        public const string KEY_DATA = "DATA_LIST";
        public const string KEY_ID = "ID_ITEM";
        public const string KEY_PATHFILE = "PATHFILE";
        private string _fileId = "";
        private string _pathDb = "";
        private string _pathFile = "";
        private List<ExTDocumentFile> _lstFileDocument = null;
        public FamilyDocument(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            TitleBar = mode == ModeForm.Edit ? "Chỉnh sửa tài liệu" : "Thêm mới tài liệu";

            this.BackColor = AppConst.PopupBackColor;

            _lstFileDocument = Params.GetValue<List<ExTDocumentFile>>(KEY_DATA, new List<ExTDocumentFile>());
            _fileId = Params.GetValue<string>(KEY_ID, "") ?? "";
            _pathFile = Params.GetValue<string>(KEY_PATHFILE, "") ?? "";
            if (_fileId != "")
            {
                var item = _lstFileDocument.FirstOrDefault(i => i.Id == _fileId);
                txtPath.Text = _pathFile;
                txtDocName.Text = item.FileName;
                txtIntroDoc.Text = item.FileIntroduce;
            }

            // txtPath.Text = null;
        }

        private void btnUploadFile_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = DialogManager.DIALOG_FILTER_ALL;
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var pathFileUp = dialog.FileName; // get name of file
                txtPath.Text = pathFileUp;
                txtDocName.Text = Path.GetFileNameWithoutExtension(pathFileUp);
            }

        }

        private void btnSaveFile_Click(object sender, EventArgs e)
        {


            var path = txtPath.Text;
            var fileName = txtDocName.Text;
            var fileIntro = txtIntroDoc.Text;
            if (path == "" && _fileId == "")
            {
                MessageBox.Show(Resources.MSG_PATH_FAILD_NULL);
                return;
            }
            if (fileName == "")
            {
                MessageBox.Show(Resources.MSG_FILE_NAME_NULL);
                return;
            }
            var pathSource = AppConst.DocumentFilesPath;
            try
            {

                var tblTDocument = AppManager.DBManager.GetTable<TDocumentFile>();

                var objFile = new TDocumentFile();
                objFile.Id = (_fileId == "") ? LiteDBManager.CreateNewId() : _fileId;
                objFile.FileName = fileName.Trim();
                objFile.FileIntroduce = fileIntro.Trim();
                if (!Directory.Exists(pathSource))
                {
                    Directory.CreateDirectory(pathSource);
                }
                var timeFile = DateTime.Now.ToString("yyyyMMddTHHmmss");
                if (_fileId == "")
                {
                    var fileNameSave = pathSource + fileName + timeFile + Path.GetExtension(path);
                    objFile.PathFile = fileNameSave.Trim();
                    File.Copy(path, fileNameSave, true);
                }
                else
                {
                    if (path != "")
                    {
                        if (File.Exists(_pathDb))
                        {
                            File.Delete(_pathDb);
                        }
                        var fileNameSave = pathSource + fileName + timeFile + Path.GetExtension(path);
                        objFile.PathFile = fileNameSave.Trim();
                        File.Copy(path, fileNameSave, true);
                    }

                }
                var rst = (_fileId == "") ? tblTDocument.InsertOne(objFile)
                                      : tblTDocument.UpdateOne(i => i.Id == _fileId, objFile);

                if (rst == false)
                {
                    if (_fileId == "")
                        AppManager.Dialog.Error(Resources.MSG_FILE_INSERT_FAILD);
                    else
                        AppManager.Dialog.Error(Resources.MSG_FILE_UPDATE_FAILD);
                    return;
                }
                else
                {
                    if (_fileId == "")
                        AppManager.Dialog.Ok(Resources.MSG_FILE_INSERT_SUCCESS);
                    else
                        AppManager.Dialog.Ok(Resources.MSG_FILE_UPDATE_SUCCESS);

                    var item = _lstFileDocument.FirstOrDefault(i => i.Id == _fileId);
                    if (item != null)
                    {
                        item.FileName = objFile.FileName;
                        item.FileIntroduce = objFile.FileIntroduce;
                        item.PathFile = objFile.PathFile;
                    }
                    else
                    {
                        item = new ExTDocumentFile();
                        item.STT = _lstFileDocument.Count + 1;
                        item.Id = objFile.Id;
                        item.FileName = objFile.FileName;
                        item.FileIntroduce = objFile.FileIntroduce;
                        item.PathFile = objFile.PathFile;
                        _lstFileDocument.Add(item);

                    }

                }

                this.Close(_lstFileDocument);
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(FamilyDocument), ex);
            }
        }

    }
}
