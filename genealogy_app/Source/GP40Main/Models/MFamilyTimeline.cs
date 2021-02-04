using GP40Main.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP40Main.Models
{
    public class MFamilyTimeline : BaseModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Content { get; set; }
    }
}
