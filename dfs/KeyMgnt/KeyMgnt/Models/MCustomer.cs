using System;

namespace KeyMgnt.Models
{
    public class MCustomer
    {
        public long ID { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string CompanyMobile { get; set; }

        public string CusName { get; set; }

        public string CusEmail { get; set; }

        public string CusMobile { get; set; }

        public DateTime? CreateDate { get; set; }

        public string KeyCode { get; set; }

        public string MachineCode { get; set; }

        public string MacAddress { get; set; }

        public string UserId { get; set; }
    }
}
