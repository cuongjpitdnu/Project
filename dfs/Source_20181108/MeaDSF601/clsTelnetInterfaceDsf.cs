using System;
using System.Collections.Generic;

using System.Text;

using System.Net.Sockets;
using System.Globalization;
using System.Threading;

namespace MeaDSF601
{
    public delegate void ClientHandlePacketData(byte[] data, int bytesRead);
    public delegate void ClientHandleWhenHasErrors();

    /// <summary>
    /// This class helps organize the data required for
    /// reading and writing to a network stream
    /// </summary>
    public class NetworkBuffer
    {
        public byte[] WriteBuffer;
        public byte[] ReadBuffer;
        public int CurrentWriteByteCount;
    }

    class TelnetInterfaceDsf
    {
        public event ClientHandlePacketData OnDataReceived;
        public event ClientHandleWhenHasErrors ProcessErrors;

        private TcpClient tcpSocket;
        private NetworkStream clientStream;
        private NetworkBuffer buffer;
        private int writeBufferSize = 10240;
        private int readBufferSize = 10240;
        private int port;
        private bool started = false;

        int TimeOutMs = 20000;
        List<Alarm> listAlarm = new List<Alarm>();
        Alarm oldAlarm;


        private void InitBuffer()
        {
            buffer = new NetworkBuffer();
            buffer.WriteBuffer = new byte[writeBufferSize];
            buffer.ReadBuffer = new byte[readBufferSize];
            buffer.CurrentWriteByteCount = 0;
        }

        public TelnetInterfaceDsf()
        {
            InitBuffer();
        }


        public TelnetInterfaceDsf(string strIP, int intPort)
        {
            InitBuffer();
            ConnectToServer(strIP, intPort);
        }

