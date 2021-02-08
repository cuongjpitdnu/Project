namespace WithMySql
{
    partial class MeasureManagement
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.dgvMeasureDetail = new System.Windows.Forms.DataGridView();
            this.dgvMeasure = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeasureID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeviceName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AlarmValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FailLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MeasureType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.device_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEnable = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.cmbResult = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbUser = new System.Windows.Forms.ComboBox();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.btnFilter = new System.Windows.Forms.Button();
            this.cboSubSecondE = new System.Windows.Forms.ComboBox();
            this.cboSubSecondS = new System.Windows.Forms.ComboBox();
            this.cboSubMimuteE = new System.Windows.Forms.ComboBox();
            this.cboSubMimuteS = new System.Windows.Forms.ComboBox();
            this.cboSubHourE = new System.Windows.Forms.ComboBox();
            this.cboSubHourS = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpSubDateE = new System.Windows.Forms.DateTimePicker();
            this.dtpSubDateS = new System.Windows.Forms.DateTimePicker();
            this.chkView = new System.Windows.Forms.CheckBox();
            this.NoDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValueDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResultDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasureDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasure)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 1;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.Controls.Add(this.dgvMeasureDetail, 0, 3);
            this.MainLayout.Controls.Add(this.dgvMeasure, 0, 1);
            this.MainLayout.Controls.Add(this.panel1, 0, 0);
            this.MainLayout.Controls.Add(this.pnlFilter, 0, 2);
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 0);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 4;
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 101F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 213F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainLayout.Size = new System.Drawing.Size(1156, 582);
            this.MainLayout.TabIndex = 145;
            // 
            // dgvMeasureDetail
            // 
            this.dgvMeasureDetail.AllowUserToAddRows = false;
            this.dgvMeasureDetail.AllowUserToDeleteRows = false;
            this.dgvMeasureDetail.AllowUserToResizeRows = false;
            this.dgvMeasureDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMeasureDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMeasureDetail.ColumnHeadersHeight = 26;
            this.dgvMeasureDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NoDetail,
            this.TimeDetail,
            this.ValueDetail,
            this.ResultDetail});
            this.dgvMeasureDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMeasureDetail.EnableHeadersVisualStyles = false;
            this.dgvMeasureDetail.Location = new System.Drawing.Point(3, 395);
            this.dgvMeasureDetail.Name = "dgvMeasureDetail";
            this.dgvMeasureDetail.ReadOnly = true;
            this.dgvMeasureDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvMeasureDetail.RowHeadersVisible = false;
            this.dgvMeasureDetail.RowTemplate.Height = 24;
            this.dgvMeasureDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMeasureDetail.Size = new System.Drawing.Size(1150, 184);
            this.dgvMeasureDetail.TabIndex = 145;
            this.dgvMeasureDetail.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvMeasureDetail_CellFormatting);
            // 
            // dgvMeasure
            // 
            this.dgvMeasure.AllowUserToAddRows = false;
            this.dgvMeasure.AllowUserToDeleteRows = false;
            this.dgvMeasure.AllowUserToResizeRows = false;
            this.dgvMeasure.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMeasure.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvMeasure.ColumnHeadersHeight = 26;
            this.dgvMeasure.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.MeasureID,
            this.DeviceName,
            this.AlarmValue,
            this.FailLevel,
            this.colPeriod,
            this.StartTime,
            this.EndTime,
            this.MeasureType,
            this.Result,
            this.User,
            this.device_id,
            this.colEnable,
            this.colDelete});
            this.dgvMeasure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMeasure.EnableHeadersVisualStyles = false;
            this.dgvMeasure.Location = new System.Drawing.Point(3, 104);
            this.dgvMeasure.Name = "dgvMeasure";
            this.dgvMeasure.ReadOnly = true;
            this.dgvMeasure.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvMeasure.RowHeadersVisible = false;
            this.dgvMeasure.RowTemplate.Height = 24;
            this.dgvMeasure.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMeasure.Size = new System.Drawing.Size(1150, 207);
            this.dgvMeasure.TabIndex = 140;
            this.dgvMeasure.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMeasure_CellClick);
            this.dgvMeasure.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvMeasure_CellFormatting);
            // 
            // No
            // 
            this.No.DataPropertyName = "no";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.No.DefaultCellStyle = dataGridViewCellStyle7;
            this.No.HeaderText = "No";
            this.No.Name = "No";
            this.No.ReadOnly = true;
            this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.No.Width = 50;
            // 
            // MeasureID
            // 
            this.MeasureID.DataPropertyName = "measure_id";
            this.MeasureID.HeaderText = "MeasureID";
            this.MeasureID.Name = "MeasureID";
            this.MeasureID.ReadOnly = true;
            this.MeasureID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MeasureID.Visible = false;
            // 
            // DeviceName
            // 
            this.DeviceName.DataPropertyName = "device_name";
            this.DeviceName.HeaderText = "Device Name";
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.ReadOnly = true;
            // 
            // AlarmValue
            // 
            this.AlarmValue.DataPropertyName = "alarm_value";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N0";
            this.AlarmValue.DefaultCellStyle = dataGridViewCellStyle8;
            this.AlarmValue.HeaderText = "AlarmValue";
            this.AlarmValue.Name = "AlarmValue";
            this.AlarmValue.ReadOnly = true;
            this.AlarmValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // FailLevel
            // 
            this.FailLevel.DataPropertyName = "fail_level";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N0";
            this.FailLevel.DefaultCellStyle = dataGridViewCellStyle9;
            this.FailLevel.HeaderText = "FailLevel";
            this.FailLevel.Name = "FailLevel";
            this.FailLevel.ReadOnly = true;
            this.FailLevel.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colPeriod
            // 
            this.colPeriod.DataPropertyName = "period";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N0";
            this.colPeriod.DefaultCellStyle = dataGridViewCellStyle10;
            this.colPeriod.HeaderText = "Period (s)";
            this.colPeriod.Name = "colPeriod";
            this.colPeriod.ReadOnly = true;
            this.colPeriod.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // StartTime
            // 
            this.StartTime.DataPropertyName = "start_time";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.Format = "yyyy, MMM, dd HH:mm:ss";
            this.StartTime.DefaultCellStyle = dataGridViewCellStyle11;
            this.StartTime.HeaderText = "Start Time";
            this.StartTime.Name = "StartTime";
            this.StartTime.ReadOnly = true;
            this.StartTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StartTime.Width = 150;
            // 
            // EndTime
            // 
            this.EndTime.DataPropertyName = "end_time";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Format = "yyyy, MMM, dd HH:mm:ss";
            dataGridViewCellStyle12.NullValue = null;
            this.EndTime.DefaultCellStyle = dataGridViewCellStyle12;
            this.EndTime.HeaderText = "EndTime";
            this.EndTime.Name = "EndTime";
            this.EndTime.ReadOnly = true;
            this.EndTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.EndTime.Width = 150;
            // 
            // MeasureType
            // 
            this.MeasureType.DataPropertyName = "measure_type";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MeasureType.DefaultCellStyle = dataGridViewCellStyle13;
            this.MeasureType.HeaderText = "Measure Type";
            this.MeasureType.Name = "MeasureType";
            this.MeasureType.ReadOnly = true;
            this.MeasureType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.MeasureType.Width = 120;
            // 
            // Result
            // 
            this.Result.DataPropertyName = "result";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Result.DefaultCellStyle = dataGridViewCellStyle14;
            this.Result.HeaderText = "Result";
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            this.Result.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // User
            // 
            this.User.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.User.DataPropertyName = "fullname";
            this.User.HeaderText = "User";
            this.User.Name = "User";
            this.User.ReadOnly = true;
            this.User.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // device_id
            // 
            this.device_id.DataPropertyName = "device_id";
            this.device_id.HeaderText = "device_id";
            this.device_id.Name = "device_id";
            this.device_id.ReadOnly = true;
            this.device_id.Visible = false;
            // 
            // colEnable
            // 
            this.colEnable.HeaderText = "";
            this.colEnable.MinimumWidth = 60;
            this.colEnable.Name = "colEnable";
            this.colEnable.ReadOnly = true;
            this.colEnable.Text = "Enable";
            this.colEnable.UseColumnTextForButtonValue = true;
            this.colEnable.Width = 60;
            // 
            // colDelete
            // 
            this.colDelete.HeaderText = "";
            this.colDelete.MinimumWidth = 60;
            this.colDelete.Name = "colDelete";
            this.colDelete.ReadOnly = true;
            this.colDelete.Text = "Delete";
            this.colDelete.UseColumnTextForButtonValue = true;
            this.colDelete.Width = 60;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cmbStatus);
            this.panel1.Controls.Add(this.cmbResult);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cmbType);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtpEnd);
            this.panel1.Controls.Add(this.dtpStart);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbUser);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1150, 95);
            this.panel1.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(650, 8);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(103, 37);
            this.btnExport.TabIndex = 155;
            this.btnExport.Text = " Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(537, 8);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(103, 37);
            this.btnSearch.TabIndex = 154;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(727, 62);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(45, 20);
            this.lblStatus.TabIndex = 153;
            this.lblStatus.Text = "Status";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(487, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 20);
            this.label5.TabIndex = 153;
            this.label5.Text = "Result";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(779, 58);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(160, 28);
            this.cmbStatus.TabIndex = 152;
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_SelectedIndexChanged);
            // 
            // cmbResult
            // 
            this.cmbResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResult.FormattingEnabled = true;
            this.cmbResult.Location = new System.Drawing.Point(537, 58);
            this.cmbResult.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbResult.Name = "cmbResult";
            this.cmbResult.Size = new System.Drawing.Size(160, 28);
            this.cmbResult.TabIndex = 152;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(252, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 20);
            this.label4.TabIndex = 151;
            this.label4.Text = "Type";
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(300, 58);
            this.cmbType.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(160, 28);
            this.cmbType.TabIndex = 150;
            this.cmbType.SelectedValueChanged += new System.EventHandler(this.cmbType_SelectedValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 20);
            this.label3.TabIndex = 149;
            this.label3.Text = "Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(276, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 20);
            this.label2.TabIndex = 148;
            this.label2.Text = "~";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Location = new System.Drawing.Point(300, 19);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(200, 26);
            this.dtpEnd.TabIndex = 147;
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(66, 19);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(200, 26);
            this.dtpStart.TabIndex = 146;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 20);
            this.label1.TabIndex = 145;
            this.label1.Text = "Users";
            // 
            // cmbUser
            // 
            this.cmbUser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUser.FormattingEnabled = true;
            this.cmbUser.Location = new System.Drawing.Point(66, 58);
            this.cmbUser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbUser.Name = "cmbUser";
            this.cmbUser.Size = new System.Drawing.Size(160, 28);
            this.cmbUser.TabIndex = 144;
            // 
            // pnlFilter
            // 
            this.pnlFilter.Controls.Add(this.btnFilter);
            this.pnlFilter.Controls.Add(this.cboSubSecondE);
            this.pnlFilter.Controls.Add(this.cboSubSecondS);
            this.pnlFilter.Controls.Add(this.cboSubMimuteE);
            this.pnlFilter.Controls.Add(this.cboSubMimuteS);
            this.pnlFilter.Controls.Add(this.cboSubHourE);
            this.pnlFilter.Controls.Add(this.cboSubHourS);
            this.pnlFilter.Controls.Add(this.label6);
            this.pnlFilter.Controls.Add(this.label7);
            this.pnlFilter.Controls.Add(this.dtpSubDateE);
            this.pnlFilter.Controls.Add(this.dtpSubDateS);
            this.pnlFilter.Controls.Add(this.chkView);
            this.pnlFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFilter.Location = new System.Drawing.Point(3, 317);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(1150, 72);
            this.pnlFilter.TabIndex = 146;
            // 
            // btnFilter
            // 
            this.btnFilter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFilter.Location = new System.Drawing.Point(1027, 18);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(103, 37);
            this.btnFilter.TabIndex = 175;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // cboSubSecondE
            // 
            this.cboSubSecondE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubSecondE.FormattingEnabled = true;
            this.cboSubSecondE.Location = new System.Drawing.Point(937, 6);
            this.cboSubSecondE.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboSubSecondE.Name = "cboSubSecondE";
            this.cboSubSecondE.Size = new System.Drawing.Size(61, 28);
            this.cboSubSecondE.TabIndex = 169;
            // 
            // cboSubSecondS
            // 
            this.cboSubSecondS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubSecondS.FormattingEnabled = true;
            this.cboSubSecondS.Location = new System.Drawing.Point(455, 6);
            this.cboSubSecondS.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboSubSecondS.Name = "cboSubSecondS";
            this.cboSubSecondS.Size = new System.Drawing.Size(61, 28);
            this.cboSubSecondS.TabIndex = 170;
            // 
            // cboSubMimuteE
            // 
            this.cboSubMimuteE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubMimuteE.FormattingEnabled = true;
            this.cboSubMimuteE.Location = new System.Drawing.Point(868, 6);
            this.cboSubMimuteE.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboSubMimuteE.Name = "cboSubMimuteE";
            this.cboSubMimuteE.Size = new System.Drawing.Size(61, 28);
            this.cboSubMimuteE.TabIndex = 171;
            // 
            // cboSubMimuteS
            // 
            this.cboSubMimuteS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubMimuteS.FormattingEnabled = true;
            this.cboSubMimuteS.Location = new System.Drawing.Point(386, 6);
            this.cboSubMimuteS.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboSubMimuteS.Name = "cboSubMimuteS";
            this.cboSubMimuteS.Size = new System.Drawing.Size(61, 28);
            this.cboSubMimuteS.TabIndex = 172;
            // 
            // cboSubHourE
            // 
            this.cboSubHourE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubHourE.FormattingEnabled = true;
            this.cboSubHourE.Location = new System.Drawing.Point(799, 6);
            this.cboSubHourE.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboSubHourE.Name = "cboSubHourE";
            this.cboSubHourE.Size = new System.Drawing.Size(61, 28);
            this.cboSubHourE.TabIndex = 173;
            // 
            // cboSubHourS
            // 
            this.cboSubHourS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubHourS.FormattingEnabled = true;
            this.cboSubHourS.Location = new System.Drawing.Point(317, 6);
            this.cboSubHourS.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboSubHourS.Name = "cboSubHourS";
            this.cboSubHourS.Size = new System.Drawing.Size(61, 28);
            this.cboSubHourS.TabIndex = 174;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(20, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 20);
            this.label6.TabIndex = 168;
            this.label6.Text = "Time";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(523, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 20);
            this.label7.TabIndex = 167;
            this.label7.Text = "~";
            // 
            // dtpSubDateE
            // 
            this.dtpSubDateE.Location = new System.Drawing.Point(546, 6);
            this.dtpSubDateE.Name = "dtpSubDateE";
            this.dtpSubDateE.Size = new System.Drawing.Size(246, 26);
            this.dtpSubDateE.TabIndex = 165;
            // 
            // dtpSubDateS
            // 
            this.dtpSubDateS.Location = new System.Drawing.Point(64, 6);
            this.dtpSubDateS.Name = "dtpSubDateS";
            this.dtpSubDateS.Size = new System.Drawing.Size(246, 26);
            this.dtpSubDateS.TabIndex = 166;
            // 
            // chkView
            // 
            this.chkView.AutoSize = true;
            this.chkView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkView.Location = new System.Drawing.Point(64, 42);
            this.chkView.Name = "chkView";
            this.chkView.Size = new System.Drawing.Size(298, 24);
            this.chkView.TabIndex = 164;
            this.chkView.Text = "Only show the voltage exceeds the threshold";
            this.chkView.UseVisualStyleBackColor = true;
            this.chkView.CheckedChanged += new System.EventHandler(this.chkView_CheckedChanged);
            // 
            // NoDetail
            // 
            this.NoDetail.DataPropertyName = "no";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.NoDetail.DefaultCellStyle = dataGridViewCellStyle2;
            this.NoDetail.HeaderText = "No";
            this.NoDetail.Name = "NoDetail";
            this.NoDetail.ReadOnly = true;
            this.NoDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.NoDetail.Width = 50;
            // 
            // TimeDetail
            // 
            this.TimeDetail.DataPropertyName = "samples_time";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "yyyy, MMM, dd HH:mm:ss.fff";
            this.TimeDetail.DefaultCellStyle = dataGridViewCellStyle3;
            this.TimeDetail.HeaderText = "Time";
            this.TimeDetail.Name = "TimeDetail";
            this.TimeDetail.ReadOnly = true;
            this.TimeDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TimeDetail.Width = 200;
            // 
            // ValueDetail
            // 
            this.ValueDetail.DataPropertyName = "actual_delegate";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            this.ValueDetail.DefaultCellStyle = dataGridViewCellStyle4;
            this.ValueDetail.HeaderText = "Value (V)";
            this.ValueDetail.Name = "ValueDetail";
            this.ValueDetail.ReadOnly = true;
            this.ValueDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ValueDetail.Width = 130;
            // 
            // ResultDetail
            // 
            this.ResultDetail.DataPropertyName = "result";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ResultDetail.DefaultCellStyle = dataGridViewCellStyle5;
            this.ResultDetail.HeaderText = "Result";
            this.ResultDetail.Name = "ResultDetail";
            this.ResultDetail.ReadOnly = true;
            this.ResultDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MeasureManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(1156, 582);
            this.Controls.Add(this.MainLayout);
            this.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MeasureManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Measurment Management";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MeasureManagement_FormClosed);
            this.MainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasureDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasure)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlFilter.ResumeLayout(false);
            this.pnlFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainLayout;
        private System.Windows.Forms.DataGridView dgvMeasureDetail;
        private System.Windows.Forms.DataGridView dgvMeasure;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbResult;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbUser;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.ComboBox cboSubSecondE;
        private System.Windows.Forms.ComboBox cboSubSecondS;
        private System.Windows.Forms.ComboBox cboSubMimuteE;
        private System.Windows.Forms.ComboBox cboSubMimuteS;
        private System.Windows.Forms.ComboBox cboSubHourE;
        private System.Windows.Forms.ComboBox cboSubHourS;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpSubDateE;
        private System.Windows.Forms.DateTimePicker dtpSubDateS;
        private System.Windows.Forms.CheckBox chkView;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasureID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviceName;
        private System.Windows.Forms.DataGridViewTextBoxColumn AlarmValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn FailLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPeriod;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn MeasureType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Result;
        private System.Windows.Forms.DataGridViewTextBoxColumn User;
        private System.Windows.Forms.DataGridViewTextBoxColumn device_id;
        private System.Windows.Forms.DataGridViewButtonColumn colEnable;
        private System.Windows.Forms.DataGridViewButtonColumn colDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValueDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResultDetail;
    }
}