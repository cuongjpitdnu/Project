namespace WithMySql
{
    partial class Management
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabMngt = new System.Windows.Forms.TabControl();
            this.tabSetting = new System.Windows.Forms.TabPage();
            this.dvcManagement = new BaseCommon.DeviceManagement();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.UserLayout = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btSaveUser = new System.Windows.Forms.Button();
            this.btCreateUser = new System.Windows.Forms.Button();
            this.cboRole = new System.Windows.Forms.ComboBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFullName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.grvUser = new System.Windows.Forms.DataGridView();
            this.colUserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFullName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRole = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDeleteUser = new System.Windows.Forms.DataGridViewLinkColumn();
            this.tabChangePassword = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btSaveChange = new System.Windows.Forms.Button();
            this.txtRetypeNew = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtNew = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtCurent = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tabMngt.SuspendLayout();
            this.tabSetting.SuspendLayout();
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
            this.tabMngt.Controls.Add(this.tabSetting);
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
            // tabSetting
            // 
            this.tabSetting.Controls.Add(this.dvcManagement);
            this.tabSetting.Location = new System.Drawing.Point(4, 27);
            this.tabSetting.Margin = new System.Windows.Forms.Padding(4);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.Padding = new System.Windows.Forms.Padding(4);
            this.tabSetting.Size = new System.Drawing.Size(1003, 438);
            this.tabSetting.TabIndex = 0;
            this.tabSetting.Text = "Setting";
            this.tabSetting.UseVisualStyleBackColor = true;
            // 
            // dvcManagement
            // 
            this.dvcManagement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dvcManagement.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dvcManagement.ListDevice = null;
            this.dvcManagement.Location = new System.Drawing.Point(4, 4);
            this.dvcManagement.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dvcManagement.Name = "dvcManagement";
            this.dvcManagement.GridDeviceSelected = null;
            this.dvcManagement.Size = new System.Drawing.Size(995, 430);
            this.dvcManagement.TabIndex = 0;
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
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.txtFullName);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.txtPassword);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtUserName);
            this.panel2.Controls.Add(this.label4);
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
            this.cboRole.Location = new System.Drawing.Point(121, 186);
            this.cboRole.Margin = new System.Windows.Forms.Padding(4);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(265, 26);
            this.cboRole.TabIndex = 18;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(121, 148);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(265, 26);
            this.txtEmail.TabIndex = 17;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(66, 189);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 18);
            this.label8.TabIndex = 0;
            this.label8.Text = "Role :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(57, 151);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 18);
            this.label7.TabIndex = 0;
            this.label7.Text = "Email :";
            // 
            // txtFullName
            // 
            this.txtFullName.Location = new System.Drawing.Point(121, 109);
            this.txtFullName.Margin = new System.Windows.Forms.Padding(4);
            this.txtFullName.Name = "txtFullName";
            this.txtFullName.Size = new System.Drawing.Size(265, 26);
            this.txtFullName.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 112);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 18);
            this.label6.TabIndex = 0;
            this.label6.Text = "Full Name :";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(121, 70);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(265, 26);
            this.txtPassword.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(27, 73);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 18);
            this.label5.TabIndex = 0;
            this.label5.Text = "Password :";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(121, 31);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(265, 26);
            this.txtUserName.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 34);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 18);
            this.label4.TabIndex = 0;
            this.label4.Text = "Username :";
            // 
            // grvUser
            // 
            this.grvUser.AllowUserToAddRows = false;
            this.grvUser.AllowUserToDeleteRows = false;
            this.grvUser.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grvUser.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.grvUser.ColumnHeadersHeight = 26;
            this.grvUser.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUserId,
            this.colUserName,
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
            this.grvUser.RowHeadersVisible = false;
            this.grvUser.RowTemplate.Height = 25;
            this.grvUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grvUser.Size = new System.Drawing.Size(575, 422);
            this.grvUser.TabIndex = 13;
            this.grvUser.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvUser_CellClick);
            this.grvUser.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grvUser_CellFormatting);
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
            this.colUserName.DataPropertyName = "username";
            this.colUserName.HeaderText = "Username";
            this.colUserName.Name = "colUserName";
            this.colUserName.ReadOnly = true;
            this.colUserName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colUserName.Width = 120;
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
            this.colRole.DataPropertyName = "role";
            this.colRole.HeaderText = "Role";
            this.colRole.Name = "colRole";
            this.colRole.ReadOnly = true;
            this.colRole.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colRole.Width = 75;
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
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.txtNew);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.txtCurent);
            this.panel3.Controls.Add(this.label10);
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
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(237, 170);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(142, 18);
            this.label12.TabIndex = 2;
            this.label12.Text = "Re-type password :";
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
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(256, 136);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(123, 18);
            this.label11.TabIndex = 2;
            this.label11.Text = "New  password :";
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
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(240, 102);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(139, 18);
            this.label10.TabIndex = 2;
            this.label10.Text = "Current password :";
            // 
            // Management
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1011, 469);
            this.Controls.Add(this.tabMngt);
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(938, 450);
            this.Name = "Management";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "System Setting Management";
            this.tabMngt.ResumeLayout(false);
            this.tabSetting.ResumeLayout(false);
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
        private System.Windows.Forms.TabPage tabSetting;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.TabPage tabChangePassword;
        private System.Windows.Forms.TableLayoutPanel UserLayout;
        private System.Windows.Forms.DataGridView grvUser;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboRole;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btSaveUser;
        private System.Windows.Forms.Button btCreateUser;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox txtRetypeNew;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtNew;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtCurent;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btSaveChange;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRole;
        private System.Windows.Forms.DataGridViewLinkColumn colDeleteUser;
        private BaseCommon.DeviceManagement dvcManagement;
    }
}