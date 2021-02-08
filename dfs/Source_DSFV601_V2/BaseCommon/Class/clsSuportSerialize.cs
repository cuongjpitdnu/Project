using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Script.Serialization;

namespace BaseCommon
{
    /// <summary>
    ///     Meno        : Suport Serialize/Deserialize object
    ///     Create By   : AKB Nguyen Thanh Tung 2017.10.31
    /// </summary>
    public class clsSuportSerialize
    {
        public static void BinSerialize(string path, object data)
        {
            Serialize(path, data, new BinaryFormatter());
        }

        public static T BinDeserialize<T>(string path)
        {
            return Deserialize<T>(path, new BinaryFormatter());
        }

        private static bool Serialize(string path, object data, IFormatter formatter)
        {
            try
            {
                using (var myStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {

                    formatter.Serialize(myStream, data);
                    return true;

                }
            }
            catch
            {
                return false;
            }
        }

        private static T Deserialize<T>(string path, IFormatter formatter)
        {
            if (!File.Exists(path)) return default(T);
            try
            {
                using (var myStream = new FileStream(path, FileMode.Open))
                {
                    T data = (T)formatter.Deserialize(myStream);
                    return data;
                }
            }
            catch
            {
                return default(T);
            }
        }

        public static string ObjectToJson(object obj)
        {
            try
            {
                return new JavaScriptSerializer().Serialize(obj);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static T JsonToObject<T>(string json)
        {
            //Note: Not suport return type dynamic

            try
            {
                return new JavaScriptSerializer().Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
