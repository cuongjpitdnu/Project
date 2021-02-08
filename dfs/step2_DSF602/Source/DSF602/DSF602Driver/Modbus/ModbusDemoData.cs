using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DSF602Driver.Modbus
{
    public class ModbusDemoData : IModbus
    {
        private const int WAITTING_MILLISECONDS_TIME = 3000;

        //private SensorBase _sensorInstance;
        //private IPEndPoint _remoteEP;
        private Thread _threadReceiveDataFromBlock;
        private bool _isRunning;

        public event EventHandler<string[]> HandleWhenReceiveDataFromBlock;

        public event EventHandler<DSF602Exception> ErrorsReceiveDataFromBlock;

        public event EventHandler HandleWhenReceiveDataEndFromBlock;

        public bool IsRunning => _threadReceiveDataFromBlock != null;

        public ModbusDemoData(string ipAddress, int portNo = 502)
        {
        }

        public void Start()
        {
            try
            {
                ResetSoft();
            }
            catch (Exception ex)
            {
                throw new DSF602Exception(ex.Message, ex.InnerException, DSF602ExceptionStatusCode.Connect);
            }
        }

        public void ResetSoft()
        {
            if (_isRunning)
            {
                _isRunning = false;

                if (_threadReceiveDataFromBlock != null)
                {
                    _threadReceiveDataFromBlock.Join();
                    Thread.Sleep(WAITTING_MILLISECONDS_TIME);
                }
            }
            
            if (!_isRunning)
            {
                _isRunning = true;
                _threadReceiveDataFromBlock = new Thread(GetDataFromBlockAsync);
                _threadReceiveDataFromBlock.Start();
            }
        }

        //private async Task GetDataFromBlockAsync()
        private void GetDataFromBlockAsync()
        {
            try
            {

                var randomTest = new Random();
                var now = DateTime.Now;

                //modbusMaster.Transport.ReadTimeout = 300;
                while (_isRunning)
                {
                    try
                    {

                        float point1 = 0;
                        float point2 = 0;
                        float point3 = 0;
                        float point4 = 0;
                        float point5 = 0;
                        float point6 = 0;
                        float point7 = 0;
                        float point8 = 0;

                        var listDataRandom = new List<int>();

                        for (var i = 50; i <= 300; i += 50)
                        {
                            listDataRandom.Add(i);
                            listDataRandom.Add(-i);
                        }

                        //if (now.AddSeconds(3) < DateTime.Now)
                        //{
                            now = DateTime.Now;
                            point1 = (float)listDataRandom[randomTest.Next(0, listDataRandom.Count - 1)] / 1000;
                            point2 = (float)listDataRandom[randomTest.Next(0, listDataRandom.Count - 1)] / 1000;
                            point3 = (float)listDataRandom[randomTest.Next(0, listDataRandom.Count - 1)] / 1000;
                            point4 = (float)listDataRandom[randomTest.Next(0, listDataRandom.Count - 1)] / 1000;
                            point5 = (float)listDataRandom[randomTest.Next(0, listDataRandom.Count - 1)] / 1000;
                            point6 = (float)listDataRandom[randomTest.Next(0, listDataRandom.Count - 1)] / 1000;
                            point7 = (float)listDataRandom[randomTest.Next(0, listDataRandom.Count - 1)] / 1000;
                            point8 = (float)listDataRandom[randomTest.Next(0, listDataRandom.Count - 1)] / 1000;
                            //point1 = (float)randomTest.Next(100, 400) / 1000;
                            //point2 = (float)randomTest.Next(-400, 400) / 1000;
                            //point3 = (float)randomTest.Next(-400, 400) / 1000;
                            //point4 = (float)randomTest.Next(-400, 400) / 1000;
                            //point5 = (float)randomTest.Next(-400, 400) / 1000;
                            //point6 = (float)randomTest.Next(-600, 300) / 1000;
                            //point7 = (float)randomTest.Next(-400, 400) / 1000;
                            //point8 = (float)randomTest.Next(-100, 300) / 1000;
                        //}

                        List<string> lstData = new List<string>();
                        lstData.Add(point1.ToString().PadRight(7, '0'));
                        lstData.Add(point2.ToString().PadRight(7, '0'));
                        lstData.Add(point3.ToString().PadRight(7, '0'));
                        lstData.Add(point4.ToString().PadRight(7, '0'));
                        lstData.Add(point5.ToString().PadRight(7, '0'));
                        lstData.Add(point6.ToString().PadRight(7, '0'));
                        lstData.Add(point7.ToString().PadRight(7, '0'));
                        lstData.Add(point8.ToString().PadRight(7, '0'));

                        if (HandleWhenReceiveDataFromBlock != null)
                        {
                            HandleWhenReceiveDataFromBlock(this, lstData.ToArray());
                        }

                        Thread.Sleep(95);
                    }
                    catch (Exception ex2)
                    {
                        if (ErrorsReceiveDataFromBlock != null)
                        {
                            ErrorsReceiveDataFromBlock(this, new DSF602Exception(ex2.Message, ex2.InnerException, DSF602ExceptionStatusCode.ReceiveData));
                        }
                    }
                }

                if (HandleWhenReceiveDataEndFromBlock != null)
                {
                    HandleWhenReceiveDataEndFromBlock(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                if (ErrorsReceiveDataFromBlock != null)
                {
                    ErrorsReceiveDataFromBlock(this, new DSF602Exception(ex.Message, ex.InnerException, DSF602ExceptionStatusCode.Connect));
                }
            }
            finally
            {
                _isRunning = false;
                _threadReceiveDataFromBlock = null;
            }
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

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ModbusDemoData()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        public void Charge(ushort sensorId, int type)
        {
            throw new NotImplementedException();
        }

        public void StopCharge(ushort sensorId, int type)
        {
            throw new NotImplementedException();
        }

        public void ConnectGround(ushort sensorId)
        {
            throw new NotImplementedException();
        }

        public void ResetAdj(ushort sensorId)
        {
            throw new NotImplementedException();
        }

        public void SelectSensor(ushort sensorId)
        {
            throw new NotImplementedException();
        }

        public void ConnectGround(ushort sensorId, bool allway = false)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
