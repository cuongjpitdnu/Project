namespace KeyMgnt.Screens
{
    partial class CustomerManagerForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomerManagerForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAddKey = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.txtKeySeach = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvCusKeys = new KeyMgnt.Common.DataGridViewBase(this.components);
            this.cusKeyKeyCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cusKeyMachineCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cusKeyMacAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCustomerList = new KeyMgnt.Common.DataGridViewBase(this.components);
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerMobile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CompanyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CompanyMobile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CompanyAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KeyCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MachineCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MacAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCusKeys)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerList)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1193F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1193, 616);
            this.tableLayoutPanel1.TabIndex = 156;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnAddKey);
            this.panel2.Controls.Add(this.btnSearch);
            this.panel2.Controls.Add(this.btnExcel);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.dtpFrom);
            this.panel2.Controls.Add(this.dtpTo);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtKeySeach);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 69);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1187, 59);
            this.panel2.TabIndex = 1;
            // 
            // btnAddKey
            // 
            this.btnAddKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddKey.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnAddKey.BackgroundImage = global::KeyMgnt.Properties.Resources.key;
            this.btnAddKey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAddKey.Location = new System.Drawing.Point(1117, 5);
            this.btnAddKey.Name = "btnAddKey";
            this.btnAddKey.Size = new System.Drawing.Size(65, 49);
            this.btnAddKey.TabIndex = 168;
            this.btnAddKey.UseVisualStyleBackColor = false;
            this.btnAddKey.Click += new System.EventHandler(this.btnAddKey_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSearch.BackgroundImage = global::KeyMgnt.Properties.Resources.search;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSearch.Location = new System.Drawing.Point(898, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(65, 49);
            this.btnSearch.TabIndex = 165;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSeach_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnExcel.BackgroundImage = global::KeyMgnt.Properties.Resources.excel;
            this.btnExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExcel.Location = new System.Drawing.Point(971, 5);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(65, 49);
            this.btnExcel.TabIndex = 166;
            this.btnExcel.UseVisualStyleBackColor = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnAdd.BackgroundImage = global::KeyMgnt.Properties.Resources.add;
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAdd.Location = new System.Drawing.Point(1044, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(65, 49);
            this.btnAdd.TabIndex = 167;
            this.btnAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(307, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 164;
            this.label3.Text = "DATE:";
            // 
            // dtpFrom
            // 
            this.dtpFrom.CustomFormat = "dd/MM/yyyy";
            this.dtpFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFrom.Location = new System.Drawing.Point(361, 16);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(163, 26);
            this.dtpFrom.TabIndex = 157;
            this.dtpFrom.Value = new System.DateTime(2019, 5, 23, 0, 0, 0, 0);
            // 
            // dtpTo
            // 
            this.dtpTo.CustomFormat = "dd/MM/yyyy";
            this.dtpTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTo.Location = new System.Drawing.Point(550, 16);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(167, 26);
            this.dtpTo.TabIndex = 158;
            this.dtpTo.Value = new System.DateTime(2019, 5, 23, 0, 0, 0, 0);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(530, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 163;
            this.label4.Text = "~";
            // 
            // txtKeySeach
            // 
            this.txtKeySeach.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKeySeach.Location = new System.Drawing.Point(45, 14);
            this.txtKeySeach.Name = "txtKeySeach";
            this.txtKeySeach.Size = new System.Drawing.Size(250, 30);
            this.txtKeySeach.TabIndex = 156;
            this.txtKeySeach.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKeySeach_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 160;
            this.label2.Text = "KEY:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1187, 60);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(485, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Customer Management";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.98315F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 476F));
            this.tableLayoutPanel2.Controls.Add(this.dgvCusKeys, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.dgvCustomerList, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 134);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1187, 479);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // dgvCusKeys
            // 
            this.dgvCusKeys.AllowUserToAddRows = false;
            this.dgvCusKeys.AllowUserToDeleteRows = false;
            this.dgvCusKeys.BackgroundColor = System.Drawing.Color.Ivory;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCusKeys.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCusKeys.ColumnHeadersHeight = 26;
            this.dgvCusKeys.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cusKeyKeyCode,
            this.cusKeyMachineCode,
            this.cusKeyMacAddress,
            this.CreatedBy,
            this.CreatedDate});
            this.dgvCusKeys.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCusKeys.EnableHeadersVisualStyles = false;
            this.dgvCusKeys.Location = new System.Drawing.Point(714, 3);
            this.dgvCusKeys.MultiSelect = false;
            this.dgvCusKeys.Name = "dgvCusKeys";
            this.dgvCusKeys.ReadOnly = true;
            this.dgvCusKeys.RowHeadersVisible = false;
            this.dgvCusKeys.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCusKeys.Size = new System.Drawing.Size(470, 473);
            this.dgvCusKeys.TabIndex = 4;
            this.dgvCusKeys.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCusKeys_CellDoubleClick);
            // 
            // cusKeyKeyCode
            // 
            this.cusKeyKeyCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cusKeyKeyCode.DataPropertyName = "KeyCode";
            this.cusKeyKeyCode.HeaderText = "KeyCode";
            this.cusKeyKeyCode.Name = "cusKeyKeyCode";
            this.cusKeyKeyCode.ReadOnly = true;
            // 
            // cusKeyMachineCode
            // 
            this.cusKeyMachineCode.DataPropertyName = "MachineCode";
            this.cusKeyMachineCode.HeaderText = "MachineCode";
            this.cusKeyMachineCode.Name = "cusKeyMachineCode";
            this.cusKeyMachineCode.ReadOnly = true;
            // 
            // cusKeyMacAddress
            // 
            this.cusKeyMacAddress.DataPropertyName = "MacAddress";
            this.cusKeyMacAddress.HeaderText = "MacAddress";
            this.cusKeyMacAddress.Name = "cusKeyMacAddress";
            this.cusKeyMacAddress.ReadOnly = true;
            // 
            // CreatedBy
            // 
            this.CreatedBy.DataPropertyName = "UserId";
            this.CreatedBy.HeaderText = "CreatedBy";
            this.CreatedBy.Name = "CreatedBy";
            this.CreatedBy.ReadOnly = true;
            this.CreatedBy.Visible = false;
            // 
            // CreatedDate
            // 
            this.CreatedDate.DataPropertyName = "CreateDate";
            this.CreatedDate.HeaderText = "CreatedDate";
            this.CreatedDate.Name = "CreatedDate";
            this.CreatedDate.ReadOnly = true;
            this.CreatedDate.Visible = false;
            // 
            // dgvCustomerList
            // 
            this.dgvCustomerList.AllowUserToAddRows = false;
            this.dgvCustomerList.AllowUserToDeleteRows = false;
            this.dgvCustomerList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCustomerList.BackgroundColor = System.Drawing.Color.Ivory;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCustomerList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCustomerList.ColumnHeadersHeight = 26;
            this.dgvCustomerList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.CustomerName,
            this.CustomerMobile,
            this.CustomerEmail,
            this.CompanyName,
            this.CompanyMobile,
            this.CompanyAddress,
            this.KeyCode,
            this.MachineCode,
            this.MacAddress,
            this.UserId,
            this.CreateDate});
            this.dgvCustomerList.EnableHeadersVisualStyles = false;
            this.dgvCustomerList.Location = new System.Drawing.Point(3, 3);
            this.dgvCustomerList.MultiSelect = false;
            this.dgvCustomerList.Name = "dgvCustomerList";
            this.dgvCustomerList.ReadOnly = true;
            this.dgvCustomerList.RowHeadersVisible = false;
            this.dgvCustomerList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCustomerList.Size = new System.Drawing.Size(705, 473);
            this.dgvCustomerList.TabIndex = 3;
            this.dgvCustomerList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomerList_CellClick);
            this.dgvCustomerList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCustomerList_CellDoubleClick);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // CustomerName
            // 
            this.CustomerName.DataPropertyName = "CusName";
            this.CustomerName.HeaderText = "CustomerName";
            this.CustomerName.Name = "CustomerName";
            this.CustomerName.ReadOnly = true;
            // 
            // CustomerMobile
            // 
            this.CustomerMobile.DataPropertyName = "CusMobile";
            this.CustomerMobile.HeaderText = "CustomerMobile";
            this.CustomerMobile.Name = "CustomerMobile";
            this.CustomerMobile.ReadOnly = true;
            // 
            // CustomerEmail
            // 
            this.CustomerEmail.DataPropertyName = "CusEmail";
            this.CustomerEmail.HeaderText = "CustomerEmail";
            this.CustomerEmail.Name = "CustomerEmail";
            this.CustomerEmail.ReadOnly = true;
            // 
            // CompanyName
            // 
            this.CompanyName.DataPropertyName = "CompanyName";
            this.CompanyName.HeaderText = "CompanyName";
            this.CompanyName.Name = "CompanyName";
            this.CompanyName.ReadOnly = true;
            // 
            // CompanyMobile
            // 
            this.CompanyMobile.DataPropertyName = "CompanyMobile";
            this.CompanyMobile.HeaderText = "CompanyMobile";
            this.CompanyMobile.Name = "CompanyMobile";
            this.CompanyMobile.ReadOnly = true;
            // 
            // CompanyAddress
            // 
            this.CompanyAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.CompanyAddress.DataPropertyName = "CompanyAddress";
            this.CompanyAddress.HeaderText = "CompanyAddress";
            this.CompanyAddress.Name = "CompanyAddress";
            this.CompanyAddress.ReadOnly = true;
            // 
            // KeyCode
            // 
            this.KeyCode.DataPropertyName = "KeyCode";
            this.KeyCode.HeaderText = "KeyCode";
            this.KeyCode.Name = "KeyCode";
            this.KeyCode.ReadOnly = true;
            this.KeyCode.Visible = false;
            // 
            // MachineCode
            // 
            this.MachineCode.DataPropertyName = "MachineCode";
            this.MachineCode.HeaderText = "MachineCode";
            this.MachineCode.Name = "MachineCode";
            this.MachineCode.ReadOnly = true;
            this.MachineCode.Visible = false;
            // 
            // MacAddress
            // 
            this.MacAddress.DataPropertyName = "MacAddress";
            this.MacAddress.HeaderText = "MacAddress";
            this.MacAddress.Name = "MacAddress";
            this.MacAddress.ReadOnly = true;
            this.MacAddress.Visible = false;
            // 
            // UserId
            // 
            this.UserId.DataPropertyName = "UserId";
            this.UserId.HeaderText = "CreatedBy";
            this.UserId.Name = "UserId";
            this.UserId.ReadOnly = true;
            this.UserId.Visible = false;
            // 
            // CreateDate
            // 
            this.CreateDate.DataPropertyName = "CreateDate";
            this.CreateDate.HeaderText = "CreatedDate";
            this.CreateDate.Name = "CreateDate";
            this.CreateDate.ReadOnly = true;
            this.CreateDate.Visible = false;
            // 
            // CustomerManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1193, 616);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CustomerManagerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CustomerManagement";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCusKeys)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtKeySeach;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Common.DataGridViewBase dgvCustomerList;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerMobile;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn CompanyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CompanyMobile;
        private System.Windows.Forms.DataGridViewTextBoxColumn CompanyAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn KeyCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn MachineCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn MacAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserId;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreateDate;
        private System.Windows.Forms.Button btnAddKey;
        private Common.DataGridViewBase dgvCusKeys;
        private System.Windows.Forms.DataGridViewTextBoxColumn cusKeyKeyCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn cusKeyMachineCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn cusKeyMacAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedDate;
    }
}