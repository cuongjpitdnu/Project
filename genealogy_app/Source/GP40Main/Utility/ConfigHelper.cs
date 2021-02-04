using System.Configuration;

namespace GP40Main.Utility
{
    /// <summary>
    /// Meno        : Support get value from app.config
    /// Create by   : AKB Nguyễn Thanh Tùng
    /// </summary>
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
