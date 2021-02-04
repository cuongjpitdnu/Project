namespace GPMain.Views
{
    partial class ShowImageAlbum
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowImageAlbum));
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnpre = new System.Windows.Forms.Button();
            this.btnsaveimage = new System.Windows.Forms.Button();
            this.btnnext = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.flowLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 410);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 60);
            this.panel1.TabIndex = 0;
            this.panel1.SizeChanged += new System.EventHandler(this.panel1_SizeChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.btnpre);
            this.flowLayoutPanel1.Controls.Add(this.btnsaveimage);
            this.flowLayoutPanel1.Controls.Add(this.btnnext);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(277, 7);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(247, 46);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btnpre
            // 
            this.btnpre.BackColor = System.Drawing.Color.White;
            this.btnpre.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnpre.BackgroundImage")));
            this.btnpre.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnpre.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnpre.FlatAppearance.BorderSize = 0;
            this.btnpre.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnpre.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnpre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnpre.Location = new System.Drawing.Point(3, 3);
            this.btnpre.Name = "btnpre";
            this.btnpre.Size = new System.Drawing.Size(77, 40);
            this.btnpre.TabIndex = 0;
            this.btnpre.UseVisualStyleBackColor = false;
            this.btnpre.Click += new System.EventHandler(this.btnpre_Click);
            // 
            // btnsaveimage
            // 
            this.btnsaveimage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnsaveimage.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnsaveimage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsaveimage.Image = global::GPMain.Properties.Resources.diskette;
            this.btnsaveimage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnsaveimage.Location = new System.Drawing.Point(86, 3);
            this.btnsaveimage.Name = "btnsaveimage";
            this.btnsaveimage.Size = new System.Drawing.Size(75, 40);
            this.btnsaveimage.TabIndex = 0;
            this.btnsaveimage.Text = "Lưu ảnh";
            this.btnsaveimage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnsaveimage.UseVisualStyleBackColor = true;
            this.btnsaveimage.Click += new System.EventHandler(this.btnsaveimage_Click);
            // 
            // btnnext
            // 
            this.btnnext.BackColor = System.Drawing.Color.White;
            this.btnnext.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnnext.BackgroundImage")));
            this.btnnext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnnext.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnnext.FlatAppearance.BorderSize = 0;
            this.btnnext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnnext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnnext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnnext.Location = new System.Drawing.Point(167, 3);
            this.btnnext.Name = "btnnext";
            this.btnnext.Size = new System.Drawing.Size(77, 40);
            this.btnnext.TabIndex = 0;
            this.btnnext.UseVisualStyleBackColor = false;
            this.btnnext.Click += new System.EventHandler(this.btnnext_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 410);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // ShowImageAlbum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 470);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShowImageAlbum";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShowImageAlbum";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ShowImageAlbum_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnnext;
        private System.Windows.Forms.Button btnpre;
        private System.Windows.Forms.Button btnsaveimage;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}