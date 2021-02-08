using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Type
{
    public class typeMeas : typeBase
    {
        private float value;

        public typeMeas(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_FLOAT, ratio, 4)
        {
            value = inivalue;
        }

        public override string ToString()
        {
            return value.ToString("F5");
        }

        public override bool FrameTo(byte[] frame, int idx)
        {
            uint num = (uint)((frame[idx] << 24) | (frame[idx + 1] << 16) | (frame[idx + 2] << 8) | frame[idx + 3]);
            value = BitConverter.ToSingle(BitConverter.GetBytes(num), 0);
            return true;
        }
    }
}
