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
using GPModels;
using System.IO;

namespace GPMain.Views.FamilyInfo
{
    public partial class FamilyAlbumDetail : BaseUserControl
    {

        TFamilyAlbum familyAlbum { get; set; }
        string[] files;
        List<string> fileselected = new List<string>();
        int page { get; set; } = 0;
        int maxPage { get; set; }
        int oldPage = 0;
        public FamilyAlbumDetail(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();

            TitleBar = "Danh sách album ảnh";

            this.BackColor = AppConst.PopupBackColor;
            flowImage.BackColor = AppConst.PopupBackColor;

            familyAlbum = parameters.GetValue<TFamilyAlbum>();
            lblnamealbum.Text = $"Tên album ảnh: {familyAlbum.AlbumName}";
            lbldescriptionalbum.Text = $"Mô tả: {familyAlbum.AlbumDescriptrion}";
            flowImage.Controls.Clear();
            flowImage.MouseWheel += (sender, e) =>
            {
                if (e.Delta < 0)
                {
                    if (page < maxPage - 1)
                    {
                        page++;
                    }
                }
                else if (e.Delta > 0)
                {
                    if (page > 0)
                    {
                        page--;
                    }
                }
                if (page != oldPage)
                {
                    flowImage.Controls.Clear();
                    LoadAlbum(flowImage, 185, 200, out int totalMember);
                    oldPage = page;
                }
            };
            LoadAlbum(flowImage, 185, 200, out int totalMember);
        }

        private void LoadAlbum(FlowLayoutPanel flowLayoutPanel, int widthControl, int heightControl, out int numControlInFlowLayoutPanel)
        {
            try
            {
                if (!Directory.Exists(Application.StartupPath + AppConst.AlbumImageFolderPath + familyAlbum.AlbumName))
                {
                    Directory.CreateDirectory(Application.StartupPath + AppConst.AlbumImageFolderPath + familyAlbum.AlbumName);
                }
                files = Directory.GetFiles(Application.StartupPath + AppConst.AlbumImageFolderPath + familyAlbum.AlbumName);
                int numUSX = (flowLayoutPanel.Width - flowLayoutPanel.Padding.Right * 2) / (widthControl + flowLayoutPanel.Padding.Right * 2);
                int numUSY = (flowLayoutPanel.Height - flowLayoutPanel.Padding.Bottom * 2) / (heightControl/* + flowLayoutPanel.Padding.Bottom * 2*/);
                int numTemp = (flowLayoutPanel.Height - flowLayoutPanel.Padding.Bottom * 2) % (heightControl + flowLayoutPanel.Padding.Bottom * 2);
                int numUS = numUSX * numUSY;
                maxPage = files.Length / numUS + (files.Length % numUS > 0.1 ? 1 : 0);
                lblpage.Text = maxPage > 0 ? $"Trang {page + 1}/{maxPage}" : "";
                flowLayoutPanel.Controls.Clear();
                int eidx = page * numUS + (numTemp > 0.1 ? 1 : 0) + numUS + numUSX * (numUS > 0 ? 1 : 0);
                eidx = eidx > files.Length ? files.Length : eidx;
                int sidx = page * numUS;
                int numImage = eidx - sidx;
                ImageTemplate[] imageTemplates = new ImageTemplate[numImage];
                for (int i = sidx; i < eidx; i++)
                {
                    imageTemplates[i - sidx] = GetImage(files[i]);
                };
                flowImage.Controls.AddRange(imageTemplates);
                int widthUS = widthControl * numUSX + (numUSX - 1) * flowLayoutPanel.Padding.Right;
                var pad = flowLayoutPanel.Padding;
                pad.Left = (flowLayoutPanel.Width - widthUS) / 2;
                flowLayoutPanel.Padding = pad;
                numControlInFlowLayoutPanel = numUS;
            }
            catch
            {
                numControlInFlowLayoutPanel = 0;
            }
        }

