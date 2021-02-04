using System;
using System.Collections.Generic;
using System.Text;

namespace GPModels
{
    public class TFamilyAlbum : BaseModel
    {
        public string AlbumName { get; set; }
        public string AlbumDescriptrion { get; set; }
        public string Thumbnail { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }

        public List<TAlbumImage> Images { get; set; }
        public TFamilyAlbum()
        {
            Images = new List<TAlbumImage>();
        }
    }
}
