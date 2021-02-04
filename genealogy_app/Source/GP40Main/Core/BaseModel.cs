using System;

namespace GP40Main.Core
{
    public class BaseModel
    {
        public string Id { get; set; }
        public DateTime? LastUpdate { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