        private ImageTemplate GetImage(string pathFile)
        {
            ImageTemplate ret = new ImageTemplate();
            using (var stream = File.OpenRead(pathFile))
            {
                Image img = Image.FromStream(stream);
                ImageTemplate imageTemplate = new ImageTemplate()
                {
                    ImageInfo = new ImageInfo()
                    {
                        Album = familyAlbum.AlbumName,
                        Image = img,
                        PathFile = pathFile
                    }
                };
                if (fileselected.Count > 0 && fileselected.Contains(pathFile))
                {
                    imageTemplate.Checked = true;
                }
                imageTemplate.Selected += ImageSelected_Event;
                imageTemplate.ImageDoubleClick += ImageDoubleClick_Event;
                return imageTemplate;
            }
        }


        private void ckselectall_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ImageSelected_Event(object sender, ImageInfo imageInfo)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.Checked)
            {
                fileselected.Add(imageInfo.PathFile);
            }
            else
            {
                fileselected.Remove(imageInfo.PathFile);
            }
            ckselectall.Checked = fileselected.Count == files.Length;//!flowImage.Controls.Cast<ImageTemplate>().ToList().Exists(x => !x.Checked);
        }

        private void ckselectall_Click(object sender, EventArgs e)
        {
            fileselected = ckselectall.Checked ? files.ToList() : new List<string>();
            flowImage.Controls.Cast<ImageTemplate>().ToList().ForEach(img => { img.Checked = ckselectall.Checked; });
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG|*.png|JPG|*.jpg|JPEG|*.jpeg|All|*.*";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (!Directory.Exists(Application.StartupPath + AppConst.AlbumImageFolderPath + familyAlbum.AlbumName))
                {
                    Directory.CreateDirectory(Application.StartupPath + AppConst.AlbumImageFolderPath + familyAlbum.AlbumName);
                }
                try
                {
                    var lstFile = ofd.FileNames;
                    var lstFileName = ofd.SafeFileNames;
                    int cnt = 0;
                    foreach (string file in lstFile)
                    {
                        if (File.Exists(Application.StartupPath + AppConst.AlbumImageFolderPath + familyAlbum.AlbumName + "\\" + lstFileName[cnt]))
                        {
                            MessageBox.Show("Ảnh đã tồn tại trong album ảnh.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        File.Copy(file, Application.StartupPath + AppConst.AlbumImageFolderPath + familyAlbum.AlbumName + "\\" + lstFileName[cnt]);
                        cnt++;
                    }
                    LoadAlbum(flowImage, 185, 200, out int totalMember);
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void btndeleteimage_Click(object sender, EventArgs e)
        {
            if (ckselectall.Checked)
            {
                string[] files = Directory.GetFiles(Application.StartupPath + AppConst.AlbumImageFolderPath + familyAlbum.AlbumName);
                flowImage.Controls.Clear();
                foreach (string file in files)
                {
                    File.Delete(file);
                }
                ckselectall.Checked = false;
            }
            else
            {
                flowImage.Controls.Cast<ImageTemplate>().Where(temp => temp.Checked).ToList().ForEach(temp =>
                  {
                      if (temp.DeleteImage())
                      {
                          flowImage.Controls.Remove(temp);
                      }
                      try
                      {
                          File.Delete(temp.ImageInfo.PathFile);
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  });
            }
        }

        private void ImageDoubleClick_Event(object sender, ImageInfo imageInfo)
        {
            ShowImageAlbum showImageAlbum = new ShowImageAlbum(imageInfo);
            showImageAlbum.ShowDialog();
        }

        private void flowImage_SizeChanged(object sender, EventArgs e)
        {
            int numTemp = (flowImage.Height - flowImage.Padding.Bottom * 2) % (200 + flowImage.Padding.Bottom * 2);
            int numUSX = (flowImage.Width - flowImage.Padding.Right * 2) / (185 + flowImage.Padding.Right * 2);
            int numUSY = (flowImage.Height - flowImage.Padding.Bottom * 2) / (200 + flowImage.Padding.Bottom * 2) + (numTemp > 0.1 ? 1 : 0);
            int numUS = numUSX * numUSY;
            if (numUS != flowImage.Controls.Count)
            {
                page = 0;
                flowImage.Controls.Clear();
                LoadAlbum(flowImage, 185, 200, out int totalMember);
            }
        }

        private void FamilyAlbumDetail_Load(object sender, EventArgs e)
        {

        }
    }
}
