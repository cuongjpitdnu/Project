namespace GP40Main.Views.Member
{
    partial class ListMemberRelation
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
            this.btnAddRelation = new MaterialSkin.Controls.MaterialButton();
            this.dgvListMemberRelation = new GP40Main.Themes.Controls.DataGridTemplate(this.components);
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGenderShow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRelationType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActionDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvListMemberRelation)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddRelation
            // 
            this.btnAddRelation.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddRelation.Depth = 0;
            this.btnAddRelation.DrawShadows = true;
            this.btnAddRelation.HighEmphasis = true;
            this.btnAddRelation.Icon = null;
            this.btnAddRelation.Location = new System.Drawing.Point(388, 6);
            this.btnAddRelation.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnAddRelation.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAddRelation.Name = "btnAddRelation";
            this.btnAddRelation.Size = new System.Drawing.Size(128, 36);
            this.btnAddRelation.TabIndex = 1;
            this.btnAddRelation.Text = "Thêm quan hệ";
            this.btnAddRelation.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnAddRelation.UseAccentColor = true;
            this.btnAddRelation.UseVisualStyleBackColor = true;
            this.btnAddRelation.Click += new System.EventHandler(this.btnAddRelation_Click);
            // 
            // dgvListMemberRelation
            // 
            this.dgvListMemberRelation.AllowUserToAddRows = false;
            this.dgvListMemberRelation.AllowUserToResizeRows = false;
            this.dgvListMemberRelation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvListMemberRelation.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgvListMemberRelation.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvListMemberRelation.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvListMemberRelation.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvListMemberRelation.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvListMemberRelation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvListMemberRelation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colGenderShow,
            this.colRelationType,
            this.colActionDelete});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvListMemberRelation.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvListMemberRelation.EnableHeadersVisualStyles = false;
            this.dgvListMemberRelation.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dgvListMemberRelation.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgvListMemberRelation.Location = new System.Drawing.Point(3, 51);
            this.dgvListMemberRelation.Name = "dgvListMemberRelation";
            this.dgvListMemberRelation.ReadOnly = true;
            this.dgvListMemberRelation.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvListMemberRelation.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvListMemberRelation.RowHeadersVisible = false;
            this.dgvListMemberRelation.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvListMemberRelation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvListMemberRelation.Size = new System.Drawing.Size(514, 200);
            this.dgvListMemberRelation.TabIndex = 2;
            this.dgvListMemberRelation.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvListMemberRelation_CellContentClick);
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Họ tên";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colGenderShow
            // 
            this.colGenderShow.DataPropertyName = "GenderShow";
            this.colGenderShow.HeaderText = "Giới tính";
            this.colGenderShow.Name = "colGenderShow";
            this.colGenderShow.ReadOnly = true;
            // 
            // colRelationType
            // 
            this.colRelationType.DataPropertyName = "RelTypeShow";
            this.colRelationType.HeaderText = "Quan hệ";
            this.colRelationType.Name = "colRelationType";
            this.colRelationType.ReadOnly = true;
            // 
            // colActionDelete
            // 
            this.colActionDelete.HeaderText = "Action delete";
            this.colActionDelete.Name = "colActionDelete";
            this.colActionDelete.ReadOnly = true;
            this.colActionDelete.Text = "Xóa";
            this.colActionDelete.UseColumnTextForButtonValue = true;
            // 
            // ListMemberRelation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.Controls.Add(this.dgvListMemberRelation);
            this.Controls.Add(this.btnAddRelation);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ListMemberRelation";
            this.Size = new System.Drawing.Size(520, 254);
            ((System.ComponentModel.ISupportInitialize)(this.dgvListMemberRelation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MaterialSkin.Controls.MaterialButton btnAddRelation;
        private Themes.Controls.DataGridTemplate dgvListMemberRelation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGenderShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRelationType;
        private System.Windows.Forms.DataGridViewButtonColumn colActionDelete;
    }
}
