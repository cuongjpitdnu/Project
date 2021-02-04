namespace GPModels
{
    public class MTypeName : BaseModel
    {
        public string TypeName { get; set; }
        public bool IsDefault { get; set; }
        public bool CanDelete { get; set; }
    }
}
