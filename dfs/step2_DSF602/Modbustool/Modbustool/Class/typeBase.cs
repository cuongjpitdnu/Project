// Modbustool.typeBase
using Modbustool;
using System;
using System.Globalization;
using System.Net;

// Modbustool.ParameterType
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
        bool flag = true;
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

public class typeBinary : typeBase
{
    private int value;

    public typeBinary(string title, int inivalue, int ratio)
        : base(title, ParameterType.TYPE_INTBYTE, ratio, 2)
    {
        value = inivalue;
    }

    public override string ToString()
    {
        string text = Convert.ToString(value, 2);
        int num = 8 - text.Length;
        for (int i = 0; i < num; i++)
        {
            text = "0" + text;
        }
        return "b'" + text;
    }

    public override bool ToValue(string strValue)
    {
        try
        {
            int num;
            if (strValue.IndexOf("b'") != 0)
            {
                num = ((strValue.IndexOf("0x") != 0) ? Convert.ToInt32(strValue) : Convert.ToInt32(strValue, 16));
            }
            else
            {
                string text = strValue.Substring(2, strValue.Length - 2);
                num = Convert.ToInt32(text, 2);
            }
            if (0 > num || num > 255)
            {
                return false;
            }
            bool result = true;
            value = num;
            return result;
        }
        catch
        {
            return false;
        }
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

public class typeBinaryShort : typeBase
{
    private ushort value;

    public typeBinaryShort(string title, int inivalue, int ratio)
        : base(title, ParameterType.TYPE_ERROR, ratio, 2)
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

    public override bool ToValue(string strValue)
    {
        try
        {
            int num;
            if (strValue.IndexOf("b'") != 0)
            {
                num = ((strValue.IndexOf("0x") != 0) ? Convert.ToInt32(strValue) : Convert.ToInt32(strValue, 16));
            }
            else
            {
                string text = strValue.Substring(2, strValue.Length - 2);
                num = Convert.ToInt32(text, 2);
            }
            if (0 > num || num > 65535)
            {
                return false;
            }
            bool result = true;
            value = (ushort)num;
            return result;
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
        value = BitConverter.ToUInt16(frame, idx);
        return true;
    }
}

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

public class typeExec : typeBase
{
    private ushort value;

    public typeExec(string title, int inivalue, int ratio)
        : base(title, ParameterType.TYPE_SHORT, ratio, 2)
    {
        value = (ushort)inivalue;
    }

    public override string ToString()
    {
        return "0x" + value.ToString("X4");
    }

    public override bool ToValue(string strValue)
    {
        try
        {
            int num;
            if (strValue.IndexOf("b'") != 0)
            {
                num = ((strValue.IndexOf("0x") != 0) ? Convert.ToInt32(strValue) : Convert.ToInt32(strValue, 16));
            }
            else
            {
                string text = strValue.Substring(2, strValue.Length - 2);
                num = Convert.ToInt32(text, 2);
            }
            if (0 > num || num > 65535)
            {
                return false;
            }
            bool result = true;
            value = (ushort)num;
            return result;
        }
        catch
        {
            return false;
        }
    }

    public override byte[] ToFrame()
    {
        byte[] bytes = BitConverter.GetBytes(value);
        byte[] array = new byte[bytes.Length];
        array[0] = bytes[1];
        array[1] = bytes[0];
        return array;
    }

    public override bool FrameTo(byte[] frame, int idx)
    {
        value = (ushort)BitConverter.ToInt16(frame, idx);
        return true;
    }
}

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

public class typeLong : typeBase
{
    private int value;

    public typeLong(string title, int inivalue, int ratio)
        : base(title, ParameterType.TYPE_LONG, ratio, 4)
    {
        value = inivalue;
    }

    public override string ToString()
    {
        string str = (value < 0) ? "-" : "";
        short num = (short)base.Ratio;
        switch (num)
        {
            case 1:
                return str + Math.Abs((long)value).ToString("D");
            case 10:
                str += Math.Abs(value / num).ToString();
                str += ".";
                return str + Math.Abs(value % num).ToString("D1");
            case 100:
                str += Math.Abs(value / num).ToString();
                str += ".";
                return str + Math.Abs(value % num).ToString("D2");
            case 1000:
                str += Math.Abs(value / num).ToString();
                str += ".";
                return str + Math.Abs(value % num).ToString("D3");
            case 10000:
                str += Math.Abs(value / num).ToString();
                str += ".";
                return str + Math.Abs(value % num).ToString("D4");
            default:
                return "---";
        }
    }

    public override bool ToValue(string strValue)
    {
        bool result;
        float result2;
        switch ((short)base.Ratio)
        {
            case 1:
                if (result = int.TryParse(strValue, out int result3))
                {
                    value = result3;
                }
                break;
            case 10:
                if (result = float.TryParse(strValue, out result2))
                {
                    result2 *= 10f;
                    result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                    value = (int)result2;
                }
                break;
            case 100:
                if (result = float.TryParse(strValue, out result2))
                {
                    result2 *= 100f;
                    result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                    value = (int)result2;
                }
                break;
            case 1000:
                if (result = float.TryParse(strValue, out result2))
                {
                    result2 *= 1000f;
                    result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                    value = (int)result2;
                }
                break;
            case 10000:
                if (result = float.TryParse(strValue, out result2))
                {
                    result2 *= 10000f;
                    result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                    value = (int)result2;
                }
                break;
            case 0:
                value = 0;
                result = true;
                break;
            default:
                result = false;
                break;
        }
        return result;
    }

    public override byte[] ToFrame()
    {
        byte[] array = new byte[4];
        byte[] bytes = BitConverter.GetBytes(value);
        array[0] = bytes[3];
        array[1] = bytes[2];
        array[2] = bytes[1];
        array[3] = bytes[0];
        return array;
    }

    public override bool FrameTo(byte[] frame, int idx)
    {
        value = ((frame[idx] << 24) | (frame[idx + 1] << 16) | (frame[idx + 2] << 8) | frame[idx + 3]);
        return true;
    }
}

public class typeMAC : typeBase
{
    private byte[] value;

    public typeMAC(string title, int inivalue, int ratio)
        : base(title, ParameterType.TYPE_STATUS, ratio, 6)
    {
        value = new byte[base.Len];
    }

    public override string ToString()
    {
        return value[0].ToString("X2") + ":" + value[1].ToString("X2") + ":" + value[2].ToString("X2") + ":" + value[3].ToString("X2") + ":" + value[4].ToString("X2") + ":" + value[5].ToString("X2");
    }

    public override bool ToValue(string strValue)
    {
        string[] array = strValue.Split(':');
        if (array.Length != base.Len)
        {
            return false;
        }
        try
        {
            int num = 0;
            string[] array2 = array;
            foreach (string s in array2)
            {
                int result = 0;
                if (int.TryParse(s, NumberStyles.HexNumber, null, out result))
                {
                    value[num++] = (byte)result;
                }
                else
                {
                    new Exception("convert error.");
                }
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    public override byte[] ToFrame()
    {
        return value;
    }

    public override bool FrameTo(byte[] frame, int idx)
    {
        try
        {
            Array.Copy(frame, idx, value, 0, value.Length);
            return true;
        }
        catch
        {
            return false;
        }
    }
}

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

public class typeParity : typeBase
{
    private int value;

    private string[] Items;

    public typeParity(string title, int inivalue, int ratio)
        : base(title, ParameterType.TYPE_SELECT, ratio, 2)
    {
        value = inivalue;
        Items = new string[3]
        {
            "None",
            "Odd",
            "Even"
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
        bool flag = true;
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
        bool flag = true;
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
        bool result = true;
        try
        {
            value = frame[idx + 1];
            return result;
        }
        catch
        {
            return false;
        }
    }
}

public class typeShort : typeBase
{
    private short value;

    public typeShort(string title, int inivalue, int ratio)
        : base(title, ParameterType.TYPE_SHORT, ratio, 2)
    {
        value = (short)inivalue;
    }

    public override string ToString()
    {
        string str = (value < 0) ? "-" : "";
        short num = (short)base.Ratio;
        switch (num)
        {
            case 1:
                return str + Math.Abs((int)value).ToString("D");
            case 10:
                str += Math.Abs(value / num).ToString();
                str += ".";
                return str + Math.Abs(value % num).ToString("D1");
            case 100:
                str += Math.Abs(value / num).ToString();
                str += ".";
                return str + Math.Abs(value % num).ToString("D2");
            case 1000:
                str += Math.Abs(value / num).ToString();
                str += ".";
                return str + Math.Abs(value % num).ToString("D3");
            case 10000:
                str += Math.Abs(value / num).ToString();
                str += ".";
                return str + Math.Abs(value % num).ToString("D4");
            default:
                return "---";
        }
    }

    public override bool ToValue(string strValue)
    {
        bool result;
        float result2;
        switch ((short)base.Ratio)
        {
            case 1:
                if (result = short.TryParse(strValue, out short result3))
                {
                    value = result3;
                }
                break;
            case 10:
                if (result = float.TryParse(strValue, out result2))
                {
                    result2 *= 10f;
                    result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                    value = (short)result2;
                }
                break;
            case 100:
                if (result = float.TryParse(strValue, out result2))
                {
                    result2 *= 100f;
                    result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                    value = (short)result2;
                }
                break;
            case 1000:
                if (result = float.TryParse(strValue, out result2))
                {
                    result2 *= 1000f;
                    result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                    value = (short)result2;
                }
                break;
            case 10000:
                if (result = float.TryParse(strValue, out result2))
                {
                    result2 *= 10000f;
                    result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                    value = (short)result2;
                }
                break;
            case 0:
                result = true;
                break;
            default:
                result = false;
                break;
        }
        return result;
    }

    public override byte[] ToFrame()
    {
        byte[] array = new byte[2];
        byte[] bytes = BitConverter.GetBytes(value);
        array[0] = bytes[1];
        array[1] = bytes[0];
        return array;
    }

    public override bool FrameTo(byte[] frame, int idx)
    {
        value = (short)((frame[idx] << 8) | frame[idx + 1]);
        return true;
    }
}


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
        bool flag = true;
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


public class typeUshort : typeBase
{
    private ushort value;

    public typeUshort(string title, int inivalue, int ratio)
        : base(title, ParameterType.TYPE_USHORT, ratio, 2)
    {
        value = (ushort)inivalue;
    }

    public override string ToString()
    {
        string str = (value < 0) ? "-" : "";
        short num = (short)base.Ratio;
        switch (num)
        {
            case 1:
                return str + Math.Abs(value).ToString("D");
            case 10:
                str += Math.Abs((int)value / (int)num).ToString();
                str += ".";
                return str + Math.Abs((int)value % (int)num).ToString("D1");
            case 100:
                str += Math.Abs((int)value / (int)num).ToString();
                str += ".";
                return str + Math.Abs((int)value % (int)num).ToString("D2");
            case 1000:
                str += Math.Abs((int)value / (int)num).ToString();
                str += ".";
                return str + Math.Abs((int)value % (int)num).ToString("D3");
            case 10000:
                str += Math.Abs((int)value / (int)num).ToString();
                str += ".";
                return str + Math.Abs((int)value % (int)num).ToString("D4");
            default:
                return "---";
        }
    }

    public override bool ToValue(string strValue)
    {
        bool result;
        float result2;
        switch ((short)base.Ratio)
        {
            case 1:
                if (result = ushort.TryParse(strValue, out ushort result3))
                {
                    value = result3;
                }
                break;
            case 10:
                if (result = float.TryParse(strValue, out result2))
                {
                    result2 *= 10f;
                    result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                    value = (ushort)result2;
                }
                break;
            case 100:
                if (result = float.TryParse(strValue, out result2))
                {
                    result2 *= 100f;
                    result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                    value = (ushort)result2;
                }
                break;
            case 1000:
                if (result = float.TryParse(strValue, out result2))
                {
                    result2 *= 1000f;
                    result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                    value = (ushort)result2;
                }
                break;
            case 10000:
                if (result = float.TryParse(strValue, out result2))
                {
                    result2 *= 10000f;
                    result2 = ((!(0f <= result2)) ? (result2 - 0.5f) : (result2 + 0.5f));
                    value = (ushort)result2;
                }
                break;
            case 0:
                result = true;
                break;
            default:
                result = false;
                break;
        }
        return result;
    }

    public override byte[] ToFrame()
    {
        byte[] array = new byte[2];
        byte[] bytes = BitConverter.GetBytes(value);
        array[0] = bytes[1];
        array[1] = bytes[0];
        return array;
    }

    public override bool FrameTo(byte[] frame, int idx)
    {
        value = (ushort)((frame[idx] << 8) | frame[idx + 1]);
        return true;
    }
}


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

