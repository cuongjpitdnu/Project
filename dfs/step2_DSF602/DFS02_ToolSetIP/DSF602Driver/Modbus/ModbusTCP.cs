using DSF602Driver.Sensor;
using DSF602Driver.Type;
using DSF602Driver.Utiity;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace DSF602Driver.Modbus
{
    public class ModbusTCP : IModbus
    {
        private const int WAITTING_MILLISECONDS_TIME = 3000;

        private SensorBase _sensorInstance;
        private IPEndPoint _remoteEP;
        private Thread _threadReceiveDataFromBlock;
        private bool _isRunning;

        public event EventHandler<string[]> HandleWhenReceiveDataFromBlock;
        public event EventHandler<Exception> ErrorsReceiveDataFromBlock;
        public event EventHandler HandleWhenReceiveDataEndFromBlock;

        public bool IsRunning => _threadReceiveDataFromBlock != null;

        public ModbusTCP(string ipAddress, int portNo = 502)
        {
            try
            {
                _remoteEP = new IPEndPoint(IPAddress.Parse(ipAddress), portNo);
                _sensorInstance = new StaticElectricityObject();
                //_isRunning = true;
                //_threadReceiveDataFromBlock = new Thread(GetDataFromBlock);
                //_threadReceiveDataFromBlock.Start();
                //ResetSoft();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Start()
        {
            try
            {
                ResetSoft();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ResetSoft()
        {
            if (_isRunning)
            {
                _isRunning = false;
                _threadReceiveDataFromBlock.Join();

                Thread.Sleep(WAITTING_MILLISECONDS_TIME);
            }

            using (var tcpClient = new TcpClient())
            {
                tcpClient.Connect(_remoteEP);

                byte[] v = _sensorInstance.ExecMaps[1].Reg.ToFrame();

                using (var modbusMaster = ModbusIpMaster.CreateIp(tcpClient))
                {
                    modbusMaster.Transport.WriteTimeout = 300;
                    modbusMaster.WriteMultipleRegisters((byte)0, _sensorInstance.ExecMaps[1].ExecAddress, Utils.ByteToFrame(v));
                }

                Thread.Sleep(WAITTING_MILLISECONDS_TIME);
            }

            if (!_isRunning)
            {
                _isRunning = true;
                _threadReceiveDataFromBlock = new Thread(async () => await GetDataFromBlockAsync());
                //_threadReceiveDataFromBlock.IsBackground = true;
                _threadReceiveDataFromBlock.Start();
            }
        }

        private async Task GetDataFromBlockAsync()
        {   
            using (var tcpClient = new TcpClient())
            {
                tcpClient.Connect(_remoteEP);

                int len = _sensorInstance.SampleMaps.Sum(i => i.Len);

                using (var modbusMaster = ModbusIpMaster.CreateIp(tcpClient))
                {
                    //modbusMaster.Transport.ReadTimeout = 300;
                    while (_isRunning)
                    {   
                        int idx = 0;
                        byte[] frame = Utils.FrameToByte(await modbusMaster.ReadInputRegistersAsync((byte)0, (ushort)_sensorInstance.SamplingAddress, (ushort)(len / 2)));
                        List<string> lstData = new List<string>();

                        foreach (typeBase sampleMap in _sensorInstance.SampleMaps)
                        {

                            if (!sampleMap.Title.ToUpper().StartsWith("CH"))
                            {
                                continue;
                            }

                            sampleMap.FrameTo(frame, idx);
                            idx += sampleMap.Len;

                            lstData.Add(sampleMap.ToString());
                        }

                        if (HandleWhenReceiveDataFromBlock != null)
                        {
                            HandleWhenReceiveDataFromBlock(this, lstData.ToArray());
                        }

                        Thread.Sleep(95);
                    }

                    if (HandleWhenReceiveDataEndFromBlock != null)
                    {
                        HandleWhenReceiveDataEndFromBlock(this, new EventArgs());
                    }
                }
            }

            _threadReceiveDataFromBlock = null;
        }

        public void End()
        {
            if (_isRunning)
            {
                _isRunning = false;
            }

            if (_threadReceiveDataFromBlock == null)
            {
                return;
            }

            try
            {
                _threadReceiveDataFromBlock.Join();
            }
            catch
            {
            }
        }

        //public async Task<string> GetDataAsync()
        //{
        //    try
        //    {
        //        byte[] v = _sensorInstance.ExecMaps[1].Reg.ToFrame();
        //        await _modbusMaster.WriteMultipleRegistersAsync((byte)0, _sensorInstance.ExecMaps[1].ExecAddress, Utils.ByteToFrame(v));

        //        await Task.Factory.StartNew(() =>
        //        {
        //            _tcpClient.Close();
        //            _tcpClient = null;
        //            _tcpClient = new TcpClient(new IPEndPoint(IPAddress.Parse(_ipAddress), _portNo));
        //            _tcpClient.Connect(_remoteEP);

        //            _modbusMaster.Dispose();
        //            _modbusMaster = null;
        //            _modbusMaster = ModbusIpMaster.CreateIp(_tcpClient);
        //            _modbusMaster.Transport.ReadTimeout = 300;
        //            _modbusMaster.Transport.WriteTimeout = 300;
        //        });

        //        int len = _sensorInstance.SampleMaps.Sum(i => i.Len);
        //        int idx = 0;
        //        //byte[] frame = Utils.FrameToByte(await _modbusMaster.ReadInputRegistersAsync((byte)0, (ushort)_sensorInstance.SamplingAddress, (ushort)(len / 2)));
        //        byte[] frame = Utils.FrameToByte(await _modbusMaster.ReadWriteMultipleRegistersAsync((byte)0, (ushort)_sensorInstance.SamplingAddress, (ushort)(len / 2), _sensorInstance.ExecMaps[1].ExecAddress, Utils.ByteToFrame(v)));
        //        StringBuilder stringBuilder = new StringBuilder();

        //        stringBuilder.Append(DateTime.Now.ToString("HH:mm:ss"));

        //        foreach (typeBase sampleMap in _sensorInstance.SampleMaps)
        //        {
        //            sampleMap.FrameTo(frame, idx);
        //            idx += sampleMap.Len;
        //            stringBuilder.Append(",");
        //            stringBuilder.Append(sampleMap.ToString());
        //        }

        //        return stringBuilder.ToString();
        //    }
        //    catch (TimeoutException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (SlaveException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    
                    _isRunning = false;

                    if (_threadReceiveDataFromBlock != null)
                    {
                        _threadReceiveDataFromBlock.Join();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ModbusTCP()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
