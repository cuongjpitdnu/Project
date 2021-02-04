namespace GPMain.Views
{
    partial class ListMember
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_addEmployee = new MaterialSkin.Controls.MaterialButton();
            this.btnSearch = new MaterialSkin.Controls.MaterialButton();
            this.chkboxInClan = new MaterialSkin.Controls.MaterialCheckbox();
            this.cmbLiveOrDie = new MaterialSkin.Controls.MaterialComboBox();
            this.cmbGender = new MaterialSkin.Controls.MaterialComboBox();
            this.txtKeyword = new MaterialSkin.Controls.MaterialTextBox();
            this.btn_exportEmployee = new MaterialSkin.Controls.MaterialButton();
            this.plnListMember = new System.Windows.Forms.Panel();
            this.MainLayout.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 1;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.Controls.Add(this.panel1, 0, 0);
            this.MainLayout.Controls.Add(this.plnListMember, 0, 1);
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 0);
            this.MainLayout.Margin = new System.Windows.Forms.Padding(0);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 2;
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.Size = new System.Drawing.Size(1234, 663);
            this.MainLayout.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_addEmployee);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.chkboxInClan);
            this.panel1.Controls.Add(this.cmbLiveOrDie);
            this.panel1.Controls.Add(this.cmbGender);
            this.panel1.Controls.Add(this.txtKeyword);
            this.panel1.Controls.Add(this.btn_exportEmployee);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1234, 60);
            this.panel1.TabIndex = 0;
            // 
            // btn_addEmployee
            // 
            this.btn_addEmployee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_addEmployee.AutoSize = false;
            this.btn_addEmployee.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_addEmployee.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_addEmployee.Depth = 0;
            this.btn_addEmployee.DrawShadows = true;
            this.btn_addEmployee.HighEmphasis = true;
            this.btn_addEmployee.Icon = null;
            this.btn_addEmployee.Location = new System.Drawing.Point(1084, 12);
            this.btn_addEmployee.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_addEmployee.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_addEmployee.Name = "btn_addEmployee";
            this.btn_addEmployee.Size = new System.Drawing.Size(146, 36);
            this.btn_addEmployee.TabIndex = 0;
            this.btn_addEmployee.Text = "Thêm thành viên";
            this.btn_addEmployee.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btn_addEmployee.UseAccentColor = true;
            this.btn_addEmployee.UseVisualStyleBackColor = true;
            this.btn_addEmployee.Click += new System.EventHandler(this.btn_addEmployee_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.AutoSize = false;
            this.btnSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Depth = 0;
            this.btnSearch.DrawShadows = true;
            this.btnSearch.HighEmphasis = true;
            this.btnSearch.Icon = null;
            this.btnSearch.Location = new System.Drawing.Point(854, 13);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSearch.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(86, 36);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnSearch.UseAccentColor = true;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // chkboxInClan
            // 
            this.chkboxInClan.AutoSize = true;
            this.chkboxInClan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chkboxInClan.Depth = 0;
            this.chkboxInClan.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chkboxInClan.Location = new System.Drawing.Point(602, 12);
            this.chkboxInClan.Margin = new System.Windows.Forms.Padding(0);
            this.chkboxInClan.MouseLocation = new System.Drawing.Point(-1, -1);
            this.chkboxInClan.MouseState = MaterialSkin.MouseState.HOVER;
            this.chkboxInClan.Name = "chkboxInClan";
            this.chkboxInClan.Ripple = true;
            this.chkboxInClan.Size = new System.Drawing.Size(139, 37);
            this.chkboxInClan.TabIndex = 4;
            this.chkboxInClan.Text = "Trong dòng họ";
            this.chkboxInClan.UseVisualStyleBackColor = true;
            // 
            // cmbLiveOrDie
            // 
            this.cmbLiveOrDie.AutoResize = false;
            this.cmbLiveOrDie.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cmbLiveOrDie.Depth = 0;
            this.cmbLiveOrDie.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbLiveOrDie.DropDownHeight = 118;
            this.cmbLiveOrDie.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLiveOrDie.DropDownWidth = 121;
            this.cmbLiveOrDie.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cmbLiveOrDie.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbLiveOrDie.FormattingEnabled = true;
            this.cmbLiveOrDie.Hint = "Trạng thái";
            this.cmbLiveOrDie.IntegralHeight = false;
            this.cmbLiveOrDie.ItemHeight = 29;
            this.cmbLiveOrDie.Location = new System.Drawing.Point(407, 13);
            this.cmbLiveOrDie.MaxDropDownItems = 4;
            this.cmbLiveOrDie.MouseState = MaterialSkin.MouseState.OUT;
            this.cmbLiveOrDie.Name = "cmbLiveOrDie";
            this.cmbLiveOrDie.Size = new System.Drawing.Size(187, 35);
            this.cmbLiveOrDie.TabIndex = 3;
            this.cmbLiveOrDie.UseTallSize = false;
            // 
            // cmbGender
            // 
            this.cmbGender.AutoResize = false;
            this.cmbGender.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cmbGender.Depth = 0;
            this.cmbGender.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cmbGender.DropDownHeight = 118;
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.DropDownWidth = 121;
            this.cmbGender.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cmbGender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Hint = "Giới tính";
            this.cmbGender.IntegralHeight = false;
            this.cmbGender.ItemHeight = 29;
            this.cmbGender.Location = new System.Drawing.Point(212, 13);
            this.cmbGender.MaxDropDownItems = 4;
            this.cmbGender.MouseState = MaterialSkin.MouseState.OUT;
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(187, 35);
            this.cmbGender.TabIndex = 3;
            this.cmbGender.UseTallSize = false;
            // 
            // txtKeyword
            // 
            this.txtKeyword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtKeyword.Depth = 0;
            this.txtKeyword.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtKeyword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtKeyword.Hint = "Từ khóa";
            this.txtKeyword.Location = new System.Drawing.Point(4, 12);
            this.txtKeyword.MaxLength = 50;
            this.txtKeyword.ModeNumber_Maximum = 999999;
            this.txtKeyword.MouseState = MaterialSkin.MouseState.OUT;
            this.txtKeyword.Multiline = false;
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(200, 36);
            this.txtKeyword.TabIndex = 2;
            this.txtKeyword.Text = "";
            this.txtKeyword.UseTallSize = false;
            // 
            // btn_exportEmployee
            // 
            this.btn_exportEmployee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_exportEmployee.AutoSize = false;
            this.btn_exportEmployee.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_exportEmployee.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_exportEmployee.Depth = 0;
            this.btn_exportEmployee.DrawShadows = true;
            this.btn_exportEmployee.HighEmphasis = true;
            this.btn_exportEmployee.Icon = null;
            this.btn_exportEmployee.Location = new System.Drawing.Point(948, 12);
            this.btn_exportEmployee.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_exportEmployee.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_exportEmployee.Name = "btn_exportEmployee";
            this.btn_exportEmployee.Size = new System.Drawing.Size(128, 36);
            this.btn_exportEmployee.TabIndex = 1;
            this.btn_exportEmployee.Text = "Export dữ liệu";
            this.btn_exportEmployee.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btn_exportEmployee.UseAccentColor = true;
            this.btn_exportEmployee.UseVisualStyleBackColor = true;
            this.btn_exportEmployee.Click += new System.EventHandler(this.btn_exportEmployee_Click);
            // 
            // plnListMember
            // 
            this.plnListMember.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plnListMember.Location = new System.Drawing.Point(0, 60);
            this.plnListMember.Margin = new System.Windows.Forms.Padding(0);
            this.plnListMember.Name = "plnListMember";
            this.plnListMember.Size = new System.Drawing.Size(1234, 603);
            this.plnListMember.TabIndex = 1;
            // 
            // ListMember
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainLayout);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ListMember";
            this.Size = new System.Drawing.Size(1234, 663);
            this.MainLayout.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel MainLayout;
        private System.Windows.Forms.Panel panel1;
        private MaterialSkin.Controls.MaterialButton btn_exportEmployee;
        private MaterialSkin.Controls.MaterialButton btn_addEmployee;
        private MaterialSkin.Controls.MaterialCheckbox chkboxInClan;
        private MaterialSkin.Controls.MaterialComboBox cmbLiveOrDie;
        private MaterialSkin.Controls.MaterialComboBox cmbGender;
        private MaterialSkin.Controls.MaterialTextBox txtKeyword;
        private MaterialSkin.Controls.MaterialButton btnSearch;
        private System.Windows.Forms.Panel plnListMember;
    }
}
