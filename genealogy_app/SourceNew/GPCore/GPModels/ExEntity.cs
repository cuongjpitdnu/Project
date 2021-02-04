namespace GPModels
{
    public class ExTMember : TMember
    {
        public string IdRelate { get; set; }
        public string BirthdayShow { get; set; }
        public string BirthdayLunarShow { get; set; }
        public string DeadDaySunShow { get; set; }
        public string DeadDayLunarShow { get; set; }
        public string GenderShow { get; set; }
        public string Tel_1 { get; set; }
        public string Tel_2 { get; set; }
        public string Email_1 { get; set; }
        public string Email_2 { get; set; }
        public string Address { get; set; }
        public string RelTypeShow { get; set; }
        public string LevelInFamilyForShow { get; set; }
    }


    public class ExTMemberJob : TMemberJob
    {
        public string TimeShow { get; set; }
    }

    public class ExTMemberSchool : TMemberSchool
    {
        public string TimeShow { get; set; }
    }

    public class ExTMemberEvent : TMemberEvent
    {
        public string TimeShow { get; set; }
    }

    public class ExMRelation : MRelation
    {
        public string MainRelationNameShow { get; set; }
        public string RelatedRelationNameShow { get; set; }
    }

    public class ExTDocumentFile : TDocumentFile
    {
        public int STT { get; set; }
    }
}
