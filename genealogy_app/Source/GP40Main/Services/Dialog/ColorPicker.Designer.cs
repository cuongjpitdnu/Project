namespace GP40Main.Services.Dialog
{
    partial class ColorPicker
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
            this.materialColorPicker1 = new MaterialSkin.Controls.MaterialColorPicker();
            this.btnOk = new MaterialSkin.Controls.MaterialButton();
            this.SuspendLayout();
            // 
            // materialColorPicker1
            // 
            this.materialColorPicker1.Depth = 0;
            this.materialColorPicker1.Location = new System.Drawing.Point(0, 0);
            this.materialColorPicker1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialColorPicker1.Name = "materialColorPicker1";
            this.materialColorPicker1.Size = new System.Drawing.Size(248, 425);
            this.materialColorPicker1.TabIndex = 0;
            this.materialColorPicker1.Text = "materialColorPicker1";
            this.materialColorPicker1.Value = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(64)))), ((int)(((byte)(129)))));
            // 
            // btnOk
            // 
            this.btnOk.AutoSize = false;
            this.btnOk.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnOk.Depth = 0;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnOk.DrawShadows = true;
            this.btnOk.HighEmphasis = true;
            this.btnOk.Icon = null;
            this.btnOk.Location = new System.Drawing.Point(0, 423);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnOk.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(248, 37);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Chọn";
            this.btnOk.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Text;
            this.btnOk.UseAccentColor = false;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ColorPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.materialColorPicker1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorPicker";
            this.Size = new System.Drawing.Size(248, 460);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TitleBar = " ";
            this.Load += new System.EventHandler(this.ColorPicker_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialColorPicker materialColorPicker1;
        private MaterialSkin.Controls.MaterialButton btnOk;
    }
}
