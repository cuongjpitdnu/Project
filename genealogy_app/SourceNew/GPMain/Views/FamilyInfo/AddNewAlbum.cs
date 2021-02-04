using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using GPMain.Common;
using GPMain.Common.Navigation;
using GPModels;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using GPMain.Common.Helper;
using GPMain.Common.Database;

namespace GPMain.Views.FamilyInfo
{
    public partial class AddNewAlbum : BaseUserControl
    {
        public string strPath = Application.StartupPath;
        public string strAlbumFolderPath = Application.StartupPath + AppConst.AlbumFolderPath;
        public string strThumbnailFolderPath = Application.StartupPath + AppConst.AlbumThumbnailFolderPath;
        public string strImageFolderPath = Application.StartupPath + AppConst.AlbumImageFolderPath;
        private string _mstrImagePath;

        private string OldAlbumName { get; set; }
        public AddNewAlbum(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            TitleBar = "Thêm album ảnh";

            this.BackColor = AppConst.PopupBackColor;

            if (this.Mode == ModeForm.Edit)
            {
                var objTFamilyAlbum = this.Params.GetValue<TFamilyAlbum>();
                txtAlbumName.Text = OldAlbumName = objTFamilyAlbum.AlbumName;
                txtAlbumDes.Text = objTFamilyAlbum.AlbumDescriptrion;
                avatarImage.Tag = objTFamilyAlbum.Thumbnail;

                if (File.Exists(strThumbnailFolderPath + @"\" + objTFamilyAlbum.Thumbnail))
                {
                    using (var img = new Bitmap(strThumbnailFolderPath + @"\" + objTFamilyAlbum.Thumbnail))
                    {
                        Image imgNew = Image.FromHbitmap(img.GetHbitmap());
                        avatarImage.Image = imgNew;
                        avatarImage.Tag = objTFamilyAlbum.Thumbnail;
                        img.Dispose();
                    }
                }
                //avatarImage.Image = new Bitmap(strThumbnailFolderPath + @"\" + objTFamilyAlbum.Thumbnail);
            }
        }

        private void btnSelectImg_Click(object sender, EventArgs e)
        {
            var strPath = AppManager.Dialog.OpenFile("Images File (*.jpg;*.png)|*.jpg;*.png;");

            var file = !string.IsNullOrEmpty(strPath) ? new FileInfo(strPath) : null;

            if (file == null || !file.Exists)
            {
                return;
            }

            if (avatarImage.Image != null)
            {
                avatarImage.Image.Dispose();
                avatarImage.Image = null;
            }

            _mstrImagePath = strPath;
            using (var stream = File.OpenRead(_mstrImagePath))
            {
                Bitmap image = new Bitmap(stream);
                avatarImage.Image = FileHepler.ResizeImage(image, avatarImage.Width, avatarImage.Height);
            }
        }

        private void btnSaveAlbum_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtAlbumName.Text))
                {
                    AppManager.Dialog.Warning("Vui lòng nhập tên album!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(_mstrImagePath) && this.Mode != ModeForm.Edit)
                {
                    AppManager.Dialog.Warning("Vui lòng chọn ảnh đại diện!");
                    return;
                }

                if (!AppManager.Dialog.Confirm("Bạn đã chắc chắn chưa?"))
                {
                    return;
                }



                TFamilyAlbum objAlbum;
                bool actionInsert = (this.Mode == ModeForm.Edit) ? false : true;

                var tblTFamilyAlbum = AppManager.DBManager.GetTable<TFamilyAlbum>();
                if (actionInsert)
                {
                    objAlbum = new TFamilyAlbum();
                    objAlbum.Id = LiteDBManager.CreateNewId();
                }
                else
                {
                    var objParam = this.Params.GetValue<TFamilyAlbum>();
                    objAlbum = tblTFamilyAlbum.CreateQuery(i => i.Id == objParam.Id).FirstOrDefault();
                    if (objAlbum == null)
                    {
                        return;
                    }
                }

                // avatar
                string oldImg = "";
                if (!string.IsNullOrEmpty(_mstrImagePath))
                {
                    var objFile = new FileInfo(_mstrImagePath);
                    if (objFile.Exists)
                    {
                        oldImg = objAlbum.Thumbnail;
                        objAlbum.Thumbnail = objAlbum.Id + objFile.Extension;
                    }
                }

                if (!string.IsNullOrEmpty(_mstrImagePath))
                {
                    var objFile = new FileInfo(_mstrImagePath);
                    if (objFile.Exists)
                    {
                        // check if diẻctory folder backup empty
                        if (!Directory.Exists(strAlbumFolderPath))
                        {
                            Directory.CreateDirectory(strAlbumFolderPath);
                        }

                        if (!Directory.Exists(strThumbnailFolderPath))
                        {
                            Directory.CreateDirectory(strThumbnailFolderPath);
                        }

                        if (!Directory.Exists(strImageFolderPath))
                        {
                            Directory.CreateDirectory(strImageFolderPath);
                        }



                        // delete old img
                        if (!string.IsNullOrEmpty(oldImg))
                        {
                            // raw
                            var oldRawPath = strImageFolderPath + oldImg;
                            var objOldRawFile = new FileInfo(oldRawPath);
                            if (objOldRawFile.Exists)
                            {
                                File.Delete(oldRawPath);
                            }
                            objOldRawFile = null;
                            // thumbnail

                            var oldThumbnailPath = strThumbnailFolderPath + oldImg;
                            var objOldThumbnailFile = new FileInfo(oldThumbnailPath);

                            if (objOldThumbnailFile.Exists)
                            {
                                File.Delete(oldThumbnailPath);
                            }
                            objOldThumbnailFile = null;
                        }

                        // save raw image
                        var fileName = objAlbum.Id + objFile.Extension;
                        var destinationRaw = strImageFolderPath + fileName;

                        if (File.Exists(destinationRaw))
                        {
                            File.Delete(destinationRaw);
                        }

                        File.Copy(_mstrImagePath, destinationRaw, true);

                        // save thumbnail image
                        var thumbnailImg = avatarImage.Image;
                        var destinationThumbnail = strThumbnailFolderPath + fileName;
                        FileHepler.SaveImage(thumbnailImg, destinationThumbnail);

                    }
                }
                objAlbum.AlbumName = txtAlbumName.Text.Trim();
                objAlbum.AlbumDescriptrion = txtAlbumDes.Text.Trim();
                objAlbum.CreateUser = AppManager.LoginUser.Id;
                List<TAlbumImage> lstImg = new List<TAlbumImage>() {
                    new TAlbumImage{Name = "Test1.jpg", Descriptrion = "Test1", CreateUser = AppManager.LoginUser.Id},
                    new TAlbumImage{Name = "Test2.jpg", Descriptrion = "Test2", CreateUser = AppManager.LoginUser.Id},
                    new TAlbumImage{Name = "Test3.jpg", Descriptrion = "Test3", CreateUser = AppManager.LoginUser.Id},
                    new TAlbumImage{Name = "Test4.jpg", Descriptrion = "Test4", CreateUser = AppManager.LoginUser.Id},
                    new TAlbumImage{Name = "Test5.jpg", Descriptrion = "Test5", CreateUser = AppManager.LoginUser.Id},
                    new TAlbumImage{Name = "Test6.jpg", Descriptrion = "Test6", CreateUser = AppManager.LoginUser.Id},

                };
                objAlbum.Images = lstImg;

                var rst = actionInsert ? tblTFamilyAlbum.InsertOne(objAlbum)
                                       : tblTFamilyAlbum.UpdateOne(i => i.Id == objAlbum.Id, objAlbum);
                if (rst == false)
                {
                    AppManager.Dialog.Error("Cập nhập thông tin thất bại!");
                    return;
                }
                string newAlbum = Application.StartupPath + AppConst.AlbumImageFolderPath + txtAlbumName.Text;
                if (actionInsert)
                {
                    if (!Directory.Exists(newAlbum))
                    {
                        Directory.CreateDirectory(newAlbum);
                    }
                }
                else
                {
                    if (!Directory.Exists(newAlbum))
                    {
                        string oldDirect = Application.StartupPath + AppConst.AlbumImageFolderPath + OldAlbumName;
                        if (oldDirect != newAlbum)
                        {
                            Directory.Move(oldDirect, newAlbum);
                        }
                        else
                        {
                            Directory.CreateDirectory(newAlbum);
                        }
                    }
                }
                this.Close();
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(AddNewAlbum), ex);
            }
        }
    }
}
