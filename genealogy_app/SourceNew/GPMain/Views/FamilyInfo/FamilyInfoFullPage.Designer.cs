namespace GPMain.Views.FamilyInfo
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
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.tabFamilyAlbum = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.gridFamilyHead = new GPMain.Views.Controls.DataGridTemplate(this.components);
            this.colFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBirthday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddFamilyHead = new MaterialSkin.Controls.MaterialButton();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.tblEvent = new GPMain.Views.Controls.DataGridTemplate(this.components);
            this.colStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEndTIme = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnEditFamilyTimeline = new MaterialSkin.Controls.MaterialButton();
            this.tabAlbum = new System.Windows.Forms.TabPage();
            this.tabDocument = new System.Windows.Forms.TabPage();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.txtPercentUnknown = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel26 = new MaterialSkin.Controls.MaterialLabel();
            this.txtPercentFemale = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel14 = new MaterialSkin.Controls.MaterialLabel();
            this.txtPercentMale = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel12 = new MaterialSkin.Controls.MaterialLabel();
            this.txtPercentLiving = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel10 = new MaterialSkin.Controls.MaterialLabel();
            this.txtPercentDear = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel9 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel7 = new MaterialSkin.Controls.MaterialLabel();
            this.materialCard2 = new MaterialSkin.Controls.MaterialCard();
            this.txtTotalMember = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel6 = new MaterialSkin.Controls.MaterialLabel();
            this.txtUserCreated = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel5 = new MaterialSkin.Controls.MaterialLabel();
            this.txtFamilyHometown = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel4 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel8 = new MaterialSkin.Controls.MaterialLabel();
            this.txtFamilyLevel = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.txtFamilyAnniversary = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.txtFamilyName = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.btnEditFamilyInfo = new MaterialSkin.Controls.MaterialButton();
            this.materialCard3 = new MaterialSkin.Controls.MaterialCard();
            this.txtAgeOver71 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel30 = new MaterialSkin.Controls.MaterialLabel();
            this.txtAge5671 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel28 = new MaterialSkin.Controls.MaterialLabel();
            this.txtAge3655 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel17 = new MaterialSkin.Controls.MaterialLabel();
            this.txtAge1835 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel19 = new MaterialSkin.Controls.MaterialLabel();
            this.txtAge617 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel21 = new MaterialSkin.Controls.MaterialLabel();
            this.txtAge05 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel23 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel24 = new MaterialSkin.Controls.MaterialLabel();
            this.llb_lstBirthday = new System.Windows.Forms.LinkLabel();
            this.llb_lstDeadDay = new System.Windows.Forms.LinkLabel();
            this.colDelAction = new System.Windows.Forms.DataGridViewLinkColumn();
            this.tabFamilyAlbum.SuspendLayout();
            this.tabPage8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilyHead)).BeginInit();
            this.tabPage9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblEvent)).BeginInit();
            this.materialCard1.SuspendLayout();
            this.materialCard2.SuspendLayout();
            this.materialCard3.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialTabSelector1.BaseTabControl = this.tabFamilyAlbum;
            this.materialTabSelector1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialTabSelector1.Location = new System.Drawing.Point(502, 17);
            this.materialTabSelector1.Margin = new System.Windows.Forms.Padding(0);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(712, 38);
            this.materialTabSelector1.TabIndex = 71;
            this.materialTabSelector1.Text = "materialTabSelector1";
            this.materialTabSelector1.Click += new System.EventHandler(this.materialTabSelector1_Click);
            // 
            // tabFamilyAlbum
            // 
            this.tabFamilyAlbum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabFamilyAlbum.Controls.Add(this.tabPage8);
            this.tabFamilyAlbum.Controls.Add(this.tabPage9);
            this.tabFamilyAlbum.Controls.Add(this.tabAlbum);
            this.tabFamilyAlbum.Controls.Add(this.tabDocument);
            this.tabFamilyAlbum.Depth = 0;
            this.tabFamilyAlbum.Location = new System.Drawing.Point(502, 56);
            this.tabFamilyAlbum.Margin = new System.Windows.Forms.Padding(0);
            this.tabFamilyAlbum.MouseState = MaterialSkin.MouseState.HOVER;
            this.tabFamilyAlbum.Name = "tabFamilyAlbum";
            this.tabFamilyAlbum.SelectedIndex = 0;
            this.tabFamilyAlbum.Size = new System.Drawing.Size(716, 623);
            this.tabFamilyAlbum.TabIndex = 70;
            this.tabFamilyAlbum.SelectedIndexChanged += new System.EventHandler(this.tabmainfamily_selectedindexchanged);
            // 
            // tabPage8
            // 
            this.tabPage8.BackColor = System.Drawing.Color.White;
            this.tabPage8.Controls.Add(this.gridFamilyHead);
            this.tabPage8.Controls.Add(this.btnAddFamilyHead);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(708, 597);
            this.tabPage8.TabIndex = 0;
            this.tabPage8.Text = "D.S Trưởng tộc";
            // 
            // gridFamilyHead
            // 
            this.gridFamilyHead.AllowUserToAddRows = false;
            this.gridFamilyHead.AllowUserToDeleteRows = false;
            this.gridFamilyHead.AllowUserToResizeRows = false;
            this.gridFamilyHead.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFamilyHead.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridFamilyHead.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFullName,
            this.colAddress,
            this.colBirthday});
            this.gridFamilyHead.EnableHeadersVisualStyles = false;
            this.gridFamilyHead.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridFamilyHead.Location = new System.Drawing.Point(3, 43);
            this.gridFamilyHead.Name = "gridFamilyHead";
            this.gridFamilyHead.ReadOnly = true;
            this.gridFamilyHead.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gridFamilyHead.RowHeadersVisible = false;
            this.gridFamilyHead.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridFamilyHead.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFamilyHead.Size = new System.Drawing.Size(702, 551);
            this.gridFamilyHead.TabIndex = 5;
            // 
            // colFullName
            // 
            this.colFullName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFullName.DataPropertyName = "Name";
            this.colFullName.HeaderText = "Họ tên";
            this.colFullName.Name = "colFullName";
            this.colFullName.ReadOnly = true;
            // 
            // colAddress
            // 
            this.colAddress.DataPropertyName = "Address";
            this.colAddress.HeaderText = "Địa chỉ";
            this.colAddress.Name = "colAddress";
            this.colAddress.ReadOnly = true;
            this.colAddress.Width = 250;
            // 
            // colBirthday
            // 
            this.colBirthday.DataPropertyName = "BirthdayShow";
            this.colBirthday.HeaderText = "Ngày sinh";
            this.colBirthday.Name = "colBirthday";
            this.colBirthday.ReadOnly = true;
            // 
            // btnAddFamilyHead
            // 
            this.btnAddFamilyHead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddFamilyHead.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddFamilyHead.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddFamilyHead.Depth = 0;
            this.btnAddFamilyHead.DrawShadows = true;
            this.btnAddFamilyHead.ForeColor = System.Drawing.Color.White;
            this.btnAddFamilyHead.HighEmphasis = true;
            this.btnAddFamilyHead.Icon = global::GPMain.Properties.Resources.edit_icon;
            this.btnAddFamilyHead.Location = new System.Drawing.Point(661, 4);
            this.btnAddFamilyHead.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnAddFamilyHead.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAddFamilyHead.Name = "btnAddFamilyHead";
            this.btnAddFamilyHead.Size = new System.Drawing.Size(44, 36);
            this.btnAddFamilyHead.TabIndex = 4;
            this.btnAddFamilyHead.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.btnAddFamilyHead.UseAccentColor = false;
            this.btnAddFamilyHead.UseVisualStyleBackColor = true;
            this.btnAddFamilyHead.Click += new System.EventHandler(this.btnAddFamilyHead_Click);
            // 
            // tabPage9
            // 
            this.tabPage9.BackColor = System.Drawing.Color.White;
            this.tabPage9.Controls.Add(this.tblEvent);
            this.tabPage9.Controls.Add(this.btnEditFamilyTimeline);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(708, 597);
            this.tabPage9.TabIndex = 1;
            this.tabPage9.Text = "L.Sử dòng họ";
            // 
            // tblEvent
            // 
            this.tblEvent.AllowUserToAddRows = false;
            this.tblEvent.AllowUserToDeleteRows = false;
            this.tblEvent.AllowUserToResizeRows = false;
            this.tblEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblEvent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tblEvent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStartTime,
            this.colEndTIme,
            this.colEvent});
            this.tblEvent.EnableHeadersVisualStyles = false;
            this.tblEvent.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.tblEvent.Location = new System.Drawing.Point(3, 43);
            this.tblEvent.Name = "tblEvent";
            this.tblEvent.ReadOnly = true;
            this.tblEvent.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.tblEvent.RowHeadersVisible = false;
            this.tblEvent.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.tblEvent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tblEvent.Size = new System.Drawing.Size(702, 552);
            this.tblEvent.TabIndex = 3;
            this.tblEvent.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tblEvent_CellDoubleClick);
            // 
            // colStartTime
            // 
            this.colStartTime.DataPropertyName = "StartDate";
            this.colStartTime.HeaderText = "Bắt đầu";
            this.colStartTime.Name = "colStartTime";
            this.colStartTime.ReadOnly = true;
            // 
            // colEndTIme
            // 
            this.colEndTIme.DataPropertyName = "EndDate";
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
            this.btnEditFamilyTimeline.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditFamilyTimeline.Depth = 0;
            this.btnEditFamilyTimeline.DrawShadows = true;
            this.btnEditFamilyTimeline.ForeColor = System.Drawing.Color.White;
            this.btnEditFamilyTimeline.HighEmphasis = true;
            this.btnEditFamilyTimeline.Icon = global::GPMain.Properties.Resources.edit_icon;
            this.btnEditFamilyTimeline.Location = new System.Drawing.Point(661, 4);
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
            // tabAlbum
            // 
            this.tabAlbum.Location = new System.Drawing.Point(4, 22);
            this.tabAlbum.Name = "tabAlbum";
            this.tabAlbum.Size = new System.Drawing.Size(708, 597);
            this.tabAlbum.TabIndex = 3;
            this.tabAlbum.Text = "Album Ảnh";
            this.tabAlbum.UseVisualStyleBackColor = true;
            // 
            // tabDocument
            // 
            this.tabDocument.BackColor = System.Drawing.Color.White;
            this.tabDocument.Location = new System.Drawing.Point(4, 22);
            this.tabDocument.Name = "tabDocument";
            this.tabDocument.Size = new System.Drawing.Size(708, 597);
            this.tabDocument.TabIndex = 4;
            this.tabDocument.Text = "Tài liệu";
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.Controls.Add(this.txtPercentUnknown);
            this.materialCard1.Controls.Add(this.materialLabel26);
            this.materialCard1.Controls.Add(this.txtPercentFemale);
            this.materialCard1.Controls.Add(this.materialLabel14);
            this.materialCard1.Controls.Add(this.txtPercentMale);
            this.materialCard1.Controls.Add(this.materialLabel12);
            this.materialCard1.Controls.Add(this.txtPercentLiving);
            this.materialCard1.Controls.Add(this.materialLabel10);
            this.materialCard1.Controls.Add(this.txtPercentDear);
            this.materialCard1.Controls.Add(this.materialLabel9);
            this.materialCard1.Controls.Add(this.materialLabel7);
            this.materialCard1.Depth = 0;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(30, 214);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(7);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(463, 184);
            this.materialCard1.TabIndex = 67;
            // 
            // txtPercentUnknown
            // 
            this.txtPercentUnknown.AutoSize = true;
            this.txtPercentUnknown.Depth = 0;
            this.txtPercentUnknown.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPercentUnknown.Location = new System.Drawing.Point(275, 156);
            this.txtPercentUnknown.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtPercentUnknown.Name = "txtPercentUnknown";
            this.txtPercentUnknown.Size = new System.Drawing.Size(79, 19);
            this.txtPercentUnknown.TabIndex = 17;
            this.txtPercentUnknown.Text = "0 người (0)";
            // 
            // materialLabel26
            // 
            this.materialLabel26.AutoSize = true;
            this.materialLabel26.Depth = 0;
            this.materialLabel26.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel26.Location = new System.Drawing.Point(17, 156);
            this.materialLabel26.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel26.Name = "materialLabel26";
            this.materialLabel26.Size = new System.Drawing.Size(169, 19);
            this.materialLabel26.TabIndex = 16;
            this.materialLabel26.Text = "Chưa xác định giới tính:";
            // 
            // txtPercentFemale
            // 
            this.txtPercentFemale.AutoSize = true;
            this.txtPercentFemale.Depth = 0;
            this.txtPercentFemale.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPercentFemale.Location = new System.Drawing.Point(276, 129);
            this.txtPercentFemale.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtPercentFemale.Name = "txtPercentFemale";
            this.txtPercentFemale.Size = new System.Drawing.Size(79, 19);
            this.txtPercentFemale.TabIndex = 15;
            this.txtPercentFemale.Text = "0 người (0)";
            // 
            // materialLabel14
            // 
            this.materialLabel14.AutoSize = true;
            this.materialLabel14.Depth = 0;
            this.materialLabel14.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel14.Location = new System.Drawing.Point(17, 129);
            this.materialLabel14.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel14.Name = "materialLabel14";
            this.materialLabel14.Size = new System.Drawing.Size(107, 19);
            this.materialLabel14.TabIndex = 14;
            this.materialLabel14.Text = "Thành viên nữ:";
            // 
            // txtPercentMale
            // 
            this.txtPercentMale.AutoSize = true;
            this.txtPercentMale.Depth = 0;
            this.txtPercentMale.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPercentMale.Location = new System.Drawing.Point(276, 102);
            this.txtPercentMale.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtPercentMale.Name = "txtPercentMale";
            this.txtPercentMale.Size = new System.Drawing.Size(79, 19);
            this.txtPercentMale.TabIndex = 15;
            this.txtPercentMale.Text = "0 người (0)";
            // 
            // materialLabel12
            // 
            this.materialLabel12.AutoSize = true;
            this.materialLabel12.Depth = 0;
            this.materialLabel12.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel12.Location = new System.Drawing.Point(17, 102);
            this.materialLabel12.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel12.Name = "materialLabel12";
            this.materialLabel12.Size = new System.Drawing.Size(120, 19);
            this.materialLabel12.TabIndex = 14;
            this.materialLabel12.Text = "Thành viên nam:";
            // 
            // txtPercentLiving
            // 
            this.txtPercentLiving.AutoSize = true;
            this.txtPercentLiving.Depth = 0;
            this.txtPercentLiving.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPercentLiving.Location = new System.Drawing.Point(276, 75);
            this.txtPercentLiving.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtPercentLiving.Name = "txtPercentLiving";
            this.txtPercentLiving.Size = new System.Drawing.Size(79, 19);
            this.txtPercentLiving.TabIndex = 15;
            this.txtPercentLiving.Text = "0 người (0)";
            // 
            // materialLabel10
            // 
            this.materialLabel10.AutoSize = true;
            this.materialLabel10.Depth = 0;
            this.materialLabel10.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel10.Location = new System.Drawing.Point(17, 75);
            this.materialLabel10.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel10.Name = "materialLabel10";
            this.materialLabel10.Size = new System.Drawing.Size(72, 19);
            this.materialLabel10.TabIndex = 14;
            this.materialLabel10.Text = "Còn sống:";
            // 
            // txtPercentDear
            // 
            this.txtPercentDear.AutoSize = true;
            this.txtPercentDear.Depth = 0;
            this.txtPercentDear.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPercentDear.Location = new System.Drawing.Point(276, 48);
            this.txtPercentDear.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtPercentDear.Name = "txtPercentDear";
            this.txtPercentDear.Size = new System.Drawing.Size(79, 19);
            this.txtPercentDear.TabIndex = 15;
            this.txtPercentDear.Text = "0 người (0)";
            // 
            // materialLabel9
            // 
            this.materialLabel9.AutoSize = true;
            this.materialLabel9.Depth = 0;
            this.materialLabel9.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel9.Location = new System.Drawing.Point(17, 48);
            this.materialLabel9.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel9.Name = "materialLabel9";
            this.materialLabel9.Size = new System.Drawing.Size(57, 19);
            this.materialLabel9.TabIndex = 14;
            this.materialLabel9.Text = "Đã mất:";
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
            this.materialLabel7.Size = new System.Drawing.Size(309, 29);
            this.materialLabel7.TabIndex = 13;
            this.materialLabel7.Text = "Thống kê theo thành viên (%)";
            // 
            // materialCard2
            // 
            this.materialCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard2.Controls.Add(this.txtTotalMember);
            this.materialCard2.Controls.Add(this.materialLabel6);
            this.materialCard2.Controls.Add(this.txtUserCreated);
            this.materialCard2.Controls.Add(this.materialLabel5);
            this.materialCard2.Controls.Add(this.txtFamilyHometown);
            this.materialCard2.Controls.Add(this.materialLabel4);
            this.materialCard2.Controls.Add(this.materialLabel8);
            this.materialCard2.Controls.Add(this.txtFamilyLevel);
            this.materialCard2.Controls.Add(this.materialLabel3);
            this.materialCard2.Controls.Add(this.txtFamilyAnniversary);
            this.materialCard2.Controls.Add(this.materialLabel2);
            this.materialCard2.Controls.Add(this.txtFamilyName);
            this.materialCard2.Controls.Add(this.materialLabel1);
            this.materialCard2.Controls.Add(this.btnEditFamilyInfo);
            this.materialCard2.Depth = 0;
            this.materialCard2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard2.Location = new System.Drawing.Point(30, 17);
            this.materialCard2.Margin = new System.Windows.Forms.Padding(7);
            this.materialCard2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard2.Name = "materialCard2";
            this.materialCard2.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard2.Size = new System.Drawing.Size(463, 189);
            this.materialCard2.TabIndex = 67;
            // 
            // txtTotalMember
            // 
            this.txtTotalMember.Depth = 0;
            this.txtTotalMember.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtTotalMember.Location = new System.Drawing.Point(339, 160);
            this.txtTotalMember.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtTotalMember.Name = "txtTotalMember";
            this.txtTotalMember.Size = new System.Drawing.Size(100, 23);
            this.txtTotalMember.TabIndex = 14;
            this.txtTotalMember.Text = "123";
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
            this.txtUserCreated.Location = new System.Drawing.Point(156, 131);
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
            this.materialLabel5.Location = new System.Drawing.Point(19, 131);
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
            this.txtFamilyHometown.Location = new System.Drawing.Point(123, 104);
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
            this.materialLabel4.Location = new System.Drawing.Point(18, 104);
            this.materialLabel4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            this.materialLabel4.Size = new System.Drawing.Size(99, 19);
            this.materialLabel4.TabIndex = 9;
            this.materialLabel4.Text = "Nguyên quán:";
            // 
            // materialLabel8
            // 
            this.materialLabel8.AutoSize = true;
            this.materialLabel8.Depth = 0;
            this.materialLabel8.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel8.Location = new System.Drawing.Point(196, 160);
            this.materialLabel8.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel8.Name = "materialLabel8";
            this.materialLabel8.Size = new System.Drawing.Size(141, 19);
            this.materialLabel8.TabIndex = 7;
            this.materialLabel8.Text = "Tổng số thành viên:";
            // 
            // txtFamilyLevel
            // 
            this.txtFamilyLevel.AutoSize = true;
            this.txtFamilyLevel.Depth = 0;
            this.txtFamilyLevel.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtFamilyLevel.Location = new System.Drawing.Point(80, 160);
            this.txtFamilyLevel.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtFamilyLevel.Name = "txtFamilyLevel";
            this.txtFamilyLevel.Size = new System.Drawing.Size(28, 19);
            this.txtFamilyLevel.TabIndex = 8;
            this.txtFamilyLevel.Text = "123";
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel3.Location = new System.Drawing.Point(17, 160);
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
            this.txtFamilyAnniversary.Location = new System.Drawing.Point(93, 77);
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
            this.materialLabel2.Location = new System.Drawing.Point(19, 77);
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
            this.txtFamilyName.Location = new System.Drawing.Point(117, 50);
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
            this.materialLabel1.Location = new System.Drawing.Point(17, 50);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(94, 19);
            this.materialLabel1.TabIndex = 3;
            this.materialLabel1.Text = "Tên dòng họ:";
            // 
            // btnEditFamilyInfo
            // 
            this.btnEditFamilyInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnEditFamilyInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditFamilyInfo.Depth = 0;
            this.btnEditFamilyInfo.DrawShadows = true;
            this.btnEditFamilyInfo.ForeColor = System.Drawing.Color.White;
            this.btnEditFamilyInfo.HighEmphasis = true;
            this.btnEditFamilyInfo.Icon = global::GPMain.Properties.Resources.edit_icon;
            this.btnEditFamilyInfo.Location = new System.Drawing.Point(415, 3);
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
            // materialCard3
            // 
            this.materialCard3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard3.Controls.Add(this.txtAgeOver71);
            this.materialCard3.Controls.Add(this.materialLabel30);
            this.materialCard3.Controls.Add(this.txtAge5671);
            this.materialCard3.Controls.Add(this.materialLabel28);
            this.materialCard3.Controls.Add(this.txtAge3655);
            this.materialCard3.Controls.Add(this.materialLabel17);
            this.materialCard3.Controls.Add(this.txtAge1835);
            this.materialCard3.Controls.Add(this.materialLabel19);
            this.materialCard3.Controls.Add(this.txtAge617);
            this.materialCard3.Controls.Add(this.materialLabel21);
            this.materialCard3.Controls.Add(this.txtAge05);
            this.materialCard3.Controls.Add(this.materialLabel23);
            this.materialCard3.Controls.Add(this.materialLabel24);
            this.materialCard3.Depth = 0;
            this.materialCard3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard3.Location = new System.Drawing.Point(30, 406);
            this.materialCard3.Margin = new System.Windows.Forms.Padding(7);
            this.materialCard3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard3.Name = "materialCard3";
            this.materialCard3.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard3.Size = new System.Drawing.Size(463, 184);
            this.materialCard3.TabIndex = 67;
            // 
            // txtAgeOver71
            // 
            this.txtAgeOver71.AutoSize = true;
            this.txtAgeOver71.Depth = 0;
            this.txtAgeOver71.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAgeOver71.Location = new System.Drawing.Point(320, 86);
            this.txtAgeOver71.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtAgeOver71.Name = "txtAgeOver71";
            this.txtAgeOver71.Size = new System.Drawing.Size(91, 19);
            this.txtAgeOver71.TabIndex = 19;
            this.txtAgeOver71.Text = "0 người (0%)";
            // 
            // materialLabel30
            // 
            this.materialLabel30.AutoSize = true;
            this.materialLabel30.Depth = 0;
            this.materialLabel30.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel30.Location = new System.Drawing.Point(244, 86);
            this.materialLabel30.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel30.Name = "materialLabel30";
            this.materialLabel30.Size = new System.Drawing.Size(63, 19);
            this.materialLabel30.TabIndex = 18;
            this.materialLabel30.Text = "Trên  71:";
            // 
            // txtAge5671
            // 
            this.txtAge5671.AutoSize = true;
            this.txtAge5671.Depth = 0;
            this.txtAge5671.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAge5671.Location = new System.Drawing.Point(320, 51);
            this.txtAge5671.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtAge5671.Name = "txtAge5671";
            this.txtAge5671.Size = new System.Drawing.Size(91, 19);
            this.txtAge5671.TabIndex = 17;
            this.txtAge5671.Text = "0 người (0%)";
            // 
            // materialLabel28
            // 
            this.materialLabel28.AutoSize = true;
            this.materialLabel28.Depth = 0;
            this.materialLabel28.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel28.Location = new System.Drawing.Point(244, 51);
            this.materialLabel28.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel28.Name = "materialLabel28";
            this.materialLabel28.Size = new System.Drawing.Size(73, 19);
            this.materialLabel28.TabIndex = 16;
            this.materialLabel28.Text = "Từ  56-71:";
            // 
            // txtAge3655
            // 
            this.txtAge3655.AutoSize = true;
            this.txtAge3655.Depth = 0;
            this.txtAge3655.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAge3655.Location = new System.Drawing.Point(85, 156);
            this.txtAge3655.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtAge3655.Name = "txtAge3655";
            this.txtAge3655.Size = new System.Drawing.Size(91, 19);
            this.txtAge3655.TabIndex = 15;
            this.txtAge3655.Text = "0 người (0%)";
            // 
            // materialLabel17
            // 
            this.materialLabel17.AutoSize = true;
            this.materialLabel17.Depth = 0;
            this.materialLabel17.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel17.Location = new System.Drawing.Point(14, 156);
            this.materialLabel17.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel17.Name = "materialLabel17";
            this.materialLabel17.Size = new System.Drawing.Size(69, 19);
            this.materialLabel17.TabIndex = 14;
            this.materialLabel17.Text = "Từ 36-55:";
            // 
            // txtAge1835
            // 
            this.txtAge1835.AutoSize = true;
            this.txtAge1835.Depth = 0;
            this.txtAge1835.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAge1835.Location = new System.Drawing.Point(85, 121);
            this.txtAge1835.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtAge1835.Name = "txtAge1835";
            this.txtAge1835.Size = new System.Drawing.Size(91, 19);
            this.txtAge1835.TabIndex = 15;
            this.txtAge1835.Text = "0 người (0%)";
            // 
            // materialLabel19
            // 
            this.materialLabel19.AutoSize = true;
            this.materialLabel19.Depth = 0;
            this.materialLabel19.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel19.Location = new System.Drawing.Point(14, 121);
            this.materialLabel19.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel19.Name = "materialLabel19";
            this.materialLabel19.Size = new System.Drawing.Size(69, 19);
            this.materialLabel19.TabIndex = 14;
            this.materialLabel19.Text = "Từ 18-35:";
            // 
            // txtAge617
            // 
            this.txtAge617.AutoSize = true;
            this.txtAge617.Depth = 0;
            this.txtAge617.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAge617.Location = new System.Drawing.Point(85, 86);
            this.txtAge617.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtAge617.Name = "txtAge617";
            this.txtAge617.Size = new System.Drawing.Size(91, 19);
            this.txtAge617.TabIndex = 15;
            this.txtAge617.Text = "0 người (0%)";
            // 
            // materialLabel21
            // 
            this.materialLabel21.AutoSize = true;
            this.materialLabel21.Depth = 0;
            this.materialLabel21.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel21.Location = new System.Drawing.Point(14, 86);
            this.materialLabel21.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel21.Name = "materialLabel21";
            this.materialLabel21.Size = new System.Drawing.Size(60, 19);
            this.materialLabel21.TabIndex = 14;
            this.materialLabel21.Text = "Từ 6-17:";
            // 
            // txtAge05
            // 
            this.txtAge05.AutoSize = true;
            this.txtAge05.Depth = 0;
            this.txtAge05.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtAge05.Location = new System.Drawing.Point(85, 51);
            this.txtAge05.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtAge05.Name = "txtAge05";
            this.txtAge05.Size = new System.Drawing.Size(91, 19);
            this.txtAge05.TabIndex = 15;
            this.txtAge05.Text = "0 người (0%)";
            // 
            // materialLabel23
            // 
            this.materialLabel23.AutoSize = true;
            this.materialLabel23.Depth = 0;
            this.materialLabel23.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel23.Location = new System.Drawing.Point(14, 51);
            this.materialLabel23.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel23.Name = "materialLabel23";
            this.materialLabel23.Size = new System.Drawing.Size(51, 19);
            this.materialLabel23.TabIndex = 14;
            this.materialLabel23.Text = "Từ 0-5:";
            // 
            // materialLabel24
            // 
            this.materialLabel24.AutoSize = true;
            this.materialLabel24.Depth = 0;
            this.materialLabel24.Font = new System.Drawing.Font("Roboto", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel24.FontType = MaterialSkin.MaterialSkinManager.fontType.H5;
            this.materialLabel24.Location = new System.Drawing.Point(16, 14);
            this.materialLabel24.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel24.Name = "materialLabel24";
            this.materialLabel24.Size = new System.Drawing.Size(274, 29);
            this.materialLabel24.TabIndex = 13;
            this.materialLabel24.Text = "Thống kê theo độ tuổi (%)";
            // 
            // llb_lstBirthday
            // 
            this.llb_lstBirthday.AutoSize = true;
            this.llb_lstBirthday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.llb_lstBirthday.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llb_lstBirthday.Location = new System.Drawing.Point(25, 600);
            this.llb_lstBirthday.Name = "llb_lstBirthday";
            this.llb_lstBirthday.Size = new System.Drawing.Size(222, 20);
            this.llb_lstBirthday.TabIndex = 72;
            this.llb_lstBirthday.TabStop = true;
            this.llb_lstBirthday.Text = "Danh sách sinh nhật gần nhất";
            this.llb_lstBirthday.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llb_lstBirthday_LinkClicked);
            // 
            // llb_lstDeadDay
            // 
            this.llb_lstDeadDay.AutoSize = true;
            this.llb_lstDeadDay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.llb_lstDeadDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llb_lstDeadDay.Location = new System.Drawing.Point(25, 629);
            this.llb_lstDeadDay.Name = "llb_lstDeadDay";
            this.llb_lstDeadDay.Size = new System.Drawing.Size(216, 20);
            this.llb_lstDeadDay.TabIndex = 73;
            this.llb_lstDeadDay.TabStop = true;
            this.llb_lstDeadDay.Text = "Danh sách ngày giỗ gần nhất";
            this.llb_lstDeadDay.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llb_lstDeadDay_LinkClicked);
            // 
            // colDelAction
            // 
            this.colDelAction.HeaderText = "Xóa";
            this.colDelAction.Name = "colDelAction";
            this.colDelAction.ReadOnly = true;
            this.colDelAction.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // FamilyInfoFullPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.llb_lstDeadDay);
            this.Controls.Add(this.llb_lstBirthday);
            this.Controls.Add(this.tabFamilyAlbum);
            this.Controls.Add(this.materialTabSelector1);
            this.Controls.Add(this.materialCard3);
            this.Controls.Add(this.materialCard1);
            this.Controls.Add(this.materialCard2);
            this.Name = "FamilyInfoFullPage";
            this.Size = new System.Drawing.Size(1244, 720);
            this.tabFamilyAlbum.ResumeLayout(false);
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFamilyHead)).EndInit();
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tblEvent)).EndInit();
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            this.materialCard2.ResumeLayout(false);
            this.materialCard2.PerformLayout();
            this.materialCard3.ResumeLayout(false);
            this.materialCard3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialCard materialCard2;
        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private MaterialSkin.Controls.MaterialButton btnEditFamilyInfo;
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
        private MaterialSkin.Controls.MaterialLabel txtPercentFemale;
        private MaterialSkin.Controls.MaterialLabel materialLabel14;
        private MaterialSkin.Controls.MaterialLabel txtPercentMale;
        private MaterialSkin.Controls.MaterialLabel materialLabel12;
        private MaterialSkin.Controls.MaterialLabel txtPercentLiving;
        private MaterialSkin.Controls.MaterialLabel materialLabel10;
        private MaterialSkin.Controls.MaterialLabel txtPercentDear;
        private MaterialSkin.Controls.MaterialLabel materialLabel9;
        private MaterialSkin.Controls.MaterialCard materialCard3;
        private MaterialSkin.Controls.MaterialLabel txtAge3655;
        private MaterialSkin.Controls.MaterialLabel materialLabel17;
        private MaterialSkin.Controls.MaterialLabel txtAge1835;
        private MaterialSkin.Controls.MaterialLabel materialLabel19;
        private MaterialSkin.Controls.MaterialLabel txtAge617;
        private MaterialSkin.Controls.MaterialLabel materialLabel21;
        private MaterialSkin.Controls.MaterialLabel txtAge05;
        private MaterialSkin.Controls.MaterialLabel materialLabel23;
        private MaterialSkin.Controls.MaterialLabel materialLabel24;
        private MaterialSkin.Controls.MaterialLabel txtPercentUnknown;
        private MaterialSkin.Controls.MaterialLabel materialLabel26;
        private MaterialSkin.Controls.MaterialLabel txtAgeOver71;
        private MaterialSkin.Controls.MaterialLabel materialLabel30;
        private MaterialSkin.Controls.MaterialLabel txtAge5671;
        private MaterialSkin.Controls.MaterialLabel materialLabel28;
        private MaterialSkin.Controls.MaterialLabel materialLabel8;
        private MaterialSkin.Controls.MaterialLabel txtTotalMember;
        private System.Windows.Forms.LinkLabel llb_lstBirthday;
        private System.Windows.Forms.LinkLabel llb_lstDeadDay;
        private System.Windows.Forms.DataGridViewLinkColumn colDelAction;
        private MaterialSkin.Controls.MaterialTabControl tabFamilyAlbum;
        private System.Windows.Forms.TabPage tabPage8;
        private Controls.DataGridTemplate gridFamilyHead;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBirthday;
        private MaterialSkin.Controls.MaterialButton btnAddFamilyHead;
        private System.Windows.Forms.TabPage tabPage9;
        private Controls.DataGridTemplate tblEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEndTIme;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEvent;
        private MaterialSkin.Controls.MaterialButton btnEditFamilyTimeline;
        private System.Windows.Forms.TabPage tabAlbum;
        private System.Windows.Forms.TabPage tabDocument;
    }
}
