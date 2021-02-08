using System.Configuration;
using System.Windows.Forms;

namespace BaseCommon
{
    public class Config
    {
        public static string PathReportTemplate
        {
            get
            {
                return Application.StartupPath + @"\Excel Template\";
            }
        }

        public static int RoundValue
        {
            get
            {
                return clsCommon.CnvNullToInt(ConfigurationManager.AppSettings["roundValue"], 1);
            }
        }

        public static int AlarmTime
        {
            get
            {
                return clsCommon.CnvNullToInt(ConfigurationManager.AppSettings["AlarmTime"]);
            }
        }

        public static int WalkingTime
        {
            get
            {
                return clsCommon.CnvNullToInt(ConfigurationManager.AppSettings["WalkingTime"]);
            }
        }
    }
}
