namespace DSF602.View
{
    public partial class DeviceManagement
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
        /// 
        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabMngt = new System.Windows.Forms.TabControl();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::DSF602.Properties.Resources.GraphType1;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 100);
            this.panel2.TabIndex = 0;
            // 
            // tabMngt
            // 
            this.tabMngt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMngt.Location = new System.Drawing.Point(0, 0);
            this.tabMngt.Margin = new System.Windows.Forms.Padding(4);
            this.tabMngt.Name = "tabMngt";
            this.tabMngt.SelectedIndex = 0;
            this.tabMngt.Size = new System.Drawing.Size(1156, 564);
            this.tabMngt.TabIndex = 21;
            // 
            // DeviceManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1156, 564);
            this.Controls.Add(this.tabMngt);
            this.DoubleBuffered = true;
            this.EscToClose = true;
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(938, 467);
            this.Name = "DeviceManagement";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Device Management";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DeviceManagement_FormClosed);
            this.Load += new System.EventHandler(this.DeviceManagement_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.TabControl tabMngt;
    }
}