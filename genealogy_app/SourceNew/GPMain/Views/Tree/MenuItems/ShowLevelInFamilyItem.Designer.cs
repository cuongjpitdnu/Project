namespace GPMain.Views.Tree.MenuItems
{
    partial class ShowLevelInFamilyItem
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
            this.pnlborderbottom = new System.Windows.Forms.Panel();
            this.pnlbordertop = new System.Windows.Forms.Panel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.cblevelInFamily = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // pnlborderbottom
            // 
            this.pnlborderbottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlborderbottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlborderbottom.Location = new System.Drawing.Point(0, 65);
            this.pnlborderbottom.Name = "pnlborderbottom";
            this.pnlborderbottom.Size = new System.Drawing.Size(200, 1);
            this.pnlborderbottom.TabIndex = 10;
            // 
            // pnlbordertop
            // 
            this.pnlbordertop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.pnlbordertop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlbordertop.Location = new System.Drawing.Point(0, 0);
            this.pnlbordertop.Name = "pnlbordertop";
            this.pnlbordertop.Size = new System.Drawing.Size(200, 1);
            this.pnlbordertop.TabIndex = 11;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialLabel1.Location = new System.Drawing.Point(9, 23);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(107, 19);
            this.materialLabel1.TabIndex = 12;
            this.materialLabel1.Text = "Số đời hiển thị:";
            this.materialLabel1.UseAccent = true;
            // 
            // cblevelInFamily
            // 
            this.cblevelInFamily.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cblevelInFamily.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cblevelInFamily.FormattingEnabled = true;
            this.cblevelInFamily.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.cblevelInFamily.Location = new System.Drawing.Point(122, 23);
            this.cblevelInFamily.Name = "cblevelInFamily";
            this.cblevelInFamily.Size = new System.Drawing.Size(67, 21);
            this.cblevelInFamily.TabIndex = 15;
            // 
            // ShowLevelInFamilyItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cblevelInFamily);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.pnlbordertop);
            this.Controls.Add(this.pnlborderbottom);
            this.Name = "ShowLevelInFamilyItem";
            this.Size = new System.Drawing.Size(200, 66);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlborderbottom;
        private System.Windows.Forms.Panel pnlbordertop;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        public System.Windows.Forms.ComboBox cblevelInFamily;
    }
}
