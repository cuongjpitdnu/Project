namespace GPMain.Views.Tree.MenuItems
{
    partial class OptionDrawTreeItem
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlbordertop = new System.Windows.Forms.Panel();
            this.pnlborderbottom = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ckshowfamilylevel = new MaterialSkin.Controls.MaterialCheckbox();
            this.ckshowgender = new MaterialSkin.Controls.MaterialCheckbox();
            this.rdnormal = new MaterialSkin.Controls.MaterialRadioButton();
            this.rdturnright = new MaterialSkin.Controls.MaterialRadioButton();
            this.rdturnleft = new MaterialSkin.Controls.MaterialRadioButton();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.panel1.SuspendLayout();
            this.pnlbordertop.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.panel1.Controls.Add(this.pnlbordertop);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 248);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 1);
            this.panel1.TabIndex = 12;
            // 
            // pnlbordertop
            // 
            this.pnlbordertop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlbordertop.Controls.Add(this.pnlborderbottom);
            this.pnlbordertop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlbordertop.Location = new System.Drawing.Point(0, 0);
            this.pnlbordertop.Name = "pnlbordertop";
            this.pnlbordertop.Size = new System.Drawing.Size(200, 1);
            this.pnlbordertop.TabIndex = 10;
            // 
            // pnlborderbottom
            // 
            this.pnlborderbottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlborderbottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlborderbottom.Location = new System.Drawing.Point(0, 0);
            this.pnlborderbottom.Name = "pnlborderbottom";
            this.pnlborderbottom.Size = new System.Drawing.Size(200, 1);
            this.pnlborderbottom.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 1);
            this.panel2.TabIndex = 13;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 1);
            this.panel3.TabIndex = 10;
            // 
            // ckshowfamilylevel
            // 
            this.ckshowfamilylevel.AutoSize = true;
            this.ckshowfamilylevel.Depth = 0;
            this.ckshowfamilylevel.Location = new System.Drawing.Point(6, 30);
            this.ckshowfamilylevel.Margin = new System.Windows.Forms.Padding(0);
            this.ckshowfamilylevel.MouseLocation = new System.Drawing.Point(-1, -1);
            this.ckshowfamilylevel.MouseState = MaterialSkin.MouseState.HOVER;
            this.ckshowfamilylevel.Name = "ckshowfamilylevel";
            this.ckshowfamilylevel.Ripple = true;
            this.ckshowfamilylevel.Size = new System.Drawing.Size(116, 37);
            this.ckshowfamilylevel.TabIndex = 14;
            this.ckshowfamilylevel.Text = "Hiển thị đời";
            this.ckshowfamilylevel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ckshowfamilylevel.UseVisualStyleBackColor = true;
            // 
            // ckshowgender
            // 
            this.ckshowgender.AutoSize = true;
            this.ckshowgender.Depth = 0;
            this.ckshowgender.Location = new System.Drawing.Point(6, 67);
            this.ckshowgender.Margin = new System.Windows.Forms.Padding(0);
            this.ckshowgender.MouseLocation = new System.Drawing.Point(-1, -1);
            this.ckshowgender.MouseState = MaterialSkin.MouseState.HOVER;
            this.ckshowgender.Name = "ckshowgender";
            this.ckshowgender.Ripple = true;
            this.ckshowgender.Size = new System.Drawing.Size(150, 37);
            this.ckshowgender.TabIndex = 14;
            this.ckshowgender.Text = "Hiển thị giới tính";
            this.ckshowgender.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.ckshowgender.UseVisualStyleBackColor = true;
            // 
            // rdnormal
            // 
            this.rdnormal.AutoSize = true;
            this.rdnormal.Checked = true;
            this.rdnormal.Depth = 0;
            this.rdnormal.Location = new System.Drawing.Point(6, 130);
            this.rdnormal.Margin = new System.Windows.Forms.Padding(0);
            this.rdnormal.MouseLocation = new System.Drawing.Point(-1, -1);
            this.rdnormal.MouseState = MaterialSkin.MouseState.HOVER;
            this.rdnormal.Name = "rdnormal";
            this.rdnormal.Ripple = true;
            this.rdnormal.Size = new System.Drawing.Size(122, 37);
            this.rdnormal.TabIndex = 15;
            this.rdnormal.TabStop = true;
            this.rdnormal.Text = "Bình thường";
            this.rdnormal.UseVisualStyleBackColor = true;
            // 
            // rdturnright
            // 
            this.rdturnright.AutoSize = true;
            this.rdturnright.Depth = 0;
            this.rdturnright.Location = new System.Drawing.Point(6, 167);
            this.rdturnright.Margin = new System.Windows.Forms.Padding(0);
            this.rdturnright.MouseLocation = new System.Drawing.Point(-1, -1);
            this.rdturnright.MouseState = MaterialSkin.MouseState.HOVER;
            this.rdturnright.Name = "rdturnright";
            this.rdturnright.Ripple = true;
            this.rdturnright.Size = new System.Drawing.Size(106, 37);
            this.rdturnright.TabIndex = 15;
            this.rdturnright.TabStop = true;
            this.rdturnright.Text = "Xoay phải";
            this.rdturnright.UseVisualStyleBackColor = true;
            // 
            // rdturnleft
            // 
            this.rdturnleft.AutoSize = true;
            this.rdturnleft.Depth = 0;
            this.rdturnleft.Location = new System.Drawing.Point(6, 204);
            this.rdturnleft.Margin = new System.Windows.Forms.Padding(0);
            this.rdturnleft.MouseLocation = new System.Drawing.Point(-1, -1);
            this.rdturnleft.MouseState = MaterialSkin.MouseState.HOVER;
            this.rdturnleft.Name = "rdturnleft";
            this.rdturnleft.Ripple = true;
            this.rdturnleft.Size = new System.Drawing.Size(98, 37);
            this.rdturnleft.TabIndex = 15;
            this.rdturnleft.TabStop = true;
            this.rdturnleft.Text = "Xoay trái";
            this.rdturnleft.UseVisualStyleBackColor = true;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(3, 111);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(119, 19);
            this.materialLabel1.TabIndex = 16;
            this.materialLabel1.Text = "Kiểu chữ hiển thị";
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.Location = new System.Drawing.Point(3, 10);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(121, 19);
            this.materialLabel2.TabIndex = 16;
            this.materialLabel2.Text = "Nội dung hiển thị";
            // 
            // OptionDrawTreeItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.rdturnleft);
            this.Controls.Add(this.rdturnright);
            this.Controls.Add(this.rdnormal);
            this.Controls.Add(this.ckshowgender);
            this.Controls.Add(this.ckshowfamilylevel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "OptionDrawTreeItem";
            this.Size = new System.Drawing.Size(200, 249);
            this.panel1.ResumeLayout(false);
            this.pnlbordertop.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlbordertop;
        private System.Windows.Forms.Panel pnlborderbottom;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private MaterialSkin.Controls.MaterialCheckbox ckshowfamilylevel;
        private MaterialSkin.Controls.MaterialCheckbox ckshowgender;
        private MaterialSkin.Controls.MaterialRadioButton rdnormal;
        private MaterialSkin.Controls.MaterialRadioButton rdturnright;
        private MaterialSkin.Controls.MaterialRadioButton rdturnleft;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
    }
}
