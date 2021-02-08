using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Type
{
    public class typeBinary : typeBase
    {
        private int value;

        public typeBinary(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_INTBYTE, ratio, 2)
        {
            value = inivalue;
        }

        public override string ToString()
        {
            string text = Convert.ToString(value, 2);
            int num = 8 - text.Length;
            for (int i = 0; i < num; i++)
            {
                text = "0" + text;
            }
            return "b'" + text;
        }

        public override bool ToValue(string strValue)
        {
            try
            {
                int num;
                if (strValue.IndexOf("b'") != 0)
                {
                    num = ((strValue.IndexOf("0x") != 0) ? Convert.ToInt32(strValue) : Convert.ToInt32(strValue, 16));
                }
                else
                {
                    string text = strValue.Substring(2, strValue.Length - 2);
                    num = Convert.ToInt32(text, 2);
                }
                if (0 > num || num > 255)
                {
                    return false;
                }
                bool result = true;
                value = num;
                return result;
            }
            catch
            {
                return false;
            }
        }

        public override byte[] ToFrame()
        {
            byte[] array = new byte[base.Len];
            array[0] = 0;
            array[1] = (byte)value;
            return array;
        }

        public override bool FrameTo(byte[] frame, int idx)
        {
            value = frame[idx + 1];
            return true;
        }
    }
}
