using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Type
{
    public class typeLong : typeBase
    {
        private int value;

        public typeLong(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_LONG, ratio, 4)
        {
            value = inivalue;
        }

        public override string ToString()
        {
            string str = (value < 0) ? "-" : "";
            short num = (short)base.Ratio;
            switch (num)
            {
                case 1:
                    return str + Math.Abs((long)value).ToString("D");
                case 10:
                    str += Math.Abs(value / num).ToString();
                    str += ".";
                    return str + Math.Abs(value % num).ToString("D1");
                case 100:
                    str += Math.Abs(value / num).ToString();
                    str += ".";
                    return str + Math.Abs(value % num).ToString("D2");
                case 1000:
                    str += Math.Abs(value / num).ToString();
                    str += ".";
                    return str + Math.Abs(value % num).ToString("D3");
                case 10000:
                    str += Math.Abs(value / num).ToString();
                    str += ".";
                    return str + Math.Abs(value % num).ToString("D4");
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
                    int result3;
                    if (result = int.TryParse(strValue, out result3))
                    {
                        value = result3;
                    }
                    break;
                case 10:
                    if (result = float.TryParse(strValue, out result2))
                    {
                        result2 *= 10f;
                        result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                        value = (int)result2;
                    }
                    break;
                case 100:
                    if (result = float.TryParse(strValue, out result2))
                    {
                        result2 *= 100f;
                        result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                        value = (int)result2;
                    }
                    break;
                case 1000:
                    if (result = float.TryParse(strValue, out result2))
                    {
                        result2 *= 1000f;
                        result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                        value = (int)result2;
                    }
                    break;
                case 10000:
                    if (result = float.TryParse(strValue, out result2))
                    {
                        result2 *= 10000f;
                        result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                        value = (int)result2;
                    }
                    break;
                case 0:
                    value = 0;
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
            byte[] array = new byte[4];
            byte[] bytes = BitConverter.GetBytes(value);
            array[0] = bytes[3];
            array[1] = bytes[2];
            array[2] = bytes[1];
            array[3] = bytes[0];
            return array;
        }

        public override bool FrameTo(byte[] frame, int idx)
        {
            value = ((frame[idx] << 24) | (frame[idx + 1] << 16) | (frame[idx + 2] << 8) | frame[idx + 3]);
            return true;
        }
    }
}
