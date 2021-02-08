using GraphLib;
using System;
using static BaseCommon.clsConst;

namespace BaseCommon
{
    [Serializable]
    public class DeviceInfo
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public int AlarmValue { get; set; }
        public int Period { get; set; }
        public int FailLevel { get; set; }
        public int Samples { get; set; }
        public bool WalkingMode { get; set; }
        public bool Active { get; set; }
        public int OrdinalDisplay { get; set; }
        public bool IsFirstAlarm { get; set; } = false;
        public string MacAddress { get; set; }

        public int MeasureId { get; set; }

        public string PathFolderData { get; set; }

        public emMeasureType MeasureType { get; set; } = emMeasureType.AlarmTest;
    }

    public class Response
    {
        public int MeasureId { get; set; }
        public string PathFolderData { get; set; }
    }

    public class CommonObject
    {
        public int Key { get; set; }

        public int Value { get; set; }
    }

    public class MacIpPair
    {
        public string MacAddress { get; set; }
        public string IpAddress { get; set; }
    }

    public class clsConst
    {
        //Config default
        public const string IPADDRESS_DEFAULT = "192.168.0.1";
        public const int PORT_DEFAULT = 10001;
        public const int ALARMVALUE_DEFAULT = 500;
        public const int PERIOD_DEFAULT = 90;
        public const int FAILLEVEL_DEFAULT = 700;
        public const int SAMPLES_DEFAULT = 5;

        // Msg
        public const string MSG_ERR_PROCESS = "Has erros in process.";

        // Device Const
        public const string MAX_DEVICE_RETURN = "OF";
        public const int MAX_DEVICE_2K = 2000;
        public const int MAX_DEVICE_20K = 20000;

        // Fomat Time
        public const string cstrDateTimeFormatNoMiliSecond = "yyyy-MM-dd HH:mm:ss";
        public const string cstrDateTimeFormatMiliSecond = "yyyy-MM-dd HH:mm:ss.fff";
        public const string cstrDateTimeFormatNoMiliSecond2 = "yyyyMMddHHmmss";
        public const string cstrDateTimeFomatShow = "yyyy, MMM, dd HH:mm:ss";
        public const string cstrDateFomatShow = "yyyy, MMM, dd";

        // Measure mode: alarm test, walking test
        public const string ALARM_TEST_KEY = "A";
        public const string WALKING_TEST_KEY = "W";
        public const string ALARM_TEST = "Alarm Test";
        public const string WALKING_TEST = "Walking Test";

        // Export Report
        public const string REPORT_NAME_ALARM = "Report.xlsx";
        public const string REPORT_NAME_WALKING = "ReportWalking.xlsx";

        public const char KEY_CHAR_SPLIT_CLIENT = '!';
        public const char KEY_CHAR_SPLIT_SERVER = ',';
        public const string KEY_CHAR_JOIN = "!";

        public enum emDeviceType
        {
            None,
            Device1,
            Device2,
        }

        public enum emMeasureType
        {
            AlarmTest,
            WalkingTest,
        }

        public enum emMeasureResult
        {
            Pass,
            Fail,
            Normal,
            Alarm
        }

        public enum emMeasureStatus
        {
            Complete,
            Error,
        }

        public struct DataSample
        {
            public DateTime t;
            public string strSample;

            public int deviceId;
            public int measureId;
            public string actualValue;
            public string[] arrActualValue;
            public int actualMaxValue;
            public int actualMinValue;
            public int actualDelegate;
            public int result;
            public bool isRaw;
            public bool isFrist;
        }

        public struct MaxValue
        {
            public int key;
            public DataSample dtSample;
            public DateTime datetime;
            public double maxValue;
            public double maxValueShow;
            public cPoint point;
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
            public int FailLevel;
            public int Period;
        }

        public struct MeasureDetail
        {
            public int No;
            public DateTime? Time;
            public int Value;
            public string Result;
        }
    }
}
