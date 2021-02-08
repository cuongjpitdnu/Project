using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Type
{
    public class typeIP : typeBase
    {
        private uint value;

        public typeIP(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_LONG, ratio, 4)
        {
            value = (uint)inivalue;
        }

        public override string ToString()
        {
            IPAddress iPAddress = new IPAddress(value);
            return iPAddress.ToString();
        }

        public override bool ToValue(string strValue)
        {
            try
            {
                IPAddress iPAddress = IPAddress.Parse(strValue);
                byte[] addressBytes = iPAddress.GetAddressBytes();
                value = BitConverter.ToUInt32(addressBytes, 0);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override byte[] ToFrame()
        {
            return BitConverter.GetBytes(value);
        }

        public override bool FrameTo(byte[] frame, int idx)
        {
            byte[] array = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                array[i] = frame[idx + i];
            }
            value = BitConverter.ToUInt32(array, 0);
            return true;
        }
    }
}
