using GP40Main.Core;
using System;

namespace GP40Main.Models
{
    public class MFamilyInfo : BaseModel
    {
        public string FamilyName { get; set; }
        public string FamilyHometown { get; set; }
        public DateTime? FamilyAnniversary { get; set; }
        public int FamilyLevel { get; set; }
        public string FamilyAbout { get; set; }
        public string RootId { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
