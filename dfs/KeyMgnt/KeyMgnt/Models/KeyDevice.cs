using System;

namespace KeyMgnt.Models
{
    public class KeyDevice
    {
        public string KeyCode { get; set; }

        public string MachineCode { get; set; }

        public string MacAddress { get; set; }

        public string UserId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
