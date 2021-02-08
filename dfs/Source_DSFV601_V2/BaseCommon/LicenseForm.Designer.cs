namespace BaseCommon
{
    partial class LicenseForm
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
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.lblKey = new System.Windows.Forms.Label();
            this.txtlMachineCode = new System.Windows.Forms.TextBox();
            this.lblMachineCode = new System.Windows.Forms.Label();
            this.btnActivate = new System.Windows.Forms.Button();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.MainLayout.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 1;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.Controls.Add(this.panel1, 0, 0);
            this.MainLayout.Controls.Add(this.panel2, 0, 1);
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 0);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 2;
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.Size = new System.Drawing.Size(435, 369);
            this.MainLayout.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 58);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(90, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(249, 25);
            this.label1.TabIndex = 104;
            this.label1.Text = "This Machine is not activated!";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Arial Narrow", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.Color.Red;
            this.label18.Location = new System.Drawing.Point(11, 29);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(407, 25);
            this.label18.TabIndex = 105;
            this.label18.Text = "Please send the Machine Code  to System Admin!";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtKey);
            this.panel2.Controls.Add(this.lblKey);
            this.panel2.Controls.Add(this.txtlMachineCode);
            this.panel2.Controls.Add(this.lblMachineCode);
            this.panel2.Controls.Add(this.btnLoadFile);
            this.panel2.Controls.Add(this.btnActivate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 67);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(429, 299);
            this.panel2.TabIndex = 1;
            // 
            // txtKey
            // 
            this.txtKey.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtKey.Font = new System.Drawing.Font("Arial Narrow", 12F);
            this.txtKey.Location = new System.Drawing.Point(15, 111);
            this.txtKey.Multiline = true;
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(402, 134);
            this.txtKey.TabIndex = 5;
            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold);
            this.lblKey.Location = new System.Drawing.Point(11, 88);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(32, 20);
            this.lblKey.TabIndex = 4;
            this.lblKey.Text = "Key";
            // 
            // txtlMachineCode
            // 
            this.txtlMachineCode.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtlMachineCode.Font = new System.Drawing.Font("Arial Narrow", 12F);
            this.txtlMachineCode.Location = new System.Drawing.Point(15, 26);
            this.txtlMachineCode.Multiline = true;
            this.txtlMachineCode.Name = "txtlMachineCode";
            this.txtlMachineCode.ReadOnly = true;
            this.txtlMachineCode.Size = new System.Drawing.Size(402, 59);
            this.txtlMachineCode.TabIndex = 3;
            // 
            // lblMachineCode
            // 
            this.lblMachineCode.AutoSize = true;
            this.lblMachineCode.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold);
            this.lblMachineCode.Location = new System.Drawing.Point(11, 3);
            this.lblMachineCode.Name = "lblMachineCode";
            this.lblMachineCode.Size = new System.Drawing.Size(97, 20);
            this.lblMachineCode.TabIndex = 2;
            this.lblMachineCode.Text = "Machine Code";
            // 
            // btnActivate
            // 
            this.btnActivate.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActivate.Location = new System.Drawing.Point(217, 251);
            this.btnActivate.Name = "btnActivate";
            this.btnActivate.Size = new System.Drawing.Size(97, 42);
            this.btnActivate.TabIndex = 7;
            this.btnActivate.Text = "Activate";
            this.btnActivate.UseVisualStyleBackColor = true;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadFile.Location = new System.Drawing.Point(114, 251);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(97, 42);
            this.btnLoadFile.TabIndex = 6;
            this.btnLoadFile.Text = "Load File";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.BtnLoadFile_Click);
            // 
            // LicenseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(435, 369);
            this.Controls.Add(this.MainLayout);
            this.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LicenseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "STATIRON DSF601 Control Software";
            this.MainLayout.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainLayout;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtKey;
        private System.Windows.Forms.Label lblKey;
        private System.Windows.Forms.TextBox txtlMachineCode;
        private System.Windows.Forms.Label lblMachineCode;
        private System.Windows.Forms.Button btnActivate;
        private System.Windows.Forms.Button btnLoadFile;
    }
}