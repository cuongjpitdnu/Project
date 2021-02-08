using EasyModbus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasyModbus
{
    class Program
    {
        private static AkbModbusClient akbModbusClient;

        static void Main(string[] args)
        {
            try
            {
                string ip = "192.168.0.1";
                int port = 502;
                akbModbusClient = new AkbModbusClient(ip, port);
                akbModbusClient.Connect();
                akbModbusClient.Start();
                akbModbusClient.Stop();
                akbModbusClient.Disconnect();
                Console.Write("Press any key to continue . . . ");
                Console.ReadKey(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
