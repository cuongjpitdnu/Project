namespace GPMain.Views.Tree.MenuItems
{
    partial class ShowBirthDateItem
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
            this.pnlbordertop = new System.Windows.Forms.Panel();
            this.pnlborderbottom = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rdshowdefauld = new MaterialSkin.Controls.MaterialRadioButton();
            this.rdshowunknow = new MaterialSkin.Controls.MaterialRadioButton();
            this.pnlbordertop.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlbordertop
            // 
            this.pnlbordertop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlbordertop.Controls.Add(this.pnlborderbottom);
            this.pnlbordertop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlbordertop.Location = new System.Drawing.Point(0, 0);
            this.pnlbordertop.Name = "pnlbordertop";
            this.pnlbordertop.Size = new System.Drawing.Size(200, 1);
            this.pnlbordertop.TabIndex = 9;
            // 
            // pnlborderbottom
            // 
            this.pnlborderbottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlborderbottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlborderbottom.Location = new System.Drawing.Point(0, 0);
            this.pnlborderbottom.Name = "pnlborderbottom";
            this.pnlborderbottom.Size = new System.Drawing.Size(200, 1);
            this.pnlborderbottom.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 92);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 1);
            this.panel1.TabIndex = 10;
            // 
            // rdshowdefauld
            // 
            this.rdshowdefauld.AutoSize = true;
            this.rdshowdefauld.Checked = true;
            this.rdshowdefauld.Depth = 0;
            this.rdshowdefauld.Location = new System.Drawing.Point(6, 9);
            this.rdshowdefauld.Margin = new System.Windows.Forms.Padding(0);
            this.rdshowdefauld.MouseLocation = new System.Drawing.Point(-1, -1);
            this.rdshowdefauld.MouseState = MaterialSkin.MouseState.HOVER;
            this.rdshowdefauld.Name = "rdshowdefauld";
            this.rdshowdefauld.Ripple = true;
            this.rdshowdefauld.Size = new System.Drawing.Size(164, 37);
            this.rdshowdefauld.TabIndex = 13;
            this.rdshowdefauld.TabStop = true;
            this.rdshowdefauld.Text = "Hiển thị mặc định ";
            this.rdshowdefauld.UseVisualStyleBackColor = true;
            // 
            // rdshowunknow
            // 
            this.rdshowunknow.AutoSize = true;
            this.rdshowunknow.Depth = 0;
            this.rdshowunknow.Location = new System.Drawing.Point(6, 46);
            this.rdshowunknow.Margin = new System.Windows.Forms.Padding(0);
            this.rdshowunknow.MouseLocation = new System.Drawing.Point(-1, -1);
            this.rdshowunknow.MouseState = MaterialSkin.MouseState.HOVER;
            this.rdshowunknow.Name = "rdshowunknow";
            this.rdshowunknow.Ripple = true;
            this.rdshowunknow.Size = new System.Drawing.Size(176, 37);
            this.rdshowunknow.TabIndex = 13;
            this.rdshowunknow.TabStop = true;
            this.rdshowunknow.Text = "Hiển thị KHÔNG RÕ ";
            this.rdshowunknow.UseVisualStyleBackColor = true;
            // 
            // ShowBirthDateItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdshowunknow);
            this.Controls.Add(this.rdshowdefauld);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlbordertop);
            this.Name = "ShowBirthDateItem";
            this.Size = new System.Drawing.Size(200, 93);
            this.pnlbordertop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlbordertop;
        private System.Windows.Forms.Panel pnlborderbottom;
        private System.Windows.Forms.Panel panel1;
        private MaterialSkin.Controls.MaterialRadioButton rdshowdefauld;
        private MaterialSkin.Controls.MaterialRadioButton rdshowunknow;
    }
}
