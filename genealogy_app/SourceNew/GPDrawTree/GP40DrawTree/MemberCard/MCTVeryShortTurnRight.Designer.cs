namespace GP40Common
{
    partial class MemberCardTemplVeryShortTurnRight
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
            this.lblLevel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFullName
            // 
            this.lblFullName.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFullName.Location = new System.Drawing.Point(19, 240);
            this.lblFullName.Name = "lblLevel";
            this.lblFullName.Size = new System.Drawing.Size(234, 23);
            this.lblFullName.TabIndex = 3;
            this.lblFullName.Text = "Họ và Tên";
            this.lblFullName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLevel
            // 
            this.lblLevel.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevel.Location = new System.Drawing.Point(55, 240);
            this.lblLevel.Name = "lblFullName";
            this.lblLevel.Size = new System.Drawing.Size(234, 27);
            this.lblLevel.TabIndex = 6;
            this.lblLevel.Text = "Đời";
            this.lblLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MemberCardTemplVeryShortTurn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblLevel);
            this.Controls.Add(this.lblFullName);
            this.Name = "MemberCardTemplVeryShortTurnRight";
            this.Size = new System.Drawing.Size(92, 240);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.Label lblLevel;
    }
}
