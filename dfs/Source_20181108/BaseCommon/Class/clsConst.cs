using GraphLib;
using System;

namespace BaseCommon
{
    public class clsConst
    {
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

        public struct DeviceInfo
        {
            public int deviceId;
            public string deviceName;
            public int deviceType;
            public string ipAddress;
            public int port;
            public int AlarmValue;
            public int period;
            public int failLevel;
            public int samples;
            public bool active;
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
