using System.Windows.Forms;
using System.Configuration;
using BaseCommon;

namespace WithMySql
{
    class clsConfig : Config
    {
        
        public const string MEASURE_NAME_FILE = "measure_";
        public const string MEASURE_DETAIL_NAME_FILE = "measuredetail_";
        public const string MEASURE_DETAIL_RAW_NAME_FILE = "measuredetailraw_";
        public const string MEASURE_DETAIL_LIMIT_NAME_FILE = "measuredetaillimit_";
        public const int MAX_RECORD_FILE_ERR = 10000;

        public enum emModeApp
        {
            Admin,
            User,
        }

        public static emModeApp ModeApp = emModeApp.Admin;
        public static int UserLoginId = 1;
        
        public static string PathDataErrors
        {
            get
            {
                return Application.StartupPath + ConfigurationManager.AppSettings["PathDataErrors"];
            }
        }

        public static int TimeDeleteMeasureDetailRaw
        {
            get
            {
                return clsCommon.CnvNullToInt(ConfigurationManager.AppSettings["timeDeleteMeasureDetailRaw"], 0);
            }
        }
    }
}
