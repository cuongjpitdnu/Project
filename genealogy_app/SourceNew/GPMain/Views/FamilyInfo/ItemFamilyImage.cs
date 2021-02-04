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
using GPMain.Common;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace GPMain.Views.FamilyInfo
{
    public partial class ItemFamilyImage : MaterialSkin.Controls.MaterialCard
    {
        public string strPath = Application.StartupPath;
        public string strAlbumFolderPath = Application.StartupPath + AppConst.AlbumFolderPath;
        public string strThumbnailFolderPath = Application.StartupPath + AppConst.AlbumThumbnailFolderPath;
        public string strImageFolderPath = Application.StartupPath + AppConst.AlbumImageFolderPath;

        public event EventHandler<TFamilyAlbum> DetailClick;
        public event EventHandler<TFamilyAlbum> EditClick;
        public event EventHandler<TFamilyAlbum> DeleteClick;

        public ItemFamilyImage()
        {
            InitializeComponent();
        }
        public ItemFamilyImage(TFamilyAlbum objAlbum)
        {
            InitializeComponent();
            TFamilyAlbum = objAlbum;
            lblAlbumName.Text = "Album: " + objAlbum.AlbumName;
            txtAlbumDes.Text = objAlbum.AlbumDescriptrion;
            txtAlbumDes.ReadOnly = true;
            lblSua.Tag = objAlbum.Id;
            lblChiTiet.Tag = objAlbum.Id;

            if (File.Exists(strThumbnailFolderPath + @"\" + objAlbum.Thumbnail))
            {
                var img = new Bitmap(strThumbnailFolderPath + @"\" + objAlbum.Thumbnail);
                Image imgNew = Image.FromHbitmap(img.GetHbitmap());
                pictureBox1.Image = imgNew;
                pictureBox1.Tag = objAlbum.Thumbnail;
                img.Dispose();
            }
        }

        private TFamilyAlbum _TFamilyAlbum;

        public TFamilyAlbum TFamilyAlbum { get => _TFamilyAlbum; set => _TFamilyAlbum = value; }

        private void lblChiTiet_Click(object sender, EventArgs e)
        {
            if (DetailClick != null)
            {
                DetailClick?.Invoke(sender, TFamilyAlbum);
            }
        }

        private void lblSua_Click(object sender, EventArgs e)
        {
            if (EditClick != null)
            {
                EditClick?.Invoke(sender, TFamilyAlbum);
            }
        }

        private void lblxoa_Click(object sender, EventArgs e)
        {
            if (DeleteClick != null)
            {
                DeleteClick?.Invoke(sender, TFamilyAlbum);
            }
        }
    }
}
