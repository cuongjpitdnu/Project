namespace GP40Tree
{
    partial class frmFMemberMgnt
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
            this.panelBottom = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnSvg = new System.Windows.Forms.Button();
            this.btnPDF2 = new System.Windows.Forms.Button();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnJSON = new System.Windows.Forms.Button();
            this.lblZoomLevel = new System.Windows.Forms.Label();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelBottom.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.label3);
            this.panelBottom.Controls.Add(this.label2);
            this.panelBottom.Controls.Add(this.label1);
            this.panelBottom.Controls.Add(this.lblEnd);
            this.panelBottom.Controls.Add(this.lblStart);
            this.panelBottom.Controls.Add(this.lblCount);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(3, 403);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(794, 44);
            this.panelBottom.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(360, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Nữ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Nam";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Tổng số thành viên";
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(709, 16);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(35, 13);
            this.lblEnd.TabIndex = 10;
            this.lblEnd.Text = "label1";
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(546, 16);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(35, 13);
            this.lblStart.TabIndex = 9;
            this.lblStart.Text = "label1";
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Location = new System.Drawing.Point(137, 16);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(35, 13);
            this.lblCount.TabIndex = 8;
            this.lblCount.Text = "label1";
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomOut.Location = new System.Drawing.Point(512, 2);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(43, 36);
            this.btnZoomOut.TabIndex = 40;
            this.btnZoomOut.Text = "+";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            // 
            // btnSvg
            // 
            this.btnSvg.Location = new System.Drawing.Point(613, 2);
            this.btnSvg.Name = "btnSvg";
            this.btnSvg.Size = new System.Drawing.Size(46, 36);
            this.btnSvg.TabIndex = 36;
            this.btnSvg.Text = "SVG";
            this.btnSvg.UseVisualStyleBackColor = true;
            // 
            // btnPDF2
            // 
            this.btnPDF2.Location = new System.Drawing.Point(561, 2);
            this.btnPDF2.Name = "btnPDF2";
            this.btnPDF2.Size = new System.Drawing.Size(46, 36);
            this.btnPDF2.TabIndex = 35;
            this.btnPDF2.Text = "PDF";
            this.btnPDF2.UseVisualStyleBackColor = true;
            // 
            // panelMiddle
            // 
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(3, 53);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(794, 344);
            this.panelMiddle.TabIndex = 1;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.btnJSON);
            this.panelTop.Controls.Add(this.lblZoomLevel);
            this.panelTop.Controls.Add(this.btnZoomOut);
            this.panelTop.Controls.Add(this.btnZoomIn);
            this.panelTop.Controls.Add(this.btnSvg);
            this.panelTop.Controls.Add(this.btnPDF2);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(3, 3);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(794, 44);
            this.panelTop.TabIndex = 0;
            // 
            // btnJSON
            // 
            this.btnJSON.Location = new System.Drawing.Point(668, 2);
            this.btnJSON.Name = "btnJSON";
            this.btnJSON.Size = new System.Drawing.Size(46, 36);
            this.btnJSON.TabIndex = 42;
            this.btnJSON.Text = "JSON";
            this.btnJSON.UseVisualStyleBackColor = true;
            // 
            // lblZoomLevel
            // 
            this.lblZoomLevel.AutoSize = true;
            this.lblZoomLevel.Location = new System.Drawing.Point(435, 15);
            this.lblZoomLevel.Name = "lblZoomLevel";
            this.lblZoomLevel.Size = new System.Drawing.Size(70, 13);
            this.lblZoomLevel.TabIndex = 37;
            this.lblZoomLevel.Text = "lblZoomLevel";
            this.lblZoomLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomIn.Location = new System.Drawing.Point(386, 4);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(43, 36);
            this.btnZoomIn.TabIndex = 39;
            this.btnZoomIn.Text = "-";
            this.btnZoomIn.UseVisualStyleBackColor = true; 
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelBottom, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panelMiddle, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelTop, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // frmFMemberMgnt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmFMemberMgnt";
            this.Text = "frmFMemberMgnt";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmFMemberMgnt_FormClosed);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblCount;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnSvg;
        private System.Windows.Forms.Button btnPDF2;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnJSON;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblZoomLevel;
        private System.Windows.Forms.Button btnZoomIn;
    }
}