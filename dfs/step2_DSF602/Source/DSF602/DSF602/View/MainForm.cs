using BaseCommon;
using BaseCommon.Core;
using BaseCommon.Threading;
using BaseCommon.Utility;
using DSF602.Class;
using DSF602.Language;
using DSF602.Model;
using DSF602.View.GraphLayout;
using DSF602Driver;
using GraphLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BaseCommon.clsConst;

namespace DSF602.View
{
    public partial class MainForm : BaseForm
    {
        private struct DataCharge
        {
            public SensorInfo Sensor { get; set; }
            public MeasureState ChargeState { get; set; }
            public BlockThreadProcess BlockThread { get; set; }

            public bool IsAlarm { get; set; }
        }

        private const int PADDING_BOTTOM = 1;
        private static object _syncErrorMsg = new object();
        private Dictionary<string, Type> _dicMapLayout;
        public int BlockActiceId = -1;
        private bool pendingGraph;
        private Panel _plnMain;

        private List<BlockThreadProcess> blockThreadProcesses = new List<BlockThreadProcess>();
        private const int ERR_LIMIT_DISPLAY = 100;
        private List<string> _errList = new List<string>();
        public bool IsStop = false;
        private bool CloseCheckMsg = true;
        private MapLocation objMap = null;
        private AlarmReport objAlarm = null;
        private System.Windows.Forms.Timer tmRefresh;

        private QueueProcessorThread<DataCharge> processorCharge = new QueueProcessorThread<DataCharge>();
        private double maxIonVal = 0;
        
        public MainForm()
        {
            InitializeComponent();
            InitForm();
            AppManager.OnSensorSelected += OnDisplaySensor;

#if DEBUG
#else
            //this.Hide();

            //var keyEncripted = clsCommon.readKeyFromRegistry();
            //var rsValidate = clsCommon.ValidateMachineAndDevices(keyEncripted);

            //if (!rsValidate)
            //{
            //    using (var lis = new LicenseForm(this))
            //    {
            //        var rs = lis.ShowDialog();
            //        if (rs == DialogResult.Cancel)
            //        {
            //            this.Close();
            //            Environment.Exit(0);
            //        }
            //    }
            //}
#endif
            tmRefresh = new System.Windows.Forms.Timer();
            tmRefresh.Interval = 300;
            tmRefresh.Tick += TmRefresh_Tick;
            tmRefresh.Start();
        }

        private void TmRefresh_Tick(object sender, EventArgs e)
        {
            AppManager.OnSensorInfoRefresh?.Invoke();
        }

        private void OnDisplaySensor(object sender, EventArgs e)
        {
            SensorInfo info = sender as SensorInfo;
            ShowSensorSetting(info);
        }

        #region Event Form

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!checkLinkUpdate())
            {
                Point ptmp = btnReset.Location;
                btnUpdate.Visible = false;
                btnReset.Location = btnUpdate.Location;
                btnAbout.Location = ptmp;
            }

            if (BlockActiceId > 0)
            {
                var blockActive = AppManager.ListBlock.Where(i => i.BlockId == BlockActiceId).FirstOrDefault();
                lblBlockName.Text = blockActive.BlockName;
            }

            tabGraphMain.Focus();

            if (tablMap.Controls.Count > 0)
            {
                var objMap = tablMap.Controls[0] as MapLocation;

                if (objMap != null)
                {
                    objMap.CalculateLocation();
                }
            }

