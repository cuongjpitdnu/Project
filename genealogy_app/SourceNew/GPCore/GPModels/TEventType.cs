using System;
using System.Collections.Generic;
using System.Text;

namespace GPModels
{
    public class TEventType : BaseModel
    {
        public string name { get; set; }
        public string code { get; set; }
        public int user_created { get; set; }
        public DateTime deleted_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

    }
}
