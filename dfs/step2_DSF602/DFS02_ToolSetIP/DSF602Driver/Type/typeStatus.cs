using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Type
{
    public class typeStatus : typeBase
    {
        private ushort value;

        public typeStatus(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_STATUS, ratio, 2)
        {
            value = (ushort)inivalue;
        }

        public override string ToString()
        {
            string text = Convert.ToString(value, 2);
            int num = 16 - text.Length;
            for (int i = 0; i < num; i++)
            {
                text = "0" + text;
            }
            return "b'" + text;
        }

        public override bool FrameTo(byte[] frame, int idx)
        {
            value = (ushort)((frame[idx] << 8) | frame[idx + 1]);
            return true;
        }
    }
}
