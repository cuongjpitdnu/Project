using GPMain.Common.Dialog;
using MaterialSkin;
using System;
using System.Windows.Forms;

namespace GPMain.Views
{
    partial class Events
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtsearchEvent = new System.Windows.Forms.TextBox();
            this.lblsearch = new System.Windows.Forms.Label();
            this.dgvEvent = new GPMain.Views.Controls.DataGridTemplate(this.components);
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.STT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Calendar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeEventEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReplyType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnaddEvent = new System.Windows.Forms.Button();
            this.btnexportExcel = new System.Windows.Forms.Button();
            this.calendar1 = new GPMain.Common.Dialog.Calendar(DateTime.Now);
            this.llb_lstDeadDay = new System.Windows.Forms.LinkLabel();
            this.llb_lstBirthday = new System.Windows.Forms.LinkLabel();
            this.lblreplyType = new System.Windows.Forms.Label();
            this.cbrepType = new System.Windows.Forms.ComboBox();
            this.lblEvent = new System.Windows.Forms.Label();
            this.cbeventStatus = new System.Windows.Forms.ComboBox();
            this.lblcalendarType = new System.Windows.Forms.Label();
            this.cbcalendarType = new System.Windows.Forms.ComboBox();
            this.lbldayinfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvent)).BeginInit();
            this.SuspendLayout();
            // 
            // txtsearchEvent
            // 
            this.txtsearchEvent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtsearchEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtsearchEvent.Location = new System.Drawing.Point(598, 16);
            this.txtsearchEvent.Name = "txtsearchEvent";
            this.txtsearchEvent.Size = new System.Drawing.Size(178, 23);
            this.txtsearchEvent.TabIndex = 6;
            this.txtsearchEvent.TextChanged += new System.EventHandler(this.txtsearchEvent_TextChanged);
            // 
            // lblsearch
            // 
            this.lblsearch.AutoSize = true;
            this.lblsearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblsearch.Location = new System.Drawing.Point(475, 19);
            this.lblsearch.Name = "lblsearch";
            this.lblsearch.Size = new System.Drawing.Size(117, 17);
            this.lblsearch.TabIndex = 4;
            this.lblsearch.Text = "Tìm kiếm sự kiện:";
            // 
            // dgvEvent
            // 
            this.dgvEvent.AllowUserToAddRows = false;
            this.dgvEvent.AllowUserToDeleteRows = false;
            this.dgvEvent.AllowUserToResizeRows = false;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(238)))), ((int)(((byte)(245)))));
            this.dgvEvent.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEvent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEvent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvEvent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Image,
            this.STT,
            this.EventName,
            this.Calendar,
            this.Time,
            this.TimeEventEnd,
            this.ReplyType});
            this.dgvEvent.EnableHeadersVisualStyles = false;
            this.dgvEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.dgvEvent.Location = new System.Drawing.Point(469, 53);
            this.dgvEvent.Name = "dgvEvent";
            this.dgvEvent.ReadOnly = true;
            this.dgvEvent.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvEvent.RowHeadersVisible = false;
            this.dgvEvent.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvEvent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEvent.Size = new System.Drawing.Size(800, 653);
            this.dgvEvent.TabIndex = 14;
            // 
            // Image
            // 
            this.Image.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Image.DataPropertyName = "Image";
            this.Image.FillWeight = 16.15584F;
            this.Image.Frozen = true;
            this.Image.HeaderText = "";
            this.Image.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.Image.Name = "Image";
            this.Image.ReadOnly = true;
            this.Image.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Image.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Image.Width = 23;
            // 
            // STT
            // 
            this.STT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.STT.DataPropertyName = "STT";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.STT.DefaultCellStyle = dataGridViewCellStyle9;
            this.STT.FillWeight = 20F;
            this.STT.HeaderText = "STT";
            this.STT.Name = "STT";
            this.STT.ReadOnly = true;
            this.STT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.STT.Width = 39;
            // 
            // EventName
            // 
            this.EventName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EventName.DataPropertyName = "EventName";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.EventName.DefaultCellStyle = dataGridViewCellStyle10;
            this.EventName.FillWeight = 319.8853F;
            this.EventName.HeaderText = "Tên sự kiện";
            this.EventName.Name = "EventName";
            this.EventName.ReadOnly = true;
            // 
            // Calendar
            // 
            this.Calendar.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Calendar.DataPropertyName = "CalendarType";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.Calendar.DefaultCellStyle = dataGridViewCellStyle11;
            this.Calendar.HeaderText = "Loại lịch";
            this.Calendar.Name = "Calendar";
            this.Calendar.ReadOnly = true;
            this.Calendar.Width = 83;
            // 
            // Time
            // 
            this.Time.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Time.DataPropertyName = "TimeEventStart";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Time.DefaultCellStyle = dataGridViewCellStyle12;
            this.Time.FillWeight = 47.80301F;
            this.Time.HeaderText = "Thời gian bắt đầu";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.Width = 142;
            // 
            // TimeEventEnd
            // 
            this.TimeEventEnd.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.TimeEventEnd.DataPropertyName = "TimeEventEnd";
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TimeEventEnd.DefaultCellStyle = dataGridViewCellStyle13;
            this.TimeEventEnd.HeaderText = "Thời gian kết thúc";
            this.TimeEventEnd.Name = "TimeEventEnd";
            this.TimeEventEnd.ReadOnly = true;
            this.TimeEventEnd.Width = 144;
            // 
            // ReplyType
            // 
            this.ReplyType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ReplyType.DataPropertyName = "ReplyType";
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ReplyType.DefaultCellStyle = dataGridViewCellStyle14;
            this.ReplyType.HeaderText = "Kiểu lặp lại";
            this.ReplyType.Name = "ReplyType";
            this.ReplyType.ReadOnly = true;
            // 
            // btnaddEvent
            // 
            this.btnaddEvent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnaddEvent.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnaddEvent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnaddEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnaddEvent.Image = global::GPMain.Properties.Resources.add_event;
            this.btnaddEvent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnaddEvent.Location = new System.Drawing.Point(310, 404);
            this.btnaddEvent.Name = "btnaddEvent";
            this.btnaddEvent.Size = new System.Drawing.Size(149, 32);
            this.btnaddEvent.TabIndex = 12;
            this.btnaddEvent.Text = "Thêm sự kiện";
            this.btnaddEvent.UseVisualStyleBackColor = true;
            this.btnaddEvent.Click += new System.EventHandler(this.btnaddEvent_Click);
            // 
            // btnexportExcel
            // 
            this.btnexportExcel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnexportExcel.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnexportExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnexportExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnexportExcel.Image = global::GPMain.Properties.Resources.excel;
            this.btnexportExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnexportExcel.Location = new System.Drawing.Point(310, 442);
            this.btnexportExcel.Name = "btnexportExcel";
            this.btnexportExcel.Size = new System.Drawing.Size(149, 32);
            this.btnexportExcel.TabIndex = 12;
            this.btnexportExcel.Text = "Xuất Excel";
            this.btnexportExcel.UseVisualStyleBackColor = true;
            this.btnexportExcel.Click += new System.EventHandler(this.btnexportExcel_Click);
            // 
            // calendar1
            // 
            this.calendar1.AllowEditingEvents = true;
            this.calendar1.CalendarSolarDate = DateTime.Now;
            this.calendar1.CalendarView = GPMain.Common.Dialog.CalendarViews.Month;
            this.calendar1.DateHeaderFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.calendar1.DayOfWeekFont = new System.Drawing.Font("Arial", 10F);
            this.calendar1.DaysFont = new System.Drawing.Font("Arial", 10F);
            this.calendar1.DayViewTimeFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.calendar1.DimDisabledEvents = true;
            this.calendar1.HighlightCurrentDay = true;
            this.calendar1.LoadPresetHolidays = true;
            this.calendar1.Location = new System.Drawing.Point(-11, 0);
            this.calendar1.Name = "calendar1";
            this.calendar1.ShowArrowControls = true;
            this.calendar1.ShowDashedBorderOnDisabledEvents = true;
            this.calendar1.ShowDateInHeader = true;
            this.calendar1.ShowDisabledEvents = false;
            this.calendar1.ShowEventTooltips = true;
            this.calendar1.ShowTodayButton = true;
            this.calendar1.Size = new System.Drawing.Size(494, 444);
            this.calendar1.TabIndex = 15;
            this.calendar1.TodayFont = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.calendar1.Load += new System.EventHandler(this.calendar1_Load);
            // 
            // llb_lstDeadDay
            // 
            this.llb_lstDeadDay.AutoSize = true;
            this.llb_lstDeadDay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.llb_lstDeadDay.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.llb_lstDeadDay.Location = new System.Drawing.Point(8, 447);
            this.llb_lstDeadDay.Name = "llb_lstDeadDay";
            this.llb_lstDeadDay.Size = new System.Drawing.Size(194, 17);
            this.llb_lstDeadDay.TabIndex = 75;
            this.llb_lstDeadDay.TabStop = true;
            this.llb_lstDeadDay.Text = "Danh sách ngày giỗ gần nhất";
            this.llb_lstDeadDay.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llb_lstDeadDay_LinkClicked);
            // 
            // llb_lstBirthday
            // 
            this.llb_lstBirthday.AutoSize = true;
            this.llb_lstBirthday.Cursor = System.Windows.Forms.Cursors.Hand;
            this.llb_lstBirthday.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.llb_lstBirthday.Location = new System.Drawing.Point(8, 412);
            this.llb_lstBirthday.Name = "llb_lstBirthday";
            this.llb_lstBirthday.Size = new System.Drawing.Size(198, 17);
            this.llb_lstBirthday.TabIndex = 74;
            this.llb_lstBirthday.TabStop = true;
            this.llb_lstBirthday.Text = "Danh sách sinh nhật gần nhất";
            this.llb_lstBirthday.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llb_lstBirthday_LinkClicked);
            // 
            // lblreplyType
            // 
            this.lblreplyType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblreplyType.AutoSize = true;
            this.lblreplyType.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblreplyType.Location = new System.Drawing.Point(923, 19);
            this.lblreplyType.Name = "lblreplyType";
            this.lblreplyType.Size = new System.Drawing.Size(81, 17);
            this.lblreplyType.TabIndex = 4;
            this.lblreplyType.Text = "Kiểu lặp lại:";
            // 
            // cbrepType
            // 
            this.cbrepType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbrepType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbrepType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbrepType.FormattingEnabled = true;
            this.cbrepType.Location = new System.Drawing.Point(1010, 16);
            this.cbrepType.Name = "cbrepType";
            this.cbrepType.Size = new System.Drawing.Size(69, 23);
            this.cbrepType.TabIndex = 76;
            this.cbrepType.TextChanged += new System.EventHandler(this.txtsearchEvent_TextChanged);
            this.cbrepType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbrepType_KeyPress);
            // 
            // lblEvent
            // 
            this.lblEvent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEvent.AutoSize = true;
            this.lblEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblEvent.Location = new System.Drawing.Point(1085, 19);
            this.lblEvent.Name = "lblEvent";
            this.lblEvent.Size = new System.Drawing.Size(63, 17);
            this.lblEvent.TabIndex = 4;
            this.lblEvent.Text = "Sự kiện :";
            // 
            // cbeventStatus
            // 
            this.cbeventStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbeventStatus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbeventStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbeventStatus.FormattingEnabled = true;
            this.cbeventStatus.Location = new System.Drawing.Point(1154, 16);
            this.cbeventStatus.Name = "cbeventStatus";
            this.cbeventStatus.Size = new System.Drawing.Size(115, 23);
            this.cbeventStatus.TabIndex = 76;
            this.cbeventStatus.TextChanged += new System.EventHandler(this.txtsearchEvent_TextChanged);
            this.cbeventStatus.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbrepType_KeyPress);
            // 
            // lblcalendarType
            // 
            this.lblcalendarType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblcalendarType.AutoSize = true;
            this.lblcalendarType.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblcalendarType.Location = new System.Drawing.Point(782, 19);
            this.lblcalendarType.Name = "lblcalendarType";
            this.lblcalendarType.Size = new System.Drawing.Size(65, 17);
            this.lblcalendarType.TabIndex = 4;
            this.lblcalendarType.Text = "Kiểu lịch:";
            // 
            // cbcalendarType
            // 
            this.cbcalendarType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbcalendarType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbcalendarType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbcalendarType.FormattingEnabled = true;
            this.cbcalendarType.Location = new System.Drawing.Point(848, 16);
            this.cbcalendarType.Name = "cbcalendarType";
            this.cbcalendarType.Size = new System.Drawing.Size(69, 23);
            this.cbcalendarType.TabIndex = 76;
            this.cbcalendarType.SelectedIndexChanged += new System.EventHandler(this.txtsearchEvent_TextChanged);
            this.cbcalendarType.TextChanged += new System.EventHandler(this.txtsearchEvent_TextChanged);
            this.cbcalendarType.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbrepType_KeyPress);
            // 
            // lbldayinfo
            // 
            this.lbldayinfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbldayinfo.Location = new System.Drawing.Point(8, 511);
            this.lbldayinfo.Name = "lbldayinfo";
            this.lbldayinfo.Size = new System.Drawing.Size(451, 118);
            this.lbldayinfo.TabIndex = 4;
            this.lbldayinfo.Text = "Day Info";
            // 
            // Events
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbeventStatus);
            this.Controls.Add(this.cbcalendarType);
            this.Controls.Add(this.cbrepType);
            this.Controls.Add(this.btnexportExcel);
            this.Controls.Add(this.btnaddEvent);
            this.Controls.Add(this.lblEvent);
            this.Controls.Add(this.lblcalendarType);
            this.Controls.Add(this.lblreplyType);
            this.Controls.Add(this.lbldayinfo);
            this.Controls.Add(this.lblsearch);
            this.Controls.Add(this.dgvEvent);
            this.Controls.Add(this.llb_lstDeadDay);
            this.Controls.Add(this.llb_lstBirthday);
            this.Controls.Add(this.calendar1);
            this.Controls.Add(this.txtsearchEvent);
            this.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Events";
            this.Size = new System.Drawing.Size(1280, 720);
            this.Load += new System.EventHandler(this.ListFamilyEvent_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtsearchEvent;
        private System.Windows.Forms.Label lblsearch;
        private Views.Controls.DataGridTemplate dgvEvent;
        private System.Windows.Forms.Button btnaddEvent;
        private System.Windows.Forms.Button btnexportExcel;
        private Calendar calendar1;
        private System.Windows.Forms.LinkLabel llb_lstDeadDay;
        private System.Windows.Forms.LinkLabel llb_lstBirthday;
        private System.Windows.Forms.Label lblreplyType;
        private System.Windows.Forms.ComboBox cbrepType;
        private System.Windows.Forms.Label lblEvent;
        private System.Windows.Forms.ComboBox cbeventStatus;
        private Label lblcalendarType;
        private ComboBox cbcalendarType;
        private Label lbldayinfo;
        private DataGridViewImageColumn Image;
        private DataGridViewTextBoxColumn STT;
        private DataGridViewTextBoxColumn EventName;
        private DataGridViewTextBoxColumn Calendar;
        private DataGridViewTextBoxColumn Time;
        private DataGridViewTextBoxColumn TimeEventEnd;
        private DataGridViewTextBoxColumn ReplyType;
    }
}
