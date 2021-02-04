namespace GPMain.Views.Tree.MenuItems
{
    partial class ShowImageItem
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
            this.rdshowimage = new MaterialSkin.Controls.MaterialRadioButton();
            this.rdhideimage = new MaterialSkin.Controls.MaterialRadioButton();
            this.SuspendLayout();
            // 
            // pnlborderbottom
            // 
            this.pnlborderbottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlborderbottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlborderbottom.Location = new System.Drawing.Point(0, 95);
            this.pnlborderbottom.Name = "pnlborderbottom";
            this.pnlborderbottom.Size = new System.Drawing.Size(200, 1);
            this.pnlborderbottom.TabIndex = 10;
            // 
            // pnlbordertop
            // 
            this.pnlbordertop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlbordertop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlbordertop.Location = new System.Drawing.Point(0, 0);
            this.pnlbordertop.Name = "pnlbordertop";
            this.pnlbordertop.Size = new System.Drawing.Size(200, 1);
            this.pnlbordertop.TabIndex = 11;
            // 
            // rdshowimage
            // 
            this.rdshowimage.AutoSize = true;
            this.rdshowimage.Checked = true;
            this.rdshowimage.Depth = 0;
            this.rdshowimage.Location = new System.Drawing.Point(6, 11);
            this.rdshowimage.Margin = new System.Windows.Forms.Padding(0);
            this.rdshowimage.MouseLocation = new System.Drawing.Point(-1, -1);
            this.rdshowimage.MouseState = MaterialSkin.MouseState.HOVER;
            this.rdshowimage.Name = "rdshowimage";
            this.rdshowimage.Ripple = true;
            this.rdshowimage.Size = new System.Drawing.Size(89, 37);
            this.rdshowimage.TabIndex = 0;
            this.rdshowimage.TabStop = true;
            this.rdshowimage.Text = "Hiển thị";
            this.rdshowimage.UseVisualStyleBackColor = true;
            // 
            // rdhideimage
            // 
            this.rdhideimage.AutoSize = true;
            this.rdhideimage.Depth = 0;
            this.rdhideimage.Location = new System.Drawing.Point(6, 48);
            this.rdhideimage.Margin = new System.Windows.Forms.Padding(0);
            this.rdhideimage.MouseLocation = new System.Drawing.Point(-1, -1);
            this.rdhideimage.MouseState = MaterialSkin.MouseState.HOVER;
            this.rdhideimage.Name = "rdhideimage";
            this.rdhideimage.Ripple = true;
            this.rdhideimage.Size = new System.Drawing.Size(137, 37);
            this.rdhideimage.TabIndex = 0;
            this.rdhideimage.TabStop = true;
            this.rdhideimage.Text = "Không hiển thị";
            this.rdhideimage.UseVisualStyleBackColor = true;
            // 
            // ShowImageItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdshowimage);
            this.Controls.Add(this.rdhideimage);
            this.Controls.Add(this.pnlbordertop);
            this.Controls.Add(this.pnlborderbottom);
            this.Name = "ShowImageItem";
            this.Size = new System.Drawing.Size(200, 96);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlborderbottom;
        private System.Windows.Forms.Panel pnlbordertop;
        private MaterialSkin.Controls.MaterialRadioButton rdshowimage;
        private MaterialSkin.Controls.MaterialRadioButton rdhideimage;
    }
}
