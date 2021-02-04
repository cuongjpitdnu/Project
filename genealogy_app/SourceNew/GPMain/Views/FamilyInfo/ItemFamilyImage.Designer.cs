using System.Windows.Forms;

namespace GPMain.Views.FamilyInfo
{
    partial class ItemFamilyImage
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblAlbumName = new MaterialSkin.Controls.MaterialLabel();
            this.txtAlbumDes = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            this.lblSua = new MaterialSkin.Controls.MaterialLabel();
            this.lblChiTiet = new MaterialSkin.Controls.MaterialLabel();
            this.lblxoa = new MaterialSkin.Controls.MaterialLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::GPMain.Properties.Resources.no_avata;
            this.pictureBox1.Location = new System.Drawing.Point(12, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 83);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.DoubleClick += new System.EventHandler(this.lblChiTiet_Click);
            this.Cursor = Cursors.Hand;
            // 
            // lblAlbumName
            // 
            this.lblAlbumName.AutoSize = true;
            this.lblAlbumName.Depth = 0;
            this.lblAlbumName.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAlbumName.FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle1;
            this.lblAlbumName.Location = new System.Drawing.Point(118, 10);
            this.lblAlbumName.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblAlbumName.Name = "lblAlbumName";
            this.lblAlbumName.Size = new System.Drawing.Size(77, 19);
            this.lblAlbumName.TabIndex = 2;
            this.lblAlbumName.Text = "Tên album";
            // 
            // txtAlbumDes
            // 
            this.txtAlbumDes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtAlbumDes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAlbumDes.Depth = 0;
            this.txtAlbumDes.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAlbumDes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAlbumDes.Hint = "";
            this.txtAlbumDes.Location = new System.Drawing.Point(118, 44);
            this.txtAlbumDes.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtAlbumDes.ReadOnly = true;
            this.txtAlbumDes.Name = "txtAlbumDes";
            this.txtAlbumDes.Size = new System.Drawing.Size(402, 49);
            this.txtAlbumDes.TabIndex = 3;
            this.txtAlbumDes.Text = "";
            // 
            // lblSua
            // 
            this.lblSua.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSua.AutoSize = true;
            this.lblSua.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblSua.Depth = 0;
            this.lblSua.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblSua.Location = new System.Drawing.Point(576, 12);
            this.lblSua.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblSua.Name = "lblSua";
            this.lblSua.Size = new System.Drawing.Size(30, 19);
            this.lblSua.TabIndex = 4;
            this.lblSua.Text = "Sửa";
            this.lblSua.Click += new System.EventHandler(this.lblSua_Click);
            // 
            // lblChiTiet
            // 
            this.lblChiTiet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblChiTiet.AutoSize = true;
            this.lblChiTiet.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblChiTiet.Depth = 0;
            this.lblChiTiet.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblChiTiet.Location = new System.Drawing.Point(556, 73);
            this.lblChiTiet.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblChiTiet.Name = "lblChiTiet";
            this.lblChiTiet.Size = new System.Drawing.Size(50, 19);
            this.lblChiTiet.TabIndex = 5;
            this.lblChiTiet.Text = "Chi tiết";
            this.lblChiTiet.Click += new System.EventHandler(this.lblChiTiet_Click);
            // 
            // lblxoa
            // 
            this.lblxoa.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblxoa.AutoSize = true;
            this.lblxoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblxoa.Depth = 0;
            this.lblxoa.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblxoa.Location = new System.Drawing.Point(576, 42);
            this.lblxoa.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblxoa.Name = "lblxoa";
            this.lblxoa.Size = new System.Drawing.Size(29, 19);
            this.lblxoa.TabIndex = 4;
            this.lblxoa.Text = "Xóa";
            this.lblxoa.Click += new System.EventHandler(this.lblxoa_Click);
            // 
            // ItemFamilyImage
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblChiTiet);
            this.Controls.Add(this.lblxoa);
            this.Controls.Add(this.lblSua);
            this.Controls.Add(this.txtAlbumDes);
            this.Controls.Add(this.lblAlbumName);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ItemFamilyImage";
            this.Size = new System.Drawing.Size(615, 105);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private MaterialSkin.Controls.MaterialLabel lblAlbumName;
        private MaterialSkin.Controls.MaterialMultiLineTextBox txtAlbumDes;
        private MaterialSkin.Controls.MaterialLabel lblSua;
        private MaterialSkin.Controls.MaterialLabel lblChiTiet;
        private MaterialSkin.Controls.MaterialLabel lblxoa;
    }
}
