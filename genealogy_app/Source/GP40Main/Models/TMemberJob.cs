using GP40Main.Core;

namespace GP40Main.Models
{
    public class TMemberJob : BaseModel
    {
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string Position { get; set; }
        public string Job { get; set; }
        public VNDate StartDate { get; set; }
        public VNDate EndDate { get; set; }
        public TMemberJob()
        {
            StartDate = new VNDate();
            EndDate = new VNDate();
        }
    }
}
