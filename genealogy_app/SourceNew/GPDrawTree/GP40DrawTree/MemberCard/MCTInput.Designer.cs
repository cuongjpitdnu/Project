namespace GP40Common
{
    partial class MemberCardTemplInput
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
            this.picImage = new System.Windows.Forms.PictureBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.lblDeadDate = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblHomeTown = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // picImage
            // 
            this.picImage.Location = new System.Drawing.Point(10, 14);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(105, 140);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImage.TabIndex = 2;
            this.picImage.TabStop = false;
            // 
            // lblFullName
            // 
            this.lblFullName.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFullName.Location = new System.Drawing.Point(117, 16);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(326, 23);
            this.lblFullName.TabIndex = 3;
            this.lblFullName.Text = "Họ và Tên";
            this.lblFullName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBirthDate.Location = new System.Drawing.Point(117, 48);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(326, 27);
            this.lblBirthDate.TabIndex = 4;
            this.lblBirthDate.Text = "Ngày Sinh Dương Lịch";
            this.lblBirthDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDeadDate
            // 
            this.lblDeadDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeadDate.Location = new System.Drawing.Point(117, 85);
            this.lblDeadDate.Name = "lblDeadDate";
            this.lblDeadDate.Size = new System.Drawing.Size(326, 27);
            this.lblDeadDate.TabIndex = 5;
            this.lblDeadDate.Text = "Ngày Mất";
            this.lblDeadDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLevel
            // 
            this.lblLevel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevel.Location = new System.Drawing.Point(8, 158);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(108, 27);
            this.lblLevel.TabIndex = 6;
            this.lblLevel.Text = "Đời";
            this.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHomeTown
            // 
            this.lblHomeTown.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHomeTown.Location = new System.Drawing.Point(117, 125);
            this.lblHomeTown.Name = "lblHomeTown";
            this.lblHomeTown.Size = new System.Drawing.Size(326, 60);
            this.lblHomeTown.TabIndex = 7;
            this.lblHomeTown.Text = "Quê Quán";
            // 
            // MemberCardTemplInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblHomeTown);
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.lblDeadDate);
            this.Controls.Add(this.lblBirthDate);
            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.picImage);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MemberCardTemplInput";
            this.Size = new System.Drawing.Size(446, 189);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.PictureBox picImage;
        public System.Windows.Forms.Label lblFullName;
        public System.Windows.Forms.Label lblBirthDate;
        public System.Windows.Forms.Label lblDeadDate;
        public System.Windows.Forms.Label lblLevel;
        public System.Windows.Forms.Label lblHomeTown;
    }
}
