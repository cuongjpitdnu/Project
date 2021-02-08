namespace DSF602.View
{
    partial class AboutForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSoft = new System.Windows.Forms.TabPage();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabCus = new System.Windows.Forms.TabPage();
            this.btnResetApp = new System.Windows.Forms.Button();
            this.lbAlert = new System.Windows.Forms.Label();
            this.btnResetKey = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabSoft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabCus.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSoft);
            this.tabControl1.Controls.Add(this.tabCus);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(505, 299);
            this.tabControl1.TabIndex = 0;
            // 
            // tabSoft
            // 
            this.tabSoft.Controls.Add(this.pictureBox3);
            this.tabSoft.Controls.Add(this.pictureBox2);
            this.tabSoft.Controls.Add(this.pictureBox1);
            this.tabSoft.Controls.Add(this.label1);
            this.tabSoft.Location = new System.Drawing.Point(4, 29);
            this.tabSoft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabSoft.Name = "tabSoft";
            this.tabSoft.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabSoft.Size = new System.Drawing.Size(497, 266);
            this.tabSoft.TabIndex = 0;
            this.tabSoft.Text = "Software";
            this.tabSoft.UseVisualStyleBackColor = true;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::DSF602.Properties.Resources.AKBlogo;
            this.pictureBox3.Location = new System.Drawing.Point(309, 61);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(131, 79);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox3.TabIndex = 11;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::DSF602.Properties.Resources.logoSSD;
            this.pictureBox2.Location = new System.Drawing.Point(181, 61);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(131, 79);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::DSF602.Properties.Resources.logo_systech3;
            this.pictureBox1.Location = new System.Drawing.Point(57, 61);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(131, 79);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(57, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(383, 62);
            this.label1.TabIndex = 8;
            this.label1.Text = "Copyright © 2019 SYSTECH . All Rights Reserved. Application Desgin & Development " +
    "by SHISHIDO ELECTROSTATIC,LTD, SYSTECH and AKBSoftware.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabCus
            // 
            this.tabCus.Controls.Add(this.btnResetApp);
            this.tabCus.Controls.Add(this.lbAlert);
            this.tabCus.Controls.Add(this.btnResetKey);
            this.tabCus.Location = new System.Drawing.Point(4, 29);
            this.tabCus.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabCus.Name = "tabCus";
            this.tabCus.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabCus.Size = new System.Drawing.Size(497, 266);
            this.tabCus.TabIndex = 1;
            this.tabCus.Text = "Customer";
            this.tabCus.UseVisualStyleBackColor = true;
            // 
            // btnResetApp
            // 
            this.btnResetApp.Location = new System.Drawing.Point(245, 135);
            this.btnResetApp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnResetApp.Name = "btnResetApp";
            this.btnResetApp.Size = new System.Drawing.Size(127, 40);
            this.btnResetApp.TabIndex = 14;
            this.btnResetApp.Text = "Reset App";
            this.btnResetApp.UseVisualStyleBackColor = true;
            this.btnResetApp.Click += new System.EventHandler(this.btnResetApp_Click);
            // 
            // lbAlert
            // 
            this.lbAlert.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAlert.Location = new System.Drawing.Point(6, 89);
            this.lbAlert.Name = "lbAlert";
            this.lbAlert.Size = new System.Drawing.Size(483, 23);
            this.lbAlert.TabIndex = 13;
            this.lbAlert.Text = "ResetKey, ResetApp will restart application!";
            this.lbAlert.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnResetKey
            // 
            this.btnResetKey.Location = new System.Drawing.Point(112, 135);
            this.btnResetKey.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnResetKey.Name = "btnResetKey";
            this.btnResetKey.Size = new System.Drawing.Size(127, 40);
            this.btnResetKey.TabIndex = 12;
            this.btnResetKey.Text = "Reset Key";
            this.btnResetKey.UseVisualStyleBackColor = true;
            this.btnResetKey.Click += new System.EventHandler(this.btnResetKey_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 299);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(351, 124);
            this.Name = "AboutForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.tabControl1.ResumeLayout(false);
            this.tabSoft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabCus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSoft;
        private System.Windows.Forms.TabPage tabCus;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnResetApp;
        private System.Windows.Forms.Label lbAlert;
        private System.Windows.Forms.Button btnResetKey;
    }
}