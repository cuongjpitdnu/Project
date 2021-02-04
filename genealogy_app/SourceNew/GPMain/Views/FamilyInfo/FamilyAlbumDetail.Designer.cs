namespace GPMain.Views.FamilyInfo
{
    partial class FamilyAlbumDetail
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblnamealbum = new MaterialSkin.Controls.MaterialLabel();
            this.ckselectall = new MaterialSkin.Controls.MaterialCheckbox();
            this.btndeleteimage = new MaterialSkin.Controls.MaterialButton();
            this.materialButton1 = new MaterialSkin.Controls.MaterialButton();
            this.flowImage = new System.Windows.Forms.FlowLayoutPanel();
            this.lblpage = new MaterialSkin.Controls.MaterialLabel();
            this.lbldescriptionalbum = new MaterialSkin.Controls.MaterialLabel();
            this.lblnote = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblnamealbum
            // 
            this.lblnamealbum.AutoSize = true;
            this.lblnamealbum.Depth = 0;
            this.lblnamealbum.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblnamealbum.Location = new System.Drawing.Point(23, 7);
            this.lblnamealbum.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblnamealbum.Name = "lblnamealbum";
            this.lblnamealbum.Size = new System.Drawing.Size(47, 19);
            this.lblnamealbum.TabIndex = 0;
            this.lblnamealbum.Text = "Album";
            // 
            // ckselectall
            // 
            this.ckselectall.AutoSize = true;
            this.ckselectall.Depth = 0;
            this.ckselectall.Location = new System.Drawing.Point(15, 59);
            this.ckselectall.Margin = new System.Windows.Forms.Padding(0);
            this.ckselectall.MouseLocation = new System.Drawing.Point(-1, -1);
            this.ckselectall.MouseState = MaterialSkin.MouseState.HOVER;
            this.ckselectall.Name = "ckselectall";
            this.ckselectall.Ripple = true;
            this.ckselectall.Size = new System.Drawing.Size(161, 37);
            this.ckselectall.TabIndex = 2;
            this.ckselectall.Text = "Chọn toàn bộ ảnh";
            this.ckselectall.UseVisualStyleBackColor = true;
            this.ckselectall.CheckedChanged += new System.EventHandler(this.ckselectall_CheckedChanged);
            this.ckselectall.Click += new System.EventHandler(this.ckselectall_Click);
            // 
            // btndeleteimage
            // 
            this.btndeleteimage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btndeleteimage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btndeleteimage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btndeleteimage.Depth = 0;
            this.btndeleteimage.DrawShadows = true;
            this.btndeleteimage.HighEmphasis = true;
            this.btndeleteimage.Icon = null;
            this.btndeleteimage.Location = new System.Drawing.Point(728, 55);
            this.btndeleteimage.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btndeleteimage.MouseState = MaterialSkin.MouseState.HOVER;
            this.btndeleteimage.Name = "btndeleteimage";
            this.btndeleteimage.Size = new System.Drawing.Size(80, 36);
            this.btndeleteimage.TabIndex = 1;
            this.btndeleteimage.Text = "XÓA ẢNH";
            this.btndeleteimage.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btndeleteimage.UseAccentColor = false;
            this.btndeleteimage.UseVisualStyleBackColor = true;
            this.btndeleteimage.Click += new System.EventHandler(this.btndeleteimage_Click);
            // 
            // materialButton1
            // 
            this.materialButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.materialButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.materialButton1.Depth = 0;
            this.materialButton1.DrawShadows = true;
            this.materialButton1.HighEmphasis = true;
            this.materialButton1.Icon = null;
            this.materialButton1.Location = new System.Drawing.Point(611, 55);
            this.materialButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.Size = new System.Drawing.Size(93, 36);
            this.materialButton1.TabIndex = 1;
            this.materialButton1.Text = "THÊM ẢNH";
            this.materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton1.UseAccentColor = false;
            this.materialButton1.UseVisualStyleBackColor = true;
            this.materialButton1.Click += new System.EventHandler(this.materialButton1_Click);
            // 
            // flowImage
            // 
            this.flowImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowImage.BackColor = System.Drawing.Color.MintCream;
            this.flowImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowImage.ForeColor = System.Drawing.Color.LightGray;
            this.flowImage.Location = new System.Drawing.Point(23, 98);
            this.flowImage.Name = "flowImage";
            this.flowImage.Padding = new System.Windows.Forms.Padding(10);
            this.flowImage.Size = new System.Drawing.Size(785, 440);
            this.flowImage.TabIndex = 4;
            this.flowImage.SizeChanged += new System.EventHandler(this.flowImage_SizeChanged);
            // 
            // lblpage
            // 
            this.lblpage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblpage.AutoSize = true;
            this.lblpage.Depth = 0;
            this.lblpage.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblpage.Location = new System.Drawing.Point(736, 548);
            this.lblpage.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblpage.Name = "lblpage";
            this.lblpage.Size = new System.Drawing.Size(72, 19);
            this.lblpage.TabIndex = 3;
            this.lblpage.Text = "Trang 1/1";
            // 
            // lbldescriptionalbum
            // 
            this.lbldescriptionalbum.AutoSize = true;
            this.lbldescriptionalbum.Depth = 0;
            this.lbldescriptionalbum.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbldescriptionalbum.Location = new System.Drawing.Point(23, 35);
            this.lbldescriptionalbum.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbldescriptionalbum.Name = "lbldescriptionalbum";
            this.lbldescriptionalbum.Size = new System.Drawing.Size(42, 19);
            this.lbldescriptionalbum.TabIndex = 0;
            this.lbldescriptionalbum.Text = "Mô tả";
            // 
            // lblnote
            // 
            this.lblnote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblnote.AutoSize = true;
            this.lblnote.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblnote.ForeColor = System.Drawing.Color.Red;
            this.lblnote.Location = new System.Drawing.Point(18, 548);
            this.lblnote.Name = "lblnote";
            this.lblnote.Size = new System.Drawing.Size(306, 19);
            this.lblnote.TabIndex = 5;
            this.lblnote.Text = "* Nháy đúp lên ảnh để xem đúng kích thước";
            // 
            // FamilyAlbumDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MintCream;
            this.Controls.Add(this.lblnote);
            this.Controls.Add(this.flowImage);
            this.Controls.Add(this.lblpage);
            this.Controls.Add(this.materialButton1);
            this.Controls.Add(this.btndeleteimage);
            this.Controls.Add(this.ckselectall);
            this.Controls.Add(this.lbldescriptionalbum);
            this.Controls.Add(this.lblnamealbum);
            this.MinimumSize = new System.Drawing.Size(830, 574);
            this.Name = "FamilyAlbumDetail";
            this.Size = new System.Drawing.Size(830, 574);
            this.Load += new System.EventHandler(this.FamilyAlbumDetail_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel lblnamealbum;
        private MaterialSkin.Controls.MaterialCheckbox ckselectall;
        private MaterialSkin.Controls.MaterialButton btndeleteimage;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private System.Windows.Forms.FlowLayoutPanel flowImage;
        private MaterialSkin.Controls.MaterialLabel lblpage;
        private MaterialSkin.Controls.MaterialLabel lbldescriptionalbum;
        private System.Windows.Forms.Label lblnote;
    }
}
