using System.Configuration;

namespace BaseCommon.Utility
{
    public class ConfigHelper
    {
        public static string GetValueOf(string key, string defaultValue = null)
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(value) ? defaultValue : value;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
