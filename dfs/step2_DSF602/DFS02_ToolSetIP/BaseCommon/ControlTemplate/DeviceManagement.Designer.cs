namespace BaseCommon.ControlTemplate
{
    partial class DeviceManagement
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.SettingLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cboOrdinalDisplay = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btResetDefault = new System.Windows.Forms.Button();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDeviceName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDevice = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btSaveSetting = new System.Windows.Forms.Button();
            this.dgvDevice = new System.Windows.Forms.DataGridView();
            this.colDeviceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDevice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIpAdress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlarmValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFailLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSamples = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderDisplay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRename = new System.Windows.Forms.Button();
            this.txtRename = new System.Windows.Forms.TextBox();
            this.SettingLayout.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingLayout
            // 
            this.SettingLayout.ColumnCount = 1;
            this.SettingLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SettingLayout.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.SettingLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingLayout.Location = new System.Drawing.Point(0, 0);
            this.SettingLayout.Name = "SettingLayout";
            this.SettingLayout.RowCount = 1;
            this.SettingLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SettingLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 536F));
            this.SettingLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.SettingLayout.Size = new System.Drawing.Size(1259, 378);
            this.SettingLayout.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 462F));
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgvDevice, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1253, 372);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightCyan;
            this.panel4.Controls.Add(this.chkActive);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.cboOrdinalDisplay);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.btResetDefault);
            this.panel4.Controls.Add(this.txtPort);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.txtDeviceName);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.txtIpAddress);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.lblDevice);
            this.panel4.Controls.Add(this.btnClear);
            this.panel4.Controls.Add(this.btSaveSetting);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(794, 63);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(456, 306);
            this.panel4.TabIndex = 0;
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(120, 178);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(15, 14);
            this.chkActive.TabIndex = 35;
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(54, 174);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 20);
            this.label6.TabIndex = 34;
            this.label6.Text = "Active :";
            // 
            // cboOrdinalDisplay
            // 
            this.cboOrdinalDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOrdinalDisplay.FormattingEnabled = true;
            this.cboOrdinalDisplay.Location = new System.Drawing.Point(120, 138);
            this.cboOrdinalDisplay.Name = "cboOrdinalDisplay";
            this.cboOrdinalDisplay.Size = new System.Drawing.Size(121, 28);
            this.cboOrdinalDisplay.TabIndex = 31;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 142);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 20);
            this.label4.TabIndex = 30;
            this.label4.Text = "Ordinal Display:";
            // 
            // btResetDefault
            // 
            this.btResetDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btResetDefault.Location = new System.Drawing.Point(299, 228);
            this.btResetDefault.Margin = new System.Windows.Forms.Padding(4);
            this.btResetDefault.Name = "btResetDefault";
            this.btResetDefault.Size = new System.Drawing.Size(126, 43);
            this.btResetDefault.TabIndex = 11;
            this.btResetDefault.Text = "Reset Default";
            this.btResetDefault.UseVisualStyleBackColor = true;
            this.btResetDefault.Click += new System.EventHandler(this.btResetDefault_Click);
            // 
            // txtPort
            // 
            this.txtPort.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtPort.Location = new System.Drawing.Point(120, 105);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.MaxLength = 5;
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(334, 26);
            this.txtPort.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 108);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 20);
            this.label2.TabIndex = 24;
            this.label2.Text = "Port :";
            // 
            // txtDeviceName
            // 
            this.txtDeviceName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtDeviceName.Location = new System.Drawing.Point(120, 37);
            this.txtDeviceName.Margin = new System.Windows.Forms.Padding(4);
            this.txtDeviceName.MaxLength = 14;
            this.txtDeviceName.Name = "txtDeviceName";
            this.txtDeviceName.Size = new System.Drawing.Size(334, 26);
            this.txtDeviceName.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 40);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 20);
            this.label9.TabIndex = 25;
            this.label9.Text = "Device Name :";
            // 
            // txtIpAddress
            // 
            this.txtIpAddress.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtIpAddress.Location = new System.Drawing.Point(120, 71);
            this.txtIpAddress.Margin = new System.Windows.Forms.Padding(4);
            this.txtIpAddress.MaxLength = 14;
            this.txtIpAddress.Name = "txtIpAddress";
            this.txtIpAddress.Size = new System.Drawing.Size(334, 26);
            this.txtIpAddress.TabIndex = 2;
            this.txtIpAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIpAddress_KeyPress);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 74);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 20);
            this.label1.TabIndex = 25;
            this.label1.Text = "IP Address :";
            // 
            // lblDevice
            // 
            this.lblDevice.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblDevice.BackColor = System.Drawing.Color.Goldenrod;
            this.lblDevice.Location = new System.Drawing.Point(2, 2);
            this.lblDevice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDevice.Name = "lblDevice";
            this.lblDevice.Size = new System.Drawing.Size(452, 26);
            this.lblDevice.TabIndex = 22;
            this.lblDevice.Text = "Device Setting";
            this.lblDevice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnClear.Location = new System.Drawing.Point(165, 228);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(126, 43);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btSaveSetting
            // 
            this.btSaveSetting.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btSaveSetting.Location = new System.Drawing.Point(31, 228);
            this.btSaveSetting.Margin = new System.Windows.Forms.Padding(4);
            this.btSaveSetting.Name = "btSaveSetting";
            this.btSaveSetting.Size = new System.Drawing.Size(126, 43);
            this.btSaveSetting.TabIndex = 9;
            this.btSaveSetting.Text = "Save ";
            this.btSaveSetting.UseVisualStyleBackColor = true;
            this.btSaveSetting.Click += new System.EventHandler(this.btSaveSetting_Click);
            // 
            // dgvDevice
            // 
            this.dgvDevice.AllowUserToAddRows = false;
            this.dgvDevice.AllowUserToDeleteRows = false;
            this.dgvDevice.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDevice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDevice.ColumnHeadersHeight = 26;
            this.dgvDevice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDeviceId,
            this.colDevice,
            this.colDeviceName,
            this.colIpAdress,
            this.colPort,
            this.colActive,
            this.colAlarmValue,
            this.colPeriod,
            this.colFailLevel,
            this.colSamples,
            this.OrderDisplay});
            this.dgvDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDevice.EnableHeadersVisualStyles = false;
            this.dgvDevice.Location = new System.Drawing.Point(4, 64);
            this.dgvDevice.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDevice.MultiSelect = false;
            this.dgvDevice.Name = "dgvDevice";
            this.dgvDevice.RowHeadersVisible = false;
            this.dgvDevice.RowTemplate.Height = 25;
            this.dgvDevice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDevice.Size = new System.Drawing.Size(783, 304);
            this.dgvDevice.TabIndex = 1;
            this.dgvDevice.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDevice_CellClick);
            // 
            // colDeviceId
            // 
            this.colDeviceId.DataPropertyName = "DeviceId";
            this.colDeviceId.HeaderText = "Device ID";
            this.colDeviceId.Name = "colDeviceId";
            this.colDeviceId.ReadOnly = true;
            this.colDeviceId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDeviceId.Visible = false;
            // 
            // colDevice
            // 
            this.colDevice.DataPropertyName = "DeviceType";
            this.colDevice.HeaderText = "Device";
            this.colDevice.Name = "colDevice";
            this.colDevice.ReadOnly = true;
            this.colDevice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDevice.Visible = false;
            this.colDevice.Width = 120;
            // 
            // colDeviceName
            // 
            this.colDeviceName.DataPropertyName = "DeviceName";
            this.colDeviceName.HeaderText = "Device";
            this.colDeviceName.Name = "colDeviceName";
            this.colDeviceName.ReadOnly = true;
            this.colDeviceName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colIpAdress
            // 
            this.colIpAdress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colIpAdress.DataPropertyName = "IpAddress";
            this.colIpAdress.HeaderText = "IP Address";
            this.colIpAdress.Name = "colIpAdress";
            this.colIpAdress.ReadOnly = true;
            this.colIpAdress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colPort
            // 
            this.colPort.DataPropertyName = "Port";
            this.colPort.HeaderText = "Port";
            this.colPort.Name = "colPort";
            this.colPort.ReadOnly = true;
            this.colPort.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPort.Width = 110;
            // 
            // colActive
            // 
            this.colActive.DataPropertyName = "Active";
            this.colActive.HeaderText = "Active";
            this.colActive.Name = "colActive";
            this.colActive.ReadOnly = true;
            this.colActive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colActive.Visible = false;
            // 
            // colAlarmValue
            // 
            this.colAlarmValue.DataPropertyName = "AlarmValue";
            this.colAlarmValue.HeaderText = "Alarm Value";
            this.colAlarmValue.Name = "colAlarmValue";
            this.colAlarmValue.ReadOnly = true;
            this.colAlarmValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAlarmValue.Visible = false;
            // 
            // colPeriod
            // 
            this.colPeriod.DataPropertyName = "Period";
            this.colPeriod.HeaderText = "Period";
            this.colPeriod.Name = "colPeriod";
            this.colPeriod.ReadOnly = true;
            this.colPeriod.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPeriod.Visible = false;
            // 
            // colFailLevel
            // 
            this.colFailLevel.DataPropertyName = "FailLevel";
            this.colFailLevel.HeaderText = "Fail Level";
            this.colFailLevel.Name = "colFailLevel";
            this.colFailLevel.ReadOnly = true;
            this.colFailLevel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colFailLevel.Visible = false;
            // 
            // colSamples
            // 
            this.colSamples.DataPropertyName = "Samples";
            this.colSamples.HeaderText = "Samples";
            this.colSamples.Name = "colSamples";
            this.colSamples.ReadOnly = true;
            this.colSamples.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSamples.Visible = false;
            // 
            // OrderDisplay
            // 
            this.OrderDisplay.DataPropertyName = "OrderDisplay";
            this.OrderDisplay.HeaderText = "OrderDisplay";
            this.OrderDisplay.Name = "OrderDisplay";
            this.OrderDisplay.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRename);
            this.panel1.Controls.Add(this.txtRename);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(785, 54);
            this.panel1.TabIndex = 2;
            // 
            // btnRename
            // 
            this.btnRename.Location = new System.Drawing.Point(235, 14);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(88, 26);
            this.btnRename.TabIndex = 1;
            this.btnRename.Text = "Rename";
            this.btnRename.UseVisualStyleBackColor = true;
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // txtRename
            // 
            this.txtRename.Location = new System.Drawing.Point(3, 14);
            this.txtRename.Name = "txtRename";
            this.txtRename.Size = new System.Drawing.Size(226, 26);
            this.txtRename.TabIndex = 0;
            // 
            // DeviceManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SettingLayout);
            this.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DeviceManagement";
            this.Size = new System.Drawing.Size(1259, 378);
            this.SettingLayout.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion


        private System.Windows.Forms.TableLayoutPanel SettingLayout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboOrdinalDisplay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btResetDefault;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDeviceName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btSaveSetting;
        private System.Windows.Forms.DataGridView dgvDevice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeviceId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDevice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeviceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIpAdress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlarmValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeriod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFailLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSamples;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderDisplay;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnRename;
        private System.Windows.Forms.TextBox txtRename;
    }
}
