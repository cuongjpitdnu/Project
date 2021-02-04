namespace GPMain.Views
{
    partial class ExportListMember
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
            this.btnword = new System.Windows.Forms.Button();
            this.btnexcel = new System.Windows.Forms.Button();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.materialCard2 = new MaterialSkin.Controls.MaterialCard();
            this.lblinfo = new MaterialSkin.Controls.MaterialLabel();
            this.materialCard1.SuspendLayout();
            this.materialCard2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnword
            // 
            this.btnword.BackgroundImage = global::GPMain.Properties.Resources.word;
            this.btnword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnword.FlatAppearance.BorderSize = 0;
            this.btnword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnword.Location = new System.Drawing.Point(17, 19);
            this.btnword.Name = "btnword";
            this.btnword.Size = new System.Drawing.Size(68, 68);
            this.btnword.TabIndex = 0;
            this.btnword.UseVisualStyleBackColor = true;
            this.btnword.Click += new System.EventHandler(this.btnword_Click);
            // 
            // btnexcel
            // 
            this.btnexcel.BackgroundImage = global::GPMain.Properties.Resources.excel128;
            this.btnexcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnexcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnexcel.FlatAppearance.BorderSize = 0;
            this.btnexcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnexcel.Location = new System.Drawing.Point(17, 19);
            this.btnexcel.Name = "btnexcel";
            this.btnexcel.Size = new System.Drawing.Size(68, 68);
            this.btnexcel.TabIndex = 0;
            this.btnexcel.UseVisualStyleBackColor = true;
            this.btnexcel.Click += new System.EventHandler(this.btnexcel_Click);
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(32, 98);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(38, 19);
            this.materialLabel1.TabIndex = 1;
            this.materialLabel1.Text = "Word";
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.Location = new System.Drawing.Point(32, 98);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(38, 19);
            this.materialLabel2.TabIndex = 1;
            this.materialLabel2.Text = "Excel";
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.Controls.Add(this.btnword);
            this.materialCard1.Controls.Add(this.materialLabel1);
            this.materialCard1.Depth = 0;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(30, 11);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(104, 136);
            this.materialCard1.TabIndex = 2;
            // 
            // materialCard2
            // 
            this.materialCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard2.Controls.Add(this.btnexcel);
            this.materialCard2.Controls.Add(this.materialLabel2);
            this.materialCard2.Depth = 0;
            this.materialCard2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard2.Location = new System.Drawing.Point(189, 11);
            this.materialCard2.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard2.Name = "materialCard2";
            this.materialCard2.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard2.Size = new System.Drawing.Size(104, 136);
            this.materialCard2.TabIndex = 3;
            // 
            // lblinfo
            // 
            this.lblinfo.AutoSize = true;
            this.lblinfo.Depth = 0;
            this.lblinfo.Font = new System.Drawing.Font("Roboto", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblinfo.FontType = MaterialSkin.MaterialSkinManager.fontType.Body2;
            this.lblinfo.ForeColor = System.Drawing.Color.Red;
            this.lblinfo.Location = new System.Drawing.Point(3, 160);
            this.lblinfo.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblinfo.Name = "lblinfo";
            this.lblinfo.Size = new System.Drawing.Size(170, 17);
            this.lblinfo.TabIndex = 4;
            this.lblinfo.Text = "*Chọn định dạng file dữ liệu";
            // 
            // ExportListMember
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.lblinfo);
            this.Controls.Add(this.materialCard2);
            this.Controls.Add(this.materialCard1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportListMember";
            this.Sizable = false;
            this.Size = new System.Drawing.Size(323, 180);
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            this.materialCard2.ResumeLayout(false);
            this.materialCard2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnword;
        private System.Windows.Forms.Button btnexcel;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialCard materialCard2;
        private MaterialSkin.Controls.MaterialLabel lblinfo;
    }
}
