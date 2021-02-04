using GPModels;

namespace GPMain.Views.FamilyInfo
{
    partial class FamilyImage
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
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.btnAddNewAlbum = new MaterialSkin.Controls.MaterialButton();
            this.flpListAlbum = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialLabel1
            // 
            this.materialLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialLabel1.BackColor = System.Drawing.Color.White;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 34F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.FontType = MaterialSkin.MaterialSkinManager.fontType.H4;
            this.materialLabel1.Location = new System.Drawing.Point(180, 11);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(316, 39);
            this.materialLabel1.TabIndex = 3;
            this.materialLabel1.Text = "Album ảnh dòng họ";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAddNewAlbum
            // 
            this.btnAddNewAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNewAlbum.AutoSize = false;
            this.btnAddNewAlbum.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddNewAlbum.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddNewAlbum.Depth = 0;
            this.btnAddNewAlbum.DrawShadows = true;
            this.btnAddNewAlbum.HighEmphasis = true;
            this.btnAddNewAlbum.Icon = null;
            this.btnAddNewAlbum.Location = new System.Drawing.Point(559, 8);
            this.btnAddNewAlbum.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnAddNewAlbum.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAddNewAlbum.Name = "btnAddNewAlbum";
            this.btnAddNewAlbum.Size = new System.Drawing.Size(114, 42);
            this.btnAddNewAlbum.TabIndex = 1;
            this.btnAddNewAlbum.Text = "THÊM ALBUM";
            this.btnAddNewAlbum.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnAddNewAlbum.UseAccentColor = false;
            this.btnAddNewAlbum.UseVisualStyleBackColor = true;
            this.btnAddNewAlbum.Click += new System.EventHandler(this.btnAddNewAlbum_Click);
            // 
            // flpListAlbum
            // 
            this.flpListAlbum.AutoScroll = true;
            this.flpListAlbum.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpListAlbum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpListAlbum.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpListAlbum.ForeColor = System.Drawing.Color.Gainsboro;
            this.flpListAlbum.Location = new System.Drawing.Point(0, 0);
            this.flpListAlbum.Name = "flpListAlbum";
            this.flpListAlbum.Size = new System.Drawing.Size(677, 479);
            this.flpListAlbum.TabIndex = 4;
            this.flpListAlbum.WrapContents = false;
            this.flpListAlbum.SizeChanged += new System.EventHandler(this.plListAlbum_SizeChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAddNewAlbum);
            this.panel1.Controls.Add(this.materialLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(677, 60);
            this.panel1.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.flpListAlbum);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 60);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(677, 479);
            this.panel2.TabIndex = 6;
            // 
            // FamilyImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FamilyImage";
            this.Size = new System.Drawing.Size(677, 539);
            this.Load += new System.EventHandler(this.FamilyImage_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private MaterialSkin.Controls.MaterialButton btnAddNewAlbum;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private System.Windows.Forms.FlowLayoutPanel flpListAlbum;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
