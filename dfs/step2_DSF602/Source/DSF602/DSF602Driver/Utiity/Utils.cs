using Modbus.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DSF602Driver.Utiity
{
    public class Utils
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

        internal static byte[] FrameToByte(Task<ushort[]> task)
        {
            throw new NotImplementedException();
        }
    }
}
