using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Type
{
    public class typeUptime : typeBase
    {
        private uint value;

        public typeUptime(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_ULONG, ratio, 4)
        {
            value = (uint)inivalue;
        }

        public override string ToString()
        {
            return Convert.ToString(value);
        }

        public override bool FrameTo(byte[] frame, int idx)
        {
            value = (uint)((frame[idx] << 24) | (frame[idx + 1] << 16) | (frame[idx + 2] << 8) | frame[idx + 3]);
            return true;
        }
    }
}
