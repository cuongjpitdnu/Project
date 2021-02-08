namespace DSF602.View
{
    partial class ChargingConfirmPopup
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtStopIonBlance = new System.Windows.Forms.TextBox();
            this.txtIonBalanceCheck = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtStopTime = new System.Windows.Forms.TextBox();
            this.txtDecayTime = new System.Windows.Forms.TextBox();
            this.txtLowVal = new System.Windows.Forms.TextBox();
            this.txtUpVal = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.AutoSize = true;
            this.btnCancel.Location = new System.Drawing.Point(103, 275);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnApply
            // 
            this.btnApply.AutoSize = true;
            this.btnApply.Location = new System.Drawing.Point(198, 275);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(93, 30);
            this.btnApply.TabIndex = 2;
            this.btnApply.Text = "OK";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(345, 199);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(26, 20);
            this.label25.TabIndex = 61;
            this.label25.Text = "(V)";
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(345, 231);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(24, 20);
            this.label24.TabIndex = 60;
            this.label24.Text = "(s)";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(347, 135);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(24, 20);
            this.label23.TabIndex = 59;
            this.label23.Text = "(s)";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(347, 106);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(24, 20);
            this.label22.TabIndex = 58;
            this.label22.Text = "(s)";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(347, 73);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(26, 20);
            this.label21.TabIndex = 57;
            this.label21.Text = "(V)";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(347, 42);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(26, 20);
            this.label20.TabIndex = 56;
            this.label20.Text = "(V)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(156, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(25, 20);
            this.label11.TabIndex = 55;
            this.label11.Text = "+/-";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(156, 41);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 20);
            this.label10.TabIndex = 54;
            this.label10.Text = "+/-";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(152, 199);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(25, 20);
            this.label9.TabIndex = 53;
            this.label9.Text = "+/-";
            // 
            // txtStopIonBlance
            // 
            this.txtStopIonBlance.Location = new System.Drawing.Point(183, 228);
            this.txtStopIonBlance.Name = "txtStopIonBlance";
            this.txtStopIonBlance.ReadOnly = true;
            this.txtStopIonBlance.Size = new System.Drawing.Size(148, 26);
            this.txtStopIonBlance.TabIndex = 52;
            this.txtStopIonBlance.Text = "0";
            this.txtStopIonBlance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtIonBalance
            // 
            this.txtIonBalanceCheck.Location = new System.Drawing.Point(183, 196);
            this.txtIonBalanceCheck.Name = "txtIonBalance";
            this.txtIonBalanceCheck.ReadOnly = true;
            this.txtIonBalanceCheck.Size = new System.Drawing.Size(148, 26);
            this.txtIonBalanceCheck.TabIndex = 51;
            this.txtIonBalanceCheck.Text = "0";
            this.txtIonBalanceCheck.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 228);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 20);
            this.label7.TabIndex = 50;
            this.label7.Text = "Stop IB Measure";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 199);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 20);
            this.label8.TabIndex = 49;
            this.label8.Text = "Alarm Peak Voltage";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(18, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 20);
            this.label6.TabIndex = 48;
            this.label6.Text = "Ion Balance";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 20);
            this.label5.TabIndex = 47;
            this.label5.Text = "Decay Time";
            // 
            // txtStopTime
            // 
            this.txtStopTime.Location = new System.Drawing.Point(183, 131);
            this.txtStopTime.Name = "txtStopTime";
            this.txtStopTime.ReadOnly = true;
            this.txtStopTime.Size = new System.Drawing.Size(148, 26);
            this.txtStopTime.TabIndex = 46;
            this.txtStopTime.Text = "0";
            this.txtStopTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtDecayTime
            // 
            this.txtDecayTime.Location = new System.Drawing.Point(183, 99);
            this.txtDecayTime.Name = "txtDecayTime";
            this.txtDecayTime.ReadOnly = true;
            this.txtDecayTime.Size = new System.Drawing.Size(148, 26);
            this.txtDecayTime.TabIndex = 45;
            this.txtDecayTime.Text = "0";
            this.txtDecayTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtLowVal
            // 
            this.txtLowVal.Location = new System.Drawing.Point(183, 69);
            this.txtLowVal.Name = "txtLowVal";
            this.txtLowVal.ReadOnly = true;
            this.txtLowVal.Size = new System.Drawing.Size(148, 26);
            this.txtLowVal.TabIndex = 44;
            this.txtLowVal.Text = "0";
            this.txtLowVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtUpVal
            // 
            this.txtUpVal.Location = new System.Drawing.Point(183, 38);
            this.txtUpVal.Name = "txtUpVal";
            this.txtUpVal.ReadOnly = true;
            this.txtUpVal.Size = new System.Drawing.Size(148, 26);
            this.txtUpVal.TabIndex = 43;
            this.txtUpVal.Text = "0";
            this.txtUpVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 134);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(130, 20);
            this.label4.TabIndex = 42;
            this.label4.Text = "Stop Decay (10-100)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 20);
            this.label3.TabIndex = 41;
            this.label3.Text = "Decay Alarm (0-10)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 20);
            this.label2.TabIndex = 40;
            this.label2.Text = "Charging (10-2000)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 20);
            this.label1.TabIndex = 39;
            this.label1.Text = "Decay to (0-1995)";
            // 
            // ChargingConfirmPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 317);
            this.ControlBox = false;
            this.Controls.Add(this.label25);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtStopIonBlance);
            this.Controls.Add(this.txtIonBalanceCheck);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtStopTime);
            this.Controls.Add(this.txtDecayTime);
            this.Controls.Add(this.txtLowVal);
            this.Controls.Add(this.txtUpVal);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial Narrow", 12F);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.MinimumSize = new System.Drawing.Size(351, 124);
            this.Name = "ChargingConfirmPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Decay Time & Ion Balance";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtStopIonBlance;
        private System.Windows.Forms.TextBox txtIonBalanceCheck;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtStopTime;
        private System.Windows.Forms.TextBox txtDecayTime;
        private System.Windows.Forms.TextBox txtLowVal;
        private System.Windows.Forms.TextBox txtUpVal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}