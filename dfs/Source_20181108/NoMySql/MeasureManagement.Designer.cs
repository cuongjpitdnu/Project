namespace NoMySql
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeasureManagement));
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
            this.device_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbResult = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.btnFilter = new System.Windows.Forms.Button();
            this.cboSubSecondE = new System.Windows.Forms.ComboBox();
            this.cboSubSecondS = new System.Windows.Forms.ComboBox();
            this.cboSubMimuteE = new System.Windows.Forms.ComboBox();
            this.cboSubMimuteS = new System.Windows.Forms.ComboBox();
            this.cboSubHourE = new System.Windows.Forms.ComboBox();
            this.cboSubHourS = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpSubDateE = new System.Windows.Forms.DateTimePicker();
            this.dtpSubDateS = new System.Windows.Forms.DateTimePicker();
            this.chkView = new System.Windows.Forms.CheckBox();
            this.NoDetail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            resources.ApplyResources(this.MainLayout, "MainLayout");
            this.MainLayout.Controls.Add(this.dgvMeasureDetail, 0, 3);
            this.MainLayout.Controls.Add(this.dgvMeasure, 0, 1);
            this.MainLayout.Controls.Add(this.panel1, 0, 0);
            this.MainLayout.Controls.Add(this.pnlFilter, 0, 2);
            this.MainLayout.Name = "MainLayout";
            // 
            // dgvMeasureDetail
            // 
            this.dgvMeasureDetail.AllowUserToAddRows = false;
            this.dgvMeasureDetail.AllowUserToDeleteRows = false;
            this.dgvMeasureDetail.AllowUserToResizeRows = false;
            this.dgvMeasureDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Narrow", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMeasureDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.dgvMeasureDetail, "dgvMeasureDetail");
            this.dgvMeasureDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NoDetail,
            this.colTime,
            this.ValueDetail,
            this.ResultDetail});
            this.dgvMeasureDetail.EnableHeadersVisualStyles = false;
            this.dgvMeasureDetail.Name = "dgvMeasureDetail";
            this.dgvMeasureDetail.ReadOnly = true;
            this.dgvMeasureDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvMeasureDetail.RowHeadersVisible = false;
            this.dgvMeasureDetail.RowTemplate.Height = 24;
            this.dgvMeasureDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
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
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial Narrow", 12F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMeasure.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.dgvMeasure, "dgvMeasure");
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
            this.device_id,
            this.colDelete});
            this.dgvMeasure.EnableHeadersVisualStyles = false;
            this.dgvMeasure.MultiSelect = false;
            this.dgvMeasure.Name = "dgvMeasure";
            this.dgvMeasure.ReadOnly = true;
            this.dgvMeasure.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvMeasure.RowHeadersVisible = false;
            this.dgvMeasure.RowTemplate.Height = 24;
            this.dgvMeasure.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMeasure.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMeasure_CellClick);
            this.dgvMeasure.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvMeasure_CellFormatting);
            // 
            // No
            // 
            this.No.DataPropertyName = "no";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.No.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.No, "No");
            this.No.Name = "No";
            this.No.ReadOnly = true;
            this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MeasureID
            // 
            this.MeasureID.DataPropertyName = "measure_id";
            resources.ApplyResources(this.MeasureID, "MeasureID");
            this.MeasureID.Name = "MeasureID";
            this.MeasureID.ReadOnly = true;
            this.MeasureID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DeviceName
            // 
            this.DeviceName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DeviceName.DataPropertyName = "device_name";
            resources.ApplyResources(this.DeviceName, "DeviceName");
            this.DeviceName.Name = "DeviceName";
            this.DeviceName.ReadOnly = true;
            // 
            // AlarmValue
            // 
            this.AlarmValue.DataPropertyName = "alarm_value";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N0";
            this.AlarmValue.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.AlarmValue, "AlarmValue");
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
            resources.ApplyResources(this.FailLevel, "FailLevel");
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
            resources.ApplyResources(this.colPeriod, "colPeriod");
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
            resources.ApplyResources(this.StartTime, "StartTime");
            this.StartTime.Name = "StartTime";
            this.StartTime.ReadOnly = true;
            this.StartTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // EndTime
            // 
            this.EndTime.DataPropertyName = "end_time";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Format = "yyyy, MMM, dd HH:mm:ss";
            dataGridViewCellStyle12.NullValue = null;
            this.EndTime.DefaultCellStyle = dataGridViewCellStyle12;
            resources.ApplyResources(this.EndTime, "EndTime");
            this.EndTime.Name = "EndTime";
            this.EndTime.ReadOnly = true;
            this.EndTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MeasureType
            // 
            this.MeasureType.DataPropertyName = "measure_type";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.MeasureType.DefaultCellStyle = dataGridViewCellStyle13;
            resources.ApplyResources(this.MeasureType, "MeasureType");
            this.MeasureType.Name = "MeasureType";
            this.MeasureType.ReadOnly = true;
            this.MeasureType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Result
            // 
            this.Result.DataPropertyName = "result";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Result.DefaultCellStyle = dataGridViewCellStyle14;
            resources.ApplyResources(this.Result, "Result");
            this.Result.Name = "Result";
            this.Result.ReadOnly = true;
            this.Result.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // device_id
            // 
            this.device_id.DataPropertyName = "device_id";
            resources.ApplyResources(this.device_id, "device_id");
            this.device_id.Name = "device_id";
            this.device_id.ReadOnly = true;
            // 
            // colDelete
            // 
            resources.ApplyResources(this.colDelete, "colDelete");
            this.colDelete.Name = "colDelete";
            this.colDelete.ReadOnly = true;
            this.colDelete.Text = "Delete";
            this.colDelete.UseColumnTextForButtonValue = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.cmbResult);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cmbType);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dtpEnd);
            this.panel1.Controls.Add(this.dtpStart);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnSearch
            // 
            resources.ApplyResources(this.btnSearch, "btnSearch");
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cmbResult
            // 
            this.cmbResult.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbResult.FormattingEnabled = true;
            resources.ApplyResources(this.cmbResult, "cmbResult");
            this.cmbResult.Name = "cmbResult";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.Name = "cmbType";
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedValueChanged);
            this.cmbType.SelectedValueChanged += new System.EventHandler(this.cmbType_SelectedValueChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // dtpEnd
            // 
            resources.ApplyResources(this.dtpEnd, "dtpEnd");
            this.dtpEnd.Name = "dtpEnd";
            // 
            // dtpStart
            // 
            resources.ApplyResources(this.dtpStart, "dtpStart");
            this.dtpStart.Name = "dtpStart";
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
            this.pnlFilter.Controls.Add(this.label1);
            this.pnlFilter.Controls.Add(this.label6);
            this.pnlFilter.Controls.Add(this.dtpSubDateE);
            this.pnlFilter.Controls.Add(this.dtpSubDateS);
            this.pnlFilter.Controls.Add(this.chkView);
            resources.ApplyResources(this.pnlFilter, "pnlFilter");
            this.pnlFilter.Name = "pnlFilter";
            // 
            // btnFilter
            // 
            resources.ApplyResources(this.btnFilter, "btnFilter");
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // cboSubSecondE
            // 
            this.cboSubSecondE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubSecondE.FormattingEnabled = true;
            resources.ApplyResources(this.cboSubSecondE, "cboSubSecondE");
            this.cboSubSecondE.Name = "cboSubSecondE";
            // 
            // cboSubSecondS
            // 
            this.cboSubSecondS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubSecondS.FormattingEnabled = true;
            resources.ApplyResources(this.cboSubSecondS, "cboSubSecondS");
            this.cboSubSecondS.Name = "cboSubSecondS";
            // 
            // cboSubMimuteE
            // 
            this.cboSubMimuteE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubMimuteE.FormattingEnabled = true;
            resources.ApplyResources(this.cboSubMimuteE, "cboSubMimuteE");
            this.cboSubMimuteE.Name = "cboSubMimuteE";
            // 
            // cboSubMimuteS
            // 
            this.cboSubMimuteS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubMimuteS.FormattingEnabled = true;
            resources.ApplyResources(this.cboSubMimuteS, "cboSubMimuteS");
            this.cboSubMimuteS.Name = "cboSubMimuteS";
            // 
            // cboSubHourE
            // 
            this.cboSubHourE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubHourE.FormattingEnabled = true;
            resources.ApplyResources(this.cboSubHourE, "cboSubHourE");
            this.cboSubHourE.Name = "cboSubHourE";
            // 
            // cboSubHourS
            // 
            this.cboSubHourS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSubHourS.FormattingEnabled = true;
            resources.ApplyResources(this.cboSubHourS, "cboSubHourS");
            this.cboSubHourS.Name = "cboSubHourS";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // dtpSubDateE
            // 
            resources.ApplyResources(this.dtpSubDateE, "dtpSubDateE");
            this.dtpSubDateE.Name = "dtpSubDateE";
            // 
            // dtpSubDateS
            // 
            resources.ApplyResources(this.dtpSubDateS, "dtpSubDateS");
            this.dtpSubDateS.Name = "dtpSubDateS";
            // 
            // chkView
            // 
            resources.ApplyResources(this.chkView, "chkView");
            this.chkView.Name = "chkView";
            this.chkView.UseVisualStyleBackColor = true;
            this.chkView.CheckedChanged += new System.EventHandler(this.chkView_CheckedChanged);
            // 
            // NoDetail
            // 
            this.NoDetail.DataPropertyName = "no";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.NoDetail.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.NoDetail, "NoDetail");
            this.NoDetail.Name = "NoDetail";
            this.NoDetail.ReadOnly = true;
            this.NoDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colTime
            // 
            this.colTime.DataPropertyName = "samples_time";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Format = "yyyy, MMM, dd HH:mm:ss.fff";
            this.colTime.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.colTime, "colTime");
            this.colTime.Name = "colTime";
            this.colTime.ReadOnly = true;
            this.colTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ValueDetail
            // 
            this.ValueDetail.DataPropertyName = "actual_delegate";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            this.ValueDetail.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.ValueDetail, "ValueDetail");
            this.ValueDetail.Name = "ValueDetail";
            this.ValueDetail.ReadOnly = true;
            this.ValueDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ResultDetail
            // 
            this.ResultDetail.DataPropertyName = "result";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ResultDetail.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.ResultDetail, "ResultDetail");
            this.ResultDetail.Name = "ResultDetail";
            this.ResultDetail.ReadOnly = true;
            this.ResultDetail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MeasureManagement
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.Controls.Add(this.MainLayout);
            this.Name = "MeasureManagement";
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
        private System.Windows.Forms.DataGridViewTextBoxColumn device_id;
        private System.Windows.Forms.DataGridViewButtonColumn colDelete;
        private System.Windows.Forms.Panel pnlFilter;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.ComboBox cboSubSecondE;
        private System.Windows.Forms.ComboBox cboSubSecondS;
        private System.Windows.Forms.ComboBox cboSubMimuteE;
        private System.Windows.Forms.ComboBox cboSubMimuteS;
        private System.Windows.Forms.ComboBox cboSubHourE;
        private System.Windows.Forms.ComboBox cboSubHourS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpSubDateE;
        private System.Windows.Forms.DateTimePicker dtpSubDateS;
        private System.Windows.Forms.CheckBox chkView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValueDetail;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResultDetail;
    }
}