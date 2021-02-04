namespace GP40Main.Views.Member
{
    partial class popupAddRelation
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
            this.cmbRelationship = new MaterialSkin.Controls.MaterialComboBox();
            this.chkboxAddFromList = new MaterialSkin.Controls.MaterialCheckbox();
            this.btnNext = new MaterialSkin.Controls.MaterialButton();
            this.SuspendLayout();
            // 
            // cmbRelationship
            // 
            this.cmbRelationship.AutoResize = false;
            this.cmbRelationship.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cmbRelationship.Depth = 0;
            this.cmbRelationship.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbRelationship.DropDownHeight = 174;
            this.cmbRelationship.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRelationship.DropDownWidth = 121;
            this.cmbRelationship.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cmbRelationship.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbRelationship.FormattingEnabled = true;
            this.cmbRelationship.IntegralHeight = false;
            this.cmbRelationship.ItemHeight = 43;
            this.cmbRelationship.Location = new System.Drawing.Point(16, 21);
            this.cmbRelationship.MaxDropDownItems = 4;
            this.cmbRelationship.MouseState = MaterialSkin.MouseState.OUT;
            this.cmbRelationship.Name = "cmbRelationship";
            this.cmbRelationship.Size = new System.Drawing.Size(325, 49);
            this.cmbRelationship.TabIndex = 1;
            // 
            // chkboxAddFromList
            // 
            this.chkboxAddFromList.AutoSize = true;
            this.chkboxAddFromList.Depth = 0;
            this.chkboxAddFromList.Location = new System.Drawing.Point(92, 83);
            this.chkboxAddFromList.Margin = new System.Windows.Forms.Padding(0);
            this.chkboxAddFromList.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkboxAddFromList.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkboxAddFromList.Name = "chkboxAddFromList";
            this.chkboxAddFromList.Ripple = true;
            this.chkboxAddFromList.Size = new System.Drawing.Size(173, 37);
            this.chkboxAddFromList.TabIndex = 2;
            this.chkboxAddFromList.Text = "Thêm từ danh sách";
            this.chkboxAddFromList.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnNext.Depth = 0;
            this.btnNext.DrawShadows = true;
            this.btnNext.HighEmphasis = true;
            this.btnNext.Icon = null;
            this.btnNext.Location = new System.Drawing.Point(133, 126);
            this.btnNext.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnNext.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(90, 36);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "Thêm mới";
            this.btnNext.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnNext.UseAccentColor = false;
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // popupAddRelation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.chkboxAddFromList);
            this.Controls.Add(this.cmbRelationship);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "popupAddRelation";
            this.Sizable = false;
            this.Size = new System.Drawing.Size(358, 180);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MaterialSkin.Controls.MaterialComboBox cmbRelationship;
        private MaterialSkin.Controls.MaterialCheckbox chkboxAddFromList;
        private MaterialSkin.Controls.MaterialButton btnNext;
    }
}
