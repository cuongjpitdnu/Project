namespace DFS02_ToolSetIP
{
    partial class ManagerInternetPotocol
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
            this.grbIPSetting = new System.Windows.Forms.GroupBox();
            this.txtDNS = new System.Windows.Forms.TextBox();
            this.txtGateway = new System.Windows.Forms.TextBox();
            this.txtSubMask = new System.Windows.Forms.TextBox();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDefaul = new BaseCommon.ControlTemplate.btn();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCurrentIp = new System.Windows.Forms.TextBox();
            this.txtIpChangeDefaul = new System.Windows.Forms.TextBox();
            this.btnReset = new BaseCommon.ControlTemplate.btn();
            this.cboNetwork = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnConnect = new BaseCommon.ControlTemplate.btn();
            this.btnChange = new BaseCommon.ControlTemplate.btn();
            this.txtIpChange = new System.Windows.Forms.TextBox();
            this.grbIPSetting.SuspendLayout();
            this.SuspendLayout();
            // 
            // grbIPSetting
            // 
            this.grbIPSetting.Controls.Add(this.txtDNS);
            this.grbIPSetting.Controls.Add(this.txtGateway);
            this.grbIPSetting.Controls.Add(this.txtSubMask);
            this.grbIPSetting.Controls.Add(this.txtIpAddress);
            this.grbIPSetting.Controls.Add(this.label4);
            this.grbIPSetting.Controls.Add(this.label3);
            this.grbIPSetting.Controls.Add(this.label2);
            this.grbIPSetting.Controls.Add(this.label1);
            this.grbIPSetting.Location = new System.Drawing.Point(25, 49);
            this.grbIPSetting.Name = "grbIPSetting";
            this.grbIPSetting.Size = new System.Drawing.Size(386, 210);
            this.grbIPSetting.TabIndex = 1;
            this.grbIPSetting.TabStop = false;
            this.grbIPSetting.Text = "Ip config";
            // 
            // txtDNS
            // 
            this.txtDNS.Location = new System.Drawing.Point(198, 155);
            this.txtDNS.Name = "txtDNS";
            this.txtDNS.ReadOnly = true;
            this.txtDNS.Size = new System.Drawing.Size(168, 26);
            this.txtDNS.TabIndex = 1;
            // 
            // txtGateway
            // 
            this.txtGateway.Location = new System.Drawing.Point(198, 113);
            this.txtGateway.Name = "txtGateway";
            this.txtGateway.ReadOnly = true;
            this.txtGateway.Size = new System.Drawing.Size(168, 26);
            this.txtGateway.TabIndex = 1;
            // 
            // txtSubMask
            // 
            this.txtSubMask.Location = new System.Drawing.Point(198, 71);
            this.txtSubMask.Name = "txtSubMask";
            this.txtSubMask.ReadOnly = true;
            this.txtSubMask.Size = new System.Drawing.Size(168, 26);
            this.txtSubMask.TabIndex = 1;
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Location = new System.Drawing.Point(198, 29);
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.ReadOnly = true;
            this.txtIpAddress.Size = new System.Drawing.Size(168, 26);
            this.txtIpAddress.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(144, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "Preferred DNS Server:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "Default gateway:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Subnet mask:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Ip address:";
            // 
            // btnDefaul
            // 
            this.btnDefaul.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDefaul.Location = new System.Drawing.Point(25, 265);
            this.btnDefaul.Name = "btnDefaul";
            this.btnDefaul.Shortcut = System.Windows.Forms.Keys.None;
            this.btnDefaul.Size = new System.Drawing.Size(102, 30);
            this.btnDefaul.TabIndex = 1;
            this.btnDefaul.Text = "Defaul Setting";
            this.btnDefaul.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnDefaul.UseVisualStyleBackColor = true;
            this.btnDefaul.Click += new System.EventHandler(this.btnDefaul_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 322);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Current IP";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 360);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 20);
            this.label6.TabIndex = 2;
            this.label6.Text = "Ip Change";
            // 
            // txtCurrentIp
            // 
            this.txtCurrentIp.Location = new System.Drawing.Point(109, 319);
            this.txtCurrentIp.Name = "txtCurrentIp";
            this.txtCurrentIp.Size = new System.Drawing.Size(168, 26);
            this.txtCurrentIp.TabIndex = 3;
            // 
            // txtIpChangeDefaul
            // 
            this.txtIpChangeDefaul.Location = new System.Drawing.Point(109, 357);
            this.txtIpChangeDefaul.Name = "txtIpChangeDefaul";
            this.txtIpChangeDefaul.Size = new System.Drawing.Size(126, 26);
            this.txtIpChangeDefaul.TabIndex = 3;
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(149, 265);
            this.btnReset.Name = "btnReset";
            this.btnReset.Shortcut = System.Windows.Forms.Keys.None;
            this.btnReset.Size = new System.Drawing.Size(102, 30);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "Reset";
            this.btnReset.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // cboNetwork
            // 
            this.cboNetwork.FormattingEnabled = true;
            this.cboNetwork.Location = new System.Drawing.Point(92, 10);
            this.cboNetwork.Name = "cboNetwork";
            this.cboNetwork.Size = new System.Drawing.Size(299, 28);
            this.cboNetwork.TabIndex = 5;
            this.cboNetwork.SelectedIndexChanged += new System.EventHandler(this.cboNetwork_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Network";
            // 
            // btnConnect
            // 
            this.btnConnect.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.Location = new System.Drawing.Point(320, 317);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Shortcut = System.Windows.Forms.Keys.None;
            this.btnConnect.Size = new System.Drawing.Size(91, 30);
            this.btnConnect.TabIndex = 7;
            this.btnConnect.Text = "Connect";
            this.btnConnect.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnChange
            // 
            this.btnChange.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChange.Location = new System.Drawing.Point(320, 355);
            this.btnChange.Name = "btnChange";
            this.btnChange.Shortcut = System.Windows.Forms.Keys.None;
            this.btnChange.Size = new System.Drawing.Size(91, 30);
            this.btnChange.TabIndex = 7;
            this.btnChange.Text = "Change Ip";
            this.btnChange.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnChange.UseVisualStyleBackColor = true;
            // 
            // txtIpChange
            // 
            this.txtIpChange.Location = new System.Drawing.Point(241, 357);
            this.txtIpChange.Name = "txtIpChange";
            this.txtIpChange.Size = new System.Drawing.Size(36, 26);
            this.txtIpChange.TabIndex = 8;
            // 
            // ManagerInternetPotocol
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 463);
            this.Controls.Add(this.txtIpChange);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cboNetwork);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.txtIpChangeDefaul);
            this.Controls.Add(this.txtCurrentIp);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnDefaul);
            this.Controls.Add(this.grbIPSetting);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(351, 123);
            this.Name = "ManagerInternetPotocol";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manager Internet Protocol";
            this.Load += new System.EventHandler(this.ManagerInternetPotocol_Load);
            this.grbIPSetting.ResumeLayout(false);
            this.grbIPSetting.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grbIPSetting;
        private System.Windows.Forms.TextBox txtDNS;
        private System.Windows.Forms.TextBox txtGateway;
        private System.Windows.Forms.TextBox txtSubMask;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private BaseCommon.ControlTemplate.btn btnDefaul;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCurrentIp;
        private System.Windows.Forms.TextBox txtIpChangeDefaul;
        private BaseCommon.ControlTemplate.btn btnReset;
        private System.Windows.Forms.ComboBox cboNetwork;
        private System.Windows.Forms.Label label7;
        private BaseCommon.ControlTemplate.btn btnConnect;
        private BaseCommon.ControlTemplate.btn btnChange;
        private System.Windows.Forms.TextBox txtIpChange;
    }
}

