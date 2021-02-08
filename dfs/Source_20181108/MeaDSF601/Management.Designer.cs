namespace MeaDSF601
{
    partial class Management
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabMngt = new System.Windows.Forms.TabControl();
            this.tabSetting = new System.Windows.Forms.TabPage();
            this.SettingLayout = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.grvDevice = new System.Windows.Forms.DataGridView();
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
            this.panel4 = new System.Windows.Forms.Panel();
            this.btResetDefault = new System.Windows.Forms.Button();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.txtSamples = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
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
            this.btClear = new System.Windows.Forms.Button();
            this.btSaveSetting = new System.Windows.Forms.Button();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.UserLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btSaveUser = new System.Windows.Forms.Button();
            this.btCreateUser = new System.Windows.Forms.Button();
            this.cboRole = new System.Windows.Forms.ComboBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.grvUser = new System.Windows.Forms.DataGridView();
            this.colUserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeleteUser = new System.Windows.Forms.DataGridViewLinkColumn();
            this.tabChangePassword = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btSaveChange = new System.Windows.Forms.Button();
            this.txtRetypeNew = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtNew = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtCurent = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tabMngt.SuspendLayout();
            this.tabSetting.SuspendLayout();
            this.SettingLayout.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvDevice)).BeginInit();
            this.panel4.SuspendLayout();
            this.tabUsers.SuspendLayout();
            this.UserLayout.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvUser)).BeginInit();
            this.tabChangePassword.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMngt
            // 
            this.tabMngt.Controls.Add(this.tabSetting);
            this.tabMngt.Controls.Add(this.tabUsers);
            this.tabMngt.Controls.Add(this.tabChangePassword);
            this.tabMngt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMngt.Location = new System.Drawing.Point(0, 0);
            this.tabMngt.Margin = new System.Windows.Forms.Padding(4);
            this.tabMngt.Name = "tabMngt";
            this.tabMngt.SelectedIndex = 0;
            this.tabMngt.Size = new System.Drawing.Size(922, 411);
            this.tabMngt.TabIndex = 21;
            this.tabMngt.SelectedIndexChanged += new System.EventHandler(this.tabMngt_SelectedIndexChanged);
            // 
            // tabSetting
            // 
            this.tabSetting.Controls.Add(this.SettingLayout);
            this.tabSetting.Location = new System.Drawing.Point(4, 27);
            this.tabSetting.Margin = new System.Windows.Forms.Padding(4);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.Padding = new System.Windows.Forms.Padding(4);
            this.tabSetting.Size = new System.Drawing.Size(914, 380);
            this.tabSetting.TabIndex = 0;
            this.tabSetting.Text = "Setting";
            this.tabSetting.UseVisualStyleBackColor = true;
            // 
            // SettingLayout
            // 
            this.SettingLayout.ColumnCount = 1;
            this.SettingLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SettingLayout.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.SettingLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingLayout.Location = new System.Drawing.Point(4, 4);
            this.SettingLayout.Name = "SettingLayout";
            this.SettingLayout.RowCount = 1;
            this.SettingLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.SettingLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 372F));
            this.SettingLayout.Size = new System.Drawing.Size(906, 372);
            this.SettingLayout.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 462F));
            this.tableLayoutPanel1.Controls.Add(this.grvDevice, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(900, 366);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // grvDevice
            // 
            this.grvDevice.AllowUserToAddRows = false;
            this.grvDevice.AllowUserToDeleteRows = false;
            this.grvDevice.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grvDevice.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grvDevice.ColumnHeadersHeight = 26;
            this.grvDevice.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDeviceId,
            this.colDevice,
            this.colDeviceName,
            this.colIpAdress,
            this.colPort,
            this.colActive,
            this.colAlarmValue,
            this.colPeriod,
            this.colFailLevel,
            this.colSamples});
            this.grvDevice.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grvDevice.EnableHeadersVisualStyles = false;
            this.grvDevice.Location = new System.Drawing.Point(4, 4);
            this.grvDevice.Margin = new System.Windows.Forms.Padding(4);
            this.grvDevice.MultiSelect = false;
            this.grvDevice.Name = "grvDevice";
            this.grvDevice.RowHeadersVisible = false;
            this.grvDevice.RowTemplate.Height = 25;
            this.grvDevice.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grvDevice.Size = new System.Drawing.Size(430, 358);
            this.grvDevice.TabIndex = 1;
            this.grvDevice.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvDevice_CellClick);
            this.grvDevice.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grvDevice_CellFormatting);
            // 
            // colDeviceId
            // 
            this.colDeviceId.DataPropertyName = "device_id";
            this.colDeviceId.HeaderText = "Device ID";
            this.colDeviceId.Name = "colDeviceId";
            this.colDeviceId.ReadOnly = true;
            this.colDeviceId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDeviceId.Visible = false;
            // 
            // colDevice
            // 
            this.colDevice.DataPropertyName = "device_type";
            this.colDevice.HeaderText = "Device";
            this.colDevice.Name = "colDevice";
            this.colDevice.ReadOnly = true;
            this.colDevice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDevice.Width = 120;
            // 
            // colDeviceName
            // 
            this.colDeviceName.DataPropertyName = "device_name";
            this.colDeviceName.HeaderText = "Device Name";
            this.colDeviceName.Name = "colDeviceName";
            this.colDeviceName.ReadOnly = true;
            this.colDeviceName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDeviceName.Visible = false;
            // 
            // colIpAdress
            // 
            this.colIpAdress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colIpAdress.DataPropertyName = "ip_address";
            this.colIpAdress.HeaderText = "IP Address";
            this.colIpAdress.Name = "colIpAdress";
            this.colIpAdress.ReadOnly = true;
            this.colIpAdress.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colPort
            // 
            this.colPort.DataPropertyName = "port";
            this.colPort.HeaderText = "Port";
            this.colPort.Name = "colPort";
            this.colPort.ReadOnly = true;
            this.colPort.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPort.Width = 110;
            // 
            // colActive
            // 
            this.colActive.DataPropertyName = "active";
            this.colActive.HeaderText = "Active";
            this.colActive.Name = "colActive";
            this.colActive.ReadOnly = true;
            this.colActive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colActive.Visible = false;
            // 
            // colAlarmValue
            // 
            this.colAlarmValue.DataPropertyName = "alarm_value";
            this.colAlarmValue.HeaderText = "Alarm Value";
            this.colAlarmValue.Name = "colAlarmValue";
            this.colAlarmValue.ReadOnly = true;
            this.colAlarmValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAlarmValue.Visible = false;
            // 
            // colPeriod
            // 
            this.colPeriod.DataPropertyName = "period";
            this.colPeriod.HeaderText = "Period";
            this.colPeriod.Name = "colPeriod";
            this.colPeriod.ReadOnly = true;
            this.colPeriod.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colPeriod.Visible = false;
            // 
            // colFailLevel
            // 
            this.colFailLevel.DataPropertyName = "fail_level";
            this.colFailLevel.HeaderText = "Fail Level";
            this.colFailLevel.Name = "colFailLevel";
            this.colFailLevel.ReadOnly = true;
            this.colFailLevel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colFailLevel.Visible = false;
            // 
            // colSamples
            // 
            this.colSamples.DataPropertyName = "samples";
            this.colSamples.HeaderText = "Samples";
            this.colSamples.Name = "colSamples";
            this.colSamples.ReadOnly = true;
            this.colSamples.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colSamples.Visible = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.LightCyan;
            this.panel4.Controls.Add(this.btResetDefault);
            this.panel4.Controls.Add(this.chkActive);
            this.panel4.Controls.Add(this.txtSamples);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.txtPort);
            this.panel4.Controls.Add(this.label18);
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
            this.panel4.Controls.Add(this.btClear);
            this.panel4.Controls.Add(this.btSaveSetting);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(441, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(456, 360);
            this.panel4.TabIndex = 0;
            // 
            // btResetDefault
            // 
            this.btResetDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btResetDefault.Location = new System.Drawing.Point(299, 303);
            this.btResetDefault.Margin = new System.Windows.Forms.Padding(4);
            this.btResetDefault.Name = "btResetDefault";
            this.btResetDefault.Size = new System.Drawing.Size(126, 43);
            this.btResetDefault.TabIndex = 11;
            this.btResetDefault.Text = "Reset Default";
            this.btResetDefault.UseVisualStyleBackColor = true;
            this.btResetDefault.Click += new System.EventHandler(this.btResetDefault_Click);
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(120, 140);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(15, 14);
            this.chkActive.TabIndex = 4;
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // txtSamples
            // 
            this.txtSamples.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtSamples.Location = new System.Drawing.Point(350, 266);
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
            this.label13.Location = new System.Drawing.Point(254, 269);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 18);
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
            // label18
            // 
            this.label18.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(53, 137);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(59, 18);
            this.label18.TabIndex = 24;
            this.label18.Text = "Active :";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 108);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 18);
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
            this.label9.Location = new System.Drawing.Point(1, 40);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(111, 18);
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
            this.label1.Location = new System.Drawing.Point(20, 74);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 18);
            this.label1.TabIndex = 25;
            this.label1.Text = "IP Address :";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.BackColor = System.Drawing.Color.Khaki;
            this.label3.Location = new System.Drawing.Point(232, 164);
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
            this.label17.Location = new System.Drawing.Point(2, 164);
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
            this.txtFailLevel.Location = new System.Drawing.Point(350, 232);
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
            this.txtPeriod.Location = new System.Drawing.Point(350, 198);
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
            this.txtAlarmValue.Location = new System.Drawing.Point(120, 198);
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
            this.label14.Location = new System.Drawing.Point(247, 201);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 18);
            this.label14.TabIndex = 19;
            this.label14.Text = "Period (s) :";
            // 
            // label16
            // 
            this.label16.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(249, 235);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(83, 18);
            this.label16.TabIndex = 18;
            this.label16.Text = "Fail Level :";
            // 
            // label15
            // 
            this.label15.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(3, 201);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 18);
            this.label15.TabIndex = 16;
            this.label15.Text = "Alarm Value :";
            // 
            // btClear
            // 
            this.btClear.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btClear.Location = new System.Drawing.Point(165, 303);
            this.btClear.Margin = new System.Windows.Forms.Padding(4);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(126, 43);
            this.btClear.TabIndex = 10;
            this.btClear.Text = "Clear";
            this.btClear.UseVisualStyleBackColor = true;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // btSaveSetting
            // 
            this.btSaveSetting.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btSaveSetting.Location = new System.Drawing.Point(31, 303);
            this.btSaveSetting.Margin = new System.Windows.Forms.Padding(4);
            this.btSaveSetting.Name = "btSaveSetting";
            this.btSaveSetting.Size = new System.Drawing.Size(126, 43);
            this.btSaveSetting.TabIndex = 9;
            this.btSaveSetting.Text = "Save ";
            this.btSaveSetting.UseVisualStyleBackColor = true;
            this.btSaveSetting.Click += new System.EventHandler(this.btSaveSetting_Click);
            // 
            // tabUsers
            // 
            this.tabUsers.Controls.Add(this.UserLayout);
            this.tabUsers.Location = new System.Drawing.Point(4, 27);
            this.tabUsers.Margin = new System.Windows.Forms.Padding(4);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Padding = new System.Windows.Forms.Padding(4);
            this.tabUsers.Size = new System.Drawing.Size(914, 380);
            this.tabUsers.TabIndex = 1;
            this.tabUsers.Text = "Users";
            this.tabUsers.UseVisualStyleBackColor = true;
            // 
            // UserLayout
            // 
            this.UserLayout.ColumnCount = 2;
            this.UserLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.UserLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 412F));
            this.UserLayout.Controls.Add(this.panel2, 0, 0);
            this.UserLayout.Controls.Add(this.grvUser, 0, 0);
            this.UserLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserLayout.Location = new System.Drawing.Point(4, 4);
            this.UserLayout.Margin = new System.Windows.Forms.Padding(4);
            this.UserLayout.Name = "UserLayout";
            this.UserLayout.RowCount = 1;
            this.UserLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.25974F));
            this.UserLayout.Size = new System.Drawing.Size(906, 372);
            this.UserLayout.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightCyan;
            this.panel2.Controls.Add(this.btSaveUser);
            this.panel2.Controls.Add(this.btCreateUser);
            this.panel2.Controls.Add(this.cboRole);
            this.panel2.Controls.Add(this.txtEmail);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.txtFullName);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.txtPassword);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtUserName);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(498, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(404, 364);
            this.panel2.TabIndex = 2;
            // 
            // btSaveUser
            // 
            this.btSaveUser.Location = new System.Drawing.Point(207, 246);
            this.btSaveUser.Margin = new System.Windows.Forms.Padding(4);
            this.btSaveUser.Name = "btSaveUser";
            this.btSaveUser.Size = new System.Drawing.Size(126, 39);
            this.btSaveUser.TabIndex = 19;
            this.btSaveUser.Text = "Save User";
            this.btSaveUser.UseVisualStyleBackColor = true;
            this.btSaveUser.Click += new System.EventHandler(this.btSaveUser_Click);
            // 
            // btCreateUser
            // 
            this.btCreateUser.Location = new System.Drawing.Point(71, 246);
            this.btCreateUser.Margin = new System.Windows.Forms.Padding(4);
            this.btCreateUser.Name = "btCreateUser";
            this.btCreateUser.Size = new System.Drawing.Size(126, 39);
            this.btCreateUser.TabIndex = 20;
            this.btCreateUser.Text = "Create User";
            this.btCreateUser.UseVisualStyleBackColor = true;
            this.btCreateUser.Click += new System.EventHandler(this.btCreateUser_Click);
            // 
            // cboRole
            // 
            this.cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole.FormattingEnabled = true;
            this.cboRole.Location = new System.Drawing.Point(121, 186);
            this.cboRole.Margin = new System.Windows.Forms.Padding(4);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(265, 26);
            this.cboRole.TabIndex = 18;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(121, 148);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(265, 26);
            this.txtEmail.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(66, 189);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 18);
            this.label8.TabIndex = 0;
            this.label8.Text = "Role :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(57, 151);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 18);
            this.label7.TabIndex = 0;
            this.label7.Text = "Email :";
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(121, 109);
            this.txtFullName.Margin = new System.Windows.Forms.Padding(4);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(265, 26);
            this.txtFullName.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 112);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "Full Name :";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(121, 70);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(265, 26);
            this.txtPassword.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 73);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "Password :";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(121, 31);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(265, 26);
            this.txtUserName.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 34);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "Username :";
            // 
            // grvUser
            // 
            this.grvUser.AllowUserToAddRows = false;
            this.grvUser.AllowUserToDeleteRows = false;
            this.grvUser.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grvUser.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grvUser.ColumnHeadersHeight = 26;
            this.grvUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUserId,
            this.colUserName,
            this.colFullName,
            this.colEmail,
            this.colRole,
            this.colDeleteUser});
            this.grvUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grvUser.EnableHeadersVisualStyles = false;
            this.grvUser.Location = new System.Drawing.Point(4, 4);
            this.grvUser.Margin = new System.Windows.Forms.Padding(4);
            this.grvUser.MultiSelect = false;
            this.grvUser.Name = "grvUser";
            this.grvUser.RowHeadersVisible = false;
            this.grvUser.RowTemplate.Height = 25;
            this.grvUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grvUser.Size = new System.Drawing.Size(486, 364);
            this.grvUser.TabIndex = 13;
            this.grvUser.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvUser_CellClick);
            this.grvUser.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grvUser_CellFormatting);
            // 
            // colUserId
            // 
            this.colUserId.DataPropertyName = "userid";
            this.colUserId.HeaderText = "User Id";
            this.colUserId.Name = "colUserId";
            this.colUserId.ReadOnly = true;
            this.colUserId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colUserId.Visible = false;
            // 
            // colUserName
            // 
            this.colUserName.DataPropertyName = "username";
            this.colUserName.HeaderText = "Username";
            this.colUserName.Name = "colUserName";
            this.colUserName.ReadOnly = true;
            this.colUserName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colUserName.Width = 120;
            // 
            // colFullName
            // 
            this.colFullName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFullName.DataPropertyName = "fullname";
            this.colFullName.HeaderText = "Full Name";
            this.colFullName.Name = "colFullName";
            this.colFullName.ReadOnly = true;
            this.colFullName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colEmail
            // 
            this.colEmail.DataPropertyName = "email";
            this.colEmail.HeaderText = "Email";
            this.colEmail.Name = "colEmail";
            this.colEmail.ReadOnly = true;
            this.colEmail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEmail.Visible = false;
            this.colEmail.Width = 180;
            // 
            // colRole
            // 
            this.colRole.DataPropertyName = "role";
            this.colRole.HeaderText = "Role";
            this.colRole.Name = "colRole";
            this.colRole.ReadOnly = true;
            this.colRole.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRole.Width = 75;
            // 
            // colDeleteUser
            // 
            this.colDeleteUser.HeaderText = "";
            this.colDeleteUser.Name = "colDeleteUser";
            this.colDeleteUser.ReadOnly = true;
            this.colDeleteUser.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDeleteUser.Text = "Delete";
            this.colDeleteUser.UseColumnTextForLinkValue = true;
            this.colDeleteUser.Width = 65;
            // 
            // tabChangePassword
            // 
            this.tabChangePassword.Controls.Add(this.panel3);
            this.tabChangePassword.Location = new System.Drawing.Point(4, 27);
            this.tabChangePassword.Margin = new System.Windows.Forms.Padding(4);
            this.tabChangePassword.Name = "tabChangePassword";
            this.tabChangePassword.Size = new System.Drawing.Size(914, 380);
            this.tabChangePassword.TabIndex = 2;
            this.tabChangePassword.Text = "Change Password";
            this.tabChangePassword.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.LightCyan;
            this.panel3.Controls.Add(this.btSaveChange);
            this.panel3.Controls.Add(this.txtRetypeNew);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.txtNew);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.txtCurent);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(914, 380);
            this.panel3.TabIndex = 0;
            // 
            // btSaveChange
            // 
            this.btSaveChange.Location = new System.Drawing.Point(387, 201);
            this.btSaveChange.Margin = new System.Windows.Forms.Padding(4);
            this.btSaveChange.Name = "btSaveChange";
            this.btSaveChange.Size = new System.Drawing.Size(126, 39);
            this.btSaveChange.TabIndex = 25;
            this.btSaveChange.Text = "Save Change";
            this.btSaveChange.UseVisualStyleBackColor = true;
            this.btSaveChange.Click += new System.EventHandler(this.btSaveChange_Click);
            // 
            // txtRetypeNew
            // 
            this.txtRetypeNew.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRetypeNew.Location = new System.Drawing.Point(387, 167);
            this.txtRetypeNew.Margin = new System.Windows.Forms.Padding(4);
            this.txtRetypeNew.Name = "txtRetypeNew";
            this.txtRetypeNew.PasswordChar = '*';
            this.txtRetypeNew.Size = new System.Drawing.Size(291, 26);
            this.txtRetypeNew.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(237, 170);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(142, 18);
            this.label12.TabIndex = 2;
            this.label12.Text = "Re-type password :";
            // 
            // txtNew
            // 
            this.txtNew.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNew.Location = new System.Drawing.Point(387, 133);
            this.txtNew.Margin = new System.Windows.Forms.Padding(4);
            this.txtNew.Name = "txtNew";
            this.txtNew.PasswordChar = '*';
            this.txtNew.Size = new System.Drawing.Size(291, 26);
            this.txtNew.TabIndex = 23;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(256, 136);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(123, 18);
            this.label11.TabIndex = 2;
            this.label11.Text = "New  password :";
            // 
            // txtCurent
            // 
            this.txtCurent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurent.Location = new System.Drawing.Point(387, 99);
            this.txtCurent.Margin = new System.Windows.Forms.Padding(4);
            this.txtCurent.Name = "txtCurent";
            this.txtCurent.PasswordChar = '*';
            this.txtCurent.Size = new System.Drawing.Size(291, 26);
            this.txtCurent.TabIndex = 22;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(240, 102);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(139, 18);
            this.label10.TabIndex = 2;
            this.label10.Text = "Current password :";
            // 
            // Management
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(922, 411);
            this.Controls.Add(this.tabMngt);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(938, 450);
            this.Name = "Management";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Setting Management";
            this.tabMngt.ResumeLayout(false);
            this.tabSetting.ResumeLayout(false);
            this.SettingLayout.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grvDevice)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabUsers.ResumeLayout(false);
            this.UserLayout.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvUser)).EndInit();
            this.tabChangePassword.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMngt;
        private System.Windows.Forms.TabPage tabSetting;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.TabPage tabChangePassword;
        private System.Windows.Forms.TableLayoutPanel UserLayout;
        private System.Windows.Forms.DataGridView grvUser;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboRole;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btSaveUser;
        private System.Windows.Forms.Button btCreateUser;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtRetypeNew;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtNew;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtCurent;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btSaveChange;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRole;
        private System.Windows.Forms.DataGridViewLinkColumn colDeleteUser;
        private System.Windows.Forms.TableLayoutPanel SettingLayout;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtIpAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtFailLevel;
        private System.Windows.Forms.TextBox txtPeriod;
        private System.Windows.Forms.TextBox txtAlarmValue;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.Button btSaveSetting;
        private System.Windows.Forms.DataGridView grvDevice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblDevice;
        private System.Windows.Forms.TextBox txtSamples;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtDeviceName;
        private System.Windows.Forms.Label label9;
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
        private System.Windows.Forms.Button btResetDefault;
    }
}