using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Type
{
    public class typeCh : typeBase
    {
        private int value;

        public int bit
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public typeCh(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_INTBYTE, ratio, 2)
        {
            value = inivalue;
        }

        public override string ToString()
        {
            return Convert.ToString(value, 2);
        }

        public override bool ToValue(string strValue)
        {
            return int.TryParse(strValue, out value);
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
