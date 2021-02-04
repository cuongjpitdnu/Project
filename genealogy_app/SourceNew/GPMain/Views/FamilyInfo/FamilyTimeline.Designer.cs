namespace GPMain.Views.FamilyInfo
{
    partial class FamilyTimeline
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
            this.btnClose = new MaterialSkin.Controls.MaterialButton();
            this.btnSave = new MaterialSkin.Controls.MaterialButton();
            this.txtStartDate = new MaterialSkin.Controls.MaterialTextBox();
            this.txtEndDate = new MaterialSkin.Controls.MaterialTextBox();
            this.txtContent = new MaterialSkin.Controls.MaterialMultiLineTextBox();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = false;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.Depth = 0;
            this.btnClose.DrawShadows = true;
            this.btnClose.HighEmphasis = true;
            this.btnClose.Icon = null;
            this.btnClose.Location = new System.Drawing.Point(225, 312);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnClose.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(84, 36);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Đóng";
            this.btnClose.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnClose.UseAccentColor = false;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = false;
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Depth = 0;
            this.btnSave.DrawShadows = true;
            this.btnSave.HighEmphasis = true;
            this.btnSave.Icon = null;
            this.btnSave.Location = new System.Drawing.Point(53, 312);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 36);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Lưu";
            this.btnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnSave.UseAccentColor = false;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtStartDate
            // 
            this.txtStartDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtStartDate.Depth = 0;
            this.txtStartDate.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtStartDate.Hint = "Bắt đầu";
            this.txtStartDate.Location = new System.Drawing.Point(12, 97);
            this.txtStartDate.MaxLength = 50;
            this.txtStartDate.ModeNumber_Maximum = 999999;
            this.txtStartDate.MouseState = MaterialSkin.MouseState.OUT;
            this.txtStartDate.Multiline = false;
            this.txtStartDate.Name = "txtStartDate";
            this.txtStartDate.Size = new System.Drawing.Size(166, 50);
            this.txtStartDate.TabIndex = 1;
            this.txtStartDate.Text = "";
            this.txtStartDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStartDate_KeyPress);
            // 
            // txtEndDate
            // 
            this.txtEndDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEndDate.Depth = 0;
            this.txtEndDate.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtEndDate.Hint = "Kết thúc";
            this.txtEndDate.Location = new System.Drawing.Point(184, 97);
            this.txtEndDate.MaxLength = 50;
            this.txtEndDate.ModeNumber_Maximum = 999999;
            this.txtEndDate.MouseState = MaterialSkin.MouseState.OUT;
            this.txtEndDate.Multiline = false;
            this.txtEndDate.Name = "txtEndDate";
            this.txtEndDate.Size = new System.Drawing.Size(166, 50);
            this.txtEndDate.TabIndex = 2;
            this.txtEndDate.Text = "";
            this.txtEndDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtStartDate_KeyPress);
            // 
            // txtContent
            // 
            this.txtContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtContent.Depth = 0;
            this.txtContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtContent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtContent.Hint = "Nội dung";
            this.txtContent.Location = new System.Drawing.Point(12, 154);
            this.txtContent.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(338, 149);
            this.txtContent.TabIndex = 3;
            this.txtContent.Text = "";
            // 
            // materialLabel1
            // 
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 34F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.FontType = MaterialSkin.MaterialSkinManager.fontType.H4;
            this.materialLabel1.Location = new System.Drawing.Point(12, 13);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(338, 46);
            this.materialLabel1.TabIndex = 6;
            this.materialLabel1.Text = "Lịch Sử Dòng Họ";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FamilyTimeline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MintCream;
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtEndDate);
            this.Controls.Add(this.txtStartDate);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FamilyTimeline";
            this.Sizable = false;
            this.Size = new System.Drawing.Size(362, 380);
            this.ResumeLayout(false);

        }

        #endregion
        private MaterialSkin.Controls.MaterialButton btnClose;
        private MaterialSkin.Controls.MaterialButton btnSave;
        private MaterialSkin.Controls.MaterialTextBox txtStartDate;
        private MaterialSkin.Controls.MaterialTextBox txtEndDate;
        private MaterialSkin.Controls.MaterialMultiLineTextBox txtContent;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
    }
}
