namespace NoMySql
{
    partial class Management
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
            this.dvcManagement = new BaseCommon.DeviceManagement();
            this.SuspendLayout();
            // 
            // dvcManagement
            // 
            this.dvcManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvcManagement.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dvcManagement.ListDevice = null;
            this.dvcManagement.Location = new System.Drawing.Point(0, 0);
            this.dvcManagement.Margin = new System.Windows.Forms.Padding(4);
            this.dvcManagement.Name = "dvcManagement";
            this.dvcManagement.Size = new System.Drawing.Size(1027, 411);
            this.dvcManagement.TabIndex = 0;
            // 
            // Management
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1027, 411);
            this.Controls.Add(this.dvcManagement);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(938, 450);
            this.Name = "Management";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Setting Management";
            this.ResumeLayout(false);

        }

        #endregion

        private BaseCommon.DeviceManagement dvcManagement;
    }
}