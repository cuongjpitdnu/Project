namespace BaseCommon
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.chkWalkingMode = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboOrdinalDisplay = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btResetDefault = new System.Windows.Forms.Button();
            this.txtSamples = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDeviceName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtIpAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblDevice = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtFailLevel = new System.Windows.Forms.TextBox();
            this.txtPeriod = new System.Windows.Forms.TextBox();
            this.txtAlarmValue = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btSaveSetting = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.SettingLayout.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).BeginInit();
            this.panel4.SuspendLayout();
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
            this.SettingLayout.Size = new System.Drawing.Size(1259, 536);
            this.SettingLayout.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 462F));
            this.tableLayoutPanel1.Controls.Add(this.dgvDevice, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1253, 530);
            this.tableLayoutPanel1.TabIndex = 0;
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
            this.dgvDevice.Location = new System.Drawing.Point(4, 4);
            this.dgvDevice.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDevice.MultiSelect = false;
            this.dgvDevice.Name = "dgvDevice";
            this.dgvDevice.RowHeadersVisible = false;
            this.dgvDevice.RowTemplate.Height = 25;
            this.dgvDevice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDevice.Size = new System.Drawing.Size(783, 522);
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
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightCyan;
            this.panel4.Controls.Add(this.chkActive);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.chkWalkingMode);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.cboOrdinalDisplay);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.btResetDefault);
            this.panel4.Controls.Add(this.txtSamples);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.txtPort);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.txtDeviceName);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.txtIpAddress);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.lblDevice);
            this.panel4.Controls.Add(this.label17);
            this.panel4.Controls.Add(this.txtFailLevel);
            this.panel4.Controls.Add(this.txtPeriod);
            this.panel4.Controls.Add(this.txtAlarmValue);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.label16);
            this.panel4.Controls.Add(this.label15);
            this.panel4.Controls.Add(this.btnClear);
            this.panel4.Controls.Add(this.btSaveSetting);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(794, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(456, 524);
            this.panel4.TabIndex = 0;
            // 
            // chkWalkingMode
            // 
            this.chkWalkingMode.AutoSize = true;
            this.chkWalkingMode.Location = new System.Drawing.Point(369, 146);
            this.chkWalkingMode.Name = "chkWalkingMode";
            this.chkWalkingMode.Size = new System.Drawing.Size(15, 14);
            this.chkWalkingMode.TabIndex = 32;
            this.chkWalkingMode.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(254, 142);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 20);
            this.label5.TabIndex = 33;
            this.label5.Text = "Walking Mode :";
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
            this.btResetDefault.Location = new System.Drawing.Point(299, 349);
            this.btResetDefault.Margin = new System.Windows.Forms.Padding(4);
            this.btResetDefault.Name = "btResetDefault";
            this.btResetDefault.Size = new System.Drawing.Size(126, 43);
            this.btResetDefault.TabIndex = 11;
            this.btResetDefault.Text = "Reset Default";
            this.btResetDefault.UseVisualStyleBackColor = true;
            this.btResetDefault.Click += new System.EventHandler(this.btResetDefault_Click);
            // 
            // txtSamples
            // 
            this.txtSamples.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtSamples.Location = new System.Drawing.Point(350, 312);
            this.txtSamples.Margin = new System.Windows.Forms.Padding(4);
            this.txtSamples.MaxLength = 6;
            this.txtSamples.Name = "txtSamples";
            this.txtSamples.Size = new System.Drawing.Size(104, 26);
            this.txtSamples.TabIndex = 8;
            this.txtSamples.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label13
            // 
            this.label13.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(254, 315);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 20);
            this.label13.TabIndex = 28;
            this.label13.Text = "Samples :";
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
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.BackColor = System.Drawing.Color.Khaki;
            this.label3.Location = new System.Drawing.Point(232, 210);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(222, 26);
            this.label3.TabIndex = 22;
            this.label3.Text = "Walking Test Mode Setting";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // label17
            // 
            this.label17.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label17.BackColor = System.Drawing.Color.Gold;
            this.label17.Location = new System.Drawing.Point(2, 210);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(222, 26);
            this.label17.TabIndex = 22;
            this.label17.Text = "Alarm Test Mode Setting";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtFailLevel
            // 
            this.txtFailLevel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtFailLevel.Location = new System.Drawing.Point(350, 278);
            this.txtFailLevel.Margin = new System.Windows.Forms.Padding(4);
            this.txtFailLevel.MaxLength = 6;
            this.txtFailLevel.Name = "txtFailLevel";
            this.txtFailLevel.Size = new System.Drawing.Size(104, 26);
            this.txtFailLevel.TabIndex = 7;
            this.txtFailLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPeriod
            // 
            this.txtPeriod.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtPeriod.Location = new System.Drawing.Point(350, 244);
            this.txtPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.txtPeriod.MaxLength = 6;
            this.txtPeriod.Name = "txtPeriod";
            this.txtPeriod.Size = new System.Drawing.Size(104, 26);
            this.txtPeriod.TabIndex = 6;
            this.txtPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtAlarmValue
            // 
            this.txtAlarmValue.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtAlarmValue.Location = new System.Drawing.Point(120, 244);
            this.txtAlarmValue.Margin = new System.Windows.Forms.Padding(4);
            this.txtAlarmValue.MaxLength = 6;
            this.txtAlarmValue.Name = "txtAlarmValue";
            this.txtAlarmValue.Size = new System.Drawing.Size(104, 26);
            this.txtAlarmValue.TabIndex = 5;
            this.txtAlarmValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(247, 247);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 20);
            this.label14.TabIndex = 19;
            this.label14.Text = "Period (s) :";
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(249, 281);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(75, 20);
            this.label16.TabIndex = 18;
            this.label16.Text = "Fail Level :";
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 247);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(89, 20);
            this.label15.TabIndex = 16;
            this.label15.Text = "Alarm Value :";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnClear.Location = new System.Drawing.Point(165, 349);
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
            this.btSaveSetting.Location = new System.Drawing.Point(31, 349);
            this.btSaveSetting.Margin = new System.Windows.Forms.Padding(4);
            this.btSaveSetting.Name = "btSaveSetting";
            this.btSaveSetting.Size = new System.Drawing.Size(126, 43);
            this.btSaveSetting.TabIndex = 9;
            this.btSaveSetting.Text = "Save ";
            this.btSaveSetting.UseVisualStyleBackColor = true;
            this.btSaveSetting.Click += new System.EventHandler(this.btSaveSetting_Click);
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
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(120, 178);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(15, 14);
            this.chkActive.TabIndex = 35;
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // DeviceManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SettingLayout);
            this.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DeviceManagement";
            this.Size = new System.Drawing.Size(1259, 536);
            this.SettingLayout.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel SettingLayout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgvDevice;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btResetDefault;
        private System.Windows.Forms.TextBox txtSamples;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDeviceName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtFailLevel;
        private System.Windows.Forms.TextBox txtPeriod;
        private System.Windows.Forms.TextBox txtAlarmValue;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btSaveSetting;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboOrdinalDisplay;
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
        private System.Windows.Forms.CheckBox chkWalkingMode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label label6;
    }
}
