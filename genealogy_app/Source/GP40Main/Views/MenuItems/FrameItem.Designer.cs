namespace GP40Main.Views.MenuItems
{
    partial class FrameItem
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
            this.pnlborderbottom = new System.Windows.Forms.Panel();
            this.pnlbordertop = new System.Windows.Forms.Panel();
            this.cbBorderColor = new MaterialSkin.Controls.MaterialDropDownColorPicker();
            this.lblBorderColor = new MaterialSkin.Controls.MaterialLabel();
            this.btnpre = new System.Windows.Forms.Button();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.cbframe = new System.Windows.Forms.ComboBox();
            this.btnnext = new System.Windows.Forms.Button();
            this.picframe = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picframe)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlborderbottom
            // 
            this.pnlborderbottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlborderbottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlborderbottom.Location = new System.Drawing.Point(0, 237);
            this.pnlborderbottom.Name = "pnlborderbottom";
            this.pnlborderbottom.Size = new System.Drawing.Size(200, 1);
            this.pnlborderbottom.TabIndex = 9;
            // 
            // pnlbordertop
            // 
            this.pnlbordertop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlbordertop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlbordertop.Location = new System.Drawing.Point(0, 0);
            this.pnlbordertop.Name = "pnlbordertop";
            this.pnlbordertop.Size = new System.Drawing.Size(200, 1);
            this.pnlbordertop.TabIndex = 8;
            // 
            // cbBorderColor
            // 
            this.cbBorderColor.AnchorSize = new System.Drawing.Size(60, 21);
            this.cbBorderColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.cbBorderColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(64)))), ((int)(((byte)(129)))));
            this.cbBorderColor.Depth = 0;
            this.cbBorderColor.DockSide = MaterialSkin.Controls.DropDownControl.eDockSide.Left;
            this.cbBorderColor.Location = new System.Drawing.Point(131, 10);
            this.cbBorderColor.MouseState = MaterialSkin.MouseState.HOVER;
            this.cbBorderColor.Name = "cbBorderColor";
            this.cbBorderColor.Size = new System.Drawing.Size(60, 21);
            this.cbBorderColor.TabIndex = 11;
            // 
            // lblBorderColor
            // 
            this.lblBorderColor.AutoSize = true;
            this.lblBorderColor.Depth = 0;
            this.lblBorderColor.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblBorderColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblBorderColor.Location = new System.Drawing.Point(8, 11);
            this.lblBorderColor.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblBorderColor.Name = "lblBorderColor";
            this.lblBorderColor.Size = new System.Drawing.Size(66, 19);
            this.lblBorderColor.TabIndex = 10;
            this.lblBorderColor.Text = "Màu viền";
            this.lblBorderColor.UseAccent = true;
            // 
            // btnpre
            // 
            this.btnpre.BackgroundImage = global::GP40Main.Properties.Resources.back;
            this.btnpre.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnpre.FlatAppearance.BorderSize = 0;
            this.btnpre.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnpre.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnpre.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnpre.Location = new System.Drawing.Point(11, 142);
            this.btnpre.Name = "btnpre";
            this.btnpre.Size = new System.Drawing.Size(30, 30);
            this.btnpre.TabIndex = 13;
            this.btnpre.UseVisualStyleBackColor = true;
            this.btnpre.Visible = false;
            this.btnpre.Click += new System.EventHandler(this.btnpre_Click);
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialLabel1.Location = new System.Drawing.Point(8, 49);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(73, 19);
            this.materialLabel1.TabIndex = 10;
            this.materialLabel1.Text = "Khung thẻ";
            this.materialLabel1.UseAccent = true;
            // 
            // cbframe
            // 
            this.cbframe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbframe.FormattingEnabled = true;
            this.cbframe.Location = new System.Drawing.Point(131, 48);
            this.cbframe.Name = "cbframe";
            this.cbframe.Size = new System.Drawing.Size(60, 21);
            this.cbframe.TabIndex = 14;
            this.cbframe.SelectedIndexChanged += new System.EventHandler(this.cbframe_SelectedIndexChanged);
            this.cbframe.ValueMemberChanged += new System.EventHandler(this.cbframe_ValueMemberChanged);
            // 
            // btnnext
            // 
            this.btnnext.BackgroundImage = global::GP40Main.Properties.Resources.next;
            this.btnnext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnnext.FlatAppearance.BorderSize = 0;
            this.btnnext.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnnext.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnnext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnnext.Location = new System.Drawing.Point(159, 142);
            this.btnnext.Name = "btnnext";
            this.btnnext.Size = new System.Drawing.Size(30, 30);
            this.btnnext.TabIndex = 13;
            this.btnnext.UseVisualStyleBackColor = true;
            this.btnnext.Click += new System.EventHandler(this.btnnext_Click);
            // 
            // picframe
            // 
            this.picframe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picframe.Location = new System.Drawing.Point(50, 90);
            this.picframe.Name = "picframe";
            this.picframe.Size = new System.Drawing.Size(100, 135);
            this.picframe.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picframe.TabIndex = 12;
            this.picframe.TabStop = false;
            // 
            // FrameItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.Controls.Add(this.cbframe);
            this.Controls.Add(this.btnnext);
            this.Controls.Add(this.btnpre);
            this.Controls.Add(this.picframe);
            this.Controls.Add(this.cbBorderColor);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.lblBorderColor);
            this.Controls.Add(this.pnlborderbottom);
            this.Controls.Add(this.pnlbordertop);
            this.Name = "FrameItem";
            this.Size = new System.Drawing.Size(200, 238);
            ((System.ComponentModel.ISupportInitialize)(this.picframe)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlborderbottom;
        private System.Windows.Forms.Panel pnlbordertop;
        private MaterialSkin.Controls.MaterialLabel lblBorderColor;
        public MaterialSkin.Controls.MaterialDropDownColorPicker cbBorderColor;
        private System.Windows.Forms.PictureBox picframe;
        private System.Windows.Forms.Button btnpre;
        private System.Windows.Forms.Button btnnext;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        public System.Windows.Forms.ComboBox cbframe;
    }
}
