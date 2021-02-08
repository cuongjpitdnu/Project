using System.Windows.Forms;

namespace DSF602.View.GraphLayout
{
    partial class BlockView
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
            this.lbDeviceTitle = new System.Windows.Forms.Panel();
            this.pnSensor = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lbDeviceTitle
            // 
            this.lbDeviceTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbDeviceTitle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDeviceTitle.Location = new System.Drawing.Point(0, 0);
            this.lbDeviceTitle.Name = "lbDeviceTitle";
            this.lbDeviceTitle.Size = new System.Drawing.Size(385, 29);
            this.lbDeviceTitle.TabIndex = 0;
            this.lbDeviceTitle.Paint += new System.Windows.Forms.PaintEventHandler(this.lbDeviceTitle_Paint);
            // 
            // pnSensor
            // 
            this.pnSensor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnSensor.Location = new System.Drawing.Point(0, 29);
            this.pnSensor.Name = "pnSensor";
            this.pnSensor.Size = new System.Drawing.Size(385, 138);
            this.pnSensor.TabIndex = 1;
            // 
            // BlockView
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.pnSensor);
            this.Controls.Add(this.lbDeviceTitle);
            this.DoubleBuffered = true;
            this.Name = "BlockView";
            this.Size = new System.Drawing.Size(385, 167);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel lbDeviceTitle;
        private System.Windows.Forms.Panel pnSensor;
    }
}
