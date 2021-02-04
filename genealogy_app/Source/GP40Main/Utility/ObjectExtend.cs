using System;
using System.Collections;

namespace GP40Main.Utility
{
    public static class ObjectExtend
    {
        public static bool IsNotHasValue(this IList list)
        {
            return !list.IsHasValue();
        }

        public static bool IsNotHasValue(this object obj)
        {
            return !obj.IsHasValue();
        }

        public static bool IsHasValue(this IList list)
        {
            return list != null && list.Count > 0;
        }

        public static bool IsHasValue(this object obj)
        {
            return obj != null;
        }

        public static void FreeMemory(this IDisposable obj)
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null;
            }
        }

        public static void FreeMemory(this IList list)
        {
            if (list != null)
            {
                list.Clear();
                list = null;
            }
        }

        public static void FreeMemory(this object obj)
        {
            GC.SuppressFinalize(obj);
            obj = null;
        }
    }
}
