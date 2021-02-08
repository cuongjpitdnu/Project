using System;

namespace KeyMgnt.Models
{
    public class MDevice
    {
        public long ID { get; set; }

        public string MacAddress { get; set; }

        public string UserId { get; set; }

        public string DeviceName { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
