namespace DSF602.View
{
    partial class PromptDialog
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
            this.txtInputValue = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.lbTitle = new System.Windows.Forms.Label();
            this.lbAlert = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtInputValue
            // 
            this.txtInputValue.Location = new System.Drawing.Point(25, 39);
            this.txtInputValue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtInputValue.Name = "txtInputValue";
            this.txtInputValue.Size = new System.Drawing.Size(375, 26);
            this.txtInputValue.TabIndex = 0;
            this.txtInputValue.UseSystemPasswordChar = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(166, 95);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 35);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lbTitle
            // 
            this.lbTitle.AutoSize = true;
            this.lbTitle.Location = new System.Drawing.Point(21, 14);
            this.lbTitle.Name = "lbTitle";
            this.lbTitle.Size = new System.Drawing.Size(82, 20);
            this.lbTitle.TabIndex = 2;
            this.lbTitle.Text = "msg content";
            // 
            // lbAlert
            // 
            this.lbAlert.AutoSize = true;
            this.lbAlert.ForeColor = System.Drawing.Color.Red;
            this.lbAlert.Location = new System.Drawing.Point(21, 70);
            this.lbAlert.Name = "lbAlert";
            this.lbAlert.Size = new System.Drawing.Size(165, 20);
            this.lbAlert.TabIndex = 3;
            this.lbAlert.Text = "Bạn chưa nhập password";
            this.lbAlert.Visible = false;
            // 
            // PromptDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 145);
            this.Controls.Add(this.lbAlert);
            this.Controls.Add(this.lbTitle);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtInputValue);
            this.DoubleBuffered = true;
            this.EscToClose = true;
            this.Font = new System.Drawing.Font("Arial Narrow", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(351, 124);
            this.Name = "PromptDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtInputValue;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lbTitle;
        private System.Windows.Forms.Label lbAlert;
    }
}