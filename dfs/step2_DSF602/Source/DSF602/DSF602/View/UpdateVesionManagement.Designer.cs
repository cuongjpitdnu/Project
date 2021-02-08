namespace DSF602.View
{
    partial class UpdateVersion
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
            this.btnClose = new BaseCommon.ControlTemplate.btn();
            this.btnUpload = new BaseCommon.ControlTemplate.btn();
            this.prgDownload = new System.Windows.Forms.ProgressBar();
            this.lblInfo = new System.Windows.Forms.Label();
            this.logo = new System.Windows.Forms.PictureBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnRestore = new BaseCommon.ControlTemplate.btn();
            this.btnCheckVersion = new BaseCommon.ControlTemplate.btn();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(443, 173);
            this.btnClose.Name = "btnClose";
            this.btnClose.Shortcut = System.Windows.Forms.Keys.None;
            this.btnClose.Size = new System.Drawing.Size(107, 30);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "Cancel";
            this.btnClose.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpload
            // 
            this.btnUpload.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpload.Location = new System.Drawing.Point(320, 173);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Shortcut = System.Windows.Forms.Keys.None;
            this.btnUpload.Size = new System.Drawing.Size(107, 30);
            this.btnUpload.TabIndex = 20;
            this.btnUpload.Text = "Upload";
            this.btnUpload.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // prgDownload
            // 
            this.prgDownload.Location = new System.Drawing.Point(23, 117);
            this.prgDownload.Name = "prgDownload";
            this.prgDownload.Size = new System.Drawing.Size(527, 16);
            this.prgDownload.TabIndex = 18;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(117, 66);
            this.lblInfo.MaximumSize = new System.Drawing.Size(420, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(45, 20);
            this.lblInfo.TabIndex = 17;
            this.lblInfo.Text = "label2";
            // 
            // logo
            // 
            this.logo.Image = global::DSF602.Properties.Resources.logoSSD;
            this.logo.Location = new System.Drawing.Point(23, 23);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(78, 72);
            this.logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.logo.TabIndex = 16;
            this.logo.TabStop = false;
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContent.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblContent.Location = new System.Drawing.Point(116, 23);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(216, 24);
            this.lblContent.TabIndex = 15;
            this.lblContent.Text = "A new version is availble";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(19, 145);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(0, 20);
            this.lblProgress.TabIndex = 21;
            // 
            // btnRestore
            // 
            this.btnRestore.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestore.Location = new System.Drawing.Point(197, 173);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Shortcut = System.Windows.Forms.Keys.None;
            this.btnRestore.Size = new System.Drawing.Size(107, 30);
            this.btnRestore.TabIndex = 22;
            this.btnRestore.Text = "Restore";
            this.btnRestore.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnCheckVersion
            // 
            this.btnCheckVersion.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckVersion.Location = new System.Drawing.Point(73, 173);
            this.btnCheckVersion.Name = "btnCheckVersion";
            this.btnCheckVersion.Shortcut = System.Windows.Forms.Keys.None;
            this.btnCheckVersion.Size = new System.Drawing.Size(107, 30);
            this.btnCheckVersion.TabIndex = 20;
            this.btnCheckVersion.Text = "CheckVersion";
            this.btnCheckVersion.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnCheckVersion.UseVisualStyleBackColor = true;
            this.btnCheckVersion.Click += new System.EventHandler(this.btnCheckVersion_Click);
            // 
            // frmUpdateVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 244);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.prgDownload);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.logo);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnCheckVersion);
            this.Controls.Add(this.btnUpload);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(351, 123);
            this.Name = "frmUpdateVersion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update Management";
            this.Load += new System.EventHandler(this.UpdateVersion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BaseCommon.ControlTemplate.btn btnClose;
        private BaseCommon.ControlTemplate.btn btnUpload;
        private System.Windows.Forms.ProgressBar prgDownload;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.Label lblProgress;
        private BaseCommon.ControlTemplate.btn btnRestore;
        private BaseCommon.ControlTemplate.btn btnCheckVersion;
    }
}