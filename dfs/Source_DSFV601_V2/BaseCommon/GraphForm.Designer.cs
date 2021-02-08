namespace BaseCommon
{
    partial class GraphForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnDevice5Stop = new System.Windows.Forms.Button();
            this.btnDevice4Stop = new System.Windows.Forms.Button();
            this.btnDevice3Stop = new System.Windows.Forms.Button();
            this.btnDevice2Stop = new System.Windows.Forms.Button();
            this.btnDevice1Stop = new System.Windows.Forms.Button();
            this.lblDevice5 = new System.Windows.Forms.Label();
            this.lblDevice4 = new System.Windows.Forms.Label();
            this.lblDevice3 = new System.Windows.Forms.Label();
            this.lblDevice2 = new System.Windows.Forms.Label();
            this.lblDevice1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDev5ActualValue = new System.Windows.Forms.TextBox();
            this.txtDev4ActualValue = new System.Windows.Forms.TextBox();
            this.txtDev3ActualValue = new System.Windows.Forms.TextBox();
            this.txtDev5AlarmValue = new System.Windows.Forms.TextBox();
            this.txtDev4AlarmValue = new System.Windows.Forms.TextBox();
            this.txtDev3AlarmValue = new System.Windows.Forms.TextBox();
            this.txtDev1AlarmValue = new System.Windows.Forms.TextBox();
            this.txtDev1ActualValue = new System.Windows.Forms.TextBox();
            this.txtDev2AlarmValue = new System.Windows.Forms.TextBox();
            this.txtDev2ActualValue = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnManagement = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.dgvMeasure = new System.Windows.Forms.DataGridView();
            this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeStamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnWalkingDevice = new System.Windows.Forms.Button();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.txtFailLevel = new System.Windows.Forms.TextBox();
            this.txtAverage = new System.Windows.Forms.TextBox();
            this.txtPeriod = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblRstWalking = new System.Windows.Forms.Label();
            this.lblWalkingTest = new System.Windows.Forms.Label();
            this.lblAlarmTest = new System.Windows.Forms.Label();
            this.lblReset = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.lblTestDeviceType = new System.Windows.Forms.Label();
            this.lblOverViewValue = new System.Windows.Forms.Label();
            this.lblDeviceType = new System.Windows.Forms.Label();
            this.pnlBottomTool = new System.Windows.Forms.Panel();
            this.btn600s = new System.Windows.Forms.Button();
            this.btn480s = new System.Windows.Forms.Button();
            this.btn360s = new System.Windows.Forms.Button();
            this.btn180s = new System.Windows.Forms.Button();
            this.btnAddPoint = new System.Windows.Forms.Button();
            this.lblXAxis = new System.Windows.Forms.Label();
            this.btn20000V = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn15000V = new System.Windows.Forms.Button();
            this.txtTimeTest = new System.Windows.Forms.TextBox();
            this.btn10000V = new System.Windows.Forms.Button();
            this.btn5000V = new System.Windows.Forms.Button();
            this.btn90s = new System.Windows.Forms.Button();
            this.btn2000V = new System.Windows.Forms.Button();
            this.btn120s = new System.Windows.Forms.Button();
            this.btn1000V = new System.Windows.Forms.Button();
            this.btn500V = new System.Windows.Forms.Button();
            this.btn60s = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pdeGraph = new GraphLib.PlotterDisplayEx();
            this.timerWalkingTest = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasure)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlBottomTool.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnAbout);
            this.panel1.Controls.Add(this.btnDevice5Stop);
            this.panel1.Controls.Add(this.btnDevice4Stop);
            this.panel1.Controls.Add(this.btnDevice3Stop);
            this.panel1.Controls.Add(this.btnDevice2Stop);
            this.panel1.Controls.Add(this.btnDevice1Stop);
            this.panel1.Controls.Add(this.lblDevice5);
            this.panel1.Controls.Add(this.lblDevice4);
            this.panel1.Controls.Add(this.lblDevice3);
            this.panel1.Controls.Add(this.lblDevice2);
            this.panel1.Controls.Add(this.lblDevice1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtDev5ActualValue);
            this.panel1.Controls.Add(this.txtDev4ActualValue);
            this.panel1.Controls.Add(this.txtDev3ActualValue);
            this.panel1.Controls.Add(this.txtDev5AlarmValue);
            this.panel1.Controls.Add(this.txtDev4AlarmValue);
            this.panel1.Controls.Add(this.txtDev3AlarmValue);
            this.panel1.Controls.Add(this.txtDev1AlarmValue);
            this.panel1.Controls.Add(this.txtDev1ActualValue);
            this.panel1.Controls.Add(this.txtDev2AlarmValue);
            this.panel1.Controls.Add(this.txtDev2ActualValue);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnManagement);
            this.panel1.Controls.Add(this.btnReport);
            this.panel1.Controls.Add(this.dgvMeasure);
            this.panel1.Controls.Add(this.btnWalkingDevice);
            this.panel1.Controls.Add(this.txtCount);
            this.panel1.Controls.Add(this.txtFailLevel);
            this.panel1.Controls.Add(this.txtAverage);
            this.panel1.Controls.Add(this.txtPeriod);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.lblRstWalking);
            this.panel1.Controls.Add(this.lblWalkingTest);
            this.panel1.Controls.Add(this.lblAlarmTest);
            this.panel1.Controls.Add(this.lblReset);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(694, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(358, 668);
            this.panel1.TabIndex = 0;
            // 
            // btnAbout
            // 
            this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAbout.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.Location = new System.Drawing.Point(241, 487);
            this.btnAbout.Margin = new System.Windows.Forms.Padding(4);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(107, 41);
            this.btnAbout.TabIndex = 179;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            // 
            // btnDevice5Stop
            // 
            this.btnDevice5Stop.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDevice5Stop.Location = new System.Drawing.Point(131, 265);
            this.btnDevice5Stop.Margin = new System.Windows.Forms.Padding(4);
            this.btnDevice5Stop.Name = "btnDevice5Stop";
            this.btnDevice5Stop.Size = new System.Drawing.Size(62, 35);
            this.btnDevice5Stop.TabIndex = 178;
            this.btnDevice5Stop.Text = "Stop";
            this.btnDevice5Stop.UseCompatibleTextRendering = true;
            this.btnDevice5Stop.UseVisualStyleBackColor = true;
            this.btnDevice5Stop.Click += new System.EventHandler(this.btnDevice5Stop_Click);
            // 
            // btnDevice4Stop
            // 
            this.btnDevice4Stop.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDevice4Stop.Location = new System.Drawing.Point(131, 226);
            this.btnDevice4Stop.Margin = new System.Windows.Forms.Padding(4);
            this.btnDevice4Stop.Name = "btnDevice4Stop";
            this.btnDevice4Stop.Size = new System.Drawing.Size(62, 35);
            this.btnDevice4Stop.TabIndex = 177;
            this.btnDevice4Stop.Text = "Stop";
            this.btnDevice4Stop.UseCompatibleTextRendering = true;
            this.btnDevice4Stop.UseVisualStyleBackColor = true;
            this.btnDevice4Stop.Click += new System.EventHandler(this.btnDevice4Stop_Click);
            // 
            // btnDevice3Stop
            // 
            this.btnDevice3Stop.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDevice3Stop.Location = new System.Drawing.Point(131, 184);
            this.btnDevice3Stop.Margin = new System.Windows.Forms.Padding(4);
            this.btnDevice3Stop.Name = "btnDevice3Stop";
            this.btnDevice3Stop.Size = new System.Drawing.Size(62, 35);
            this.btnDevice3Stop.TabIndex = 176;
            this.btnDevice3Stop.Text = "Stop";
            this.btnDevice3Stop.UseCompatibleTextRendering = true;
            this.btnDevice3Stop.UseVisualStyleBackColor = true;
            this.btnDevice3Stop.Click += new System.EventHandler(this.btnDevice3Stop_Click);
            // 
            // btnDevice2Stop
            // 
            this.btnDevice2Stop.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDevice2Stop.Location = new System.Drawing.Point(131, 144);
            this.btnDevice2Stop.Margin = new System.Windows.Forms.Padding(4);
            this.btnDevice2Stop.Name = "btnDevice2Stop";
            this.btnDevice2Stop.Size = new System.Drawing.Size(62, 35);
            this.btnDevice2Stop.TabIndex = 175;
            this.btnDevice2Stop.Text = "Stop";
            this.btnDevice2Stop.UseCompatibleTextRendering = true;
            this.btnDevice2Stop.UseVisualStyleBackColor = true;
            this.btnDevice2Stop.Click += new System.EventHandler(this.btnDevice2Stop_Click);
            // 
            // btnDevice1Stop
            // 
            this.btnDevice1Stop.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDevice1Stop.Location = new System.Drawing.Point(131, 103);
            this.btnDevice1Stop.Margin = new System.Windows.Forms.Padding(4);
            this.btnDevice1Stop.Name = "btnDevice1Stop";
            this.btnDevice1Stop.Size = new System.Drawing.Size(62, 35);
            this.btnDevice1Stop.TabIndex = 174;
            this.btnDevice1Stop.Text = "Stop";
            this.btnDevice1Stop.UseCompatibleTextRendering = true;
            this.btnDevice1Stop.UseVisualStyleBackColor = true;
            this.btnDevice1Stop.Click += new System.EventHandler(this.btnDevice1Stop_Click);
            // 
            // lblDevice5
            // 
            this.lblDevice5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblDevice5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDevice5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDevice5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDevice5.Location = new System.Drawing.Point(4, 265);
            this.lblDevice5.Name = "lblDevice5";
            this.lblDevice5.Size = new System.Drawing.Size(120, 35);
            this.lblDevice5.TabIndex = 166;
            this.lblDevice5.Text = "DEVICE 5";
            this.lblDevice5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDevice5.Click += new System.EventHandler(this.lblDevice5_Click);
            // 
            // lblDevice4
            // 
            this.lblDevice4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblDevice4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDevice4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDevice4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDevice4.Location = new System.Drawing.Point(4, 226);
            this.lblDevice4.Name = "lblDevice4";
            this.lblDevice4.Size = new System.Drawing.Size(120, 35);
            this.lblDevice4.TabIndex = 165;
            this.lblDevice4.Text = "DEVICE 4";
            this.lblDevice4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDevice4.Click += new System.EventHandler(this.lblDevice4_Click);
            // 
            // lblDevice3
            // 
            this.lblDevice3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblDevice3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDevice3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDevice3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDevice3.Location = new System.Drawing.Point(4, 184);
            this.lblDevice3.Name = "lblDevice3";
            this.lblDevice3.Size = new System.Drawing.Size(120, 35);
            this.lblDevice3.TabIndex = 164;
            this.lblDevice3.Text = "DEVICE 3";
            this.lblDevice3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDevice3.Click += new System.EventHandler(this.lblDevice3_Click);
            // 
            // lblDevice2
            // 
            this.lblDevice2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblDevice2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDevice2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDevice2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDevice2.Location = new System.Drawing.Point(4, 144);
            this.lblDevice2.Name = "lblDevice2";
            this.lblDevice2.Size = new System.Drawing.Size(120, 35);
            this.lblDevice2.TabIndex = 163;
            this.lblDevice2.Text = "DEVICE 2";
            this.lblDevice2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDevice2.Click += new System.EventHandler(this.lblDevice2_Click);
            // 
            // lblDevice1
            // 
            this.lblDevice1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblDevice1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDevice1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDevice1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDevice1.Location = new System.Drawing.Point(4, 103);
            this.lblDevice1.Name = "lblDevice1";
            this.lblDevice1.Size = new System.Drawing.Size(120, 35);
            this.lblDevice1.TabIndex = 162;
            this.lblDevice1.Text = "DEVICE 1";
            this.lblDevice1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDevice1.Click += new System.EventHandler(this.lblDevice1_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(4, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 48);
            this.label2.TabIndex = 161;
            this.label2.Text = "Device";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtDev5ActualValue
            // 
            this.txtDev5ActualValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDev5ActualValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtDev5ActualValue.Location = new System.Drawing.Point(279, 265);
            this.txtDev5ActualValue.Name = "txtDev5ActualValue";
            this.txtDev5ActualValue.ReadOnly = true;
            this.txtDev5ActualValue.Size = new System.Drawing.Size(75, 35);
            this.txtDev5ActualValue.TabIndex = 160;
            this.txtDev5ActualValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDev4ActualValue
            // 
            this.txtDev4ActualValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDev4ActualValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtDev4ActualValue.Location = new System.Drawing.Point(279, 226);
            this.txtDev4ActualValue.Name = "txtDev4ActualValue";
            this.txtDev4ActualValue.ReadOnly = true;
            this.txtDev4ActualValue.Size = new System.Drawing.Size(75, 35);
            this.txtDev4ActualValue.TabIndex = 159;
            this.txtDev4ActualValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDev3ActualValue
            // 
            this.txtDev3ActualValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDev3ActualValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtDev3ActualValue.Location = new System.Drawing.Point(279, 185);
            this.txtDev3ActualValue.Name = "txtDev3ActualValue";
            this.txtDev3ActualValue.ReadOnly = true;
            this.txtDev3ActualValue.Size = new System.Drawing.Size(75, 35);
            this.txtDev3ActualValue.TabIndex = 158;
            this.txtDev3ActualValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDev5AlarmValue
            // 
            this.txtDev5AlarmValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDev5AlarmValue.ForeColor = System.Drawing.Color.Red;
            this.txtDev5AlarmValue.Location = new System.Drawing.Point(199, 265);
            this.txtDev5AlarmValue.Name = "txtDev5AlarmValue";
            this.txtDev5AlarmValue.Size = new System.Drawing.Size(74, 35);
            this.txtDev5AlarmValue.TabIndex = 157;
            this.txtDev5AlarmValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDev4AlarmValue
            // 
            this.txtDev4AlarmValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDev4AlarmValue.ForeColor = System.Drawing.Color.Red;
            this.txtDev4AlarmValue.Location = new System.Drawing.Point(199, 226);
            this.txtDev4AlarmValue.Name = "txtDev4AlarmValue";
            this.txtDev4AlarmValue.Size = new System.Drawing.Size(74, 35);
            this.txtDev4AlarmValue.TabIndex = 156;
            this.txtDev4AlarmValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDev3AlarmValue
            // 
            this.txtDev3AlarmValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDev3AlarmValue.ForeColor = System.Drawing.Color.Red;
            this.txtDev3AlarmValue.Location = new System.Drawing.Point(199, 185);
            this.txtDev3AlarmValue.Name = "txtDev3AlarmValue";
            this.txtDev3AlarmValue.Size = new System.Drawing.Size(74, 35);
            this.txtDev3AlarmValue.TabIndex = 155;
            this.txtDev3AlarmValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDev1AlarmValue
            // 
            this.txtDev1AlarmValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDev1AlarmValue.ForeColor = System.Drawing.Color.Red;
            this.txtDev1AlarmValue.Location = new System.Drawing.Point(199, 103);
            this.txtDev1AlarmValue.Name = "txtDev1AlarmValue";
            this.txtDev1AlarmValue.Size = new System.Drawing.Size(74, 35);
            this.txtDev1AlarmValue.TabIndex = 148;
            this.txtDev1AlarmValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDev1ActualValue
            // 
            this.txtDev1ActualValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDev1ActualValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtDev1ActualValue.Location = new System.Drawing.Point(279, 103);
            this.txtDev1ActualValue.Name = "txtDev1ActualValue";
            this.txtDev1ActualValue.ReadOnly = true;
            this.txtDev1ActualValue.Size = new System.Drawing.Size(75, 35);
            this.txtDev1ActualValue.TabIndex = 149;
            this.txtDev1ActualValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDev2AlarmValue
            // 
            this.txtDev2AlarmValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDev2AlarmValue.ForeColor = System.Drawing.Color.Red;
            this.txtDev2AlarmValue.Location = new System.Drawing.Point(199, 144);
            this.txtDev2AlarmValue.Name = "txtDev2AlarmValue";
            this.txtDev2AlarmValue.Size = new System.Drawing.Size(74, 35);
            this.txtDev2AlarmValue.TabIndex = 150;
            this.txtDev2AlarmValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDev2ActualValue
            // 
            this.txtDev2ActualValue.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDev2ActualValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtDev2ActualValue.Location = new System.Drawing.Point(279, 144);
            this.txtDev2ActualValue.Name = "txtDev2ActualValue";
            this.txtDev2ActualValue.ReadOnly = true;
            this.txtDev2ActualValue.Size = new System.Drawing.Size(75, 35);
            this.txtDev2ActualValue.TabIndex = 151;
            this.txtDev2ActualValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::BaseCommon.Properties.Resources.SystechLogo;
            this.pictureBox1.Location = new System.Drawing.Point(94, 571);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(170, 97);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 141;
            this.pictureBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(279, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 48);
            this.label7.TabIndex = 139;
            this.label7.Text = "Actual Value";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(199, 50);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 48);
            this.label6.TabIndex = 138;
            this.label6.Text = "Alarm Setup";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnManagement
            // 
            this.btnManagement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManagement.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManagement.Location = new System.Drawing.Point(11, 487);
            this.btnManagement.Margin = new System.Windows.Forms.Padding(4);
            this.btnManagement.Name = "btnManagement";
            this.btnManagement.Size = new System.Drawing.Size(107, 41);
            this.btnManagement.TabIndex = 139;
            this.btnManagement.Text = "Management";
            this.btnManagement.UseVisualStyleBackColor = true;
            // 
            // btnReport
            // 
            this.btnReport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReport.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnReport.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReport.Location = new System.Drawing.Point(126, 487);
            this.btnReport.Margin = new System.Windows.Forms.Padding(4);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(107, 41);
            this.btnReport.TabIndex = 140;
            this.btnReport.Text = "Report";
            this.btnReport.UseVisualStyleBackColor = true;
            // 
            // dgvMeasure
            // 
            this.dgvMeasure.AllowUserToAddRows = false;
            this.dgvMeasure.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvMeasure.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMeasure.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.TimeStamp,
            this.Value});
            this.dgvMeasure.Location = new System.Drawing.Point(7, 566);
            this.dgvMeasure.Name = "dgvMeasure";
            this.dgvMeasure.RowHeadersVisible = false;
            this.dgvMeasure.Size = new System.Drawing.Size(347, 99);
            this.dgvMeasure.TabIndex = 138;
            this.dgvMeasure.Visible = false;
            // 
            // No
            // 
            this.No.HeaderText = "No";
            this.No.Name = "No";
            this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.No.Width = 30;
            // 
            // TimeStamp
            // 
            this.TimeStamp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TimeStamp.HeaderText = "TimeStamp";
            this.TimeStamp.Name = "TimeStamp";
            this.TimeStamp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Value.Width = 120;
            // 
            // btnWalkingDevice
            // 
            this.btnWalkingDevice.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWalkingDevice.Location = new System.Drawing.Point(4, 357);
            this.btnWalkingDevice.Margin = new System.Windows.Forms.Padding(4);
            this.btnWalkingDevice.Name = "btnWalkingDevice";
            this.btnWalkingDevice.Size = new System.Drawing.Size(189, 39);
            this.btnWalkingDevice.TabIndex = 136;
            this.btnWalkingDevice.Text = "Device";
            this.btnWalkingDevice.UseCompatibleTextRendering = true;
            this.btnWalkingDevice.UseVisualStyleBackColor = true;
            this.btnWalkingDevice.Click += new System.EventHandler(this.btnWalkingStart_Click);
            // 
            // txtCount
            // 
            this.txtCount.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtCount.Location = new System.Drawing.Point(239, 533);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(114, 29);
            this.txtCount.TabIndex = 133;
            this.txtCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCount.Visible = false;
            // 
            // txtFailLevel
            // 
            this.txtFailLevel.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFailLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtFailLevel.Location = new System.Drawing.Point(279, 357);
            this.txtFailLevel.Name = "txtFailLevel";
            this.txtFailLevel.Size = new System.Drawing.Size(74, 29);
            this.txtFailLevel.TabIndex = 132;
            this.txtFailLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtAverage
            // 
            this.txtAverage.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAverage.ForeColor = System.Drawing.Color.Red;
            this.txtAverage.Location = new System.Drawing.Point(279, 427);
            this.txtAverage.Name = "txtAverage";
            this.txtAverage.ReadOnly = true;
            this.txtAverage.Size = new System.Drawing.Size(75, 29);
            this.txtAverage.TabIndex = 131;
            this.txtAverage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtPeriod
            // 
            this.txtPeriod.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPeriod.ForeColor = System.Drawing.Color.Red;
            this.txtPeriod.Location = new System.Drawing.Point(279, 392);
            this.txtPeriod.Name = "txtPeriod";
            this.txtPeriod.Size = new System.Drawing.Size(75, 29);
            this.txtPeriod.TabIndex = 131;
            this.txtPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(198, 427);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 29);
            this.label5.TabIndex = 129;
            this.label5.Text = "Average";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.White;
            this.label10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(186, 357);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(87, 29);
            this.label10.TabIndex = 130;
            this.label10.Text = "Fail Level";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.White;
            this.label11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label11.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(192, 392);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(84, 29);
            this.label11.TabIndex = 129;
            this.label11.Text = "Period (s)";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRstWalking
            // 
            this.lblRstWalking.BackColor = System.Drawing.Color.Lime;
            this.lblRstWalking.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRstWalking.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRstWalking.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRstWalking.ForeColor = System.Drawing.Color.Blue;
            this.lblRstWalking.Location = new System.Drawing.Point(199, 305);
            this.lblRstWalking.Name = "lblRstWalking";
            this.lblRstWalking.Size = new System.Drawing.Size(155, 41);
            this.lblRstWalking.TabIndex = 128;
            this.lblRstWalking.Text = "PASS";
            this.lblRstWalking.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblWalkingTest
            // 
            this.lblWalkingTest.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblWalkingTest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblWalkingTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblWalkingTest.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWalkingTest.Location = new System.Drawing.Point(4, 305);
            this.lblWalkingTest.Name = "lblWalkingTest";
            this.lblWalkingTest.Size = new System.Drawing.Size(189, 41);
            this.lblWalkingTest.TabIndex = 127;
            this.lblWalkingTest.Text = "WALKING TEST";
            this.lblWalkingTest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAlarmTest
            // 
            this.lblAlarmTest.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblAlarmTest.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAlarmTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblAlarmTest.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlarmTest.Location = new System.Drawing.Point(4, 4);
            this.lblAlarmTest.Name = "lblAlarmTest";
            this.lblAlarmTest.Size = new System.Drawing.Size(189, 41);
            this.lblAlarmTest.TabIndex = 116;
            this.lblAlarmTest.Text = "ALARM TEST";
            this.lblAlarmTest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblReset
            // 
            this.lblReset.BackColor = System.Drawing.Color.Lime;
            this.lblReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblReset.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReset.ForeColor = System.Drawing.Color.Blue;
            this.lblReset.Location = new System.Drawing.Point(199, 3);
            this.lblReset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReset.Name = "lblReset";
            this.lblReset.Size = new System.Drawing.Size(155, 41);
            this.lblReset.TabIndex = 115;
            this.lblReset.Text = "RESET";
            this.lblReset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblReset.Click += new System.EventHandler(this.lblReset_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 364F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 729F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1055, 674);
            this.tableLayoutPanel1.TabIndex = 115;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.panel2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.pnlBottomTool, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel4, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 51F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 103F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(685, 668);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.lblTestDeviceType);
            this.panel2.Controls.Add(this.lblOverViewValue);
            this.panel2.Controls.Add(this.lblDeviceType);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(679, 45);
            this.panel2.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(401, 2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 41);
            this.label4.TabIndex = 54;
            this.label4.Text = "Max Value (V)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTestDeviceType
            // 
            this.lblTestDeviceType.BackColor = System.Drawing.Color.Transparent;
            this.lblTestDeviceType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTestDeviceType.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTestDeviceType.Location = new System.Drawing.Point(6, 2);
            this.lblTestDeviceType.Name = "lblTestDeviceType";
            this.lblTestDeviceType.Size = new System.Drawing.Size(145, 41);
            this.lblTestDeviceType.TabIndex = 54;
            this.lblTestDeviceType.Text = "Test Device Type";
            this.lblTestDeviceType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOverViewValue
            // 
            this.lblOverViewValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblOverViewValue.BackColor = System.Drawing.Color.Lime;
            this.lblOverViewValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOverViewValue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblOverViewValue.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOverViewValue.ForeColor = System.Drawing.Color.Blue;
            this.lblOverViewValue.Location = new System.Drawing.Point(535, 2);
            this.lblOverViewValue.Name = "lblOverViewValue";
            this.lblOverViewValue.Size = new System.Drawing.Size(144, 40);
            this.lblOverViewValue.TabIndex = 53;
            this.lblOverViewValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDeviceType
            // 
            this.lblDeviceType.BackColor = System.Drawing.Color.Lime;
            this.lblDeviceType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDeviceType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDeviceType.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeviceType.ForeColor = System.Drawing.Color.Blue;
            this.lblDeviceType.Location = new System.Drawing.Point(157, 2);
            this.lblDeviceType.Name = "lblDeviceType";
            this.lblDeviceType.Size = new System.Drawing.Size(144, 40);
            this.lblDeviceType.TabIndex = 52;
            this.lblDeviceType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlBottomTool
            // 
            this.pnlBottomTool.BackColor = System.Drawing.Color.White;
            this.pnlBottomTool.Controls.Add(this.btn600s);
            this.pnlBottomTool.Controls.Add(this.btn480s);
            this.pnlBottomTool.Controls.Add(this.btn360s);
            this.pnlBottomTool.Controls.Add(this.btn180s);
            this.pnlBottomTool.Controls.Add(this.btnAddPoint);
            this.pnlBottomTool.Controls.Add(this.lblXAxis);
            this.pnlBottomTool.Controls.Add(this.btn20000V);
            this.pnlBottomTool.Controls.Add(this.label1);
            this.pnlBottomTool.Controls.Add(this.btn15000V);
            this.pnlBottomTool.Controls.Add(this.txtTimeTest);
            this.pnlBottomTool.Controls.Add(this.btn10000V);
            this.pnlBottomTool.Controls.Add(this.btn5000V);
            this.pnlBottomTool.Controls.Add(this.btn90s);
            this.pnlBottomTool.Controls.Add(this.btn2000V);
            this.pnlBottomTool.Controls.Add(this.btn120s);
            this.pnlBottomTool.Controls.Add(this.btn1000V);
            this.pnlBottomTool.Controls.Add(this.btn500V);
            this.pnlBottomTool.Controls.Add(this.btn60s);
            this.pnlBottomTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottomTool.Location = new System.Drawing.Point(3, 568);
            this.pnlBottomTool.Name = "pnlBottomTool";
            this.pnlBottomTool.Size = new System.Drawing.Size(679, 97);
            this.pnlBottomTool.TabIndex = 1;
            // 
            // btn600s
            // 
            this.btn600s.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn600s.Location = new System.Drawing.Point(539, 4);
            this.btn600s.Margin = new System.Windows.Forms.Padding(4);
            this.btn600s.Name = "btn600s";
            this.btn600s.Size = new System.Drawing.Size(64, 40);
            this.btn600s.TabIndex = 98;
            this.btn600s.Text = "600s";
            this.btn600s.UseVisualStyleBackColor = true;
            // 
            // btn480s
            // 
            this.btn480s.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn480s.Location = new System.Drawing.Point(467, 4);
            this.btn480s.Margin = new System.Windows.Forms.Padding(4);
            this.btn480s.Name = "btn480s";
            this.btn480s.Size = new System.Drawing.Size(64, 40);
            this.btn480s.TabIndex = 98;
            this.btn480s.Text = "480s";
            this.btn480s.UseVisualStyleBackColor = true;
            // 
            // btn360s
            // 
            this.btn360s.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn360s.Location = new System.Drawing.Point(395, 4);
            this.btn360s.Margin = new System.Windows.Forms.Padding(4);
            this.btn360s.Name = "btn360s";
            this.btn360s.Size = new System.Drawing.Size(64, 40);
            this.btn360s.TabIndex = 98;
            this.btn360s.Text = "360s";
            this.btn360s.UseVisualStyleBackColor = true;
            // 
            // btn180s
            // 
            this.btn180s.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn180s.Location = new System.Drawing.Point(323, 4);
            this.btn180s.Margin = new System.Windows.Forms.Padding(4);
            this.btn180s.Name = "btn180s";
            this.btn180s.Size = new System.Drawing.Size(64, 40);
            this.btn180s.TabIndex = 98;
            this.btn180s.Text = "180s";
            this.btn180s.UseVisualStyleBackColor = true;
            // 
            // btnAddPoint
            // 
            this.btnAddPoint.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddPoint.Location = new System.Drawing.Point(609, 3);
            this.btnAddPoint.Margin = new System.Windows.Forms.Padding(4);
            this.btnAddPoint.Name = "btnAddPoint";
            this.btnAddPoint.Size = new System.Drawing.Size(49, 34);
            this.btnAddPoint.TabIndex = 106;
            this.btnAddPoint.Text = "Add Point";
            this.btnAddPoint.UseVisualStyleBackColor = true;
            this.btnAddPoint.Visible = false;
            this.btnAddPoint.Click += new System.EventHandler(this.btnAddPoint_Click);
            // 
            // lblXAxis
            // 
            this.lblXAxis.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblXAxis.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblXAxis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblXAxis.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXAxis.Location = new System.Drawing.Point(1, 4);
            this.lblXAxis.Name = "lblXAxis";
            this.lblXAxis.Size = new System.Drawing.Size(99, 40);
            this.lblXAxis.TabIndex = 92;
            this.lblXAxis.Text = "X AXIS";
            this.lblXAxis.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn20000V
            // 
            this.btn20000V.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn20000V.Location = new System.Drawing.Point(539, 53);
            this.btn20000V.Margin = new System.Windows.Forms.Padding(4);
            this.btn20000V.Name = "btn20000V";
            this.btn20000V.Size = new System.Drawing.Size(64, 40);
            this.btn20000V.TabIndex = 105;
            this.btn20000V.Text = "20 kV";
            this.btn20000V.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 40);
            this.label1.TabIndex = 93;
            this.label1.Text = "Y AXIS";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn15000V
            // 
            this.btn15000V.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn15000V.Location = new System.Drawing.Point(467, 54);
            this.btn15000V.Margin = new System.Windows.Forms.Padding(4);
            this.btn15000V.Name = "btn15000V";
            this.btn15000V.Size = new System.Drawing.Size(64, 40);
            this.btn15000V.TabIndex = 104;
            this.btn15000V.Text = "15 kV";
            this.btn15000V.UseVisualStyleBackColor = true;
            // 
            // txtTimeTest
            // 
            this.txtTimeTest.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTimeTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtTimeTest.Location = new System.Drawing.Point(609, 44);
            this.txtTimeTest.Name = "txtTimeTest";
            this.txtTimeTest.Size = new System.Drawing.Size(49, 29);
            this.txtTimeTest.TabIndex = 133;
            this.txtTimeTest.Text = "10";
            this.txtTimeTest.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTimeTest.Visible = false;
            // 
            // btn10000V
            // 
            this.btn10000V.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn10000V.Location = new System.Drawing.Point(395, 53);
            this.btn10000V.Margin = new System.Windows.Forms.Padding(4);
            this.btn10000V.Name = "btn10000V";
            this.btn10000V.Size = new System.Drawing.Size(64, 40);
            this.btn10000V.TabIndex = 103;
            this.btn10000V.Text = "10 kV";
            this.btn10000V.UseVisualStyleBackColor = true;
            // 
            // btn5000V
            // 
            this.btn5000V.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn5000V.Location = new System.Drawing.Point(323, 53);
            this.btn5000V.Margin = new System.Windows.Forms.Padding(4);
            this.btn5000V.Name = "btn5000V";
            this.btn5000V.Size = new System.Drawing.Size(64, 40);
            this.btn5000V.TabIndex = 102;
            this.btn5000V.Text = "5 kV";
            this.btn5000V.UseVisualStyleBackColor = true;
            // 
            // btn90s
            // 
            this.btn90s.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn90s.Location = new System.Drawing.Point(179, 4);
            this.btn90s.Margin = new System.Windows.Forms.Padding(4);
            this.btn90s.Name = "btn90s";
            this.btn90s.Size = new System.Drawing.Size(64, 40);
            this.btn90s.TabIndex = 96;
            this.btn90s.Text = "90s";
            this.btn90s.UseVisualStyleBackColor = true;
            // 
            // btn2000V
            // 
            this.btn2000V.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2000V.Location = new System.Drawing.Point(251, 53);
            this.btn2000V.Margin = new System.Windows.Forms.Padding(4);
            this.btn2000V.Name = "btn2000V";
            this.btn2000V.Size = new System.Drawing.Size(64, 40);
            this.btn2000V.TabIndex = 101;
            this.btn2000V.Text = "2 kV";
            this.btn2000V.UseVisualStyleBackColor = true;
            // 
            // btn120s
            // 
            this.btn120s.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn120s.Location = new System.Drawing.Point(251, 4);
            this.btn120s.Margin = new System.Windows.Forms.Padding(4);
            this.btn120s.Name = "btn120s";
            this.btn120s.Size = new System.Drawing.Size(64, 40);
            this.btn120s.TabIndex = 97;
            this.btn120s.Text = "120s";
            this.btn120s.UseVisualStyleBackColor = true;
            // 
            // btn1000V
            // 
            this.btn1000V.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn1000V.Location = new System.Drawing.Point(178, 53);
            this.btn1000V.Margin = new System.Windows.Forms.Padding(4);
            this.btn1000V.Name = "btn1000V";
            this.btn1000V.Size = new System.Drawing.Size(64, 40);
            this.btn1000V.TabIndex = 100;
            this.btn1000V.Text = "1 kV";
            this.btn1000V.UseVisualStyleBackColor = true;
            // 
            // btn500V
            // 
            this.btn500V.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn500V.Location = new System.Drawing.Point(107, 53);
            this.btn500V.Margin = new System.Windows.Forms.Padding(4);
            this.btn500V.Name = "btn500V";
            this.btn500V.Size = new System.Drawing.Size(64, 40);
            this.btn500V.TabIndex = 99;
            this.btn500V.Text = "500V";
            this.btn500V.UseVisualStyleBackColor = true;
            // 
            // btn60s
            // 
            this.btn60s.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn60s.Location = new System.Drawing.Point(107, 4);
            this.btn60s.Margin = new System.Windows.Forms.Padding(4);
            this.btn60s.Name = "btn60s";
            this.btn60s.Size = new System.Drawing.Size(64, 40);
            this.btn60s.TabIndex = 95;
            this.btn60s.Text = "60s";
            this.btn60s.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.pdeGraph);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 54);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(679, 508);
            this.panel4.TabIndex = 2;
            // 
            // pdeGraph
            // 
            this.pdeGraph.BackColor = System.Drawing.Color.Transparent;
            this.pdeGraph.BackgroundColorBot = System.Drawing.Color.White;
            this.pdeGraph.BackgroundColorTop = System.Drawing.Color.White;
            this.pdeGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pdeGraph.DashedGridColor = System.Drawing.Color.DarkGray;
            this.pdeGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdeGraph.DoubleBuffering = true;
            this.pdeGraph.drawAlarmLine = false;
            this.pdeGraph.drawMaxPoint = false;
            this.pdeGraph.Location = new System.Drawing.Point(0, 0);
            this.pdeGraph.lockMouseMove = false;
            this.pdeGraph.Margin = new System.Windows.Forms.Padding(153, 104, 153, 104);
            this.pdeGraph.Name = "pdeGraph";
            this.pdeGraph.PlaySpeed = 0.5F;
            this.pdeGraph.Size = new System.Drawing.Size(679, 508);
            this.pdeGraph.SolidGridColor = System.Drawing.Color.DarkGray;
            this.pdeGraph.starting_idx = 0;
            this.pdeGraph.StartingIndexOff = 0;
            this.pdeGraph.TabIndex = 35;
            // 
            // timerWalkingTest
            // 
            this.timerWalkingTest.Interval = 1000;
            // 
            // GraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1055, 674);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GraphForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "STATIRON DSF601 v2 Control Software";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GraphForm_FormClosing);
            this.Load += new System.EventHandler(this.GraphForm_Load);
            this.Resize += new System.EventHandler(this.GraphForm_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMeasure)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlBottomTool.ResumeLayout(false);
            this.pnlBottomTool.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnManagement;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.DataGridView dgvMeasure;
        private System.Windows.Forms.Button btnWalkingDevice;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.TextBox txtFailLevel;
        private System.Windows.Forms.TextBox txtPeriod;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblRstWalking;
        private System.Windows.Forms.Label lblWalkingTest;
        private System.Windows.Forms.Label lblAlarmTest;
        private System.Windows.Forms.Label lblReset;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTestDeviceType;
        private System.Windows.Forms.Label lblOverViewValue;
        private System.Windows.Forms.Label lblDeviceType;
        private System.Windows.Forms.Panel pnlBottomTool;
        private System.Windows.Forms.Button btn180s;
        private System.Windows.Forms.Label lblXAxis;
        private System.Windows.Forms.Button btn20000V;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn15000V;
        private System.Windows.Forms.Button btn10000V;
        private System.Windows.Forms.Button btn60s;
        private System.Windows.Forms.Button btn5000V;
        private System.Windows.Forms.Button btn90s;
        private System.Windows.Forms.Button btn2000V;
        private System.Windows.Forms.Button btn120s;
        private System.Windows.Forms.Button btn1000V;
        private System.Windows.Forms.Button btn500V;
        private System.Windows.Forms.Panel panel4;
        private GraphLib.PlotterDisplayEx pdeGraph;
        private System.Windows.Forms.Timer timerWalkingTest;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTimeTest;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeStamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtAverage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDev5ActualValue;
        private System.Windows.Forms.TextBox txtDev4ActualValue;
        private System.Windows.Forms.TextBox txtDev3ActualValue;
        private System.Windows.Forms.TextBox txtDev5AlarmValue;
        private System.Windows.Forms.TextBox txtDev4AlarmValue;
        private System.Windows.Forms.TextBox txtDev3AlarmValue;
        private System.Windows.Forms.TextBox txtDev1AlarmValue;
        private System.Windows.Forms.TextBox txtDev1ActualValue;
        private System.Windows.Forms.TextBox txtDev2AlarmValue;
        private System.Windows.Forms.TextBox txtDev2ActualValue;
        private System.Windows.Forms.Label lblDevice5;
        private System.Windows.Forms.Label lblDevice4;
        private System.Windows.Forms.Label lblDevice3;
        private System.Windows.Forms.Label lblDevice2;
        private System.Windows.Forms.Label lblDevice1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAddPoint;
        private System.Windows.Forms.Button btnDevice5Stop;
        private System.Windows.Forms.Button btnDevice4Stop;
        private System.Windows.Forms.Button btnDevice3Stop;
        private System.Windows.Forms.Button btnDevice2Stop;
        private System.Windows.Forms.Button btnDevice1Stop;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btn360s;
        private System.Windows.Forms.Button btn480s;
        private System.Windows.Forms.Button btn600s;
    }
}