// Modbustool.Utils
using Modbus.Utility;
using System;
using System.Net;

internal static class Utils
{
    public static byte[] FrameToByte(ushort[] pdu)
    {
        byte[] array = new byte[pdu.Length * 2];
        for (int i = 0; i < pdu.Length; i++)
        {
            byte[] bytes = BitConverter.GetBytes((ushort)IPAddress.HostToNetworkOrder((short)pdu[i]));
            Buffer.BlockCopy(bytes, 0, array, i * 2, bytes.Length);
        }
        return array;
    }

    public static ushort[] ByteToFrame(byte[] pdu)
    {
        return ModbusUtility.NetworkBytesToHostUInt16(pdu);
    }
}
