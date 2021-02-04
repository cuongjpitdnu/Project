using GP40Main.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP40Main.Models
{
    public class MTypeName : BaseModel
    {
        public string TypeName { get; set; }
        public bool IsDefault { get; set; }
        public bool CanDelete { get; set; }
    }
}
