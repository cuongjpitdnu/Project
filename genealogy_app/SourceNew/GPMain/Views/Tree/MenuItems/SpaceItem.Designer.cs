namespace GPMain.Views.MenuItems
{
    partial class SpaceItem
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
            this.pblbordertop = new System.Windows.Forms.Panel();
            this.pnlborderbottom = new System.Windows.Forms.Panel();
            this.sliderMemberVerticalSpace = new MaterialSkin.Controls.MaterialSlider();
            this.sliderMemberHorizonSpace = new MaterialSkin.Controls.MaterialSlider();
            this.lblMemberVerticalSpace = new MaterialSkin.Controls.MaterialLabel();
            this.lblMemberHorizonSpace = new MaterialSkin.Controls.MaterialLabel();
            this.SuspendLayout();
            // 
            // pblbordertop
            // 
            this.pblbordertop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pblbordertop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pblbordertop.Location = new System.Drawing.Point(0, 0);
            this.pblbordertop.Name = "pblbordertop";
            this.pblbordertop.Size = new System.Drawing.Size(200, 1);
            this.pblbordertop.TabIndex = 0;
            // 
            // pnlborderbottom
            // 
            this.pnlborderbottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlborderbottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlborderbottom.Location = new System.Drawing.Point(0, 160);
            this.pnlborderbottom.Name = "pnlborderbottom";
            this.pnlborderbottom.Size = new System.Drawing.Size(200, 1);
            this.pnlborderbottom.TabIndex = 1;
            // 
            // sliderMemberVerticalSpace
            // 
            this.sliderMemberVerticalSpace.Depth = 0;
            this.sliderMemberVerticalSpace.Location = new System.Drawing.Point(8, 112);
            this.sliderMemberVerticalSpace.MaxValue = 200;
            this.sliderMemberVerticalSpace.MinValue = 1;
            this.sliderMemberVerticalSpace.MouseState = MaterialSkin.MouseState.HOVER;
            this.sliderMemberVerticalSpace.Name = "sliderMemberVerticalSpace";
            this.sliderMemberVerticalSpace.Size = new System.Drawing.Size(180, 40);
            this.sliderMemberVerticalSpace.TabIndex = 7;
            this.sliderMemberVerticalSpace.Text = "Khoảng cách chiều dọc";
            this.sliderMemberVerticalSpace.Value = 50;
            // 
            // sliderMemberHorizonSpace
            // 
            this.sliderMemberHorizonSpace.Depth = 0;
            this.sliderMemberHorizonSpace.Location = new System.Drawing.Point(10, 37);
            this.sliderMemberHorizonSpace.MaxValue = 200;
            this.sliderMemberHorizonSpace.MinValue = 1;
            this.sliderMemberHorizonSpace.MouseState = MaterialSkin.MouseState.HOVER;
            this.sliderMemberHorizonSpace.Name = "sliderMemberHorizonSpace";
            this.sliderMemberHorizonSpace.Size = new System.Drawing.Size(180, 40);
            this.sliderMemberHorizonSpace.TabIndex = 8;
            this.sliderMemberHorizonSpace.Text = "Khoảng cách chiều ngang";
            this.sliderMemberHorizonSpace.Value = 50;
            // 
            // lblMemberVerticalSpace
            // 
            this.lblMemberVerticalSpace.AutoSize = true;
            this.lblMemberVerticalSpace.Depth = 0;
            this.lblMemberVerticalSpace.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblMemberVerticalSpace.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblMemberVerticalSpace.Location = new System.Drawing.Point(7, 85);
            this.lblMemberVerticalSpace.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblMemberVerticalSpace.Name = "lblMemberVerticalSpace";
            this.lblMemberVerticalSpace.Size = new System.Drawing.Size(166, 19);
            this.lblMemberVerticalSpace.TabIndex = 5;
            this.lblMemberVerticalSpace.Text = "Khoảng cách chiều dọc";
            // 
            // lblMemberHorizonSpace
            // 
            this.lblMemberHorizonSpace.AutoSize = true;
            this.lblMemberHorizonSpace.Depth = 0;
            this.lblMemberHorizonSpace.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblMemberHorizonSpace.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblMemberHorizonSpace.Location = new System.Drawing.Point(7, 10);
            this.lblMemberHorizonSpace.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblMemberHorizonSpace.Name = "lblMemberHorizonSpace";
            this.lblMemberHorizonSpace.Size = new System.Drawing.Size(185, 19);
            this.lblMemberHorizonSpace.TabIndex = 6;
            this.lblMemberHorizonSpace.Text = "Khoảng cách chiều ngang";
            this.lblMemberHorizonSpace.UseAccent = true;
            // 
            // SpaceItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.Controls.Add(this.sliderMemberVerticalSpace);
            this.Controls.Add(this.sliderMemberHorizonSpace);
            this.Controls.Add(this.lblMemberVerticalSpace);
            this.Controls.Add(this.lblMemberHorizonSpace);
            this.Controls.Add(this.pnlborderbottom);
            this.Controls.Add(this.pblbordertop);
            this.Name = "SpaceItem";
            this.Size = new System.Drawing.Size(200, 161);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pblbordertop;
        private System.Windows.Forms.Panel pnlborderbottom;
        private MaterialSkin.Controls.MaterialLabel lblMemberVerticalSpace;
        private MaterialSkin.Controls.MaterialLabel lblMemberHorizonSpace;
        public MaterialSkin.Controls.MaterialSlider sliderMemberVerticalSpace;
        public MaterialSkin.Controls.MaterialSlider sliderMemberHorizonSpace;
    }
}
