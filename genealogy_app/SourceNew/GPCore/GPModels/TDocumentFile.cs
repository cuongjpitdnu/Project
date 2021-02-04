namespace GPModels
{
    public class TDocumentFile: BaseModel
    {
        public string FileName { get; set; }
        public string FileIntroduce { get; set; }
        public string PathFile { get; set; }
        public VNDate CreateDate { get; set; }
        public TDocumentFile()
        {
            CreateDate = new VNDate();
        }
    }
}
