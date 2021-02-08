using System;

namespace DSF602Driver.Type
{
    public class typeBaud : typeBase
    {
        private int value;

        private string[] Items;

        public typeBaud(string title, int inivalue, int ratio)
            : base(title, ParameterType.TYPE_SELECT, ratio, 4)
        {
            value = inivalue;
            Items = new string[5]
            {
                "4800",
                "9600",
                "19200",
                "38400",
                "57600"
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
            byte[] array = (value == 0) ? BitConverter.GetBytes(4800u) : ((value == 1) ? BitConverter.GetBytes(9600u) : ((value == 2) ? BitConverter.GetBytes(19200u) : ((value != 3) ? BitConverter.GetBytes(57600u) : BitConverter.GetBytes(38400u))));
            byte[] array2 = new byte[array.Length];
            array2[0] = array[3];
            array2[1] = array[2];
            array2[2] = array[1];
            array2[3] = array[0];
            return array2;
        }

        public override bool FrameTo(byte[] frame, int idx)
        {
            switch ((frame[idx] << 24) | (frame[idx + 1] << 16) | (frame[idx + 2] << 8) | frame[idx + 3])
            {
                case 4800:
                    value = 0;
                    return true;
                case 9600:
                    value = 1;
                    return true;
                case 19200:
                    value = 2;
                    return true;
                case 38400:
                    value = 3;
                    return true;
                case 57600:
                    value = 4;
                    return true;
                default:
                    return false;
            }
        }
    }
}
