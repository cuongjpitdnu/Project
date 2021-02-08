namespace DSF602.View
{
    partial class MapDevices
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
            this.plnMain = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // plnMain
            // 
            this.plnMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plnMain.BackColor = System.Drawing.Color.LightGray;
            this.plnMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plnMain.Location = new System.Drawing.Point(14, 49);
            this.plnMain.Name = "plnMain";
            this.plnMain.Size = new System.Drawing.Size(1344, 525);
            this.plnMain.TabIndex = 0;
            // 
            // MapDevices
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(1370, 591);
            this.Controls.Add(this.plnMain);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(351, 124);
            this.Name = "MapDevices";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plnMain;
    }
}