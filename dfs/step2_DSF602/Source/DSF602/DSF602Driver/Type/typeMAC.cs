using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Type
{
    public class typeMAC : typeBase
    {
        private byte[] value;

        public typeMAC(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_STATUS, ratio, 6)
        {
            value = new byte[base.Len];
        }

        public override string ToString()
        {
            return value[0].ToString("X2") + ":" + value[1].ToString("X2") + ":" + value[2].ToString("X2") + ":" + value[3].ToString("X2") + ":" + value[4].ToString("X2") + ":" + value[5].ToString("X2");
        }

        public override bool ToValue(string strValue)
        {
            string[] array = strValue.Split(':');
            if (array.Length != base.Len)
            {
                return false;
            }
            try
            {
                int num = 0;
                string[] array2 = array;
                foreach (string s in array2)
                {
                    int result = 0;
                    if (int.TryParse(s, NumberStyles.HexNumber, null, out result))
                    {
                        value[num++] = (byte)result;
                    }
                    else
                    {
                        new Exception("convert error.");
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override byte[] ToFrame()
        {
            return value;
        }

        public override bool FrameTo(byte[] frame, int idx)
        {
            try
            {
                Array.Copy(frame, idx, value, 0, value.Length);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
