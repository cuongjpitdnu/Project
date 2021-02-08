namespace DSF602.View.GraphLayout
{
    partial class SensorItemView
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
            this.pnMain = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnMain
            // 
            this.pnMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnMain.BackColor = System.Drawing.Color.White;
            this.pnMain.Location = new System.Drawing.Point(8, 0);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new System.Drawing.Size(120, 128);
            this.pnMain.TabIndex = 5;
            this.pnMain.Click += new System.EventHandler(this.SensorItemView_Click);
            this.pnMain.Paint += new System.Windows.Forms.PaintEventHandler(this.pnMain_Paint);
            // 
            // SensorItemView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.pnMain);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SensorItemView";
            this.Size = new System.Drawing.Size(136, 148);
            this.Click += new System.EventHandler(this.SensorItemView_Click);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnMain;
    }
}
