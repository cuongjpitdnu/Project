using System;
using System.Drawing;

namespace GPMain.Common.Dialog
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
