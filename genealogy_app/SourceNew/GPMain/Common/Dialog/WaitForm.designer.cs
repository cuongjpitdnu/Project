namespace GPMain.Common.Dialog
{
    partial class WaitForm
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
            this.progressBarCommon = new GPMain.Common.Dialog.ProgressBarCommon();
            this.txtTitle = new MaterialSkin.Controls.MaterialLabel();
            this.lblcount = new System.Windows.Forms.Label();
            this.lbledge = new System.Windows.Forms.Label();
            this.lbltotal = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBarCommon
            // 
            this.progressBarCommon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBarCommon.Location = new System.Drawing.Point(18, 87);
            this.progressBarCommon.Name = "progressBarCommon";
            this.progressBarCommon.Size = new System.Drawing.Size(417, 23);
            this.progressBarCommon.TabIndex = 0;
            this.progressBarCommon.UseWaitCursor = true;
            // 
            // txtTitle
            // 
            this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTitle.Depth = 0;
            this.txtTitle.Font = new System.Drawing.Font("Roboto", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txtTitle.FontType = MaterialSkin.MaterialSkinManager.fontType.H5;
            this.txtTitle.Location = new System.Drawing.Point(3, 14);
            this.txtTitle.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(447, 60);
            this.txtTitle.TabIndex = 1;
            this.txtTitle.Text = "materialLabel1";
            this.txtTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtTitle.UseWaitCursor = true;
            // 
            // lblcount
            // 
            this.lblcount.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblcount.AutoSize = true;
            this.lblcount.Location = new System.Drawing.Point(201, 115);
            this.lblcount.Name = "lblcount";
            this.lblcount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblcount.Size = new System.Drawing.Size(13, 13);
            this.lblcount.TabIndex = 2;
            this.lblcount.Text = "0";
            this.lblcount.UseWaitCursor = true;
            this.lblcount.Click += new System.EventHandler(this.lblcount_Click);
            // 
            // lbledge
            // 
            this.lbledge.AutoSize = true;
            this.lbledge.Location = new System.Drawing.Point(220, 115);
            this.lbledge.Name = "lbledge";
            this.lbledge.Size = new System.Drawing.Size(12, 13);
            this.lbledge.TabIndex = 3;
            this.lbledge.Text = "/";
            this.lbledge.UseWaitCursor = true;
            // 
            // lbltotal
            // 
            this.lbltotal.AutoSize = true;
            this.lbltotal.Location = new System.Drawing.Point(238, 115);
            this.lbltotal.Name = "lbltotal";
            this.lbltotal.Size = new System.Drawing.Size(13, 13);
            this.lbltotal.TabIndex = 4;
            this.lbltotal.Text = "0";
            this.lbltotal.UseWaitCursor = true;
            // 
            // WaitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbltotal);
            this.Controls.Add(this.lbledge);
            this.Controls.Add(this.lblcount);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.progressBarCommon);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WaitForm";
            this.Sizable = false;
            this.Size = new System.Drawing.Size(453, 133);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.UseWaitCursor = true;
            this.Load += new System.EventHandler(this.WaitForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ProgressBarCommon progressBarCommon;
        private MaterialSkin.Controls.MaterialLabel txtTitle;
        private System.Windows.Forms.Label lblcount;
        private System.Windows.Forms.Label lbledge;
        private System.Windows.Forms.Label lbltotal;
    }
}
