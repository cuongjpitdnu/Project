using BaseCommon;
using System.Configuration;
using System.Windows.Forms;

namespace NoMySql
{
    class clsConfig : Config
    {
        public const string FILE_NAME_RAW = "raw_";
        public const string FILE_NAME_DETAIL = "detail_";
        public const string FILE_NAME_LIMIT = "limit_";
        public const string FOLDER_NAME_TEMP = "temp_";
        
        public static string PathDataMeasure
        {
            get
            {
                return Application.StartupPath + @"\Data\";
            }
        }
        
        public static int MaxRecordCsv
        {
            get
            {
                return clsCommon.CnvNullToInt(ConfigurationManager.AppSettings["maxRecordCsv"], 100000);
            }
        }
    }
}
