using System;
using System.Drawing;

namespace GP40Common
{
    internal class CalendarEvent
    {
        public Rectangle EventArea
        {
            get;
            set;
        }

        public IEvent Event
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }
    }
}
