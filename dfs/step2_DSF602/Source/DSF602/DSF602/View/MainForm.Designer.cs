using DSF602.Language;

namespace DSF602.View
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.plnTop = new System.Windows.Forms.Panel();
            this.btnAbout = new BaseCommon.ControlTemplate.btn();
            this.lblBlockName = new System.Windows.Forms.Label();
            this.btnReset = new BaseCommon.ControlTemplate.btn();
            this.btnUpdate = new BaseCommon.ControlTemplate.btn();
            this.btnLanguage = new BaseCommon.ControlTemplate.btn();
            this.btnUser = new BaseCommon.ControlTemplate.btn();
            this.btnData = new BaseCommon.ControlTemplate.btn();
            this.btnDevice = new BaseCommon.ControlTemplate.btn();
            this.GraphLayout = new System.Windows.Forms.TableLayoutPanel();
            this.plnRight = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picLayout3 = new System.Windows.Forms.PictureBox();
            this.picLayout2 = new System.Windows.Forms.PictureBox();
            this.picLayout1 = new System.Windows.Forms.PictureBox();
            this.lblView = new System.Windows.Forms.Label();
            this.plnLocation = new System.Windows.Forms.Panel();
            this.btn9 = new BaseCommon.ControlTemplate.btn();
            this.btn14 = new BaseCommon.ControlTemplate.btn();
            this.btn8 = new BaseCommon.ControlTemplate.btn();
            this.btn7 = new BaseCommon.ControlTemplate.btn();
            this.btn6 = new BaseCommon.ControlTemplate.btn();
            this.btn5 = new BaseCommon.ControlTemplate.btn();
            this.btn4 = new BaseCommon.ControlTemplate.btn();
            this.btn3 = new BaseCommon.ControlTemplate.btn();
            this.btn2 = new BaseCommon.ControlTemplate.btn();
            this.btn1 = new BaseCommon.ControlTemplate.btn();
            this.lblLocation = new System.Windows.Forms.Label();
            this.tabGraphMain = new System.Windows.Forms.TabControl();
            this.tabGraph = new System.Windows.Forms.TabPage();
            this.tablMap = new System.Windows.Forms.TabPage();
            this.tabErr = new System.Windows.Forms.TabPage();
            this.txtErrors = new System.Windows.Forms.TextBox();
            this.tabAlarmReport = new System.Windows.Forms.TabPage();
            this.timerUpdateGraph = new System.Windows.Forms.Timer(this.components);
            this.timerCheckAlarm = new System.Windows.Forms.Timer(this.components);
            this.MainLayout.SuspendLayout();
            this.plnTop.SuspendLayout();
            this.GraphLayout.SuspendLayout();
            this.plnRight.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLayout3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLayout2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLayout1)).BeginInit();
            this.plnLocation.SuspendLayout();
            this.tabGraphMain.SuspendLayout();
            this.tabErr.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 1;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainLayout.Controls.Add(this.plnTop, 0, 0);
            this.MainLayout.Controls.Add(this.GraphLayout, 0, 1);
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 0);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 2;
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainLayout.Size = new System.Drawing.Size(1017, 597);
            this.MainLayout.TabIndex = 0;
            // 
            // plnTop
            // 
            this.plnTop.Controls.Add(this.btnAbout);
            this.plnTop.Controls.Add(this.lblBlockName);
            this.plnTop.Controls.Add(this.btnReset);
            this.plnTop.Controls.Add(this.btnUpdate);
            this.plnTop.Controls.Add(this.btnLanguage);
            this.plnTop.Controls.Add(this.btnUser);
            this.plnTop.Controls.Add(this.btnData);
            this.plnTop.Controls.Add(this.btnDevice);
            this.plnTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plnTop.Location = new System.Drawing.Point(3, 3);
            this.plnTop.Name = "plnTop";
            this.plnTop.Size = new System.Drawing.Size(1011, 36);
            this.plnTop.TabIndex = 0;
            // 
            // btnAbout
            // 
            this.btnAbout.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.Location = new System.Drawing.Point(691, 3);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Shortcut = System.Windows.Forms.Keys.None;
            this.btnAbout.Size = new System.Drawing.Size(109, 30);
            this.btnAbout.TabIndex = 2;
            this.btnAbout.Text = "About";
            this.btnAbout.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // lblBlockName
            // 
            this.lblBlockName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBlockName.BackColor = System.Drawing.Color.Yellow;
            this.lblBlockName.Location = new System.Drawing.Point(885, 4);
            this.lblBlockName.Name = "lblBlockName";
            this.lblBlockName.Size = new System.Drawing.Size(123, 29);
            this.lblBlockName.TabIndex = 1;
            this.lblBlockName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(576, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Shortcut = System.Windows.Forms.Keys.None;
            this.btnReset.Size = new System.Drawing.Size(109, 30);
            this.btnReset.TabIndex = 0;
            this.btnReset.Text = "Reset";
            this.btnReset.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdate.Location = new System.Drawing.Point(461, 3);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Shortcut = System.Windows.Forms.Keys.None;
            this.btnUpdate.Size = new System.Drawing.Size(109, 30);
            this.btnUpdate.TabIndex = 0;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnLanguage
            // 
            this.btnLanguage.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLanguage.Location = new System.Drawing.Point(346, 3);
            this.btnLanguage.Name = "btnLanguage";
            this.btnLanguage.Shortcut = System.Windows.Forms.Keys.None;
            this.btnLanguage.Size = new System.Drawing.Size(109, 30);
            this.btnLanguage.TabIndex = 0;
            this.btnLanguage.Text = "Language";
            this.btnLanguage.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnLanguage.UseVisualStyleBackColor = true;
            this.btnLanguage.Click += new System.EventHandler(this.btnLanguage_Click);
            // 
            // btnUser
            // 
            this.btnUser.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUser.Location = new System.Drawing.Point(231, 3);
            this.btnUser.Name = "btnUser";
            this.btnUser.Shortcut = System.Windows.Forms.Keys.None;
            this.btnUser.Size = new System.Drawing.Size(109, 30);
            this.btnUser.TabIndex = 0;
            this.btnUser.Text = "User";
            this.btnUser.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnUser.UseVisualStyleBackColor = true;
            this.btnUser.Click += new System.EventHandler(this.btnUser_Click);
            // 
            // btnData
            // 
            this.btnData.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnData.Location = new System.Drawing.Point(116, 3);
            this.btnData.Name = "btnData";
            this.btnData.Shortcut = System.Windows.Forms.Keys.None;
            this.btnData.Size = new System.Drawing.Size(109, 30);
            this.btnData.TabIndex = 0;
            this.btnData.Text = "Data";
            this.btnData.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnData.UseVisualStyleBackColor = true;
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
            // 
            // btnDevice
            // 
            this.btnDevice.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDevice.Location = new System.Drawing.Point(1, 3);
            this.btnDevice.Name = "btnDevice";
            this.btnDevice.Shortcut = System.Windows.Forms.Keys.None;
            this.btnDevice.Size = new System.Drawing.Size(109, 30);
            this.btnDevice.TabIndex = 0;
            this.btnDevice.Text = "Device";
            this.btnDevice.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btnDevice.UseVisualStyleBackColor = true;
            this.btnDevice.Click += new System.EventHandler(this.btnDevice_Click);
            // 
            // GraphLayout
            // 
            this.GraphLayout.ColumnCount = 2;
            this.GraphLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GraphLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.GraphLayout.Controls.Add(this.plnRight, 1, 0);
            this.GraphLayout.Controls.Add(this.tabGraphMain, 0, 0);
            this.GraphLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GraphLayout.Location = new System.Drawing.Point(3, 45);
            this.GraphLayout.Name = "GraphLayout";
            this.GraphLayout.RowCount = 1;
            this.GraphLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GraphLayout.Size = new System.Drawing.Size(1011, 549);
            this.GraphLayout.TabIndex = 1;
            // 
            // plnRight
            // 
            this.plnRight.Controls.Add(this.panel1);
            this.plnRight.Controls.Add(this.plnLocation);
            this.plnRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plnRight.Location = new System.Drawing.Point(885, 3);
            this.plnRight.Name = "plnRight";
            this.plnRight.Size = new System.Drawing.Size(123, 543);
            this.plnRight.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.panel1.Controls.Add(this.picLayout3);
            this.panel1.Controls.Add(this.picLayout2);
            this.panel1.Controls.Add(this.picLayout1);
            this.panel1.Controls.Add(this.lblView);
            this.panel1.Location = new System.Drawing.Point(4, 349);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(114, 191);
            this.panel1.TabIndex = 0;
            // 
            // picLayout3
            // 
            this.picLayout3.Image = ((System.Drawing.Image)(resources.GetObject("picLayout3.Image")));
            this.picLayout3.Location = new System.Drawing.Point(7, 135);
            this.picLayout3.Name = "picLayout3";
            this.picLayout3.Size = new System.Drawing.Size(100, 50);
            this.picLayout3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLayout3.TabIndex = 1;
            this.picLayout3.TabStop = false;
            // 
            // picLayout2
            // 
            this.picLayout2.Image = ((System.Drawing.Image)(resources.GetObject("picLayout2.Image")));
            this.picLayout2.Location = new System.Drawing.Point(7, 79);
            this.picLayout2.Name = "picLayout2";
            this.picLayout2.Size = new System.Drawing.Size(100, 50);
            this.picLayout2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLayout2.TabIndex = 1;
            this.picLayout2.TabStop = false;
            // 
            // picLayout1
            // 
            this.picLayout1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLayout1.Image = ((System.Drawing.Image)(resources.GetObject("picLayout1.Image")));
            this.picLayout1.Location = new System.Drawing.Point(7, 23);
            this.picLayout1.Name = "picLayout1";
            this.picLayout1.Size = new System.Drawing.Size(100, 50);
            this.picLayout1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLayout1.TabIndex = 1;
            this.picLayout1.TabStop = false;
            // 
            // lblView
            // 
            this.lblView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblView.BackColor = System.Drawing.Color.DodgerBlue;
            this.lblView.Font = new System.Drawing.Font("Arial Narrow", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblView.ForeColor = System.Drawing.Color.White;
            this.lblView.Location = new System.Drawing.Point(0, 0);
            this.lblView.Name = "lblView";
            this.lblView.Size = new System.Drawing.Size(114, 20);
            this.lblView.TabIndex = 0;
            this.lblView.Text = "VIEW";
            this.lblView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // plnLocation
            // 
            this.plnLocation.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.plnLocation.Controls.Add(this.btn9);
            this.plnLocation.Controls.Add(this.btn14);
            this.plnLocation.Controls.Add(this.btn8);
            this.plnLocation.Controls.Add(this.btn7);
            this.plnLocation.Controls.Add(this.btn6);
            this.plnLocation.Controls.Add(this.btn5);
            this.plnLocation.Controls.Add(this.btn4);
            this.plnLocation.Controls.Add(this.btn3);
            this.plnLocation.Controls.Add(this.btn2);
            this.plnLocation.Controls.Add(this.btn1);
            this.plnLocation.Controls.Add(this.lblLocation);
            this.plnLocation.Location = new System.Drawing.Point(4, 3);
            this.plnLocation.Name = "plnLocation";
            this.plnLocation.Size = new System.Drawing.Size(114, 339);
            this.plnLocation.TabIndex = 0;
            // 
            // btn9
            // 
            this.btn9.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn9.Location = new System.Drawing.Point(11, 295);
            this.btn9.Name = "btn9";
            this.btn9.Shortcut = System.Windows.Forms.Keys.None;
            this.btn9.Size = new System.Drawing.Size(93, 30);
            this.btn9.TabIndex = 1;
            this.btn9.Text = "Block 10";
            this.btn9.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btn9.UseVisualStyleBackColor = true;
            // 
            // btn14
            // 
            this.btn14.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn14.Location = new System.Drawing.Point(11, 265);
            this.btn14.Name = "btn14";
            this.btn14.Shortcut = System.Windows.Forms.Keys.None;
            this.btn14.Size = new System.Drawing.Size(93, 30);
            this.btn14.TabIndex = 1;
            this.btn14.Text = "Block 9";
            this.btn14.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btn14.UseVisualStyleBackColor = true;
            // 
            // btn8
            // 
            this.btn8.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn8.Location = new System.Drawing.Point(11, 235);
            this.btn8.Name = "btn8";
            this.btn8.Shortcut = System.Windows.Forms.Keys.None;
            this.btn8.Size = new System.Drawing.Size(93, 30);
            this.btn8.TabIndex = 1;
            this.btn8.Text = "Block 8";
            this.btn8.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btn8.UseVisualStyleBackColor = true;
            // 
            // btn7
            // 
            this.btn7.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn7.Location = new System.Drawing.Point(11, 205);
            this.btn7.Name = "btn7";
            this.btn7.Shortcut = System.Windows.Forms.Keys.None;
            this.btn7.Size = new System.Drawing.Size(93, 30);
            this.btn7.TabIndex = 1;
            this.btn7.Text = "Block 7";
            this.btn7.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btn7.UseVisualStyleBackColor = true;
            // 
            // btn6
            // 
            this.btn6.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn6.Location = new System.Drawing.Point(11, 175);
            this.btn6.Name = "btn6";
            this.btn6.Shortcut = System.Windows.Forms.Keys.None;
            this.btn6.Size = new System.Drawing.Size(93, 30);
            this.btn6.TabIndex = 1;
            this.btn6.Text = "Block 6";
            this.btn6.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btn6.UseVisualStyleBackColor = true;
            // 
            // btn5
            // 
            this.btn5.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn5.Location = new System.Drawing.Point(11, 145);
            this.btn5.Name = "btn5";
            this.btn5.Shortcut = System.Windows.Forms.Keys.None;
            this.btn5.Size = new System.Drawing.Size(93, 30);
            this.btn5.TabIndex = 1;
            this.btn5.Text = "Block 5";
            this.btn5.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btn5.UseVisualStyleBackColor = true;
            // 
            // btn4
            // 
            this.btn4.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn4.Location = new System.Drawing.Point(11, 115);
            this.btn4.Name = "btn4";
            this.btn4.Shortcut = System.Windows.Forms.Keys.None;
            this.btn4.Size = new System.Drawing.Size(93, 30);
            this.btn4.TabIndex = 1;
            this.btn4.Text = "Block 4";
            this.btn4.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btn4.UseVisualStyleBackColor = true;
            // 
            // btn3
            // 
            this.btn3.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3.Location = new System.Drawing.Point(11, 85);
            this.btn3.Name = "btn3";
            this.btn3.Shortcut = System.Windows.Forms.Keys.None;
            this.btn3.Size = new System.Drawing.Size(93, 30);
            this.btn3.TabIndex = 1;
            this.btn3.Text = "Block 3";
            this.btn3.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btn3.UseVisualStyleBackColor = true;
            // 
            // btn2
            // 
            this.btn2.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2.Location = new System.Drawing.Point(11, 55);
            this.btn2.Name = "btn2";
            this.btn2.Shortcut = System.Windows.Forms.Keys.None;
            this.btn2.Size = new System.Drawing.Size(93, 30);
            this.btn2.TabIndex = 1;
            this.btn2.Text = "Block 2";
            this.btn2.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btn2.UseVisualStyleBackColor = true;
            // 
            // btn1
            // 
            this.btn1.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn1.Location = new System.Drawing.Point(11, 25);
            this.btn1.Name = "btn1";
            this.btn1.Shortcut = System.Windows.Forms.Keys.None;
            this.btn1.Size = new System.Drawing.Size(93, 30);
            this.btn1.TabIndex = 1;
            this.btn1.Text = "Block 1";
            this.btn1.TypeSize = BaseCommon.ControlTemplate.btn.emSize.Custom;
            this.btn1.UseVisualStyleBackColor = true;
            // 
            // lblLocation
            // 
            this.lblLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLocation.BackColor = System.Drawing.Color.DodgerBlue;
            this.lblLocation.Font = new System.Drawing.Font("Arial Narrow", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.ForeColor = System.Drawing.Color.White;
            this.lblLocation.Location = new System.Drawing.Point(0, 0);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(114, 20);
            this.lblLocation.TabIndex = 0;
            this.lblLocation.Text = "LOCATION";
            this.lblLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabGraphMain
            // 
            this.tabGraphMain.Controls.Add(this.tabGraph);
            this.tabGraphMain.Controls.Add(this.tablMap);
            this.tabGraphMain.Controls.Add(this.tabErr);
            this.tabGraphMain.Controls.Add(this.tabAlarmReport);
            this.tabGraphMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabGraphMain.Location = new System.Drawing.Point(3, 3);
            this.tabGraphMain.Name = "tabGraphMain";
            this.tabGraphMain.SelectedIndex = 0;
            this.tabGraphMain.Size = new System.Drawing.Size(876, 543);
            this.tabGraphMain.TabIndex = 1;
            this.tabGraphMain.SelectedIndexChanged += new System.EventHandler(this.tabGraphMain_SelectedIndexChanged);
            // 
            // tabGraph
            // 
            this.tabGraph.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.tabGraph.Location = new System.Drawing.Point(4, 29);
            this.tabGraph.Name = "tabGraph";
            this.tabGraph.Padding = new System.Windows.Forms.Padding(3);
            this.tabGraph.Size = new System.Drawing.Size(868, 510);
            this.tabGraph.TabIndex = 0;
            this.tabGraph.Text = "GRAPH";
            // 
            // tablMap
            // 
            this.tablMap.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.tablMap.Location = new System.Drawing.Point(4, 26);
            this.tablMap.Name = "tablMap";
            this.tablMap.Padding = new System.Windows.Forms.Padding(3);
            this.tablMap.Size = new System.Drawing.Size(868, 513);
            this.tablMap.TabIndex = 1;
            this.tablMap.Text = "MAP";
            // 
            // tabErr
            // 
            this.tabErr.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.tabErr.Controls.Add(this.txtErrors);
            this.tabErr.Location = new System.Drawing.Point(4, 29);
            this.tabErr.Name = "tabErr";
            this.tabErr.Size = new System.Drawing.Size(868, 510);
            this.tabErr.TabIndex = 2;
            this.tabErr.Text = "ERROR";
            // 
            // txtErrors
            // 
            this.txtErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtErrors.Location = new System.Drawing.Point(0, 0);
            this.txtErrors.Multiline = true;
            this.txtErrors.Name = "txtErrors";
            this.txtErrors.ReadOnly = true;
            this.txtErrors.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtErrors.Size = new System.Drawing.Size(868, 510);
            this.txtErrors.TabIndex = 0;
            // 
            // tabAlarmReport
            // 
            this.tabAlarmReport.Location = new System.Drawing.Point(4, 29);
            this.tabAlarmReport.Name = "tabAlarmReport";
            this.tabAlarmReport.Padding = new System.Windows.Forms.Padding(3);
            this.tabAlarmReport.Size = new System.Drawing.Size(868, 510);
            this.tabAlarmReport.TabIndex = 3;
            this.tabAlarmReport.Text = "Alarm Report";
            this.tabAlarmReport.UseVisualStyleBackColor = true;
            // 
            // timerUpdateGraph
            // 
            this.timerUpdateGraph.Tick += new System.EventHandler(this.timerUpdateGraph_Tick);
            // 
            // timerCheckAlarm
            // 
            this.timerCheckAlarm.Tick += new System.EventHandler(this.timerCheckAlarm_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1017, 597);
            this.Controls.Add(this.MainLayout);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(351, 124);
            this.Name = "MainForm";
            this.Text = "";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainLayout.ResumeLayout(false);
            this.plnTop.ResumeLayout(false);
            this.GraphLayout.ResumeLayout(false);
            this.plnRight.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLayout3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLayout2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLayout1)).EndInit();
            this.plnLocation.ResumeLayout(false);
            this.tabGraphMain.ResumeLayout(false);
            this.tabErr.ResumeLayout(false);
            this.tabErr.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainLayout;
        private System.Windows.Forms.Panel plnTop;
        private BaseCommon.ControlTemplate.btn btnUpdate;
        private BaseCommon.ControlTemplate.btn btnLanguage;
        private BaseCommon.ControlTemplate.btn btnUser;
        private BaseCommon.ControlTemplate.btn btnData;
        private BaseCommon.ControlTemplate.btn btnDevice;
        private System.Windows.Forms.Label lblBlockName;
        private System.Windows.Forms.TableLayoutPanel GraphLayout;
        private System.Windows.Forms.Panel plnRight;
        private System.Windows.Forms.Panel plnLocation;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblView;
        private BaseCommon.ControlTemplate.btn btn9;
        private BaseCommon.ControlTemplate.btn btn14;
        private BaseCommon.ControlTemplate.btn btn8;
        private BaseCommon.ControlTemplate.btn btn7;
        private BaseCommon.ControlTemplate.btn btn6;
        private BaseCommon.ControlTemplate.btn btn5;
        private BaseCommon.ControlTemplate.btn btn4;
        private BaseCommon.ControlTemplate.btn btn3;
        private BaseCommon.ControlTemplate.btn btn2;
        private BaseCommon.ControlTemplate.btn btn1;
        private System.Windows.Forms.TabControl tabGraphMain;
        private System.Windows.Forms.TabPage tabGraph;
        private System.Windows.Forms.TabPage tablMap;
        private System.Windows.Forms.PictureBox picLayout1;
        private System.Windows.Forms.PictureBox picLayout3;
        private System.Windows.Forms.PictureBox picLayout2;
        private BaseCommon.ControlTemplate.btn btnReset;
        private System.Windows.Forms.Timer timerUpdateGraph;
        private System.Windows.Forms.Timer timerCheckAlarm;
        private System.Windows.Forms.TabPage tabErr;
        private System.Windows.Forms.TextBox txtErrors;
        private System.Windows.Forms.TabPage tabAlarmReport;
        private BaseCommon.ControlTemplate.btn btnAbout;
    }
}