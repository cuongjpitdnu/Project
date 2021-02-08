namespace DSF602.View
{
    partial class RestoreVersion
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
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnRestore = new BaseCommon.ControlTemplate.btn();
            this.btnCancel = new BaseCommon.ControlTemplate.btn();
            this.lstbVersion = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(16, 9);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(99, 20);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Select version:";
            // 
            // btnRestore
            // 
            this.btnRestore.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRestore.Location = new System.Drawing.Point(96, 342);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Shortcut = System.Windows.Forms.Keys.None;
            this.btnRestore.Size = new System.Drawing.Size(79, 30);
            this.btnRestore.TabIndex = 2;
            this.btnRestore.Text = "Agree";
            this.btnRestore.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(191, 342);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Shortcut = System.Windows.Forms.Keys.None;
            this.btnCancel.Size = new System.Drawing.Size(79, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lstbVersion
            // 
            this.lstbVersion.FormattingEnabled = true;
            this.lstbVersion.ItemHeight = 20;
            this.lstbVersion.Location = new System.Drawing.Point(20, 50);
            this.lstbVersion.Name = "lstbVersion";
            this.lstbVersion.Size = new System.Drawing.Size(326, 264);
            this.lstbVersion.TabIndex = 1;
            // 
            // RestoreVersion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 384);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.lstbVersion);
            this.Controls.Add(this.lblHeader);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(351, 123);
            this.Name = "RestoreVersion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Restore Version";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private BaseCommon.ControlTemplate.btn btnRestore;
        private BaseCommon.ControlTemplate.btn btnCancel;
        private System.Windows.Forms.ListBox lstbVersion;
    }
}