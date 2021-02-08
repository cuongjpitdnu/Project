using DSF602.View.ControlLayout;

namespace BaseCommon.ControlTemplate
{
    partial class DeviceLayout
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ckbActiveBlock = new System.Windows.Forms.CheckBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblBlockName = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.lblActiveBlock = new System.Windows.Forms.Label();
            this.txtPortBlock = new System.Windows.Forms.TextBox();
            this.txtBlockName = new System.Windows.Forms.TextBox();
            this.btnSaveBlock = new System.Windows.Forms.Button();
            this.lbl_IpAddress = new System.Windows.Forms.Label();
            this.dgvDevice = new BaseCommon.ControlTemplate.dgv();
            this.colLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlarmValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMeasureTypeShow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDecayTimeCheck = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIonValueCheck = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbMeasureTitle = new System.Windows.Forms.Label();
            this.cboMeasureType = new System.Windows.Forms.ComboBox();
            this.ckbActiveSensor = new System.Windows.Forms.CheckBox();
            this.lblActiveSensor = new System.Windows.Forms.Label();
            this.cboOrdinalDisplay = new System.Windows.Forms.ComboBox();
            this.lblOrdinalDisplay = new System.Windows.Forms.Label();
            this.btResetDefault = new System.Windows.Forms.Button();
            this.txtAlaimValue = new DSF602.View.ControlLayout.NumericTextBox();
            this.lblAlaimValue = new System.Windows.Forms.Label();
            this.txtSensorName = new System.Windows.Forms.TextBox();
            this.lblSensorName = new System.Windows.Forms.Label();
            this.lblSensorSetting = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btSaveSetting = new System.Windows.Forms.Button();
            this.btnSetAllAlarm = new System.Windows.Forms.Button();
            this.lblHeaderText = new System.Windows.Forms.Label();
            this.grpBlockSetting = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnValueMode = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtIonStopTimeCheck = new DSF602.View.ControlLayout.NumericTextBox();
            this.txtIonCheck = new DSF602.View.ControlLayout.NumericTextBox();
            this.lbIonStopTime = new System.Windows.Forms.Label();
            this.lbIonBalance = new System.Windows.Forms.Label();
            this.txtDecayStopTime = new DSF602.View.ControlLayout.NumericTextBox();
            this.txtDecayTime = new DSF602.View.ControlLayout.NumericTextBox();
            this.txtLowVal = new DSF602.View.ControlLayout.NumericTextBox();
            this.txtUpVal = new DSF602.View.ControlLayout.NumericTextBox();
            this.lbDecayStopTime = new System.Windows.Forms.Label();
            this.lbDecayTime = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbUpVal = new System.Windows.Forms.Label();
            this.lbLowVal = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpDecay = new System.Windows.Forms.GroupBox();
            this.grpIon = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).BeginInit();
            this.grpBlockSetting.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnValueMode.SuspendLayout();
            this.panel3.SuspendLayout();
            this.grpDecay.SuspendLayout();
            this.grpIon.SuspendLayout();
            this.SuspendLayout();
            // 
            // ckbActiveBlock
            // 
            this.ckbActiveBlock.AutoSize = true;
            this.ckbActiveBlock.Location = new System.Drawing.Point(453, 31);
            this.ckbActiveBlock.Name = "ckbActiveBlock";
            this.ckbActiveBlock.Size = new System.Drawing.Size(15, 14);
            this.ckbActiveBlock.TabIndex = 44;
            this.ckbActiveBlock.UseVisualStyleBackColor = true;
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(783, 28);
            this.lblPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPort.Size = new System.Drawing.Size(80, 20);
            this.lblPort.TabIndex = 24;
            this.lblPort.Text = "Port";
            // 
            // lblBlockName
            // 
            this.lblBlockName.Location = new System.Drawing.Point(95, 28);
            this.lblBlockName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBlockName.Name = "lblBlockName";
            this.lblBlockName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblBlockName.Size = new System.Drawing.Size(99, 20);
            this.lblBlockName.TabIndex = 24;
            this.lblBlockName.Text = "Block Name";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(629, 25);
            this.txtIp.Margin = new System.Windows.Forms.Padding(4);
            this.txtIp.MaxLength = 14;
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(142, 26);
            this.txtIp.TabIndex = 2;
            this.txtIp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIpAddress_KeyPress);
            // 
            // lblActiveBlock
            // 
            this.lblActiveBlock.Location = new System.Drawing.Point(347, 28);
            this.lblActiveBlock.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActiveBlock.Name = "lblActiveBlock";
            this.lblActiveBlock.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblActiveBlock.Size = new System.Drawing.Size(99, 20);
            this.lblActiveBlock.TabIndex = 42;
            this.lblActiveBlock.Text = "Active";
            // 
            // txtPortBlock
            // 
            this.txtPortBlock.Location = new System.Drawing.Point(867, 25);
            this.txtPortBlock.Margin = new System.Windows.Forms.Padding(4);
            this.txtPortBlock.MaxLength = 5;
            this.txtPortBlock.Name = "txtPortBlock";
            this.txtPortBlock.Size = new System.Drawing.Size(79, 26);
            this.txtPortBlock.TabIndex = 3;
            // 
            // txtBlockName
            // 
            this.txtBlockName.Location = new System.Drawing.Point(202, 25);
            this.txtBlockName.Name = "txtBlockName";
            this.txtBlockName.Size = new System.Drawing.Size(142, 26);
            this.txtBlockName.TabIndex = 0;
            // 
            // btnSaveBlock
            // 
            this.btnSaveBlock.Location = new System.Drawing.Point(1039, 20);
            this.btnSaveBlock.Margin = new System.Windows.Forms.Padding(4);
            this.btnSaveBlock.Name = "btnSaveBlock";
            this.btnSaveBlock.Size = new System.Drawing.Size(97, 36);
            this.btnSaveBlock.TabIndex = 9;
            this.btnSaveBlock.Text = "Save ";
            this.btnSaveBlock.UseVisualStyleBackColor = true;
            this.btnSaveBlock.Click += new System.EventHandler(this.btnSaveBlock_Click);
            // 
            // lbl_IpAddress
            // 
            this.lbl_IpAddress.Location = new System.Drawing.Point(522, 28);
            this.lbl_IpAddress.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_IpAddress.Name = "lbl_IpAddress";
            this.lbl_IpAddress.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbl_IpAddress.Size = new System.Drawing.Size(99, 20);
            this.lbl_IpAddress.TabIndex = 25;
            this.lbl_IpAddress.Text = "IP Address";
            // 
            // dgvDevice
            // 
            this.dgvDevice.AllowUserToAddRows = false;
            this.dgvDevice.AllowUserToDeleteRows = false;
            this.dgvDevice.AllowUserToResizeRows = false;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDevice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvDevice.ColumnHeadersHeight = 30;
            this.dgvDevice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLine,
            this.colDeviceName,
            this.colActive,
            this.colAlarmValue,
            this.colMeasureTypeShow,
            this.colDecayTimeCheck,
            this.colIonValueCheck});
            this.dgvDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDevice.EnableHeadersVisualStyles = false;
            this.dgvDevice.Location = new System.Drawing.Point(0, 0);
            this.dgvDevice.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDevice.MultiSelect = false;
            this.dgvDevice.Name = "dgvDevice";
            this.dgvDevice.ReadOnly = true;
            this.dgvDevice.RowHeadersVisible = false;
            this.dgvDevice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDevice.Size = new System.Drawing.Size(761, 460);
            this.dgvDevice.TabIndex = 1;
            this.dgvDevice.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDevice_CellFormatting);
            this.dgvDevice.SelectionChanged += new System.EventHandler(this.dgvDevice_SelectionChanged);
            // 
            // colLine
            // 
            this.colLine.DataPropertyName = "Ordinal_Display";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colLine.DefaultCellStyle = dataGridViewCellStyle12;
            this.colLine.HeaderText = "No";
            this.colLine.Name = "colLine";
            this.colLine.ReadOnly = true;
            this.colLine.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colLine.Width = 50;
            // 
            // colDeviceName
            // 
            this.colDeviceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDeviceName.DataPropertyName = "SensorName";
            this.colDeviceName.HeaderText = "Sensor Name";
            this.colDeviceName.Name = "colDeviceName";
            this.colDeviceName.ReadOnly = true;
            this.colDeviceName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colActive
            // 
            this.colActive.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colActive.DataPropertyName = "Active";
            this.colActive.HeaderText = "Active";
            this.colActive.Name = "colActive";
            this.colActive.ReadOnly = true;
            this.colActive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colActive.Width = 52;
            // 
            // colAlarmValue
            // 
            this.colAlarmValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colAlarmValue.DataPropertyName = "Alarm_Value";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colAlarmValue.DefaultCellStyle = dataGridViewCellStyle13;
            this.colAlarmValue.HeaderText = "Alarm Value";
            this.colAlarmValue.Name = "colAlarmValue";
            this.colAlarmValue.ReadOnly = true;
            this.colAlarmValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAlarmValue.Width = 87;
            // 
            // colMeasureTypeShow
            // 
            this.colMeasureTypeShow.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMeasureTypeShow.DataPropertyName = "MeasureTypeShow";
            this.colMeasureTypeShow.HeaderText = "Measure Type";
            this.colMeasureTypeShow.Name = "colMeasureTypeShow";
            this.colMeasureTypeShow.ReadOnly = true;
            this.colMeasureTypeShow.Width = 120;
            // 
            // colDecayTimeCheck
            // 
            this.colDecayTimeCheck.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colDecayTimeCheck.DataPropertyName = "DecayTimeCheck";
            this.colDecayTimeCheck.HeaderText = "Decay Alarm(s)";
            this.colDecayTimeCheck.Name = "colDecayTimeCheck";
            this.colDecayTimeCheck.ReadOnly = true;
            this.colDecayTimeCheck.Width = 125;
            // 
            // colIonValueCheck
            // 
            this.colIonValueCheck.DataPropertyName = "IonValueCheck";
            this.colIonValueCheck.HeaderText = "IB (V)";
            this.colIonValueCheck.Name = "colIonValueCheck";
            this.colIonValueCheck.ReadOnly = true;
            // 
            // lbMeasureTitle
            // 
            this.lbMeasureTitle.AutoSize = true;
            this.lbMeasureTitle.Font = new System.Drawing.Font("Arial Narrow", 12F);
            this.lbMeasureTitle.Location = new System.Drawing.Point(11, 43);
            this.lbMeasureTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbMeasureTitle.Name = "lbMeasureTitle";
            this.lbMeasureTitle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbMeasureTitle.Size = new System.Drawing.Size(95, 20);
            this.lbMeasureTitle.TabIndex = 45;
            this.lbMeasureTitle.Text = "Measure Type";
            // 
            // cboMeasureType
            // 
            this.cboMeasureType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMeasureType.FormattingEnabled = true;
            this.cboMeasureType.Location = new System.Drawing.Point(147, 40);
            this.cboMeasureType.Name = "cboMeasureType";
            this.cboMeasureType.Size = new System.Drawing.Size(207, 28);
            this.cboMeasureType.TabIndex = 44;
            // 
            // ckbActiveSensor
            // 
            this.ckbActiveSensor.AutoSize = true;
            this.ckbActiveSensor.Location = new System.Drawing.Point(147, 111);
            this.ckbActiveSensor.Name = "ckbActiveSensor";
            this.ckbActiveSensor.Size = new System.Drawing.Size(15, 14);
            this.ckbActiveSensor.TabIndex = 43;
            this.ckbActiveSensor.UseVisualStyleBackColor = true;
            // 
            // lblActiveSensor
            // 
            this.lblActiveSensor.AutoSize = true;
            this.lblActiveSensor.Location = new System.Drawing.Point(11, 107);
            this.lblActiveSensor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblActiveSensor.Name = "lblActiveSensor";
            this.lblActiveSensor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblActiveSensor.Size = new System.Drawing.Size(46, 20);
            this.lblActiveSensor.TabIndex = 42;
            this.lblActiveSensor.Text = "Active";
            // 
            // cboOrdinalDisplay
            // 
            this.cboOrdinalDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOrdinalDisplay.FormattingEnabled = true;
            this.cboOrdinalDisplay.Location = new System.Drawing.Point(147, 74);
            this.cboOrdinalDisplay.Name = "cboOrdinalDisplay";
            this.cboOrdinalDisplay.Size = new System.Drawing.Size(207, 28);
            this.cboOrdinalDisplay.TabIndex = 41;
            // 
            // lblOrdinalDisplay
            // 
            this.lblOrdinalDisplay.AutoSize = true;
            this.lblOrdinalDisplay.Location = new System.Drawing.Point(11, 77);
            this.lblOrdinalDisplay.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOrdinalDisplay.Name = "lblOrdinalDisplay";
            this.lblOrdinalDisplay.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblOrdinalDisplay.Size = new System.Drawing.Size(99, 20);
            this.lblOrdinalDisplay.TabIndex = 40;
            this.lblOrdinalDisplay.Text = "Ordinal Display";
            // 
            // btResetDefault
            // 
            this.btResetDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btResetDefault.Location = new System.Drawing.Point(246, 409);
            this.btResetDefault.Margin = new System.Windows.Forms.Padding(4);
            this.btResetDefault.Name = "btResetDefault";
            this.btResetDefault.Size = new System.Drawing.Size(120, 43);
            this.btResetDefault.TabIndex = 11;
            this.btResetDefault.Text = "Reset Default";
            this.btResetDefault.UseVisualStyleBackColor = true;
            this.btResetDefault.Click += new System.EventHandler(this.btResetDefault_Click);
            // 
            // txtAlaimValue
            // 
            this.txtAlaimValue.Location = new System.Drawing.Point(143, 4);
            this.txtAlaimValue.Name = "txtAlaimValue";
            this.txtAlaimValue.Size = new System.Drawing.Size(207, 26);
            this.txtAlaimValue.TabIndex = 0;
            this.txtAlaimValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblAlaimValue
            // 
            this.lblAlaimValue.AutoSize = true;
            this.lblAlaimValue.Location = new System.Drawing.Point(7, 7);
            this.lblAlaimValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAlaimValue.Name = "lblAlaimValue";
            this.lblAlaimValue.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblAlaimValue.Size = new System.Drawing.Size(81, 20);
            this.lblAlaimValue.TabIndex = 24;
            this.lblAlaimValue.Text = "Alarm Value";
            // 
            // txtSensorName
            // 
            this.txtSensorName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtSensorName.Location = new System.Drawing.Point(147, 7);
            this.txtSensorName.Margin = new System.Windows.Forms.Padding(4);
            this.txtSensorName.MaxLength = 14;
            this.txtSensorName.Name = "txtSensorName";
            this.txtSensorName.Size = new System.Drawing.Size(207, 26);
            this.txtSensorName.TabIndex = 2;
            // 
            // lblSensorName
            // 
            this.lblSensorName.AutoSize = true;
            this.lblSensorName.Location = new System.Drawing.Point(11, 10);
            this.lblSensorName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSensorName.Name = "lblSensorName";
            this.lblSensorName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSensorName.Size = new System.Drawing.Size(91, 20);
            this.lblSensorName.TabIndex = 25;
            this.lblSensorName.Text = "Sensor Name";
            // 
            // lblSensorSetting
            // 
            this.lblSensorSetting.BackColor = System.Drawing.Color.Gold;
            this.lblSensorSetting.Location = new System.Drawing.Point(761, 8);
            this.lblSensorSetting.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSensorSetting.Name = "lblSensorSetting";
            this.lblSensorSetting.Size = new System.Drawing.Size(408, 26);
            this.lblSensorSetting.TabIndex = 22;
            this.lblSensorSetting.Text = "Sensor Setting";
            this.lblSensorSetting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Location = new System.Drawing.Point(147, 409);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(95, 43);
            this.btnClear.TabIndex = 10;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btSaveSetting
            // 
            this.btSaveSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btSaveSetting.Location = new System.Drawing.Point(46, 409);
            this.btSaveSetting.Margin = new System.Windows.Forms.Padding(4);
            this.btSaveSetting.Name = "btSaveSetting";
            this.btSaveSetting.Size = new System.Drawing.Size(97, 43);
            this.btSaveSetting.TabIndex = 9;
            this.btSaveSetting.Text = "Save ";
            this.btSaveSetting.UseVisualStyleBackColor = true;
            this.btSaveSetting.Click += new System.EventHandler(this.btSaveSetting_Click);
            // 
            // btnSetAllAlarm
            // 
            this.btnSetAllAlarm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSetAllAlarm.Location = new System.Drawing.Point(600, 4);
            this.btnSetAllAlarm.Margin = new System.Windows.Forms.Padding(4);
            this.btnSetAllAlarm.Name = "btnSetAllAlarm";
            this.btnSetAllAlarm.Size = new System.Drawing.Size(161, 32);
            this.btnSetAllAlarm.TabIndex = 10;
            this.btnSetAllAlarm.Text = "Set default values";
            this.btnSetAllAlarm.UseVisualStyleBackColor = true;
            this.btnSetAllAlarm.Click += new System.EventHandler(this.BtnSetAllAlarm_Click);
            // 
            // lblHeaderText
            // 
            this.lblHeaderText.AutoSize = true;
            this.lblHeaderText.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderText.Location = new System.Drawing.Point(22, 11);
            this.lblHeaderText.Name = "lblHeaderText";
            this.lblHeaderText.Size = new System.Drawing.Size(126, 20);
            this.lblHeaderText.TabIndex = 0;
            this.lblHeaderText.Text = "List sensor of block";
            // 
            // grpBlockSetting
            // 
            this.grpBlockSetting.BackColor = System.Drawing.Color.LightCyan;
            this.grpBlockSetting.Controls.Add(this.txtBlockName);
            this.grpBlockSetting.Controls.Add(this.ckbActiveBlock);
            this.grpBlockSetting.Controls.Add(this.lbl_IpAddress);
            this.grpBlockSetting.Controls.Add(this.btnSaveBlock);
            this.grpBlockSetting.Controls.Add(this.lblPort);
            this.grpBlockSetting.Controls.Add(this.txtPortBlock);
            this.grpBlockSetting.Controls.Add(this.lblActiveBlock);
            this.grpBlockSetting.Controls.Add(this.lblBlockName);
            this.grpBlockSetting.Controls.Add(this.txtIp);
            this.grpBlockSetting.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpBlockSetting.Location = new System.Drawing.Point(0, 0);
            this.grpBlockSetting.Name = "grpBlockSetting";
            this.grpBlockSetting.Size = new System.Drawing.Size(1173, 68);
            this.grpBlockSetting.TabIndex = 46;
            this.grpBlockSetting.TabStop = false;
            this.grpBlockSetting.Text = "Block Setting";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gold;
            this.panel1.Controls.Add(this.lblHeaderText);
            this.panel1.Controls.Add(this.btnSetAllAlarm);
            this.panel1.Controls.Add(this.lblSensorSetting);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1173, 41);
            this.panel1.TabIndex = 47;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grpIon);
            this.panel2.Controls.Add(this.grpDecay);
            this.panel2.Controls.Add(this.pnValueMode);
            this.panel2.Controls.Add(this.lblSensorName);
            this.panel2.Controls.Add(this.btResetDefault);
            this.panel2.Controls.Add(this.lbMeasureTitle);
            this.panel2.Controls.Add(this.txtSensorName);
            this.panel2.Controls.Add(this.lblOrdinalDisplay);
            this.panel2.Controls.Add(this.cboMeasureType);
            this.panel2.Controls.Add(this.cboOrdinalDisplay);
            this.panel2.Controls.Add(this.ckbActiveSensor);
            this.panel2.Controls.Add(this.btnClear);
            this.panel2.Controls.Add(this.btSaveSetting);
            this.panel2.Controls.Add(this.lblActiveSensor);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(761, 109);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(412, 460);
            this.panel2.TabIndex = 48;
            // 
            // pnValueMode
            // 
            this.pnValueMode.Controls.Add(this.label1);
            this.pnValueMode.Controls.Add(this.lblAlaimValue);
            this.pnValueMode.Controls.Add(this.txtAlaimValue);
            this.pnValueMode.Location = new System.Drawing.Point(4, 368);
            this.pnValueMode.Name = "pnValueMode";
            this.pnValueMode.Size = new System.Drawing.Size(404, 36);
            this.pnValueMode.TabIndex = 55;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(354, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 20);
            this.label1.TabIndex = 62;
            this.label1.Text = "(V)";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(354, 21);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(26, 20);
            this.label25.TabIndex = 61;
            this.label25.Text = "(V)";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(354, 51);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(24, 20);
            this.label24.TabIndex = 60;
            this.label24.Text = "(s)";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(354, 112);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(24, 20);
            this.label23.TabIndex = 59;
            this.label23.Text = "(s)";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(354, 83);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(24, 20);
            this.label22.TabIndex = 58;
            this.label22.Text = "(s)";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(354, 53);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(26, 20);
            this.label21.TabIndex = 57;
            this.label21.Text = "(V)";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(354, 22);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(26, 20);
            this.label20.TabIndex = 56;
            this.label20.Text = "(V)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(177, 53);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(25, 20);
            this.label11.TabIndex = 55;
            this.label11.Text = "+/-";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(177, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 20);
            this.label10.TabIndex = 54;
            this.label10.Text = "+/-";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(177, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 20);
            this.label9.TabIndex = 53;
            this.label9.Text = "+/-";
            // 
            // txtIonStopTimeCheck
            // 
            this.txtIonStopTimeCheck.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIonStopTimeCheck.Location = new System.Drawing.Point(206, 48);
            this.txtIonStopTimeCheck.Name = "txtIonStopTimeCheck";
            this.txtIonStopTimeCheck.Size = new System.Drawing.Size(144, 26);
            this.txtIonStopTimeCheck.TabIndex = 52;
            this.txtIonStopTimeCheck.Text = "5";
            this.txtIonStopTimeCheck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtIonCheck
            // 
            this.txtIonCheck.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIonCheck.Location = new System.Drawing.Point(206, 18);
            this.txtIonCheck.Name = "txtIonCheck";
            this.txtIonCheck.Size = new System.Drawing.Size(144, 26);
            this.txtIonCheck.TabIndex = 51;
            this.txtIonCheck.Text = "35";
            this.txtIonCheck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lbIonStopTime
            // 
            this.lbIonStopTime.AutoSize = true;
            this.lbIonStopTime.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbIonStopTime.Location = new System.Drawing.Point(7, 51);
            this.lbIonStopTime.Name = "lbIonStopTime";
            this.lbIonStopTime.Size = new System.Drawing.Size(109, 20);
            this.lbIonStopTime.TabIndex = 50;
            this.lbIonStopTime.Text = "Stop IB Measure";
            // 
            // lbIonBalance
            // 
            this.lbIonBalance.AutoSize = true;
            this.lbIonBalance.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbIonBalance.Location = new System.Drawing.Point(7, 21);
            this.lbIonBalance.Name = "lbIonBalance";
            this.lbIonBalance.Size = new System.Drawing.Size(128, 20);
            this.lbIonBalance.TabIndex = 49;
            this.lbIonBalance.Text = "Alarm Peak Voltage";
            // 
            // txtDecayStopTime
            // 
            this.txtDecayStopTime.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDecayStopTime.Location = new System.Drawing.Point(206, 109);
            this.txtDecayStopTime.Name = "txtDecayStopTime";
            this.txtDecayStopTime.Size = new System.Drawing.Size(144, 26);
            this.txtDecayStopTime.TabIndex = 46;
            this.txtDecayStopTime.Text = "5";
            this.txtDecayStopTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDecayTime
            // 
            this.txtDecayTime.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDecayTime.Location = new System.Drawing.Point(206, 80);
            this.txtDecayTime.Name = "txtDecayTime";
            this.txtDecayTime.Size = new System.Drawing.Size(144, 26);
            this.txtDecayTime.TabIndex = 45;
            this.txtDecayTime.Text = "3";
            this.txtDecayTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLowVal
            // 
            this.txtLowVal.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLowVal.Location = new System.Drawing.Point(206, 50);
            this.txtLowVal.Name = "txtLowVal";
            this.txtLowVal.Size = new System.Drawing.Size(144, 26);
            this.txtLowVal.TabIndex = 44;
            this.txtLowVal.Text = "100";
            this.txtLowVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtUpVal
            // 
            this.txtUpVal.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUpVal.Location = new System.Drawing.Point(206, 19);
            this.txtUpVal.Name = "txtUpVal";
            this.txtUpVal.Size = new System.Drawing.Size(144, 26);
            this.txtUpVal.TabIndex = 43;
            this.txtUpVal.Text = "1000";
            this.txtUpVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lbDecayStopTime
            // 
            this.lbDecayStopTime.AutoSize = true;
            this.lbDecayStopTime.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDecayStopTime.Location = new System.Drawing.Point(7, 112);
            this.lbDecayStopTime.Name = "lbDecayStopTime";
            this.lbDecayStopTime.Size = new System.Drawing.Size(79, 20);
            this.lbDecayStopTime.TabIndex = 42;
            this.lbDecayStopTime.Text = "Stop Decay";
            // 
            // lbDecayTime
            // 
            this.lbDecayTime.AutoSize = true;
            this.lbDecayTime.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDecayTime.Location = new System.Drawing.Point(7, 83);
            this.lbDecayTime.Name = "lbDecayTime";
            this.lbDecayTime.Size = new System.Drawing.Size(85, 20);
            this.lbDecayTime.TabIndex = 41;
            this.lbDecayTime.Text = "Decay Alarm";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(110, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 20);
            this.label5.TabIndex = 40;
            this.label5.Text = "(10-100)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(124, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 20);
            this.label4.TabIndex = 40;
            this.label4.Text = "(0-10)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(110, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 20);
            this.label3.TabIndex = 40;
            this.label3.Text = "(0-1995)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(103, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 40;
            this.label2.Text = "(10-2000)";
            // 
            // lbUpVal
            // 
            this.lbUpVal.AutoSize = true;
            this.lbUpVal.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUpVal.Location = new System.Drawing.Point(7, 22);
            this.lbUpVal.Name = "lbUpVal";
            this.lbUpVal.Size = new System.Drawing.Size(62, 20);
            this.lbUpVal.TabIndex = 40;
            this.lbUpVal.Text = "Charging";
            // 
            // lbLowVal
            // 
            this.lbLowVal.AutoSize = true;
            this.lbLowVal.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLowVal.Location = new System.Drawing.Point(7, 53);
            this.lbLowVal.Name = "lbLowVal";
            this.lbLowVal.Size = new System.Drawing.Size(62, 20);
            this.lbLowVal.TabIndex = 39;
            this.lbLowVal.Text = "Decay to";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dgvDevice);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 109);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(761, 460);
            this.panel3.TabIndex = 49;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Ordinal_Display";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle14;
            this.dataGridViewTextBoxColumn1.HeaderText = "No";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 50;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "SensorName";
            this.dataGridViewTextBoxColumn2.HeaderText = "Sensor Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Active";
            this.dataGridViewTextBoxColumn3.HeaderText = "Active";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Alarm_Value";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle15;
            this.dataGridViewTextBoxColumn4.HeaderText = "Alarm Value";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "MeasureTypeShow";
            this.dataGridViewTextBoxColumn5.HeaderText = "Measure Type";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // grpDecay
            // 
            this.grpDecay.Controls.Add(this.lbUpVal);
            this.grpDecay.Controls.Add(this.lbLowVal);
            this.grpDecay.Controls.Add(this.label23);
            this.grpDecay.Controls.Add(this.label2);
            this.grpDecay.Controls.Add(this.label22);
            this.grpDecay.Controls.Add(this.label3);
            this.grpDecay.Controls.Add(this.label21);
            this.grpDecay.Controls.Add(this.label4);
            this.grpDecay.Controls.Add(this.label20);
            this.grpDecay.Controls.Add(this.label5);
            this.grpDecay.Controls.Add(this.label11);
            this.grpDecay.Controls.Add(this.lbDecayTime);
            this.grpDecay.Controls.Add(this.label10);
            this.grpDecay.Controls.Add(this.lbDecayStopTime);
            this.grpDecay.Controls.Add(this.txtUpVal);
            this.grpDecay.Controls.Add(this.txtLowVal);
            this.grpDecay.Controls.Add(this.txtDecayTime);
            this.grpDecay.Controls.Add(this.txtDecayStopTime);
            this.grpDecay.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpDecay.Location = new System.Drawing.Point(4, 131);
            this.grpDecay.Name = "grpDecay";
            this.grpDecay.Size = new System.Drawing.Size(404, 144);
            this.grpDecay.TabIndex = 56;
            this.grpDecay.TabStop = false;
            this.grpDecay.Text = "Decay time";
            // 
            // grpIon
            // 
            this.grpIon.Controls.Add(this.label25);
            this.grpIon.Controls.Add(this.lbIonBalance);
            this.grpIon.Controls.Add(this.label24);
            this.grpIon.Controls.Add(this.lbIonStopTime);
            this.grpIon.Controls.Add(this.label9);
            this.grpIon.Controls.Add(this.txtIonCheck);
            this.grpIon.Controls.Add(this.txtIonStopTimeCheck);
            this.grpIon.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpIon.Location = new System.Drawing.Point(4, 281);
            this.grpIon.Name = "grpIon";
            this.grpIon.Size = new System.Drawing.Size(404, 81);
            this.grpIon.TabIndex = 57;
            this.grpIon.TabStop = false;
            this.grpIon.Text = "Ion Balance";
            // 
            // DeviceLayout
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpBlockSetting);
            this.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "DeviceLayout";
            this.Size = new System.Drawing.Size(1173, 569);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDevice)).EndInit();
            this.grpBlockSetting.ResumeLayout(false);
            this.grpBlockSetting.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.pnValueMode.ResumeLayout(false);
            this.pnValueMode.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.grpDecay.ResumeLayout(false);
            this.grpDecay.PerformLayout();
            this.grpIon.ResumeLayout(false);
            this.grpIon.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private BaseCommon.ControlTemplate.dgv dgvDevice;
        private NumericTextBox txtAlaimValue;
        private System.Windows.Forms.Button btResetDefault;
        private System.Windows.Forms.TextBox txtSensorName;
        private System.Windows.Forms.Label lblSensorName;
        private System.Windows.Forms.Label lblSensorSetting;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btSaveSetting;
        private System.Windows.Forms.Label lblAlaimValue;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Label lbl_IpAddress;
        private System.Windows.Forms.TextBox txtBlockName;
        private System.Windows.Forms.Label lblBlockName;
        private System.Windows.Forms.Label lblActiveBlock;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPortBlock;
        private System.Windows.Forms.ComboBox cboOrdinalDisplay;
        private System.Windows.Forms.Label lblOrdinalDisplay;
        private System.Windows.Forms.CheckBox ckbActiveSensor;
        private System.Windows.Forms.Label lblActiveSensor;
        private System.Windows.Forms.Button btnSaveBlock;
        private System.Windows.Forms.Label lblHeaderText;
        private System.Windows.Forms.CheckBox ckbActiveBlock;
        private System.Windows.Forms.Label lbMeasureTitle;
        private System.Windows.Forms.ComboBox cboMeasureType;
        private System.Windows.Forms.Button btnSetAllAlarm;
        private System.Windows.Forms.GroupBox grpBlockSetting;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel pnValueMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private NumericTextBox txtIonStopTimeCheck;
        private NumericTextBox txtIonCheck;
        private System.Windows.Forms.Label lbIonStopTime;
        private System.Windows.Forms.Label lbIonBalance;
        private NumericTextBox txtDecayStopTime;
        private NumericTextBox txtDecayTime;
        private NumericTextBox txtLowVal;
        private NumericTextBox txtUpVal;
        private System.Windows.Forms.Label lbDecayStopTime;
        private System.Windows.Forms.Label lbDecayTime;
        private System.Windows.Forms.Label lbUpVal;
        private System.Windows.Forms.Label lbLowVal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDeviceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlarmValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMeasureTypeShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDecayTimeCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIonValueCheck;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox grpDecay;
        private System.Windows.Forms.GroupBox grpIon;
    }
}
