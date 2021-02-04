namespace GPModels
{
    public class TMemberSchool : BaseModel
    {
        public string SchoolName { get; set; }
        public string Description { get; set; }
        public VNDate StartDate { get; set; }
        public VNDate EndDate { get; set; }
        public TMemberSchool()
        {
            StartDate = new VNDate();
            EndDate = new VNDate();
        }
    }
}