        /// <summary>
        /// Initiates a TCP connection to a TCP server with a given address and port
        /// </summary>
        /// <param name="ipAddress">The IP address (IPV4) of the server</param>
        /// <param name="port">The port the server is listening on</param>
        public bool ConnectToServer(string ipAddress, int port)
        {
            try
            {
                if (IsConnected)
                {
                    Disconnect();
                }

                this.port = port;

                tcpSocket = new TcpClient(ipAddress, port);
                clientStream = tcpSocket.GetStream();
                Console.WriteLine("Connected to server, listening for packets");

                //Thread t = new Thread(new ThreadStart(ListenForPackets));
                //started = true;
                //t.Start();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Start()
        {
            if (IsConnected)
            {
                Thread t = new Thread(new ThreadStart(ListenForPackets));
                started = true;
                t.Start();
                return true;
            }
            
            return false;
        }

        public bool IsConnected
        {
            get
            {
                return tcpSocket != null ? tcpSocket.Connected : false;
            }
        }

        /// <summary>
        /// This method runs on its own thread, and is responsible for
        /// receiving data from the server and raising an event when data
        /// is received
        /// </summary>
        private void ListenForPackets()
        {
            int bytesRead;

            while (started)
            {
                bytesRead = 0;

                try
                {
                    //Blocks until a message is received from the server
                    bytesRead = clientStream.Read(buffer.ReadBuffer, 0, readBufferSize);
                }
                catch
                {
                    //A socket error has occurred
                    Console.WriteLine("A socket error has occurred with the client socket " + tcpSocket?.ToString());
                    xProcessErrors();   
                    break;
                }

                //The server has disconnected
                if (bytesRead == 0)
                {
                    xProcessErrors();
                    break;
                }

                if (OnDataReceived != null)
                {
                    //Send off the data for other classes to handle
                    OnDataReceived(buffer.ReadBuffer, bytesRead);
                }

                Thread.Sleep(15);
            }

            //started = false;
            Disconnect();
        }

        /// <summary>
        /// Sends the byte array data immediately to the server
        /// </summary>
        /// <param name="data"></param>
        public void SendImmediate(byte[] data)
        {
            AddToPacket(data);
            FlushData();
        }

        /// <summary>
        /// Adds data to the packet to be sent out, but does not send it across the network
        /// </summary>
        /// <param name="data">The data to be sent</param>
        public void AddToPacket(byte[] data)
        {
            if (buffer.CurrentWriteByteCount + data.Length > buffer.WriteBuffer.Length)
            {
                FlushData();
            }

            Array.ConstrainedCopy(data, 0, buffer.WriteBuffer, buffer.CurrentWriteByteCount, data.Length);
            buffer.CurrentWriteByteCount += data.Length;
        }

        /// <summary>
        /// Flushes all outgoing data to the server
        /// </summary>
        public void FlushData()
        {
            clientStream.Write(buffer.WriteBuffer, 0, buffer.CurrentWriteByteCount);
            clientStream.Flush();
            buffer.CurrentWriteByteCount = 0;
        }

        /// <summary>
        /// Disconnect from the server
        /// </summary>
        public void Disconnect()
        {
            started = false;
            Thread.Sleep(100);

            if (tcpSocket == null)
            {
                return;
            }

            Console.WriteLine("Disconnected from server");
           
            tcpSocket.Close();
            tcpSocket = null;

            if (clientStream != null)
            {
                clientStream.Dispose();
                clientStream = null;
            }
        }

        public Alarm Read()
        {
            if (!tcpSocket.Connected) return null;
            StringBuilder sb = new StringBuilder();
            ParseTelnet(sb);
            //if (sb.ToString().Trim().EndsWith("\""))
            {
                string resultStr = sb.ToString();//.Substring(1, sb.ToString().Count() - 2);
                sb.Clear();
                resultStr = resultStr.Replace(" ", String.Empty);
                // resultStr = resultStr.Replace(".", ",");
                double resultDouble = 0;
                int index = resultStr.IndexOf('.');
                bool is2kDevice = true;
                if (index == resultStr.Length - 3)
                {
                    is2kDevice = false;
                }
                try
                {
                    resultDouble = Double.Parse(resultStr, CultureInfo.InvariantCulture);
                    resultDouble *= 1000;
                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.ToString());
                    if (resultStr != null && resultStr.ToLower().Contains("off") && oldAlarm != null)
                    {
                        if (is2kDevice)
                        {
                            resultDouble = 2001;
                        }
                        else
                        {
                            resultDouble = 20001;
                        }
                        if (oldAlarm.Volt < 0)
                        {
                            resultDouble = -resultDouble;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }

                //NumberStyles style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
                //CultureInfo culture = CultureInfo.CreateSpecificCulture("es-ES");
                //if (!Double.TryParse(resultStr, style, culture, out resultDouble))
                //{
                //    if (resultStr != null && resultStr.Length > 0 && oldAlarm != null)
                //    {
                //        if (is2kDevice)
                //        {
                //            resultDouble = 2001;
                //        }
                //        else
                //        {
                //            resultDouble = 20001;
                //        }
                //        if (oldAlarm.Volt < 0)
                //        {
                //            resultDouble = -resultDouble;
                //        }
                //    }
                //    else
                //    {
                //        return null;
                //    }
                //} else
                //{
                //    resultDouble *= 1000;
                //}
                //Console.WriteLine(DateTime.Now.ToString() + '\t' + sb.ToString().Substring(1, sb.ToString().Count() - 2));
                Alarm alarm = new Alarm(0, DateTime.Now.Date.ToString("yyyy/MM/dd"), DateTime.Now.ToString("hh:mm:ss.fff"), "-" + Math.Abs(resultDouble), "+" + Math.Abs(resultDouble), resultDouble, is2kDevice);
                oldAlarm = new Alarm(0, DateTime.Now.Date.ToString("yyyy/MM/dd"), DateTime.Now.ToString("hh:mm:ss.fff"), "-" + Math.Abs(resultDouble), "+" + Math.Abs(resultDouble), resultDouble, is2kDevice);
                return alarm;

            }

            return null;
        }

        //public void disconnect()
        //{
        //    if (tcpSocket != null && tcpSocket.Connected)
        //    {
        //        tcpSocket.Close();
        //    }
        //}

        void ParseTelnet(StringBuilder sb)
        {
            int input = 0;
            GETDATA:
            if (tcpSocket.Available > 0)
            {
                byte[] b = new byte[tcpSocket.ReceiveBufferSize];
                var a = tcpSocket.GetStream().Read(b, 0, tcpSocket.ReceiveBufferSize);
                string result = System.Text.Encoding.UTF8.GetString(b);
                string[] temp = result.Split('"');
                sb.Append(temp[temp.Length - 2]);

                //input = tcpSocket.GetStream().ReadByte();
                //if (input != 34)
                //    goto GETDATA;
                ////sb.Append((char)input);
                //while (tcpSocket.Available > 0)
                //{
                //    sb.Append((char)input);
                //    input = tcpSocket.GetStream().ReadByte();
                //    if (input == 34)
                //    {
                //        sb.Append((char)input);
                //        break;
                //    }
                //}
            }
        }

        private void xProcessErrors()
        {
            if (!started)
            {
                return;
            }

            started = false;
            System.Threading.Tasks.Task.Factory.StartNew(() => {
                if (!started && ProcessErrors != null)
                {
                    ProcessErrors();
                }
            });
        }
    }
}
