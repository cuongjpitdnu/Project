using System;
using System.Collections.Generic;
using System.Text;

namespace GPModels
{
    public class TAlbumImage : BaseModel
    {
        public string Name { get; set; }
        public string Descriptrion { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
    }
}
