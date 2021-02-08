using DSF602.View.ControlLayout;

namespace DSF602.View
{
    partial class DefaultSettingPopup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lbLowVal = new System.Windows.Forms.Label();
            this.lbUpVal = new System.Windows.Forms.Label();
            this.lbDecayTime = new System.Windows.Forms.Label();
            this.lbDecayStopTime = new System.Windows.Forms.Label();
            this.txtUpVal = new DSF602.View.ControlLayout.NumericTextBox();
            this.txtLowVal = new DSF602.View.ControlLayout.NumericTextBox();
            this.txtDecayTime = new DSF602.View.ControlLayout.NumericTextBox();
            this.txtDecayStopTime = new DSF602.View.ControlLayout.NumericTextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btSaveSetting = new System.Windows.Forms.Button();
            this.lbDecayParams = new System.Windows.Forms.Label();
            this.lbIBParams = new System.Windows.Forms.Label();
            this.txtIonStopTimeCheck = new DSF602.View.ControlLayout.NumericTextBox();
            this.txtIonCheck = new DSF602.View.ControlLayout.NumericTextBox();
            this.lbIonStopTime = new System.Windows.Forms.Label();
            this.lbIonBalance = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.chkAutoTime = new System.Windows.Forms.CheckBox();
            this.chkDecayGrp = new System.Windows.Forms.CheckBox();
            this.chkVoltGrp = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtVoltAlarm = new DSF602.View.ControlLayout.NumericTextBox();
            this.lbAlarmVolt = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtIonAlarm = new DSF602.View.ControlLayout.NumericTextBox();
            this.lbAlarmIon = new System.Windows.Forms.Label();
            this.chkIonGrp = new System.Windows.Forms.CheckBox();
            this.btResetDefault = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.dg = new System.Windows.Forms.DataGridView();
            this.colNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTime = new DSF602.View.ControlLayout.DataGridViewMaskedTextColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chkSun = new System.Windows.Forms.CheckBox();
            this.chkSat = new System.Windows.Forms.CheckBox();
            this.chkFri = new System.Windows.Forms.CheckBox();
            this.chkThu = new System.Windows.Forms.CheckBox();
            this.chkWed = new System.Windows.Forms.CheckBox();
            this.chkTue = new System.Windows.Forms.CheckBox();
            this.chkMon = new System.Windows.Forms.CheckBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewMaskedTextColumn1 = new DSF602.View.ControlLayout.DataGridViewMaskedTextColumn();
            this.btnAddRow = new System.Windows.Forms.Button();
            this.btnRemoveRow = new System.Windows.Forms.Button();
            this.pnVolt = new System.Windows.Forms.GroupBox();
            this.pnIB = new System.Windows.Forms.GroupBox();
            this.pnDecay = new System.Windows.Forms.GroupBox();
            this.pnAuto = new DSF602.View.MyPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnVolt.SuspendLayout();
            this.pnIB.SuspendLayout();
            this.pnDecay.SuspendLayout();
            this.pnAuto.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbLowVal
            // 
            this.lbLowVal.AutoSize = true;
            this.lbLowVal.Location = new System.Drawing.Point(8, 87);
            this.lbLowVal.Name = "lbLowVal";
            this.lbLowVal.Size = new System.Drawing.Size(62, 20);
            this.lbLowVal.TabIndex = 0;
            this.lbLowVal.Text = "Decay to";
            // 
            // lbUpVal
            // 
            this.lbUpVal.AutoSize = true;
            this.lbUpVal.Location = new System.Drawing.Point(8, 55);
            this.lbUpVal.Name = "lbUpVal";
            this.lbUpVal.Size = new System.Drawing.Size(62, 20);
            this.lbUpVal.TabIndex = 1;
            this.lbUpVal.Text = "Charging";
            // 
            // lbDecayTime
            // 
            this.lbDecayTime.AutoSize = true;
            this.lbDecayTime.Location = new System.Drawing.Point(8, 119);
            this.lbDecayTime.Name = "lbDecayTime";
            this.lbDecayTime.Size = new System.Drawing.Size(85, 20);
            this.lbDecayTime.TabIndex = 2;
            this.lbDecayTime.Text = "Decay Alarm";
            // 
            // lbDecayStopTime
            // 
            this.lbDecayStopTime.AutoSize = true;
            this.lbDecayStopTime.Location = new System.Drawing.Point(9, 151);
            this.lbDecayStopTime.Name = "lbDecayStopTime";
            this.lbDecayStopTime.Size = new System.Drawing.Size(79, 20);
            this.lbDecayStopTime.TabIndex = 3;
            this.lbDecayStopTime.Text = "Stop Decay";
            // 
            // txtUpVal
            // 
            this.txtUpVal.Location = new System.Drawing.Point(196, 52);
            this.txtUpVal.Name = "txtUpVal";
            this.txtUpVal.Size = new System.Drawing.Size(148, 26);
            this.txtUpVal.TabIndex = 4;
            this.txtUpVal.Text = "1000";
            this.txtUpVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLowVal
            // 
            this.txtLowVal.Location = new System.Drawing.Point(196, 84);
            this.txtLowVal.Name = "txtLowVal";
            this.txtLowVal.Size = new System.Drawing.Size(148, 26);
            this.txtLowVal.TabIndex = 5;
            this.txtLowVal.Text = "100";
            this.txtLowVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDecayTime
            // 
            this.txtDecayTime.Location = new System.Drawing.Point(196, 116);
            this.txtDecayTime.Name = "txtDecayTime";
            this.txtDecayTime.Size = new System.Drawing.Size(148, 26);
            this.txtDecayTime.TabIndex = 6;
            this.txtDecayTime.Text = "3";
            this.txtDecayTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDecayStopTime
            // 
            this.txtDecayStopTime.Location = new System.Drawing.Point(196, 148);
            this.txtDecayStopTime.Name = "txtDecayStopTime";
            this.txtDecayStopTime.Size = new System.Drawing.Size(148, 26);
            this.txtDecayStopTime.TabIndex = 7;
            this.txtDecayStopTime.Text = "5";
            this.txtDecayStopTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(512, 393);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(104, 28);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btSaveSetting
            // 
            this.btSaveSetting.AccessibleDescription = "s";
            this.btSaveSetting.Location = new System.Drawing.Point(399, 393);
            this.btSaveSetting.Name = "btSaveSetting";
            this.btSaveSetting.Size = new System.Drawing.Size(104, 28);
            this.btSaveSetting.TabIndex = 9;
            this.btSaveSetting.Text = "OK";
            this.btSaveSetting.UseVisualStyleBackColor = true;
            this.btSaveSetting.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lbDecayParams
            // 
            this.lbDecayParams.AutoSize = true;
            this.lbDecayParams.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDecayParams.Location = new System.Drawing.Point(8, 26);
            this.lbDecayParams.Name = "lbDecayParams";
            this.lbDecayParams.Size = new System.Drawing.Size(81, 20);
            this.lbDecayParams.TabIndex = 10;
            this.lbDecayParams.Text = "Decay Time";
            // 
            // lbIBParams
            // 
            this.lbIBParams.AutoSize = true;
            this.lbIBParams.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbIBParams.Location = new System.Drawing.Point(8, 188);
            this.lbIBParams.Name = "lbIBParams";
            this.lbIBParams.Size = new System.Drawing.Size(82, 20);
            this.lbIBParams.TabIndex = 11;
            this.lbIBParams.Text = "Ion Balance";
            // 
            // txtIonStopTimeCheck
            // 
            this.txtIonStopTimeCheck.Location = new System.Drawing.Point(196, 242);
            this.txtIonStopTimeCheck.Name = "txtIonStopTimeCheck";
            this.txtIonStopTimeCheck.Size = new System.Drawing.Size(148, 26);
            this.txtIonStopTimeCheck.TabIndex = 15;
            this.txtIonStopTimeCheck.Text = "5";
            this.txtIonStopTimeCheck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtIonCheck
            // 
            this.txtIonCheck.Location = new System.Drawing.Point(196, 210);
            this.txtIonCheck.Name = "txtIonCheck";
            this.txtIonCheck.Size = new System.Drawing.Size(148, 26);
            this.txtIonCheck.TabIndex = 14;
            this.txtIonCheck.Text = "35";
            this.txtIonCheck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lbIonStopTime
            // 
            this.lbIonStopTime.AutoSize = true;
            this.lbIonStopTime.Location = new System.Drawing.Point(9, 245);
            this.lbIonStopTime.Name = "lbIonStopTime";
            this.lbIonStopTime.Size = new System.Drawing.Size(109, 20);
            this.lbIonStopTime.TabIndex = 13;
            this.lbIonStopTime.Text = "Stop IB Measure";
            // 
            // lbIonBalance
            // 
            this.lbIonBalance.AutoSize = true;
            this.lbIonBalance.Location = new System.Drawing.Point(9, 213);
            this.lbIonBalance.Name = "lbIonBalance";
            this.lbIonBalance.Size = new System.Drawing.Size(128, 20);
            this.lbIonBalance.TabIndex = 12;
            this.lbIonBalance.Text = "Alarm Peak Voltage";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(167, 213);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 20);
            this.label9.TabIndex = 16;
            this.label9.Text = "+/-";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(167, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 20);
            this.label10.TabIndex = 17;
            this.label10.Text = "+/-";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(167, 87);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(25, 20);
            this.label11.TabIndex = 18;
            this.label11.Text = "+/-";
            // 
            // chkAutoTime
            // 
            this.chkAutoTime.AutoSize = true;
            this.chkAutoTime.Checked = true;
            this.chkAutoTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoTime.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoTime.Location = new System.Drawing.Point(424, 33);
            this.chkAutoTime.Name = "chkAutoTime";
            this.chkAutoTime.Size = new System.Drawing.Size(148, 24);
            this.chkAutoTime.TabIndex = 19;
            this.chkAutoTime.Text = "Auto Measure Time";
            this.chkAutoTime.CheckedChanged += new System.EventHandler(this.chkAuto_CheckedChanged);
            // 
            // chkDecayGrp
            // 
            this.chkDecayGrp.AutoSize = true;
            this.chkDecayGrp.Checked = true;
            this.chkDecayGrp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDecayGrp.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDecayGrp.ForeColor = System.Drawing.Color.DodgerBlue;
            this.chkDecayGrp.Location = new System.Drawing.Point(30, 88);
            this.chkDecayGrp.Name = "chkDecayGrp";
            this.chkDecayGrp.Size = new System.Drawing.Size(214, 24);
            this.chkDecayGrp.TabIndex = 23;
            this.chkDecayGrp.Text = "DECAY TIME && ION BALANCE";
            this.chkDecayGrp.CheckedChanged += new System.EventHandler(this.titleDecay_CheckedChanged);
            // 
            // chkVoltGrp
            // 
            this.chkVoltGrp.AutoSize = true;
            this.chkVoltGrp.Checked = true;
            this.chkVoltGrp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVoltGrp.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkVoltGrp.ForeColor = System.Drawing.Color.DodgerBlue;
            this.chkVoltGrp.Location = new System.Drawing.Point(31, 12);
            this.chkVoltGrp.Name = "chkVoltGrp";
            this.chkVoltGrp.Size = new System.Drawing.Size(91, 24);
            this.chkVoltGrp.TabIndex = 24;
            this.chkVoltGrp.Text = "VOLTAGE";
            this.chkVoltGrp.CheckedChanged += new System.EventHandler(this.titleVolt_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(167, 21);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(25, 20);
            this.label15.TabIndex = 27;
            this.label15.Text = "+/-";
            // 
            // txtVoltAlarm
            // 
            this.txtVoltAlarm.Location = new System.Drawing.Point(196, 18);
            this.txtVoltAlarm.Name = "txtVoltAlarm";
            this.txtVoltAlarm.Size = new System.Drawing.Size(148, 26);
            this.txtVoltAlarm.TabIndex = 26;
            this.txtVoltAlarm.Text = "1000";
            this.txtVoltAlarm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lbAlarmVolt
            // 
            this.lbAlarmVolt.AutoSize = true;
            this.lbAlarmVolt.Location = new System.Drawing.Point(8, 21);
            this.lbAlarmVolt.Name = "lbAlarmVolt";
            this.lbAlarmVolt.Size = new System.Drawing.Size(81, 20);
            this.lbAlarmVolt.TabIndex = 25;
            this.lbAlarmVolt.Text = "Alarm Value";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(156, 21);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(25, 20);
            this.label17.TabIndex = 31;
            this.label17.Text = "+/-";
            // 
            // txtIonAlarm
            // 
            this.txtIonAlarm.Location = new System.Drawing.Point(183, 18);
            this.txtIonAlarm.Name = "txtIonAlarm";
            this.txtIonAlarm.Size = new System.Drawing.Size(148, 26);
            this.txtIonAlarm.TabIndex = 30;
            this.txtIonAlarm.Text = "1000";
            this.txtIonAlarm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lbAlarmIon
            // 
            this.lbAlarmIon.AutoSize = true;
            this.lbAlarmIon.Location = new System.Drawing.Point(17, 21);
            this.lbAlarmIon.Name = "lbAlarmIon";
            this.lbAlarmIon.Size = new System.Drawing.Size(81, 20);
            this.lbAlarmIon.TabIndex = 29;
            this.lbAlarmIon.Text = "Alarm Value";
            // 
            // chkIonGrp
            // 
            this.chkIonGrp.AutoSize = true;
            this.chkIonGrp.Checked = true;
            this.chkIonGrp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIonGrp.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIonGrp.ForeColor = System.Drawing.Color.DodgerBlue;
            this.chkIonGrp.Location = new System.Drawing.Point(476, 11);
            this.chkIonGrp.Name = "chkIonGrp";
            this.chkIonGrp.Size = new System.Drawing.Size(117, 24);
            this.chkIonGrp.TabIndex = 28;
            this.chkIonGrp.Text = "ION BALANCE";
            this.chkIonGrp.CheckedChanged += new System.EventHandler(this.titleIon_CheckedChanged);
            // 
            // btResetDefault
            // 
            this.btResetDefault.Location = new System.Drawing.Point(256, 393);
            this.btResetDefault.Name = "btResetDefault";
            this.btResetDefault.Size = new System.Drawing.Size(137, 28);
            this.btResetDefault.TabIndex = 32;
            this.btResetDefault.Text = "Restore Default";
            this.btResetDefault.UseVisualStyleBackColor = true;
            this.btResetDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(358, 55);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(26, 20);
            this.label20.TabIndex = 33;
            this.label20.Text = "(V)";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(358, 87);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(26, 20);
            this.label21.TabIndex = 34;
            this.label21.Text = "(V)";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(358, 119);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(24, 20);
            this.label22.TabIndex = 35;
            this.label22.Text = "(s)";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(358, 151);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(24, 20);
            this.label23.TabIndex = 36;
            this.label23.Text = "(s)";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(358, 245);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(24, 20);
            this.label24.TabIndex = 37;
            this.label24.Text = "(s)";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(358, 213);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(26, 20);
            this.label25.TabIndex = 38;
            this.label25.Text = "(V)";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(337, 21);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(26, 20);
            this.label26.TabIndex = 40;
            this.label26.Text = "(V)";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(356, 21);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(26, 20);
            this.label27.TabIndex = 39;
            this.label27.Text = "(V)";
            // 
            // dg
            // 
            this.dg.AllowUserToAddRows = false;
            this.dg.BackgroundColor = System.Drawing.Color.White;
            this.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNo,
            this.colTime});
            this.dg.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dg.Location = new System.Drawing.Point(4, 34);
            this.dg.Name = "dg";
            this.dg.Size = new System.Drawing.Size(433, 147);
            this.dg.TabIndex = 41;
            this.dg.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dg_CellValidated);
            this.dg.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dg_CellValidating);
            this.dg.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dg_RowsAdded);
            // 
            // colNo
            // 
            this.colNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray;
            this.colNo.DefaultCellStyle = dataGridViewCellStyle1;
            this.colNo.HeaderText = "No.";
            this.colNo.Name = "colNo";
            this.colNo.ReadOnly = true;
            this.colNo.Width = 55;
            // 
            // colTime
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colTime.DefaultCellStyle = dataGridViewCellStyle2;
            this.colTime.HeaderText = "Time (24h)";
            this.colTime.Mask = "00:00";
            this.colTime.Name = "colTime";
            this.colTime.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colTime.Width = 120;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel1.Controls.Add(this.chkSun, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkSat, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkFri, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkThu, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkWed, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkTue, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkMon, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(7, 189);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(433, 26);
            this.tableLayoutPanel1.TabIndex = 43;
            // 
            // chkSun
            // 
            this.chkSun.AutoSize = true;
            this.chkSun.Checked = true;
            this.chkSun.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSun.Location = new System.Drawing.Point(369, 3);
            this.chkSun.Name = "chkSun";
            this.chkSun.Size = new System.Drawing.Size(51, 20);
            this.chkSun.TabIndex = 6;
            this.chkSun.Text = "Sun";
            this.chkSun.UseVisualStyleBackColor = true;
            // 
            // chkSat
            // 
            this.chkSat.AutoSize = true;
            this.chkSat.Checked = true;
            this.chkSat.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSat.Location = new System.Drawing.Point(308, 3);
            this.chkSat.Name = "chkSat";
            this.chkSat.Size = new System.Drawing.Size(47, 20);
            this.chkSat.TabIndex = 5;
            this.chkSat.Text = "Sat";
            this.chkSat.UseVisualStyleBackColor = true;
            // 
            // chkFri
            // 
            this.chkFri.AutoSize = true;
            this.chkFri.Checked = true;
            this.chkFri.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFri.Location = new System.Drawing.Point(247, 3);
            this.chkFri.Name = "chkFri";
            this.chkFri.Size = new System.Drawing.Size(43, 20);
            this.chkFri.TabIndex = 4;
            this.chkFri.Text = "Fri";
            this.chkFri.UseVisualStyleBackColor = true;
            // 
            // chkThu
            // 
            this.chkThu.AutoSize = true;
            this.chkThu.Checked = true;
            this.chkThu.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkThu.Location = new System.Drawing.Point(186, 3);
            this.chkThu.Name = "chkThu";
            this.chkThu.Size = new System.Drawing.Size(49, 20);
            this.chkThu.TabIndex = 3;
            this.chkThu.Text = "Thu";
            this.chkThu.UseVisualStyleBackColor = true;
            // 
            // chkWed
            // 
            this.chkWed.AutoSize = true;
            this.chkWed.Checked = true;
            this.chkWed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWed.Location = new System.Drawing.Point(125, 3);
            this.chkWed.Name = "chkWed";
            this.chkWed.Size = new System.Drawing.Size(55, 20);
            this.chkWed.TabIndex = 2;
            this.chkWed.Text = "Wed";
            this.chkWed.UseVisualStyleBackColor = true;
            // 
            // chkTue
            // 
            this.chkTue.AutoSize = true;
            this.chkTue.Checked = true;
            this.chkTue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTue.Location = new System.Drawing.Point(64, 3);
            this.chkTue.Name = "chkTue";
            this.chkTue.Size = new System.Drawing.Size(50, 20);
            this.chkTue.TabIndex = 1;
            this.chkTue.Text = "Tue";
            this.chkTue.UseVisualStyleBackColor = true;
            // 
            // chkMon
            // 
            this.chkMon.AutoSize = true;
            this.chkMon.Checked = true;
            this.chkMon.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMon.Location = new System.Drawing.Point(3, 3);
            this.chkMon.Name = "chkMon";
            this.chkMon.Size = new System.Drawing.Size(54, 20);
            this.chkMon.TabIndex = 0;
            this.chkMon.Text = "Mon";
            this.chkMon.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumn1.HeaderText = "No.";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewMaskedTextColumn1
            // 
            this.dataGridViewMaskedTextColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "##:##";
            this.dataGridViewMaskedTextColumn1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewMaskedTextColumn1.HeaderText = "Time";
            this.dataGridViewMaskedTextColumn1.Mask = "00:00";
            this.dataGridViewMaskedTextColumn1.Name = "dataGridViewMaskedTextColumn1";
            this.dataGridViewMaskedTextColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // btnAddRow
            // 
            this.btnAddRow.BackgroundImage = global::DSF602.Properties.Resources.add;
            this.btnAddRow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAddRow.Location = new System.Drawing.Point(341, 3);
            this.btnAddRow.Name = "btnAddRow";
            this.btnAddRow.Size = new System.Drawing.Size(45, 28);
            this.btnAddRow.TabIndex = 44;
            this.btnAddRow.UseVisualStyleBackColor = true;
            this.btnAddRow.Click += new System.EventHandler(this.btnAddRow_Click);
            // 
            // btnRemoveRow
            // 
            this.btnRemoveRow.BackgroundImage = global::DSF602.Properties.Resources.remove;
            this.btnRemoveRow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRemoveRow.Location = new System.Drawing.Point(392, 3);
            this.btnRemoveRow.Name = "btnRemoveRow";
            this.btnRemoveRow.Size = new System.Drawing.Size(45, 28);
            this.btnRemoveRow.TabIndex = 45;
            this.btnRemoveRow.UseVisualStyleBackColor = true;
            this.btnRemoveRow.Click += new System.EventHandler(this.btnRemoveRow_Click);
            // 
            // pnVolt
            // 
            this.pnVolt.Controls.Add(this.lbAlarmVolt);
            this.pnVolt.Controls.Add(this.txtVoltAlarm);
            this.pnVolt.Controls.Add(this.label15);
            this.pnVolt.Controls.Add(this.label27);
            this.pnVolt.Location = new System.Drawing.Point(18, 15);
            this.pnVolt.Name = "pnVolt";
            this.pnVolt.Size = new System.Drawing.Size(427, 55);
            this.pnVolt.TabIndex = 46;
            this.pnVolt.TabStop = false;
            // 
            // pnIB
            // 
            this.pnIB.Controls.Add(this.lbAlarmIon);
            this.pnIB.Controls.Add(this.txtIonAlarm);
            this.pnIB.Controls.Add(this.label17);
            this.pnIB.Controls.Add(this.label26);
            this.pnIB.Location = new System.Drawing.Point(462, 15);
            this.pnIB.Name = "pnIB";
            this.pnIB.Size = new System.Drawing.Size(427, 55);
            this.pnIB.TabIndex = 47;
            this.pnIB.TabStop = false;
            // 
            // pnDecay
            // 
            this.pnDecay.Controls.Add(this.chkAutoTime);
            this.pnDecay.Controls.Add(this.pnAuto);
            this.pnDecay.Controls.Add(this.lbDecayParams);
            this.pnDecay.Controls.Add(this.lbLowVal);
            this.pnDecay.Controls.Add(this.label4);
            this.pnDecay.Controls.Add(this.label3);
            this.pnDecay.Controls.Add(this.label2);
            this.pnDecay.Controls.Add(this.label1);
            this.pnDecay.Controls.Add(this.lbUpVal);
            this.pnDecay.Controls.Add(this.lbDecayTime);
            this.pnDecay.Controls.Add(this.lbDecayStopTime);
            this.pnDecay.Controls.Add(this.txtUpVal);
            this.pnDecay.Controls.Add(this.txtLowVal);
            this.pnDecay.Controls.Add(this.label25);
            this.pnDecay.Controls.Add(this.txtDecayTime);
            this.pnDecay.Controls.Add(this.label24);
            this.pnDecay.Controls.Add(this.txtDecayStopTime);
            this.pnDecay.Controls.Add(this.label23);
            this.pnDecay.Controls.Add(this.lbIBParams);
            this.pnDecay.Controls.Add(this.label22);
            this.pnDecay.Controls.Add(this.lbIonBalance);
            this.pnDecay.Controls.Add(this.label21);
            this.pnDecay.Controls.Add(this.lbIonStopTime);
            this.pnDecay.Controls.Add(this.label20);
            this.pnDecay.Controls.Add(this.txtIonCheck);
            this.pnDecay.Controls.Add(this.txtIonStopTimeCheck);
            this.pnDecay.Controls.Add(this.label9);
            this.pnDecay.Controls.Add(this.label10);
            this.pnDecay.Controls.Add(this.label11);
            this.pnDecay.Location = new System.Drawing.Point(18, 92);
            this.pnDecay.Name = "pnDecay";
            this.pnDecay.Size = new System.Drawing.Size(871, 286);
            this.pnDecay.TabIndex = 48;
            this.pnDecay.TabStop = false;
            // 
            // pnAuto
            // 
            this.pnAuto.Controls.Add(this.dg);
            this.pnAuto.Controls.Add(this.btnAddRow);
            this.pnAuto.Controls.Add(this.btnRemoveRow);
            this.pnAuto.Controls.Add(this.tableLayoutPanel1);
            this.pnAuto.Location = new System.Drawing.Point(417, 27);
            this.pnAuto.Name = "pnAuto";
            this.pnAuto.Size = new System.Drawing.Size(443, 223);
            this.pnAuto.TabIndex = 46;
            this.pnAuto.EnabledChanged += new System.EventHandler(this.pnAuto_EnabledChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(113, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "(10-100)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(127, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "(0-10)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "(0-1995)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "(10-2000)";
            // 
            // DefaultSettingPopup
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(907, 432);
            this.Controls.Add(this.chkIonGrp);
            this.Controls.Add(this.chkDecayGrp);
            this.Controls.Add(this.chkVoltGrp);
            this.Controls.Add(this.pnDecay);
            this.Controls.Add(this.pnIB);
            this.Controls.Add(this.pnVolt);
            this.Controls.Add(this.btResetDefault);
            this.Controls.Add(this.btSaveSetting);
            this.Controls.Add(this.btnCancel);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(351, 124);
            this.Name = "DefaultSettingPopup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Default Setting";
            this.Load += new System.EventHandler(this.DefaultSettingPopup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dg)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.pnVolt.ResumeLayout(false);
            this.pnVolt.PerformLayout();
            this.pnIB.ResumeLayout(false);
            this.pnIB.PerformLayout();
            this.pnDecay.ResumeLayout(false);
            this.pnDecay.PerformLayout();
            this.pnAuto.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbLowVal;
        private System.Windows.Forms.Label lbUpVal;
        private System.Windows.Forms.Label lbDecayTime;
        private System.Windows.Forms.Label lbDecayStopTime;
        private NumericTextBox txtUpVal;
        private NumericTextBox txtLowVal;
        private NumericTextBox txtDecayTime;
        private NumericTextBox txtDecayStopTime;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btSaveSetting;
        private System.Windows.Forms.Label lbDecayParams;
        private System.Windows.Forms.Label lbIBParams;
        private NumericTextBox txtIonStopTimeCheck;
        private NumericTextBox txtIonCheck;
        private System.Windows.Forms.Label lbIonStopTime;
        private System.Windows.Forms.Label lbIonBalance;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chkAutoTime;
        private System.Windows.Forms.CheckBox chkDecayGrp;
        private System.Windows.Forms.CheckBox chkVoltGrp;
        private System.Windows.Forms.Label label15;
        private NumericTextBox txtVoltAlarm;
        private System.Windows.Forms.Label lbAlarmVolt;
        private System.Windows.Forms.Label label17;
        private NumericTextBox txtIonAlarm;
        private System.Windows.Forms.Label lbAlarmIon;
        private System.Windows.Forms.CheckBox chkIonGrp;
        private System.Windows.Forms.Button btResetDefault;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.DataGridView dg;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox chkSun;
        private System.Windows.Forms.CheckBox chkSat;
        private System.Windows.Forms.CheckBox chkFri;
        private System.Windows.Forms.CheckBox chkThu;
        private System.Windows.Forms.CheckBox chkWed;
        private System.Windows.Forms.CheckBox chkTue;
        private System.Windows.Forms.CheckBox chkMon;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private ControlLayout.DataGridViewMaskedTextColumn dataGridViewMaskedTextColumn1;
        private System.Windows.Forms.Button btnAddRow;
        private System.Windows.Forms.Button btnRemoveRow;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNo;
        private ControlLayout.DataGridViewMaskedTextColumn colTime;
        private System.Windows.Forms.GroupBox pnVolt;
        private System.Windows.Forms.GroupBox pnIB;
        private System.Windows.Forms.GroupBox pnDecay;
        private MyPanel pnAuto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}