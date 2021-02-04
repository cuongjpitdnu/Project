namespace GPMain.Views.Tree.MenuItems
{
    partial class TreeModeItem
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
            this.rbNormal = new MaterialSkin.Controls.MaterialRadioButton();
            this.rbexpand = new MaterialSkin.Controls.MaterialRadioButton();
            this.SuspendLayout();
            // 
            // pnlborderbottom
            // 
            this.pnlborderbottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlborderbottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlborderbottom.Location = new System.Drawing.Point(0, 93);
            this.pnlborderbottom.Name = "pnlborderbottom";
            this.pnlborderbottom.Size = new System.Drawing.Size(200, 1);
            this.pnlborderbottom.TabIndex = 11;
            // 
            // pnlbordertop
            // 
            this.pnlbordertop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlbordertop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlbordertop.Location = new System.Drawing.Point(0, 0);
            this.pnlbordertop.Name = "pnlbordertop";
            this.pnlbordertop.Size = new System.Drawing.Size(200, 1);
            this.pnlbordertop.TabIndex = 10;
            // 
            // rbNormal
            // 
            this.rbNormal.AutoSize = true;
            this.rbNormal.Checked = true;
            this.rbNormal.Depth = 0;
            this.rbNormal.Location = new System.Drawing.Point(9, 12);
            this.rbNormal.Margin = new System.Windows.Forms.Padding(0);
            this.rbNormal.MouseLocation = new System.Drawing.Point(-1, -1);
            this.rbNormal.MouseState = MaterialSkin.MouseState.HOVER;
            this.rbNormal.Name = "rbNormal";
            this.rbNormal.Ripple = true;
            this.rbNormal.Size = new System.Drawing.Size(152, 37);
            this.rbNormal.TabIndex = 12;
            this.rbNormal.TabStop = true;
            this.rbNormal.Text = "Cây bình thường";
            this.rbNormal.UseVisualStyleBackColor = true;
            // 
            // rbexpand
            // 
            this.rbexpand.AutoSize = true;
            this.rbexpand.Depth = 0;
            this.rbexpand.Location = new System.Drawing.Point(9, 49);
            this.rbexpand.Margin = new System.Windows.Forms.Padding(0);
            this.rbexpand.MouseLocation = new System.Drawing.Point(-1, -1);
            this.rbexpand.MouseState = MaterialSkin.MouseState.HOVER;
            this.rbexpand.Name = "rbexpand";
            this.rbexpand.Ripple = true;
            this.rbexpand.Size = new System.Drawing.Size(125, 37);
            this.rbexpand.TabIndex = 12;
            this.rbexpand.TabStop = true;
            this.rbexpand.Text = "Cây mở rộng";
            this.rbexpand.UseVisualStyleBackColor = true;
            // 
            // TreeModeItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rbexpand);
            this.Controls.Add(this.rbNormal);
            this.Controls.Add(this.pnlborderbottom);
            this.Controls.Add(this.pnlbordertop);
            this.Name = "TreeModeItem";
            this.Size = new System.Drawing.Size(200, 94);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlborderbottom;
        private System.Windows.Forms.Panel pnlbordertop;
        private MaterialSkin.Controls.MaterialRadioButton rbNormal;
        private MaterialSkin.Controls.MaterialRadioButton rbexpand;
    }
}
