using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;

namespace EasyModbus
{
    public class AkbModbusClient
    {
        private ModbusClient modbusClient;
        private string ip = "192.168.0.1";
        private int port = 502;

        private const int CH1_STATIC_ELEC = 0;
        private const int CH2_STATIC_ELEC = 2;
        private const int CH3_STATIC_ELEC = 4;
        private const int CH4_STATIC_ELEC = 6;
        private const int CH5_STATIC_ELEC = 8;
        private const int CH6_STATIC_ELEC = 10;
        private const int CH7_STATIC_ELEC = 12;
        private const int CH8_STATIC_ELEC = 14;

        private const int DEFAULT_QUANTITY = 2;

        public ConcurrentQueue<ElecInfo> dataCH1;
        public ConcurrentQueue<ElecInfo> dataCH2;
        public ConcurrentQueue<ElecInfo> dataCH3;
        public ConcurrentQueue<ElecInfo> dataCH4;
        public ConcurrentQueue<ElecInfo> dataCH5;
        public ConcurrentQueue<ElecInfo> dataCH6;
        public ConcurrentQueue<ElecInfo> dataCH7;
        public ConcurrentQueue<ElecInfo> dataCH8;

        private bool isMeasureRunning = false;
        private Thread processThread;
        private bool isStop = false;

        public AkbModbusClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
            dataCH1 = new ConcurrentQueue<ElecInfo>();
            dataCH2 = new ConcurrentQueue<ElecInfo>();
            dataCH3 = new ConcurrentQueue<ElecInfo>();
            dataCH4 = new ConcurrentQueue<ElecInfo>();
            dataCH5 = new ConcurrentQueue<ElecInfo>();
            dataCH6 = new ConcurrentQueue<ElecInfo>();
            dataCH7 = new ConcurrentQueue<ElecInfo>();
            dataCH8 = new ConcurrentQueue<ElecInfo>();
            processThread = new Thread(processData);
        }

        public bool Connect()
        {
            try
            {
                modbusClient = new ModbusClient(ip, port);
                modbusClient.Connect();
                modbusClient.akbReceiveDataChanged += OnReceiveDataChanged;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Disconnect()
        {
            try
            {
                isStop = true;
                modbusClient.Disconnect();
                modbusClient.akbReceiveDataChanged -= OnReceiveDataChanged;

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Start()
        {
            if (isMeasureRunning)
            {
                return;
            }

            isMeasureRunning = true;

            while (isMeasureRunning)
            {
                try
                {
                    readInputRegisters(CH1_STATIC_ELEC);
                    readInputRegisters(CH2_STATIC_ELEC);
                    readInputRegisters(CH3_STATIC_ELEC);
                    readInputRegisters(CH4_STATIC_ELEC);
                    readInputRegisters(CH5_STATIC_ELEC);
                    readInputRegisters(CH6_STATIC_ELEC);
                    readInputRegisters(CH7_STATIC_ELEC);
                    readInputRegisters(CH8_STATIC_ELEC);

                    Thread.Sleep(10);
                }
                catch (Exception)
                {
                }
            }
        }

        public void Stop()
        {
            isMeasureRunning = false;
        }

        public void CollectElec(int startAddress, int quantity = DEFAULT_QUANTITY)
        {
            try
            {
                readInputRegisters(startAddress, quantity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void processData()
        {
            while (!isStop)
            {
                writeTofile(ref dataCH1, @"G:\data1.csv");
                writeTofile(ref dataCH2, @"G:\data2.csv");
                writeTofile(ref dataCH3, @"G:\data3.csv");
                writeTofile(ref dataCH4, @"G:\data4.csv");
                writeTofile(ref dataCH5, @"G:\data5.csv");
                writeTofile(ref dataCH6, @"G:\data6.csv");
                writeTofile(ref dataCH7, @"G:\data7.csv");
                writeTofile(ref dataCH8, @"G:\data8.csv");
            }
        }

        private void writeTofile(ref ConcurrentQueue<ElecInfo> data, string filePath)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(string.Format("IP : {0}", ip));
                stringBuilder.AppendLine(string.Format("PORT : {0}", port));
                stringBuilder.AppendLine("------------------VALUES------------------");

                while (data.Count > 0)
                {
                    ElecInfo elecInfo;
                    data.TryDequeue(out elecInfo);

                    if (elecInfo != null)
                    {
                        stringBuilder.AppendLine(string.Format("  TIME: {0}, QUANTITY: {1} VALUES: {2} ", elecInfo.DateTime, elecInfo.Quantity, elecInfo.Value));
                    }
                }

                if (File.Exists(filePath))
                {
                    File.AppendAllText(filePath, stringBuilder.ToString());
                }
                else
                {
                    File.WriteAllText(filePath, stringBuilder.ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void readInputRegisters(int startingAddress, int quantity = DEFAULT_QUANTITY)
        {
            try
            {
                modbusClient.ReadInputRegisters(startingAddress, quantity);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void OnReceiveDataChanged(int startingAddress, int quantity, int[] response)
        {
            addRequestData(startingAddress, quantity, response, DateTime.Now);
        }

        private void addRequestData(int startingAddress, int quantity, int[] response, DateTime time)
        {
            ElecInfo data = new ElecInfo
            {
                DateTime = time,
                Data = response,
                StartAddress = startingAddress,
                Quantity = quantity,
                Value = ModbusClient.ConvertRegistersToFloat(response)
            };

            switch (startingAddress)
            {
                case CH1_STATIC_ELEC:
                    dataCH1.Enqueue(data);
                    break;

                case CH2_STATIC_ELEC:
                    dataCH2.Enqueue(data);
                    break;

                case CH3_STATIC_ELEC:
                    dataCH3.Enqueue(data);
                    break;

                case CH4_STATIC_ELEC:
                    dataCH4.Enqueue(data);
                    break;

                case CH5_STATIC_ELEC:
                    dataCH5.Enqueue(data);
                    break;

                case CH6_STATIC_ELEC:
                    dataCH6.Enqueue(data);
                    break;

                case CH7_STATIC_ELEC:
                    dataCH7.Enqueue(data);
                    break;

                case CH8_STATIC_ELEC:
                    dataCH8.Enqueue(data);
                    break;

                default:
                    break;
            }
        }

        public class ElecInfo
        {
            public DateTime DateTime { get; set; }
            public int[] Data { get; set; }
            public int StartAddress { get; set; }
            public float Value { get; set; }
            public int Quantity { get; set; }
        }

    }
}
