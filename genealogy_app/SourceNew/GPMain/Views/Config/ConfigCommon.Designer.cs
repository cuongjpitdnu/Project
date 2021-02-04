namespace GPMain.Views.Config
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
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.tabMain = new MaterialSkin.Controls.MaterialTabControl();
            this.tabSystem = new System.Windows.Forms.TabPage();
            this.dgvListVersion = new GPMain.Views.Controls.DataGridTemplate(this.components);
            this.colVersionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTimeCreateShow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnBackup = new MaterialSkin.Controls.MaterialButton();
            this.btnRestoreNew = new MaterialSkin.Controls.MaterialButton();
            this.btnRestoreData = new MaterialSkin.Controls.MaterialButton();
            this.tabNational = new System.Windows.Forms.TabPage();
            this.gridNational = new GPMain.Views.Controls.DataGridTemplate(this.components);
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabReligion = new System.Windows.Forms.TabPage();
            this.gridReligion = new GPMain.Views.Controls.DataGridTemplate(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabTypeName = new System.Windows.Forms.TabPage();
            this.gridTypeName = new GPMain.Views.Controls.DataGridTemplate(this.components);
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabRelation = new System.Windows.Forms.TabPage();
            this.gridRelation = new GPMain.Views.Controls.DataGridTemplate(this.components);
            this.colMainRelation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRelatedRelation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabUpdateRestoreVersion = new System.Windows.Forms.TabPage();
            this.lblAppVersion = new MaterialSkin.Controls.MaterialLabel();
            this.lblAppName = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.btnRestore = new MaterialSkin.Controls.MaterialButton();
            this.btnUpdate = new MaterialSkin.Controls.MaterialButton();
            this.colActionDownload = new GPMain.Views.Controls.DataGridViewIconColumn(this.components);
            this.colActionRestore = new GPMain.Views.Controls.DataGridViewIconColumn(this.components);
            this.colActionDelete = new GPMain.Views.Controls.DataGridViewIconColumn(this.components);
            this.btnAdd = new MaterialSkin.Controls.MaterialButton();
            this.tabMain.SuspendLayout();
            this.tabSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListVersion)).BeginInit();
            this.tabNational.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridNational)).BeginInit();
            this.tabReligion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReligion)).BeginInit();
            this.tabTypeName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTypeName)).BeginInit();
            this.tabRelation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRelation)).BeginInit();
            this.tabUpdateRestoreVersion.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.BaseTabControl = this.tabMain;
            this.materialTabSelector1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialTabSelector1.Location = new System.Drawing.Point(4, 6);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(735, 36);
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
            this.tabMain.Controls.Add(this.tabUpdateRestoreVersion);
            this.tabMain.Depth = 0;
            this.tabMain.Location = new System.Drawing.Point(4, 48);
            this.tabMain.MouseState = MaterialSkin.MouseState.HOVER;
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1013, 481);
            this.tabMain.TabIndex = 1;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.TabMain_SelectedIndexChanged);
            // 
            // tabSystem
            // 
            this.tabSystem.BackColor = System.Drawing.Color.White;
            this.tabSystem.Controls.Add(this.dgvListVersion);
            this.tabSystem.Controls.Add(this.btnBackup);
            this.tabSystem.Controls.Add(this.btnRestoreNew);
            this.tabSystem.Controls.Add(this.btnRestoreData);
            this.tabSystem.Location = new System.Drawing.Point(4, 22);
            this.tabSystem.Name = "tabSystem";
            this.tabSystem.Size = new System.Drawing.Size(1005, 455);
            this.tabSystem.TabIndex = 4;
            this.tabSystem.Text = "Hệ thống";
            // 
            // dgvListVersion
            // 
            this.dgvListVersion.AllowUserToAddRows = false;
            this.dgvListVersion.AllowUserToDeleteRows = false;
            this.dgvListVersion.AllowUserToResizeRows = false;
            this.dgvListVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvListVersion.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgvListVersion.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvListVersion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colVersionName,
            this.colTimeCreateShow,
            this.colFileSize});
            this.dgvListVersion.EnableHeadersVisualStyles = false;
            this.dgvListVersion.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dgvListVersion.Location = new System.Drawing.Point(210, 15);
            this.dgvListVersion.Name = "dgvListVersion";
            this.dgvListVersion.ReadOnly = true;
            this.dgvListVersion.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvListVersion.RowHeadersVisible = false;
            this.dgvListVersion.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvListVersion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListVersion.Size = new System.Drawing.Size(792, 437);
            this.dgvListVersion.TabIndex = 3;
            this.dgvListVersion.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvListVersion_CellContentClick);
            // 
            // colVersionName
            // 
            this.colVersionName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colVersionName.DataPropertyName = "VersionName";
            this.colVersionName.HeaderText = "Phiên bản";
            this.colVersionName.Name = "colVersionName";
            this.colVersionName.ReadOnly = true;
            this.colVersionName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colTimeCreateShow
            // 
            this.colTimeCreateShow.DataPropertyName = "TimeCreate";
            this.colTimeCreateShow.HeaderText = "Thời gian tạo";
            this.colTimeCreateShow.Name = "colTimeCreateShow";
            this.colTimeCreateShow.ReadOnly = true;
            this.colTimeCreateShow.Width = 150;
            // 
            // colFileSize
            // 
            this.colFileSize.DataPropertyName = "Size";
            this.colFileSize.HeaderText = "Size";
            this.colFileSize.Name = "colFileSize";
            this.colFileSize.ReadOnly = true;
            // 
            // btnBackup
            // 
            this.btnBackup.AutoSize = false;
            this.btnBackup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBackup.BackColor = System.Drawing.Color.White;
            this.btnBackup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBackup.Depth = 0;
            this.btnBackup.DrawShadows = true;
            this.btnBackup.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBackup.HighEmphasis = true;
            this.btnBackup.Icon = null;
            this.btnBackup.Location = new System.Drawing.Point(16, 111);
            this.btnBackup.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnBackup.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(184, 36);
            this.btnBackup.TabIndex = 0;
            this.btnBackup.Text = "Sao lưu dữ liệu";
            this.btnBackup.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnBackup.UseAccentColor = true;
            this.btnBackup.UseVisualStyleBackColor = false;
            this.btnBackup.Click += new System.EventHandler(this.BtnBackup_Click);
            // 
            // btnRestoreNew
            // 
            this.btnRestoreNew.AutoSize = false;
            this.btnRestoreNew.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRestoreNew.BackColor = System.Drawing.Color.White;
            this.btnRestoreNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestoreNew.Depth = 0;
            this.btnRestoreNew.DrawShadows = true;
            this.btnRestoreNew.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRestoreNew.HighEmphasis = true;
            this.btnRestoreNew.Icon = null;
            this.btnRestoreNew.Location = new System.Drawing.Point(16, 63);
            this.btnRestoreNew.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnRestoreNew.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnRestoreNew.Name = "btnRestoreNew";
            this.btnRestoreNew.Size = new System.Drawing.Size(184, 36);
            this.btnRestoreNew.TabIndex = 0;
            this.btnRestoreNew.Text = "Khôi phục dữ liệu mới";
            this.btnRestoreNew.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnRestoreNew.UseAccentColor = true;
            this.btnRestoreNew.UseVisualStyleBackColor = false;
            this.btnRestoreNew.Click += new System.EventHandler(this.BtnRestoreNew_Click);
            // 
            // btnRestoreData
            // 
            this.btnRestoreData.AutoSize = false;
            this.btnRestoreData.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRestoreData.BackColor = System.Drawing.Color.White;
            this.btnRestoreData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestoreData.Depth = 0;
            this.btnRestoreData.DrawShadows = true;
            this.btnRestoreData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRestoreData.HighEmphasis = true;
            this.btnRestoreData.Icon = null;
            this.btnRestoreData.Location = new System.Drawing.Point(16, 15);
            this.btnRestoreData.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnRestoreData.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnRestoreData.Name = "btnRestoreData";
            this.btnRestoreData.Size = new System.Drawing.Size(184, 36);
            this.btnRestoreData.TabIndex = 0;
            this.btnRestoreData.Text = "Khôi phục dữ liệu cũ";
            this.btnRestoreData.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnRestoreData.UseAccentColor = true;
            this.btnRestoreData.UseVisualStyleBackColor = false;
            this.btnRestoreData.Click += new System.EventHandler(this.BtnRestoreData_Click);
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
            this.gridNational.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName});
            this.gridNational.EnableHeadersVisualStyles = false;
            this.gridNational.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridNational.Location = new System.Drawing.Point(4, 3);
            this.gridNational.Name = "gridNational";
            this.gridNational.ReadOnly = true;
            this.gridNational.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gridNational.RowHeadersVisible = false;
            this.gridNational.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridNational.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridNational.Size = new System.Drawing.Size(995, 452);
            this.gridNational.TabIndex = 0;
            this.gridNational.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GridNational_MouseClick);
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "NatName";
            this.colName.HeaderText = "Tên quốc gia";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
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
            this.gridReligion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.gridReligion.EnableHeadersVisualStyles = false;
            this.gridReligion.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridReligion.Location = new System.Drawing.Point(5, 3);
            this.gridReligion.Name = "gridReligion";
            this.gridReligion.ReadOnly = true;
            this.gridReligion.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gridReligion.RowHeadersVisible = false;
            this.gridReligion.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridReligion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridReligion.Size = new System.Drawing.Size(995, 449);
            this.gridReligion.TabIndex = 1;
            this.gridReligion.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GridReligion_MouseClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "RelName";
            this.dataGridViewTextBoxColumn1.HeaderText = "Tên tôn giáo";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
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
            this.gridTypeName.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2});
            this.gridTypeName.EnableHeadersVisualStyles = false;
            this.gridTypeName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridTypeName.Location = new System.Drawing.Point(5, 3);
            this.gridTypeName.Name = "gridTypeName";
            this.gridTypeName.ReadOnly = true;
            this.gridTypeName.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gridTypeName.RowHeadersVisible = false;
            this.gridTypeName.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridTypeName.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTypeName.Size = new System.Drawing.Size(995, 449);
            this.gridTypeName.TabIndex = 2;
            this.gridTypeName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GridTypeName_MouseClick);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "TypeName";
            this.dataGridViewTextBoxColumn2.HeaderText = "Kiểu tên";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
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
            this.gridRelation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMainRelation,
            this.colRelatedRelation});
            this.gridRelation.EnableHeadersVisualStyles = false;
            this.gridRelation.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridRelation.Location = new System.Drawing.Point(4, 4);
            this.gridRelation.Name = "gridRelation";
            this.gridRelation.ReadOnly = true;
            this.gridRelation.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gridRelation.RowHeadersVisible = false;
            this.gridRelation.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridRelation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRelation.Size = new System.Drawing.Size(998, 448);
            this.gridRelation.TabIndex = 0;
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
            // tabUpdateRestoreVersion
            // 
            this.tabUpdateRestoreVersion.BackColor = System.Drawing.Color.White;
            this.tabUpdateRestoreVersion.Controls.Add(this.lblAppVersion);
            this.tabUpdateRestoreVersion.Controls.Add(this.lblAppName);
            this.tabUpdateRestoreVersion.Controls.Add(this.materialLabel1);
            this.tabUpdateRestoreVersion.Controls.Add(this.btnRestore);
            this.tabUpdateRestoreVersion.Controls.Add(this.btnUpdate);
            this.tabUpdateRestoreVersion.Location = new System.Drawing.Point(4, 22);
            this.tabUpdateRestoreVersion.Name = "tabUpdateRestoreVersion";
            this.tabUpdateRestoreVersion.Size = new System.Drawing.Size(1005, 455);
            this.tabUpdateRestoreVersion.TabIndex = 5;
            this.tabUpdateRestoreVersion.Text = "Cập nhật & Khôi phục";
            // 
            // lblAppVersion
            // 
            this.lblAppVersion.AutoSize = true;
            this.lblAppVersion.Depth = 0;
            this.lblAppVersion.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAppVersion.Location = new System.Drawing.Point(43, 72);
            this.lblAppVersion.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblAppVersion.Name = "lblAppVersion";
            this.lblAppVersion.Size = new System.Drawing.Size(84, 19);
            this.lblAppVersion.TabIndex = 4;
            this.lblAppVersion.Text = "App version";
            // 
            // lblAppName
            // 
            this.lblAppName.AutoSize = true;
            this.lblAppName.Depth = 0;
            this.lblAppName.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblAppName.Location = new System.Drawing.Point(43, 45);
            this.lblAppName.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblAppName.Name = "lblAppName";
            this.lblAppName.Size = new System.Drawing.Size(73, 19);
            this.lblAppName.TabIndex = 3;
            this.lblAppName.Text = "App name";
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(15, 17);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(147, 19);
            this.materialLabel1.TabIndex = 2;
            this.materialLabel1.Text = "Thông tin phiên bản:";
            // 
            // btnRestore
            // 
            this.btnRestore.AutoSize = false;
            this.btnRestore.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnRestore.BackColor = System.Drawing.Color.White;
            this.btnRestore.Depth = 0;
            this.btnRestore.DrawShadows = true;
            this.btnRestore.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRestore.HighEmphasis = true;
            this.btnRestore.Icon = null;
            this.btnRestore.Location = new System.Drawing.Point(159, 119);
            this.btnRestore.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnRestore.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(136, 36);
            this.btnRestore.TabIndex = 1;
            this.btnRestore.Text = "Khôi phục";
            this.btnRestore.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnRestore.UseAccentColor = true;
            this.btnRestore.UseVisualStyleBackColor = false;
            this.btnRestore.Click += new System.EventHandler(this.BtnRestore_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.AutoSize = false;
            this.btnUpdate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnUpdate.BackColor = System.Drawing.Color.White;
            this.btnUpdate.Depth = 0;
            this.btnUpdate.DrawShadows = true;
            this.btnUpdate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnUpdate.HighEmphasis = true;
            this.btnUpdate.Icon = null;
            this.btnUpdate.Location = new System.Drawing.Point(15, 119);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnUpdate.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(136, 36);
            this.btnUpdate.TabIndex = 1;
            this.btnUpdate.Text = "Cập nhật";
            this.btnUpdate.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnUpdate.UseAccentColor = true;
            this.btnUpdate.UseVisualStyleBackColor = false;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // colActionDownload
            // 
            this.colActionDownload.HeaderText = "Action download";
            this.colActionDownload.Name = "colActionDownload";
            this.colActionDownload.ReadOnly = true;
            this.colActionDownload.Text = "Download";
            this.colActionDownload.UseColumnTextForButtonValue = true;
            // 
            // colActionRestore
            // 
            this.colActionRestore.HeaderText = "Action restore";
            this.colActionRestore.Name = "colActionRestore";
            this.colActionRestore.ReadOnly = true;
            this.colActionRestore.Text = "Khôi phục";
            this.colActionRestore.UseColumnTextForButtonValue = true;
            // 
            // colActionDelete
            // 
            this.colActionDelete.HeaderText = "Action delete";
            this.colActionDelete.Name = "colActionDelete";
            this.colActionDelete.ReadOnly = true;
            this.colActionDelete.Text = "Xóa";
            this.colActionDelete.UseColumnTextForButtonValue = true;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAdd.Depth = 0;
            this.btnAdd.DrawShadows = true;
            this.btnAdd.HighEmphasis = true;
            this.btnAdd.Icon = null;
            this.btnAdd.Location = new System.Drawing.Point(923, 6);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnAdd.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(90, 36);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "Thêm mới";
            this.btnAdd.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnAdd.UseAccentColor = true;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // ConfigCommon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.materialTabSelector1);
            this.Name = "ConfigCommon";
            this.Size = new System.Drawing.Size(1020, 532);
            this.tabMain.ResumeLayout(false);
            this.tabSystem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListVersion)).EndInit();
            this.tabNational.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridNational)).EndInit();
            this.tabReligion.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReligion)).EndInit();
            this.tabTypeName.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridTypeName)).EndInit();
            this.tabRelation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRelation)).EndInit();
            this.tabUpdateRestoreVersion.ResumeLayout(false);
            this.tabUpdateRestoreVersion.PerformLayout();
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
        private Views.Controls.DataGridTemplate gridNational;
        private Views.Controls.DataGridTemplate gridReligion;
        private Views.Controls.DataGridTemplate gridTypeName;
        private System.Windows.Forms.TabPage tabRelation;
        private Views.Controls.DataGridTemplate gridRelation;
        private System.Windows.Forms.TabPage tabSystem;
        private MaterialSkin.Controls.MaterialButton btnRestoreData;
        private MaterialSkin.Controls.MaterialButton btnRestoreNew;
        private MaterialSkin.Controls.MaterialButton btnBackup;
        private System.Windows.Forms.TabPage tabUpdateRestoreVersion;
        private MaterialSkin.Controls.MaterialButton btnRestore;
        private MaterialSkin.Controls.MaterialButton btnUpdate;
        private MaterialSkin.Controls.MaterialLabel lblAppName;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel lblAppVersion;
        private Controls.DataGridTemplate dgvListVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVersionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTimeCreateShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileSize;
        private GPMain.Views.Controls.DataGridViewIconColumn colActionDownload;
        private GPMain.Views.Controls.DataGridViewIconColumn colActionRestore;
        private GPMain.Views.Controls.DataGridViewIconColumn colActionDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMainRelation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRelatedRelation;
    }
}
