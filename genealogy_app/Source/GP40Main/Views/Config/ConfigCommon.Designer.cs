namespace GP40Main.Views.Config
{
    partial class ConfigCommon
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
            this.components = new System.ComponentModel.Container();
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.tabMain = new MaterialSkin.Controls.MaterialTabControl();
            this.tabSystem = new System.Windows.Forms.TabPage();
            this.btnBackup = new MaterialSkin.Controls.MaterialButton();
            this.btnRestoreNew = new MaterialSkin.Controls.MaterialButton();
            this.btnRestoreData = new MaterialSkin.Controls.MaterialButton();
            this.tabNational = new System.Windows.Forms.TabPage();
            this.gridNational = new GP40Main.Themes.Controls.DataGridTemplate(this.components);
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColActionNational = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tabReligion = new System.Windows.Forms.TabPage();
            this.gridReligion = new GP40Main.Themes.Controls.DataGridTemplate(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActionReligion = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tabTypeName = new System.Windows.Forms.TabPage();
            this.gridTypeName = new GP40Main.Themes.Controls.DataGridTemplate(this.components);
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActionTypeName = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tabRelation = new System.Windows.Forms.TabPage();
            this.gridRelation = new GP40Main.Themes.Controls.DataGridTemplate(this.components);
            this.colMainRelation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRelatedRelation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActionRelation = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnAdd = new MaterialSkin.Controls.MaterialButton();
            this.tabMain.SuspendLayout();
            this.tabSystem.SuspendLayout();
            this.tabNational.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridNational)).BeginInit();
            this.tabReligion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReligion)).BeginInit();
            this.tabTypeName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTypeName)).BeginInit();
            this.tabRelation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRelation)).BeginInit();
            this.SuspendLayout();
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTabSelector1.BaseTabControl = this.tabMain;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialTabSelector1.Location = new System.Drawing.Point(4, 6);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(572, 36);
            this.materialTabSelector1.TabIndex = 0;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabSystem);
            this.tabMain.Controls.Add(this.tabNational);
            this.tabMain.Controls.Add(this.tabReligion);
            this.tabMain.Controls.Add(this.tabTypeName);
            this.tabMain.Controls.Add(this.tabRelation);
            this.tabMain.Depth = 0;
            this.tabMain.Location = new System.Drawing.Point(4, 48);
            this.tabMain.MouseState = MaterialSkin.MouseState.HOVER;
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1013, 481);
            this.tabMain.TabIndex = 1;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            // 
            // tabSystem
            // 
            this.tabSystem.BackColor = System.Drawing.Color.White;
            this.tabSystem.Controls.Add(this.btnBackup);
            this.tabSystem.Controls.Add(this.btnRestoreNew);
            this.tabSystem.Controls.Add(this.btnRestoreData);
            this.tabSystem.Location = new System.Drawing.Point(4, 22);
            this.tabSystem.Name = "tabSystem";
            this.tabSystem.Size = new System.Drawing.Size(1005, 455);
            this.tabSystem.TabIndex = 4;
            this.tabSystem.Text = "Hệ thống";
            // 
            // btnBackup
            // 
            this.btnBackup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBackup.BackColor = System.Drawing.Color.White;
            this.btnBackup.Depth = 0;
            this.btnBackup.DrawShadows = true;
            this.btnBackup.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBackup.HighEmphasis = true;
            this.btnBackup.Icon = null;
            this.btnBackup.Location = new System.Drawing.Point(375, 15);
            this.btnBackup.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnBackup.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(139, 36);
            this.btnBackup.TabIndex = 0;
            this.btnBackup.Text = "Sao lưu dữ liệu";
            this.btnBackup.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnBackup.UseAccentColor = true;
            this.btnBackup.UseVisualStyleBackColor = false;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // btnRestoreNew
            // 
            this.btnRestoreNew.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRestoreNew.BackColor = System.Drawing.Color.White;
            this.btnRestoreNew.Depth = 0;
            this.btnRestoreNew.DrawShadows = true;
            this.btnRestoreNew.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRestoreNew.HighEmphasis = true;
            this.btnRestoreNew.Icon = null;
            this.btnRestoreNew.Location = new System.Drawing.Point(180, 15);
            this.btnRestoreNew.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnRestoreNew.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnRestoreNew.Name = "btnRestoreNew";
            this.btnRestoreNew.Size = new System.Drawing.Size(187, 36);
            this.btnRestoreNew.TabIndex = 0;
            this.btnRestoreNew.Text = "Khôi phục dữ liệu mới";
            this.btnRestoreNew.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnRestoreNew.UseAccentColor = true;
            this.btnRestoreNew.UseVisualStyleBackColor = false;
            this.btnRestoreNew.Click += new System.EventHandler(this.btnRestoreNew_Click);
            // 
            // btnRestoreData
            // 
            this.btnRestoreData.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRestoreData.BackColor = System.Drawing.Color.White;
            this.btnRestoreData.Depth = 0;
            this.btnRestoreData.DrawShadows = true;
            this.btnRestoreData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRestoreData.HighEmphasis = true;
            this.btnRestoreData.Icon = null;
            this.btnRestoreData.Location = new System.Drawing.Point(16, 15);
            this.btnRestoreData.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnRestoreData.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnRestoreData.Name = "btnRestoreData";
            this.btnRestoreData.Size = new System.Drawing.Size(156, 36);
            this.btnRestoreData.TabIndex = 0;
            this.btnRestoreData.Text = "Khôi phục dữ liệu";
            this.btnRestoreData.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnRestoreData.UseAccentColor = true;
            this.btnRestoreData.UseVisualStyleBackColor = false;
            this.btnRestoreData.Click += new System.EventHandler(this.btnRestoreData_Click);
            // 
            // tabNational
            // 
            this.tabNational.Controls.Add(this.gridNational);
            this.tabNational.Location = new System.Drawing.Point(4, 22);
            this.tabNational.Name = "tabNational";
            this.tabNational.Padding = new System.Windows.Forms.Padding(3);
            this.tabNational.Size = new System.Drawing.Size(1005, 455);
            this.tabNational.TabIndex = 0;
            this.tabNational.Text = "Quốc gia";
            this.tabNational.UseVisualStyleBackColor = true;
            // 
            // gridNational
            // 
            this.gridNational.AllowUserToAddRows = false;
            this.gridNational.AllowUserToResizeRows = false;
            this.gridNational.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridNational.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridNational.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridNational.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridNational.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridNational.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridNational.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridNational.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.ColActionNational});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridNational.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridNational.EnableHeadersVisualStyles = false;
            this.gridNational.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridNational.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridNational.Location = new System.Drawing.Point(4, 3);
            this.gridNational.Name = "gridNational";
            this.gridNational.ReadOnly = true;
            this.gridNational.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridNational.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridNational.RowHeadersVisible = false;
            this.gridNational.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridNational.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridNational.Size = new System.Drawing.Size(995, 452);
            this.gridNational.TabIndex = 0;
            this.gridNational.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridNational_CellContentClick);
            this.gridNational.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridNational_CellDoubleClick);
            this.gridNational.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridNational_MouseClick);
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "NatName";
            this.colName.HeaderText = "Tên quốc gia";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // ColActionNational
            // 
            this.ColActionNational.HeaderText = "Action";
            this.ColActionNational.Name = "ColActionNational";
            this.ColActionNational.ReadOnly = true;
            this.ColActionNational.Text = "Xóa";
            this.ColActionNational.UseColumnTextForButtonValue = true;
            // 
            // tabReligion
            // 
            this.tabReligion.Controls.Add(this.gridReligion);
            this.tabReligion.Location = new System.Drawing.Point(4, 22);
            this.tabReligion.Name = "tabReligion";
            this.tabReligion.Padding = new System.Windows.Forms.Padding(3);
            this.tabReligion.Size = new System.Drawing.Size(1005, 455);
            this.tabReligion.TabIndex = 1;
            this.tabReligion.Text = "Tôn giáo";
            this.tabReligion.UseVisualStyleBackColor = true;
            // 
            // gridReligion
            // 
            this.gridReligion.AllowUserToAddRows = false;
            this.gridReligion.AllowUserToResizeRows = false;
            this.gridReligion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridReligion.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridReligion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridReligion.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridReligion.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridReligion.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridReligion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridReligion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.colActionReligion});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridReligion.DefaultCellStyle = dataGridViewCellStyle5;
            this.gridReligion.EnableHeadersVisualStyles = false;
            this.gridReligion.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridReligion.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridReligion.Location = new System.Drawing.Point(5, 3);
            this.gridReligion.Name = "gridReligion";
            this.gridReligion.ReadOnly = true;
            this.gridReligion.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridReligion.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.gridReligion.RowHeadersVisible = false;
            this.gridReligion.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridReligion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridReligion.Size = new System.Drawing.Size(995, 449);
            this.gridReligion.TabIndex = 1;
            this.gridReligion.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridReligion_CellContentClick);
            this.gridReligion.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridReligion_CellDoubleClick);
            this.gridReligion.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridReligion_MouseClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "RelName";
            this.dataGridViewTextBoxColumn1.HeaderText = "Tên tôn giáo";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // colActionReligion
            // 
            this.colActionReligion.HeaderText = "Action";
            this.colActionReligion.Name = "colActionReligion";
            this.colActionReligion.ReadOnly = true;
            this.colActionReligion.Text = "Xóa";
            this.colActionReligion.UseColumnTextForButtonValue = true;
            // 
            // tabTypeName
            // 
            this.tabTypeName.Controls.Add(this.gridTypeName);
            this.tabTypeName.Location = new System.Drawing.Point(4, 22);
            this.tabTypeName.Name = "tabTypeName";
            this.tabTypeName.Padding = new System.Windows.Forms.Padding(3);
            this.tabTypeName.Size = new System.Drawing.Size(1005, 455);
            this.tabTypeName.TabIndex = 2;
            this.tabTypeName.Text = "Kiểu tên";
            this.tabTypeName.UseVisualStyleBackColor = true;
            // 
            // gridTypeName
            // 
            this.gridTypeName.AllowUserToAddRows = false;
            this.gridTypeName.AllowUserToResizeRows = false;
            this.gridTypeName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridTypeName.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridTypeName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridTypeName.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridTypeName.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridTypeName.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.gridTypeName.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTypeName.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.colActionTypeName});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridTypeName.DefaultCellStyle = dataGridViewCellStyle8;
            this.gridTypeName.EnableHeadersVisualStyles = false;
            this.gridTypeName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridTypeName.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridTypeName.Location = new System.Drawing.Point(5, 3);
            this.gridTypeName.Name = "gridTypeName";
            this.gridTypeName.ReadOnly = true;
            this.gridTypeName.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridTypeName.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.gridTypeName.RowHeadersVisible = false;
            this.gridTypeName.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridTypeName.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTypeName.Size = new System.Drawing.Size(995, 449);
            this.gridTypeName.TabIndex = 2;
            this.gridTypeName.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTypeName_CellContentClick);
            this.gridTypeName.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridTypeName_CellDoubleClick);
            this.gridTypeName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridTypeName_MouseClick);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "TypeName";
            this.dataGridViewTextBoxColumn2.HeaderText = "Kiểu tên";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // colActionTypeName
            // 
            this.colActionTypeName.HeaderText = "Action";
            this.colActionTypeName.Name = "colActionTypeName";
            this.colActionTypeName.ReadOnly = true;
            this.colActionTypeName.Text = "Xóa";
            this.colActionTypeName.UseColumnTextForButtonValue = true;
            // 
            // tabRelation
            // 
            this.tabRelation.Controls.Add(this.gridRelation);
            this.tabRelation.Location = new System.Drawing.Point(4, 22);
            this.tabRelation.Name = "tabRelation";
            this.tabRelation.Size = new System.Drawing.Size(1005, 455);
            this.tabRelation.TabIndex = 3;
            this.tabRelation.Text = "Quan hệ";
            this.tabRelation.UseVisualStyleBackColor = true;
            // 
            // gridRelation
            // 
            this.gridRelation.AllowUserToAddRows = false;
            this.gridRelation.AllowUserToResizeRows = false;
            this.gridRelation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridRelation.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridRelation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridRelation.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridRelation.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridRelation.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.gridRelation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRelation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMainRelation,
            this.colRelatedRelation,
            this.colActionRelation});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridRelation.DefaultCellStyle = dataGridViewCellStyle11;
            this.gridRelation.EnableHeadersVisualStyles = false;
            this.gridRelation.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridRelation.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridRelation.Location = new System.Drawing.Point(4, 4);
            this.gridRelation.Name = "gridRelation";
            this.gridRelation.ReadOnly = true;
            this.gridRelation.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridRelation.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.gridRelation.RowHeadersVisible = false;
            this.gridRelation.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridRelation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRelation.Size = new System.Drawing.Size(998, 448);
            this.gridRelation.TabIndex = 0;
            this.gridRelation.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridRelation_CellContentClick);
            this.gridRelation.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridRelation_CellDoubleClick);
            // 
            // colMainRelation
            // 
            this.colMainRelation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMainRelation.DataPropertyName = "MainRelationNameShow";
            this.colMainRelation.HeaderText = "Quan hệ chính";
            this.colMainRelation.Name = "colMainRelation";
            this.colMainRelation.ReadOnly = true;
            // 
            // colRelatedRelation
            // 
            this.colRelatedRelation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRelatedRelation.DataPropertyName = "RelatedRelationNameShow";
            this.colRelatedRelation.HeaderText = "Quan hệ liên quan";
            this.colRelatedRelation.Name = "colRelatedRelation";
            this.colRelatedRelation.ReadOnly = true;
            // 
            // colActionRelation
            // 
            this.colActionRelation.HeaderText = "Action";
            this.colActionRelation.Name = "colActionRelation";
            this.colActionRelation.ReadOnly = true;
            this.colActionRelation.Text = "Xóa";
            this.colActionRelation.UseColumnTextForButtonValue = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAdd.Depth = 0;
            this.btnAdd.DrawShadows = true;
            this.btnAdd.HighEmphasis = true;
            this.btnAdd.Icon = null;
            this.btnAdd.Location = new System.Drawing.Point(920, 6);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(93, 36);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Thêm mới";
            this.btnAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnAdd.UseAccentColor = true;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // ConfigCommon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.materialTabSelector1);
            this.Name = "ConfigCommon";
            this.Size = new System.Drawing.Size(1020, 532);
            this.tabMain.ResumeLayout(false);
            this.tabSystem.ResumeLayout(false);
            this.tabSystem.PerformLayout();
            this.tabNational.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridNational)).EndInit();
            this.tabReligion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReligion)).EndInit();
            this.tabTypeName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTypeName)).EndInit();
            this.tabRelation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRelation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private MaterialSkin.Controls.MaterialTabControl tabMain;
        private System.Windows.Forms.TabPage tabNational;
        private System.Windows.Forms.TabPage tabReligion;
        private System.Windows.Forms.TabPage tabTypeName;
        private MaterialSkin.Controls.MaterialButton btnAdd;
        private Themes.Controls.DataGridTemplate gridNational;
        private Themes.Controls.DataGridTemplate gridReligion;
        private Themes.Controls.DataGridTemplate gridTypeName;
        private System.Windows.Forms.TabPage tabRelation;
        private Themes.Controls.DataGridTemplate gridRelation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMainRelation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRelatedRelation;
        private System.Windows.Forms.DataGridViewButtonColumn colActionRelation;
        private System.Windows.Forms.TabPage tabSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewButtonColumn ColActionNational;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewButtonColumn colActionReligion;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewButtonColumn colActionTypeName;
        private MaterialSkin.Controls.MaterialButton btnRestoreData;
        private MaterialSkin.Controls.MaterialButton btnRestoreNew;
        private MaterialSkin.Controls.MaterialButton btnBackup;
    }
}
