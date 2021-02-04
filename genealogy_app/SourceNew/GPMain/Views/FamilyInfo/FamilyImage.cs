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
using OpenTK.Graphics.ES10;
using GPModels;
using System.IO;

namespace GPMain.Views.FamilyInfo
{
    public partial class FamilyImage : BaseUserControl
    {
        private List<TFamilyAlbum> _lstTFamilyAlbum = new List<TFamilyAlbum>();
        private const int MARGIN = 110;
        bool bLoadingAlbumComplete = true;
        public FamilyImage()
        {
            InitializeComponent();
        }

        private void FamilyImage_Load(object sender, EventArgs e)
        {
            xLoadListAlbum();
        }

        private void btnAddNewAlbum_Click(object sender, EventArgs e)
        {
            AppManager.Navigation.ShowDialog<AddNewAlbum>(ModeForm.New, AppConst.StatusBarColor);
            xLoadListAlbum();
        }

        private void xLoadListAlbum()
        {
            //if (!bLoadingAlbumComplete) return;
            bLoadingAlbumComplete = false;
            flpListAlbum.Controls.Clear();
            var db = AppManager.DBManager.GetTable<TFamilyAlbum>();
            _lstTFamilyAlbum = db.ToList();
            if (_lstTFamilyAlbum.Count == 0) return;
            ItemFamilyImage[] arrItem = new ItemFamilyImage[_lstTFamilyAlbum.Count];
            int cnt = 0;
            int numItem = flpListAlbum.Height / (115);
            foreach (var objFamily in _lstTFamilyAlbum)
            {
                var itemFamilyImage = new ItemFamilyImage(objFamily);
                itemFamilyImage.Margin = new Padding(5);
                itemFamilyImage.Width = flpListAlbum.Width - itemFamilyImage.Margin.Left * 2 - 3 - (_lstTFamilyAlbum.Count > numItem ? AppConst.ScrollWidth : 0);
                itemFamilyImage.EditClick += albumFamilySua_Click;
                itemFamilyImage.DetailClick += itemFamilyImage_Click;
                itemFamilyImage.DeleteClick += DeleteAlbum_Click;
                arrItem[cnt] = itemFamilyImage;
                cnt++;
            }
            flpListAlbum.SuspendLayout();
            flpListAlbum.Controls.AddRange(arrItem);
            flpListAlbum.ResumeLayout();
            bLoadingAlbumComplete = true;
        }

        private void itemFamilyImage_Click(object sender, TFamilyAlbum familyAlbum)
        {
            AppManager.Navigation.ShowDialogWithParam<FamilyAlbumDetail, TFamilyAlbum>(familyAlbum, ModeForm.View, AppConst.StatusBarColor);
            xLoadListAlbum();
        }
        private void albumFamilySua_Click(object sender, TFamilyAlbum familyAlbum)
        {
            if (familyAlbum == null)
            {
                return;
            }
            AppManager.Navigation.ShowDialogWithParam<AddNewAlbum, TFamilyAlbum>(familyAlbum, ModeForm.Edit, AppConst.StatusBarColor);
            xLoadListAlbum();
        }
        private void DeleteAlbum_Click(object sender, TFamilyAlbum objAlbum)
        {
            if (!AppManager.Dialog.Confirm($"Bạn có chắc chắn xóa album {objAlbum.AlbumName}")) return;
            objAlbum.DeleteDate = DateTime.Now;
            var tblTFamilyAlbum = AppManager.DBManager.GetTable<TFamilyAlbum>();
            bool bUpdate = tblTFamilyAlbum.UpdateOne(i => i.Id == objAlbum.Id, objAlbum);
            if (!bUpdate)
            {
                AppManager.Dialog.Error("Xóa album ảnh không thành công!");
                return;
            }

            string pathThumbnail = Application.StartupPath + AppConst.AlbumThumbnailFolderPath + objAlbum.Thumbnail;
            if (File.Exists(pathThumbnail))
            {
                try
                {
                    File.Delete(pathThumbnail);
                }
                catch { }
            }

            string pathAvarta = Application.StartupPath + AppConst.AlbumImageFolderPath + objAlbum.Thumbnail;
            if (File.Exists(pathAvarta))
            {
                try
                {
                    File.Delete(pathAvarta);
                }
                catch { }
            }

            string pathAlbum = Application.StartupPath + AppConst.AlbumImageFolderPath + objAlbum.AlbumName;
            if (Directory.Exists(pathAlbum))
            {
                string[] allImage = Directory.GetFiles(pathAlbum);
                foreach (string image in allImage)
                {
                    try
                    {
                        File.Delete(image);
                    }
                    catch { }
                }
                Directory.Delete(pathAlbum);
            }
            AppManager.Dialog.Ok($"Xóa album ảnh {objAlbum.AlbumName} thành công!");
            xLoadListAlbum();
        }

        private void plListAlbum_SizeChanged(object sender, EventArgs e)
        {
            xLoadListAlbum();
        }
    }
}
