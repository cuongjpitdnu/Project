using MaterialSkin;

namespace GPMain.Common.FamilyEvent
{
    partial class AddFamilyEvent
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
            this.ckactiveEvent = new System.Windows.Forms.CheckBox();
            this.txtEventName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.rdcalendarM = new System.Windows.Forms.RadioButton();
            this.rdcalendarS = new System.Windows.Forms.RadioButton();
            this.cbrepType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtEventPlace = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btncancel = new System.Windows.Forms.Button();
            this.btnsave = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.btncalendarstartDate = new System.Windows.Forms.Button();
            this.btncalendarendDate = new System.Windows.Forms.Button();
            this.ckimportant = new System.Windows.Forms.CheckBox();
            this.cbForeColor = new MaterialSkin.Controls.MaterialDropDownColorPicker();
            this.cbBackColor = new MaterialSkin.Controls.MaterialDropDownColorPicker();
            this.lbltimestart = new System.Windows.Forms.Label();
            this.lbltimeend = new System.Windows.Forms.Label();
            this.lblcalendarSunStart = new System.Windows.Forms.Label();
            this.lblcalendarSunEnd = new System.Windows.Forms.Label();
            this.lblcalendarMoonStart = new System.Windows.Forms.Label();
            this.lblcalendarMoonEnd = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbldayStartM = new System.Windows.Forms.Label();
            this.lblmonthStartM = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblyearStartM = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblyearEndM = new System.Windows.Forms.Label();
            this.lbldayEndM = new System.Windows.Forms.Label();
            this.lblmonthEndM = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ckactiveEvent
            // 
            this.ckactiveEvent.AutoSize = true;
            this.ckactiveEvent.Checked = true;
            this.ckactiveEvent.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckactiveEvent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ckactiveEvent.Location = new System.Drawing.Point(20, 489);
            this.ckactiveEvent.Margin = new System.Windows.Forms.Padding(6);
            this.ckactiveEvent.Name = "ckactiveEvent";
            this.ckactiveEvent.Size = new System.Drawing.Size(135, 21);
            this.ckactiveEvent.TabIndex = 0;
            this.ckactiveEvent.Text = "Kích hoạt sự kiện";
            this.ckactiveEvent.UseVisualStyleBackColor = true;
            this.ckactiveEvent.Visible = false;
            // 
            // txtEventName
            // 
            this.txtEventName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtEventName.Location = new System.Drawing.Point(187, 21);
            this.txtEventName.Margin = new System.Windows.Forms.Padding(6);
            this.txtEventName.Name = "txtEventName";
            this.txtEventName.Size = new System.Drawing.Size(594, 23);
            this.txtEventName.TabIndex = 2;
            this.txtEventName.TextChanged += new System.EventHandler(this.txtEventName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(17, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tên sự kiện(*):";
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtDescription.Location = new System.Drawing.Point(187, 62);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(6);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(594, 50);
            this.txtDescription.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.Location = new System.Drawing.Point(17, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mô tả:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.Location = new System.Drawing.Point(17, 165);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Ngày bắt đầu(*):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.Location = new System.Drawing.Point(17, 225);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Ngày kết thúc(*):";
            // 
            // rdcalendarM
            // 
            this.rdcalendarM.AutoSize = true;
            this.rdcalendarM.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.rdcalendarM.Location = new System.Drawing.Point(372, 127);
            this.rdcalendarM.Margin = new System.Windows.Forms.Padding(6);
            this.rdcalendarM.Name = "rdcalendarM";
            this.rdcalendarM.Size = new System.Drawing.Size(75, 21);
            this.rdcalendarM.TabIndex = 8;
            this.rdcalendarM.Text = "Lịch âm";
            this.rdcalendarM.UseVisualStyleBackColor = true;
            this.rdcalendarM.CheckedChanged += new System.EventHandler(this.rdcalendarS_CheckedChanged);
            // 
            // rdcalendarS
            // 
            this.rdcalendarS.AutoSize = true;
            this.rdcalendarS.Checked = true;
            this.rdcalendarS.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.rdcalendarS.Location = new System.Drawing.Point(257, 127);
            this.rdcalendarS.Margin = new System.Windows.Forms.Padding(6);
            this.rdcalendarS.Name = "rdcalendarS";
            this.rdcalendarS.Size = new System.Drawing.Size(96, 21);
            this.rdcalendarS.TabIndex = 8;
            this.rdcalendarS.TabStop = true;
            this.rdcalendarS.Text = "Lịch dương";
            this.rdcalendarS.UseVisualStyleBackColor = true;
            this.rdcalendarS.CheckedChanged += new System.EventHandler(this.rdcalendarS_CheckedChanged);
            // 
            // cbrepType
            // 
            this.cbrepType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbrepType.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cbrepType.FormattingEnabled = true;
            this.cbrepType.Location = new System.Drawing.Point(187, 284);
            this.cbrepType.Margin = new System.Windows.Forms.Padding(6);
            this.cbrepType.Name = "cbrepType";
            this.cbrepType.Size = new System.Drawing.Size(149, 25);
            this.cbrepType.TabIndex = 6;
            this.cbrepType.SelectedIndexChanged += new System.EventHandler(this.cbtyperecurring_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.Location = new System.Drawing.Point(17, 288);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "Kiểu lặp lại(*):";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label10.Location = new System.Drawing.Point(17, 331);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(132, 17);
            this.label10.TabIndex = 3;
            this.label10.Text = "Nơi tổ chức sự kiện:";
            // 
            // txtEventPlace
            // 
            this.txtEventPlace.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtEventPlace.Location = new System.Drawing.Point(187, 327);
            this.txtEventPlace.Margin = new System.Windows.Forms.Padding(6);
            this.txtEventPlace.Name = "txtEventPlace";
            this.txtEventPlace.Size = new System.Drawing.Size(594, 23);
            this.txtEventPlace.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label11.Location = new System.Drawing.Point(17, 369);
            this.label11.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(252, 17);
            this.label11.TabIndex = 3;
            this.label11.Text = "Cấp độ sự kiện(mặc định bình thường):";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label12.Location = new System.Drawing.Point(416, 370);
            this.label12.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 17);
            this.label12.TabIndex = 3;
            this.label12.Text = "Màu nền:";
            this.label12.Visible = false;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label13.Location = new System.Drawing.Point(596, 370);
            this.label13.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(66, 17);
            this.label13.TabIndex = 3;
            this.label13.Text = "Màu chữ:";
            this.label13.Visible = false;
            // 
            // txtNote
            // 
            this.txtNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtNote.Location = new System.Drawing.Point(187, 406);
            this.txtNote.Margin = new System.Windows.Forms.Padding(6);
            this.txtNote.Multiline = true;
            this.txtNote.Name = "txtNote";
            this.txtNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtNote.Size = new System.Drawing.Size(594, 50);
            this.txtNote.TabIndex = 2;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label14.Location = new System.Drawing.Point(17, 409);
            this.label14.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(61, 17);
            this.label14.TabIndex = 3;
            this.label14.Text = "Ghi chú:";
            // 
            // btncancel
            // 
            this.btncancel.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btncancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btncancel.Image = global::GPMain.Properties.Resources.logout;
            this.btncancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btncancel.Location = new System.Drawing.Point(662, 475);
            this.btncancel.Margin = new System.Windows.Forms.Padding(4);
            this.btncancel.Name = "btncancel";
            this.btncancel.Size = new System.Drawing.Size(119, 35);
            this.btncancel.TabIndex = 9;
            this.btncancel.Text = "Hủy";
            this.btncancel.UseVisualStyleBackColor = true;
            this.btncancel.Click += new System.EventHandler(this.btncancel_Click);
            // 
            // btnsave
            // 
            this.btnsave.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnsave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnsave.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.btnsave.Image = global::GPMain.Properties.Resources.diskette1;
            this.btnsave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnsave.Location = new System.Drawing.Point(522, 475);
            this.btnsave.Margin = new System.Windows.Forms.Padding(4);
            this.btnsave.Name = "btnsave";
            this.btnsave.Size = new System.Drawing.Size(119, 35);
            this.btnsave.TabIndex = 9;
            this.btnsave.Text = "Lưu";
            this.btnsave.UseVisualStyleBackColor = true;
            this.btnsave.Click += new System.EventHandler(this.btnsave_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label15.Location = new System.Drawing.Point(17, 129);
            this.label15.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(196, 17);
            this.label15.TabIndex = 3;
            this.label15.Text = "Thời gian diễn ra sự kiện theo";
            // 
            // btncalendarstartDate
            // 
            this.btncalendarstartDate.BackgroundImage = global::GPMain.Properties.Resources.calendar_on_day_17;
            this.btncalendarstartDate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btncalendarstartDate.FlatAppearance.BorderSize = 0;
            this.btncalendarstartDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncalendarstartDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btncalendarstartDate.Location = new System.Drawing.Point(503, 156);
            this.btncalendarstartDate.Margin = new System.Windows.Forms.Padding(6);
            this.btncalendarstartDate.Name = "btncalendarstartDate";
            this.btncalendarstartDate.Size = new System.Drawing.Size(36, 35);
            this.btncalendarstartDate.TabIndex = 9;
            this.btncalendarstartDate.UseVisualStyleBackColor = true;
            this.btncalendarstartDate.Click += new System.EventHandler(this.btncalendarstartDate_Click);
            // 
            // btncalendarendDate
            // 
            this.btncalendarendDate.BackgroundImage = global::GPMain.Properties.Resources.calendar_on_day_17;
            this.btncalendarendDate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btncalendarendDate.FlatAppearance.BorderSize = 0;
            this.btncalendarendDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btncalendarendDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btncalendarendDate.Location = new System.Drawing.Point(503, 216);
            this.btncalendarendDate.Margin = new System.Windows.Forms.Padding(6);
            this.btncalendarendDate.Name = "btncalendarendDate";
            this.btncalendarendDate.Size = new System.Drawing.Size(36, 35);
            this.btncalendarendDate.TabIndex = 9;
            this.btncalendarendDate.UseVisualStyleBackColor = true;
            this.btncalendarendDate.Click += new System.EventHandler(this.button1_Click);
            // 
            // ckimportant
            // 
            this.ckimportant.AutoSize = true;
            this.ckimportant.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ckimportant.Location = new System.Drawing.Point(298, 366);
            this.ckimportant.Margin = new System.Windows.Forms.Padding(4);
            this.ckimportant.Name = "ckimportant";
            this.ckimportant.Size = new System.Drawing.Size(99, 21);
            this.ckimportant.TabIndex = 11;
            this.ckimportant.Text = "Quan trọng";
            this.ckimportant.UseVisualStyleBackColor = true;
            // 
            // cbForeColor
            // 
            this.cbForeColor.AnchorSize = new System.Drawing.Size(80, 25);
            this.cbForeColor.BackColor = System.Drawing.SystemColors.Control;
            this.cbForeColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(64)))), ((int)(((byte)(129)))));
            this.cbForeColor.Depth = 0;
            this.cbForeColor.DockSide = MaterialSkin.Controls.DropDownControl.eDockSide.Left;
            this.cbForeColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cbForeColor.Location = new System.Drawing.Point(677, 366);
            this.cbForeColor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbForeColor.MouseState = MaterialSkin.MouseState.HOVER;
            this.cbForeColor.Name = "cbForeColor";
            this.cbForeColor.Size = new System.Drawing.Size(80, 25);
            this.cbForeColor.TabIndex = 10;
            this.cbForeColor.Visible = false;
            // 
            // cbBackColor
            // 
            this.cbBackColor.AnchorSize = new System.Drawing.Size(80, 25);
            this.cbBackColor.BackColor = System.Drawing.SystemColors.Control;
            this.cbBackColor.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(64)))), ((int)(((byte)(129)))));
            this.cbBackColor.Depth = 0;
            this.cbBackColor.DockSide = MaterialSkin.Controls.DropDownControl.eDockSide.Left;
            this.cbBackColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBackColor.Location = new System.Drawing.Point(493, 366);
            this.cbBackColor.Margin = new System.Windows.Forms.Padding(4);
            this.cbBackColor.MouseState = MaterialSkin.MouseState.HOVER;
            this.cbBackColor.Name = "cbBackColor";
            this.cbBackColor.Size = new System.Drawing.Size(80, 25);
            this.cbBackColor.TabIndex = 7;
            this.cbBackColor.Visible = false;
            this.cbBackColor.Load += new System.EventHandler(this.materialDropDownColorPicker1_Load);
            // 
            // lbltimestart
            // 
            this.lbltimestart.AutoSize = true;
            this.lbltimestart.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbltimestart.Location = new System.Drawing.Point(143, 165);
            this.lbltimestart.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbltimestart.Name = "lbltimestart";
            this.lbltimestart.Size = new System.Drawing.Size(81, 17);
            this.lbltimestart.TabIndex = 12;
            this.lbltimestart.Text = "08:00 Sáng";
            // 
            // lbltimeend
            // 
            this.lbltimeend.AutoSize = true;
            this.lbltimeend.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbltimeend.Location = new System.Drawing.Point(143, 225);
            this.lbltimeend.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbltimeend.Name = "lbltimeend";
            this.lbltimeend.Size = new System.Drawing.Size(81, 17);
            this.lbltimeend.TabIndex = 12;
            this.lbltimeend.Text = "08:00 Sáng";
            // 
            // lblcalendarSunStart
            // 
            this.lblcalendarSunStart.AutoSize = true;
            this.lblcalendarSunStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblcalendarSunStart.Location = new System.Drawing.Point(260, 165);
            this.lblcalendarSunStart.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblcalendarSunStart.Name = "lblcalendarSunStart";
            this.lblcalendarSunStart.Size = new System.Drawing.Size(80, 17);
            this.lblcalendarSunStart.TabIndex = 12;
            this.lblcalendarSunStart.Text = "03/12/2020";
            this.lblcalendarSunStart.TextChanged += new System.EventHandler(this.lblcalendarSunStart_TextChanged);
            // 
            // lblcalendarSunEnd
            // 
            this.lblcalendarSunEnd.AutoSize = true;
            this.lblcalendarSunEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblcalendarSunEnd.Location = new System.Drawing.Point(260, 225);
            this.lblcalendarSunEnd.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblcalendarSunEnd.Name = "lblcalendarSunEnd";
            this.lblcalendarSunEnd.Size = new System.Drawing.Size(80, 17);
            this.lblcalendarSunEnd.TabIndex = 12;
            this.lblcalendarSunEnd.Text = "03/12/2020";
            this.lblcalendarSunEnd.TextChanged += new System.EventHandler(this.lblcalendarSunEnd_TextChanged);
            // 
            // lblcalendarMoonStart
            // 
            this.lblcalendarMoonStart.AutoSize = true;
            this.lblcalendarMoonStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblcalendarMoonStart.Location = new System.Drawing.Point(369, 165);
            this.lblcalendarMoonStart.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblcalendarMoonStart.Name = "lblcalendarMoonStart";
            this.lblcalendarMoonStart.Size = new System.Drawing.Size(80, 17);
            this.lblcalendarMoonStart.TabIndex = 12;
            this.lblcalendarMoonStart.Text = "03/12/2020";
            this.lblcalendarMoonStart.TextChanged += new System.EventHandler(this.lblcalendarMoonStart_TextChanged);
            // 
            // lblcalendarMoonEnd
            // 
            this.lblcalendarMoonEnd.AutoSize = true;
            this.lblcalendarMoonEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblcalendarMoonEnd.Location = new System.Drawing.Point(369, 225);
            this.lblcalendarMoonEnd.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblcalendarMoonEnd.Name = "lblcalendarMoonEnd";
            this.lblcalendarMoonEnd.Size = new System.Drawing.Size(80, 17);
            this.lblcalendarMoonEnd.TabIndex = 12;
            this.lblcalendarMoonEnd.Text = "03/12/2020";
            this.lblcalendarMoonEnd.TextChanged += new System.EventHandler(this.lblcalendarMoonEnd_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.Location = new System.Drawing.Point(369, 192);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "Ngày:";
            // 
            // lbldayStartM
            // 
            this.lbldayStartM.AutoSize = true;
            this.lbldayStartM.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbldayStartM.Location = new System.Drawing.Point(415, 192);
            this.lbldayStartM.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbldayStartM.Name = "lbldayStartM";
            this.lbldayStartM.Size = new System.Drawing.Size(68, 17);
            this.lbldayStartM.TabIndex = 12;
            this.lbldayStartM.Text = "Canh thìn";
            // 
            // lblmonthStartM
            // 
            this.lblmonthStartM.AutoSize = true;
            this.lblmonthStartM.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblmonthStartM.Location = new System.Drawing.Point(551, 192);
            this.lblmonthStartM.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblmonthStartM.Name = "lblmonthStartM";
            this.lblmonthStartM.Size = new System.Drawing.Size(60, 17);
            this.lblmonthStartM.TabIndex = 12;
            this.lblmonthStartM.Text = "Đinh hợi";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.Location = new System.Drawing.Point(496, 192);
            this.label7.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 17);
            this.label7.TabIndex = 12;
            this.label7.Text = "Tháng:";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label8.Location = new System.Drawing.Point(624, 192);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 17);
            this.label8.TabIndex = 12;
            this.label8.Text = "Năm:";
            this.label8.Click += new System.EventHandler(this.label7_Click);
            // 
            // lblyearStartM
            // 
            this.lblyearStartM.AutoSize = true;
            this.lblyearStartM.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblyearStartM.Location = new System.Drawing.Point(667, 192);
            this.lblyearStartM.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblyearStartM.Name = "lblyearStartM";
            this.lblyearStartM.Size = new System.Drawing.Size(56, 17);
            this.lblyearStartM.TabIndex = 12;
            this.lblyearStartM.Text = "Canh tý";
            this.lblyearStartM.Click += new System.EventHandler(this.label7_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label16.Location = new System.Drawing.Point(369, 252);
            this.label16.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(45, 17);
            this.label16.TabIndex = 12;
            this.label16.Text = "Ngày:";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label17.Location = new System.Drawing.Point(496, 252);
            this.label17.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 17);
            this.label17.TabIndex = 12;
            this.label17.Text = "Tháng:";
            this.label17.Click += new System.EventHandler(this.label7_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label18.Location = new System.Drawing.Point(624, 252);
            this.label18.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(41, 17);
            this.label18.TabIndex = 12;
            this.label18.Text = "Năm:";
            this.label18.Click += new System.EventHandler(this.label7_Click);
            // 
            // lblyearEndM
            // 
            this.lblyearEndM.AutoSize = true;
            this.lblyearEndM.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblyearEndM.Location = new System.Drawing.Point(667, 252);
            this.lblyearEndM.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblyearEndM.Name = "lblyearEndM";
            this.lblyearEndM.Size = new System.Drawing.Size(56, 17);
            this.lblyearEndM.TabIndex = 12;
            this.lblyearEndM.Text = "Canh tý";
            this.lblyearEndM.Click += new System.EventHandler(this.label7_Click);
            // 
            // lbldayEndM
            // 
            this.lbldayEndM.AutoSize = true;
            this.lbldayEndM.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lbldayEndM.Location = new System.Drawing.Point(415, 252);
            this.lbldayEndM.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lbldayEndM.Name = "lbldayEndM";
            this.lbldayEndM.Size = new System.Drawing.Size(68, 17);
            this.lbldayEndM.TabIndex = 12;
            this.lbldayEndM.Text = "Canh thìn";
            // 
            // lblmonthEndM
            // 
            this.lblmonthEndM.AutoSize = true;
            this.lblmonthEndM.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblmonthEndM.Location = new System.Drawing.Point(551, 252);
            this.lblmonthEndM.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblmonthEndM.Name = "lblmonthEndM";
            this.lblmonthEndM.Size = new System.Drawing.Size(60, 17);
            this.lblmonthEndM.TabIndex = 12;
            this.lblmonthEndM.Text = "Đinh hợi";
            // 
            // AddFamilyEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.lbltimeend);
            this.Controls.Add(this.lblcalendarMoonEnd);
            this.Controls.Add(this.lblcalendarSunEnd);
            this.Controls.Add(this.lblcalendarMoonStart);
            this.Controls.Add(this.lblmonthEndM);
            this.Controls.Add(this.lblmonthStartM);
            this.Controls.Add(this.lbldayEndM);
            this.Controls.Add(this.lbldayStartM);
            this.Controls.Add(this.lblyearEndM);
            this.Controls.Add(this.lblyearStartM);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblcalendarSunStart);
            this.Controls.Add(this.lbltimestart);
            this.Controls.Add(this.ckimportant);
            this.Controls.Add(this.cbForeColor);
            this.Controls.Add(this.rdcalendarM);
            this.Controls.Add(this.rdcalendarS);
            this.Controls.Add(this.btncalendarendDate);
            this.Controls.Add(this.btncalendarstartDate);
            this.Controls.Add(this.btnsave);
            this.Controls.Add(this.cbrepType);
            this.Controls.Add(this.btncancel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cbBackColor);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtEventPlace);
            this.Controls.Add(this.txtEventName);
            this.Controls.Add(this.ckactiveEvent);
            this.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "AddFamilyEvent";
            this.Sizable = false;
            this.Size = new System.Drawing.Size(800, 520);
            this.Tag = "a";
            this.Load += new System.EventHandler(this.AddMember_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ckactiveEvent;
        private System.Windows.Forms.TextBox txtEventName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbrepType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtEventPlace;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private MaterialSkin.Controls.MaterialDropDownColorPicker cbBackColor;
        private System.Windows.Forms.RadioButton rdcalendarM;
        private System.Windows.Forms.RadioButton rdcalendarS;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btncancel;
        private System.Windows.Forms.Button btnsave;
        private System.Windows.Forms.Label label15;
        private MaterialSkin.Controls.MaterialDropDownColorPicker cbForeColor;
        private System.Windows.Forms.Button btncalendarstartDate;
        private System.Windows.Forms.Button btncalendarendDate;
        private System.Windows.Forms.CheckBox ckimportant;
        private System.Windows.Forms.Label lbltimestart;
        private System.Windows.Forms.Label lbltimeend;
        private System.Windows.Forms.Label lblcalendarSunStart;
        private System.Windows.Forms.Label lblcalendarSunEnd;
        private System.Windows.Forms.Label lblcalendarMoonStart;
        private System.Windows.Forms.Label lblcalendarMoonEnd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbldayStartM;
        private System.Windows.Forms.Label lblmonthStartM;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblyearStartM;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lblyearEndM;
        private System.Windows.Forms.Label lbldayEndM;
        private System.Windows.Forms.Label lblmonthEndM;
    }
}
