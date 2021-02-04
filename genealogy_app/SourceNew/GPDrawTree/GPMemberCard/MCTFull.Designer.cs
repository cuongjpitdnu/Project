namespace GPMemberCard
{
    partial class MemberCardTemplFull
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
            this.picFrame = new System.Windows.Forms.PictureBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.lblDeadDate = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // picImage
            // 
            this.picImage.Location = new System.Drawing.Point(44, 8);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(125, 125);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImage.TabIndex = 2;
            this.picImage.TabStop = false;
            // 
            // picFrame
            // 
            this.picFrame.Location = new System.Drawing.Point(0, 3);
            this.picFrame.Name = "picFrame";
            this.picFrame.Size = new System.Drawing.Size(208, 276);
            this.picFrame.TabIndex = 1;
            this.picFrame.TabStop = false;
            this.picFrame.Visible = false;
            // 
            // lblFullName
            // 
            this.lblFullName.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFullName.Location = new System.Drawing.Point(0, 138);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(207, 33);
            this.lblFullName.TabIndex = 3;
            this.lblFullName.Text = "Họ và Tên";
            this.lblFullName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBirthDate.Location = new System.Drawing.Point(5, 171);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(195, 27);
            this.lblBirthDate.TabIndex = 4;
            this.lblBirthDate.Text = "Ngày Sinh Dương Lịch";
            this.lblBirthDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDeadDate
            // 
            this.lblDeadDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeadDate.Location = new System.Drawing.Point(5, 205);
            this.lblDeadDate.Name = "lblDeadDate";
            this.lblDeadDate.Size = new System.Drawing.Size(195, 27);
            this.lblDeadDate.TabIndex = 5;
            this.lblDeadDate.Text = "Ngày Mất";
            this.lblDeadDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLevel
            // 
            this.lblLevel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevel.Location = new System.Drawing.Point(5, 234);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(195, 27);
            this.lblLevel.TabIndex = 6;
            this.lblLevel.Text = "Đời";
            this.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MemberCardTemplFull
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.lblDeadDate);
            this.Controls.Add(this.lblBirthDate);
            this.Controls.Add(this.lblFullName);
            this.Controls.Add(this.picImage);
            this.Controls.Add(this.picFrame);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MemberCardTemplFull";
            this.Size = new System.Drawing.Size(207, 281);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFrame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.PictureBox picFrame;
        public System.Windows.Forms.PictureBox picImage;
        public System.Windows.Forms.Label lblFullName;
        public System.Windows.Forms.Label lblBirthDate;
        public System.Windows.Forms.Label lblDeadDate;
        public System.Windows.Forms.Label lblLevel;
    }
}
