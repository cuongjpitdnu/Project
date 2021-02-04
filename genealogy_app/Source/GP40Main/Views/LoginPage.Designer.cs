namespace GP40Main.Views
{
    partial class LoginPage
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
            this.txtPassword = new MaterialSkin.Controls.MaterialTextBox();
            this.txtUserName = new MaterialSkin.Controls.MaterialTextBox();
            this.btnExit = new MaterialSkin.Controls.MaterialButton();
            this.btnLogin = new MaterialSkin.Controls.MaterialButton();
            this.pictureLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Depth = 0;
            this.txtPassword.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPassword.Hint = "Mật khẩu";
            this.txtPassword.Location = new System.Drawing.Point(13, 220);
            this.txtPassword.MaxLength = 50;
            this.txtPassword.MouseState = MaterialSkin.MouseState.OUT;
            this.txtPassword.Multiline = false;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Password = true;
            this.txtPassword.Size = new System.Drawing.Size(303, 50);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "admin";
            // 
            // txtUserName
            // 
            this.txtUserName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserName.Depth = 0;
            this.txtUserName.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtUserName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtUserName.Hint = "Tên đăng nhập";
            this.txtUserName.Location = new System.Drawing.Point(13, 164);
            this.txtUserName.MaxLength = 50;
            this.txtUserName.MouseState = MaterialSkin.MouseState.OUT;
            this.txtUserName.Multiline = false;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(303, 50);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.Text = "admin";
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = false;
            this.btnExit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnExit.Depth = 0;
            this.btnExit.DrawShadows = true;
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnExit.HighEmphasis = true;
            this.btnExit.Icon = null;
            this.btnExit.Location = new System.Drawing.Point(168, 279);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnExit.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(105, 36);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Thoát";
            this.btnExit.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnExit.UseAccentColor = true;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.btnLogin.Depth = 0;
            this.btnLogin.DrawShadows = true;
            this.btnLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnLogin.HighEmphasis = true;
            this.btnLogin.Icon = null;
            this.btnLogin.Location = new System.Drawing.Point(55, 279);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnLogin.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(105, 36);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Outlined;
            this.btnLogin.UseAccentColor = true;
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // pictureLogo
            // 
            this.pictureLogo.Image = global::GP40Main.Properties.Resources.compary_logo;
            this.pictureLogo.Location = new System.Drawing.Point(2, 3);
            this.pictureLogo.Name = "pictureLogo";
            this.pictureLogo.Size = new System.Drawing.Size(324, 155);
            this.pictureLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureLogo.TabIndex = 7;
            this.pictureLogo.TabStop = false;
            // 
            // LoginPage
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.PeachPuff;
            this.CancelButton = this.btnExit;
            this.ComfirmMsgExit = "Bạn có chắc muốn thoát phần mềm?";
            this.Controls.Add(this.pictureLogo);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.MaximizeBox = false;
            this.Name = "LoginPage";
            this.Sizable = false;
            this.Size = new System.Drawing.Size(328, 326);
            ((System.ComponentModel.ISupportInitialize)(this.pictureLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialTextBox txtPassword;
        private MaterialSkin.Controls.MaterialTextBox txtUserName;
        private MaterialSkin.Controls.MaterialButton btnExit;
        private MaterialSkin.Controls.MaterialButton btnLogin;
        private System.Windows.Forms.PictureBox pictureLogo;
    }
}
