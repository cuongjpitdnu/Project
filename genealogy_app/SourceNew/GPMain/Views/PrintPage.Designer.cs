namespace GPMain.Views
{
    partial class PrintPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintPage));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.cbdpi = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbprint = new System.Windows.Forms.ComboBox();
            this.cbsizePage = new System.Windows.Forms.ComboBox();
            this.panelDocument = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.cbdpi);
            this.panelHeader.Controls.Add(this.label2);
            this.panelHeader.Controls.Add(this.panel1);
            this.panelHeader.Controls.Add(this.btnPrint);
            this.panelHeader.Controls.Add(this.label3);
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Controls.Add(this.cbprint);
            this.panelHeader.Controls.Add(this.cbsizePage);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(4);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(787, 73);
            this.panelHeader.TabIndex = 0;
            this.panelHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.panelHeader_Paint);
            // 
            // cbdpi
            // 
            this.cbdpi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbdpi.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbdpi.FormattingEnabled = true;
            this.cbdpi.Items.AddRange(new object[] {
            "240",
            "300",
            "400",
            "600",
            "1200"});
            this.cbdpi.Location = new System.Drawing.Point(285, 23);
            this.cbdpi.Margin = new System.Windows.Forms.Padding(4);
            this.cbdpi.Name = "cbdpi";
            this.cbdpi.Size = new System.Drawing.Size(87, 25);
            this.cbdpi.TabIndex = 0;
            this.cbdpi.SelectedIndexChanged += new System.EventHandler(this.cbsizePage_SelectedIndexChanged);
            this.cbdpi.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbzoom_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(185, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Độ phân giải:";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 72);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 1);
            this.panel1.TabIndex = 4;
            // 
            // btnPrint
            // 
            this.btnPrint.BackgroundImage = global::GPMain.Properties.Resources.printer;
            this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnPrint.FlatAppearance.BorderSize = 0;
            this.btnPrint.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Location = new System.Drawing.Point(749, 17);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(26, 34);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            this.btnPrint.MouseLeave += new System.EventHandler(this.btnPrint_MouseLeave);
            this.btnPrint.MouseHover += new System.EventHandler(this.btnPrint_MouseHover);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(391, 27);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Máy in:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Cỡ giấy:";
            // 
            // cbprint
            // 
            this.cbprint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbprint.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbprint.FormattingEnabled = true;
            this.cbprint.Location = new System.Drawing.Point(452, 23);
            this.cbprint.Margin = new System.Windows.Forms.Padding(4);
            this.cbprint.Name = "cbprint";
            this.cbprint.Size = new System.Drawing.Size(279, 25);
            this.cbprint.TabIndex = 0;
            this.cbprint.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbzoom_KeyPress);
            // 
            // cbsizePage
            // 
            this.cbsizePage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbsizePage.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbsizePage.FormattingEnabled = true;
            this.cbsizePage.Items.AddRange(new object[] {
            "A0",
            "A1",
            "A2",
            "A3",
            "A4",
            "A5"});
            this.cbsizePage.Location = new System.Drawing.Point(89, 23);
            this.cbsizePage.Margin = new System.Windows.Forms.Padding(4);
            this.cbsizePage.Name = "cbsizePage";
            this.cbsizePage.Size = new System.Drawing.Size(88, 25);
            this.cbsizePage.TabIndex = 0;
            this.cbsizePage.SelectedIndexChanged += new System.EventHandler(this.cbsizePage_SelectedIndexChanged);
            this.cbsizePage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbzoom_KeyPress);
            // 
            // panelDocument
            // 
            this.panelDocument.AutoScroll = true;
            this.panelDocument.BackColor = System.Drawing.Color.White;
            this.panelDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDocument.Location = new System.Drawing.Point(0, 73);
            this.panelDocument.Margin = new System.Windows.Forms.Padding(4);
            this.panelDocument.Name = "panelDocument";
            this.panelDocument.Size = new System.Drawing.Size(787, 458);
            this.panelDocument.TabIndex = 1;
            this.panelDocument.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelDocument_MouseDown);
            // 
            // PrintPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(787, 531);
            this.Controls.Add(this.panelDocument);
            this.Controls.Add(this.panelHeader);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimizeBox = false;
            this.Name = "PrintPage";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PrintPage";
            this.Load += new System.EventHandler(this.PrintPage_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbprint;
        private System.Windows.Forms.ComboBox cbsizePage;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Panel panelDocument;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbdpi;
        private System.Windows.Forms.Label label2;
    }
}