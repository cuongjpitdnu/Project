namespace GP40Common
{
    partial class frmCalendarVNEvent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCalendarVNEvent));
            this.calendarMain = new GP40Common.Calendar();
            this.btnExit = new GP40Common.CoolButton();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvEvent = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvent)).BeginInit();
            this.SuspendLayout();
            // 
            // calendarMain
            // 
            this.calendarMain.AllowEditingEvents = true;
            this.calendarMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.calendarMain.CalendarSolarDate = new System.DateTime(2020, 5, 11, 0, 0, 0, 0);
            this.calendarMain.CalendarView = GP40Common.CalendarViews.Month;
            this.calendarMain.DateHeaderFont = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.calendarMain.DayOfWeekFont = new System.Drawing.Font("Arial", 10F);
            this.calendarMain.DaysFont = new System.Drawing.Font("Arial", 10F);
            this.calendarMain.DayViewTimeFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.calendarMain.DimDisabledEvents = true;
            this.calendarMain.HighlightCurrentDay = true;
            this.calendarMain.LoadPresetHolidays = true;
            this.calendarMain.Location = new System.Drawing.Point(12, 51);
            this.calendarMain.Name = "calendarMain";
            this.calendarMain.ShowArrowControls = true;
            this.calendarMain.ShowDashedBorderOnDisabledEvents = true;
            this.calendarMain.ShowDateInHeader = true;
            this.calendarMain.ShowDisabledEvents = false;
            this.calendarMain.ShowEventTooltips = true;
            this.calendarMain.ShowTodayButton = true;
            this.calendarMain.Size = new System.Drawing.Size(538, 410);
            this.calendarMain.TabIndex = 0;
            this.calendarMain.TodayFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnExit.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this.btnExit.ButtonFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ButtonText = "Thoát";
            this.btnExit.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(144)))), ((int)(((byte)(254)))));
            this.btnExit.HighlightBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(198)))), ((int)(((byte)(198)))));
            this.btnExit.HighlightButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.btnExit.Location = new System.Drawing.Point(12, 467);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(84, 37);
            this.btnExit.TabIndex = 2;
            this.btnExit.TextColor = System.Drawing.Color.Black;
            this.btnExit.Load += new System.EventHandler(this.btnExit_Load);
            this.btnExit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnExit_MouseClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(348, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(268, 32);
            this.label1.TabIndex = 3;
            this.label1.Text = "SỰ KIỆN DÒNG HỌ";
            // 
            // dgvEvent
            // 
            this.dgvEvent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEvent.Location = new System.Drawing.Point(579, 51);
            this.dgvEvent.Name = "dgvEvent";
            this.dgvEvent.Size = new System.Drawing.Size(436, 410);
            this.dgvEvent.TabIndex = 4;
            // 
            // frmCalendarVNEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1027, 505);
            this.Controls.Add(this.dgvEvent);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.calendarMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCalendarVNEvent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Màn hình chọn lịch";
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Calendar calendarMain;
        private CoolButton btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvEvent;
    }
}