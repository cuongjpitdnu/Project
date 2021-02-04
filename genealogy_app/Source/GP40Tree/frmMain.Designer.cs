namespace GP40Tree
{
    partial class frmMain
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelTopM = new System.Windows.Forms.Panel();
            this.dgvMember = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.txtKeyword = new System.Windows.Forms.TextBox();
            this.panelTopL = new System.Windows.Forms.Panel();
            this.lblMenuMember = new System.Windows.Forms.Label();
            this.picMenuMember = new System.Windows.Forms.PictureBox();
            this.lblMenuSetting = new System.Windows.Forms.Label();
            this.picMenuSetting = new System.Windows.Forms.PictureBox();
            this.lblMenuExit = new System.Windows.Forms.Label();
            this.lblMenuFamilyTree = new System.Windows.Forms.Label();
            this.lblMenuFamilyInfo = new System.Windows.Forms.Label();
            this.picMenuExit = new System.Windows.Forms.PictureBox();
            this.picMenuHambuger = new System.Windows.Forms.PictureBox();
            this.picMenuFamilyTree = new System.Windows.Forms.PictureBox();
            this.picFamily = new System.Windows.Forms.PictureBox();
            this.panelTopR = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelTopM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMember)).BeginInit();
            this.panelTopL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMenuMember)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenuSetting)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenuExit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenuHambuger)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenuFamilyTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFamily)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.panelTopM, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelTopL, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelTopR, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1095, 617);
            this.tableLayoutPanel1.TabIndex = 2;
            this.tableLayoutPanel1.CellPaint += new System.Windows.Forms.TableLayoutCellPaintEventHandler(this.tableLayoutPanel1_CellPaint);
            // 
            // panelTopM
            // 
            this.panelTopM.BackColor = System.Drawing.Color.White;
            this.panelTopM.Controls.Add(this.dgvMember);
            this.panelTopM.Controls.Add(this.button1);
            this.panelTopM.Controls.Add(this.txtKeyword);
            this.panelTopM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTopM.Location = new System.Drawing.Point(83, 3);
            this.panelTopM.Name = "panelTopM";
            this.panelTopM.Size = new System.Drawing.Size(244, 611);
            this.panelTopM.TabIndex = 7;
            // 
            // dgvMember
            // 
            this.dgvMember.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMember.Location = new System.Drawing.Point(5, 52);
            this.dgvMember.Name = "dgvMember";
            this.dgvMember.Size = new System.Drawing.Size(236, 343);
            this.dgvMember.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(180, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 36);
            this.button1.TabIndex = 5;
            this.button1.Text = "Tìm Kiếm";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // txtKeyword
            // 
            this.txtKeyword.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKeyword.Location = new System.Drawing.Point(5, 14);
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(171, 29);
            this.txtKeyword.TabIndex = 0;
            // 
            // panelTopL
            // 
            this.panelTopL.BackColor = System.Drawing.Color.Transparent;
            this.panelTopL.Controls.Add(this.lblMenuMember);
            this.panelTopL.Controls.Add(this.picMenuMember);
            this.panelTopL.Controls.Add(this.lblMenuSetting);
            this.panelTopL.Controls.Add(this.picMenuSetting);
            this.panelTopL.Controls.Add(this.lblMenuExit);
            this.panelTopL.Controls.Add(this.lblMenuFamilyTree);
            this.panelTopL.Controls.Add(this.lblMenuFamilyInfo);
            this.panelTopL.Controls.Add(this.picMenuExit);
            this.panelTopL.Controls.Add(this.picMenuHambuger);
            this.panelTopL.Controls.Add(this.picMenuFamilyTree);
            this.panelTopL.Controls.Add(this.picFamily);
            this.panelTopL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTopL.Location = new System.Drawing.Point(3, 3);
            this.panelTopL.Name = "panelTopL";
            this.panelTopL.Size = new System.Drawing.Size(74, 611);
            this.panelTopL.TabIndex = 2;
            // 
            // lblMenuMember
            // 
            this.lblMenuMember.BackColor = System.Drawing.Color.Transparent;
            this.lblMenuMember.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenuMember.Location = new System.Drawing.Point(1, 213);
            this.lblMenuMember.Name = "lblMenuMember";
            this.lblMenuMember.Size = new System.Drawing.Size(71, 50);
            this.lblMenuMember.TabIndex = 10;
            this.lblMenuMember.Text = "THÀNH VIÊN DÒNG HỌ";
            this.lblMenuMember.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMenuMember.Click += new System.EventHandler(this.Menu_Click);
            this.lblMenuMember.MouseLeave += new System.EventHandler(this.Menu_MouseLeave);
            this.lblMenuMember.MouseHover += new System.EventHandler(this.Menu_MouseHover);
            // 
            // picMenuMember
            // 
            this.picMenuMember.BackColor = System.Drawing.Color.Transparent;
            this.picMenuMember.Image = global::GP40Tree.Properties.Resources.member1;
            this.picMenuMember.Location = new System.Drawing.Point(18, 170);
            this.picMenuMember.Name = "picMenuMember";
            this.picMenuMember.Size = new System.Drawing.Size(40, 40);
            this.picMenuMember.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMenuMember.TabIndex = 9;
            this.picMenuMember.TabStop = false;
            this.picMenuMember.Click += new System.EventHandler(this.Menu_Click);
            this.picMenuMember.MouseLeave += new System.EventHandler(this.Menu_MouseLeave);
            this.picMenuMember.MouseHover += new System.EventHandler(this.Menu_MouseHover);
            // 
            // lblMenuSetting
            // 
            this.lblMenuSetting.BackColor = System.Drawing.Color.Transparent;
            this.lblMenuSetting.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenuSetting.Location = new System.Drawing.Point(2, 426);
            this.lblMenuSetting.Name = "lblMenuSetting";
            this.lblMenuSetting.Size = new System.Drawing.Size(71, 23);
            this.lblMenuSetting.TabIndex = 8;
            this.lblMenuSetting.Text = "CẤU HÌNH";
            this.lblMenuSetting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMenuSetting.Click += new System.EventHandler(this.Menu_Click);
            this.lblMenuSetting.MouseLeave += new System.EventHandler(this.Menu_MouseLeave);
            this.lblMenuSetting.MouseHover += new System.EventHandler(this.Menu_MouseHover);
            // 
            // picMenuSetting
            // 
            this.picMenuSetting.BackColor = System.Drawing.Color.Transparent;
            this.picMenuSetting.Image = global::GP40Tree.Properties.Resources.setting;
            this.picMenuSetting.Location = new System.Drawing.Point(18, 383);
            this.picMenuSetting.Name = "picMenuSetting";
            this.picMenuSetting.Size = new System.Drawing.Size(40, 40);
            this.picMenuSetting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMenuSetting.TabIndex = 7;
            this.picMenuSetting.TabStop = false;
            this.picMenuSetting.Click += new System.EventHandler(this.Menu_Click);
            this.picMenuSetting.MouseLeave += new System.EventHandler(this.Menu_MouseLeave);
            this.picMenuSetting.MouseHover += new System.EventHandler(this.Menu_MouseHover);
            // 
            // lblMenuExit
            // 
            this.lblMenuExit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblMenuExit.BackColor = System.Drawing.Color.Transparent;
            this.lblMenuExit.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenuExit.Location = new System.Drawing.Point(4, 528);
            this.lblMenuExit.Name = "lblMenuExit";
            this.lblMenuExit.Size = new System.Drawing.Size(68, 23);
            this.lblMenuExit.TabIndex = 6;
            this.lblMenuExit.Text = "THOÁT";
            this.lblMenuExit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMenuExit.Click += new System.EventHandler(this.Menu_Click);
            this.lblMenuExit.MouseLeave += new System.EventHandler(this.Menu_MouseLeave);
            this.lblMenuExit.MouseHover += new System.EventHandler(this.Menu_MouseHover);
            // 
            // lblMenuFamilyTree
            // 
            this.lblMenuFamilyTree.BackColor = System.Drawing.Color.Transparent;
            this.lblMenuFamilyTree.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenuFamilyTree.Location = new System.Drawing.Point(8, 118);
            this.lblMenuFamilyTree.Name = "lblMenuFamilyTree";
            this.lblMenuFamilyTree.Size = new System.Drawing.Size(60, 23);
            this.lblMenuFamilyTree.TabIndex = 5;
            this.lblMenuFamilyTree.Text = "PHẢ ĐỒ";
            this.lblMenuFamilyTree.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMenuFamilyTree.Click += new System.EventHandler(this.Menu_Click);
            this.lblMenuFamilyTree.MouseLeave += new System.EventHandler(this.Menu_MouseLeave);
            this.lblMenuFamilyTree.MouseHover += new System.EventHandler(this.Menu_MouseHover);
            // 
            // lblMenuFamilyInfo
            // 
            this.lblMenuFamilyInfo.BackColor = System.Drawing.Color.Transparent;
            this.lblMenuFamilyInfo.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMenuFamilyInfo.Location = new System.Drawing.Point(1, 329);
            this.lblMenuFamilyInfo.Name = "lblMenuFamilyInfo";
            this.lblMenuFamilyInfo.Size = new System.Drawing.Size(71, 23);
            this.lblMenuFamilyInfo.TabIndex = 4;
            this.lblMenuFamilyInfo.Text = "DÒNG HỌ";
            this.lblMenuFamilyInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMenuFamilyInfo.Click += new System.EventHandler(this.Menu_Click);
            this.lblMenuFamilyInfo.MouseLeave += new System.EventHandler(this.Menu_MouseLeave);
            this.lblMenuFamilyInfo.MouseHover += new System.EventHandler(this.Menu_MouseHover);
            // 
            // picMenuExit
            // 
            this.picMenuExit.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.picMenuExit.BackColor = System.Drawing.Color.White;
            this.picMenuExit.Image = global::GP40Tree.Properties.Resources.close;
            this.picMenuExit.Location = new System.Drawing.Point(18, 485);
            this.picMenuExit.Name = "picMenuExit";
            this.picMenuExit.Size = new System.Drawing.Size(40, 40);
            this.picMenuExit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMenuExit.TabIndex = 1;
            this.picMenuExit.TabStop = false;
            this.picMenuExit.Click += new System.EventHandler(this.Menu_Click);
            this.picMenuExit.MouseLeave += new System.EventHandler(this.Menu_MouseLeave);
            this.picMenuExit.MouseHover += new System.EventHandler(this.Menu_MouseHover);
            // 
            // picMenuHambuger
            // 
            this.picMenuHambuger.BackColor = System.Drawing.Color.Transparent;
            this.picMenuHambuger.Image = global::GP40Tree.Properties.Resources.expand;
            this.picMenuHambuger.Location = new System.Drawing.Point(18, 7);
            this.picMenuHambuger.Name = "picMenuHambuger";
            this.picMenuHambuger.Size = new System.Drawing.Size(40, 40);
            this.picMenuHambuger.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMenuHambuger.TabIndex = 0;
            this.picMenuHambuger.TabStop = false;
            this.picMenuHambuger.Click += new System.EventHandler(this.Menu_Click);
            this.picMenuHambuger.MouseLeave += new System.EventHandler(this.Menu_MouseLeave);
            this.picMenuHambuger.MouseHover += new System.EventHandler(this.Menu_MouseHover);
            // 
            // picMenuFamilyTree
            // 
            this.picMenuFamilyTree.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.picMenuFamilyTree.Image = global::GP40Tree.Properties.Resources.ftree;
            this.picMenuFamilyTree.Location = new System.Drawing.Point(18, 75);
            this.picMenuFamilyTree.Name = "picMenuFamilyTree";
            this.picMenuFamilyTree.Size = new System.Drawing.Size(40, 40);
            this.picMenuFamilyTree.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMenuFamilyTree.TabIndex = 3;
            this.picMenuFamilyTree.TabStop = false;
            this.picMenuFamilyTree.Click += new System.EventHandler(this.Menu_Click);
            this.picMenuFamilyTree.MouseLeave += new System.EventHandler(this.Menu_MouseLeave);
            this.picMenuFamilyTree.MouseHover += new System.EventHandler(this.Menu_MouseHover);
            // 
            // picFamily
            // 
            this.picFamily.BackColor = System.Drawing.Color.Transparent;
            this.picFamily.Image = global::GP40Tree.Properties.Resources.family;
            this.picFamily.Location = new System.Drawing.Point(18, 286);
            this.picFamily.Name = "picFamily";
            this.picFamily.Size = new System.Drawing.Size(40, 40);
            this.picFamily.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFamily.TabIndex = 2;
            this.picFamily.TabStop = false;
            this.picFamily.Click += new System.EventHandler(this.Menu_Click);
            this.picFamily.MouseLeave += new System.EventHandler(this.Menu_MouseLeave);
            this.picFamily.MouseHover += new System.EventHandler(this.Menu_MouseHover);
            // 
            // panelTopR
            // 
            this.panelTopR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTopR.Location = new System.Drawing.Point(333, 3);
            this.panelTopR.Name = "panelTopR";
            this.panelTopR.Size = new System.Drawing.Size(759, 611);
            this.panelTopR.TabIndex = 10;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 617);
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chương trình trình phả đồ";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelTopM.ResumeLayout(false);
            this.panelTopM.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMember)).EndInit();
            this.panelTopL.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picMenuMember)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenuSetting)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenuExit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenuHambuger)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMenuFamilyTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picFamily)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelTopL;
        private System.Windows.Forms.PictureBox picMenuHambuger;
        private System.Windows.Forms.Panel panelTopM;
        private System.Windows.Forms.PictureBox picMenuExit;
        private System.Windows.Forms.TextBox txtKeyword;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox picFamily;
        private System.Windows.Forms.PictureBox picMenuFamilyTree;
        private System.Windows.Forms.Label lblMenuFamilyInfo;
        private System.Windows.Forms.Label lblMenuExit;
        private System.Windows.Forms.Label lblMenuFamilyTree;
        private System.Windows.Forms.Label lblMenuSetting;
        private System.Windows.Forms.PictureBox picMenuSetting;
        private System.Windows.Forms.Label lblMenuMember;
        private System.Windows.Forms.PictureBox picMenuMember;
        private System.Windows.Forms.Panel panelTopR;
        private System.Windows.Forms.DataGridView dgvMember;
    }
}