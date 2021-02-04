using GPModels;
using System;
using System.Drawing;

namespace GPMain.Common.Dialog
{
    /// <summary>
    /// An event that defines a holiday
    /// </summary>
    public class HolidayEvent : IEvent
    {
        public int Rank { get; set; }
        public float EventLengthInHours { get; set; }
        public bool Enabled { get; set; }
        public CustomRecurringFrequenciesHandler CustomRecurringFunction { get; set; }
        public bool IgnoreTimeComponent { get; set; }
        public bool ReadOnlyEvent { get; set; }
        public DateTime Date { get; set; }
        public Color EventColor { get; set; }
        public Font EventFont { get; set; }
        public string EventText { get; set; }
        public Color EventTextColor { get; set; }
        public RecurringFrequencies RecurringFrequency { get; set; }
        public bool TooltipEnabled { get; set; }
        public bool ThisDayForwardOnly { get; set; }
        /// <summary>
        /// HolidayEvent Constructor
        /// </summary>
        public HolidayEvent()
        {
            EventColor = Color.FromArgb(80, 170, 255);
            EventFont = new Font("Arial", 8, FontStyle.Bold);
            EventTextColor = Color.FromArgb(255, 255, 255);
            Rank = 1;
            EventLengthInHours = 24;
            ReadOnlyEvent = true;
            Enabled = true;
            IgnoreTimeComponent = true;
            TooltipEnabled = true;
            ThisDayForwardOnly = false;
            RecurringFrequency = RecurringFrequencies.None;
        }

        public IEvent Clone()
        {
            return new HolidayEvent
            {
                CustomRecurringFunction = CustomRecurringFunction,
                Date = Date,
                Enabled = Enabled,
                EventColor = EventColor,
                EventFont = EventFont,
                EventText = EventText,
                EventTextColor = EventTextColor,
                IgnoreTimeComponent = IgnoreTimeComponent,
                Rank = Rank,
                ReadOnlyEvent = ReadOnlyEvent,
                RecurringFrequency = RecurringFrequency,
                ThisDayForwardOnly = ThisDayForwardOnly,
                EventLengthInHours = EventLengthInHours,
                TooltipEnabled = TooltipEnabled
            };
        }

        public TEvent THolidayEvent
        {
            get
            {
                ColorConverter colorConverter = new ColorConverter();
                return new TEvent()
                {
                    //s_date = Date.ToString("dd/MM/yyyy"),
                    //Enabled = Enabled,
                    //EventColor = colorConverter.ConvertToString(EventColor),
                    //EventText = EventText,
                    //EventTextColor = colorConverter.ConvertToString(EventTextColor),
                    //IgnoreTimeComponent = IgnoreTimeComponent,
                    //Rank = Rank,
                    //RecurringFrequency = RecurringFrequency.ToString(),
                    //ThisDayForwardOnly = ThisDayForwardOnly,
                    //EventLengthInHours = EventLengthInHours,
                    //TooltipEnabled = TooltipEnabled

                };
            }
        }
        public bool FromTHolidayEvent(TEvent holidayEvent)
        {
            try
            {
                //    ColorConverter colorConverter = new ColorConverter();
                //    CustomRecurringFunction = CustomRecurringFunction;
                //    Date = DateTime.Parse(holidayEvent.Date);
                //    Enabled = holidayEvent.Enabled;
                //    EventColor = (Color)colorConverter.ConvertFromString(holidayEvent.EventColor);
                //    string[] sFont = holidayEvent.EventFont?.Split(',');
                //    if (sFont.Length == 2)
                //    {
                //        float fontSize = 8;
                //        float.TryParse(sFont[1].Replace("pt", "").Trim(), out fontSize);
                //        EventFont = new Font(new FontFamily(sFont[0]), fontSize);
                //    }
                //    EventText = EventText;
                //    EventTextColor = (Color)colorConverter.ConvertFromString(holidayEvent.EventTextColor); ;
                //    IgnoreTimeComponent = holidayEvent.IgnoreTimeComponent;
                //    Rank = holidayEvent.Rank;
                //    ReadOnlyEvent = ReadOnlyEvent;
                //    RecurringFrequencies recurringFrequency = RecurringFrequencies.None;
                //    Enum.TryParse<RecurringFrequencies>(holidayEvent.RecurringFrequency, out recurringFrequency);
                //    RecurringFrequency = recurringFrequency;
                //    ThisDayForwardOnly = ThisDayForwardOnly;
                //    EventLengthInHours = holidayEvent.EventLengthInHours;
                //    TooltipEnabled = holidayEvent.TooltipEnabled;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
