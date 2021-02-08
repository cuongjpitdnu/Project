namespace DSF602.View.GraphLayout
{
    partial class DisplaySensor
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
            this.lblSensorName = new System.Windows.Forms.Label();
            this.lblValueSensor = new System.Windows.Forms.Label();
            this.lblPeak = new System.Windows.Forms.Label();
            this.btnCharge = new System.Windows.Forms.Button();
            this.btnData = new System.Windows.Forms.Button();
            this.lbDecayPositive = new System.Windows.Forms.Label();
            this.lbDecayNegative = new System.Windows.Forms.Label();
            this.lbIonCheck = new System.Windows.Forms.Label();
            this.pnDecay = new System.Windows.Forms.Panel();
            this.pnFunction = new System.Windows.Forms.Panel();
            this.pnDecay.SuspendLayout();
            this.pnFunction.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSensorName
            // 
            this.lblSensorName.BackColor = System.Drawing.Color.Transparent;
            this.lblSensorName.Cursor = System.Windows.Forms.Cursors.NoMove2D;
            this.lblSensorName.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.lblSensorName.Location = new System.Drawing.Point(6, 8);
            this.lblSensorName.Margin = new System.Windows.Forms.Padding(0);
            this.lblSensorName.Name = "lblSensorName";
            this.lblSensorName.Size = new System.Drawing.Size(86, 20);
            this.lblSensorName.TabIndex = 0;
            this.lblSensorName.Text = "SensorName";
            this.lblSensorName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSensorName.Paint += new System.Windows.Forms.PaintEventHandler(this.lblSensorName_Paint);
            // 
            // lblValueSensor
            // 
            this.lblValueSensor.AutoSize = true;
            this.lblValueSensor.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblValueSensor.Location = new System.Drawing.Point(185, 8);
            this.lblValueSensor.Name = "lblValueSensor";
            this.lblValueSensor.Size = new System.Drawing.Size(43, 16);
            this.lblValueSensor.TabIndex = 1;
            this.lblValueSensor.Text = "VALUE";
            // 
            // lblPeak
            // 
            this.lblPeak.AutoSize = true;
            this.lblPeak.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPeak.Location = new System.Drawing.Point(109, 8);
            this.lblPeak.Name = "lblPeak";
            this.lblPeak.Size = new System.Drawing.Size(36, 16);
            this.lblPeak.TabIndex = 2;
            this.lblPeak.Text = "PEAK";
            // 
            // btnCharge
            // 
            this.btnCharge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCharge.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.btnCharge.Location = new System.Drawing.Point(2, 3);
            this.btnCharge.Name = "btnCharge";
            this.btnCharge.Size = new System.Drawing.Size(79, 27);
            this.btnCharge.TabIndex = 3;
            this.btnCharge.Text = "Charge";
            this.btnCharge.UseVisualStyleBackColor = true;
            this.btnCharge.Click += new System.EventHandler(this.btnCharge_Click);
            // 
            // btnData
            // 
            this.btnData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnData.Font = new System.Drawing.Font("Arial Narrow", 10F);
            this.btnData.Location = new System.Drawing.Point(82, 3);
            this.btnData.Name = "btnData";
            this.btnData.Size = new System.Drawing.Size(76, 27);
            this.btnData.TabIndex = 4;
            this.btnData.Text = "Data";
            this.btnData.UseVisualStyleBackColor = true;
            this.btnData.Click += new System.EventHandler(this.btnData_Click);
            // 
            // lbDecayPositive
            // 
            this.lbDecayPositive.AutoSize = true;
            this.lbDecayPositive.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDecayPositive.Location = new System.Drawing.Point(4, 6);
            this.lbDecayPositive.Name = "lbDecayPositive";
            this.lbDecayPositive.Size = new System.Drawing.Size(90, 16);
            this.lbDecayPositive.TabIndex = 7;
            this.lbDecayPositive.Tag = "DECAY(+): {0} sec";
            this.lbDecayPositive.Text = "DECAY(+): 0 sec";
            // 
            // lbDecayNegative
            // 
            this.lbDecayNegative.AutoSize = true;
            this.lbDecayNegative.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDecayNegative.Location = new System.Drawing.Point(137, 6);
            this.lbDecayNegative.Name = "lbDecayNegative";
            this.lbDecayNegative.Size = new System.Drawing.Size(88, 16);
            this.lbDecayNegative.TabIndex = 9;
            this.lbDecayNegative.Tag = "DECAY(-): {0} sec";
            this.lbDecayNegative.Text = "DECAY(-): 0 sec";
            // 
            // lbIonCheck
            // 
            this.lbIonCheck.AutoSize = true;
            this.lbIonCheck.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbIonCheck.Location = new System.Drawing.Point(272, 6);
            this.lbIonCheck.Name = "lbIonCheck";
            this.lbIonCheck.Size = new System.Drawing.Size(40, 16);
            this.lbIonCheck.TabIndex = 10;
            this.lbIonCheck.Tag = "IB: {0} V";
            this.lbIonCheck.Text = "IB: 0 V";
            // 
            // pnDecay
            // 
            this.pnDecay.BackColor = System.Drawing.Color.Transparent;
            this.pnDecay.Controls.Add(this.lbDecayPositive);
            this.pnDecay.Controls.Add(this.lbIonCheck);
            this.pnDecay.Controls.Add(this.lbDecayNegative);
            this.pnDecay.Location = new System.Drawing.Point(289, 2);
            this.pnDecay.Name = "pnDecay";
            this.pnDecay.Size = new System.Drawing.Size(333, 29);
            this.pnDecay.TabIndex = 11;
            this.pnDecay.Visible = false;
            // 
            // pnFunction
            // 
            this.pnFunction.Controls.Add(this.btnData);
            this.pnFunction.Controls.Add(this.btnCharge);
            this.pnFunction.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnFunction.Location = new System.Drawing.Point(693, 0);
            this.pnFunction.Name = "pnFunction";
            this.pnFunction.Size = new System.Drawing.Size(161, 33);
            this.pnFunction.TabIndex = 11;
            this.pnFunction.Visible = false;
            // 
            // DisplaySensor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblValueSensor);
            this.Controls.Add(this.pnFunction);
            this.Controls.Add(this.lblPeak);
            this.Controls.Add(this.lblSensorName);
            this.Controls.Add(this.pnDecay);
            this.Font = new System.Drawing.Font("Arial Narrow", 12F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "DisplaySensor";
            this.Size = new System.Drawing.Size(854, 33);
            this.pnDecay.ResumeLayout(false);
            this.pnDecay.PerformLayout();
            this.pnFunction.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSensorName;
        private System.Windows.Forms.Label lblValueSensor;
        private System.Windows.Forms.Label lblPeak;
        private System.Windows.Forms.Button btnCharge;
        private System.Windows.Forms.Button btnData;
        private System.Windows.Forms.Label lbDecayPositive;
        private System.Windows.Forms.Label lbDecayNegative;
        private System.Windows.Forms.Label lbIonCheck;
        private System.Windows.Forms.Panel pnDecay;
        private System.Windows.Forms.Panel pnFunction;
    }
}
