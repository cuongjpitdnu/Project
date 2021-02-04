namespace GPMain.Views.FamilyInfo
{
    partial class ItemImage
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
            this.pnlImage = new System.Windows.Forms.Panel();
            this.ckbImage = new MaterialSkin.Controls.MaterialCheckbox();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.pnlImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlImage
            // 
            this.pnlImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlImage.Controls.Add(this.ckbImage);
            this.pnlImage.Controls.Add(this.picImage);
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImage.Location = new System.Drawing.Point(0, 0);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(218, 253);
            this.pnlImage.TabIndex = 0;
            // 
            // ckbImage
            // 
            this.ckbImage.AutoSize = true;
            this.ckbImage.BackColor = System.Drawing.Color.Transparent;
            this.ckbImage.Depth = 0;
            this.ckbImage.Location = new System.Drawing.Point(3, 3);
            this.ckbImage.Margin = new System.Windows.Forms.Padding(0);
            this.ckbImage.MouseLocation = new System.Drawing.Point(-1, -1);
            this.ckbImage.MouseState = MaterialSkin.MouseState.HOVER;
            this.ckbImage.Name = "ckbImage";
            this.ckbImage.Ripple = true;
            this.ckbImage.Size = new System.Drawing.Size(35, 37);
            this.ckbImage.TabIndex = 0;
            this.ckbImage.UseVisualStyleBackColor = false;
            // 
            // picImage
            // 
            this.picImage.Image = global::GPMain.Properties.Resources.no_avata;
            this.picImage.Location = new System.Drawing.Point(3, 3);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(210, 243);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picImage.TabIndex = 1;
            this.picImage.TabStop = false;
            // 
            // ItemImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlImage);
            this.Name = "ItemImage";
            this.Size = new System.Drawing.Size(218, 253);
            this.pnlImage.ResumeLayout(false);
            this.pnlImage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlImage;
        public System.Windows.Forms.PictureBox picImage;
        public MaterialSkin.Controls.MaterialCheckbox ckbImage;
    }
}
