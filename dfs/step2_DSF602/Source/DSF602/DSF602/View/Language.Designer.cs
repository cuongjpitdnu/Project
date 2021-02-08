namespace DSF602.View
{
    partial class Language
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
            this.btnChangeLanguage = new BaseCommon.ControlTemplate.btn();
            this.btnCancelLanguage = new BaseCommon.ControlTemplate.btn();
            this.lstbLanguage = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Location = new System.Drawing.Point(13, 7);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(157, 20);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Select display language:";
            // 
            // btnChangeLanguage
            // 
            this.btnChangeLanguage.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeLanguage.Location = new System.Drawing.Point(53, 287);
            this.btnChangeLanguage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnChangeLanguage.Name = "btnChangeLanguage";
            this.btnChangeLanguage.Shortcut = System.Windows.Forms.Keys.None;
            this.btnChangeLanguage.Size = new System.Drawing.Size(100, 40);
            this.btnChangeLanguage.TabIndex = 2;
            this.btnChangeLanguage.Text = "Agree";
            this.btnChangeLanguage.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Medium;
            this.btnChangeLanguage.UseVisualStyleBackColor = true;
            this.btnChangeLanguage.Click += new System.EventHandler(this.btnChangeLanguage_Click);
            // 
            // btnCancelLanguage
            // 
            this.btnCancelLanguage.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelLanguage.Location = new System.Drawing.Point(161, 287);
            this.btnCancelLanguage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancelLanguage.Name = "btnCancelLanguage";
            this.btnCancelLanguage.Shortcut = System.Windows.Forms.Keys.None;
            this.btnCancelLanguage.Size = new System.Drawing.Size(100, 40);
            this.btnCancelLanguage.TabIndex = 2;
            this.btnCancelLanguage.Text = "Cancel";
            this.btnCancelLanguage.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Medium;
            this.btnCancelLanguage.UseVisualStyleBackColor = true;
            this.btnCancelLanguage.Click += new System.EventHandler(this.btnCancelLanguage_Click);
            // 
            // lstbLanguage
            // 
            this.lstbLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstbLanguage.FormattingEnabled = true;
            this.lstbLanguage.ItemHeight = 20;
            this.lstbLanguage.Location = new System.Drawing.Point(12, 35);
            this.lstbLanguage.Name = "lstbLanguage";
            this.lstbLanguage.ScrollAlwaysVisible = true;
            this.lstbLanguage.Size = new System.Drawing.Size(290, 244);
            this.lstbLanguage.TabIndex = 3;
            this.lstbLanguage.DoubleClick += new System.EventHandler(this.lstbLanguage_DoubleClick);
            // 
            // Language
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 334);
            this.Controls.Add(this.lstbLanguage);
            this.Controls.Add(this.btnCancelLanguage);
            this.Controls.Add(this.btnChangeLanguage);
            this.Controls.Add(this.lblHeader);
            this.DoubleBuffered = true;
            this.EscToClose = true;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2555, 1641);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(223, 124);
            this.Name = "Language";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Language";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Language_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHeader;
        private BaseCommon.ControlTemplate.btn btnChangeLanguage;
        private BaseCommon.ControlTemplate.btn btnCancelLanguage;
        private System.Windows.Forms.ListBox lstbLanguage;
    }
}