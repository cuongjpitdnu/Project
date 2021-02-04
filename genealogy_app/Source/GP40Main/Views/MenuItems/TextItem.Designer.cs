namespace GP40Main.Views.MenuItems
{
    partial class TextItem
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
            this.cbTextColor = new MaterialSkin.Controls.MaterialDropDownColorPicker();
            this.lblTextColor = new MaterialSkin.Controls.MaterialLabel();
            this.SuspendLayout();
            // 
            // pnlborderbottom
            // 
            this.pnlborderbottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlborderbottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlborderbottom.Location = new System.Drawing.Point(0, 47);
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
            // cbTextColor
            // 
            this.cbTextColor.AnchorSize = new System.Drawing.Size(60, 21);
            this.cbTextColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.cbTextColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(64)))), ((int)(((byte)(129)))));
            this.cbTextColor.Depth = 0;
            this.cbTextColor.DockSide = MaterialSkin.Controls.DropDownControl.eDockSide.Left;
            this.cbTextColor.Location = new System.Drawing.Point(131, 12);
            this.cbTextColor.MouseState = MaterialSkin.MouseState.HOVER;
            this.cbTextColor.Name = "cbTextColor";
            this.cbTextColor.Size = new System.Drawing.Size(60, 21);
            this.cbTextColor.TabIndex = 11;
            // 
            // lblTextColor
            // 
            this.lblTextColor.AutoSize = true;
            this.lblTextColor.Depth = 0;
            this.lblTextColor.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTextColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblTextColor.Location = new System.Drawing.Point(8, 13);
            this.lblTextColor.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblTextColor.Name = "lblTextColor";
            this.lblTextColor.Size = new System.Drawing.Size(64, 19);
            this.lblTextColor.TabIndex = 10;
            this.lblTextColor.Text = "Màu chữ";
            this.lblTextColor.UseAccent = true;
            // 
            // TextItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.Controls.Add(this.cbTextColor);
            this.Controls.Add(this.lblTextColor);
            this.Controls.Add(this.pnlborderbottom);
            this.Controls.Add(this.pnlbordertop);
            this.Name = "TextItem";
            this.Size = new System.Drawing.Size(200, 48);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlborderbottom;
        private System.Windows.Forms.Panel pnlbordertop;
        private MaterialSkin.Controls.MaterialLabel lblTextColor;
        public MaterialSkin.Controls.MaterialDropDownColorPicker cbTextColor;
    }
}
