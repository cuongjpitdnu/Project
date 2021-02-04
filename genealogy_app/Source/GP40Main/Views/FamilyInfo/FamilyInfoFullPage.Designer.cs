namespace GP40Main.Views.FamilyInfo
{
    partial class FamilyInfoFullPage
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.materialTabControl2 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.materialListView1 = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.tblEvent = new GP40Main.Themes.Controls.DataGridTemplate(this.components);
            this.colStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndTIme = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnEditFamilyTimeline = new MaterialSkin.Controls.MaterialButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.materialLabel7 = new MaterialSkin.Controls.MaterialLabel();
            this.materialCard2 = new MaterialSkin.Controls.MaterialCard();
            this.materialLabel6 = new MaterialSkin.Controls.MaterialLabel();
            this.txtUserCreated = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel5 = new MaterialSkin.Controls.MaterialLabel();
            this.txtFamilyHometown = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel4 = new MaterialSkin.Controls.MaterialLabel();
            this.txtFamilyLevel = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.txtFamilyAnniversary = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.txtFamilyName = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.btnEditFamilyInfo = new MaterialSkin.Controls.MaterialButton();
            this.materialTabControl2.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblEvent)).BeginInit();
            this.materialCard1.SuspendLayout();
            this.materialCard2.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialTabControl2
            // 
            this.materialTabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTabControl2.Controls.Add(this.tabPage1);
            this.materialTabControl2.Controls.Add(this.tabPage8);
            this.materialTabControl2.Controls.Add(this.tabPage9);
            this.materialTabControl2.Controls.Add(this.tabPage2);
            this.materialTabControl2.Controls.Add(this.tabPage3);
            this.materialTabControl2.Depth = 0;
            this.materialTabControl2.Location = new System.Drawing.Point(502, 55);
            this.materialTabControl2.Margin = new System.Windows.Forms.Padding(0);
            this.materialTabControl2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl2.Name = "materialTabControl2";
            this.materialTabControl2.SelectedIndex = 0;
            this.materialTabControl2.Size = new System.Drawing.Size(716, 387);
            this.materialTabControl2.TabIndex = 70;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(708, 361);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Giới thiệu";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage8
            // 
            this.tabPage8.BackColor = System.Drawing.Color.White;
            this.tabPage8.Controls.Add(this.materialListView1);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(708, 361);
            this.tabPage8.TabIndex = 0;
            this.tabPage8.Text = "D.S Trưởng tộc";
            // 
            // materialListView1
            // 
            this.materialListView1.AllowColumnReorder = true;
            this.materialListView1.AutoSizeTable = true;
            this.materialListView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialListView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.materialListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.materialListView1.Depth = 0;
            this.materialListView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 34F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialListView1.FullRowSelect = true;
            this.materialListView1.HideSelection = false;
            this.materialListView1.Location = new System.Drawing.Point(54, 37);
            this.materialListView1.MinimumSize = new System.Drawing.Size(600, 200);
            this.materialListView1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialListView1.MouseState = MaterialSkin.MouseState.OUT;
            this.materialListView1.Name = "materialListView1";
            this.materialListView1.OwnerDraw = true;
            this.materialListView1.Scrollable = false;
            this.materialListView1.Size = new System.Drawing.Size(600, 200);
            this.materialListView1.TabIndex = 1;
            this.materialListView1.UseCompatibleStateImageBehavior = false;
            this.materialListView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "STT";
            this.columnHeader1.Width = 80;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Tên tộc trưởng";
            this.columnHeader2.Width = 150;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Ngày sinh";
            this.columnHeader3.Width = 120;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Ngày mất";
            this.columnHeader4.Width = 120;
            // 
            // tabPage9
            // 
            this.tabPage9.BackColor = System.Drawing.Color.White;
            this.tabPage9.Controls.Add(this.tblEvent);
            this.tabPage9.Controls.Add(this.btnEditFamilyTimeline);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(708, 361);
            this.tabPage9.TabIndex = 1;
            this.tabPage9.Text = "L.Sử dòng họ";
            // 
            // tblEvent
            // 
            this.tblEvent.AllowUserToDeleteRows = false;
            this.tblEvent.AllowUserToResizeRows = false;
            this.tblEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblEvent.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tblEvent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tblEvent.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.tblEvent.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tblEvent.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.tblEvent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tblEvent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStartTime,
            this.colEndTIme,
            this.colEvent});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.tblEvent.DefaultCellStyle = dataGridViewCellStyle4;
            this.tblEvent.EnableHeadersVisualStyles = false;
            this.tblEvent.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tblEvent.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.tblEvent.Location = new System.Drawing.Point(3, 45);
            this.tblEvent.Name = "tblEvent";
            this.tblEvent.ReadOnly = true;
            this.tblEvent.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.tblEvent.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.tblEvent.RowHeadersVisible = false;
            this.tblEvent.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.tblEvent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tblEvent.Size = new System.Drawing.Size(735, 320);
            this.tblEvent.TabIndex = 3;
            this.tblEvent.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tblEvent_CellDoubleClick);
            // 
            // colStartTime
            // 
            this.colStartTime.DataPropertyName = "StartDate";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colStartTime.DefaultCellStyle = dataGridViewCellStyle2;
            this.colStartTime.HeaderText = "Bắt đầu";
            this.colStartTime.Name = "colStartTime";
            this.colStartTime.ReadOnly = true;
            // 
            // colEndTIme
            // 
            this.colEndTIme.DataPropertyName = "EndDate";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colEndTIme.DefaultCellStyle = dataGridViewCellStyle3;
            this.colEndTIme.HeaderText = "Kết thúc";
            this.colEndTIme.Name = "colEndTIme";
            this.colEndTIme.ReadOnly = true;
            // 
            // colEvent
            // 
            this.colEvent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colEvent.DataPropertyName = "Content";
            this.colEvent.HeaderText = "Sự kiện";
            this.colEvent.Name = "colEvent";
            this.colEvent.ReadOnly = true;
            // 
            // btnEditFamilyTimeline
            // 
            this.btnEditFamilyTimeline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditFamilyTimeline.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnEditFamilyTimeline.Depth = 0;
            this.btnEditFamilyTimeline.DrawShadows = true;
            this.btnEditFamilyTimeline.ForeColor = System.Drawing.Color.White;
            this.btnEditFamilyTimeline.HighEmphasis = true;
            this.btnEditFamilyTimeline.Icon = global::GP40Main.Properties.Resources.edit_icon;
            this.btnEditFamilyTimeline.Location = new System.Drawing.Point(657, 6);
            this.btnEditFamilyTimeline.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnEditFamilyTimeline.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnEditFamilyTimeline.Name = "btnEditFamilyTimeline";
            this.btnEditFamilyTimeline.Size = new System.Drawing.Size(44, 36);
            this.btnEditFamilyTimeline.TabIndex = 2;
            this.btnEditFamilyTimeline.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.btnEditFamilyTimeline.UseAccentColor = false;
            this.btnEditFamilyTimeline.UseVisualStyleBackColor = true;
            this.btnEditFamilyTimeline.Click += new System.EventHandler(this.btnEditFamilyTimeline_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(708, 361);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "Album Ảnh";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(708, 361);
            this.tabPage3.TabIndex = 4;
            this.tabPage3.Text = "Tài liệu";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTabSelector1.BaseTabControl = this.materialTabControl2;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialTabSelector1.Location = new System.Drawing.Point(502, 16);
            this.materialTabSelector1.Margin = new System.Windows.Forms.Padding(0);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(716, 38);
            this.materialTabSelector1.TabIndex = 71;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.Controls.Add(this.materialLabel7);
            this.materialCard1.Depth = 0;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(27, 303);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(7);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(463, 139);
            this.materialCard1.TabIndex = 67;
            // 
            // materialLabel7
            // 
            this.materialLabel7.AutoSize = true;
            this.materialLabel7.Depth = 0;
            this.materialLabel7.Font = new System.Drawing.Font("Roboto", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel7.FontType = MaterialSkin.MaterialSkinManager.fontType.H5;
            this.materialLabel7.Location = new System.Drawing.Point(16, 14);
            this.materialLabel7.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel7.Name = "materialLabel7";
            this.materialLabel7.Size = new System.Drawing.Size(192, 29);
            this.materialLabel7.TabIndex = 13;
            this.materialLabel7.Text = "Thống kê dòng họ";
            // 
            // materialCard2
            // 
            this.materialCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard2.Controls.Add(this.materialLabel6);
            this.materialCard2.Controls.Add(this.txtUserCreated);
            this.materialCard2.Controls.Add(this.materialLabel5);
            this.materialCard2.Controls.Add(this.txtFamilyHometown);
            this.materialCard2.Controls.Add(this.materialLabel4);
            this.materialCard2.Controls.Add(this.txtFamilyLevel);
            this.materialCard2.Controls.Add(this.materialLabel3);
            this.materialCard2.Controls.Add(this.txtFamilyAnniversary);
            this.materialCard2.Controls.Add(this.materialLabel2);
            this.materialCard2.Controls.Add(this.txtFamilyName);
            this.materialCard2.Controls.Add(this.materialLabel1);
            this.materialCard2.Controls.Add(this.btnEditFamilyInfo);
            this.materialCard2.Depth = 0;
            this.materialCard2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard2.Location = new System.Drawing.Point(27, 16);
            this.materialCard2.Margin = new System.Windows.Forms.Padding(7);
            this.materialCard2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard2.Name = "materialCard2";
            this.materialCard2.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard2.Size = new System.Drawing.Size(463, 273);
            this.materialCard2.TabIndex = 67;
            // 
            // materialLabel6
            // 
            this.materialLabel6.AutoSize = true;
            this.materialLabel6.Depth = 0;
            this.materialLabel6.Font = new System.Drawing.Font("Roboto", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel6.FontType = MaterialSkin.MaterialSkinManager.fontType.H5;
            this.materialLabel6.Location = new System.Drawing.Point(17, 14);
            this.materialLabel6.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel6.Name = "materialLabel6";
            this.materialLabel6.Size = new System.Drawing.Size(194, 29);
            this.materialLabel6.TabIndex = 13;
            this.materialLabel6.Text = "Thông tin dòng họ";
            // 
            // txtUserCreated
            // 
            this.txtUserCreated.AutoSize = true;
            this.txtUserCreated.Depth = 0;
            this.txtUserCreated.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtUserCreated.Location = new System.Drawing.Point(156, 213);
            this.txtUserCreated.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtUserCreated.Name = "txtUserCreated";
            this.txtUserCreated.Size = new System.Drawing.Size(107, 19);
            this.txtUserCreated.TabIndex = 12;
            this.txtUserCreated.Text = "materialLabel6";
            // 
            // materialLabel5
            // 
            this.materialLabel5.AutoSize = true;
            this.materialLabel5.Depth = 0;
            this.materialLabel5.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel5.Location = new System.Drawing.Point(19, 213);
            this.materialLabel5.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel5.Name = "materialLabel5";
            this.materialLabel5.Size = new System.Drawing.Size(131, 19);
            this.materialLabel5.TabIndex = 11;
            this.materialLabel5.Text = "Người lập gia phả:";
            // 
            // txtFamilyHometown
            // 
            this.txtFamilyHometown.AutoSize = true;
            this.txtFamilyHometown.Depth = 0;
            this.txtFamilyHometown.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFamilyHometown.Location = new System.Drawing.Point(124, 161);
            this.txtFamilyHometown.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtFamilyHometown.Name = "txtFamilyHometown";
            this.txtFamilyHometown.Size = new System.Drawing.Size(107, 19);
            this.txtFamilyHometown.TabIndex = 10;
            this.txtFamilyHometown.Text = "materialLabel5";
            // 
            // materialLabel4
            // 
            this.materialLabel4.AutoSize = true;
            this.materialLabel4.Depth = 0;
            this.materialLabel4.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel4.Location = new System.Drawing.Point(18, 162);
            this.materialLabel4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            this.materialLabel4.Size = new System.Drawing.Size(99, 19);
            this.materialLabel4.TabIndex = 9;
            this.materialLabel4.Text = "Nguyên quán:";
            // 
            // txtFamilyLevel
            // 
            this.txtFamilyLevel.AutoSize = true;
            this.txtFamilyLevel.Depth = 0;
            this.txtFamilyLevel.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFamilyLevel.Location = new System.Drawing.Point(313, 112);
            this.txtFamilyLevel.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtFamilyLevel.Name = "txtFamilyLevel";
            this.txtFamilyLevel.Size = new System.Drawing.Size(107, 19);
            this.txtFamilyLevel.TabIndex = 8;
            this.txtFamilyLevel.Text = "materialLabel4";
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel3.Location = new System.Drawing.Point(250, 112);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(57, 19);
            this.materialLabel3.TabIndex = 7;
            this.materialLabel3.Text = "Đời thứ:";
            // 
            // txtFamilyAnniversary
            // 
            this.txtFamilyAnniversary.AutoSize = true;
            this.txtFamilyAnniversary.Depth = 0;
            this.txtFamilyAnniversary.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFamilyAnniversary.Location = new System.Drawing.Point(93, 112);
            this.txtFamilyAnniversary.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtFamilyAnniversary.Name = "txtFamilyAnniversary";
            this.txtFamilyAnniversary.Size = new System.Drawing.Size(107, 19);
            this.txtFamilyAnniversary.TabIndex = 6;
            this.txtFamilyAnniversary.Text = "materialLabel3";
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.Location = new System.Drawing.Point(19, 112);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(68, 19);
            this.materialLabel2.TabIndex = 5;
            this.materialLabel2.Text = "Ngày giỗ:";
            // 
            // txtFamilyName
            // 
            this.txtFamilyName.AutoSize = true;
            this.txtFamilyName.Depth = 0;
            this.txtFamilyName.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFamilyName.Location = new System.Drawing.Point(118, 61);
            this.txtFamilyName.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtFamilyName.Name = "txtFamilyName";
            this.txtFamilyName.Size = new System.Drawing.Size(107, 19);
            this.txtFamilyName.TabIndex = 4;
            this.txtFamilyName.Text = "materialLabel2";
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(17, 61);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(94, 19);
            this.materialLabel1.TabIndex = 3;
            this.materialLabel1.Text = "Tên dòng họ:";
            // 
            // btnEditFamilyInfo
            // 
            this.btnEditFamilyInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnEditFamilyInfo.Depth = 0;
            this.btnEditFamilyInfo.DrawShadows = true;
            this.btnEditFamilyInfo.ForeColor = System.Drawing.Color.White;
            this.btnEditFamilyInfo.HighEmphasis = true;
            this.btnEditFamilyInfo.Icon = global::GP40Main.Properties.Resources.edit_icon;
            this.btnEditFamilyInfo.Location = new System.Drawing.Point(408, 3);
            this.btnEditFamilyInfo.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnEditFamilyInfo.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnEditFamilyInfo.Name = "btnEditFamilyInfo";
            this.btnEditFamilyInfo.Size = new System.Drawing.Size(44, 36);
            this.btnEditFamilyInfo.TabIndex = 2;
            this.btnEditFamilyInfo.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.btnEditFamilyInfo.UseAccentColor = false;
            this.btnEditFamilyInfo.UseVisualStyleBackColor = true;
            this.btnEditFamilyInfo.Click += new System.EventHandler(this.btnEditFamilyInfo_Click);
            // 
            // FamilyInfoFullPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.Controls.Add(this.materialTabControl2);
            this.Controls.Add(this.materialTabSelector1);
            this.Controls.Add(this.materialCard1);
            this.Controls.Add(this.materialCard2);
            this.Name = "FamilyInfoFullPage";
            this.Size = new System.Drawing.Size(1244, 456);
            this.materialTabControl2.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblEvent)).EndInit();
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            this.materialCard2.ResumeLayout(false);
            this.materialCard2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialCard materialCard2;
        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialTabControl materialTabControl2;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.TabPage tabPage9;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private MaterialSkin.Controls.MaterialListView materialListView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private MaterialSkin.Controls.MaterialButton btnEditFamilyInfo;
        private MaterialSkin.Controls.MaterialButton btnEditFamilyTimeline;
        private Themes.Controls.DataGridTemplate tblEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndTIme;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEvent;
        private MaterialSkin.Controls.MaterialLabel txtFamilyName;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel txtUserCreated;
        private MaterialSkin.Controls.MaterialLabel materialLabel5;
        private MaterialSkin.Controls.MaterialLabel txtFamilyHometown;
        private MaterialSkin.Controls.MaterialLabel materialLabel4;
        private MaterialSkin.Controls.MaterialLabel txtFamilyLevel;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialLabel txtFamilyAnniversary;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialLabel materialLabel6;
        private MaterialSkin.Controls.MaterialLabel materialLabel7;
    }
}
