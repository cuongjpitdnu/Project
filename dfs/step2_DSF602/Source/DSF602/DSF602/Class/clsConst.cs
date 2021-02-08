using DSF602.Language;
using System;

namespace BaseCommon
{
    [Serializable]
    public class clsConst
    {
        public const string FILE_ZIP_NAME = "NewVersion.zip";
        public const string FILE_SETUP_UPDATE = "ToolUpdate.exe";

        public const string PREFIX_LBL_BLOCK = "lblBlock";
        public const string PREFIX_LBL_SENSOR = "lblSensor";
        public const string PREFIX_BTN_BLOCK = "btnBlock";

        public const int FREQUENCY_GET_DATE = 100;

        //Config default
        public const string IPADDRESS_DEFAULT = "192.168.0.1";
        public const int PORT_DEFAULT = 502;
        public const int ALARMVALUE_DEFAULT = 200;
        //public const int PERIOD_DEFAULT = 90;
        //public const int FAILLEVEL_DEFAULT = 700;
        //public const int SAMPLES_DEFAULT = 5;

        // Device Const
        public const int MAX_SENSORS = 8;
        public const string MAX_DEVICE_RETURN = "OF";
        public const int MAX_DEVICE_2K = 2000;
        public const int MAX_DEVICE_20K = 20000;
        public const int ACTIVE = 1;
        public const int NOT_ACTIVE = 0;

        // Fomat Time
        public const string cstrDateTimeFormatNoMiliSecond = "yyyy-MM-dd HH:mm:ss";
        public const string cstrDateTimeFormatNoMiliSecond2 = "yyyyMMddHHmmss";
        public const string cstrDateTimeFormatMiliSecond = "yyyy-MM-dd HH:mm:ss.fff";
        public const string cstrDateTimeFormatMiliSecond2 = "yyyyMMddHHmmssfff";
        public const string cstrDateTimeFomatShow = "yyyy, MMM, dd HH:mm:ss";
        public const string cstrDateFomatShow = "yyyy, MMM, dd";
        public const string cstrDateFomatShow1 = "dd/MMM/yyyy";

        // Measure mode: alarm test, walking test
        public static string ALARM_TEST = LanguageHelper.GetValueOf("ALARM_TEST");
        public const string WALKING_TEST = "Walking Test";

        // Export Report
        public const string REPORT_NAME_ALARM = "Report.xlsx";
        public const string REPORT_NAME_WALKING = "ReportWalking.xlsx";

        public const string KEY_CHAR_JOIN = "!";

        public enum emMeasureType
        {
            AlarmTest,
            WalkingTest,
        }

        public enum emMeasureResult
        {
            Pass,
            Fail,
            //Normal,
            //Alarm
        }

        public enum emMeasureStatus
        {
            Complete,
            Error,
        }

        public struct MeasureInfo
        {
            public DateTime? ReportDate;
            public DateTime? MeasureStart;
            public DateTime? MeasureEnd;
            public emMeasureType MeasureType;
            public string MeasureResult;
            public string DeviceName;
            public string UserName;
            public int MeasureId;
            public int AlarmValue;
            //public int FailLevel;
            //public int Period;
        }

        public struct MeasureDetailExport
        {
            public int No;
            public DateTime? Time;
            public int Value;
            public string Result;
        }

        public enum emBlockStatus
        {
            Inactive,
            Active,
        }

        public const int MeasureMode_Volt = 0;
        public const int MeasureMode_Ion = 1;
        public const int MeasureMode_Decay = 2;
        public const int IonAlarmValue = 35;

        public enum MeasureState
        {
            None,
            Positive,
            StopPositive,
            Negative,
            StopNegative,
            Ion,
            StopIon,
        }
    }
}
