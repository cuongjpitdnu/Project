namespace GP40Main.Views
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.gridListMember = new GP40Main.Themes.Controls.DataGridTemplate(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSearch = new MaterialSkin.Controls.MaterialButton();
            this.chkboxInClan = new MaterialSkin.Controls.MaterialCheckbox();
            this.cmbLiveOrDie = new MaterialSkin.Controls.MaterialComboBox();
            this.cmbGender = new MaterialSkin.Controls.MaterialComboBox();
            this.txtKeyword = new MaterialSkin.Controls.MaterialTextBox();
            this.btn_exportEmployee = new MaterialSkin.Controls.MaterialButton();
            this.btn_addEmployee = new MaterialSkin.Controls.MaterialButton();
            this.FullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Gender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Birthday = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tel_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tel_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Email_2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Address = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ViewRelationship = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ActionDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.MainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridListMember)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 1;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.Controls.Add(this.gridListMember, 0, 1);
            this.MainLayout.Controls.Add(this.panel1, 0, 0);
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
            // gridListMember
            // 
            this.gridListMember.AllowUserToAddRows = false;
            this.gridListMember.AllowUserToResizeRows = false;
            this.gridListMember.BackgroundColor = System.Drawing.Color.PeachPuff;
            this.gridListMember.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridListMember.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridListMember.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridListMember.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridListMember.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridListMember.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FullName,
            this.Gender,
            this.Birthday,
            this.Tel_1,
            this.Tel_2,
            this.Email_1,
            this.Email_2,
            this.Address,
            this.ViewRelationship,
            this.ActionDelete});
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridListMember.DefaultCellStyle = dataGridViewCellStyle9;
            this.gridListMember.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridListMember.EnableHeadersVisualStyles = false;
            this.gridListMember.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridListMember.GridColor = System.Drawing.Color.SeaShell;
            this.gridListMember.Location = new System.Drawing.Point(3, 63);
            this.gridListMember.Name = "gridListMember";
            this.gridListMember.ReadOnly = true;
            this.gridListMember.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridListMember.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.gridListMember.RowHeadersVisible = false;
            this.gridListMember.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.PeachPuff;
            this.gridListMember.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.gridListMember.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridListMember.Size = new System.Drawing.Size(1228, 597);
            this.gridListMember.TabIndex = 1;
            this.gridListMember.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridListMember_CellContentClick);
            this.gridListMember.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridListMember_CellDoubleClick);
            this.gridListMember.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gridListMember_MouseClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.chkboxInClan);
            this.panel1.Controls.Add(this.cmbLiveOrDie);
            this.panel1.Controls.Add(this.cmbGender);
            this.panel1.Controls.Add(this.txtKeyword);
            this.panel1.Controls.Add(this.btn_exportEmployee);
            this.panel1.Controls.Add(this.btn_addEmployee);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1234, 60);
            this.panel1.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = false;
            this.btnSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSearch.Depth = 0;
            this.btnSearch.DrawShadows = true;
            this.btnSearch.HighEmphasis = true;
            this.btnSearch.Icon = null;
            this.btnSearch.Location = new System.Drawing.Point(749, 12);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSearch.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(152, 36);
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
            this.btn_exportEmployee.AutoSize = false;
            this.btn_exportEmployee.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_exportEmployee.Depth = 0;
            this.btn_exportEmployee.DrawShadows = true;
            this.btn_exportEmployee.HighEmphasis = true;
            this.btn_exportEmployee.Icon = null;
            this.btn_exportEmployee.Location = new System.Drawing.Point(909, 12);
            this.btn_exportEmployee.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_exportEmployee.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_exportEmployee.Name = "btn_exportEmployee";
            this.btn_exportEmployee.Size = new System.Drawing.Size(152, 36);
            this.btn_exportEmployee.TabIndex = 1;
            this.btn_exportEmployee.Text = "Export dữ liệu";
            this.btn_exportEmployee.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btn_exportEmployee.UseAccentColor = true;
            this.btn_exportEmployee.UseVisualStyleBackColor = true;
            this.btn_exportEmployee.Click += new System.EventHandler(this.btn_exportEmployee_Click);
            // 
            // btn_addEmployee
            // 
            this.btn_addEmployee.AutoSize = false;
            this.btn_addEmployee.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_addEmployee.Depth = 0;
            this.btn_addEmployee.DrawShadows = true;
            this.btn_addEmployee.HighEmphasis = true;
            this.btn_addEmployee.Icon = null;
            this.btn_addEmployee.Location = new System.Drawing.Point(1069, 12);
            this.btn_addEmployee.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_addEmployee.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_addEmployee.Name = "btn_addEmployee";
            this.btn_addEmployee.Size = new System.Drawing.Size(152, 36);
            this.btn_addEmployee.TabIndex = 0;
            this.btn_addEmployee.Text = "Thêm thành viên";
            this.btn_addEmployee.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btn_addEmployee.UseAccentColor = true;
            this.btn_addEmployee.UseVisualStyleBackColor = true;
            this.btn_addEmployee.Click += new System.EventHandler(this.btn_addEmployee_Click);
            // 
            // FullName
            // 
            this.FullName.DataPropertyName = "Name";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            this.FullName.DefaultCellStyle = dataGridViewCellStyle2;
            this.FullName.HeaderText = "Tên";
            this.FullName.Name = "FullName";
            this.FullName.ReadOnly = true;
            this.FullName.Width = 150;
            // 
            // Gender
            // 
            this.Gender.DataPropertyName = "GenderShow";
            this.Gender.HeaderText = "Giới tính";
            this.Gender.Name = "Gender";
            this.Gender.ReadOnly = true;
            // 
            // Birthday
            // 
            this.Birthday.DataPropertyName = "BirthdayShow";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Birthday.DefaultCellStyle = dataGridViewCellStyle3;
            this.Birthday.HeaderText = "Ngày sinh";
            this.Birthday.Name = "Birthday";
            this.Birthday.ReadOnly = true;
            this.Birthday.Width = 150;
            // 
            // Tel_1
            // 
            this.Tel_1.DataPropertyName = "Tel_1";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Tel_1.DefaultCellStyle = dataGridViewCellStyle4;
            this.Tel_1.HeaderText = "Số điện thoại 1";
            this.Tel_1.Name = "Tel_1";
            this.Tel_1.ReadOnly = true;
            this.Tel_1.Width = 120;
            // 
            // Tel_2
            // 
            this.Tel_2.DataPropertyName = "Tel_2";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Tel_2.DefaultCellStyle = dataGridViewCellStyle5;
            this.Tel_2.HeaderText = "Số điện thoại 2";
            this.Tel_2.Name = "Tel_2";
            this.Tel_2.ReadOnly = true;
            this.Tel_2.Width = 120;
            // 
            // Email_1
            // 
            this.Email_1.DataPropertyName = "Email_1";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Email_1.DefaultCellStyle = dataGridViewCellStyle6;
            this.Email_1.HeaderText = "Email 1";
            this.Email_1.Name = "Email_1";
            this.Email_1.ReadOnly = true;
            this.Email_1.Width = 130;
            // 
            // Email_2
            // 
            this.Email_2.DataPropertyName = "Email_2";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Email_2.DefaultCellStyle = dataGridViewCellStyle7;
            this.Email_2.HeaderText = "Email 2";
            this.Email_2.Name = "Email_2";
            this.Email_2.ReadOnly = true;
            this.Email_2.Width = 130;
            // 
            // Address
            // 
            this.Address.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Address.DataPropertyName = "Address";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Address.DefaultCellStyle = dataGridViewCellStyle8;
            this.Address.HeaderText = "Địa chỉ";
            this.Address.Name = "Address";
            this.Address.ReadOnly = true;
            // 
            // ViewRelationship
            // 
            this.ViewRelationship.HeaderText = "Quan hệ";
            this.ViewRelationship.Name = "ViewRelationship";
            this.ViewRelationship.ReadOnly = true;
            this.ViewRelationship.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ViewRelationship.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ViewRelationship.Text = "Xem";
            this.ViewRelationship.UseColumnTextForButtonValue = true;
            // 
            // ActionDelete
            // 
            this.ActionDelete.HeaderText = "Action Delete";
            this.ActionDelete.Name = "ActionDelete";
            this.ActionDelete.ReadOnly = true;
            this.ActionDelete.Text = "Xóa";
            this.ActionDelete.UseColumnTextForButtonValue = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.gridListMember)).EndInit();
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
        private Themes.Controls.DataGridTemplate gridListMember;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Gender;
        private System.Windows.Forms.DataGridViewTextBoxColumn Birthday;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tel_1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tel_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email_1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Email_2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Address;
        private System.Windows.Forms.DataGridViewButtonColumn ViewRelationship;
        private System.Windows.Forms.DataGridViewButtonColumn ActionDelete;
    }
}
