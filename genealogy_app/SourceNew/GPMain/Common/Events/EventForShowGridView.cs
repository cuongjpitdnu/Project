using GPModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMain.Common.Events
{
    public class EventForShowGridView : TEvent
    {
        public Bitmap Image { get; set; }
        public bool Important { get; set; }
        public int STT { get; set; }
        public string EventName { get; set; }
        public string Desciption { get; set; }
        public string TimeEventStart { get; set; }
        public string TimeEventEnd { get; set; }
        public string ReplyType { get; set; }
        public string CalendarType { get; set; }
    }
}
