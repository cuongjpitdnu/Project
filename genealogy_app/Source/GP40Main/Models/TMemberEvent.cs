using GP40Main.Core;

namespace GP40Main.Models
{
    public class TMemberEvent : BaseModel
    {
        public string EventName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public VNDate StartDate { get; set; }
        public VNDate EndDate { get; set; }
        public TMemberEvent()
        {
            StartDate = new VNDate();
            EndDate = new VNDate();
        }
    }
}
