namespace GPMain.Views.FamilyInfo
{
    partial class ImageTemplate
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ckselect = new MaterialSkin.Controls.MaterialCheckbox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::GPMain.Properties.Resources.no_avata;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(183, 198);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.DoubleClick += new System.EventHandler(this.pictureBox1_DoubleClick);
            // 
            // ckselect
            // 
            this.ckselect.AutoSize = true;
            this.ckselect.BackColor = System.Drawing.Color.Transparent;
            this.ckselect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ckselect.Depth = 0;
            this.ckselect.Location = new System.Drawing.Point(0, 0);
            this.ckselect.Margin = new System.Windows.Forms.Padding(0);
            this.ckselect.MouseLocation = new System.Drawing.Point(-1, -1);
            this.ckselect.MouseState = MaterialSkin.MouseState.HOVER;
            this.ckselect.Name = "ckselect";
            this.ckselect.Ripple = true;
            this.ckselect.Size = new System.Drawing.Size(35, 37);
            this.ckselect.TabIndex = 1;
            this.ckselect.UseVisualStyleBackColor = false;
            this.ckselect.Click += new System.EventHandler(this.ckselect_Click);
            // 
            // ImageTemplate
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            //this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.ckselect);
            this.Controls.Add(this.pictureBox1);
            this.ForeColor = System.Drawing.Color.LightGray;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ImageTemplate";
            this.Size = new System.Drawing.Size(183, 198);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private MaterialSkin.Controls.MaterialCheckbox ckselect;
    }
}
