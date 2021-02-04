using MaterialSkin;

namespace GPMain.Common.Events
{
    partial class CalendarShort
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
            this.btncancel = new MaterialSkin.Controls.MaterialButton();
            this.btnselect = new MaterialSkin.Controls.MaterialButton();
            this.txtyear = new MaterialSkin.Controls.MaterialTextBox();
            this.cbmonth = new MaterialSkin.Controls.MaterialComboBox();
            this.cbday = new MaterialSkin.Controls.MaterialComboBox();
            this.numHour = new MaterialSkin.Controls.MaterialTextBox();
            this.numMinute = new MaterialSkin.Controls.MaterialTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btncancel
            // 
            this.btncancel.AutoSize = false;
            this.btncancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btncancel.Depth = 0;
            this.btncancel.DrawShadows = true;
            this.btncancel.HighEmphasis = true;
            this.btncancel.Icon = null;
            this.btncancel.Location = new System.Drawing.Point(388, 144);
            this.btncancel.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btncancel.MouseState = MaterialSkin.MouseState.HOVER;
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(128, 36);
            this.btncancel.TabIndex = 8;
            this.btncancel.Text = "Đóng";
            this.btncancel.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btncancel.UseAccentColor = false;
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // btnselect
            // 
            this.btnselect.AutoSize = false;
            this.btnselect.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnselect.Depth = 0;
            this.btnselect.DrawShadows = true;
            this.btnselect.HighEmphasis = true;
            this.btnselect.Icon = null;
            this.btnselect.Location = new System.Drawing.Point(238, 144);
            this.btnselect.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnselect.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnselect.Name = "btnselect";
            this.btnselect.Size = new System.Drawing.Size(128, 36);
            this.btnselect.TabIndex = 9;
            this.btnselect.Text = "Chọn";
            this.btnselect.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btnselect.UseAccentColor = false;
            this.btnselect.UseVisualStyleBackColor = true;
            this.btnselect.Click += new System.EventHandler(this.btnselect_Click);
            // 
            // txtyear
            // 
            this.txtyear.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtyear.Depth = 0;
            this.txtyear.Font = new System.Drawing.Font("Roboto", 12F);
            this.txtyear.Location = new System.Drawing.Point(101, 86);
            this.txtyear.MaxLength = 50;
            this.txtyear.ModeNumber = true;
            this.txtyear.ModeNumber_CheckMaximum = true;
            this.txtyear.ModeNumber_CheckMinimum = true;
            this.txtyear.ModeNumber_Maximum = 2999;
            this.txtyear.ModeNumber_Minimum = 1200;
            this.txtyear.MouseState = MaterialSkin.MouseState.OUT;
            this.txtyear.Multiline = false;
            this.txtyear.Name = "txtyear";
            this.txtyear.Size = new System.Drawing.Size(87, 36);
            this.txtyear.TabIndex = 10;
            this.txtyear.Text = "";
            this.txtyear.UseTallSize = false;
            this.txtyear.TextChanged += new System.EventHandler(this.txtyear_TextChanged);
            // 
            // cbmonth
            // 
            this.cbmonth.AutoResize = false;
            this.cbmonth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cbmonth.Depth = 0;
            this.cbmonth.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbmonth.DropDownHeight = 118;
            this.cbmonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbmonth.DropDownWidth = 121;
            this.cbmonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cbmonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbmonth.FormattingEnabled = true;
            this.cbmonth.IntegralHeight = false;
            this.cbmonth.ItemHeight = 29;
            this.cbmonth.Location = new System.Drawing.Point(265, 87);
            this.cbmonth.MaxDropDownItems = 4;
            this.cbmonth.MouseState = MaterialSkin.MouseState.OUT;
            this.cbmonth.Name = "cbmonth";
            this.cbmonth.Size = new System.Drawing.Size(87, 35);
            this.cbmonth.TabIndex = 11;
            this.cbmonth.UseTallSize = false;
            this.cbmonth.SelectedIndexChanged += new System.EventHandler(this.cbmonth_SelectedIndexChanged);
            this.cbmonth.TextChanged += new System.EventHandler(this.cbmonth_TextChanged);
            // 
            // cbday
            // 
            this.cbday.AutoResize = false;
            this.cbday.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cbday.Depth = 0;
            this.cbday.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbday.DropDownHeight = 118;
            this.cbday.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbday.DropDownWidth = 121;
            this.cbday.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.cbday.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbday.FormattingEnabled = true;
            this.cbday.IntegralHeight = false;
            this.cbday.ItemHeight = 29;
            this.cbday.Location = new System.Drawing.Point(429, 87);
            this.cbday.MaxDropDownItems = 4;
            this.cbday.MouseState = MaterialSkin.MouseState.OUT;
            this.cbday.Name = "cbday";
            this.cbday.Size = new System.Drawing.Size(87, 35);
            this.cbday.TabIndex = 11;
            this.cbday.UseTallSize = false;
            // 
            // numHour
            // 
            this.numHour.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numHour.Depth = 0;
            this.numHour.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.numHour.Location = new System.Drawing.Point(101, 24);
            this.numHour.MaxLength = 2;
            this.numHour.ModeNumber = true;
            this.numHour.ModeNumber_CheckMaximum = true;
            this.numHour.ModeNumber_CheckMinimum = true;
            this.numHour.ModeNumber_Maximum = 23;
            this.numHour.MouseState = MaterialSkin.MouseState.OUT;
            this.numHour.Multiline = false;
            this.numHour.Name = "numHour";
            this.numHour.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.numHour.Size = new System.Drawing.Size(87, 36);
            this.numHour.TabIndex = 10;
            this.numHour.Text = "0";
            this.numHour.UseTallSize = false;
            // 
            // numMinute
            // 
            this.numMinute.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numMinute.Depth = 0;
            this.numMinute.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.numMinute.Location = new System.Drawing.Point(265, 24);
            this.numMinute.MaxLength = 2;
            this.numMinute.ModeNumber = true;
            this.numMinute.ModeNumber_CheckMaximum = true;
            this.numMinute.ModeNumber_CheckMinimum = true;
            this.numMinute.ModeNumber_Maximum = 59;
            this.numMinute.MouseState = MaterialSkin.MouseState.OUT;
            this.numMinute.Multiline = false;
            this.numMinute.Name = "numMinute";
            this.numMinute.Size = new System.Drawing.Size(87, 36);
            this.numMinute.TabIndex = 10;
            this.numMinute.Text = "0";
            this.numMinute.UseTallSize = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(4, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 24);
            this.label1.TabIndex = 13;
            this.label1.Text = "Thời gian:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Location = new System.Drawing.Point(194, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 24);
            this.label2.TabIndex = 13;
            this.label2.Text = "Giờ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DimGray;
            this.label3.Location = new System.Drawing.Point(357, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 24);
            this.label3.TabIndex = 13;
            this.label3.Text = "Phút";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DimGray;
            this.label4.Location = new System.Drawing.Point(5, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 24);
            this.label4.TabIndex = 13;
            this.label4.Text = "Năm:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.DimGray;
            this.label5.Location = new System.Drawing.Point(194, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 24);
            this.label5.TabIndex = 13;
            this.label5.Text = "Tháng:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.DimGray;
            this.label6.Location = new System.Drawing.Point(357, 92);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 24);
            this.label6.TabIndex = 13;
            this.label6.Text = "Ngày:";
            // 
            // CalendarShort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbday);
            this.Controls.Add(this.cbmonth);
            this.Controls.Add(this.numMinute);
            this.Controls.Add(this.numHour);
            this.Controls.Add(this.txtyear);
            this.Controls.Add(this.btnselect);
            this.Controls.Add(this.btncancel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "CalendarShort";
            this.Sizable = false;
            this.Size = new System.Drawing.Size(528, 202);
            this.Load += new System.EventHandler(this.CalendarShort_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MaterialSkin.Controls.MaterialButton btncancel;
        private MaterialSkin.Controls.MaterialButton btnselect;
        private MaterialSkin.Controls.MaterialTextBox txtyear;
        private MaterialSkin.Controls.MaterialComboBox cbmonth;
        private MaterialSkin.Controls.MaterialComboBox cbday;
        private MaterialSkin.Controls.MaterialTextBox numHour;
        private MaterialSkin.Controls.MaterialTextBox numMinute;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}
