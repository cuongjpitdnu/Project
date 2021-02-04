namespace GPMain.Views.Tree.Build
{
    partial class BuildViewer
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
            this.pnlSpouse = new System.Windows.Forms.Panel();
            this.flpSpouse = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlSpouseBottom = new System.Windows.Forms.Panel();
            this.lbllistspouse = new System.Windows.Forms.Label();
            this.btnlistSpouse = new System.Windows.Forms.Button();
            this.btnaddSpouse = new System.Windows.Forms.Button();
            this.pnlSpouseTop = new System.Windows.Forms.Panel();
            this.lblinfospouse = new System.Windows.Forms.Label();
            this.pnlMainMember = new System.Windows.Forms.Panel();
            this.btnCancelRelationship = new System.Windows.Forms.Button();
            this.btnaddChild = new System.Windows.Forms.Button();
            this.pnlContainerChild = new System.Windows.Forms.Panel();
            this.flpChild = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlSpouse.SuspendLayout();
            this.pnlSpouseBottom.SuspendLayout();
            this.pnlSpouseTop.SuspendLayout();
            this.pnlMainMember.SuspendLayout();
            this.pnlContainerChild.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlSpouse
            // 
            this.pnlSpouse.AutoSize = true;
            this.pnlSpouse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlSpouse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSpouse.Controls.Add(this.flpSpouse);
            this.pnlSpouse.Controls.Add(this.pnlSpouseBottom);
            this.pnlSpouse.Controls.Add(this.pnlSpouseTop);
            this.pnlSpouse.Location = new System.Drawing.Point(364, 12);
            this.pnlSpouse.Name = "pnlSpouse";
            this.pnlSpouse.Size = new System.Drawing.Size(300, 122);
            this.pnlSpouse.TabIndex = 0;
            // 
            // flpSpouse
            // 
            this.flpSpouse.AutoScroll = true;
            this.flpSpouse.AutoSize = true;
            this.flpSpouse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpSpouse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpSpouse.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpSpouse.Location = new System.Drawing.Point(0, 45);
            this.flpSpouse.MinimumSize = new System.Drawing.Size(298, 25);
            this.flpSpouse.Name = "flpSpouse";
            this.flpSpouse.Size = new System.Drawing.Size(298, 25);
            this.flpSpouse.TabIndex = 2;
            this.flpSpouse.WrapContents = false;
            // 
            // pnlSpouseBottom
            // 
            this.pnlSpouseBottom.Controls.Add(this.lbllistspouse);
            this.pnlSpouseBottom.Controls.Add(this.btnlistSpouse);
            this.pnlSpouseBottom.Controls.Add(this.btnaddSpouse);
            this.pnlSpouseBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSpouseBottom.Location = new System.Drawing.Point(0, 70);
            this.pnlSpouseBottom.Name = "pnlSpouseBottom";
            this.pnlSpouseBottom.Size = new System.Drawing.Size(298, 50);
            this.pnlSpouseBottom.TabIndex = 1;
            // 
            // lbllistspouse
            // 
            this.lbllistspouse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbllistspouse.AutoSize = true;
            this.lbllistspouse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbllistspouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbllistspouse.ForeColor = System.Drawing.Color.Blue;
            this.lbllistspouse.Location = new System.Drawing.Point(179, 29);
            this.lbllistspouse.Name = "lbllistspouse";
            this.lbllistspouse.Size = new System.Drawing.Size(74, 16);
            this.lbllistspouse.TabIndex = 1;
            this.lbllistspouse.Text = "Danh Sách";
            // 
            // btnlistSpouse
            // 
            this.btnlistSpouse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnlistSpouse.BackgroundImage = global::GPMain.Properties.Resources.list3;
            this.btnlistSpouse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnlistSpouse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnlistSpouse.FlatAppearance.BorderSize = 0;
            this.btnlistSpouse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnlistSpouse.Location = new System.Drawing.Point(259, 6);
            this.btnlistSpouse.Name = "btnlistSpouse";
            this.btnlistSpouse.Size = new System.Drawing.Size(36, 39);
            this.btnlistSpouse.TabIndex = 1;
            this.btnlistSpouse.UseVisualStyleBackColor = true;
            // 
            // btnaddSpouse
            // 
            this.btnaddSpouse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnaddSpouse.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnaddSpouse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnaddSpouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnaddSpouse.ForeColor = System.Drawing.Color.Blue;
            this.btnaddSpouse.Image = global::GPMain.Properties.Resources.add_user;
            this.btnaddSpouse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnaddSpouse.Location = new System.Drawing.Point(3, 5);
            this.btnaddSpouse.Name = "btnaddSpouse";
            this.btnaddSpouse.Size = new System.Drawing.Size(150, 40);
            this.btnaddSpouse.TabIndex = 1;
            this.btnaddSpouse.Text = "Thêm vợ";
            this.btnaddSpouse.UseVisualStyleBackColor = true;
            // 
            // pnlSpouseTop
            // 
            this.pnlSpouseTop.Controls.Add(this.lblinfospouse);
            this.pnlSpouseTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSpouseTop.Location = new System.Drawing.Point(0, 0);
            this.pnlSpouseTop.Name = "pnlSpouseTop";
            this.pnlSpouseTop.Size = new System.Drawing.Size(298, 45);
            this.pnlSpouseTop.TabIndex = 0;
            // 
            // lblinfospouse
            // 
            this.lblinfospouse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblinfospouse.BackColor = System.Drawing.Color.Transparent;
            this.lblinfospouse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblinfospouse.ForeColor = System.Drawing.Color.Black;
            this.lblinfospouse.Location = new System.Drawing.Point(3, 3);
            this.lblinfospouse.Name = "lblinfospouse";
            this.lblinfospouse.Size = new System.Drawing.Size(292, 40);
            this.lblinfospouse.TabIndex = 0;
            this.lblinfospouse.Text = "Thông tin thành viên";
            // 
            // pnlMainMember
            // 
            this.pnlMainMember.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMainMember.Controls.Add(this.btnCancelRelationship);
            this.pnlMainMember.Controls.Add(this.btnaddChild);
            this.pnlMainMember.Location = new System.Drawing.Point(13, 12);
            this.pnlMainMember.MinimumSize = new System.Drawing.Size(300, 176);
            this.pnlMainMember.Name = "pnlMainMember";
            this.pnlMainMember.Size = new System.Drawing.Size(300, 176);
            this.pnlMainMember.TabIndex = 0;
            // 
            // btnCancelRelationship
            // 
            this.btnCancelRelationship.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelRelationship.BackgroundImage = global::GPMain.Properties.Resources.cancel;
            this.btnCancelRelationship.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancelRelationship.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancelRelationship.Location = new System.Drawing.Point(271, 3);
            this.btnCancelRelationship.Name = "btnCancelRelationship";
            this.btnCancelRelationship.Size = new System.Drawing.Size(24, 24);
            this.btnCancelRelationship.TabIndex = 0;
            this.btnCancelRelationship.UseVisualStyleBackColor = true;
            // 
            // btnaddChild
            // 
            this.btnaddChild.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnaddChild.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnaddChild.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnaddChild.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnaddChild.ForeColor = System.Drawing.Color.Blue;
            this.btnaddChild.Image = global::GPMain.Properties.Resources.add_user;
            this.btnaddChild.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnaddChild.Location = new System.Drawing.Point(3, 129);
            this.btnaddChild.Name = "btnaddChild";
            this.btnaddChild.Size = new System.Drawing.Size(150, 40);
            this.btnaddChild.TabIndex = 1;
            this.btnaddChild.Text = "Thêm con";
            this.btnaddChild.UseVisualStyleBackColor = true;
            // 
            // pnlContainerChild
            // 
            this.pnlContainerChild.AutoScroll = true;
            this.pnlContainerChild.Controls.Add(this.flpChild);
            this.pnlContainerChild.Location = new System.Drawing.Point(13, 216);
            this.pnlContainerChild.Name = "pnlContainerChild";
            this.pnlContainerChild.Size = new System.Drawing.Size(651, 120);
            this.pnlContainerChild.TabIndex = 1;
            // 
            // flpChild
            // 
            this.flpChild.AutoSize = true;
            this.flpChild.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpChild.Location = new System.Drawing.Point(3, 3);
            this.flpChild.MinimumSize = new System.Drawing.Size(100, 100);
            this.flpChild.Name = "flpChild";
            this.flpChild.Size = new System.Drawing.Size(100, 100);
            this.flpChild.TabIndex = 0;
            // 
            // BuildViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlContainerChild);
            this.Controls.Add(this.pnlMainMember);
            this.Controls.Add(this.pnlSpouse);
            this.Name = "BuildViewer";
            this.Size = new System.Drawing.Size(678, 341);
            this.pnlSpouse.ResumeLayout(false);
            this.pnlSpouse.PerformLayout();
            this.pnlSpouseBottom.ResumeLayout(false);
            this.pnlSpouseBottom.PerformLayout();
            this.pnlSpouseTop.ResumeLayout(false);
            this.pnlMainMember.ResumeLayout(false);
            this.pnlContainerChild.ResumeLayout(false);
            this.pnlContainerChild.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlSpouse;
        private System.Windows.Forms.Panel pnlMainMember;
        private System.Windows.Forms.FlowLayoutPanel flpSpouse;
        private System.Windows.Forms.Panel pnlSpouseBottom;
        private System.Windows.Forms.Panel pnlSpouseTop;
        private System.Windows.Forms.Panel pnlContainerChild;
        private System.Windows.Forms.FlowLayoutPanel flpChild;
        private System.Windows.Forms.Button btnCancelRelationship;
        private System.Windows.Forms.Button btnaddSpouse;
        private System.Windows.Forms.Button btnlistSpouse;
        private System.Windows.Forms.Label lbllistspouse;
        private System.Windows.Forms.Label lblinfospouse;
        private System.Windows.Forms.Button btnaddChild;
    }
}
