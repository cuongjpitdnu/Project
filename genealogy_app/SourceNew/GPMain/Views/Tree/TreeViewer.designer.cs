namespace GPMain.Views.Tree
{
    partial class TreeViewer
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
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.btnTreeView = new MaterialSkin.Controls.MaterialButton();
            this.btnBuildTree = new MaterialSkin.Controls.MaterialButton();
            this.plnMain = new System.Windows.Forms.Panel();
            this.btnshowmenumember = new MaterialSkin.Controls.MaterialButton();
            this.btnEditTree = new MaterialSkin.Controls.MaterialButton();
            this.plnControl = new System.Windows.Forms.Panel();
            this.materialButton5 = new MaterialSkin.Controls.MaterialButton();
            this.materialButton4 = new MaterialSkin.Controls.MaterialButton();
            this.btnCenterRoot = new MaterialSkin.Controls.MaterialButton();
            this.cboTypeTree = new MaterialSkin.Controls.MaterialComboBox();
            this.plnConfigTree = new System.Windows.Forms.Panel();
            this.menuConfig = new GPMain.Views.Controls.MenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCloseMenuConfig = new MaterialSkin.Controls.MaterialButton();
            this.btnSaveTheme = new MaterialSkin.Controls.MaterialButton();
            this.cboThemes = new MaterialSkin.Controls.MaterialComboBox();
            this.plnTree = new System.Windows.Forms.Panel();
            this.plnListMember = new System.Windows.Forms.Panel();
            this.btnhidemenumember = new System.Windows.Forms.Button();
            this.lblTitle = new MaterialSkin.Controls.MaterialLabel();
            this.menuMember1 = new GPMain.Views.Controls.MenuMember();
            this.MainLayout.SuspendLayout();
            this.plnMain.SuspendLayout();
            this.plnControl.SuspendLayout();
            this.plnConfigTree.SuspendLayout();
            this.panel1.SuspendLayout();
            this.plnListMember.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 3;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainLayout.Controls.Add(this.btnTreeView, 2, 0);
            this.MainLayout.Controls.Add(this.btnBuildTree, 1, 0);
            this.MainLayout.Controls.Add(this.plnMain, 1, 1);
            this.MainLayout.Controls.Add(this.plnListMember, 0, 0);
            this.MainLayout.Controls.Add(this.menuMember1, 0, 1);
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 0);
            this.MainLayout.Margin = new System.Windows.Forms.Padding(0);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 2;
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.Size = new System.Drawing.Size(986, 477);
            this.MainLayout.TabIndex = 0;
            // 
            // btnTreeView
            // 
            this.btnTreeView.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnTreeView.Depth = 0;
            this.btnTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTreeView.DrawShadows = true;
            this.btnTreeView.HighEmphasis = true;
            this.btnTreeView.Icon = null;
            this.btnTreeView.Location = new System.Drawing.Point(624, 1);
            this.btnTreeView.Margin = new System.Windows.Forms.Padding(1);
            this.btnTreeView.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnTreeView.Name = "btnTreeView";
            this.btnTreeView.Size = new System.Drawing.Size(361, 28);
            this.btnTreeView.TabIndex = 1;
            this.btnTreeView.Text = "Gia phả";
            this.btnTreeView.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnTreeView.UseAccentColor = true;
            this.btnTreeView.UseVisualStyleBackColor = true;
            this.btnTreeView.Click += new System.EventHandler(this.BtnTreeView_Click);
            // 
            // btnBuildTree
            // 
            this.btnBuildTree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnBuildTree.Depth = 0;
            this.btnBuildTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBuildTree.DrawShadows = true;
            this.btnBuildTree.HighEmphasis = true;
            this.btnBuildTree.Icon = null;
            this.btnBuildTree.Location = new System.Drawing.Point(261, 1);
            this.btnBuildTree.Margin = new System.Windows.Forms.Padding(1);
            this.btnBuildTree.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnBuildTree.Name = "btnBuildTree";
            this.btnBuildTree.Size = new System.Drawing.Size(361, 28);
            this.btnBuildTree.TabIndex = 0;
            this.btnBuildTree.Text = "Xây dựng phả hệ";
            this.btnBuildTree.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnBuildTree.UseAccentColor = true;
            this.btnBuildTree.UseVisualStyleBackColor = true;
            this.btnBuildTree.Click += new System.EventHandler(this.BtnBuildTree_Click);
            // 
            // plnMain
            // 
            this.plnMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.plnMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.MainLayout.SetColumnSpan(this.plnMain, 2);
            this.plnMain.Controls.Add(this.btnshowmenumember);
            this.plnMain.Controls.Add(this.btnEditTree);
            this.plnMain.Controls.Add(this.plnControl);
            this.plnMain.Controls.Add(this.plnConfigTree);
            this.plnMain.Controls.Add(this.plnTree);
            this.plnMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plnMain.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.plnMain.Location = new System.Drawing.Point(261, 31);
            this.plnMain.Margin = new System.Windows.Forms.Padding(1);
            this.plnMain.Name = "plnMain";
            this.plnMain.Size = new System.Drawing.Size(724, 445);
            this.plnMain.TabIndex = 2;
            // 
            // btnshowmenumember
            // 
            this.btnshowmenumember.AutoSize = false;
            this.btnshowmenumember.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnshowmenumember.BackColor = System.Drawing.Color.Transparent;
            this.btnshowmenumember.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnshowmenumember.Depth = 0;
            this.btnshowmenumember.DrawShadows = true;
            this.btnshowmenumember.HighEmphasis = true;
            this.btnshowmenumember.Icon = null;
            this.btnshowmenumember.Location = new System.Drawing.Point(1, 1);
            this.btnshowmenumember.Margin = new System.Windows.Forms.Padding(0);
            this.btnshowmenumember.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnshowmenumember.Name = "btnshowmenumember";
            this.btnshowmenumember.Size = new System.Drawing.Size(43, 23);
            this.btnshowmenumember.TabIndex = 2;
            this.btnshowmenumember.Text = "Menu";
            this.btnshowmenumember.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.btnshowmenumember.UseAccentColor = true;
            this.btnshowmenumember.UseVisualStyleBackColor = false;
            this.btnshowmenumember.Visible = false;
            this.btnshowmenumember.Click += new System.EventHandler(this.Btnshowmenumember_Click);
            // 
            // btnEditTree
            // 
            this.btnEditTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditTree.AutoSize = false;
            this.btnEditTree.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnEditTree.BackColor = System.Drawing.Color.Transparent;
            this.btnEditTree.Depth = 0;
            this.btnEditTree.DrawShadows = true;
            this.btnEditTree.HighEmphasis = true;
            this.btnEditTree.Icon = null;
            this.btnEditTree.Location = new System.Drawing.Point(464, 1);
            this.btnEditTree.Margin = new System.Windows.Forms.Padding(0);
            this.btnEditTree.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnEditTree.Name = "btnEditTree";
            this.btnEditTree.Size = new System.Drawing.Size(43, 23);
            this.btnEditTree.TabIndex = 0;
            this.btnEditTree.Text = "Edit";
            this.btnEditTree.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.btnEditTree.UseAccentColor = true;
            this.btnEditTree.UseVisualStyleBackColor = false;
            this.btnEditTree.Click += new System.EventHandler(this.BtnEditTree_Click);
            // 
            // plnControl
            // 
            this.plnControl.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.plnControl.BackColor = System.Drawing.SystemColors.Control;
            this.plnControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plnControl.Controls.Add(this.materialButton5);
            this.plnControl.Controls.Add(this.materialButton4);
            this.plnControl.Controls.Add(this.btnCenterRoot);
            this.plnControl.Controls.Add(this.cboTypeTree);
            this.plnControl.Location = new System.Drawing.Point(125, 399);
            this.plnControl.Margin = new System.Windows.Forms.Padding(0);
            this.plnControl.Name = "plnControl";
            this.plnControl.Size = new System.Drawing.Size(446, 43);
            this.plnControl.TabIndex = 0;
            // 
            // materialButton5
            // 
            this.materialButton5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton5.Depth = 0;
            this.materialButton5.DrawShadows = true;
            this.materialButton5.HighEmphasis = true;
            this.materialButton5.Icon = null;
            this.materialButton5.Location = new System.Drawing.Point(379, 3);
            this.materialButton5.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton5.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton5.Name = "materialButton5";
            this.materialButton5.Size = new System.Drawing.Size(48, 36);
            this.materialButton5.TabIndex = 1;
            this.materialButton5.Text = "PDF";
            this.materialButton5.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.materialButton5.UseAccentColor = true;
            this.materialButton5.UseVisualStyleBackColor = true;
            this.materialButton5.Click += new System.EventHandler(this.BtnnExportPDF_Click);
            // 
            // materialButton4
            // 
            this.materialButton4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton4.Depth = 0;
            this.materialButton4.DrawShadows = true;
            this.materialButton4.HighEmphasis = true;
            this.materialButton4.Icon = null;
            this.materialButton4.Location = new System.Drawing.Point(321, 4);
            this.materialButton4.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton4.Name = "materialButton4";
            this.materialButton4.Size = new System.Drawing.Size(48, 36);
            this.materialButton4.TabIndex = 1;
            this.materialButton4.Text = "SVG";
            this.materialButton4.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.materialButton4.UseAccentColor = true;
            this.materialButton4.UseVisualStyleBackColor = true;
            this.materialButton4.Click += new System.EventHandler(this.BtnSVG_Click);
            // 
            // btnCenterRoot
            // 
            this.btnCenterRoot.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCenterRoot.Depth = 0;
            this.btnCenterRoot.DrawShadows = true;
            this.btnCenterRoot.HighEmphasis = true;
            this.btnCenterRoot.Icon = null;
            this.btnCenterRoot.Location = new System.Drawing.Point(195, 3);
            this.btnCenterRoot.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnCenterRoot.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnCenterRoot.Name = "btnCenterRoot";
            this.btnCenterRoot.Size = new System.Drawing.Size(70, 36);
            this.btnCenterRoot.TabIndex = 1;
            this.btnCenterRoot.Text = "Về gốc";
            this.btnCenterRoot.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.btnCenterRoot.UseAccentColor = true;
            this.btnCenterRoot.UseVisualStyleBackColor = true;
            this.btnCenterRoot.Click += new System.EventHandler(this.BtnCenterRoot_Click);
            // 
            // cboTypeTree
            // 
            this.cboTypeTree.AutoResize = false;
            this.cboTypeTree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cboTypeTree.Depth = 0;
            this.cboTypeTree.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboTypeTree.DropDownHeight = 118;
            this.cboTypeTree.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTypeTree.DropDownWidth = 121;
            this.cboTypeTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cboTypeTree.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboTypeTree.FormattingEnabled = true;
            this.cboTypeTree.Hint = "Chọn kiểu cây";
            this.cboTypeTree.IntegralHeight = false;
            this.cboTypeTree.ItemHeight = 29;
            this.cboTypeTree.Location = new System.Drawing.Point(18, 4);
            this.cboTypeTree.MaxDropDownItems = 4;
            this.cboTypeTree.MouseState = MaterialSkin.MouseState.OUT;
            this.cboTypeTree.Name = "cboTypeTree";
            this.cboTypeTree.Size = new System.Drawing.Size(170, 35);
            this.cboTypeTree.TabIndex = 0;
            this.cboTypeTree.UseTallSize = false;
            this.cboTypeTree.SelectedIndexChanged += new System.EventHandler(this.CboTypeTree_SelectedIndexChanged);
            // 
            // plnConfigTree
            // 
            this.plnConfigTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plnConfigTree.BackColor = System.Drawing.SystemColors.Control;
            this.plnConfigTree.Controls.Add(this.menuConfig);
            this.plnConfigTree.Controls.Add(this.panel1);
            this.plnConfigTree.Location = new System.Drawing.Point(510, 0);
            this.plnConfigTree.Margin = new System.Windows.Forms.Padding(0);
            this.plnConfigTree.Name = "plnConfigTree";
            this.plnConfigTree.Size = new System.Drawing.Size(212, 443);
            this.plnConfigTree.TabIndex = 1;
            // 
            // menuConfig
            // 
            this.menuConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.menuConfig.BackColor = System.Drawing.SystemColors.Control;
            this.menuConfig.Location = new System.Drawing.Point(0, 70);
            this.menuConfig.Margin = new System.Windows.Forms.Padding(0);
            this.menuConfig.Name = "menuConfig";
            this.menuConfig.Size = new System.Drawing.Size(212, 375);
            this.menuConfig.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCloseMenuConfig);
            this.panel1.Controls.Add(this.btnSaveTheme);
            this.panel1.Controls.Add(this.cboThemes);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(212, 70);
            this.panel1.TabIndex = 0;
            // 
            // btnCloseMenuConfig
            // 
            this.btnCloseMenuConfig.AutoSize = false;
            this.btnCloseMenuConfig.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnCloseMenuConfig.Depth = 0;
            this.btnCloseMenuConfig.DrawShadows = true;
            this.btnCloseMenuConfig.HighEmphasis = true;
            this.btnCloseMenuConfig.Icon = null;
            this.btnCloseMenuConfig.Location = new System.Drawing.Point(106, 42);
            this.btnCloseMenuConfig.Margin = new System.Windows.Forms.Padding(0);
            this.btnCloseMenuConfig.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnCloseMenuConfig.Name = "btnCloseMenuConfig";
            this.btnCloseMenuConfig.Size = new System.Drawing.Size(102, 23);
            this.btnCloseMenuConfig.TabIndex = 1;
            this.btnCloseMenuConfig.Text = "Đóng";
            this.btnCloseMenuConfig.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnCloseMenuConfig.UseAccentColor = true;
            this.btnCloseMenuConfig.UseVisualStyleBackColor = true;
            this.btnCloseMenuConfig.Click += new System.EventHandler(this.BtnCloseMenuConfig_Click);
            // 
            // btnSaveTheme
            // 
            this.btnSaveTheme.AutoSize = false;
            this.btnSaveTheme.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSaveTheme.Depth = 0;
            this.btnSaveTheme.DrawShadows = true;
            this.btnSaveTheme.HighEmphasis = true;
            this.btnSaveTheme.Icon = null;
            this.btnSaveTheme.Location = new System.Drawing.Point(2, 42);
            this.btnSaveTheme.Margin = new System.Windows.Forms.Padding(0);
            this.btnSaveTheme.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSaveTheme.Name = "btnSaveTheme";
            this.btnSaveTheme.Size = new System.Drawing.Size(103, 23);
            this.btnSaveTheme.TabIndex = 1;
            this.btnSaveTheme.Text = "Lưu";
            this.btnSaveTheme.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnSaveTheme.UseAccentColor = true;
            this.btnSaveTheme.UseVisualStyleBackColor = true;
            this.btnSaveTheme.Click += new System.EventHandler(this.BtnSaveTheme_Click);
            // 
            // cboThemes
            // 
            this.cboThemes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboThemes.AutoResize = false;
            this.cboThemes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cboThemes.Depth = 0;
            this.cboThemes.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboThemes.DropDownHeight = 118;
            this.cboThemes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboThemes.DropDownWidth = 121;
            this.cboThemes.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cboThemes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cboThemes.FormattingEnabled = true;
            this.cboThemes.IntegralHeight = false;
            this.cboThemes.ItemHeight = 29;
            this.cboThemes.Location = new System.Drawing.Point(2, 6);
            this.cboThemes.MaxDropDownItems = 4;
            this.cboThemes.MouseState = MaterialSkin.MouseState.OUT;
            this.cboThemes.Name = "cboThemes";
            this.cboThemes.Size = new System.Drawing.Size(206, 35);
            this.cboThemes.TabIndex = 0;
            this.cboThemes.UseTallSize = false;
            this.cboThemes.SelectedIndexChanged += new System.EventHandler(this.CboThemes_SelectedIndexChanged);
            // 
            // plnTree
            // 
            this.plnTree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.plnTree.Location = new System.Drawing.Point(0, 0);
            this.plnTree.Name = "plnTree";
            this.plnTree.Size = new System.Drawing.Size(314, 138);
            this.plnTree.TabIndex = 1;
            // 
            // plnListMember
            // 
            this.plnListMember.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(244)))), ((int)(((byte)(246)))));
            this.plnListMember.Controls.Add(this.btnhidemenumember);
            this.plnListMember.Controls.Add(this.lblTitle);
            this.plnListMember.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plnListMember.Location = new System.Drawing.Point(0, 0);
            this.plnListMember.Margin = new System.Windows.Forms.Padding(0);
            this.plnListMember.Name = "plnListMember";
            this.plnListMember.Size = new System.Drawing.Size(260, 30);
            this.plnListMember.TabIndex = 0;
            // 
            // btnhidemenumember
            // 
            this.btnhidemenumember.BackgroundImage = global::GPMain.Properties.Resources.hidden;
            this.btnhidemenumember.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnhidemenumember.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnhidemenumember.FlatAppearance.BorderSize = 0;
            this.btnhidemenumember.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnhidemenumember.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnhidemenumember.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnhidemenumember.Location = new System.Drawing.Point(231, 3);
            this.btnhidemenumember.Name = "btnhidemenumember";
            this.btnhidemenumember.Size = new System.Drawing.Size(24, 24);
            this.btnhidemenumember.TabIndex = 3;
            this.btnhidemenumember.UseVisualStyleBackColor = true;
            this.btnhidemenumember.Click += new System.EventHandler(this.Btnhidemenumember_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTitle.AutoSize = true;
            this.lblTitle.Depth = 0;
            this.lblTitle.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblTitle.FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle1;
            this.lblTitle.HighEmphasis = true;
            this.lblTitle.Location = new System.Drawing.Point(38, 6);
            this.lblTitle.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(184, 19);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "DANH SÁCH THÀNH VIÊN";
            this.lblTitle.UseAccent = true;
            // 
            // menuMember1
            // 
            this.menuMember1.BackColor = System.Drawing.SystemColors.Control;
            this.menuMember1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuMember1.Location = new System.Drawing.Point(3, 33);
            this.menuMember1.Name = "menuMember1";
            this.menuMember1.Size = new System.Drawing.Size(254, 441);
            this.menuMember1.TabIndex = 3;
            // 
            // TreeViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainLayout);
            this.Name = "TreeViewer";
            this.Size = new System.Drawing.Size(986, 477);
            this.Load += new System.EventHandler(this.TreeViewer_Load);
            this.MainLayout.ResumeLayout(false);
            this.MainLayout.PerformLayout();
            this.plnMain.ResumeLayout(false);
            this.plnControl.ResumeLayout(false);
            this.plnControl.PerformLayout();
            this.plnConfigTree.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.plnListMember.ResumeLayout(false);
            this.plnListMember.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainLayout;
        private MaterialSkin.Controls.MaterialButton btnTreeView;
        private MaterialSkin.Controls.MaterialButton btnBuildTree;
        private System.Windows.Forms.Panel plnMain;
        private System.Windows.Forms.Panel plnControl;
        private MaterialSkin.Controls.MaterialComboBox cboTypeTree;
        private MaterialSkin.Controls.MaterialButton btnCenterRoot;
        private MaterialSkin.Controls.MaterialButton materialButton5;
        private MaterialSkin.Controls.MaterialButton materialButton4;
        private System.Windows.Forms.Panel plnTree;

        private MaterialSkin.Controls.MaterialButton btnEditTree;

        private System.Windows.Forms.Panel plnListMember;
        private MaterialSkin.Controls.MaterialLabel lblTitle;
        private Controls.MenuMember menuMember1;
        private System.Windows.Forms.Panel plnConfigTree;
        private Controls.MenuItem menuConfig;
        private System.Windows.Forms.Panel panel1;
        private MaterialSkin.Controls.MaterialComboBox cboThemes;
        private MaterialSkin.Controls.MaterialButton btnCloseMenuConfig;
        private MaterialSkin.Controls.MaterialButton btnSaveTheme;
        private MaterialSkin.Controls.MaterialButton btnshowmenumember;
        private System.Windows.Forms.Button btnhidemenumember;
    }
}
