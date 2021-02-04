using GP40Main.Core;

namespace GP40Main.Models
{
    public class MUser : BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Role { get; set; }
        public string FullName { get; set; }
        public string FamilyId { get; set; }
    }
}