namespace Systech_Tool_Setting
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.btCreateSetting = new System.Windows.Forms.Button();
            this.rdVersion1 = new System.Windows.Forms.RadioButton();
            this.rdVersion2 = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP Address :";
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Location = new System.Drawing.Point(127, 26);
            this.txtIpAddress.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtIpAddress.MaxLength = 15;
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(221, 26);
            this.txtIpAddress.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(78, 63);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port :";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(127, 60);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPort.MaxLength = 5;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(221, 26);
            this.txtPort.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 97);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Username :";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(127, 94);
            this.txtUser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtUser.MaxLength = 128;
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(221, 26);
            this.txtUser.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 131);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Password :";
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(127, 128);
            this.txtPass.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPass.MaxLength = 128;
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(221, 26);
            this.txtPass.TabIndex = 7;
            this.txtPass.UseSystemPasswordChar = true;
            // 
            // btCreateSetting
            // 
            this.btCreateSetting.Location = new System.Drawing.Point(127, 223);
            this.btCreateSetting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btCreateSetting.Name = "btCreateSetting";
            this.btCreateSetting.Size = new System.Drawing.Size(119, 45);
            this.btCreateSetting.TabIndex = 8;
            this.btCreateSetting.Text = "Create Setting";
            this.btCreateSetting.UseVisualStyleBackColor = true;
            this.btCreateSetting.Click += new System.EventHandler(this.btCreateSetting_Click);
            // 
            // rdVersion1
            // 
            this.rdVersion1.AutoSize = true;
            this.rdVersion1.Location = new System.Drawing.Point(127, 175);
            this.rdVersion1.Name = "rdVersion1";
            this.rdVersion1.Size = new System.Drawing.Size(97, 24);
            this.rdVersion1.TabIndex = 9;
            this.rdVersion1.Text = "DSF601_V1";
            this.rdVersion1.UseVisualStyleBackColor = true;
            this.rdVersion1.CheckedChanged += new System.EventHandler(this.rdVersion1_CheckedChanged);
            // 
            // rdVersion2
            // 
            this.rdVersion2.AutoSize = true;
            this.rdVersion2.Checked = true;
            this.rdVersion2.Location = new System.Drawing.Point(251, 175);
            this.rdVersion2.Name = "rdVersion2";
            this.rdVersion2.Size = new System.Drawing.Size(97, 24);
            this.rdVersion2.TabIndex = 10;
            this.rdVersion2.TabStop = true;
            this.rdVersion2.Text = "DSF601_V2";
            this.rdVersion2.UseVisualStyleBackColor = true;
            this.rdVersion2.CheckedChanged += new System.EventHandler(this.rdVersion2_CheckedChanged);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(384, 282);
            this.Controls.Add(this.rdVersion2);
            this.Controls.Add(this.rdVersion1);
            this.Controls.Add(this.btCreateSetting);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIpAddress);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AKB - Systech Tool Setting MySQL and AppConfig";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Button btCreateSetting;
        private System.Windows.Forms.RadioButton rdVersion1;
        private System.Windows.Forms.RadioButton rdVersion2;
    }
}

