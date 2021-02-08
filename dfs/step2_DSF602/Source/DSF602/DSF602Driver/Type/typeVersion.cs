using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Type
{
    public class typeVersion : typeBase
    {
        private int value;

        public typeVersion(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_USHORT, ratio, 2)
        {
            value = inivalue;
        }

        public override string ToString()
        {
            int num = (value >> 12) & 0xF;
            int num2 = (value >> 8) & 0xF;
            int num3 = (value >> 4) & 0xF;
            int num4 = value & 0xF;
            return num.ToString("X") + "." + num2.ToString("X") + "." + num3.ToString("X") + "." + num4.ToString("X");
        }

        public override bool FrameTo(byte[] frame, int idx)
        {
            value = (ushort)((frame[idx] << 8) | frame[idx + 1]);
            return true;
        }
    }
}
