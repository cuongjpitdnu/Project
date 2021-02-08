using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Type
{
    public class typeRange : typeBase
    {
        private int value;

        private string[] Items;

        public typeRange(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_SHORT, ratio, 2)
        {
            value = inivalue;
            Items = new string[5]
            {
            "Unknown",
            "レンジ1",
            "レンジ2",
            "レンジ3",
            "レンジ4"
            };
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

        public override bool FrameTo(byte[] frame, int idx)
        {
            try
            {
                value = frame[idx + 1];
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
