namespace DSF602.View.GraphLayout
{
    partial class MapLocation
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
            this.plnMain = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // plnMain
            // 
            this.plnMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.plnMain.AutoScroll = true;
            this.plnMain.BackColor = System.Drawing.Color.White;
            this.plnMain.Location = new System.Drawing.Point(227, 28);
            this.plnMain.Name = "plnMain";
            this.plnMain.Size = new System.Drawing.Size(160, 257);
            this.plnMain.TabIndex = 0;
            this.plnMain.Scroll += new System.Windows.Forms.ScrollEventHandler(this.plnMain_Scroll);
            // 
            // MapLocation
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.plnMain);
            this.Name = "MapLocation";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel plnMain;
    }
}
