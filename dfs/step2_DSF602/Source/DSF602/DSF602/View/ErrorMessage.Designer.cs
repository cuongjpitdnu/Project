namespace DSF602.View
{
    partial class ErrorMessage
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
            this.lblMsgError = new System.Windows.Forms.Label();
            this.txtMsgError = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblMsgError
            // 
            this.lblMsgError.AutoSize = true;
            this.lblMsgError.Location = new System.Drawing.Point(17, 14);
            this.lblMsgError.Name = "lblMsgError";
            this.lblMsgError.Size = new System.Drawing.Size(102, 20);
            this.lblMsgError.TabIndex = 0;
            this.lblMsgError.Text = "Error Message:";
            // 
            // txtMsgError
            // 
            this.txtMsgError.Location = new System.Drawing.Point(21, 37);
            this.txtMsgError.Multiline = true;
            this.txtMsgError.Name = "txtMsgError";
            this.txtMsgError.ReadOnly = true;
            this.txtMsgError.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtMsgError.Size = new System.Drawing.Size(492, 204);
            this.txtMsgError.TabIndex = 1;
            // 
            // frmMsgError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 253);
            this.Controls.Add(this.txtMsgError);
            this.Controls.Add(this.lblMsgError);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(351, 123);
            this.Name = "frmMsgError";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error Message Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ErrorMessage_FormClosed);
            this.Load += new System.EventHandler(this.ErrorMessage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMsgError;
        private System.Windows.Forms.TextBox txtMsgError;
    }
}