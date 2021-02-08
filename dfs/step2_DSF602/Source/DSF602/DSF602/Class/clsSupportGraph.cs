using BaseCommon;
using BaseCommon.Threading;
using BaseCommon.Utility;
using DSF602.Model;
using DSF602Driver;
using DSF602Driver.Modbus;
using GraphLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSF602.Class
{
    public class BlockThreadProcess : IDisposable
    {
        public bool IsRunning { get; private set; }
        public Block BlockData { get; private set; }
        public List<SensorInfo> ListSensorChild { get; private set; }

        public event EventHandler<MeasureDetail> HandleReceiveMeasureDetail;
        public event EventHandler<DSF602Exception> ErrorsReceiveDataFromBlock;

        private Dictionary<int, int> mappingSensorMeasureId = new Dictionary<int, int>();
        private Dictionary<int, List<MeasureDetail>> mappingSensorMeasureData = new Dictionary<int, List<MeasureDetail>>();
        private Dictionary<int, QueueProcessorThread<List<MeasureDetail>>> mappingSensorProcessorInsert = new Dictionary<int, QueueProcessorThread<List<MeasureDetail>>>();
        private Dictionary<int, QueueProcessorThread<DSF602Exception>> mappingSensorProcessorException = new Dictionary<int, QueueProcessorThread<DSF602Exception>>();

        private IModbus Modbus { get; set; }
        private readonly object _syncObj = new object();
        private readonly object _syncObjAlarm = new object();

        public bool IsRunningModbus => Modbus != null && Modbus.IsRunning;
        private bool resetAlarm = false;

        private const int BATCH_PROCESS_SQL = 200;

        public BlockThreadProcess(Block block)
        {
            this.IsRunning = false;
            this.BlockData = block;
            this.ListSensorChild = AppManager.ListSensor.Where(i => i.OfBlock == this.BlockData.BlockId).ToList();


            //this.Modbus = new ModbusDemoData(block.Ip_Address, block.Port);
            this.Modbus = new ModbusTCP(block.Ip_Address, block.Port);
            this.Modbus.HandleWhenReceiveDataFromBlock += HandleWhenReceiveDataFromBlock;
            this.Modbus.HandleWhenReceiveDataEndFromBlock += HandleWhenReceiveDataEndFromBlock;
            if (ErrorsReceiveDataFromBlock != null)
            {
                this.Modbus.ErrorsReceiveDataFromBlock += ErrorsReceiveDataFromBlock;
            }
        }

        public void BlockThreadProcess_Discharge(int sensorid)
        {
            //this.Modbus.Discharge(sensorid);
        }

        public async Task StartAsync()
        {
            await Task.Run(() =>
            {
                this.Modbus.Start();
                this.InsertMesureForSensor();

                lock (_syncObj)
                {
                    this.IsRunning = true;
                }
            });
        }

        public void Start()
        {
            this.Modbus.Start();
            this.InsertMesureForSensor();

            lock (_syncObj)
            {
                this.IsRunning = true;
            }
        }

        public void End()
        {
            if (!this.IsRunning)
            {
                return;
            }

            lock (_syncObj)
            {
                this.IsRunning = false;
            }

            this.Modbus.End();
            this.ListSensorChild.Clear();
            this.mappingSensorMeasureId.Clear();
            this.mappingSensorMeasureData.Clear();
            this.mappingSensorProcessorInsert.Clear();
            this.mappingSensorProcessorException.Clear();
        }

        public void ResetAlarm()
        {
            lock (_syncObjAlarm)
            {
                resetAlarm = true;
            }
        }

        private void HandleWhenReceiveDataFromBlock(object sender, string[] data)
        {
            if (!this.IsRunning)
            {
                return;
            }

            var blockData = AppManager.ListBlock.FirstOrDefault(i => i.BlockId == this.BlockData.BlockId);
            var lstSensorDB = AppManager.ListSensor.Where(i => i.OfBlock == this.BlockData.BlockId && i.Active == (int)clsConst.emBlockStatus.Active).ToList();

            var maxLenght = 0;
            if (lstSensorDB != null && lstSensorDB.Count > 0)
            {
                maxLenght = lstSensorDB.Select(i => i.SensorName.Length).Max();
            }

            foreach (var sensor in this.ListSensorChild)
            {
                //if (sensor.MeasureType == clsConst.MeasureMode_Decay && sensor.IsMeasuring == false)
                //{
                //    return;
                //}

                var sensorDB = lstSensorDB.FirstOrDefault(i => i.SensorId == sensor.SensorId);
                if (sensorDB == null) continue;
                var tempPoint = data[sensor.Ordinal_Display - 1];
                if (data[sensor.Ordinal_Display - 1] == DSF602Const.ERR_DATA && ErrorsReceiveDataFromBlock != null)
                {
                    mappingSensorProcessorException[sensor.SensorId].Add(new DSF602Exception("SensorError NaN", DSF602ExceptionStatusCode.SensorError));
                    tempPoint = "0";
                    sensor.Active = clsConst.NOT_ACTIVE;
                }

                //var keyDbName = DBManagerChild.GetDBName(sensor.SensorId);
                //var randomTest = new Random();
                var pointGraphTemp = ConvertHelper.CnvNullToFloat(tempPoint, 0); // + randomTest.Next(-500, 500);
                var rateConvert = tempPoint.Replace("-", "").Length <= 7 ? 1000 : 10000;
                int pointGraph = 0;
                if (Math.Abs(pointGraphTemp * rateConvert) > int.MaxValue)
                {
                    if (rateConvert == 1000)
                    {
                        pointGraph = 2000;
                    }
                    else
                    {
                        pointGraph = 20000;
                    }
                    pointGraph = pointGraphTemp > 0 ? pointGraph : -pointGraph;
                }
                else
                {
                    pointGraph = (int)(pointGraphTemp * rateConvert);
                }
                //Console.WriteLine(pointGraph);
                int alarmVal = sensorDB.MeasureType == clsConst.MeasureMode_Ion ? clsConst.IonAlarmValue : sensorDB.Alarm_Value;
                var objMeasureDetail = new MeasureDetail
                {
                    SensorId = sensor.SensorId,
                    MeasureId = mappingSensorMeasureId[sensor.SensorId],
                    DBName = DBManagerChild.GetDBName(sensor.SensorId),
                    Samples_time = DateTime.Now,
                    Actual_Value = pointGraph,
                    Detail_Result = Math.Abs(pointGraph) >= alarmVal ? (int)clsConst.emMeasureResult.Fail : (int)clsConst.emMeasureResult.Pass,
                };

                if (sensor.MeasureType != clsConst.MeasureMode_Decay)
                {
                    if (objMeasureDetail.Detail_Result == (int)clsConst.emMeasureResult.Fail)
                    {
                        sensor.Result_Measure = (int)clsConst.emMeasureResult.Fail;
                        sensor.Alarm = true;
                    }
                }

                if (resetAlarm)
                {
                    sensor.Alarm = false;
                }

                //mappingSensorMeasureData[sensor.SensorId].Add(objMeasureDetail);


                if (sensor.MeasureType != clsConst.MeasureMode_Decay || sensor.MeasureState != clsConst.MeasureState.None)
                {
                    mappingSensorMeasureData[sensor.SensorId].Add(objMeasureDetail);
                }

                if (sensor.MeasureType != clsConst.MeasureMode_Decay && mappingSensorMeasureData[sensor.SensorId].Count == BATCH_PROCESS_SQL)
                {
                    mappingSensorProcessorInsert[sensor.SensorId].Add(mappingSensorMeasureData[sensor.SensorId].ToList());
                    mappingSensorMeasureData[sensor.SensorId].Clear();
                }

                if (sensor.MeasureType == clsConst.MeasureMode_Decay && sensor.MeasureState != clsConst.MeasureState.None)
                {
                    mappingSensorProcessorInsert[sensor.SensorId].Add(mappingSensorMeasureData[sensor.SensorId].ToList());
                    mappingSensorMeasureData[sensor.SensorId].Clear();
                }

                //var checkBatchProcessSql = sensor.MeasureType == clsConst.MeasureMode_Decay && sensor.MeasureState != clsConst.MeasureState.None;
                //checkBatchProcessSql = checkBatchProcessSql ? checkBatchProcessSql : mappingSensorMeasureData[sensor.SensorId].Count == BATCH_PROCESS_SQL;

                //if (checkBatchProcessSql)
                //{
                //    mappingSensorProcessorInsert[sensor.SensorId].Add(mappingSensorMeasureData[sensor.SensorId].ToList());
                //    mappingSensorMeasureData[sensor.SensorId].Clear();
                //}

                if (sensor.GraphData == null)
                {
                    sensor.GraphData = new DataSource();
                }

                int x = sensor.GraphData.Samples == null ? 0 : sensor.GraphData.Samples.Count;
                if (sensor.MeasureType != clsConst.MeasureMode_Decay || sensor.MeasureState != clsConst.MeasureState.None)
                {
                    sensor.GraphData.AddPoint(x, pointGraph, DateTime.Now, (int)sensor.MeasureState);
                }
                
                if (HandleReceiveMeasureDetail != null)
                {
                    sensorDB.GraphData = sensor.GraphData;
                    sensorDB.GraphName = sensorDB.SensorName.PadRight(maxLenght + 1, ' ');

                    //Task.Factory.StartNew(() =>
                    System.Threading.ThreadPool.QueueUserWorkItem(delegate
                    {
                        sensorDB.ActualValue = objMeasureDetail.Actual_Value;
                        //if (sensorDB.OfBlock == 1 && sensorDB.SensorId == 2)
                        //{
                        //    Console.WriteLine(sensorDB.Active);
                        //}
                        HandleReceiveMeasureDetail(sensorDB, objMeasureDetail);
                    });
                }
            }

            lock (_syncObjAlarm)
            {
                resetAlarm = false;
            }
        }

        private void HandleWhenReceiveDataEndFromBlock(object sender, EventArgs e)
        {
            foreach (var sensor in this.ListSensorChild)
            {
                mappingSensorProcessorInsert[sensor.SensorId].Add(mappingSensorMeasureData[sensor.SensorId]);
                mappingSensorProcessorInsert[sensor.SensorId].Stop(true);
                mappingSensorProcessorException[sensor.SensorId].Stop(true);
            }
        }

        private void InsertMesureForSensor()
        {
            foreach (var sensor in this.ListSensorChild)
            {
                var keyDbName = DBManagerChild.GetDBName(sensor.SensorId);
                mappingSensorMeasureId.Add(sensor.SensorId, AppManager.GetDBChildConnection(keyDbName).InsertMeasure(sensor));
                mappingSensorMeasureData.Add(sensor.SensorId, new List<MeasureDetail>());
                mappingSensorProcessorInsert.Add(sensor.SensorId, new QueueProcessorThread<List<MeasureDetail>>()
                {
                    Name = sensor.SensorId.ToString(),
                });
                mappingSensorProcessorInsert[sensor.SensorId].ProcessItem += InsertMesureDetailForSensor;
                mappingSensorProcessorInsert[sensor.SensorId].ProcessQueueEnd += UpdateMesureEndForSensor;
                mappingSensorProcessorInsert[sensor.SensorId].Start();

                mappingSensorProcessorException.Add(sensor.SensorId, new QueueProcessorThread<DSF602Exception>()
                {
                    Name = sensor.SensorId.ToString(),
                });

                mappingSensorProcessorException[sensor.SensorId].ProcessItem += ExceptionForSensor;
                mappingSensorProcessorException[sensor.SensorId].Start();
            }
        }

        private void UpdateMesureEndForSensor(object sender)
        {
            var obj = (QueueProcessorThread<List<MeasureDetail>>)sender;
            var sensorId = ConvertHelper.CnvNullToInt(obj.Name);

            if (sensorId > 0)
            {
                var measureResult = ListSensorChild.Where(i => i.SensorId == sensorId).Select(i => i.Result_Measure).FirstOrDefault() == (int)clsConst.emMeasureResult.Fail ? (int)clsConst.emMeasureResult.Fail : (int)clsConst.emMeasureResult.Pass;
                AppManager.GetDBChildConnection(DBManagerChild.GetDBName(sensorId)).UpdateEndTimeMeasure(mappingSensorMeasureId[sensorId], DateTime.Now, measureResult);
                Console.WriteLine("End sensor: " + DBManagerChild.GetDBName(sensorId));
            }
        }

        private void ExceptionForSensor(object sender, ProcessQueueItemEventArgs<DSF602Exception> exception)
        {
            if (exception == null || exception.ProcessItem == null)
            {
                return;
            }

            var obj = (QueueProcessorThread<DSF602Exception>)sender;
            var sensorId = ConvertHelper.CnvNullToInt(obj.Name);

            if (ErrorsReceiveDataFromBlock != null)
            {
                ErrorsReceiveDataFromBlock(sensorId, exception.ProcessItem);
            }
        }

        private void InsertMesureDetailForSensor(object sender, ProcessQueueItemEventArgs<List<MeasureDetail>> data)
        {
            if (data == null || data.ProcessItem == null || data.ProcessItem.Count == 0)
            {
                return;
            }

            //Console.WriteLine("sensor: " + data.ProcessItem.First().DBName);
            AppManager.GetDBChildConnection(data.ProcessItem.First().DBName).InsertMeasureDetailBulk(data.ProcessItem);
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
        // ~BlockThreadProcess()
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

        public void Charge(int sensorId, int type)
        {
            this.Modbus.Charge((ushort)sensorId, type);
        }

        public void StopCharge(int sensorId, int type)
        {
            this.Modbus.StopCharge((ushort)sensorId, type);
        }

        public void ConnectGround(int sensorId, bool allway = false)
        {
            this.Modbus.ConnectGround((ushort)sensorId, allway);
        }

        public void ResetAdj(int sensorId)
        {
            this.Modbus.ResetAdj((ushort)sensorId);
        }

        public void SelectSensor(int sensorId)
        {
            this.Modbus.SelectSensor((ushort)sensorId);
        }

        #endregion IDisposable Support
    }

    public class GraphData
    {
        public EventHandler<int> EventAddPoint;

        public DataSource Data { get; private set; }

        public int Length
        {
            get
            {
                return this.Data != null ? this.Data.Length : 0;
            }
        }

        public string NameGraph { get; private set; }
        public int LimitGraph { get; private set; }

        private GraphData(string nameGraph, int limitGraph)
        {
            // Info Graph
            this.NameGraph = nameGraph;
            this.LimitGraph = limitGraph;

            // Config Graph
            this.Data = new DataSource();
            this.Data.Name = nameGraph;
            this.Data.AutoScaleY = false;
            this.Data.SetDisplayRangeY(-501, 501);
            this.Data.SetGridDistanceY(100);
            this.Data.UpLimit = limitGraph;
            this.Data.DownLimit = -limitGraph;
            this.Data.GraphColor = Color.FromArgb(0, 255, 0);
            this.Data.OnRenderXAxisLabel += OnRenderXAxisLabel;
            this.AddPoint(0);
        }

        private string OnRenderXAxisLabel(DataSource src, int idx)
        {
            //return DateTime.Now.ToString("HH:mm:ss");

            return src.Samples[idx].dtPoint.ToString("HH:mm:ss");
        }

        public void AddPoint(int pointGraph)
        {
            int x = this.Data.Samples != null ? this.Data.Samples.Count : 0;
            this.Data.AddPoint(x, pointGraph, DateTime.Now);

            if (EventAddPoint != null)
            {
                EventAddPoint(this, pointGraph);
            }
        }

        public static GraphData CreateDefault(string nameGraph = "", int limitGraph = 0)
        {
            return new GraphData(nameGraph, limitGraph);
        }
    }

    public class clsSupportGraph
    {
        private delegate void RefreshGraphCallBack(PlotterDisplayEx pdeGraph);

        public static void GraphInit(PlotterDisplayEx pdeGraph, string graphName = "")
        {
            if (pdeGraph == null)
            {
                return;
            }

            pdeGraph.DoubleBuffering = true;
            pdeGraph.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;
            pdeGraph.Smoothing = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pdeGraph.BackgroundColorTop = Color.FromArgb(0, 15, 33);
            pdeGraph.BackgroundColorBot = Color.FromArgb(0, 15, 33);
            pdeGraph.SolidGridColor = Color.FromArgb(0, 128, 0);
            pdeGraph.DashedGridColor = Color.FromArgb(0, 128, 0);
            pdeGraph.StartingIndexOff = 0;
            pdeGraph.DataSources.Clear();
            pdeGraph.DataSources.Add(GraphData.CreateDefault(graphName).Data);
            pdeGraph.SetPlayPanelInvisible();
            pdeGraph.SetDisplayRangeX(0, 60);
            pdeGraph.SetGridDistanceX(10);
            pdeGraph.SetStartingIdx(0);

            foreach (var control in pdeGraph.Controls)
            {
                if (control is VScrollBar vsBar && vsBar != null)
                {
                    SetGridDistanceY(pdeGraph, vsBar.Value);
                    break;
                }
            }

            foreach (var control in pdeGraph.Controls)
            {
                if (control is HScrollBar hBar && hBar != null)
                {
                    SetGridDistanceX(pdeGraph, hBar.Value);
                    break;
                }
            }

            RefreshGraph(pdeGraph);
        }

        public static void SetGridDistanceY(PlotterDisplayEx pdeGraph, int intY, bool autoRefresh = true)
        {
            if (pdeGraph == null || pdeGraph.DataSources == null || pdeGraph.DataSources.Count == 0)
            {
                return;
            }

            if (intY > 0)
            {
                intY++;
                var graphDS = pdeGraph.DataSources[pdeGraph.DataSources.Count - 1];
                graphDS.SetDisplayRangeY(-intY, intY);
                graphDS.SetDisplayRangeY(-intY, intY);
                graphDS.SetGridDistanceY(intY > 5000 ? 1000 : 100);

                if (autoRefresh)
                {
                    RefreshGraph(pdeGraph);
                }
            }
        }

        public static void UpdateAlarm(PlotterDisplayEx pdeGraph, SensorInfo sensor, bool autoRefresh = true)
        {
            int limitGraph = sensor.Alarm_Value;
            if (pdeGraph == null || pdeGraph.DataSources == null || pdeGraph.DataSources.Count == 0)
            {
                return;
            }

            var limitSet = pdeGraph.DataSources[pdeGraph.DataSources.Count - 1].YD1 > limitGraph ? limitGraph : 0;
            pdeGraph.drawAlarmLine = pdeGraph.DataSources[pdeGraph.DataSources.Count - 1].YD1 > limitGraph;
            if (sensor.MeasureType == clsConst.MeasureMode_Decay)
            {
                pdeGraph.drawAlarmLine = false;
            }
            pdeGraph.DataSources[pdeGraph.DataSources.Count - 1].maxValue = limitSet;
            pdeGraph.DataSources[pdeGraph.DataSources.Count - 1].UpLimit = limitSet;
            pdeGraph.DataSources[pdeGraph.DataSources.Count - 1].DownLimit = -limitSet;

            if (autoRefresh)
            {
                RefreshGraph(pdeGraph);
            }
        }

        public static void UpdateGraphName(PlotterDisplayEx pdeGraph, string graphName, bool autoRefresh = true)
        {
            if (pdeGraph == null || pdeGraph.DataSources == null || pdeGraph.DataSources.Count == 0)
            {
                return;
            }

            pdeGraph.DataSources[pdeGraph.DataSources.Count - 1].Name = graphName;

            if (autoRefresh)
            {
                RefreshGraph(pdeGraph);
            }
        }

        public static void AddPoint(PlotterDisplayEx pdeGraph, int pointGraph, bool autoRefresh = true)
        {
            if (pdeGraph == null || pdeGraph.DataSources == null || pdeGraph.DataSources.Count == 0)
            {
                return;
            }

            var data = pdeGraph.DataSources[pdeGraph.DataSources.Count - 1];
            int x = data.Samples != null ? data.Samples.Count : 0;
            data.AddPoint(x, pointGraph, DateTime.Now);

            //if ((data.Length - pdeGraph.starting_idx) >= (int)pdeGraph.endX)
            //{
            //    float dx = (pdeGraph.endX - pdeGraph.startX);
            //    pdeGraph.SetStartingIdx((int)(data.Length - dx - (Math.Abs(pdeGraph.MoveMouse) > 0 ? pdeGraph.MoveMouse - 1 : 0)));
            //}

            if (autoRefresh)
            {
                RefreshGraph(pdeGraph);
            }
        }

        public static void ReplaceListPoint(PlotterDisplayEx pdeGraph, List<cPoint> lstPoint, bool autoRefresh = true)
        {
            if (pdeGraph == null || pdeGraph.DataSources == null || pdeGraph.DataSources.Count == 0)
            {
                return;
            }

            var data = pdeGraph.DataSources[pdeGraph.DataSources.Count - 1];

            if (data.Samples != null)
            {
                data.Samples.Clear();
            }
            else
            {
                data.Samples = new List<cPoint>();
            }

            data.Samples.AddRange(lstPoint);

            //if ((data.Length - pdeGraph.starting_idx) >= (int)pdeGraph.endX)
            //{
            //    float dx = (pdeGraph.endX - pdeGraph.startX);
            //    pdeGraph.SetStartingIdx((int)(data.Length - dx - (Math.Abs(pdeGraph.MoveMouse) > 0 ? pdeGraph.MoveMouse - 1 : 0)));
            //}

            if (autoRefresh)
            {
                RefreshGraph(pdeGraph);
            }
        }

        public static void RefreshGraph(PlotterDisplayEx pdeGraph)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            try
            {
                if (pdeGraph.InvokeRequired)
                {
                    RefreshGraphCallBack d = new RefreshGraphCallBack(RefreshGraph);
                    pdeGraph.BeginInvoke(d, new object[] { pdeGraph });
                }
                else
                {
                    if (pdeGraph.DataSources == null || pdeGraph.DataSources.Count == 0)
                    {
                        return;
                    }

                    var data = pdeGraph.DataSources[pdeGraph.DataSources.Count - 1];

                    if ((data.Length - pdeGraph.starting_idx) >= (int)pdeGraph.endX)
                    {
                        float dx = pdeGraph.endX - pdeGraph.startX;
                        pdeGraph.SetStartingIdx((int)(data.Length - dx - (Math.Abs(pdeGraph.MoveMouse) > 0 ? pdeGraph.MoveMouse - 1 : 0)));
                    }

                    pdeGraph.UpdateUI();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("RefreshGraph: " + ex.Message);
            }
        }

        public static void SetGridDistanceX(PlotterDisplayEx pdeGraph, int intX, bool autoRefresh = true)
        {
            if (pdeGraph == null || pdeGraph.DataSources == null || pdeGraph.DataSources.Count == 0)
            {
                return;
            }

            if (intX > 0)
            {
                intX++;
                pdeGraph.SetDisplayRangeX(0, intX);
                pdeGraph.SetGridDistanceX(10);

                if (autoRefresh)
                {
                    RefreshGraph(pdeGraph);
                }
            }
        }

        public static void AddVScrollBar(PlotterDisplayEx pdeGraph, int Maximun = 2000)
        {
            if (pdeGraph == null)
            {
                return;
            }

            VScrollBar vScrollBar = new VScrollBar()
            {
                Dock = DockStyle.Left,
                LargeChange = 100,
                Location = new Point(0, 0),
                Minimum = 500,
                Maximum = Maximun + 100,
                Name = "vScrollBar" + Guid.NewGuid().ToString(),
                Size = new Size(17, 150),
                TabIndex = 0,
            };

            vScrollBar.Scroll += new ScrollEventHandler((sender, e) =>
            {
                var bar = sender as VScrollBar;
                if (bar != null)
                {
                    SetGridDistanceY(pdeGraph, bar.Value);
                }
            });
            vScrollBar.Value = Maximun;
            pdeGraph.Controls.Add(vScrollBar);
        }

        public static void AddHScrollBar(PlotterDisplayEx pdeGraph, int Maximun = 600)
        {
            if (pdeGraph == null)
            {
                return;
            }

            HScrollBar hScrollBar = new HScrollBar()
            {
                Dock = DockStyle.Bottom,
                Location = new Point(0, 0),
                LargeChange = 30,
                Minimum = 60,
                Maximum = Maximun,
                Name = "hScrollBar" + Guid.NewGuid().ToString(),
                Size = new Size(150, 17),
                TabIndex = 0,
            };

            hScrollBar.Scroll += new ScrollEventHandler((sender, e) =>
            {
                var bar = sender as HScrollBar;
                if (bar != null)
                {
                    SetGridDistanceX(pdeGraph, bar.Value);
                }
            });

            pdeGraph.Controls.Add(hScrollBar);
        }

    }
}