using System;

namespace GPModels
{
    public class TFMemberMain
    {
        public int MEMBER_ID { get; set; }
        public string LAST_NAME { get; set; }
        public string MIDDLE_NAME { get; set; }
        public string FIRST_NAME { get; set; }
        public string ALIAS_NAME { get; set; }
        public DateTime? BIRTH_DAY { get; set; }
        public int GENDER { get; set; }
        public string HOMETOWN { get; set; }
        public string BIRTH_PLACE { get; set; }
        public string NATIONALITY { get; set; }
        public string RELIGION { get; set; }
        public int DECEASED { get; set; }
        public DateTime? DECEASED_DATE { get; set; }
        public string BURY_PLACE { get; set; }
        public string AVATAR_PATH { get; set; }
        public string FAMILY_ORDER { get; set; }
        public string REMARK { get; set; }
        public int BIR_DAY { get; set; }
        public int BIR_MON { get; set; }
        public int BIR_YEA { get; set; }
        public int DEA_DAY { get; set; }
        public int DEA_MON { get; set; }
        public int DEA_YEA { get; set; }
        public int DEA_DAY_SUN { get; set; }
        public int DEA_MON_SUN { get; set; }
        public int DEA_YEA_SUN { get; set; }
        public int BIR_DAY_LUNAR { get; set; }
        public int BIR_MON_LUNAR { get; set; }
        public int BIR_YEA_LUNAR { get; set; }
        public int CAREER_TYPE { get; set; }
        public int EDUCATION_TYPE { get; set; }
        public int FACT_TYPE { get; set; }
        public string CAREER { get; set; }
        public string EDUCATION { get; set; }
        public string FACT { get; set; }
        public int LEVEL { get; set; }
    }

    public class TFMemberContact
    {
        public int MEMBER_ID { get; set; }
        public string HOMETOWN { get; set; }
        public string HOME_ADD { get; set; }
        public string PHONENUM1 { get; set; }
        public string PHONENUM2 { get; set; }
        public string MAIL_ADD1 { get; set; }
        public string MAIL_ADD2 { get; set; }
        public string FAXNUM { get; set; }
        public string URL { get; set; }
        public string IMNICK { get; set; }
        public string REMARK { get; set; }
    }
    public class TFMemberCareer
    {
        public int MEMBER_ID { get; set; }
        public int CAREER_TYPE { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public string OCCUPATION { get; set; }
        public string POSITION { get; set; }
        public string OFFICE_NAME { get; set; }
        public string OFFICE_PLACE { get; set; }
        public string REMARK { get; set; }
        public int START_DAY { get; set; }
        public int START_MON { get; set; }
        public int START_YEA { get; set; }
        public int END_DAY { get; set; }
        public int END_MON { get; set; }
        public int END_YEA { get; set; }
    }
    public class TFMemberFact
    {
        public int MEMBER_ID { get; set; }
        public string FACT_PLACE { get; set; }
        public string FACT_NAME { get; set; }
        public DateTime? START_DATE { get; set; }
        public DateTime? END_DATE { get; set; }
        public string DESCRIPTION { get; set; }
        public int START_DAY { get; set; }
        public int START_MON { get; set; }
        public int START_YEA { get; set; }
        public int END_DAY { get; set; }
        public int END_MON { get; set; }
        public int END_YEA { get; set; }
    }

    public class TFMemberRelation
    {
        public int MEMBER_ID { get; set; }
        public int REL_FMEMBER_ID { get; set; }
        public int RELID { get; set; }
        public int ROLE_ORDER { get; set; }
    }

    public class MRoot
    {
        public int ROOT_ID { get; set; }
        public int MEMBER_ID { get; set; }
    }

    public class MFamilyInfoOld
    {
        public string FAMILY_NAME { get; set; }
        public string FAMILY_HOMETOWN { get; set; }
        public string FAMILY_ANNIVERSARY { get; set; }
    }
}
