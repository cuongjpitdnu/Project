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

namespace GPMain.Views.FamilyInfo
{
    public partial class ImageTemplate : MaterialSkin.Controls.MaterialCard
    {
        public event EventHandler<ImageInfo> Selected;
        public event EventHandler<ImageInfo> ImageDoubleClick;
        public ImageTemplate()
        {
            InitializeComponent();
        }
        public bool Checked
        {
            get
            {
                return ckselect.Checked;
            }
            set
            {
                ckselect.Checked = value;
            }
        }

        private ImageInfo _ImageInfo;
        public ImageInfo ImageInfo
        {
            get => _ImageInfo;
            set
            {
                _ImageInfo = value;
                if (_ImageInfo != null)
                {
                    if (_ImageInfo.Image.Width > pictureBox1.Width || _ImageInfo.Image.Height > pictureBox1.Height)
                    {
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                    }
                    else
                    {
                        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                    }
                    pictureBox1.Image = _ImageInfo.Image;
                }
            }
        }

        private void ckselect_Click(object sender, EventArgs e)
        {
            if (Selected != null)
            {
                Selected?.Invoke(sender, ImageInfo);
            }
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (ImageDoubleClick != null)
            {
                ImageDoubleClick?.Invoke(sender, ImageInfo);
            }
        }

        public bool DeleteImage()
        {
            pictureBox1.Image = null;
            ImageInfo.Image = null;
            return true;
        }
    }
    public class ImageInfo
    {
        public string Album { get; set; }
        public string PathFile { get; set; }
        public Image Image { get; set; }
    }
}
