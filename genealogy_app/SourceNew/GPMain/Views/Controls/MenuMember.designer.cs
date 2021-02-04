using MaterialSkin.Controls;

namespace GPMain.Views.Controls
{
    partial class MenuMember
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
            this.panelfilter = new System.Windows.Forms.Panel();
            this.flpPage = new System.Windows.Forms.FlowLayoutPanel();
            this.btnpage1 = new System.Windows.Forms.Button();
            this.btnpage2 = new System.Windows.Forms.Button();
            this.btnpage3 = new System.Windows.Forms.Button();
            this.btnpage4 = new System.Windows.Forms.Button();
            this.lbltotalmember = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.cmbLiveOrDie = new MaterialSkin.Controls.MaterialComboBox();
            this.cmbGender = new MaterialSkin.Controls.MaterialComboBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.txtKeyword = new MaterialSkin.Controls.MaterialTextBox();
            this.btnnextpage = new System.Windows.Forms.Button();
            this.btnprepage = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlLayoutMember = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.flpMember = new System.Windows.Forms.FlowLayoutPanel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timerDelaySearchByKeyword = new System.Windows.Forms.Timer(this.components);
            this.panelfilter.SuspendLayout();
            this.flpPage.SuspendLayout();
            this.pnlLayoutMember.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelfilter
            // 
            this.panelfilter.BackColor = System.Drawing.SystemColors.Control;
            this.panelfilter.Controls.Add(this.flpPage);
            this.panelfilter.Controls.Add(this.lbltotalmember);
            this.panelfilter.Controls.Add(this.materialLabel1);
            this.panelfilter.Controls.Add(this.cmbLiveOrDie);
            this.panelfilter.Controls.Add(this.cmbGender);
            this.panelfilter.Controls.Add(this.panel9);
            this.panelfilter.Controls.Add(this.panel8);
            this.panelfilter.Controls.Add(this.txtKeyword);
            this.panelfilter.Controls.Add(this.btnnextpage);
            this.panelfilter.Controls.Add(this.btnprepage);
            this.panelfilter.Controls.Add(this.panel2);
            this.panelfilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelfilter.Location = new System.Drawing.Point(0, 0);
            this.panelfilter.Margin = new System.Windows.Forms.Padding(0);
            this.panelfilter.Name = "panelfilter";
            this.panelfilter.Size = new System.Drawing.Size(237, 160);
            this.panelfilter.TabIndex = 1;
            this.panelfilter.Paint += new System.Windows.Forms.PaintEventHandler(this.panelfilter_Paint);
            // 
            // flpPage
            // 
            this.flpPage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpPage.AutoSize = true;
            this.flpPage.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpPage.Controls.Add(this.btnpage1);
            this.flpPage.Controls.Add(this.btnpage2);
            this.flpPage.Controls.Add(this.btnpage3);
            this.flpPage.Controls.Add(this.btnpage4);
            this.flpPage.Location = new System.Drawing.Point(30, 112);
            this.flpPage.Margin = new System.Windows.Forms.Padding(0);
            this.flpPage.Name = "flpPage";
            this.flpPage.Padding = new System.Windows.Forms.Padding(1);
            this.flpPage.Size = new System.Drawing.Size(178, 26);
            this.flpPage.TabIndex = 28;
            this.flpPage.WrapContents = false;
            // 
            // btnpage1
            // 
            this.btnpage1.BackgroundImage = global::GPMain.Properties.Resources.focus;
            this.btnpage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnpage1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnpage1.FlatAppearance.BorderSize = 0;
            this.btnpage1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnpage1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnpage1.ForeColor = System.Drawing.Color.Red;
            this.btnpage1.Location = new System.Drawing.Point(1, 1);
            this.btnpage1.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnpage1.Name = "btnpage1";
            this.btnpage1.Size = new System.Drawing.Size(43, 24);
            this.btnpage1.TabIndex = 23;
            this.btnpage1.Tag = "0";
            this.btnpage1.Text = "1";
            this.btnpage1.UseVisualStyleBackColor = true;
            this.btnpage1.ForeColorChanged += new System.EventHandler(this.btnpage1_ForeColorChanged);
            // 
            // btnpage2
            // 
            this.btnpage2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnpage2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnpage2.FlatAppearance.BorderSize = 0;
            this.btnpage2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnpage2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnpage2.ForeColor = System.Drawing.Color.Black;
            this.btnpage2.Location = new System.Drawing.Point(45, 1);
            this.btnpage2.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnpage2.Name = "btnpage2";
            this.btnpage2.Size = new System.Drawing.Size(43, 24);
            this.btnpage2.TabIndex = 23;
            this.btnpage2.Tag = "0";
            this.btnpage2.Text = "2";
            this.btnpage2.UseVisualStyleBackColor = true;
            this.btnpage2.ForeColorChanged += new System.EventHandler(this.btnpage1_ForeColorChanged);
            // 
            // btnpage3
            // 
            this.btnpage3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnpage3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnpage3.FlatAppearance.BorderSize = 0;
            this.btnpage3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnpage3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnpage3.ForeColor = System.Drawing.Color.Black;
            this.btnpage3.Location = new System.Drawing.Point(89, 1);
            this.btnpage3.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnpage3.Name = "btnpage3";
            this.btnpage3.Size = new System.Drawing.Size(43, 24);
            this.btnpage3.TabIndex = 23;
            this.btnpage3.Tag = "0";
            this.btnpage3.Text = "3";
            this.btnpage3.UseVisualStyleBackColor = true;
            this.btnpage3.ForeColorChanged += new System.EventHandler(this.btnpage1_ForeColorChanged);
            // 
            // btnpage4
            // 
            this.btnpage4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnpage4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnpage4.FlatAppearance.BorderSize = 0;
            this.btnpage4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnpage4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnpage4.ForeColor = System.Drawing.Color.Black;
            this.btnpage4.Location = new System.Drawing.Point(133, 1);
            this.btnpage4.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.btnpage4.Name = "btnpage4";
            this.btnpage4.Size = new System.Drawing.Size(43, 24);
            this.btnpage4.TabIndex = 23;
            this.btnpage4.Tag = "0";
            this.btnpage4.Text = "4";
            this.btnpage4.UseVisualStyleBackColor = true;
            this.btnpage4.ForeColorChanged += new System.EventHandler(this.btnpage1_ForeColorChanged);
            // 
            // lbltotalmember
            // 
            this.lbltotalmember.AutoSize = true;
            this.lbltotalmember.Depth = 0;
            this.lbltotalmember.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbltotalmember.Location = new System.Drawing.Point(150, 141);
            this.lbltotalmember.MouseState = MaterialSkin.MouseState.HOVER;
            this.lbltotalmember.Name = "lbltotalmember";
            this.lbltotalmember.Size = new System.Drawing.Size(10, 19);
            this.lbltotalmember.TabIndex = 27;
            this.lbltotalmember.Text = "0";
            this.lbltotalmember.TextChanged += new System.EventHandler(this.lbltotalmember_TextChanged);
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(3, 141);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(145, 19);
            this.materialLabel1.TabIndex = 27;
            this.materialLabel1.Text = "Tổng số thành viên: ";
            // 
            // cmbLiveOrDie
            // 
            this.cmbLiveOrDie.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.cmbLiveOrDie.Location = new System.Drawing.Point(3, 74);
            this.cmbLiveOrDie.MaxDropDownItems = 4;
            this.cmbLiveOrDie.MouseState = MaterialSkin.MouseState.OUT;
            this.cmbLiveOrDie.Name = "cmbLiveOrDie";
            this.cmbLiveOrDie.Size = new System.Drawing.Size(231, 35);
            this.cmbLiveOrDie.TabIndex = 26;
            this.cmbLiveOrDie.UseTallSize = false;
            // 
            // cmbGender
            // 
            this.cmbGender.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.cmbGender.Location = new System.Drawing.Point(3, 38);
            this.cmbGender.MaxDropDownItems = 4;
            this.cmbGender.MouseState = MaterialSkin.MouseState.OUT;
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(231, 35);
            this.cmbGender.TabIndex = 25;
            this.cmbGender.UseTallSize = false;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.SeaShell;
            this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1, 159);
            this.panel9.TabIndex = 4;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.SeaShell;
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(236, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(1, 159);
            this.panel8.TabIndex = 3;
            // 
            // txtKeyword
            // 
            this.txtKeyword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKeyword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtKeyword.Depth = 0;
            this.txtKeyword.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtKeyword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txtKeyword.Hint = "Từ khóa";
            this.txtKeyword.Location = new System.Drawing.Point(3, 2);
            this.txtKeyword.MaxLength = 50;
            this.txtKeyword.ModeNumber_Maximum = 999999;
            this.txtKeyword.MouseState = MaterialSkin.MouseState.OUT;
            this.txtKeyword.Multiline = false;
            this.txtKeyword.Name = "txtKeyword";
            this.txtKeyword.Size = new System.Drawing.Size(231, 36);
            this.txtKeyword.TabIndex = 24;
            this.txtKeyword.Text = "";
            this.txtKeyword.UseTallSize = false;
            this.txtKeyword.TextChanged += new System.EventHandler(this.txtKeyword_TextChanged);
            // 
            // btnnextpage
            // 
            this.btnnextpage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnnextpage.BackgroundImage = global::GPMain.Properties.Resources.next_page;
            this.btnnextpage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnnextpage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnnextpage.FlatAppearance.BorderSize = 0;
            this.btnnextpage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnnextpage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnnextpage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnnextpage.ForeColor = System.Drawing.Color.Black;
            this.btnnextpage.Location = new System.Drawing.Point(217, 113);
            this.btnnextpage.Name = "btnnextpage";
            this.btnnextpage.Size = new System.Drawing.Size(24, 24);
            this.btnnextpage.TabIndex = 23;
            this.btnnextpage.UseVisualStyleBackColor = true;
            this.btnnextpage.MouseLeave += new System.EventHandler(this.btnnextpage_MouseLeave);
            this.btnnextpage.MouseHover += new System.EventHandler(this.btnnextpage_MouseHover);
            // 
            // btnprepage
            // 
            this.btnprepage.BackgroundImage = global::GPMain.Properties.Resources.back_page;
            this.btnprepage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnprepage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnprepage.FlatAppearance.BorderSize = 0;
            this.btnprepage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnprepage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnprepage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnprepage.ForeColor = System.Drawing.Color.Black;
            this.btnprepage.Location = new System.Drawing.Point(-3, 113);
            this.btnprepage.Margin = new System.Windows.Forms.Padding(0);
            this.btnprepage.Name = "btnprepage";
            this.btnprepage.Size = new System.Drawing.Size(24, 24);
            this.btnprepage.TabIndex = 23;
            this.btnprepage.UseVisualStyleBackColor = true;
            this.btnprepage.MouseLeave += new System.EventHandler(this.btnprepage_MouseLeave);
            this.btnprepage.MouseHover += new System.EventHandler(this.btnprepage_MouseHover);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SeaShell;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 159);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(237, 1);
            this.panel2.TabIndex = 0;
            // 
            // pnlLayoutMember
            // 
            this.pnlLayoutMember.BackColor = System.Drawing.Color.White;
            this.pnlLayoutMember.Controls.Add(this.panel7);
            this.pnlLayoutMember.Controls.Add(this.flpMember);
            this.pnlLayoutMember.Controls.Add(this.panel6);
            this.pnlLayoutMember.Controls.Add(this.panel1);
            this.pnlLayoutMember.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLayoutMember.Location = new System.Drawing.Point(0, 160);
            this.pnlLayoutMember.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLayoutMember.Name = "pnlLayoutMember";
            this.pnlLayoutMember.Size = new System.Drawing.Size(237, 397);
            this.pnlLayoutMember.TabIndex = 3;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.SeaShell;
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(236, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(1, 396);
            this.panel7.TabIndex = 2;
            // 
            // flpMember
            // 
            this.flpMember.BackColor = System.Drawing.SystemColors.Control;
            this.flpMember.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.flpMember.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMember.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpMember.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.flpMember.Location = new System.Drawing.Point(1, 0);
            this.flpMember.Margin = new System.Windows.Forms.Padding(0);
            this.flpMember.Name = "flpMember";
            this.flpMember.Size = new System.Drawing.Size(236, 396);
            this.flpMember.TabIndex = 0;
            this.flpMember.WrapContents = false;
            this.flpMember.SizeChanged += new System.EventHandler(this.flpMember_SizeChanged);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.SeaShell;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(1, 396);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(236, 1);
            this.panel6.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.SeaShell;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1, 397);
            this.panel1.TabIndex = 1;
            // 
            // timerDelaySearchByKeyword
            // 
            this.timerDelaySearchByKeyword.Tick += new System.EventHandler(this.timerDelayFilter_Tick);
            // 
            // MenuMember
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.Controls.Add(this.pnlLayoutMember);
            this.Controls.Add(this.panelfilter);
            this.Name = "MenuMember";
            this.Size = new System.Drawing.Size(237, 557);
            this.Load += new System.EventHandler(this.MenuMember_Load);
            this.panelfilter.ResumeLayout(false);
            this.panelfilter.PerformLayout();
            this.flpPage.ResumeLayout(false);
            this.pnlLayoutMember.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelfilter;
        private System.Windows.Forms.FlowLayoutPanel flpPage;
        private System.Windows.Forms.Button btnpage1;
        private System.Windows.Forms.Button btnpage2;
        private System.Windows.Forms.Button btnpage3;
        private System.Windows.Forms.Button btnpage4;
        private MaterialLabel lbltotalmember;
        private MaterialLabel materialLabel1;
        private MaterialComboBox cmbLiveOrDie;
        private MaterialComboBox cmbGender;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel8;
        private MaterialTextBox txtKeyword;
        private System.Windows.Forms.Button btnnextpage;
        private System.Windows.Forms.Button btnprepage;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel pnlLayoutMember;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.FlowLayoutPanel flpMember;
        private System.Windows.Forms.Timer timerDelaySearchByKeyword;
    }
}
