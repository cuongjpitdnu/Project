using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Type
{
    public class typeUshort : typeBase
    {
        private ushort value;

        public typeUshort(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_USHORT, ratio, 2)
        {
            value = (ushort)inivalue;
        }

        public override string ToString()
        {
            string str = (value < 0) ? "-" : "";
            short num = (short)base.Ratio;
            switch (num)
            {
                case 1:
                    return str + Math.Abs(value).ToString("D");
                case 10:
                    str += Math.Abs((int)value / (int)num).ToString();
                    str += ".";
                    return str + Math.Abs((int)value % (int)num).ToString("D1");
                case 100:
                    str += Math.Abs((int)value / (int)num).ToString();
                    str += ".";
                    return str + Math.Abs((int)value % (int)num).ToString("D2");
                case 1000:
                    str += Math.Abs((int)value / (int)num).ToString();
                    str += ".";
                    return str + Math.Abs((int)value % (int)num).ToString("D3");
                case 10000:
                    str += Math.Abs((int)value / (int)num).ToString();
                    str += ".";
                    return str + Math.Abs((int)value % (int)num).ToString("D4");
                default:
                    return "---";
            }
        }

        public override bool ToValue(string strValue)
        {
            bool result;
            float result2;
            switch ((short)base.Ratio)
            {
                case 1:
                    ushort result3;
                    if (result = ushort.TryParse(strValue, out result3))
                    {
                        value = result3;
                    }
                    break;
                case 10:
                    if (result = float.TryParse(strValue, out result2))
                    {
                        result2 *= 10f;
                        result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                        value = (ushort)result2;
                    }
                    break;
                case 100:
                    if (result = float.TryParse(strValue, out result2))
                    {
                        result2 *= 100f;
                        result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                        value = (ushort)result2;
                    }
                    break;
                case 1000:
                    if (result = float.TryParse(strValue, out result2))
                    {
                        result2 *= 1000f;
                        result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                        value = (ushort)result2;
                    }
                    break;
                case 10000:
                    if (result = float.TryParse(strValue, out result2))
                    {
                        result2 *= 10000f;
                        result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                        value = (ushort)result2;
                    }
                    break;
                case 0:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        public override byte[] ToFrame()
        {
            byte[] array = new byte[2];
            byte[] bytes = BitConverter.GetBytes(value);
            array[0] = bytes[1];
            array[1] = bytes[0];
            return array;
        }

        public override bool FrameTo(byte[] frame, int idx)
        {
            value = (ushort)((frame[idx] << 8) | frame[idx + 1]);
            return true;
        }
    }
}
