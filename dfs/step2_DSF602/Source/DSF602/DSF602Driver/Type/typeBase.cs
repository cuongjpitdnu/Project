using System;

namespace DSF602Driver.Type
{
    public enum ParameterType
    {
        TYPE_AD,
        TYPE_INTBYTE,
        TYPE_SHORT,
        TYPE_USHORT,
        TYPE_LONG,
        TYPE_ULONG,
        TYPE_FLOAT,
        TYPE_SELECT,
        TYPE_STATUS,
        TYPE_ERROR
    }

    public class typeBase
    {
        private int value;

        public ParameterType Type
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public int Len
        {
            get;
            set;
        }

        public int Ratio
        {
            get;
            set;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public virtual bool ToValue(string strValue)
        {
            return int.TryParse(strValue, out value);
        }

        public virtual byte[] ToFrame()
        {
            return BitConverter.GetBytes((ushort)value);
        }

        public virtual bool FrameTo(byte[] frame, int idx)
        {
            bool result = true;
            try
            {
                value = frame[idx];
                return result;
            }
            catch
            {
                return false;
            }
        }

        public virtual string[] SelectItem()
        {
            return null;
        }

        public typeBase(string title, ParameterType type, int ratio, int len)
        {
            Type = type;
            Title = title;
            Ratio = ratio;
            Len = len;
            value = 0;
        }

        public typeBase(string title, int inivalue, int ratio)
        {
            Type = ParameterType.TYPE_INTBYTE;
            Title = title;
            Len = 2;
            Ratio = ratio;
            value = inivalue;
        }
    }
}
