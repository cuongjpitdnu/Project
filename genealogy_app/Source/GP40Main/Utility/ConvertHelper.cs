using Newtonsoft.Json;
using System;
using System.Globalization;

namespace GP40Main.Utility
{
    /// <summary>
    /// Meno        : Support convert type data
    /// Create by   : AKB Nguyễn Thanh Tùng
    /// </summary>
    public static class ConvertHelper
    {
        public static T ToObject<T>(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson<T>(this T obj)
        {
            if (null == obj || obj.Equals(default(T)))
            {
                return string.Empty;
            }

            return JsonConvert.SerializeObject(obj);
        }

        public static string CnvNullToString(object obj, string defaultValue = "")
        {
            string rst = defaultValue;

            if (obj != null)
            {
                rst = obj.ToString();
            }

            return rst;
        }

        public static float CnvNullToFloat(object obj, float defaultValue = -1)
        {
            float rst = defaultValue;
            string temp = CnvNullToString(obj);

            if (!float.TryParse(temp, out rst))
            {
                rst = defaultValue;
            }

            return rst;
        }

        public static int CnvNullToInt(object obj, int defaultValue = -1)
        {
            int rst = defaultValue;
            string temp = CnvNullToString(obj);

            if (!int.TryParse(temp, out rst))
            {
                rst = defaultValue;
            }

            return rst;
        }

        public static long CnvNullToLong(object obj, long defaultValue = -1)
        {
            long rst = defaultValue;
            string temp = CnvNullToString(obj);

            if (!long.TryParse(temp, out rst))
            {
                rst = defaultValue;
            }

            return rst;
        }

        public static double CnvNullToDouble(object obj, int defaultValue = -1)
        {
            double rst = defaultValue;
            string temp = CnvNullToString(obj);

            if (!double.TryParse(temp, out rst))
            {
                rst = defaultValue;
            }

            return rst;
        }

        public static DateTime CnvStringToDateTime(string str, string fomat = "yyyy/MM/dd HH:mm:ss")
        {
            DateTime dtRst;
            DateTime.TryParseExact(str, fomat, null, DateTimeStyles.None, out dtRst);
            return dtRst;
        }

        public static DateTime? CnvStringToDateTimeNull(object obj, DateTime? defaultValue = null, string fomat = "")
        {
            if (obj == null || obj == DBNull.Value)
            {
                return defaultValue;
            }
            try
            {
                if (string.IsNullOrEmpty(fomat))
                {
                    return (DateTime)obj;
                }

                DateTime rst;

                if (!DateTime.TryParseExact(CnvNullToString(obj), fomat, null, DateTimeStyles.None, out rst))
                {
                    return defaultValue;
                }

                return rst;
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