            var thread = new Thread(registrationBlockAsync);
            thread.IsBackground = true;
            thread.Start();
            //Task.Run(registrationBlockAsync);
        }

        public void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            StopProcess(200);

            if (IsStop)
            {
                var dataMsg = sender as MsgData;
                var jsonData = JsonConvert.SerializeObject(dataMsg);

                try
                {
                    using (Process myProcess = new Process())
                    {
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.FileName = Application.StartupPath + @"\" + clsConst.FILE_SETUP_UPDATE;
                        myProcess.StartInfo.Arguments = clsCommon.Base64Encode(jsonData);
                        myProcess.StartInfo.CreateNoWindow = false;
                        myProcess.Start();
                    }
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, LanguageHelper.GetValueOf("MSG_ERROR_TITLE"));
                }

            }
        }

        private void EventClickModeLayoutGraph(object sender, EventArgs e)
        {
            var objPicLayout = sender as PictureBox;

            if (objPicLayout == null || !_dicMapLayout.Keys.Contains(objPicLayout.Name))
            {
                return;
            }

            if (objPicLayout.BorderStyle == BorderStyle.FixedSingle)
            {
                return;
            }

            this.pendingGraph = true;
            this.SetModeWaiting();

            try
            {
                picLayout1.BorderStyle = BorderStyle.None;
                picLayout2.BorderStyle = BorderStyle.None;
                picLayout3.BorderStyle = BorderStyle.None;
                objPicLayout.BorderStyle = BorderStyle.FixedSingle;

                foreach (var obj in tabGraph.Controls)
                {
                    var temp = obj as GraphTypeBase;
                    temp?.Dispose();
                }

                tabGraph.SuspendLayout();
                tabGraph.Controls.Clear();

                var objLayout = Activator.CreateInstance(_dicMapLayout[objPicLayout.Name], new object[] { }) as GraphTypeBase;

                if (objLayout == null)
                {
                    return;
                }

                tabGraph.Controls.Add(objLayout);
                objLayout.Dock = DockStyle.Fill;
                objLayout.ShowChargeButton();
                tabGraph.ResumeLayout();
                tabGraph.PerformLayout();

                Application.DoEvents();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                this.pendingGraph = false;
                this.SetModeWaiting(false);
            }
        }

        public void btnDevice_Click(object sender, EventArgs e)
        {
            var pwd = AppCore.ShowDialog<PromptDialog>(LanguageHelper.GetValueOf("LOGIN_PASSWORD")) as string;
            if (AppManager.UserLogin.Password == pwd)
            {
                AppManager.ShowDialog<MapDevices>(sender, this);
            }
        }

        private void ShowSensorSetting(SensorInfo sensor)
        {
            if (!ComfirmMsg(LanguageHelper.GetValueOf("DEVICE_CONFIRM_MSG"), LanguageHelper.GetValueOf("DEVICE_CONFIRM_MSG_TITLE")))
            {
                return;
            }

            this.SetModeWaiting(true);

            try
            {
                StopProcess(100);

                foreach (var block in AppManager.ListBlock)
                {
                    this.UpdateChildControl(plnLocation, clsConst.PREFIX_BTN_BLOCK + block.BlockId, (c) =>
                    {
                        c.Enabled = block.Active == (int)clsConst.emBlockStatus.Active ? true : false;
                    });
                }

                picLayout1.BorderStyle = BorderStyle.None;
                EventClickModeLayoutGraph(picLayout1, new EventArgs());

                var rst = AppManager.ShowDialog<DeviceManagement>(sensor);

                AppManager.ListBlock = AppManager.GetConnection().GetListBlock();
                AppManager.ListSensor = AppManager.GetConnection().GetListSensor();

                var resultObjectChanged = rst as ObjChanged;
                if (resultObjectChanged != null)
                {
                    AppManager.OnSettingChanged?.Invoke(resultObjectChanged, null);
                    this.BeginInvoke((MethodInvoker)(() =>
                    {
                        UpdateInfoChanged(resultObjectChanged);
                        UpdateAlarmReport();
                    }));
                }

                var thread = new Thread(registrationBlockAsync);
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                this.ShowMsg(MessageBoxIcon.Error, ex.Message);
            }
            finally
            {
                this.SetModeWaiting(false);
            }
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            AppManager.ShowDialog<UserManagement>();
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            AppManager.ShowDialog<MeasureManagement>(null, this);
        }

        private void btnLanguage_Click(object sender, EventArgs e)
        {
            var rst = AppManager.ShowDialog<Language>();

            if (rst != null && (bool)rst == true)
            {
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    this.SetModeWaiting(true);
                    this.SetLanguageControl();
                    this.SetModeWaiting(false);
                }));
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            AppManager.ShowDialog<UpdateVersion>(null, this);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (var blockThread in blockThreadProcesses)
            {
                blockThread.ResetAlarm();
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            var rest = AppManager.ShowDialog<AboutForm>() as bool?;
            if ((rest ?? false))
            {
                CloseCheckMsg = false;
                this.Close();
                this.Dispose();
            }
        }

        private void btnBlock_Click(object sender, EventArgs e)
        {
            var btnBlock = sender as Button;
            int blockId = ConvertHelper.CnvNullToInt(btnBlock?.Tag);

            if (blockId < 1)
            {
                return;
            }

            this.pendingGraph = true;
            lblBlockName.Text = btnBlock.Text;
            BlockActiceId = blockId;
            this.SetModeWaiting();

            try
            {
                if (tabGraph.Controls.Count > 0)
                {
                    var objLayout = tabGraph.Controls[0] as GraphTypeBase;

                    if (objLayout != null)
                    {
                        objLayout.ResetNameGraphChild();
                    }
                }

                this.pendingGraph = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                this.SetModeWaiting(false);
            }
        }

        #endregion Event Form

        #region Private Funtion

        private void InitForm()
        {
            this.Icon = Properties.Resources.line_chart;
            this.WindowState = FormWindowState.Maximized;

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor, false);

            try
            {
                RenderButtonBlock();
                InitModeLayout();
                InitMapLocation();
                InitAlarmReport();

                timerUpdateGraph.Interval = 143; //7fpss
                timerCheckAlarm.Interval = 500;


                processorCharge.ProcessItem += eventProcessCharge;
                processorCharge.Start();
            }
            catch (Exception ex)
            {
                this.ShowMsg(MessageBoxIcon.Error, ex.Message);
            }
        }

        public volatile bool isDecayRuning = false;

        private void eventProcessCharge(object sender, ProcessQueueItemEventArgs<DataCharge> e)
        {
            var sensor = e.ProcessItem.Sensor;
            var state = e.ProcessItem.ChargeState;
            var item = e.ProcessItem.BlockThread;
            var sensorId = sensor.SensorId;
            var isAlarm = e.ProcessItem.IsAlarm;

            if (isAlarm)
            {
                sensor.Result_Measure = (int)clsConst.emMeasureResult.Fail;
                sensor.Alarm = true;
            }

            sensor.MeasureState = state;

            if (state == MeasureState.Positive)
            {
                isDecayRuning = true;
                var objLayout = tabGraph.Controls[0] as GraphTypeBase;
                if (objLayout == null) return;

                var graph = objLayout.GetPlotterDisplayExByIndex(sensor.Ordinal_Display - 1);

                if (graph.InvokeRequired)
                {
                    graph.Invoke(new MethodInvoker(() => clsSupportGraph.GraphInit(graph)));
                }
                else
                {
                    clsSupportGraph.GraphInit(graph);
                }

                sensor.ActualDecayPositiveTime = 0;
                sensor.ActualDecayNegativeTime = 0;
                sensor.ActualIBMax = 0;

                item.SelectSensor(sensorId);
                item.Charge(sensorId, 1);
            }
            else if (state == MeasureState.StopPositive)
            {
                item.StopCharge(sensorId, 1);
                Thread.Sleep(800);
            }
            else if (state == MeasureState.Negative)
            {
                item.Charge(sensorId, -1);
            }
            else if (state == MeasureState.StopNegative)
            {
                item.StopCharge(sensorId, -1);
            }
            else if (state == MeasureState.Ion)
            {
                item.ConnectGround(sensorId);
            }
            else if (state == MeasureState.StopIon)
            {
                item.ConnectGround(sensorId, true);
                var objLayout = tabGraph.Controls[0] as GraphTypeBase;
                if (objLayout == null) return;
                var lbl = objLayout.GetDisplaySensorByIndex(sensor.Ordinal_Display - 1);

                if (lbl != null)
                {
                    this.UpdateSafeControl(lbl, l =>
                    {
                        if (sensor.Active == (int)clsConst.emBlockStatus.Active)
                        {
                            sensor.ActualIBMax = maxIonVal;
                            ((DisplaySensor)l).DisplayDecayValue(maxIonVal, sensor.MeasureState);
                        }
                    });
                }

                isDecayRuning = false;
            }

            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss:ffff") + ": " + state.ToString());
        }

        private void InitAlarmReport()
        {
            //tabGraphMain.TabPages.RemoveAt(3);
            objAlarm = new AlarmReport();
            objAlarm.Dock = DockStyle.Fill;
            tabAlarmReport.Controls.Add(objAlarm);
        }

        private void UpdateAlarmReport()
        {
            AlarmReport frm = null;
            foreach (Control ctr in tabAlarmReport.Controls)
            {
                if (ctr.Name == nameof(AlarmReport))
                {
                    frm = ctr as AlarmReport;
                    break;
                }
            }
            if (frm != null)
            {
                frm.GetData();
            }
        }

        private void InitModeLayout()
        {
            _dicMapLayout = new Dictionary<string, Type>();
            _dicMapLayout.Add(picLayout1.Name, typeof(GraphType1));
            _dicMapLayout.Add(picLayout2.Name, typeof(GraphType2));
            _dicMapLayout.Add(picLayout3.Name, typeof(GraphType3));

            picLayout1.Click += EventClickModeLayoutGraph;
            picLayout2.Click += EventClickModeLayoutGraph;
            picLayout3.Click += EventClickModeLayoutGraph;

            picLayout1.BorderStyle = BorderStyle.None;
            EventClickModeLayoutGraph(picLayout1, null);
        }

        private void InitMapLocation()
        {
            objMap = new MapLocation();
            objMap.OnSelectedSensor += btnDevice_Click;
            //objMap.EventSendListBlockIdChanged += UpdateInfoChanged;
            objMap.Dock = DockStyle.Fill;
            tablMap.Controls.Add(objMap);
            _plnMain = objMap.plnMain;
        }

        private void ObjMap_OnDischarged(object data, EventArgs e)
        {
            var pr = new SensorInfo() { SensorId = 1, OfBlock = 1 };
            foreach (var blockThread in blockThreadProcesses)
            {
                var sensor = blockThread.ListSensorChild.FirstOrDefault(i => i.OfBlock == pr.OfBlock && i.SensorId == pr.SensorId);
                if (sensor != null)
                {
                    blockThread.BlockThreadProcess_Discharge(pr.SensorId);
                    break;
                }
            }
        }

        private void HandleReceiveMeasureDetail(object senser, MeasureDetail data)
        {
            var sensorData = senser as SensorInfo;

            if (sensorData == null)
            {
                return;
            }

            if (!AppManager.ActualListSensorIdActive.Contains(sensorData.SensorId))
            {
                AppManager.ActualListSensorIdActive.Add(sensorData.SensorId);
            }

            SensorInfo s = AppManager.ListSensor.FirstOrDefault(i => i.SensorId == sensorData.SensorId);
            s = sensorData;

            if (BlockActiceId != sensorData.OfBlock) return;

            if (this.pendingGraph)
            {
                return;
            }

            if (tabGraph.Controls.Count == 0)
            {
                return;
            }

            var objLayout = tabGraph.Controls[0] as GraphTypeBase;
            if (objLayout == null) return;

            var lbl = objLayout.GetDisplaySensorByIndex(sensorData.Ordinal_Display - 1);
            var graph = objLayout.GetPlotterDisplayExByIndex(sensorData.Ordinal_Display - 1);
            var graphName = string.Format("graph_{0}_{1}", sensorData.OfBlock, sensorData.SensorId);

            if (s.MeasureType == clsConst.MeasureMode_Decay)
            {
                if (s.MeasureState == MeasureState.None)
                {
                    graph.AlowRefesh = false;
                    return;
                }
                else 
                {
                    graph.AlowRefesh = true;
                }
            }

            if (lbl != null)
            {
                this.UpdateSafeControl(lbl, l =>
                {
                    //((DisplaySensor)l).SetSensorName(sensorData.GraphName, sensorData.Alarm);
                    if (sensorData.Active == (int)clsConst.emBlockStatus.Active)
                    {
                        ((DisplaySensor)l).DisplayValueSensor(data.Actual_Value);
                    }
                });
            }

            if (sensorData.Active == (int)clsConst.emBlockStatus.Inactive)
            {
                clsSupportGraph.UpdateGraphName(graph, "", false);
                clsSupportGraph.ReplaceListPoint(graph, new List<cPoint>() { new cPoint() { x = 0, y = 0 } }, false);
                graph.Name = graphName;
                return;
            }

            if (graph.Name == graphName)
            {
                //clsSupportGraph.UpdateGraphName(graph, "", false);
                clsSupportGraph.UpdateAlarm(graph, sensorData, false);
                clsSupportGraph.AddPoint(graph, data.Actual_Value, false);

                if (sensorData.MeasureType == clsConst.MeasureMode_Decay)
                {
                    var listDataPositivePoint = sensorData.GraphData.Samples.Where(i => i.PointFlag == (int)MeasureState.Positive);
                    var listDataNegativePoint = sensorData.GraphData.Samples.Where(i => i.PointFlag == (int)MeasureState.Negative);
                    var listDataIBPoint = sensorData.GraphData.Samples.Where(i => i.PointFlag == (int)MeasureState.Ion);

                    var maxY = listDataPositivePoint.Max<cPoint, float?>(i => i.y) ?? 0;
                    var maxX = listDataPositivePoint.Where(i => i.y == maxY)
                                                    .OrderByDescending(i => i.x)
                                                    .Select(i => i.x)
                                                    .FirstOrDefault();

                    if (maxX > 0 && maxY >= sensorData.DecayUpperValue)
                    {
                        var dataProcess = listDataPositivePoint.Where(i => i.x >= maxX).ToList();
                        sensorData.GraphData.PositivePoint = dataProcess.Where(i => i.y <= sensorData.DecayUpperValue && i.y >= sensorData.DecayLowerValue).ToList();
                        graph.DataSources[graph.DataSources.Count - 1].PositivePoint = sensorData.GraphData.PositivePoint;

                        if (sensorData.GraphData.PositivePoint.Count > 1)
                        {
                            var startDecay = sensorData.GraphData.PositivePoint.OrderBy(i => i.x).First();
                            var endDecay = sensorData.GraphData.PositivePoint.OrderBy(i => i.x).Last();

                            var decayTime = (endDecay.dtPoint - startDecay.dtPoint).TotalMilliseconds;

                            if (lbl != null && sensorData.MeasureState == MeasureState.Positive)
                            {
                                this.UpdateSafeControl(lbl, l =>
                                {
                                    //((DisplaySensor)l).SetSensorName(sensorData.GraphName, sensorData.Alarm);
                                    if (sensorData.Active == (int)clsConst.emBlockStatus.Active)
                                    {
                                        sensorData.ActualDecayPositiveTime = decayTime / 1000;
                                        ((DisplaySensor)l).DisplayDecayValue(sensorData.ActualDecayPositiveTime, sensorData.MeasureState);
                                    }
                                });
                            }

                            //Console.WriteLine(startDecay.dtPoint.ToString("HH:mm:ss:ffff") 
                            //        + " - " + endDecay.dtPoint.ToString("HH:mm:ss:ffff") 
                            //        + " - " + decayTime
                            //        + " - " + startDecay.y);
                        }
                    }

                    var minY = listDataNegativePoint.Min<cPoint, float?>(i => i.y) ?? 0;
                    var minX = listDataNegativePoint.Where(i => i.y == minY)
                                                    .OrderByDescending(i => i.x)
                                                    .Select(i => i.x)
                                                    .FirstOrDefault();

                    if (minX > 0 && Math.Abs(minY) >= sensorData.DecayUpperValue)
                    {
                        var dataProcess = listDataNegativePoint.Where(i => i.x >= minX).ToList();
                        sensorData.GraphData.NegativePoint = dataProcess.Where(i => Math.Abs(i.y) <= sensorData.DecayUpperValue && Math.Abs(i.y) >= sensorData.DecayLowerValue).ToList();
                        graph.DataSources[graph.DataSources.Count - 1].NegativePoint = sensorData.GraphData.NegativePoint;

                        if (sensorData.GraphData.NegativePoint.Count > 1)
                        {
                            var startDecay = sensorData.GraphData.NegativePoint.OrderBy(i => i.x).First();
                            var endDecay = sensorData.GraphData.NegativePoint.OrderBy(i => i.x).Last();

                            var decayTime = (endDecay.dtPoint - startDecay.dtPoint).TotalMilliseconds;

                            if (lbl != null && sensorData.MeasureState == MeasureState.Negative)
                            {
                                this.UpdateSafeControl(lbl, l =>
                                {
                                    //((DisplaySensor)l).SetSensorName(sensorData.GraphName, sensorData.Alarm);
                                    if (sensorData.Active == (int)clsConst.emBlockStatus.Active)
                                    {
                                        sensorData.ActualDecayNegativeTime = (decayTime / 1000);
                                        ((DisplaySensor)l).DisplayDecayValue(sensorData.ActualDecayNegativeTime, sensorData.MeasureState);
                                    }
                                });
                            }

                            //Console.WriteLine(startDecay.dtPoint.ToString("HH:mm:ss:ffff") 
                            //        + " - " + endDecay.dtPoint.ToString("HH:mm:ss:ffff") 
                            //        + " - " + decayTime
                            //        + " - " + startDecay.y);
                        }
                    }

                    if (listDataIBPoint != null && listDataIBPoint.Count() > 0)
                    {
                        graph.DataSources[graph.DataSources.Count - 1].IBPoint = listDataIBPoint.ToList();
                        var maxIBY = listDataIBPoint.Max<cPoint, float?>(i => Math.Abs(i.y)) ?? 0;
                        var maxIB = listDataIBPoint.Where(i => Math.Abs(i.y) == maxIBY)
                                                        .OrderByDescending(i => i.x)
                                                        .FirstOrDefault();
                        maxIonVal = maxIB.y;
                        //if (lbl != null && sensorData.MeasureState == MeasureState.Ion)
                        //{
                        //    this.UpdateSafeControl(lbl, l =>
                        //    {
                        //        //((DisplaySensor)l).SetSensorName(sensorData.GraphName, sensorData.Alarm);
                        //        if (sensorData.Active == (int)clsConst.emBlockStatus.Active)
                        //        {
                        //            ((DisplaySensor)l).DisplayDecayValue(maxIB.y.ToString("#,###"), sensorData.MeasureState);
                        //        }
                        //    });
                        //}

                        if (maxIonVal > sensorData.IonValueCheck && sensorData.MeasureState == MeasureState.Ion)
                        {
                            sensorData.Result_Measure = (int)clsConst.emMeasureResult.Fail;
                            sensorData.Alarm = true;
                        }

                    }

                }                
            }
            else
            {
                clsSupportGraph.ReplaceListPoint(graph, sensorData.GraphData.Samples, false);
                graph.DataSources[graph.DataSources.Count - 1].PositivePoint = sensorData.GraphData.PositivePoint;
                graph.Name = graphName;
            }
        }

        private void ErrorsReceiveDataFromBlock(object senser, DSF602Exception exception)
        {
            try
            {
                var flagUpdate = false;
                var sensorId = ConvertHelper.CnvNullToInt(senser);
                var sensorName = "";

                if (sensorId > 0)
                {
                    var sensorInfo = AppManager.ListSensor.FirstOrDefault(i => i.SensorId == sensorId);

                    if (sensorInfo != null)
                    {
                        sensorName = AppManager.ListBlock.FirstOrDefault(i => i.BlockId == sensorInfo.OfBlock)?.BlockName + " - " + sensorInfo.SensorName;
                    }
                }

                lock (_syncErrorMsg)
                {
                    var errMsg = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " " + exception.Message;

                    if (!_errList.Contains(errMsg))
                    {
                        _errList.Add(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " " + sensorName + " - " + exception.Message);
                        flagUpdate = true;
                    }

                    if (_errList.Count > ERR_LIMIT_DISPLAY)
                    {
                        _errList.RemoveAt(0);
                    }

                    if (flagUpdate && _errList.Count > 0)
                    {
                        var temp = _errList.ToArray();

                        this.UpdateSafeControl(txtErrors, (txt) =>
                        {
                            txt.Text = string.Join(Environment.NewLine, temp);
                        });

                        flagUpdate = false;
                    }
                }
            }
            catch
            {
            }
        }

        //private async Task registrationBlockAsync()
        private void registrationBlockAsync()
        {
            this.BeginInvoke(new Action(() =>
            {
                timerUpdateGraph.Start();
                timerCheckAlarm.Start();
                tmRefresh.Start();
            }));

            foreach (var block in AppManager.ListBlock)
            {

                if (block.Active == (int)clsConst.emBlockStatus.Inactive)
                {
                    continue;
                }

                var blockThread = new BlockThreadProcess(block);
                blockThread.HandleReceiveMeasureDetail += HandleReceiveMeasureDetail;
                blockThread.ErrorsReceiveDataFromBlock += ErrorsReceiveDataFromBlock;
                blockThreadProcesses.Add(blockThread);

                ThreadPool.QueueUserWorkItem(delegate
                {
                    try
                    {
                        blockThread.Start();
                    }
                    catch (DSF602Exception ex)
                    {
                        ErrorsReceiveDataFromBlock(null, new DSF602Exception(block.BlockName + " not connect", DSF602ExceptionStatusCode.Connect));
                        ChangeStatusBlock(block);
                    }
                });

                //Task.Factory.StartNew(() =>
                //{

                //    try
                //    {
                //        blockThread.Start();
                //        //await blockThread.StartAsync();
                //    }
                //    catch (DSF602Exception ex)
                //    {
                //        ErrorsReceiveDataFromBlock(null, new DSF602Exception(block.BlockName + " not connect", DSF602ExceptionStatusCode.Connect));
                //        ChangeStatusBlock(block);
                //    }
                //});
            }
        }

        private void ChangeStatusBlock(Block block)
        {
            var sensorsOfBlock = AppManager.ListSensor.Where(i => i.OfBlock == block.BlockId).ToList();
            this.UpdateChildControl(plnLocation, clsConst.PREFIX_BTN_BLOCK + block.BlockId, (c) =>
            {
                c.Enabled = false;
            });
            this.UpdateSafeControl(tabGraphMain, (c) =>
            {
                foreach (var sensor in sensorsOfBlock)
                {
                    var objChildControl = _plnMain.Controls.ContainsKey(clsConst.PREFIX_LBL_SENSOR + sensor.SensorId) ? _plnMain.Controls[clsConst.PREFIX_LBL_SENSOR + sensor.SensorId] : null;
                    var objMapBlockContent = _plnMain.Controls.ContainsKey(clsConst.PREFIX_LBL_BLOCK + block.BlockId) ? _plnMain.Controls[clsConst.PREFIX_LBL_BLOCK + block.BlockId] : null;

                    if (sensor.Alarm != true && objChildControl != null)
                    {
                        objMapBlockContent.BackColor = Color.DarkGray;
                        objChildControl.BackColor = Color.DarkGray;
                    }
                }
            });
        }

        private void RenderButtonBlock()
        {
            var sizeTemplate = btn1.Size;
            var locationTemplate = btn1.Location;
            var cnn = 1;
            var flag = false;

            this.plnLocation.Controls.Clear();
            this.plnLocation.Controls.Add(this.lblLocation);

            //Gắn vào panel
            this.plnLocation.SuspendLayout();

            foreach (var block in AppManager.ListBlock)
            {
                var btnBlock = CreateButtonAdd(clsConst.PREFIX_BTN_BLOCK + cnn.ToString(), block.BlockName, sizeTemplate, new Point(locationTemplate.X, locationTemplate.Y), block.BlockId);
                locationTemplate.Y += (btnBlock.Size.Height + PADDING_BOTTOM);

                if (block.Active == (int)clsConst.emBlockStatus.Inactive)
                {
                    btnBlock.Enabled = false;
                }
                else if (flag == false)
                {
                    BlockActiceId = block.BlockId;
                    btnBlock_Click(btnBlock, null);
                    flag = true;
                }

                this.plnLocation.Controls.Add(btnBlock);
                cnn++;
            }

            this.plnLocation.SuspendLayout();
            this.plnLocation.ResumeLayout(false);
        }

        private Button CreateButtonAdd(string buttonName, string buttonText, Size sizeTemplate, Point buttonLocation, int blockId)
        {
            var btnBlock = new Button();
            btnBlock.Location = buttonLocation;
            btnBlock.Name = buttonName;
            btnBlock.Size = sizeTemplate;
            btnBlock.UseVisualStyleBackColor = true;
            btnBlock.Tag = blockId;
            btnBlock.Text = buttonText;

            //set event
            btnBlock.Click += btnBlock_Click;

            return btnBlock;
        }

        private void UpdateInfoChanged(ObjChanged objChanged)
        {
            if (objChanged == null)
            {
                return;
            }

            var lstBlockActive = AppManager.ListBlock.Where(i => i.Active == (int)clsConst.emBlockStatus.Active).OrderBy(i => i.BlockId).ToList();

            if (lstBlockActive != null && lstBlockActive.Count > 0)
            {
                BlockActiceId = lstBlockActive.First().BlockId;
                lblBlockName.Text = lstBlockActive.First().BlockName;

                this.UpdateChildControl(plnLocation, clsConst.PREFIX_BTN_BLOCK + BlockActiceId, (c) =>
                {
                    c.Text = lstBlockActive.First().BlockName;
                    btnBlock_Click((Button)c, null);
                });
            }

            if (objChanged.ListBlockIdChanged != null)
            {
                var listBlockUpdate = AppManager.ListBlock.Where(i => objChanged.ListBlockIdChanged.Contains(i.BlockId));
                var listSensorUpdate = AppManager.ListSensor.Where(i => objChanged.ListSensorIdChanged.Contains(i.SensorId));

                foreach (var sensor in listSensorUpdate)
                {
                    this.UpdateSafeControl(tabGraphMain, (c) =>
                    {
                        var objChildControl = _plnMain.Controls.ContainsKey(clsConst.PREFIX_LBL_SENSOR + sensor.SensorId) ? _plnMain.Controls[clsConst.PREFIX_LBL_SENSOR + sensor.SensorId] : null;

                        if (objChildControl != null)
                        {
                            objChildControl.Text = sensor.SensorName;

                            objChildControl.BackColor = sensor.Active == (int)clsConst.emBlockStatus.Active ? Color.DarkCyan : Color.DarkGray;
                        }

                    });
                }

                foreach (var block in listBlockUpdate)
                {
                    this.UpdateChildControl(plnLocation, clsConst.PREFIX_BTN_BLOCK + block.BlockId, (c) =>
                    {
                        c.Text = block.BlockName;

                        if (block.Active == (int)clsConst.emBlockStatus.Inactive)
                        {
                            c.BackColor = btnUpdate.BackColor;
                        }

                        c.Enabled = block.Active == (int)clsConst.emBlockStatus.Active ? true : false;


                    });

                    this.UpdateSafeControl(tabGraphMain, (c) =>
                    {
                        var objChildControl = _plnMain.Controls.ContainsKey(clsConst.PREFIX_LBL_BLOCK + block.BlockId) ? _plnMain.Controls[clsConst.PREFIX_LBL_BLOCK + block.BlockId] : null;

                        if (objChildControl != null)
                        {
                            objChildControl.Text = block.BlockName;
                            //objChildControl.BackColor = block.Active == (int)clsConst.emBlockStatus.Active ? Color.DarkCyan : Color.DarkGray;

                            if (block.Active == (int)clsConst.emBlockStatus.Active)
                            {
                                objChildControl.BackColor = Color.DarkCyan;
                            }
                            else
                            {
                                objChildControl.BackColor = Color.DarkGray;
                                var listSensor = AppManager.ListSensor.Where(i => i.OfBlock == block.BlockId).ToList();
                                foreach (var sensor in listSensor)
                                {
                                    this.UpdateSafeControl(tabGraphMain, (s) =>
                                    {
                                        var objSensorControl = _plnMain.Controls.ContainsKey(clsConst.PREFIX_LBL_SENSOR + sensor.SensorId) ? _plnMain.Controls[clsConst.PREFIX_LBL_SENSOR + sensor.SensorId] : null;

                                        if (objSensorControl != null)
                                        {
                                            objSensorControl.BackColor = Color.DarkGray;
                                        }

                                    });
                                }
                            }
                        }
                    });
                }

                Application.DoEvents();
            }
        }

        public void StopProcess(int intSleep)
        {
            timerUpdateGraph.Stop();
            timerCheckAlarm.Stop();
            tmRefresh.Stop();
            //this.Hide();

            while (blockThreadProcesses.Any(i => i.IsRunningModbus))
            {
                foreach (var blockThread in blockThreadProcesses)
                {
                    blockThread.End();
                }

                Thread.Sleep(intSleep);
            }

            blockThreadProcesses.Clear();
            blockThreadProcesses = new List<BlockThreadProcess>();
        }
        #endregion Private Funtion

        #region Override Function

        protected override void WndProc(ref Message message)
        {
            const int WM_SYSCOMMAND = 0x0112;
            //const int SC_MAXIMIZE = 0xF030;
            //const int SC_MINIMIZE = 0xF020;
            const int SC_RESTORE = 0xF120;
            const int SC_MOVE = 0xF010;

            if (message.Msg == WM_SYSCOMMAND)
            {
                int command = message.WParam.ToInt32() & 0xfff0;

                switch (command)
                {
                    case SC_RESTORE:
                        if (this.WindowState == FormWindowState.Maximized)
                        {
                            return;
                        }
                        break;

                    case SC_MOVE:
                        return;
                }
            }

            base.WndProc(ref message);
        }

        protected override void SetLanguageControl()
        {
            LanguageHelper.SetValueOf(this, "MAIN_TITLE");
            LanguageHelper.SetValueOf(btnDevice, "MAIN_BTN_DEVICE");
            LanguageHelper.SetValueOf(btnData, "MAIN_BTN_DATA");
            LanguageHelper.SetValueOf(btnUser, "MAIN_BTN_USER");
            LanguageHelper.SetValueOf(btnLanguage, "MAIN_BTN_LANGUAGE");
            LanguageHelper.SetValueOf(btnUpdate, "MAIN_BTN_UPDATE");
            LanguageHelper.SetValueOf(btnReset, "MAIN_BTN_RESET");
            LanguageHelper.SetValueOf(tabGraph, "MAIN_TAB_GRAPH");
            LanguageHelper.SetValueOf(tablMap, "MAIN_TAB_MAP");
            LanguageHelper.SetValueOf(tabErr, "MAIN_TAB_ERR");
            LanguageHelper.SetValueOf(lblView, "MAIN_VIEW");
            LanguageHelper.SetValueOf(lblLocation, "MAIN_LOCATION");
            LanguageHelper.SetValueOf(btnAbout, "ABOUT");
            LanguageHelper.SetValueOf(tabAlarmReport, "ALARM_TAB");
            this.Text = clsCommon.AddVersionApp(this.Text);
            Application.DoEvents();
        }

        #endregion Override Function

        private void timerUpdateGraph_Tick(object sender, EventArgs e)
        {
            if (tabGraph.Controls.Count == 0) return;
            var objlayout = tabGraph.Controls[0] as GraphTypeBase;
            if (objlayout == null) return;
            objlayout.RefreshGraph();
            //Application.DoEvents();
        }

        private void timerCheckAlarm_Tick(object sender, EventArgs e)
        {
            if (blockThreadProcesses == null || blockThreadProcesses.Count == 0)
            {
                return;
            }

            foreach (var blockThread in blockThreadProcesses)
            {

                if (!blockThread.IsRunning)
                {
                    continue;
                }

                //var sensorAlam = blockThread.ListSensorChild;
                var sensorAlam = blockThread.ListSensorChild.Where(i => i.Active == (int)clsConst.emBlockStatus.Active).ToList();

                if (sensorAlam == null || sensorAlam.Count == 0)
                {
                    continue;
                }

                //Task.Run(() =>
                ThreadPool.QueueUserWorkItem(delegate
                {
                    this.UpdateChildControl(plnLocation, clsConst.PREFIX_BTN_BLOCK + blockThread.BlockData.BlockId, (c) =>
                    {
                        if (sensorAlam.Any(i => i.Alarm == true))
                        {
                            c.BackColor = c.BackColor == Color.Red ? btnUpdate.BackColor : Color.Red;
                        }
                        else
                        {
                            c.BackColor = btnUpdate.BackColor;
                        }
                    });
                });

                if (blockThread.BlockData.BlockId == BlockActiceId && tabGraphMain.SelectedIndex == tabGraph.TabIndex)
                {
                    //Task.Run(() =>
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        //    this.UpdateSafeControl(tabGraphMain, (c) =>
                        //    {
                        //        //if (tabGraphMain.SelectedIndex == tablMap.TabIndex)
                        //        //{
                        //        //    foreach (var sensor in sensorAlam)
                        //        //    {
                        //        //        var objChildControl = _plnMain.Controls.ContainsKey(clsConst.PREFIX_LBL_SENSOR + sensor.SensorId) ? _plnMain.Controls[clsConst.PREFIX_LBL_SENSOR + sensor.SensorId] : null;
                        //        //        if (objChildControl == null) return;

                        //        //        if (sensor.Alarm == true)
                        //        //        {
                        //        //            objChildControl.BackColor = objChildControl.BackColor == Color.Red ? Color.DarkCyan : Color.Red;
                        //        //        }
                        //        //        else
                        //        //        {
                        //        //            objChildControl.BackColor = Color.DarkCyan;
                        //        //        }
                        //        //    }
                        //        //}
                        //        //else if (tabGraphMain.SelectedIndex == tabGraph.TabIndex)
                        //        //{

                        //        //}

                        var objLayout = tabGraph.Controls[0] as GraphTypeBase;
                        if (objLayout == null) return;

                        foreach (SensorInfo sensorData in sensorAlam)
                        {
                            var lbl = objLayout.GetDisplaySensorByIndex(sensorData.Ordinal_Display - 1);
                            this.UpdateSafeControl(lbl, l =>
                            {
                                ((DisplaySensor)l).SetSensorName(sensorData);
                            });
                        }

                        //});
                    });
                }
            }
        }

        private bool checkLinkUpdate()
        {
            if (String.IsNullOrEmpty(Properties.Settings.Default.URI_FileUpdate)) return false;

            if (String.IsNullOrEmpty(Properties.Settings.Default.URI_FileVersion)) return false;

            return true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseCheckMsg)
            {
                e.Cancel = !ComfirmMsg(LanguageHelper.GetValueOf("APPCLOSE_CONFIRM_MSG"), LanguageHelper.GetValueOf("APPCLOSE_CONFIRM_MSG_TITLE"));
            }
        }

        private void tabGraphMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (objMap != null)
            {
                objMap.AllowRuningUpdate = tabGraphMain.SelectedIndex == 1;
            }

            //if (objAlarm != null)
            //{
            //    objAlarm.GetData(BlockActiceId);
            //}
        }

        public void Charge(int sensorId, MeasureState state, bool isAlarm = false)
        {
            // Check decay params
            var sensor = AppManager.ListSensor.FirstOrDefault(s => s.SensorId == sensorId);
            //if (sensor == null || sensor.DecayUpperValue == 0 || sensor.DecayTimeCheck == 0 || sensor.DecayStopTime == 0) return;

            //if (isDecayRuning && state == MeasureState.Positive)
            //{
            //    ShowMsg(MessageBoxIcon.Warning, "Has one device is running. Please waiting!");
            //    return;
            //}

            // Excecute
            if (blockThreadProcesses != null)
            {
                foreach (var item in blockThreadProcesses)
                {
                    if (item.BlockData.BlockId == BlockActiceId)
                    {
                        processorCharge.Add(new DataCharge
                        {
                            Sensor = sensor,
                            ChargeState = state,
                            BlockThread = item,
                            IsAlarm = isAlarm
                        });
                        break;
                    }
                }
            }

            //var AppManager.ListSensor.FirstOrDefault(s => s.SensorId == sensorId);
            //if (s)
        }

    }
}