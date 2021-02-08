namespace DSF602.View
{
    partial class UserManagement
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabMngt = new System.Windows.Forms.TabControl();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.UserLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btSaveUser = new System.Windows.Forms.Button();
            this.btCreateUser = new System.Windows.Forms.Button();
            this.cboRole = new System.Windows.Forms.ComboBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.lblFullName = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.grvUser = new BaseCommon.ControlTemplate.dgv();
            this.tabChangePassword = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btSaveChange = new System.Windows.Forms.Button();
            this.txtRetypeNew = new System.Windows.Forms.TextBox();
            this.lblRetypePass = new System.Windows.Forms.Label();
            this.txtNew = new System.Windows.Forms.TextBox();
            this.lblNewPass = new System.Windows.Forms.Label();
            this.txtCurent = new System.Windows.Forms.TextBox();
            this.lblCurrentPass = new System.Windows.Forms.Label();
            this.colUserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeleteUser = new System.Windows.Forms.DataGridViewLinkColumn();
            this.tabMngt.SuspendLayout();
            this.tabUsers.SuspendLayout();
            this.UserLayout.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvUser)).BeginInit();
            this.tabChangePassword.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMngt
            // 
            this.tabMngt.Controls.Add(this.tabUsers);
            this.tabMngt.Controls.Add(this.tabChangePassword);
            this.tabMngt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMngt.Location = new System.Drawing.Point(0, 0);
            this.tabMngt.Margin = new System.Windows.Forms.Padding(4);
            this.tabMngt.Name = "tabMngt";
            this.tabMngt.SelectedIndex = 0;
            this.tabMngt.Size = new System.Drawing.Size(1011, 469);
            this.tabMngt.TabIndex = 21;
            this.tabMngt.SelectedIndexChanged += new System.EventHandler(this.tabMngt_SelectedIndexChanged);
            // 
            // tabUsers
            // 
            this.tabUsers.Controls.Add(this.UserLayout);
            this.tabUsers.Location = new System.Drawing.Point(4, 27);
            this.tabUsers.Margin = new System.Windows.Forms.Padding(4);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Padding = new System.Windows.Forms.Padding(4);
            this.tabUsers.Size = new System.Drawing.Size(1003, 438);
            this.tabUsers.TabIndex = 1;
            this.tabUsers.Text = "Users";
            this.tabUsers.UseVisualStyleBackColor = true;
            // 
            // UserLayout
            // 
            this.UserLayout.ColumnCount = 2;
            this.UserLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.UserLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 412F));
            this.UserLayout.Controls.Add(this.panel2, 0, 0);
            this.UserLayout.Controls.Add(this.grvUser, 0, 0);
            this.UserLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UserLayout.Location = new System.Drawing.Point(4, 4);
            this.UserLayout.Margin = new System.Windows.Forms.Padding(4);
            this.UserLayout.Name = "UserLayout";
            this.UserLayout.RowCount = 1;
            this.UserLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60.25974F));
            this.UserLayout.Size = new System.Drawing.Size(995, 430);
            this.UserLayout.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightCyan;
            this.panel2.Controls.Add(this.btSaveUser);
            this.panel2.Controls.Add(this.btCreateUser);
            this.panel2.Controls.Add(this.cboRole);
            this.panel2.Controls.Add(this.txtEmail);
            this.panel2.Controls.Add(this.lblRole);
            this.panel2.Controls.Add(this.lblEmail);
            this.panel2.Controls.Add(this.txtFullName);
            this.panel2.Controls.Add(this.lblFullName);
            this.panel2.Controls.Add(this.txtPassword);
            this.panel2.Controls.Add(this.lblPassword);
            this.panel2.Controls.Add(this.txtUserName);
            this.panel2.Controls.Add(this.lblUsername);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(587, 4);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(404, 422);
            this.panel2.TabIndex = 2;
            // 
            // btSaveUser
            // 
            this.btSaveUser.Location = new System.Drawing.Point(207, 246);
            this.btSaveUser.Margin = new System.Windows.Forms.Padding(4);
            this.btSaveUser.Name = "btSaveUser";
            this.btSaveUser.Size = new System.Drawing.Size(126, 39);
            this.btSaveUser.TabIndex = 19;
            this.btSaveUser.Text = "Save User";
            this.btSaveUser.UseVisualStyleBackColor = true;
            this.btSaveUser.Click += new System.EventHandler(this.btSaveUser_Click);
            // 
            // btCreateUser
            // 
            this.btCreateUser.Location = new System.Drawing.Point(71, 246);
            this.btCreateUser.Margin = new System.Windows.Forms.Padding(4);
            this.btCreateUser.Name = "btCreateUser";
            this.btCreateUser.Size = new System.Drawing.Size(126, 39);
            this.btCreateUser.TabIndex = 20;
            this.btCreateUser.Text = "Create User";
            this.btCreateUser.UseVisualStyleBackColor = true;
            this.btCreateUser.Click += new System.EventHandler(this.btCreateUser_Click);
            // 
            // cboRole
            // 
            this.cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole.FormattingEnabled = true;
            this.cboRole.Location = new System.Drawing.Point(143, 186);
            this.cboRole.Margin = new System.Windows.Forms.Padding(4);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(243, 26);
            this.cboRole.TabIndex = 18;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(143, 148);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(243, 26);
            this.txtEmail.TabIndex = 17;
            // 
            // lblRole
            // 
            this.lblRole.Location = new System.Drawing.Point(4, 189);
            this.lblRole.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRole.Name = "lblRole";
            this.lblRole.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblRole.Size = new System.Drawing.Size(131, 18);
            this.lblRole.TabIndex = 0;
            this.lblRole.Text = "Role";
            // 
            // lblEmail
            // 
            this.lblEmail.Location = new System.Drawing.Point(4, 151);
            this.lblEmail.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblEmail.Size = new System.Drawing.Size(131, 18);
            this.lblEmail.TabIndex = 0;
            this.lblEmail.Text = "Email";
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(143, 109);
            this.txtFullName.Margin = new System.Windows.Forms.Padding(4);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(243, 26);
            this.txtFullName.TabIndex = 16;
            // 
            // lblFullName
            // 
            this.lblFullName.Location = new System.Drawing.Point(4, 112);
            this.lblFullName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFullName.Name = "lblFullName";
            this.lblFullName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFullName.Size = new System.Drawing.Size(131, 18);
            this.lblFullName.TabIndex = 0;
            this.lblFullName.Text = "Full Name";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(143, 70);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(243, 26);
            this.txtPassword.TabIndex = 15;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(4, 73);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblPassword.Size = new System.Drawing.Size(131, 18);
            this.lblPassword.TabIndex = 0;
            this.lblPassword.Text = "Password";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(143, 31);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(243, 26);
            this.txtUserName.TabIndex = 14;
            // 
            // lblUsername
            // 
            this.lblUsername.Location = new System.Drawing.Point(4, 34);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblUsername.Size = new System.Drawing.Size(131, 18);
            this.lblUsername.TabIndex = 0;
            this.lblUsername.Text = "Username ";
            // 
            // grvUser
            // 
            this.grvUser.AllowUserToAddRows = false;
            this.grvUser.AllowUserToDeleteRows = false;
            this.grvUser.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grvUser.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grvUser.ColumnHeadersHeight = 30;
            this.grvUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUserId,
            this.colUserName,
            this.colPass,
            this.colFullName,
            this.colEmail,
            this.colRole,
            this.colDeleteUser});
            this.grvUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grvUser.EnableHeadersVisualStyles = false;
            this.grvUser.Location = new System.Drawing.Point(4, 4);
            this.grvUser.Margin = new System.Windows.Forms.Padding(4);
            this.grvUser.MultiSelect = false;
            this.grvUser.Name = "grvUser";
            this.grvUser.ReadOnly = true;
            this.grvUser.RowHeadersVisible = false;
            this.grvUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grvUser.Size = new System.Drawing.Size(575, 422);
            this.grvUser.TabIndex = 13;
            this.grvUser.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvUser_CellClick);
            this.grvUser.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grvUser_CellFormatting);
            // 
            // tabChangePassword
            // 
            this.tabChangePassword.Controls.Add(this.panel3);
            this.tabChangePassword.Location = new System.Drawing.Point(4, 27);
            this.tabChangePassword.Margin = new System.Windows.Forms.Padding(4);
            this.tabChangePassword.Name = "tabChangePassword";
            this.tabChangePassword.Size = new System.Drawing.Size(1003, 438);
            this.tabChangePassword.TabIndex = 2;
            this.tabChangePassword.Text = "Change Password";
            this.tabChangePassword.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.LightCyan;
            this.panel3.Controls.Add(this.btSaveChange);
            this.panel3.Controls.Add(this.txtRetypeNew);
            this.panel3.Controls.Add(this.lblRetypePass);
            this.panel3.Controls.Add(this.txtNew);
            this.panel3.Controls.Add(this.lblNewPass);
            this.panel3.Controls.Add(this.txtCurent);
            this.panel3.Controls.Add(this.lblCurrentPass);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1003, 438);
            this.panel3.TabIndex = 0;
            // 
            // btSaveChange
            // 
            this.btSaveChange.Location = new System.Drawing.Point(387, 201);
            this.btSaveChange.Margin = new System.Windows.Forms.Padding(4);
            this.btSaveChange.Name = "btSaveChange";
            this.btSaveChange.Size = new System.Drawing.Size(126, 39);
            this.btSaveChange.TabIndex = 25;
            this.btSaveChange.Text = "Save Change";
            this.btSaveChange.UseVisualStyleBackColor = true;
            this.btSaveChange.Click += new System.EventHandler(this.btSaveChange_Click);
            // 
            // txtRetypeNew
            // 
            this.txtRetypeNew.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRetypeNew.Location = new System.Drawing.Point(387, 167);
            this.txtRetypeNew.Margin = new System.Windows.Forms.Padding(4);
            this.txtRetypeNew.Name = "txtRetypeNew";
            this.txtRetypeNew.PasswordChar = '*';
            this.txtRetypeNew.Size = new System.Drawing.Size(380, 26);
            this.txtRetypeNew.TabIndex = 24;
            // 
            // lblRetypePass
            // 
            this.lblRetypePass.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRetypePass.Location = new System.Drawing.Point(145, 170);
            this.lblRetypePass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRetypePass.Name = "lblRetypePass";
            this.lblRetypePass.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblRetypePass.Size = new System.Drawing.Size(226, 18);
            this.lblRetypePass.TabIndex = 2;
            this.lblRetypePass.Text = "Re-type password";
            // 
            // txtNew
            // 
            this.txtNew.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNew.Location = new System.Drawing.Point(387, 133);
            this.txtNew.Margin = new System.Windows.Forms.Padding(4);
            this.txtNew.Name = "txtNew";
            this.txtNew.PasswordChar = '*';
            this.txtNew.Size = new System.Drawing.Size(380, 26);
            this.txtNew.TabIndex = 23;
            // 
            // lblNewPass
            // 
            this.lblNewPass.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNewPass.Location = new System.Drawing.Point(145, 136);
            this.lblNewPass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNewPass.Name = "lblNewPass";
            this.lblNewPass.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblNewPass.Size = new System.Drawing.Size(226, 18);
            this.lblNewPass.TabIndex = 2;
            this.lblNewPass.Text = "New  password";
            // 
            // txtCurent
            // 
            this.txtCurent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurent.Location = new System.Drawing.Point(387, 99);
            this.txtCurent.Margin = new System.Windows.Forms.Padding(4);
            this.txtCurent.Name = "txtCurent";
            this.txtCurent.PasswordChar = '*';
            this.txtCurent.Size = new System.Drawing.Size(380, 26);
            this.txtCurent.TabIndex = 22;
            // 
            // lblCurrentPass
            // 
            this.lblCurrentPass.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentPass.Location = new System.Drawing.Point(145, 102);
            this.lblCurrentPass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCurrentPass.Name = "lblCurrentPass";
            this.lblCurrentPass.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblCurrentPass.Size = new System.Drawing.Size(226, 18);
            this.lblCurrentPass.TabIndex = 2;
            this.lblCurrentPass.Text = "Current password";
            // 
            // colUserId
            // 
            this.colUserId.DataPropertyName = "userid";
            this.colUserId.HeaderText = "User Id";
            this.colUserId.Name = "colUserId";
            this.colUserId.ReadOnly = true;
            this.colUserId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colUserId.Visible = false;
            // 
            // colUserName
            // 
            this.colUserName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colUserName.DataPropertyName = "username";
            this.colUserName.HeaderText = "Username";
            this.colUserName.Name = "colUserName";
            this.colUserName.ReadOnly = true;
            this.colUserName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colUserName.Width = 86;
            // 
            // colPass
            // 
            this.colPass.DataPropertyName = "password";
            this.colPass.HeaderText = "Password";
            this.colPass.Name = "colPass";
            this.colPass.ReadOnly = true;
            this.colPass.Visible = false;
            // 
            // colFullName
            // 
            this.colFullName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFullName.DataPropertyName = "fullname";
            this.colFullName.HeaderText = "Full Name";
            this.colFullName.Name = "colFullName";
            this.colFullName.ReadOnly = true;
            this.colFullName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colEmail
            // 
            this.colEmail.DataPropertyName = "email";
            this.colEmail.HeaderText = "Email";
            this.colEmail.Name = "colEmail";
            this.colEmail.ReadOnly = true;
            this.colEmail.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colEmail.Visible = false;
            this.colEmail.Width = 180;
            // 
            // colRole
            // 
            this.colRole.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colRole.DataPropertyName = "role";
            this.colRole.HeaderText = "Role";
            this.colRole.Name = "colRole";
            this.colRole.ReadOnly = true;
            this.colRole.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRole.Width = 46;
            // 
            // colDeleteUser
            // 
            this.colDeleteUser.HeaderText = "";
            this.colDeleteUser.Name = "colDeleteUser";
            this.colDeleteUser.ReadOnly = true;
            this.colDeleteUser.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDeleteUser.Text = "Delete";
            this.colDeleteUser.UseColumnTextForLinkValue = true;
            this.colDeleteUser.Width = 65;
            // 
            // UserManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1011, 469);
            this.Controls.Add(this.tabMngt);
            this.DoubleBuffered = true;
            this.EscToClose = true;
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(938, 450);
            this.Name = "UserManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Users Management";
            this.tabMngt.ResumeLayout(false);
            this.tabUsers.ResumeLayout(false);
            this.UserLayout.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grvUser)).EndInit();
            this.tabChangePassword.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabMngt;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.TabPage tabChangePassword;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtRetypeNew;
        private System.Windows.Forms.Label lblRetypePass;
        private System.Windows.Forms.TextBox txtNew;
        private System.Windows.Forms.Label lblNewPass;
        private System.Windows.Forms.TextBox txtCurent;
        private System.Windows.Forms.Label lblCurrentPass;
        private System.Windows.Forms.Button btSaveChange;
        private System.Windows.Forms.TableLayoutPanel UserLayout;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btSaveUser;
        private System.Windows.Forms.Button btCreateUser;
        private System.Windows.Forms.ComboBox cboRole;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUsername;
        private BaseCommon.ControlTemplate.dgv grvUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRole;
        private System.Windows.Forms.DataGridViewLinkColumn colDeleteUser;
    }
}