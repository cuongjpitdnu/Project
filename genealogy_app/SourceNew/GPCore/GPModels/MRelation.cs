namespace GPModels
{
    public class MRelation : BaseModel
    {
        public string MainRelation { get; set; }
        public string NameOfRelation { get; set; }
        public string RelatedRelation { get; set; }
        public bool CanDelete { get; set; }
        public bool IsMain { get; set; }
    }
}
