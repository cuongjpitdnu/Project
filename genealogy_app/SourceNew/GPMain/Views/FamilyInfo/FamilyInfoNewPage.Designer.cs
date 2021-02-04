namespace GPMain.Views.FamilyInfo
{
    partial class FamilyInfoNewPage
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
            this.txtFamilyName = new MaterialSkin.Controls.MaterialTextBox();
            this.txtFamilyAnniversary = new MaterialSkin.Controls.MaterialTextBox();
            this.txtFamilyLevel = new MaterialSkin.Controls.MaterialTextBox();
            this.txtFamilyHometown = new MaterialSkin.Controls.MaterialTextBox();
            this.txtUserCreated = new MaterialSkin.Controls.MaterialTextBox();
            this.btnContinue = new MaterialSkin.Controls.MaterialButton();
            this.btnClose = new MaterialSkin.Controls.MaterialButton();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.btncalender = new MaterialSkin.Controls.MaterialButton();
            this.SuspendLayout();
            // 
            // txtFamilyName
            // 
            this.txtFamilyName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFamilyName.Depth = 0;
            this.txtFamilyName.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtFamilyName.Hint = "Tên dòng họ *";
            this.txtFamilyName.Location = new System.Drawing.Point(32, 63);
            this.txtFamilyName.MaxLength = 50;
            this.txtFamilyName.ModeNumber_Maximum = 999999;
            this.txtFamilyName.MouseState = MaterialSkin.MouseState.OUT;
            this.txtFamilyName.Multiline = false;
            this.txtFamilyName.Name = "txtFamilyName";
            this.txtFamilyName.Size = new System.Drawing.Size(472, 50);
            this.txtFamilyName.TabIndex = 1;
            this.txtFamilyName.Text = "";
            // 
            // txtFamilyAnniversary
            // 
            this.txtFamilyAnniversary.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFamilyAnniversary.Depth = 0;
            this.txtFamilyAnniversary.Enabled = false;
            this.txtFamilyAnniversary.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtFamilyAnniversary.Hint = "Ngày giỗ (Âm lịch)";
            this.txtFamilyAnniversary.Location = new System.Drawing.Point(32, 119);
            this.txtFamilyAnniversary.MaxLength = 10;
            this.txtFamilyAnniversary.ModeNumber_Maximum = 999999;
            this.txtFamilyAnniversary.MouseState = MaterialSkin.MouseState.OUT;
            this.txtFamilyAnniversary.Multiline = false;
            this.txtFamilyAnniversary.Name = "txtFamilyAnniversary";
            this.txtFamilyAnniversary.Size = new System.Drawing.Size(200, 50);
            this.txtFamilyAnniversary.TabIndex = 2;
            this.txtFamilyAnniversary.Text = "";
            this.txtFamilyAnniversary.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFamilyAnniversary_KeyPress);
            // 
            // txtFamilyLevel
            // 
            this.txtFamilyLevel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFamilyLevel.Depth = 0;
            this.txtFamilyLevel.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtFamilyLevel.Hint = "Đời thứ";
            this.txtFamilyLevel.Location = new System.Drawing.Point(372, 119);
            this.txtFamilyLevel.MaxLength = 50;
            this.txtFamilyLevel.ModeNumber = true;
            this.txtFamilyLevel.ModeNumber_Maximum = 999999;
            this.txtFamilyLevel.MouseState = MaterialSkin.MouseState.OUT;
            this.txtFamilyLevel.Multiline = false;
            this.txtFamilyLevel.Name = "txtFamilyLevel";
            this.txtFamilyLevel.Size = new System.Drawing.Size(132, 50);
            this.txtFamilyLevel.TabIndex = 3;
            this.txtFamilyLevel.Text = "";
            // 
            // txtFamilyHometown
            // 
            this.txtFamilyHometown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFamilyHometown.Depth = 0;
            this.txtFamilyHometown.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtFamilyHometown.Hint = "Nguyên quán";
            this.txtFamilyHometown.Location = new System.Drawing.Point(32, 175);
            this.txtFamilyHometown.MaxLength = 50;
            this.txtFamilyHometown.ModeNumber_Maximum = 999999;
            this.txtFamilyHometown.MouseState = MaterialSkin.MouseState.OUT;
            this.txtFamilyHometown.Multiline = false;
            this.txtFamilyHometown.Name = "txtFamilyHometown";
            this.txtFamilyHometown.Size = new System.Drawing.Size(472, 50);
            this.txtFamilyHometown.TabIndex = 4;
            this.txtFamilyHometown.Text = "";
            // 
            // txtUserCreated
            // 
            this.txtUserCreated.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserCreated.Depth = 0;
            this.txtUserCreated.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtUserCreated.Hint = "Người lập gia phả";
            this.txtUserCreated.Location = new System.Drawing.Point(32, 231);
            this.txtUserCreated.MaxLength = 50;
            this.txtUserCreated.ModeNumber_Maximum = 999999;
            this.txtUserCreated.MouseState = MaterialSkin.MouseState.OUT;
            this.txtUserCreated.Multiline = false;
            this.txtUserCreated.Name = "txtUserCreated";
            this.txtUserCreated.Size = new System.Drawing.Size(472, 50);
            this.txtUserCreated.TabIndex = 5;
            this.txtUserCreated.Text = "";
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnContinue.AutoSize = false;
            this.btnContinue.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnContinue.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnContinue.Depth = 0;
            this.btnContinue.DrawShadows = true;
            this.btnContinue.HighEmphasis = true;
            this.btnContinue.Icon = null;
            this.btnContinue.Location = new System.Drawing.Point(80, 290);
            this.btnContinue.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnContinue.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(84, 36);
            this.btnContinue.TabIndex = 6;
            this.btnContinue.Text = "Tiếp tục";
            this.btnContinue.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnContinue.UseAccentColor = false;
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.AutoSize = false;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Depth = 0;
            this.btnClose.DrawShadows = true;
            this.btnClose.HighEmphasis = true;
            this.btnClose.Icon = null;
            this.btnClose.Location = new System.Drawing.Point(375, 290);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnClose.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(84, 36);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Đóng";
            this.btnClose.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnClose.UseAccentColor = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // materialLabel1
            // 
            this.materialLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 34F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.FontType = MaterialSkin.MaterialSkinManager.fontType.H4;
            this.materialLabel1.Location = new System.Drawing.Point(36, 14);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(468, 41);
            this.materialLabel1.TabIndex = 7;
            this.materialLabel1.Text = "Thông Tin Dòng Họ";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btncalender
            // 
            this.btncalender.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btncalender.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btncalender.Depth = 0;
            this.btncalender.DrawShadows = true;
            this.btncalender.HighEmphasis = true;
            this.btncalender.Icon = null;
            this.btncalender.Location = new System.Drawing.Point(239, 126);
            this.btncalender.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btncalender.MouseState = MaterialSkin.MouseState.HOVER;
            this.btncalender.Name = "btncalender";
            this.btncalender.Size = new System.Drawing.Size(98, 36);
            this.btncalender.TabIndex = 28;
            this.btncalender.Text = "Chọn lịch";
            this.btncalender.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btncalender.UseAccentColor = true;
            this.btncalender.UseVisualStyleBackColor = true;
            this.btncalender.Click += new System.EventHandler(this.btncalender_Click);
            // 
            // FamilyInfoNewPage
            // 
            this.AcceptButton = this.btnContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btncalender);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.txtUserCreated);
            this.Controls.Add(this.txtFamilyHometown);
            this.Controls.Add(this.txtFamilyLevel);
            this.Controls.Add(this.txtFamilyAnniversary);
            this.Controls.Add(this.txtFamilyName);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FamilyInfoNewPage";
            this.Sizable = false;
            this.Size = new System.Drawing.Size(539, 346);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialTextBox txtFamilyName;
        private MaterialSkin.Controls.MaterialTextBox txtFamilyAnniversary;
        private MaterialSkin.Controls.MaterialTextBox txtFamilyLevel;
        private MaterialSkin.Controls.MaterialTextBox txtFamilyHometown;
        private MaterialSkin.Controls.MaterialTextBox txtUserCreated;
        private MaterialSkin.Controls.MaterialButton btnContinue;
        private MaterialSkin.Controls.MaterialButton btnClose;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialButton btncalender;
    }
}
