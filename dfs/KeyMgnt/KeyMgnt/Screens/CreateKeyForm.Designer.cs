namespace KeyMgnt.Screens
{
    partial class CreateKeyForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateKeyForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvDevices = new KeyMgnt.Common.DataGridViewBase(this.components);
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MacAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceUserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deviceCreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnFile = new System.Windows.Forms.Button();
            this.btnAddDevice = new System.Windows.Forms.Button();
            this.txtMac3 = new System.Windows.Forms.TextBox();
            this.btnCreateKey = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.txtMac4 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtMac5 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMac2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMac1 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMachineCode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(871, 379);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label14);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(865, 56);
            this.panel1.TabIndex = 0;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label14.Location = new System.Drawing.Point(291, 16);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(283, 25);
            this.label14.TabIndex = 43;
            this.label14.Text = "Add Key For Customer";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.dgvDevices, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 65);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(865, 311);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // dgvDevices
            // 
            this.dgvDevices.AllowUserToAddRows = false;
            this.dgvDevices.AllowUserToDeleteRows = false;
            this.dgvDevices.BackgroundColor = System.Drawing.Color.Ivory;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDevices.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDevices.ColumnHeadersHeight = 26;
            this.dgvDevices.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.DeviceName,
            this.MacAddress,
            this.deviceUserId,
            this.deviceCreateDate});
            this.dgvDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDevices.EnableHeadersVisualStyles = false;
            this.dgvDevices.Location = new System.Drawing.Point(435, 3);
            this.dgvDevices.MultiSelect = false;
            this.dgvDevices.Name = "dgvDevices";
            this.dgvDevices.ReadOnly = true;
            this.dgvDevices.RowHeadersVisible = false;
            this.dgvDevices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDevices.Size = new System.Drawing.Size(427, 305);
            this.dgvDevices.TabIndex = 46;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // DeviceName
            // 
            this.DeviceName.DataPropertyName = "DeviceName";
            this.DeviceName.HeaderText = "DeviceName";
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.ReadOnly = true;
            // 
            // MacAddress
            // 
            this.MacAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.MacAddress.DataPropertyName = "MacAddress";
            this.MacAddress.HeaderText = "MacAddress";
            this.MacAddress.Name = "MacAddress";
            this.MacAddress.ReadOnly = true;
            // 
            // deviceUserId
            // 
            this.deviceUserId.DataPropertyName = "UserId";
            this.deviceUserId.HeaderText = "CreatedBy";
            this.deviceUserId.Name = "deviceUserId";
            this.deviceUserId.ReadOnly = true;
            this.deviceUserId.Visible = false;
            // 
            // deviceCreateDate
            // 
            this.deviceCreateDate.DataPropertyName = "CreateDate";
            this.deviceCreateDate.HeaderText = "CreateDate";
            this.deviceCreateDate.Name = "deviceCreateDate";
            this.deviceCreateDate.ReadOnly = true;
            this.deviceCreateDate.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Azure;
            this.panel2.Controls.Add(this.btnExport);
            this.panel2.Controls.Add(this.btnFile);
            this.panel2.Controls.Add(this.btnAddDevice);
            this.panel2.Controls.Add(this.txtMac3);
            this.panel2.Controls.Add(this.btnCreateKey);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.txtMac4);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.txtMac5);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.txtMac2);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.txtMac1);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.txtMachineCode);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(426, 305);
            this.panel2.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.SystemColors.Control;
            this.btnExport.Location = new System.Drawing.Point(260, 259);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(76, 31);
            this.btnExport.TabIndex = 60;
            this.btnExport.Text = "ExportKey";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnFile
            // 
            this.btnFile.BackColor = System.Drawing.SystemColors.Control;
            this.btnFile.Location = new System.Drawing.Point(348, 17);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(75, 26);
            this.btnFile.TabIndex = 59;
            this.btnFile.Text = "Load File";
            this.btnFile.UseVisualStyleBackColor = false;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // btnAddDevice
            // 
            this.btnAddDevice.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddDevice.Location = new System.Drawing.Point(90, 259);
            this.btnAddDevice.Name = "btnAddDevice";
            this.btnAddDevice.Size = new System.Drawing.Size(82, 31);
            this.btnAddDevice.TabIndex = 7;
            this.btnAddDevice.Text = "AddDevice";
            this.btnAddDevice.UseVisualStyleBackColor = false;
            this.btnAddDevice.Click += new System.EventHandler(this.btnAddDevice_Click);
            // 
            // txtMac3
            // 
            this.txtMac3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMac3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMac3.Location = new System.Drawing.Point(89, 156);
            this.txtMac3.Margin = new System.Windows.Forms.Padding(4);
            this.txtMac3.MaxLength = 25;
            this.txtMac3.Name = "txtMac3";
            this.txtMac3.Size = new System.Drawing.Size(249, 26);
            this.txtMac3.TabIndex = 4;
            // 
            // btnCreateKey
            // 
            this.btnCreateKey.BackColor = System.Drawing.SystemColors.Control;
            this.btnCreateKey.Location = new System.Drawing.Point(178, 259);
            this.btnCreateKey.Name = "btnCreateKey";
            this.btnCreateKey.Size = new System.Drawing.Size(76, 31);
            this.btnCreateKey.TabIndex = 8;
            this.btnCreateKey.Text = "CreateKey";
            this.btnCreateKey.UseVisualStyleBackColor = false;
            this.btnCreateKey.Click += new System.EventHandler(this.btnCreateKey_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(4, 194);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(57, 18);
            this.label13.TabIndex = 58;
            this.label13.Text = "MAC4:";
            // 
            // txtMac4
            // 
            this.txtMac4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMac4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMac4.Location = new System.Drawing.Point(89, 190);
            this.txtMac4.Margin = new System.Windows.Forms.Padding(4);
            this.txtMac4.MaxLength = 25;
            this.txtMac4.Name = "txtMac4";
            this.txtMac4.Size = new System.Drawing.Size(249, 26);
            this.txtMac4.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(4, 228);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 18);
            this.label12.TabIndex = 57;
            this.label12.Text = "MAC5:";
            // 
            // txtMac5
            // 
            this.txtMac5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMac5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMac5.Location = new System.Drawing.Point(89, 224);
            this.txtMac5.Margin = new System.Windows.Forms.Padding(4);
            this.txtMac5.MaxLength = 25;
            this.txtMac5.Name = "txtMac5";
            this.txtMac5.Size = new System.Drawing.Size(249, 26);
            this.txtMac5.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(4, 160);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 18);
            this.label10.TabIndex = 56;
            this.label10.Text = "MAC3:";
            // 
            // txtMac2
            // 
            this.txtMac2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMac2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMac2.Location = new System.Drawing.Point(89, 122);
            this.txtMac2.Margin = new System.Windows.Forms.Padding(4);
            this.txtMac2.MaxLength = 25;
            this.txtMac2.Name = "txtMac2";
            this.txtMac2.Size = new System.Drawing.Size(249, 26);
            this.txtMac2.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(4, 126);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 18);
            this.label9.TabIndex = 55;
            this.label9.Text = "MAC2:";
            // 
            // txtMac1
            // 
            this.txtMac1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMac1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMac1.Location = new System.Drawing.Point(89, 88);
            this.txtMac1.Margin = new System.Windows.Forms.Padding(4);
            this.txtMac1.MaxLength = 25;
            this.txtMac1.Name = "txtMac1";
            this.txtMac1.Size = new System.Drawing.Size(249, 26);
            this.txtMac1.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(4, 92);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 18);
            this.label11.TabIndex = 52;
            this.label11.Text = "MAC1:";
            // 
            // txtMachineCode
            // 
            this.txtMachineCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMachineCode.Location = new System.Drawing.Point(89, 17);
            this.txtMachineCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtMachineCode.MaxLength = 500;
            this.txtMachineCode.Multiline = true;
            this.txtMachineCode.Name = "txtMachineCode";
            this.txtMachineCode.Size = new System.Drawing.Size(249, 63);
            this.txtMachineCode.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(4, 21);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(51, 18);
            this.label7.TabIndex = 44;
            this.label7.Text = "Code:";
            // 
            // CreateKeyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(871, 379);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateKeyForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CreateKey";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CreateKeyForm_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevices)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label14;
        private Common.DataGridViewBase dgvDevices;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn MacAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceUserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn deviceCreateDate;
        private System.Windows.Forms.Button btnAddDevice;
        private System.Windows.Forms.TextBox txtMac3;
        private System.Windows.Forms.Button btnCreateKey;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtMac4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtMac5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMac2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMac1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMachineCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.Button btnExport;
    }
}