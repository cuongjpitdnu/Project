namespace GP40Main.Themes
{
    partial class MenuForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuForm));
            this.tabMain = new MaterialSkin.Controls.MaterialTabControl();
            this.tabTree = new System.Windows.Forms.TabPage();
            this.tabFamily = new System.Windows.Forms.TabPage();
            this.tabMember = new System.Windows.Forms.TabPage();
            this.tabConfig = new System.Windows.Forms.TabPage();
            this.tabInfo = new System.Windows.Forms.TabPage();
            this.imgListMenu = new System.Windows.Forms.ImageList(this.components);
            this.tabMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabTree);
            this.tabMain.Controls.Add(this.tabFamily);
            this.tabMain.Controls.Add(this.tabMember);
            this.tabMain.Controls.Add(this.tabConfig);
            this.tabMain.Controls.Add(this.tabInfo);
            this.tabMain.Depth = 0;
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.ImageList = this.imgListMenu;
            this.tabMain.Location = new System.Drawing.Point(1, 64);
            this.tabMain.Margin = new System.Windows.Forms.Padding(0);
            this.tabMain.MouseState = MaterialSkin.MouseState.HOVER;
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1274, 661);
            this.tabMain.TabIndex = 0;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
            // 
            // tabTree
            // 
            this.tabTree.BackColor = System.Drawing.Color.White;
            this.tabTree.ImageKey = "menu-tree.png";
            this.tabTree.Location = new System.Drawing.Point(4, 39);
            this.tabTree.Name = "tabTree";
            this.tabTree.Size = new System.Drawing.Size(1266, 618);
            this.tabTree.TabIndex = 0;
            this.tabTree.Text = "Phả đồ dòng họ";
            this.tabTree.ToolTipText = "Phả đồ dòng họ";
            // 
            // tabFamily
            // 
            this.tabFamily.BackColor = System.Drawing.Color.White;
            this.tabFamily.ImageKey = "menu-family.png";
            this.tabFamily.Location = new System.Drawing.Point(4, 39);
            this.tabFamily.Margin = new System.Windows.Forms.Padding(0);
            this.tabFamily.Name = "tabFamily";
            this.tabFamily.Size = new System.Drawing.Size(990, 618);
            this.tabFamily.TabIndex = 2;
            this.tabFamily.Text = "Thông tin dòng họ";
            this.tabFamily.ToolTipText = "Dòng họ";
            // 
            // tabMember
            // 
            this.tabMember.BackColor = System.Drawing.Color.White;
            this.tabMember.ImageKey = "menu-member.png";
            this.tabMember.Location = new System.Drawing.Point(4, 39);
            this.tabMember.Name = "tabMember";
            this.tabMember.Size = new System.Drawing.Size(990, 618);
            this.tabMember.TabIndex = 1;
            this.tabMember.Text = "Danh sách thành viên";
            this.tabMember.ToolTipText = "Thành viên";
            // 
            // tabConfig
            // 
            this.tabConfig.BackColor = System.Drawing.Color.White;
            this.tabConfig.ImageKey = "menu-settings.png";
            this.tabConfig.Location = new System.Drawing.Point(4, 39);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Size = new System.Drawing.Size(990, 618);
            this.tabConfig.TabIndex = 3;
            this.tabConfig.Text = "Cấu hình";
            this.tabConfig.ToolTipText = "Cấu hình";
            // 
            // tabInfo
            // 
            this.tabInfo.BackColor = System.Drawing.Color.White;
            this.tabInfo.ImageKey = "menu-support.png";
            this.tabInfo.Location = new System.Drawing.Point(4, 39);
            this.tabInfo.Name = "tabInfo";
            this.tabInfo.Size = new System.Drawing.Size(990, 618);
            this.tabInfo.TabIndex = 5;
            this.tabInfo.Text = "Thông tin - Hỗ trợ";
            this.tabInfo.ToolTipText = "Thông tin - Hỗ trợ";
            // 
            // imgListMenu
            // 
            this.imgListMenu.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListMenu.ImageStream")));
            this.imgListMenu.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListMenu.Images.SetKeyName(0, "menu-member.png");
            this.imgListMenu.Images.SetKeyName(1, "menu-tree.png");
            this.imgListMenu.Images.SetKeyName(2, "menu-family.png");
            this.imgListMenu.Images.SetKeyName(3, "menu-support.png");
            this.imgListMenu.Images.SetKeyName(4, "menu-settings.png");
            // 
            // MenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.ClientSize = new System.Drawing.Size(1276, 726);
            this.Controls.Add(this.tabMain);
            this.DrawerAutoHide = false;
            this.DrawerBackgroundWithAccent = true;
            this.DrawerIndicatorWidth = 3;
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.tabMain;
            this.DrawerWidth = 250;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MsgComfirmExit = "Bạn có chắc muốn thoát phần mềm?";
            this.Name = "MenuForm";
            this.Padding = new System.Windows.Forms.Padding(1, 64, 1, 1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MenuForm";
            this.Load += new System.EventHandler(this.MenuForm_Load);
            this.tabMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabControl tabMain;
        private System.Windows.Forms.TabPage tabTree;
        private System.Windows.Forms.TabPage tabMember;
        private System.Windows.Forms.TabPage tabFamily;
        private System.Windows.Forms.TabPage tabConfig;
        private System.Windows.Forms.TabPage tabInfo;
        private System.Windows.Forms.ImageList imgListMenu;
    }
}