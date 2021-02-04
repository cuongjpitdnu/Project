using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP40Main.Models
{
    public class MemberContact
    {
        public string Address { get; set; }
        public string Tel_1 { get; set; }
        public string Tel_2 { get; set; }
        public string Email_1 { get; set; }
        public string Email_2 { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public Dictionary<string, string> SocialNetwork { get; set; } = new Dictionary<string, string>();
        public string Note { get; set; }
    }
}
