namespace GPMain.Views.FamilyInfo
{
    partial class FamilyPopup
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.materialButton1 = new MaterialSkin.Controls.MaterialButton();
            this.btnSearch = new MaterialSkin.Controls.MaterialButton();
            this.txtKeyword = new MaterialSkin.Controls.MaterialTextBox();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.lb_titlePopup = new MaterialSkin.Controls.MaterialLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dgv_lstPopMem = new GPMain.Views.Controls.DataGridTemplate(this.components);
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameMember = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MemberAge = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DayMember = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_lstPopMem)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(842, 511);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.materialButton1);
            this.panel1.Controls.Add(this.btnSearch);
            this.panel1.Controls.Add(this.txtKeyword);
            this.panel1.Controls.Add(this.materialLabel1);
            this.panel1.Controls.Add(this.lb_titlePopup);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(836, 93);
            this.panel1.TabIndex = 0;
            // 
            // materialButton1
            // 
            this.materialButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.materialButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.materialButton1.Depth = 0;
            this.materialButton1.DrawShadows = true;
            this.materialButton1.HighEmphasis = true;
            this.materialButton1.Icon = null;
            this.materialButton1.Location = new System.Drawing.Point(723, 49);
            this.materialButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.Size = new System.Drawing.Size(103, 36);
            this.materialButton1.TabIndex = 5;
            this.materialButton1.Text = "Xuất Excel";
            this.materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton1.UseAccentColor = false;
            this.materialButton1.UseVisualStyleBackColor = true;
            this.materialButton1.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnSearch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearch.Depth = 0;
            this.btnSearch.DrawShadows = true;
            this.btnSearch.HighEmphasis = true;
            this.btnSearch.Icon = null;
            this.btnSearch.Location = new System.Drawing.Point(625, 49);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSearch.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(83, 36);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnSearch.UseAccentColor = false;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtKeyword
            // 
            this.txtKeyword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKeyword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtKeyword.Depth = 0;
            this.txtKeyword.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtKeyword.Location = new System.Drawing.Point(85, 49);
            this.txtKeyword.MaxLength = 50;
            this.txtKeyword.ModeNumber_Maximum = 999999;
            this.txtKeyword.MouseState = MaterialSkin.MouseState.OUT;
            this.txtKeyword.Multiline = false;
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(530, 36);
            this.txtKeyword.TabIndex = 2;
            this.txtKeyword.Text = "";
            this.txtKeyword.UseTallSize = false;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(3, 59);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(60, 19);
            this.materialLabel1.TabIndex = 1;
            this.materialLabel1.Text = "Từ khóa";
            // 
            // lb_titlePopup
            // 
            this.lb_titlePopup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_titlePopup.Depth = 0;
            this.lb_titlePopup.Font = new System.Drawing.Font("Roboto", 34F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.lb_titlePopup.FontType = MaterialSkin.MaterialSkinManager.fontType.H4;
            this.lb_titlePopup.Location = new System.Drawing.Point(4, 3);
            this.lb_titlePopup.MouseState = MaterialSkin.MouseState.HOVER;
            this.lb_titlePopup.Name = "lb_titlePopup";
            this.lb_titlePopup.Size = new System.Drawing.Size(827, 40);
            this.lb_titlePopup.TabIndex = 0;
            this.lb_titlePopup.Text = "Danh sách thành viên";
            this.lb_titlePopup.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dgv_lstPopMem);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 102);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(836, 406);
            this.panel2.TabIndex = 1;
            // 
            // dgv_lstPopMem
            // 
            this.dgv_lstPopMem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_lstPopMem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_lstPopMem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.STT,
            this.NameMember,
            this.MemberAge,
            this.DayMember});
            this.dgv_lstPopMem.EnableHeadersVisualStyles = false;
            this.dgv_lstPopMem.Location = new System.Drawing.Point(0, 0);
            this.dgv_lstPopMem.Name = "dgv_lstPopMem";
            this.dgv_lstPopMem.ReadOnly = true;
            this.dgv_lstPopMem.RowHeadersVisible = false;
            this.dgv_lstPopMem.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_lstPopMem.Size = new System.Drawing.Size(835, 405);
            this.dgv_lstPopMem.TabIndex = 0;
            // 
            // STT
            // 
            this.STT.DataPropertyName = "STT";
            this.STT.HeaderText = "STT";
            this.STT.Name = "STT";
            this.STT.ReadOnly = true;
            this.STT.Width = 177;
            // 
            // NameMember
            // 
            this.NameMember.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NameMember.DataPropertyName = "Name";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.NameMember.DefaultCellStyle = dataGridViewCellStyle1;
            this.NameMember.HeaderText = "Tên thành viên";
            this.NameMember.Name = "NameMember";
            this.NameMember.ReadOnly = true;
            // 
            // MemberAge
            // 
            this.MemberAge.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.MemberAge.DataPropertyName = "Age";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.MemberAge.DefaultCellStyle = dataGridViewCellStyle2;
            this.MemberAge.HeaderText = "Tuổi";
            this.MemberAge.Name = "MemberAge";
            this.MemberAge.ReadOnly = true;
            this.MemberAge.Width = 56;
            // 
            // DayMember
            // 
            this.DayMember.DataPropertyName = "Date";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.DayMember.DefaultCellStyle = dataGridViewCellStyle3;
            this.DayMember.HeaderText = "Ngày";
            this.DayMember.Name = "DayMember";
            this.DayMember.ReadOnly = true;
            this.DayMember.Width = 177;
            // 
            // FamilyPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MintCream;
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FamilyPopup";
            this.Sizable = false;
            this.Size = new System.Drawing.Size(842, 511);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_lstPopMem)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private MaterialSkin.Controls.MaterialLabel lb_titlePopup;
        private System.Windows.Forms.Panel panel2;
        private Controls.DataGridTemplate dgv_lstPopMem;
        private MaterialSkin.Controls.MaterialButton btnSearch;
        private MaterialSkin.Controls.MaterialTextBox txtKeyword;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private System.Windows.Forms.DataGridViewTextBoxColumn STT;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameMember;
        private System.Windows.Forms.DataGridViewTextBoxColumn MemberAge;
        private System.Windows.Forms.DataGridViewTextBoxColumn DayMember;
    }
}
