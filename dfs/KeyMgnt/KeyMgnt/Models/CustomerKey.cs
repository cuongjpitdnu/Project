using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyMgnt.Models
{
    public class CustomerKey
    {
        public long CusId { get; set; }

        public string KeyCode { get; set; }    

        public string UserId { get; set; }

        public DateTime CreateDate { get; set; }

        public string MachineCode { get; set; }

        public string MacAddress { get; set; }

        public List<string> ListMacAddress { get; set; }
    }
}
