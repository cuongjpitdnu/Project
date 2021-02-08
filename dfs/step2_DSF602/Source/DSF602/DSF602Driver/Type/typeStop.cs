using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Type
{
    public class typeStop : typeBase
    {
        private int value;

        private string[] Items;

        public typeStop(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_SELECT, ratio, 2)
        {
            value = inivalue;
            Items = new string[3]
            {
            "Unknown",
            "1Stop",
            "2Stop"
            };
        }

        public override string[] SelectItem()
        {
            return Items;
        }

        public override string ToString()
        {
            if (value < Items.Length)
            {
                return Items[value];
            }
            return "Unknown";
        }

        public override bool ToValue(string strValue)
        {
            int num;
            if ((num = Array.IndexOf(Items, strValue)) >= 0)
            {
                value = num;
                return true;
            }
            return false;
        }

        public override byte[] ToFrame()
        {
            byte[] array = new byte[base.Len];
            byte[] bytes = BitConverter.GetBytes((ushort)value);
            array[0] = bytes[1];
            array[1] = bytes[0];
            return array;
        }

        public override bool FrameTo(byte[] frame, int idx)
        {
            value = frame[idx + 1];
            return true;
        }
    }
}
