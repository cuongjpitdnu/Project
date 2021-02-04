using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace GP40Common
{
    /// <summary>
    /// An enumeration describing various ways to view the calendar
    /// </summary>
    public enum CalendarViews
    {
        /// <summary>
        /// Renders the Calendar in a month view
        /// </summary>
        Month = 1,
        /// <summary>
        /// Renders the Calendar in a day view
        /// </summary>
        Day = 2
    }

    /// <summary>
    /// A Winforms Calendar Control
    /// </summary>
    public class Calendar : UserControl
    {
        enum ENUM_DAYOFWEEK
        {
            Sun = 0,
            Mon,
            Tue,
            Wed,
            Thu,
            Fri,
            Sat       
        }
        private string[] dayofWeek = { "Chủ Nhật", "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy" };

        private class DayInView
        {
            public Rectangle rectDay;
            public clsLunarCalendar lnDate;

            public DayInView(Rectangle rect, clsLunarCalendar dtD)
            {
                //rectDay = new Rectangle(rect.X, rect.Y, rect.Width,rect.Height);
                rectDay = rect;
                lnDate = dtD;
            }
        }

        List<SizeF> _lstDaySize;
        List<DayInView> _lstDayinView;        

        private DateTime _calendarSolarDate;
        private clsLunarCalendar _calendarLunarDate;
        private Font _dayOfWeekFont;
        private Font _daysFont;
        private Font _todayFont;
        private Font _dateHeaderFont;
        private Font _dayViewTimeFont;
        private bool _showArrowControls;
        private bool _showTodayButton;
        private bool _showDateInHeader;
        private CoolButton _btnToday;
        private CoolButton _btnLeft;
        private CoolButton _btnRight;
        private bool _showingToolTip;
        private bool _showEventTooltips;
        private bool _loadPresetHolidays;
        private CalendarEvent _clickedEvent;
        private bool _showDisabledEvents;
        private bool _showDashedBorderOnDisabledEvents;
        private bool _dimDisabledEvents;
        private bool _highlightCurrentDay;
        private CalendarViews _calendarView;
        private readonly ScrollPanel _scrollPanel;

        private readonly List<IEvent> _events;
        private readonly List<Rectangle> _rectangles;
        private readonly Dictionary<int, Point> _calendarDays;
        private readonly List<CalendarEvent> _calendarEvents;
        private readonly EventToolTip _eventTip;
        private ContextMenuStrip _contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem _miProperties;
        private DateTimePicker dtpYear;
        private ComboBox cboMonth;        
        private Label lblLunarDate;
        private const int MarginSize = 20;

        /// <summary>
        /// Indicates the font for the times on the day view
        /// </summary>
        public Font DayViewTimeFont
        {
            get { return _dayViewTimeFont; }
            set
            {
                _dayViewTimeFont = value;
                if (_calendarView == CalendarViews.Day)
                    _scrollPanel.Refresh();
                else Refresh();
            }
        }

        /// <summary>
        /// Indicates the type of calendar to render, Month or Day view
        /// </summary>
        public CalendarViews CalendarView
        {
            get { return _calendarView; }
            set
            {
                _calendarView = value;
                _scrollPanel.Visible = value == CalendarViews.Day;                
                Refresh();
            }
        }

        /// <summary>
        /// Indicates whether today's date should be highlighted
        /// </summary>
        public bool HighlightCurrentDay
        {
            get { return _highlightCurrentDay; }
            set
            {
                _highlightCurrentDay = value;
                Refresh();
            }
        }

        /// <summary>
        /// Indicates whether events can be right-clicked and edited
        /// </summary>
        public bool AllowEditingEvents
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether disabled events will appear as "dimmed".
        /// This property is only used if <see cref="ShowDisabledEvents"/> is set to true.
        /// </summary>
        public bool DimDisabledEvents
        {
            get { return _dimDisabledEvents; }
            set
            {
                _dimDisabledEvents = value;
                Refresh();
            }
        }

        /// <summary>
        /// Indicates if a dashed border should show up around disabled events.
        /// This property is only used if <see cref="ShowDisabledEvents"/> is set to true.
        /// </summary>
        public bool ShowDashedBorderOnDisabledEvents
        {
            get { return _showDashedBorderOnDisabledEvents; }
            set
            {
                _showDashedBorderOnDisabledEvents = value;
                Refresh();
            }
        }

        /// <summary>
        /// Indicates whether disabled events should show up on the calendar control
        /// </summary>
        public bool ShowDisabledEvents
        {
            get { return _showDisabledEvents; }
            set
            {
                _showDisabledEvents = value;
                Refresh();
            }
        }

        /// <summary>
        /// Indicates whether Federal Holidays are automatically preloaded onto the calendar
        /// </summary>
        public bool LoadPresetHolidays
        {
            get { return _loadPresetHolidays; }
            set
            {
                _loadPresetHolidays = value;
                if (_loadPresetHolidays)
                {
                    _events.Clear();
                    PresetHolidays();
                    Refresh();
                }
                else
                {
                    _events.Clear();
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Indicates whether hovering over an event will display a tooltip of the event
        /// </summary>
        public bool ShowEventTooltips
        {
            get { return _showEventTooltips; }
            set { _showEventTooltips = value; _eventTip.Visible = false; }
        }

        /// <summary>
        /// Get or Set this value to the Font you wish to use to render the date in the upper right corner
        /// </summary>
        public Font DateHeaderFont
        {
            get { return _dateHeaderFont; }
            set
            {
                _dateHeaderFont = value;
                Refresh();
            }
        }

        /// <summary>
        /// Indicates whether the date should be displayed in the upper right hand corner of the calendar control
        /// </summary>
        public bool ShowDateInHeader
        {
            get { return _showDateInHeader; }
            set
            {
                _showDateInHeader = value;
                if (_calendarView == CalendarViews.Day)
                    ResizeScrollPanel();

                Refresh();
            }
        }

        /// <summary>
        /// Indicates whether the calendar control should render the previous/next month buttons
        /// </summary>
        public bool ShowArrowControls
        {
            get { return _showArrowControls; }
            set
            {
                _showArrowControls = value;
                _btnLeft.Visible = value;
                _btnRight.Visible = value;
                if (_calendarView == CalendarViews.Day)
                    ResizeScrollPanel();
                Refresh();
            }
        }

        /// <summary>
        /// Indicates whether the calendar control should render the Today button
        /// </summary>
        public bool ShowTodayButton
        {
            get { return _showTodayButton; }
            set
            {
                _showTodayButton = value;
                _btnToday.Visible = value;
                if (_calendarView == CalendarViews.Day)
                    ResizeScrollPanel();
                Refresh();
            }
        }

        /// <summary>
        /// The font used to render the Today button
        /// </summary>
        public Font TodayFont
        {
            get { return _todayFont; }
            set
            {
                _todayFont = value;
                Refresh();
            }
        }

        /// <summary>
        /// The font used to render the number days on the calendar
        /// </summary>
        public Font DaysFont
        {
            get { return _daysFont; }
            set
            {
                _daysFont = value;
                Refresh();
            }
        }

        /// <summary>
        /// The font used to render the days of the week text
        /// </summary>
        public Font DayOfWeekFont
        {
            get { return _dayOfWeekFont; }
            set
            {
                _dayOfWeekFont = value;
                Refresh();
            }
        }

        /// <summary>
        /// The Date that the calendar is currently showing
        /// </summary>
        public DateTime CalendarSolarDate
        {
            get { return _calendarSolarDate; }
            set
            {
                _calendarSolarDate = value;
                dtpYear.Value = _calendarSolarDate;
                cboMonth.SelectedItem = "Tháng " + _calendarSolarDate.Month.ToString();
                _calendarLunarDate = new clsLunarCalendar(_calendarSolarDate);
                lblLunarDate.Text = _calendarLunarDate.GetLunarDateInfo();
                Refresh();
            }
        }

        public clsLunarCalendar CalendarLunarDate
        {
            get { return _calendarLunarDate; }
        }

        /// <summary>
        /// Calendar Constructor
        /// </summary>
        public Calendar()
        {
            InitializeComponent();
            _lstDaySize = new List<SizeF>();
            _lstDayinView = new List<DayInView>();
            _calendarSolarDate = DateTime.Now;
            _dayOfWeekFont = new Font("Arial", 10, FontStyle.Regular);
            _daysFont = new Font("Arial", 10, FontStyle.Regular);
            _todayFont = new Font("Arial", 12, FontStyle.Bold);
            _dateHeaderFont = new Font("Arial", 12, FontStyle.Bold);
            _dayViewTimeFont = new Font("Arial", 10, FontStyle.Bold);
            _showArrowControls = true;
            _showDateInHeader = true;
            _showTodayButton = true;
            _showingToolTip = false;
            _clickedEvent = null;
            _showDisabledEvents = false;
            _showDashedBorderOnDisabledEvents = true;
            _dimDisabledEvents = true;
            AllowEditingEvents = true;
            _highlightCurrentDay = true;
            _calendarView = CalendarViews.Month;
            _scrollPanel = new ScrollPanel();

            _scrollPanel.RightButtonClicked += ScrollPanelRightButtonClicked;

            _events = new List<IEvent>();
            _rectangles = new List<Rectangle>();
            _calendarDays = new Dictionary<int, Point>();
            _calendarEvents = new List<CalendarEvent>();
            _showEventTooltips = true;
            _eventTip = new EventToolTip { Visible = false };

            Controls.Add(_eventTip);

            LoadPresetHolidays = true;

            _scrollPanel.Visible = false;
            Controls.Add(_scrollPanel);           
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._miProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.dtpYear = new System.Windows.Forms.DateTimePicker();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.lblLunarDate = new System.Windows.Forms.Label();
            this._btnRight = new GP40Common.CoolButton();
            this._btnLeft = new GP40Common.CoolButton();
            this._btnToday = new GP40Common.CoolButton();
            this._contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _contextMenuStrip1
            // 
            this._contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._miProperties});
            this._contextMenuStrip1.Name = "_contextMenuStrip1";
            this._contextMenuStrip1.Size = new System.Drawing.Size(137, 26);
            // 
            // _miProperties
            // 
            this._miProperties.Name = "_miProperties";
            this._miProperties.Size = new System.Drawing.Size(136, 22);
            this._miProperties.Text = "Properties...";
            this._miProperties.Click += new System.EventHandler(this.MenuItemPropertiesClick);
            // 
            // dtpYear
            // 
            this.dtpYear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpYear.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpYear.CustomFormat = "yyyy";
            this.dtpYear.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpYear.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpYear.Location = new System.Drawing.Point(216, 13);
            this.dtpYear.Name = "dtpYear";
            this.dtpYear.ShowUpDown = true;
            this.dtpYear.Size = new System.Drawing.Size(70, 26);
            this.dtpYear.TabIndex = 3;
            this.dtpYear.ValueChanged += new System.EventHandler(this.dtpYear_ValueChanged);
            // 
            // cboMonth
            // 
            this.cboMonth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboMonth.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboMonth.FormattingEnabled = true;
            this.cboMonth.Items.AddRange(new object[] {
            "Tháng 1",
            "Tháng 2",
            "Tháng 3",
            "Tháng 4",
            "Tháng 5",
            "Tháng 6",
            "Tháng 7",
            "Tháng 8",
            "Tháng 9",
            "Tháng 10",
            "Tháng 11",
            "Tháng 12"});
            this.cboMonth.Location = new System.Drawing.Point(349, 13);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.Size = new System.Drawing.Size(90, 28);
            this.cboMonth.TabIndex = 4;
            this.cboMonth.SelectedValueChanged += new System.EventHandler(this.cboMonth_SelectedValueChanged);
            // 
            // lblLunarDate
            // 
            this.lblLunarDate.AutoSize = true;
            this.lblLunarDate.Location = new System.Drawing.Point(16, 463);
            this.lblLunarDate.Name = "lblLunarDate";
            this.lblLunarDate.Size = new System.Drawing.Size(35, 13);
            this.lblLunarDate.TabIndex = 5;
            this.lblLunarDate.Text = "label1";
            // 
            // _btnRight
            // 
            this._btnRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnRight.BackColor = System.Drawing.Color.Transparent;
            this._btnRight.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._btnRight.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this._btnRight.ButtonFont = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this._btnRight.ButtonText = ">";
            this._btnRight.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(144)))), ((int)(((byte)(254)))));
            this._btnRight.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnRight.HighlightBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(198)))), ((int)(((byte)(198)))));
            this._btnRight.HighlightButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this._btnRight.Location = new System.Drawing.Point(443, 9);
            this._btnRight.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnRight.Name = "_btnRight";
            this._btnRight.Size = new System.Drawing.Size(54, 37);
            this._btnRight.TabIndex = 2;
            this._btnRight.TextColor = System.Drawing.Color.Black;
            this._btnRight.ButtonClicked += new GP40Common.CoolButton.ButtonClickedArgs(this.BtnRightButtonClicked);
            // 
            // _btnLeft
            // 
            this._btnLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._btnLeft.BackColor = System.Drawing.Color.Transparent;
            this._btnLeft.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._btnLeft.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this._btnLeft.ButtonFont = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this._btnLeft.ButtonText = "<";
            this._btnLeft.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(144)))), ((int)(((byte)(254)))));
            this._btnLeft.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnLeft.HighlightBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(198)))), ((int)(((byte)(198)))));
            this._btnLeft.HighlightButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this._btnLeft.Location = new System.Drawing.Point(292, 9);
            this._btnLeft.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnLeft.Name = "_btnLeft";
            this._btnLeft.Size = new System.Drawing.Size(54, 37);
            this._btnLeft.TabIndex = 1;
            this._btnLeft.TextColor = System.Drawing.Color.Black;
            this._btnLeft.ButtonClicked += new GP40Common.CoolButton.ButtonClickedArgs(this.BtnLeftButtonClicked);
            // 
            // _btnToday
            // 
            this._btnToday.BackColor = System.Drawing.Color.Transparent;
            this._btnToday.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this._btnToday.ButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(243)))), ((int)(((byte)(243)))));
            this._btnToday.ButtonFont = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._btnToday.ButtonText = "Hôm nay";
            this._btnToday.FocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(144)))), ((int)(((byte)(254)))));
            this._btnToday.HighlightBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(198)))), ((int)(((byte)(198)))), ((int)(((byte)(198)))));
            this._btnToday.HighlightButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this._btnToday.Location = new System.Drawing.Point(19, 9);
            this._btnToday.Name = "_btnToday";
            this._btnToday.Size = new System.Drawing.Size(84, 37);
            this._btnToday.TabIndex = 0;
            this._btnToday.TextColor = System.Drawing.Color.Black;
            this._btnToday.ButtonClicked += new GP40Common.CoolButton.ButtonClickedArgs(this.BtnTodayButtonClicked);
            this._btnToday.Load += new System.EventHandler(this._btnToday_Load);
            // 
            // Calendar
            // 
            this.Controls.Add(this.lblLunarDate);
            this.Controls.Add(this.cboMonth);
            this.Controls.Add(this.dtpYear);
            this.Controls.Add(this._btnRight);
            this.Controls.Add(this._btnLeft);
            this.Controls.Add(this._btnToday);
            this.DoubleBuffered = true;
            this.Name = "Calendar";
            this.Size = new System.Drawing.Size(512, 490);
            this.Load += new System.EventHandler(this.CalendarLoad);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.CalendarPaint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CalendarMouseClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CalendarMouseMove);
            this.Resize += new System.EventHandler(this.CalendarResize);
            this._contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// Adds an event to the calendar
        /// </summary>
        /// <param name="calendarEvent">The <see cref="IEvent"/> to add to the calendar</param>
        public void AddEvent(IEvent calendarEvent)
        {
            _events.Add(calendarEvent);
            Refresh();
        }

        /// <summary>
        /// Removes an event from the calendar
        /// </summary>
        /// <param name="calendarEvent">The <see cref="IEvent"/> to remove to the calendar</param>
        public void RemoveEvent(IEvent calendarEvent)
        {
            _events.Remove(calendarEvent);
            Refresh();
        }

        private void CalendarLoad(object sender, EventArgs e)
        {
            if (Parent != null)
                Parent.Resize += ParentResize;
            ResizeScrollPanel();
        }

        private void CalendarPaint(object sender, PaintEventArgs e)
        {
            if (_showingToolTip)
                return;

            if (_calendarView == CalendarViews.Month)
                RenderMonthCalendar(e);
            if (_calendarView == CalendarViews.Day)
                RenderDayCalendar(e);
        }

        private void CalendarMouseMove(object sender, MouseEventArgs e)
        {
            if (!_showEventTooltips)
                return;

            int num = _calendarEvents.Count;
            for (int i = 0; i < num; i++)
            {
                var z = _calendarEvents[i];

                if ((z.EventArea.Contains(e.X, e.Y) && z.Event.TooltipEnabled && _calendarView == CalendarViews.Month) ||
                    (_calendarView == CalendarViews.Day && z.EventArea.Contains(e.X, e.Y + _scrollPanel.ScrollOffset) && z.Event.TooltipEnabled))
                {
                    _eventTip.ShouldRender = false;
                    _showingToolTip = true;
                    _eventTip.EventToolTipText = z.Event.EventText;
                    if (z.Event.IgnoreTimeComponent == false)
                        _eventTip.EventToolTipText += "\n" + z.Event.Date.ToShortTimeString();
                    _eventTip.Location = new Point(e.X + 5, e.Y - _eventTip.CalculateSize().Height);
                    _eventTip.ShouldRender = true;
                    _eventTip.Visible = true;

                    _showingToolTip = false;
                    return;
                }
            }

            _eventTip.Visible = false;
            _eventTip.ShouldRender = false;
        }

        private void ScrollPanelRightButtonClicked(object sender, MouseEventArgs e)
        {
            if (AllowEditingEvents && _calendarView == CalendarViews.Day)
            {
                int num = _calendarEvents.Count;
                for (int i = 0; i < num; i++)
                {
                    var z = _calendarEvents[i];

                    if (z.EventArea.Contains(e.X, e.Y + _scrollPanel.ScrollOffset) && !z.Event.ReadOnlyEvent)
                    {
                        _clickedEvent = z;
                        _contextMenuStrip1.Show(_scrollPanel, new Point(e.X, e.Y));
                        _eventTip.Visible = false;
                        _eventTip.ShouldRender = false;
                        break;
                    }
                }
            }
        }

        private void CalendarMouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < _lstDayinView.Count; i++)
                {
                    if (_lstDayinView[i].rectDay.Contains(e.X, e.Y))
                    {
                        //lblLunarDate.Text = _lstDayinView[i].dtDate.GetLunarDateName();
                        //lblLunarDate.Text = _lstDayinView[i].lnDate.DayInfo;
                        CalendarSolarDate = _lstDayinView[i].lnDate.mdtSolarDate;
                        //_selectLunarDate = _lstDayinView[i].lnDate;
                        //this.Invalidate();
                        return;
                    }    
                }    

                return;
            }

            if (e.Button == MouseButtons.Right && AllowEditingEvents)
            {
                if (_calendarView == CalendarViews.Month)
                {
                    int num = _calendarEvents.Count;
                    for (int i = 0; i < num; i++)
                    {
                        var z = _calendarEvents[i];

                        if (z.EventArea.Contains(e.X, e.Y) && !z.Event.ReadOnlyEvent)
                        {
                            _clickedEvent = z;
                            _contextMenuStrip1.Show(this, e.Location);
                            _eventTip.Visible = false;
                            _eventTip.ShouldRender = false;
                            break;
                        }
                    }
                }
                return;
            }
        }

        private void BtnTodayButtonClicked(object sender)
        {
            CalendarSolarDate = DateTime.Now;
            Refresh();
        }

        private void BtnLeftButtonClicked(object sender)
        {
            if (_calendarView == CalendarViews.Month)
                CalendarSolarDate = _calendarSolarDate.AddMonths(-1);
            else if (_calendarView == CalendarViews.Day)
                CalendarSolarDate = _calendarSolarDate.AddDays(-1);
            Refresh();
        }

        private void BtnRightButtonClicked(object sender)
        {
            if (_calendarView == CalendarViews.Day)            
                CalendarSolarDate = _calendarSolarDate.AddDays(1);            
            else if (_calendarView == CalendarViews.Month)
                CalendarSolarDate = _calendarSolarDate.AddMonths(1);
            
            Refresh();
        }

        private void MenuItemPropertiesClick(object sender, EventArgs e)
        {
            if (_clickedEvent == null)
                return;

            var ed = new EventDetails { Event = _clickedEvent.Event };

            if (ed.ShowDialog(this) == DialogResult.OK)
            {
                _events.Remove(_clickedEvent.Event);
                _events.Add(ed.NewEvent);
                Refresh();
            }
            _clickedEvent = null;
        }

        private void ParentResize(object sender, EventArgs e)
        {
            ResizeScrollPanel();
            Refresh();
        }

        private void PresetHolidays()
        {           

            //var memorialDay = new HolidayEvent
            //{
            //    Date = new DateTime(DateTime.Now.Year, 5, 28),
            //    RecurringFrequency = RecurringFrequencies.Custom,
            //    EventText = "Memorial Day",
            //    CustomRecurringFunction = MemorialDayHandler
            //};
            //AddEvent(memorialDay);

            var newYears = new HolidayEvent
            {
                Date = new DateTime(DateTime.Now.Year, 1, 1),
                RecurringFrequency = RecurringFrequencies.Yearly,
                EventText = "Tết Dương Lịch"
            };
            AddEvent(newYears);

            var aprilFools = new HolidayEvent
            {
                Date = new DateTime(DateTime.Now.Year, 4, 1),
                RecurringFrequency = RecurringFrequencies.Yearly,
                EventText = "Ngày Nói Dối"
            };
            AddEvent(aprilFools);

            //var mlkDay = new HolidayEvent
            //{
            //    Date = new DateTime(DateTime.Now.Year, 1, 15),
            //    RecurringFrequency = RecurringFrequencies.Custom,
            //    EventText = "Martin Luther King Jr. Day",
            //    CustomRecurringFunction = MlkDayHandler
            //};
            //AddEvent(mlkDay);

            var presidentsDay = new HolidayEvent
            {
                Date = new DateTime(DateTime.Now.Year, 5, 26),
                //RecurringFrequency = RecurringFrequencies.Custom,
                RecurringFrequency = RecurringFrequencies.None,
                EventText = "President's Day",
                //CustomRecurringFunction = MlkDayHandler
            };
            AddEvent(presidentsDay);

            var southFree = new HolidayEvent
            {
                Date = new DateTime(DateTime.Now.Year, 4, 30),
                RecurringFrequency = RecurringFrequencies.Yearly,
                EventText = "Ngày Giải Phóng Miền Nam"
            };
            AddEvent(southFree);

            var laborDay = new HolidayEvent
            {
                Date = new DateTime(DateTime.Now.Year, 5, 1),
                RecurringFrequency = RecurringFrequencies.Yearly,
                EventText = "Quốc tế Lao Động",
                CustomRecurringFunction = LaborDayHandler
            };
            AddEvent(laborDay);

            var chilDay = new HolidayEvent
            {
                Date = new DateTime(DateTime.Now.Year, 6, 1),
                RecurringFrequency = RecurringFrequencies.Yearly,
                EventText = "Quốc tế thiếu nhi",
                CustomRecurringFunction = LaborDayHandler
            };
            AddEvent(chilDay);

            var independanceDay = new HolidayEvent
            {
                Date = new DateTime(DateTime.Now.Year, 9, 2),
                RecurringFrequency = RecurringFrequencies.Yearly,
                EventText = "Ngày Quốc Khánh"
            };
            AddEvent(independanceDay);

           

            //var columbusDay = new HolidayEvent
            //{
            //    Date = new DateTime(DateTime.Now.Year, 10, 14),
            //    RecurringFrequency = RecurringFrequencies.Custom,
            //    EventText = "Columbus Day",
            //    CustomRecurringFunction = ColumbusDayHandler
            //};
            //AddEvent(columbusDay);

            //var veteransDay = new HolidayEvent
            //{
            //    Date = new DateTime(DateTime.Now.Year, 11, 11),
            //    RecurringFrequency = RecurringFrequencies.Yearly,
            //    EventText = "Veteran's Day"
            //};
            //AddEvent(veteransDay);

            //var thanksgivingDay = new HolidayEvent
            //{
            //    Date = new DateTime(DateTime.Now.Year, 11, 11),
            //    RecurringFrequency = RecurringFrequencies.Custom,
            //    EventText = "Thanksgiving Day",
            //    CustomRecurringFunction = ThanksgivingDayHandler
            //};
            //AddEvent(thanksgivingDay);

            var christmas = new HolidayEvent
            {
                Date = new DateTime(DateTime.Now.Year, 12, 25),
                RecurringFrequency = RecurringFrequencies.Yearly,
                EventText = "Giáng Sinh"
            };
            AddEvent(christmas);
        }

        [CustomRecurringFunction("Thanksgiving Day Handler", "Selects the fourth Thursday in the month")]
        private bool ThanksgivingDayHandler(IEvent evnt, DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Thursday && dt.Day > 21 && dt.Day <= 28 && dt.Month == evnt.Date.Month)
                return true;
            return false;
        }

        [CustomRecurringFunction("Columbus Day Handler", "Selects the second Monday in the month")]
        private bool ColumbusDayHandler(IEvent evnt, DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Monday && dt.Day > 7 && dt.Day <= 14 && dt.Month == evnt.Date.Month)
                return true;
            return false;
        }

        [CustomRecurringFunction("Labor Day Handler", "Selects the first Monday in the month")]
        private bool LaborDayHandler(IEvent evnt, DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Monday && dt.Day <= 7 && dt.Month == evnt.Date.Month)
                return true;
            return false;
        }

        [CustomRecurringFunction("Martin Luther King Jr. Day Handler", "Selects the third Monday in the month")]
        private bool MlkDayHandler(IEvent evnt, DateTime dt)
        {
            if (dt.DayOfWeek == DayOfWeek.Monday && dt.Day > 14 && dt.Day <= 21 && dt.Month == evnt.Date.Month)
                return true;
            return false;
        }

        [CustomRecurringFunction("Memorial Day Handler", "Selects the last Monday in the month")]
        private bool MemorialDayHandler(IEvent evnt, DateTime dt)
        {
            DateTime dt2 = LastDayOfWeekInMonth(dt, DayOfWeek.Monday);
            if (dt.Month == evnt.Date.Month && dt2.Day == dt.Date.Day)
                return true;

            return false;
        }

        private DateTime LastDayOfWeekInMonth(DateTime day, DayOfWeek dow)
        {
            DateTime lastDay = new DateTime(day.Year, day.Month, 1).AddMonths(1).AddDays(-1);
            DayOfWeek lastDow = lastDay.DayOfWeek;

            int diff = dow - lastDow;

            if (diff > 0) diff -= 7;

            System.Diagnostics.Debug.Assert(diff <= 0);

            return lastDay.AddDays(diff);
        }

        private int Max(params float[] value)
        {
            return (int)value.Max(i => Math.Ceiling(i));
        }

        private bool DayForward(IEvent evnt, DateTime day)
        {
            if (evnt.ThisDayForwardOnly)
            {
                int c = DateTime.Compare(day, evnt.Date);

                if (c >= 0)
                    return true;

                return false;
            }

            return true;
        }

        internal Bitmap RequestImage()
        {
            const int cellHourWidth = 60;
            const int cellHourHeight = 30;
            var bmp = new Bitmap(ClientSize.Width, cellHourWidth * 24);
            Graphics g = Graphics.FromImage(bmp);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            var dt = new DateTime(_calendarSolarDate.Year, _calendarSolarDate.Month, _calendarSolarDate.Day, 0, 0, 0);
            int xStart = 0;
            int yStart = 0;

            g.DrawRectangle(Pens.Black, 0, 0, ClientSize.Width - MarginSize * 2 - 2, cellHourHeight * 24);
            for (int i = 0; i < 24; i++)
            {
                var textWidth = (int)g.MeasureString(dt.ToString("htt").ToLower(), _dayViewTimeFont).Width;
                g.DrawRectangle(Pens.Black, xStart, yStart, cellHourWidth, cellHourHeight);
                g.DrawLine(Pens.Black, xStart + cellHourWidth, yStart + cellHourHeight,
                           ClientSize.Width - MarginSize * 2 - 3, yStart + cellHourHeight);
                g.DrawLine(Pens.DarkGray, xStart + cellHourWidth, yStart + cellHourHeight / 2,
                           ClientSize.Width - MarginSize * 2 - 3, yStart + cellHourHeight / 2);

                g.DrawString(dt.ToString("htt").ToLower(), _dayViewTimeFont, Brushes.Black, xStart + cellHourWidth - textWidth, yStart);
                yStart += cellHourHeight;
                dt = dt.AddHours(1);
            }

            dt = new DateTime(_calendarSolarDate.Year, _calendarSolarDate.Month, _calendarSolarDate.Day, 23, 59, 0);

            List<IEvent> evnts = _events.Where(evnt => NeedsRendering(evnt, dt)).ToList().OrderBy(d => d.Date).ToList();

            xStart = cellHourWidth + 1;
            yStart = 0;

            g.Clip = new Region(new Rectangle(0, 0, ClientSize.Width - MarginSize * 2 - 2, cellHourHeight * 24));
            _calendarEvents.Clear();
            for (int i = 0; i < 24; i++)
            {
                dt = new DateTime(_calendarSolarDate.Year, _calendarSolarDate.Month, _calendarSolarDate.Day, 0, 0, 0);
                dt = dt.AddHours(i);
                foreach (var evnt in evnts)
                {
                    TimeSpan ts = TimeSpan.FromHours(evnt.EventLengthInHours);

                    if (evnt.Date.Ticks >= dt.Ticks && evnt.Date.Ticks < dt.Add(ts).Ticks && evnt.EventLengthInHours > 0 && i >= evnt.Date.Hour)
                    {
                        int divisor = evnt.Date.Minute == 0 ? 1 : 60 / evnt.Date.Minute;
                        Color clr = Color.FromArgb(175, evnt.EventColor.R, evnt.EventColor.G, evnt.EventColor.B);
                        g.FillRectangle(new SolidBrush(GetFinalBackColor()), xStart, yStart + cellHourHeight / divisor + 1, ClientSize.Width - MarginSize * 2 - cellHourWidth - 3, cellHourHeight * ts.Hours - 1);
                        g.FillRectangle(new SolidBrush(clr), xStart, yStart + cellHourHeight / divisor + 1, ClientSize.Width - MarginSize * 2 - cellHourWidth - 3, cellHourHeight * ts.Hours - 1);
                        g.DrawString(evnt.EventText, evnt.EventFont, new SolidBrush(evnt.EventTextColor), xStart, yStart + cellHourHeight / divisor);

                        var ce = new CalendarEvent
                                     {
                                         Event = evnt,
                                         Date = dt,
                                         EventArea = new Rectangle(xStart, yStart + cellHourHeight / divisor + 1,
                                                                   ClientSize.Width - MarginSize * 2 - cellHourWidth - 3,
                                                                   cellHourHeight * ts.Hours)
                                     };
                        _calendarEvents.Add(ce);
                    }
                }
                yStart += cellHourHeight;
            }

            g.Dispose();
            return bmp;
        }

        private Color GetFinalBackColor()
        {
            Control c = this;

            while (c != null)
            {
                if (c.BackColor != Color.Transparent)
                    return c.BackColor;
                c = c.Parent;
            }

            return Color.Transparent;
        }

        private void ResizeScrollPanel()
        {
            int controlsSpacing = ((!_showTodayButton) && (!_showDateInHeader) && (!_showArrowControls)) ? 0 : 30;

            _scrollPanel.Location = new Point(MarginSize, MarginSize + controlsSpacing);
            _scrollPanel.Size = new Size(ClientSize.Width - MarginSize * 2 - 1, ClientSize.Height - MarginSize - 1 - controlsSpacing);
        }

        private void RenderDayCalendar(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (_showDateInHeader)
            {
                string strValue = xCalendarValue(_calendarSolarDate, _calendarSolarDate.Day) + ", " + _calendarSolarDate.Year.ToString(CultureInfo.InvariantCulture);
                SizeF dateHeaderSize = g.MeasureString(strValue, DateHeaderFont);
                g.DrawString(strValue,_dateHeaderFont, Brushes.Black, 
                    ClientSize.Width - MarginSize - dateHeaderSize.Width, MarginSize);
            }
        }

        SizeF xDaySize(Graphics g, ENUM_DAYOFWEEK dow)
        {
            return g.MeasureString(dayofWeek[(int)dow], _dayOfWeekFont);
        }

        private void xDaySize(Graphics g)
        {
            for (int i = 0; i < dayofWeek.Length; i++)
            {
                _lstDaySize.Add(xDaySize(g, (ENUM_DAYOFWEEK)i));
            }       
        }

        public float FindMax(List<SizeF> list)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Empty list");
            }
            float maxValue = float.MinValue;
            foreach (SizeF type in list)
            {
                if (type.Height > maxValue)
                {
                    maxValue = type.Height;
                }
            }
            return maxValue;
        }

        int XCalCulate(int cellWidth, ENUM_DAYOFWEEK enDOW)
        {
            return MarginSize + ((cellWidth - (int)_lstDaySize[(int)enDOW].Width) / 2) + cellWidth*(int)enDOW;
        }

        void xDrawDayofWeek(Graphics g, int x, int y, ENUM_DAYOFWEEK enDOW)
        {
            g.DrawString(dayofWeek[(int)enDOW], _dayOfWeekFont, Brushes.Black, x, y);
        }

        string xCalendarHeaderYearValue()
        {
            return xCalendarValue(_calendarSolarDate.Year);
        }

        string xCalendarValue(int intValue)
        {
            return xCalendarValue(_calendarSolarDate, intValue);
        }

        string xCalendarValue(DateTime dt, int intValue)
        {
            return dt.ToString("MMMM") + " " + intValue.ToString(CultureInfo.InvariantCulture);
        }

        string xCalendarValue2(DateTime dt)
        {
            if (dt.Day == 1) return  dt.Day.ToString(CultureInfo.InvariantCulture) + " Th " + dt.ToString("MM");
            return dt.Day.ToString(CultureInfo.InvariantCulture);
        }
        bool xIsToDay(int intDay)
        {
            return _calendarSolarDate.Year == DateTime.Now.Year && _calendarSolarDate.Month == DateTime.Now.Month
                         && intDay == DateTime.Now.Day;
        }

        bool xIsToDay(DateTime dtValue)
        {
            return dtValue.Year == DateTime.Now.Year && dtValue.Month == DateTime.Now.Month
                         && dtValue.Day == DateTime.Now.Day;
        }

        bool xIsSelectedDay(DateTime dtValue)
        {
            return dtValue.Year == _calendarSolarDate.Year && dtValue.Month == _calendarSolarDate.Month
                         && dtValue.Day == _calendarSolarDate.Day;
        }

        void xDrawStringLunarDate(Graphics g, Rectangle rectBox, DateTime dtValue)
        {
            clsLunarCalendar objLunarDate = new clsLunarCalendar(dtValue);
            string strValue = objLunarDate.intLunarDay.ToString();
            int intLunarYPos = 25;
            if (objLunarDate.intLunarDay== 1 )
            {
                strValue += "/" + objLunarDate.intLunarMonth.ToString();
                strValue += " " + objLunarDate.GetThieuDu();
                intLunarYPos = intLunarYPos + 20;
                if (objLunarDate.LunarMonthType == clsLunarCalendar.ENUM_LEAP_MONTH.LeapMonth)
                {
                    strValue += " N ";
                    intLunarYPos = intLunarYPos + 20;
                }
            }

            if (dtValue.Month != _calendarSolarDate.Month || dtValue.Year != _calendarSolarDate.Year)
            { 
                g.DrawString(strValue, _daysFont, new SolidBrush(Color.LightGray), rectBox.Right - intLunarYPos, rectBox.Bottom - 20);
                return;
            }

            g.DrawString(strValue, xIsToDay(dtValue) ? _todayFont : _daysFont,
                                  Brushes.Black, rectBox.Right - intLunarYPos, rectBox.Bottom - 20);
        }

        void xFillDayBox(Graphics g, Rectangle rectBox, DateTime dtValue)
        {
            Brush forceColor;

            if (dtValue.Month !=  _calendarSolarDate.Month || dtValue.Year != _calendarSolarDate.Year)
            {
                g.FillRectangle(Brushes.LightSlateGray, rectBox);
                g.DrawString(xCalendarValue2(dtValue), _daysFont, new SolidBrush(Color.LightGray), rectBox.Left + 5, rectBox.Top + 2);
                xDrawStringLunarDate(g, rectBox, dtValue);
                return;
            }

            forceColor = Brushes.Blue;
            if ((dtValue.DayOfWeek == DayOfWeek.Saturday) || (dtValue.DayOfWeek == DayOfWeek.Sunday))
            {
                g.FillRectangle(Brushes.White, rectBox);
                forceColor = Brushes.Red;
            }

            if (xIsSelectedDay(dtValue)) g.FillRectangle(new SolidBrush(Color.Yellow), rectBox);
            else
            if (xIsToDay(dtValue)) g.FillRectangle(new SolidBrush(Color.LightGreen), rectBox);
            
            g.DrawString(xCalendarValue2(dtValue), xIsToDay(dtValue)?_todayFont: _daysFont,
                                    forceColor, rectBox.Left + 5, rectBox.Top + 2);

            xDrawStringLunarDate(g, rectBox, dtValue);
            return;            
        }

        private void RenderMonthCalendar(PaintEventArgs e)
        {
            _lstDaySize.Clear();
            _calendarDays.Clear();
            _calendarEvents.Clear();
            _lstDayinView.Clear();
            var bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            Graphics g = Graphics.FromImage(bmp);
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

            xDaySize(g);
            
            //SizeF dateHeaderSize = g.MeasureString(xCalendarHeaderYearValue(), _dateHeaderFont);
            int headerSpacing = (int)FindMax(_lstDaySize) + 5;
            int controlsSpacing = ((!_showTodayButton) && (!_showDateInHeader) && (!_showArrowControls)) ? 0 : 30;
            //int controlsSpacing = 5;
            int cellWidth = (ClientSize.Width - MarginSize * 2) / 7;
          
            int numWeeks = NumberOfWeeks(_calendarSolarDate.Year, _calendarSolarDate.Month);
            int cellHeight = (ClientSize.Height - MarginSize * 4 - headerSpacing - controlsSpacing) / numWeeks;
            int xStart = MarginSize;
            int yStart = MarginSize;
            DayOfWeek startWeekEnum = new DateTime(_calendarSolarDate.Year, _calendarSolarDate.Month, 1).DayOfWeek;
            int startWeek = ((int)startWeekEnum) + 1;
            int rogueDays = startWeek - 1;

            yStart += headerSpacing + controlsSpacing;

            int counter = 1;
            int counter2 = 1;                       

            //_btnToday.Location = new Point(MarginSize, MarginSize);
            yStart = _btnToday.Bottom + controlsSpacing/4;
            for (int i = 0; i < dayofWeek.Length; i++)
            {
                xStart = XCalCulate(cellWidth, (ENUM_DAYOFWEEK)i);
                xDrawDayofWeek(g, xStart, yStart, (ENUM_DAYOFWEEK)i);
            }

            //if (_showDateInHeader)
            //{
            //    g.DrawString(xCalendarHeaderYearValue(),
            //        _dateHeaderFont, Brushes.RoyalBlue, ClientSize.Width - MarginSize - dateHeaderSize.Width,
            //        MarginSize);
            //}

            yStart += (int)_lstDaySize[0].Height + controlsSpacing/4;
            xStart = MarginSize;
            for (int y = 0; y < numWeeks; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    Rectangle rectBox = new Rectangle(xStart, yStart, cellWidth, cellHeight);
                    clsLunarCalendar objLNDate;
                    
                    if (rogueDays == 0 && counter <= DateTime.DaysInMonth(_calendarSolarDate.Year, _calendarSolarDate.Month))                    
                    {
                       
                        if (!_calendarDays.ContainsKey(counter))
                            _calendarDays.Add(counter, new Point(xStart, (int)(yStart + 2f + g.MeasureString(counter.ToString(CultureInfo.InvariantCulture), _daysFont).Height)));
                                               
                        DateTime dtValue = new DateTime(_calendarSolarDate.Year, _calendarSolarDate.Month, counter);
                        objLNDate = new clsLunarCalendar(dtValue);
                        _lstDayinView.Add(new DayInView(rectBox, objLNDate));

                        xFillDayBox(g, rectBox, dtValue);
                        counter++;
                     }
                    else if (rogueDays > 0)
                    {
                        
                        DateTime dtValue = _calendarSolarDate.AddMonths(-1);
                        int dm = DateTime.DaysInMonth(dtValue.Year, dtValue.Month) - rogueDays + 1;
                       
                        dtValue = new DateTime(dtValue.Year, dtValue.Month, dm);
                        objLNDate = new clsLunarCalendar(dtValue);
                        _lstDayinView.Add(new DayInView(rectBox, objLNDate));
                        xFillDayBox(g, rectBox, dtValue);
                        
                        rogueDays--;                       
                    }
                   
                    if (rogueDays == 0 && counter > DateTime.DaysInMonth(_calendarSolarDate.Year, _calendarSolarDate.Month))
                    {
                        
                        DateTime dtValue = _calendarSolarDate.AddMonths(1);
                        dtValue = new DateTime(dtValue.Year, dtValue.Month, counter2);
                        if ((int)dtValue.DayOfWeek == x)
                        {
                            
                            xFillDayBox(g, rectBox, dtValue);
                            objLNDate = new clsLunarCalendar(dtValue);
                            _lstDayinView.Add(new DayInView(rectBox, objLNDate));
                            counter2++;
                        }         
                    }
                    g.DrawRectangle(Pens.DarkGray, rectBox);
                    xStart += cellWidth;
                }
                xStart = MarginSize;
                yStart += cellHeight;
            }

            g.DrawString(lblLunarDate.Text, _daysFont, Brushes.RoyalBlue, MarginSize, yStart + 5);
            _events.Sort(new EventComparer());

            for (int i = 1; i <= DateTime.DaysInMonth(_calendarSolarDate.Year, _calendarSolarDate.Month); i++)
            {
                int renderOffsetY = 0;

                foreach (IEvent evItem in _events)
                {
                    var dt = new DateTime(_calendarSolarDate.Year, _calendarSolarDate.Month, i, 23, 59, _calendarSolarDate.Second);
                    if (NeedsRendering(evItem, dt))
                    {
                        int alpha = 255;
                        if (!evItem.Enabled && _dimDisabledEvents) alpha = 64;

                        Color alphaColor = Color.FromArgb(alpha, evItem.EventColor.R, evItem.EventColor.G, evItem.EventColor.B);

                        int offsetY = renderOffsetY;
                        Region r = g.Clip;
                        Point pointDay = _calendarDays[i];
                        SizeF sz = g.MeasureString(evItem.EventText, evItem.EventFont);
                        int yy = pointDay.Y - 1;
                        int xx = ((cellWidth - (int)sz.Width) / 2) + pointDay.X;

                        if (sz.Width > cellWidth)
                            xx = pointDay.X;
                        if (renderOffsetY + sz.Height > cellHeight - 10)
                            continue;
                        g.Clip = new Region(new Rectangle(pointDay.X + 1, pointDay.Y + offsetY, cellWidth - 1, (int)sz.Height));
                        g.FillRectangle(new SolidBrush(alphaColor), pointDay.X + 1, pointDay.Y + offsetY, cellWidth - 1, sz.Height);
                        if (!evItem.Enabled && _showDashedBorderOnDisabledEvents)
                        {
                            var p = new Pen(new SolidBrush(Color.FromArgb(255, 0, 0, 0))) { DashStyle = DashStyle.Dash };
                            g.DrawRectangle(p, pointDay.X + 1, pointDay.Y + offsetY, cellWidth - 2, sz.Height - 1);
                        }
                        g.DrawString(evItem.EventText, evItem.EventFont, new SolidBrush(evItem.EventTextColor), xx, yy + offsetY);
                        g.Clip = r;

                        var ev = new CalendarEvent
                        {
                            EventArea =
                                new Rectangle(pointDay.X + 1, pointDay.Y + offsetY, cellWidth - 1,
                                              (int)sz.Height),
                            Event = evItem,
                            Date = dt
                        };

                        _calendarEvents.Add(ev);
                        renderOffsetY += (int)sz.Height + 1;
                    }
                }
            }
            _rectangles.Clear();

            g.Dispose();
            e.Graphics.DrawImage(bmp, 0, 0, ClientSize.Width, ClientSize.Height);
            bmp.Dispose();
        }

        private bool NeedsRendering(IEvent evnt, DateTime day)
        {
            if (!evnt.Enabled && !_showDisabledEvents)
                return false;

            DayOfWeek dw = evnt.Date.DayOfWeek;

            if (evnt.RecurringFrequency == RecurringFrequencies.Daily)
            {
                return DayForward(evnt, day);
            }
            if (evnt.RecurringFrequency == RecurringFrequencies.Weekly && day.DayOfWeek == dw)
            {
                return DayForward(evnt, day);
            }
            if (evnt.RecurringFrequency == RecurringFrequencies.EveryWeekend && (day.DayOfWeek == DayOfWeek.Saturday ||
                day.DayOfWeek == DayOfWeek.Sunday))
                return DayForward(evnt, day);
            if (evnt.RecurringFrequency == RecurringFrequencies.EveryMonWedFri && (day.DayOfWeek == DayOfWeek.Monday ||
                day.DayOfWeek == DayOfWeek.Wednesday || day.DayOfWeek == DayOfWeek.Friday))
            {
                return DayForward(evnt, day);
            }
            if (evnt.RecurringFrequency == RecurringFrequencies.EveryTueThurs && (day.DayOfWeek == DayOfWeek.Thursday ||
                day.DayOfWeek == DayOfWeek.Tuesday))
                return DayForward(evnt, day);
            if (evnt.RecurringFrequency == RecurringFrequencies.EveryWeekday && (day.DayOfWeek != DayOfWeek.Sunday &&
                day.DayOfWeek != DayOfWeek.Saturday))
                return DayForward(evnt, day);
            if (evnt.RecurringFrequency == RecurringFrequencies.Yearly && evnt.Date.Month == day.Month &&
                evnt.Date.Day == day.Day)
                return DayForward(evnt, day);
            if (evnt.RecurringFrequency == RecurringFrequencies.Monthly && evnt.Date.Day == day.Day)
                return DayForward(evnt, day);
            if (evnt.RecurringFrequency == RecurringFrequencies.Custom && evnt.CustomRecurringFunction != null)
            {
                if (evnt.CustomRecurringFunction(evnt, day))
                    return DayForward(evnt, day);
                return false;
            }

            if (evnt.RecurringFrequency == RecurringFrequencies.None && evnt.Date.Year == day.Year &&
                evnt.Date.Month == day.Month && evnt.Date.Day == day.Day)
                return DayForward(evnt, day);
            return false;
        }

        private int NumberOfWeeks(int year, int month)
        {
            return NumberOfWeeks(new DateTime(year, month, DateTime.DaysInMonth(year, month)));
        }

        private int NumberOfWeeks(DateTime date)
        {
            var beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate(date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }

        private void CalendarResize(object sender, EventArgs e)
        {
            if (_calendarView == CalendarViews.Day)
                ResizeScrollPanel();
        }

        private void _btnToday_Load(object sender, EventArgs e)
        {

        }

        private void dtpYear_ValueChanged(object sender, EventArgs e)
        {
            CalendarSolarDate = new DateTime(dtpYear.Value.Year, _calendarSolarDate.Month, _calendarSolarDate.Day);
        }

        private void cboMonth_SelectedValueChanged(object sender, EventArgs e)
        {
            int intNewMonth = clsCommon.ConvertToInt(cboMonth.SelectedItem.ToString().Replace("Tháng ", "")) - CalendarSolarDate.Month;
            CalendarSolarDate = CalendarSolarDate.AddMonths(intNewMonth);
        }
    }
}