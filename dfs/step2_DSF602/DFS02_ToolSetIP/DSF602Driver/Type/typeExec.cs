using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Type
{
    public class typeExec : typeBase
    {
        private ushort value;

        public typeExec(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_SHORT, ratio, 2)
        {
            value = (ushort)inivalue;
        }

        public override string ToString()
        {
            return "0x" + value.ToString("X4");
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
                if (0 > num || num > 65535)
                {
                    return false;
                }
                bool result = true;
                value = (ushort)num;
                return result;
            }
            catch
            {
                return false;
            }
        }

        public override byte[] ToFrame()
        {
            byte[] bytes = BitConverter.GetBytes(value);
            byte[] array = new byte[bytes.Length];
            array[0] = bytes[1];
            array[1] = bytes[0];
            return array;
        }

        public override bool FrameTo(byte[] frame, int idx)
        {
            value = (ushort)BitConverter.ToInt16(frame, idx);
            return true;
        }
    }
}
