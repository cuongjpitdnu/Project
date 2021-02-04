using GP40Main.Themes.Controls;

namespace GP40Main.Services.Dialog
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
            this.progressBarCommon = new GP40Main.Themes.Controls.ProgressBarCommon();
            this.txtTitle = new MaterialSkin.Controls.MaterialLabel();
            this.SuspendLayout();
            // 
            // progressBarCommon
            // 
            this.progressBarCommon.Location = new System.Drawing.Point(18, 56);
            this.progressBarCommon.Name = "progressBarCommon";
            this.progressBarCommon.Size = new System.Drawing.Size(371, 23);
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
            this.txtTitle.Size = new System.Drawing.Size(400, 29);
            this.txtTitle.TabIndex = 1;
            this.txtTitle.Text = "materialLabel1";
            this.txtTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.txtTitle.UseWaitCursor = true;
            // 
            // WaitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.progressBarCommon);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WaitForm";
            this.Sizable = false;
            this.Size = new System.Drawing.Size(406, 92);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.UseWaitCursor = true;
            this.ResumeLayout(false);

        }

        #endregion

        private ProgressBarCommon progressBarCommon;
        private MaterialSkin.Controls.MaterialLabel txtTitle;
    }
}
