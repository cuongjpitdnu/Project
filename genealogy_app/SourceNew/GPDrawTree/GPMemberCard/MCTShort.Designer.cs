namespace GPMemberCard
{
    partial class MemberCardTemplShort
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
            this.lblFullName = new System.Windows.Forms.Label();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.lblDeadDate = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFullName
            // 
            this.lblFullName.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFullName.Location = new System.Drawing.Point(3, 16);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.Size = new System.Drawing.Size(234, 23);
            this.lblFullName.TabIndex = 3;
            this.lblFullName.Text = "Họ và Tên";
            this.lblFullName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBirthDate.Location = new System.Drawing.Point(3, 49);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(234, 27);
            this.lblBirthDate.TabIndex = 4;
            this.lblBirthDate.Text = "Ngày Sinh Dương Lịch";
            this.lblBirthDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDeadDate
            // 
            this.lblDeadDate.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeadDate.Location = new System.Drawing.Point(3, 83);
            this.lblDeadDate.Name = "lblDeadDate";
            this.lblDeadDate.Size = new System.Drawing.Size(234, 27);
            this.lblDeadDate.TabIndex = 5;
            this.lblDeadDate.Text = "Ngày Mất";
            this.lblDeadDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLevel
            // 
            this.lblLevel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevel.Location = new System.Drawing.Point(3, 112);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(234, 27);
            this.lblLevel.TabIndex = 6;
            this.lblLevel.Text = "Đời";
            this.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MemberCardTemplShort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.lblDeadDate);
            this.Controls.Add(this.lblBirthDate);
            this.Controls.Add(this.lblFullName);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MemberCardTemplShort";
            this.Size = new System.Drawing.Size(240, 148);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Label lblFullName;
        public System.Windows.Forms.Label lblBirthDate;
        public System.Windows.Forms.Label lblDeadDate;
        public System.Windows.Forms.Label lblLevel;
    }
}
