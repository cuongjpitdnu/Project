using GPMain.Common;
using GPMain.Views.FamilyInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPMain.Views
{
    public partial class ShowImageAlbum : Form
    {
        int indexImage = 0;
        string[] files;
        public ShowImageAlbum(ImageInfo _imageInfo)
        {
            InitializeComponent();
            imageInfo = _imageInfo;
        }
        ImageInfo imageInfo { get; set; }
        private void ShowImageAlbum_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(Application.StartupPath + AppConst.AlbumImageFolderPath + imageInfo.Album))
            {
                files = Directory.GetFiles(Application.StartupPath + AppConst.AlbumImageFolderPath + imageInfo.Album);
                indexImage = files.ToList().IndexOf(imageInfo.PathFile);
                if (indexImage != -1)
                {
                    ShowImage();
                }
            }
        }

        private void btnpre_Click(object sender, EventArgs e)
        {
            if (indexImage > 0)
            {
                indexImage--;
                ShowImage();
            }
        }

        private void ShowImage()
        {
            if (indexImage != -1 && indexImage < files.Length)
            {
                using (var stream = File.OpenRead(files[indexImage]))
                {
                    Image img = Image.FromStream(stream);
                    if (img.Width > pictureBox1.Width || img.Height > pictureBox1.Height)
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    else
                        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                    pictureBox1.Image = img;
                }
                this.Text ="Album ảnh "+ imageInfo.Album + " - " + GetFileName(files[indexImage]);
            }
        }

        private string GetFileName(string pathFile)
        {
            string[] temp = pathFile.Split('\\');
            return temp[temp.Length - 1];
        }

        private void btnnext_Click(object sender, EventArgs e)
        {
            if (indexImage < files.Length - 1)
            {
                indexImage++;
                ShowImage();
            }
        }

        private void btnsaveimage_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();           
            var temp = files[indexImage].Split('\\');
            sfd.FileName = temp[temp.Length - 1];
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.Copy(files[indexImage], sfd.FileName);
            }
        }

        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            flowLayoutPanel1.Left = (panel1.Width - flowLayoutPanel1.Width) / 2;
        }
    }
}
