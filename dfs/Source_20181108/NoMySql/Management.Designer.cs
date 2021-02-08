namespace NoMySql
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
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
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
            this.btSaveSetting = new System.Windows.Forms.Button();
            this.MainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvDevice)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 2;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 462F));
            this.MainLayout.Controls.Add(this.grvDevice, 0, 0);
            this.MainLayout.Controls.Add(this.panel4, 1, 0);
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 0);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 1;
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.Size = new System.Drawing.Size(922, 411);
            this.MainLayout.TabIndex = 1;
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
            this.grvDevice.Size = new System.Drawing.Size(452, 403);
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
            this.panel4.Controls.Add(this.btSaveSetting);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(463, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(456, 405);
            this.panel4.TabIndex = 0;
            // 
            // btResetDefault
            // 
            this.btResetDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btResetDefault.Location = new System.Drawing.Point(235, 332);
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
            this.txtSamples.Location = new System.Drawing.Point(350, 280);
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
            this.label13.Location = new System.Drawing.Point(254, 283);
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
            this.label3.Location = new System.Drawing.Point(232, 178);
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
            this.label17.Location = new System.Drawing.Point(2, 178);
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
            this.txtFailLevel.Location = new System.Drawing.Point(350, 246);
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
            this.txtPeriod.Location = new System.Drawing.Point(350, 212);
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
            this.txtAlarmValue.Location = new System.Drawing.Point(120, 212);
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
            this.label14.Location = new System.Drawing.Point(247, 215);
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
            this.label16.Location = new System.Drawing.Point(249, 249);
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
            this.label15.Location = new System.Drawing.Point(3, 215);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(100, 18);
            this.label15.TabIndex = 16;
            this.label15.Text = "Alarm Value :";
            // 
            // btSaveSetting
            // 
            this.btSaveSetting.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btSaveSetting.Location = new System.Drawing.Point(98, 332);
            this.btSaveSetting.Margin = new System.Windows.Forms.Padding(4);
            this.btSaveSetting.Name = "btSaveSetting";
            this.btSaveSetting.Size = new System.Drawing.Size(126, 43);
            this.btSaveSetting.TabIndex = 9;
            this.btSaveSetting.Text = "Save ";
            this.btSaveSetting.UseVisualStyleBackColor = true;
            this.btSaveSetting.Click += new System.EventHandler(this.btSaveSetting_Click);
            // 
            // Management
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(922, 411);
            this.Controls.Add(this.MainLayout);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(938, 450);
            this.Name = "Management";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Setting Management";
            this.MainLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grvDevice)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainLayout;
        private System.Windows.Forms.DataGridView grvDevice;
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
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btResetDefault;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.TextBox txtSamples;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.Label label18;
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
        private System.Windows.Forms.Button btSaveSetting;
    }
}