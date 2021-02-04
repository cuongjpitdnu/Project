namespace GPConst
{
    public enum EmGender : int
    {
        Male,
        FeMale,
        Unknown,
    }

    public class PageInfo
    {
        public string PageName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public struct PageSize
    {
        public static PageInfo A0 = new PageInfo() { PageName = "A0", Width = 1189, Height = 841 };
        public static PageInfo A1 = new PageInfo() { PageName = "A1", Width = 840, Height = 594 };
        public static PageInfo A2 = new PageInfo() { PageName = "A2", Width = 594, Height = 420 };
        public static PageInfo A3 = new PageInfo() { PageName = "A3", Width = 420, Height = 297 };
        public static PageInfo A4 = new PageInfo() { PageName = "A4", Width = 297, Height = 210 };
        public static PageInfo A5 = new PageInfo() { PageName = "A5", Width = 210, Height = 148 };
    }
    public struct Zoom
    {
        public static int FullPage = -1;
        public static int Zoom10 = 10;
        public static int Zoom25 = 25;
        public static int Zoom50 = 50;
        public static int Zoom75 = 75;
        public static int Zoom100 = 100;
        public static int Zoom150 = 150;
        public static int Zoom200 = 200;
        public static int Zoom400 = 400;
        public static int Zoom600 = 600;
        public static int Zoom800 = 800;
    }
    public class Relation
    {
        public const string PREFIX_DAD = "DAD";
        public const string PREFIX_MOM = "MOM";
        public const string PREFIX_HUSBAND = "HUS";
        public const string PREFIX_WIFE = "WIF";
        public const string PREFIX_CHILD = "CHI";
    }

    public class ReplyType
    {
        public const string NONE = "Không";
        public const string YEAR = "Năm";
        public const string MOTH = "Tháng";
        //public const string WEEK = "Tuần";
    }
    public class EventType
    {
        public const string BIRTH = "Sinh nhật";
        public const string DEATH = "Ngày giỗ";
        public const string OTHER = "Khác";
    }

    public enum EnumReplyType
    {
        ALL,
        NONE, YEAR, WEEK,
        MOTH
    }
    public enum EnumEventType
    {
        BIRTH,
        DEATH,
        OTHER
    }
    public enum EnumEventStatus
    {
        ALL, OPEN, CLOSE
    }

    public enum EnumCalendarType
    {
        MOON, SUN, ALL
    }
   
}