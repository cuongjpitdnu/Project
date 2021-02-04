namespace GP40Main.Views.Config
{
    partial class popupAddNewRelation
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
            this.lblMain = new MaterialSkin.Controls.MaterialLabel();
            this.cboMainPrefix = new MaterialSkin.Controls.MaterialComboBox();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.cboRelatedPrefix = new MaterialSkin.Controls.MaterialComboBox();
            this.txtMainName = new MaterialSkin.Controls.MaterialTextBox();
            this.materialTextBox2 = new MaterialSkin.Controls.MaterialTextBox();
            this.txtRelatedName = new MaterialSkin.Controls.MaterialTextBox();
            this.btnSave = new MaterialSkin.Controls.MaterialButton();
            this.btnClose = new MaterialSkin.Controls.MaterialButton();
            this.chkIsMain = new MaterialSkin.Controls.MaterialCheckbox();
            this.SuspendLayout();
            // 
            // lblMain
            // 
            this.lblMain.AutoSize = true;
            this.lblMain.Depth = 0;
            this.lblMain.Font = new System.Drawing.Font("Roboto", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lblMain.FontType = MaterialSkin.MaterialSkinManager.fontType.H3;
            this.lblMain.Location = new System.Drawing.Point(48, 22);
            this.lblMain.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblMain.Name = "lblMain";
            this.lblMain.Size = new System.Drawing.Size(319, 58);
            this.lblMain.TabIndex = 0;
            this.lblMain.Text = "materialLabel1";
            // 
            // cboMainPrefix
            // 
            this.cboMainPrefix.AutoResize = false;
            this.cboMainPrefix.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cboMainPrefix.Depth = 0;
            this.cboMainPrefix.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboMainPrefix.DropDownHeight = 174;
            this.cboMainPrefix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMainPrefix.DropDownWidth = 121;
            this.cboMainPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cboMainPrefix.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboMainPrefix.FormattingEnabled = true;
            this.cboMainPrefix.IntegralHeight = false;
            this.cboMainPrefix.ItemHeight = 43;
            this.cboMainPrefix.Location = new System.Drawing.Point(162, 118);
            this.cboMainPrefix.MaxDropDownItems = 4;
            this.cboMainPrefix.MouseState = MaterialSkin.MouseState.OUT;
            this.cboMainPrefix.Name = "cboMainPrefix";
            this.cboMainPrefix.Size = new System.Drawing.Size(121, 49);
            this.cboMainPrefix.TabIndex = 0;
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.Location = new System.Drawing.Point(53, 133);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(103, 19);
            this.materialLabel2.TabIndex = 2;
            this.materialLabel2.Text = "Quan hệ chính";
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel3.Location = new System.Drawing.Point(18, 188);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(138, 19);
            this.materialLabel3.TabIndex = 2;
            this.materialLabel3.Text = "Quan hệ tương ứng";
            // 
            // cboRelatedPrefix
            // 
            this.cboRelatedPrefix.AutoResize = false;
            this.cboRelatedPrefix.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cboRelatedPrefix.Depth = 0;
            this.cboRelatedPrefix.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboRelatedPrefix.DropDownHeight = 174;
            this.cboRelatedPrefix.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRelatedPrefix.DropDownWidth = 121;
            this.cboRelatedPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cboRelatedPrefix.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboRelatedPrefix.FormattingEnabled = true;
            this.cboRelatedPrefix.IntegralHeight = false;
            this.cboRelatedPrefix.ItemHeight = 43;
            this.cboRelatedPrefix.Location = new System.Drawing.Point(162, 173);
            this.cboRelatedPrefix.MaxDropDownItems = 4;
            this.cboRelatedPrefix.MouseState = MaterialSkin.MouseState.OUT;
            this.cboRelatedPrefix.Name = "cboRelatedPrefix";
            this.cboRelatedPrefix.Size = new System.Drawing.Size(121, 49);
            this.cboRelatedPrefix.TabIndex = 2;
            // 
            // txtMainName
            // 
            this.txtMainName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMainName.Depth = 0;
            this.txtMainName.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtMainName.Hint = "Tên hiển thị";
            this.txtMainName.Location = new System.Drawing.Point(289, 117);
            this.txtMainName.MaxLength = 50;
            this.txtMainName.MouseState = MaterialSkin.MouseState.OUT;
            this.txtMainName.Multiline = false;
            this.txtMainName.Name = "txtMainName";
            this.txtMainName.Size = new System.Drawing.Size(203, 50);
            this.txtMainName.TabIndex = 1;
            this.txtMainName.Text = "";
            // 
            // materialTextBox2
            // 
            this.materialTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.materialTextBox2.Depth = 0;
            this.materialTextBox2.Font = new System.Drawing.Font("Roboto", 12F);
            this.materialTextBox2.Location = new System.Drawing.Point(289, 173);
            this.materialTextBox2.MaxLength = 50;
            this.materialTextBox2.MouseState = MaterialSkin.MouseState.OUT;
            this.materialTextBox2.Multiline = false;
            this.materialTextBox2.Name = "materialTextBox2";
            this.materialTextBox2.Size = new System.Drawing.Size(203, 50);
            this.materialTextBox2.TabIndex = 3;
            this.materialTextBox2.Text = "";
            // 
            // txtRelatedName
            // 
            this.txtRelatedName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtRelatedName.Depth = 0;
            this.txtRelatedName.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtRelatedName.Hint = "Tên hiển thị";
            this.txtRelatedName.Location = new System.Drawing.Point(289, 172);
            this.txtRelatedName.MaxLength = 50;
            this.txtRelatedName.MouseState = MaterialSkin.MouseState.OUT;
            this.txtRelatedName.Multiline = false;
            this.txtRelatedName.Name = "txtRelatedName";
            this.txtRelatedName.Size = new System.Drawing.Size(203, 50);
            this.txtRelatedName.TabIndex = 3;
            this.txtRelatedName.Text = "";
            // 
            // btnSave
            // 
            this.btnSave.AutoSize = false;
            this.btnSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSave.Depth = 0;
            this.btnSave.DrawShadows = true;
            this.btnSave.HighEmphasis = true;
            this.btnSave.Icon = null;
            this.btnSave.Location = new System.Drawing.Point(162, 272);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(83, 49);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Lưu";
            this.btnSave.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnSave.UseAccentColor = true;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.AutoSize = false;
            this.btnClose.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnClose.Depth = 0;
            this.btnClose.DrawShadows = true;
            this.btnClose.HighEmphasis = true;
            this.btnClose.Icon = null;
            this.btnClose.Location = new System.Drawing.Point(253, 272);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnClose.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(83, 49);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Đóng";
            this.btnClose.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnClose.UseAccentColor = true;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkIsMain
            // 
            this.chkIsMain.AutoSize = true;
            this.chkIsMain.Depth = 0;
            this.chkIsMain.Location = new System.Drawing.Point(162, 229);
            this.chkIsMain.Margin = new System.Windows.Forms.Padding(0);
            this.chkIsMain.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkIsMain.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkIsMain.Name = "chkIsMain";
            this.chkIsMain.Ripple = true;
            this.chkIsMain.Size = new System.Drawing.Size(83, 37);
            this.chkIsMain.TabIndex = 6;
            this.chkIsMain.Text = "isMain";
            this.chkIsMain.UseVisualStyleBackColor = true;
            // 
            // popupAddNewRelation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.Controls.Add(this.chkIsMain);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtRelatedName);
            this.Controls.Add(this.materialTextBox2);
            this.Controls.Add(this.txtMainName);
            this.Controls.Add(this.materialLabel3);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.cboRelatedPrefix);
            this.Controls.Add(this.cboMainPrefix);
            this.Controls.Add(this.lblMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "popupAddNewRelation";
            this.Size = new System.Drawing.Size(514, 340);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel lblMain;
        private MaterialSkin.Controls.MaterialComboBox cboMainPrefix;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialComboBox cboRelatedPrefix;
        private MaterialSkin.Controls.MaterialTextBox txtMainName;
        private MaterialSkin.Controls.MaterialTextBox materialTextBox2;
        private MaterialSkin.Controls.MaterialTextBox txtRelatedName;
        private MaterialSkin.Controls.MaterialButton btnSave;
        private MaterialSkin.Controls.MaterialButton btnClose;
        private MaterialSkin.Controls.MaterialCheckbox chkIsMain;
    }
}
