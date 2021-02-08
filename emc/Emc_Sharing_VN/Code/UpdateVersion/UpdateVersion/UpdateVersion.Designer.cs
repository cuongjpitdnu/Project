namespace UpdateVersion
{
    partial class frmUpdateVersion
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
            this.lblContent = new System.Windows.Forms.Label();
            this.logo = new System.Windows.Forms.PictureBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.prgDownload = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContent.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblContent.Location = new System.Drawing.Point(116, 23);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(216, 24);
            this.lblContent.TabIndex = 3;
            this.lblContent.Text = "A new version is availble";
            // 
            // logo
            // 
            this.logo.BackgroundImage = global::UpdateVersion.Properties.Resources.Lighthouse;
            this.logo.Image = global::UpdateVersion.Properties.Resources.Lighthouse;
            this.logo.Location = new System.Drawing.Point(23, 23);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(78, 72);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logo.TabIndex = 4;
            this.logo.TabStop = false;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(117, 66);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(35, 13);
            this.lblInfo.TabIndex = 5;
            this.lblInfo.Text = "label2";
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(187, 165);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(134, 23);
            this.btnDownload.TabIndex = 6;
            this.btnDownload.Text = "Upload";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(343, 165);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(134, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // prgDownload
            // 
            this.prgDownload.Location = new System.Drawing.Point(23, 117);
            this.prgDownload.Name = "prgDownload";
            this.prgDownload.Size = new System.Drawing.Size(395, 16);
            this.prgDownload.TabIndex = 7;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(20, 145);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 13);
            this.lblProgress.TabIndex = 5;
            // 
            // frmUpdateVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 198);
            this.ControlBox = false;
            this.Controls.Add(this.prgDownload);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.logo);
            this.Controls.Add(this.lblContent);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpdateVersion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UpdateVersion";
            this.Load += new System.EventHandler(this.frmUpdateVersion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ProgressBar prgDownload;
        private System.Windows.Forms.Label lblProgress;
    }
}

