namespace GPMain.Views.FamilyInfo
{
    partial class AddNewAlbum
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.btnSelectImg = new MaterialSkin.Controls.MaterialButton();
            this.btnSaveAlbum = new MaterialSkin.Controls.MaterialButton();
            this.avatarImage = new System.Windows.Forms.PictureBox();
            this.txtAlbumDes = new MaterialSkin.Controls.MaterialTextBox();
            this.txtAlbumName = new MaterialSkin.Controls.MaterialTextBox();
            this.openFileImage = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.avatarImage)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MintCream;
            this.panel1.Controls.Add(this.materialLabel1);
            this.panel1.Controls.Add(this.btnSelectImg);
            this.panel1.Controls.Add(this.btnSaveAlbum);
            this.panel1.Controls.Add(this.avatarImage);
            this.panel1.Controls.Add(this.txtAlbumDes);
            this.panel1.Controls.Add(this.txtAlbumName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(445, 410);
            this.panel1.TabIndex = 0;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(165, 180);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(90, 19);
            this.materialLabel1.TabIndex = 5;
            this.materialLabel1.Text = "Ảnh đại diện";
            // 
            // btnSelectImg
            // 
            this.btnSelectImg.AutoSize = false;
            this.btnSelectImg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSelectImg.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectImg.Depth = 0;
            this.btnSelectImg.DrawShadows = true;
            this.btnSelectImg.HighEmphasis = true;
            this.btnSelectImg.Icon = null;
            this.btnSelectImg.Location = new System.Drawing.Point(262, 172);
            this.btnSelectImg.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSelectImg.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSelectImg.Name = "btnSelectImg";
            this.btnSelectImg.Size = new System.Drawing.Size(84, 37);
            this.btnSelectImg.TabIndex = 4;
            this.btnSelectImg.Text = "Chọn ảnh";
            this.btnSelectImg.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnSelectImg.UseAccentColor = true;
            this.btnSelectImg.UseVisualStyleBackColor = true;
            this.btnSelectImg.Click += new System.EventHandler(this.btnSelectImg_Click);
            // 
            // btnSaveAlbum
            // 
            this.btnSaveAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAlbum.AutoSize = false;
            this.btnSaveAlbum.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSaveAlbum.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveAlbum.Depth = 0;
            this.btnSaveAlbum.DrawShadows = true;
            this.btnSaveAlbum.HighEmphasis = true;
            this.btnSaveAlbum.Icon = null;
            this.btnSaveAlbum.Location = new System.Drawing.Point(345, 354);
            this.btnSaveAlbum.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSaveAlbum.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSaveAlbum.Name = "btnSaveAlbum";
            this.btnSaveAlbum.Size = new System.Drawing.Size(78, 38);
            this.btnSaveAlbum.TabIndex = 3;
            this.btnSaveAlbum.Text = "Lưu";
            this.btnSaveAlbum.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnSaveAlbum.UseAccentColor = true;
            this.btnSaveAlbum.UseVisualStyleBackColor = true;
            this.btnSaveAlbum.Click += new System.EventHandler(this.btnSaveAlbum_Click);
            // 
            // avatarImage
            // 
            this.avatarImage.Image = global::GPMain.Properties.Resources.no_avata;
            this.avatarImage.Location = new System.Drawing.Point(21, 165);
            this.avatarImage.Name = "avatarImage";
            this.avatarImage.Size = new System.Drawing.Size(100, 83);
            this.avatarImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.avatarImage.TabIndex = 2;
            this.avatarImage.TabStop = false;
            // 
            // txtAlbumDes
            // 
            this.txtAlbumDes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAlbumDes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAlbumDes.Depth = 0;
            this.txtAlbumDes.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtAlbumDes.Hint = "Mô tả";
            this.txtAlbumDes.Location = new System.Drawing.Point(21, 94);
            this.txtAlbumDes.MaxLength = 50;
            this.txtAlbumDes.ModeNumber_Maximum = 999999;
            this.txtAlbumDes.MouseState = MaterialSkin.MouseState.OUT;
            this.txtAlbumDes.Multiline = false;
            this.txtAlbumDes.Name = "txtAlbumDes";
            this.txtAlbumDes.Size = new System.Drawing.Size(402, 50);
            this.txtAlbumDes.TabIndex = 1;
            this.txtAlbumDes.Text = "";
            // 
            // txtAlbumName
            // 
            this.txtAlbumName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAlbumName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAlbumName.Depth = 0;
            this.txtAlbumName.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtAlbumName.Hint = "Tên album ảnh*";
            this.txtAlbumName.Location = new System.Drawing.Point(21, 21);
            this.txtAlbumName.MaxLength = 50;
            this.txtAlbumName.ModeNumber_Maximum = 999999;
            this.txtAlbumName.MouseState = MaterialSkin.MouseState.OUT;
            this.txtAlbumName.Multiline = false;
            this.txtAlbumName.Name = "txtAlbumName";
            this.txtAlbumName.Size = new System.Drawing.Size(402, 50);
            this.txtAlbumName.TabIndex = 0;
            this.txtAlbumName.Text = "";
            // 
            // openFileImage
            // 
            this.openFileImage.FileName = "openFileImage";
            // 
            // AddNewAlbum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(445, 410);
            this.Name = "AddNewAlbum";
            this.Sizable = false;
            this.Size = new System.Drawing.Size(445, 410);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.avatarImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MaterialSkin.Controls.MaterialTextBox txtAlbumName;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialButton btnSelectImg;
        private MaterialSkin.Controls.MaterialButton btnSaveAlbum;
        private System.Windows.Forms.PictureBox avatarImage;
        private MaterialSkin.Controls.MaterialTextBox txtAlbumDes;
        private System.Windows.Forms.OpenFileDialog openFileImage;
    }
}
