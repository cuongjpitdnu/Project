namespace GPModels
{
    public class TMemberRelation : BaseModel
    {
        public string memberId { get; set; }
        public string relMemberId { get; set; }
        public string relType { get; set; }
        public int roleOrder { get; set; }
    }
}
