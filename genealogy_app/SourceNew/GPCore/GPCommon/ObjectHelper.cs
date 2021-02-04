using System;
using System.Collections;
using System.Collections.Generic;

namespace GPCommon
{
    public static class ObjectHelper
    {
        public static bool HasValue(this IList list) => list != null && list.Count > 0;

        public static bool HasValue(this IDictionary list) => list != null && list.Count > 0;

        public static bool HasValue(this object obj) => obj != null;

        public static void FreeMemory<T>(ref T obj) where T : class
        {
            if (obj.HasValue())
            {
                return;
            }

            if (obj is IDisposable)
            {
                ((IDisposable)obj).Dispose();
            }
            else
            {
                if (obj is IList)
                {
                    ((IList)obj).Clear();
                }

                if (obj is IDictionary)
                {
                    ((IDictionary)obj).Clear();
                }
                if (obj != null)
                {
                    GC.SuppressFinalize(obj);
                }
            }

            obj = null;
        }

        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultGet = default(TValue))
        {
            if (dictionary == null)
            {
                return defaultGet;
            }
            TValue rst = defaultGet;
            return dictionary.TryGetValue(key, out rst) ? rst : defaultGet;
        }

        public static string ToDayFormat(this int day)
        {
            if (day < 1) return "__";
            return day.ToString().PadLeft(2, '0');
        }
        public static string ToYearFormat(this int year)
        {
            if (year < 1200 || year > 2199) return "____";
            return year.ToString();
        }
    }
}