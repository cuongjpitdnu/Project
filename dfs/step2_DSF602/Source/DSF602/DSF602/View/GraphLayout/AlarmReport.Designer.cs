namespace DSF602.View.GraphLayout
{
    partial class AlarmReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvMeasure = new BaseCommon.ControlTemplate.dgv();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMesId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSensorId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBlockName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSensorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAlarmValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMesType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvMeasureDetail = new BaseCommon.ControlTemplate.dgv();
            this.colDtlNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDtlTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDtlActualValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblSensor = new System.Windows.Forms.Label();
            this.lblBlock = new System.Windows.Forms.Label();
            this.cmbSensor = new System.Windows.Forms.ComboBox();
            this.cmbBlock = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasureDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMeasure
            // 
            this.dgvMeasure.AllowUserToAddRows = false;
            this.dgvMeasure.AllowUserToDeleteRows = false;
            this.dgvMeasure.AllowUserToResizeRows = false;
            this.dgvMeasure.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMeasure.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Narrow", 12F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMeasure.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMeasure.ColumnHeadersHeight = 30;
            this.dgvMeasure.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo,
            this.colMesId,
            this.colSensorId,
            this.colBlockName,
            this.colSensorName,
            this.colAlarmValue,
            this.colStartTime,
            this.colEndTime,
            this.colMesType,
            this.colResult});
            this.dgvMeasure.EnableHeadersVisualStyles = false;
            this.dgvMeasure.Location = new System.Drawing.Point(8, 63);
            this.dgvMeasure.MultiSelect = false;
            this.dgvMeasure.Name = "dgvMeasure";
            this.dgvMeasure.ReadOnly = true;
            this.dgvMeasure.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvMeasure.RowHeadersVisible = false;
            this.dgvMeasure.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMeasure.Size = new System.Drawing.Size(858, 214);
            this.dgvMeasure.TabIndex = 141;
            this.dgvMeasure.SelectionChanged += new System.EventHandler(this.dgvMeasure_SelectionChanged);
            // 
            // colNo
            // 
            this.colNo.DataPropertyName = "No";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colNo.DefaultCellStyle = dataGridViewCellStyle2;
            this.colNo.HeaderText = "No";
            this.colNo.Name = "colNo";
            this.colNo.ReadOnly = true;
            this.colNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colNo.Width = 50;
            // 
            // colMesId
            // 
            this.colMesId.DataPropertyName = "MeasureId";
            this.colMesId.HeaderText = "MeasureId";
            this.colMesId.Name = "colMesId";
            this.colMesId.ReadOnly = true;
            this.colMesId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMesId.Visible = false;
            // 
            // colSensorId
            // 
            this.colSensorId.DataPropertyName = "SensorId";
            this.colSensorId.HeaderText = "Sensor Id";
            this.colSensorId.Name = "colSensorId";
            this.colSensorId.ReadOnly = true;
            this.colSensorId.Visible = false;
            // 
            // colBlockName
            // 
            this.colBlockName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colBlockName.DataPropertyName = "BlockName";
            this.colBlockName.HeaderText = "Block Name";
            this.colBlockName.Name = "colBlockName";
            this.colBlockName.ReadOnly = true;
            this.colBlockName.Width = 106;
            // 
            // colSensorName
            // 
            this.colSensorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colSensorName.DataPropertyName = "SensorName";
            this.colSensorName.HeaderText = "Sensor Name";
            this.colSensorName.Name = "colSensorName";
            this.colSensorName.ReadOnly = true;
            this.colSensorName.Width = 115;
            // 
            // colAlarmValue
            // 
            this.colAlarmValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colAlarmValue.DataPropertyName = "Alarm_Value";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            this.colAlarmValue.DefaultCellStyle = dataGridViewCellStyle3;
            this.colAlarmValue.HeaderText = "Alarm Value";
            this.colAlarmValue.Name = "colAlarmValue";
            this.colAlarmValue.ReadOnly = true;
            this.colAlarmValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colAlarmValue.Width = 86;
            // 
            // colStartTime
            // 
            this.colStartTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colStartTime.DataPropertyName = "start_time";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Format = "yyyy, MMM, dd HH:mm:ss";
            this.colStartTime.DefaultCellStyle = dataGridViewCellStyle4;
            this.colStartTime.HeaderText = "Start Time";
            this.colStartTime.Name = "colStartTime";
            this.colStartTime.ReadOnly = true;
            this.colStartTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colStartTime.Width = 73;
            // 
            // colEndTime
            // 
            this.colEndTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colEndTime.DataPropertyName = "end_time";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Format = "yyyy, MMM, dd HH:mm:ss";
            dataGridViewCellStyle5.NullValue = null;
            this.colEndTime.DefaultCellStyle = dataGridViewCellStyle5;
            this.colEndTime.HeaderText = "End Time";
            this.colEndTime.Name = "colEndTime";
            this.colEndTime.ReadOnly = true;
            this.colEndTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEndTime.Width = 71;
            // 
            // colMesType
            // 
            this.colMesType.DataPropertyName = "measure_type";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colMesType.DefaultCellStyle = dataGridViewCellStyle6;
            this.colMesType.HeaderText = "Measure Type";
            this.colMesType.Name = "colMesType";
            this.colMesType.ReadOnly = true;
            this.colMesType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colMesType.Visible = false;
            this.colMesType.Width = 120;
            // 
            // colResult
            // 
            this.colResult.DataPropertyName = "MeasureDisplay";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colResult.DefaultCellStyle = dataGridViewCellStyle7;
            this.colResult.HeaderText = "Result";
            this.colResult.Name = "colResult";
            this.colResult.ReadOnly = true;
            this.colResult.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvMeasureDetail
            // 
            this.dgvMeasureDetail.AllowUserToAddRows = false;
            this.dgvMeasureDetail.AllowUserToDeleteRows = false;
            this.dgvMeasureDetail.AllowUserToResizeRows = false;
            this.dgvMeasureDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMeasureDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial Narrow", 12F);
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvMeasureDetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvMeasureDetail.ColumnHeadersHeight = 30;
            this.dgvMeasureDetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDtlNo,
            this.colDtlTime,
            this.colDtlActualValue});
            this.dgvMeasureDetail.EnableHeadersVisualStyles = false;
            this.dgvMeasureDetail.Location = new System.Drawing.Point(8, 295);
            this.dgvMeasureDetail.MultiSelect = false;
            this.dgvMeasureDetail.Name = "dgvMeasureDetail";
            this.dgvMeasureDetail.ReadOnly = true;
            this.dgvMeasureDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvMeasureDetail.RowHeadersVisible = false;
            this.dgvMeasureDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMeasureDetail.Size = new System.Drawing.Size(858, 302);
            this.dgvMeasureDetail.TabIndex = 146;
            // 
            // colDtlNo
            // 
            this.colDtlNo.DataPropertyName = "no";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colDtlNo.DefaultCellStyle = dataGridViewCellStyle9;
            this.colDtlNo.HeaderText = "No";
            this.colDtlNo.Name = "colDtlNo";
            this.colDtlNo.ReadOnly = true;
            this.colDtlNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDtlNo.Width = 50;
            // 
            // colDtlTime
            // 
            this.colDtlTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colDtlTime.DataPropertyName = "samples_time";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.Format = "yyyy, MMM, dd HH:mm:ss.fff";
            this.colDtlTime.DefaultCellStyle = dataGridViewCellStyle10;
            this.colDtlTime.HeaderText = "Time";
            this.colDtlTime.Name = "colDtlTime";
            this.colDtlTime.ReadOnly = true;
            this.colDtlTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDtlTime.Width = 43;
            // 
            // colDtlActualValue
            // 
            this.colDtlActualValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colDtlActualValue.DataPropertyName = "actual_value";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle11.Format = "N0";
            this.colDtlActualValue.DefaultCellStyle = dataGridViewCellStyle11;
            this.colDtlActualValue.HeaderText = "Value (V)";
            this.colDtlActualValue.Name = "colDtlActualValue";
            this.colDtlActualValue.ReadOnly = true;
            this.colDtlActualValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colDtlActualValue.Width = 69;
            // 
            // lblSensor
            // 
            this.lblSensor.Location = new System.Drawing.Point(245, 23);
            this.lblSensor.Name = "lblSensor";
            this.lblSensor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblSensor.Size = new System.Drawing.Size(78, 26);
            this.lblSensor.TabIndex = 149;
            this.lblSensor.Text = "Sensor";
            // 
            // lblBlock
            // 
            this.lblBlock.Location = new System.Drawing.Point(17, 23);
            this.lblBlock.Name = "lblBlock";
            this.lblBlock.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblBlock.Size = new System.Drawing.Size(78, 26);
            this.lblBlock.TabIndex = 150;
            this.lblBlock.Text = "Block";
            // 
            // cmbSensor
            // 
            this.cmbSensor.DisplayMember = "SensorName";
            this.cmbSensor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSensor.FormattingEnabled = true;
            this.cmbSensor.Location = new System.Drawing.Point(325, 21);
            this.cmbSensor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbSensor.Name = "cmbSensor";
            this.cmbSensor.Size = new System.Drawing.Size(144, 28);
            this.cmbSensor.TabIndex = 147;
            this.cmbSensor.ValueMember = "SensorId";
            // 
            // cmbBlock
            // 
            this.cmbBlock.DisplayMember = "BlockName";
            this.cmbBlock.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBlock.FormattingEnabled = true;
            this.cmbBlock.Location = new System.Drawing.Point(97, 21);
            this.cmbBlock.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbBlock.Name = "cmbBlock";
            this.cmbBlock.Size = new System.Drawing.Size(144, 28);
            this.cmbBlock.TabIndex = 148;
            this.cmbBlock.ValueMember = "BlockId";
            this.cmbBlock.SelectedIndexChanged += new System.EventHandler(this.cmbBlock_SelectedIndexChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(513, 21);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(103, 28);
            this.btnSearch.TabIndex = 155;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // AlarmReport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblSensor);
            this.Controls.Add(this.lblBlock);
            this.Controls.Add(this.cmbSensor);
            this.Controls.Add(this.cmbBlock);
            this.Controls.Add(this.dgvMeasureDetail);
            this.Controls.Add(this.dgvMeasure);
            this.Font = new System.Drawing.Font("Arial Narrow", 12F);
            this.Name = "AlarmReport";
            this.Size = new System.Drawing.Size(873, 611);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasureDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseCommon.ControlTemplate.dgv dgvMeasure;
        private BaseCommon.ControlTemplate.dgv dgvMeasureDetail;
        private System.Windows.Forms.Label lblSensor;
        private System.Windows.Forms.Label lblBlock;
        private System.Windows.Forms.ComboBox cmbSensor;
        private System.Windows.Forms.ComboBox cmbBlock;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMesId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSensorId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBlockName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSensorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAlarmValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMesType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDtlNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDtlTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDtlActualValue;
    }
}
