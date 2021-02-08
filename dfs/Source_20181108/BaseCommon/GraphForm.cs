using BaseCommon.Properties;
using GraphLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BaseCommon.clsConst;

namespace BaseCommon
{
    public partial class GraphForm : BaseForm
    {

        #region Const

        private const string MSG_EXIT_APP = "Measure is starting";
        private const string MSG_ERR_CONNECT = "Cannot connect to this device, please check network or connection information!";
        private const string MSG_ERR_INPUT_NUMBER = "Please input {0} is a number than 0.";
        private const string COMFIRM_ERR_CONNECT_DEVICE = "Cannot connect to this device, please check network or connection information!\n Do you want re connect?";

        private const string DEVICE_ERROR = "DEVICE ERROR";
        private const int DEVICE1 = 0;
        private const int DEVICE2 = 1;

        #endregion Const

        #region Variable

        private IGraphForm _iSetting;
        private TelnetInterfaceDsf telnetDSF = null;
        private Thread _threadInsert = null;

        // Measure Info
        private bool _flagInsertDetail = false;
        private bool _blnStartMeasure = false;
        private int _measureResult;
        private string _message = string.Empty;
        private object syncObj = new object();
        private List<MaxValue> lstMaxValue;
        private List<DataSample> lstSample;
        private DataSample dtsCurrent;
        private DateTime? _measureEndTime;
        private bool _isMeasureUp = true;
        private int _MaxKey = 0;
        private int _beforeActualDelegate = 0;

        // Device Info
        private DeviceInfo[] _deviceInfo = new DeviceInfo[2];
        private DeviceInfo _deviceCurrent;

        // Graph Info        
        private int intDataSrcIndex = 0;
        private DataSource graphDS;

        // UI Info
        private bool blnAlarm = false;
        private Button currentXButton;
        private Button currentYButton;
        private Color alarmLabelColor;

        #endregion Variable

        #region Properties

        public bool IsDeBugMode { get; set; } = false;

        public bool IsWaitMode
        {
            get
            {
                return this.UseWaitCursor;
            }
            set
            {
                if (this.Visible && this.InvokeRequired)
                {
                    this.Invoke(new Action(() => {
                        this.IsWaitMode = value;
                    }));
                }
                else
                {
                    this.UseWaitCursor = value;
                    this.Enabled = !value;
                }
            }
        }

        public emMeasureType MeasureType { get; set; } = emMeasureType.AlarmTest;
        public int MeasureResult
        {
            get
            {
                return _measureResult;
            }
        }

        public string xAxis
        {
            get
            {
                return Settings.Default.xAxis;
            }
            set
            {
                Settings.Default.xAxis = value;
                Settings.Default.Save();
            }
        }

        public string yAxis
        {
            get
            {
                return Settings.Default.yAxis;
            }
            set
            {
                Settings.Default.yAxis = value;
                Settings.Default.Save();
            }
        }

        public List<MaxValue> ListMaxValue
        {
            get
            {
                return lstMaxValue;
            }
        }

        public DateTime MeasureEndTime
        {
            get
            {
                return _measureEndTime != null ? (DateTime)_measureEndTime : DateTime.Now;
            }
        }

        #endregion Properties

        #region Event Form

        public GraphForm(IGraphForm iSetting, bool blnIsDebug = false)
        {
            _iSetting = iSetting;
            _iSetting.FormSetting = this;
            IsDeBugMode = blnIsDebug;

            InitializeComponent();
            IntForm();
        }

        private void GraphForm_Load(object sender, EventArgs e)
        {
            GraphInit();
        }
        
        private void GraphForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_blnStartMeasure)
            {
                e.Cancel = !ShowMsg(MessageBoxIcon.Warning, MSG_EXIT_APP);
                return;
            }

            Disconnect();
            
            if (_iSetting != null)
            {
                _iSetting.Dispose();
                _iSetting = null;
            }
        }
        
        private void btnDevice1_Click(object sender, EventArgs e)
        {
            MeasureStart(DEVICE1);
        }

        private void btnDevice2_Click(object sender, EventArgs e)
        {
            MeasureStart(DEVICE2);
        }
        
        private void btnWalkingStart_Click(object sender, EventArgs e)
        {
            MeasureStart(DEVICE1, emMeasureType.WalkingTest);
        }
        
        private void btnWalkingStart2_Click(object sender, EventArgs e)
        {
            MeasureStart(DEVICE2, emMeasureType.WalkingTest);
        }
        
        private void xAxis_Click(object sender, EventArgs e)
        {
            if (currentXButton != null) currentXButton.BackColor = Button.DefaultBackColor;

            Button btnBox = (Button)sender;
            xAxis = btnBox.Name;

            btnBox.BackColor = Color.LightBlue;
            currentXButton = btnBox;

            int xAxisValue = clsCommon.CnvNullToInt(btnBox.Text.Replace("s", ""));

            pdeGraph.SetDisplayRangeX(0, xAxisValue);
            pdeGraph.SetGridDistanceX(10);
            RefreshGraph();
        }

        private void yAxis_Click(object sender, EventArgs e)
        {
            if (currentYButton != null) currentYButton.BackColor = Button.DefaultBackColor;

            Button btnBox = (Button)sender;
            yAxis = btnBox.Name;

            btnBox.BackColor = Color.LightBlue;
            currentYButton = btnBox;

            int yAxisValue = clsCommon.CnvNullToInt(btnBox.Text.Replace("V", "").Replace("k", ""));
            if (yAxisValue < 500) yAxisValue = yAxisValue * _iSetting.RoundValue;
            yAxisValue++;
            graphDS.SetDisplayRangeY(-yAxisValue, yAxisValue);

            if (yAxisValue >= 5000) graphDS.SetGridDistanceY(1000);
            else graphDS.SetGridDistanceY(100);

            RefreshGraph();
        }

        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            var randomTest = new Random();

            GraphInit("Test", 60);

            var cnn = 0;
            var type = 1;
            MeasureType = emMeasureType.AlarmTest;
            GetInputDevice(DEVICE1);
            ResetMeasure();
            ModeMeasure(_deviceCurrent.deviceType);

            _blnStartMeasure = true;
            _flagInsertDetail = true;
            _iSetting.MeasureType = MeasureType;
            _iSetting.DeviceCurrent = _deviceCurrent;
            _iSetting.MeasureStart();
            if (MeasureType == emMeasureType.WalkingTest)
            {
                timerWalkingTest.Interval = Math.Abs(_deviceCurrent.period * 1000);
                timerWalkingTest.Start();
            }
            _threadInsert = new Thread(new ThreadStart(CallProcessData));
            _threadInsert.Start();
            for (var i = 0; i < clsCommon.CnvNullToInt(txtTimeTest.Text); i++)
            {
                if (cnn == 6)
                {
                    cnn = 0;
                    type = -type;
                }
                //float intValue = (float)(type * 59) / 1000;
                float intValue = (float)randomTest.Next(-100, 100) / 1000;
                //float intValue = cnn < 6 && cnn > 2 ? (float)(type * 60) / 1000 : (float)(type * 59) / 1000;
                //float intValue = cnn < 5 && cnn > 3 ? (float)randomTest.Next(-100, 100) / 1000 : (float)60 / 1000;
                //var dts = new DataSample { t = DateTime.Now, strSample = intValue.ToString() };
                //DrawValueToGraph(dts);

                //int intValue = i < 2 || i > 5 ? 1 : 2000;

                ParseData2("\"" + intValue + "\"");
                cnn++;
                //Application.DoEvents();
                Thread.Sleep(100);
            }

            //_blnStartMeasure = false;
            //_flagInsertDetail = false;

            //RefreshGraph();

            MeasureEnd(null, null);
        }
                
        #endregion Event Form

        #region Measure

        private void MeasureStart(int intDev, emMeasureType type = emMeasureType.AlarmTest)
        {

            MeasureType = type;

            // Get info custom input
            if (!GetInputDevice(intDev))
            {
                return;
            }

            var GraphTitle = lblAlarmTest.Text.Trim();
            telnetDSF = new TelnetInterfaceDsf();
            _measureResult = (int)emMeasureResult.Normal;
            
            if (MeasureType == emMeasureType.WalkingTest)
            {
                _measureResult = (int)emMeasureResult.Pass;
                GraphTitle = lblWalkingTest.Text.Trim();
            }

            try
            {
                if (telnetDSF.ConnectToServer(_deviceCurrent.ipAddress, _deviceCurrent.port))
                {
                    telnetDSF.OnDataReceived += new ClientHandlePacketData(client_OnDataReceived);
                    telnetDSF.ProcessErrors += new ClientHandleWhenHasErrors(client_ProcessErrors);
                    
                    ResetMeasure();
                    ModeMeasure(_deviceCurrent.deviceType);

                    GraphInit(GraphTitle + " " + DateTime.Now.ToString(cstrDateTimeFomatShow), 
                              MeasureType == emMeasureType.WalkingTest ? _deviceCurrent.failLevel : _deviceCurrent.AlarmValue);
                    
                    _iSetting.MeasureType = MeasureType;
                    _iSetting.DeviceCurrent = _deviceCurrent;
                    _iSetting.MeasureStart();

                    _flagInsertDetail = true;
                    _threadInsert = new Thread(new ThreadStart(CallProcessData));
                    _threadInsert.Start();
                    telnetDSF.Start();

                    if (MeasureType == emMeasureType.WalkingTest)
                    {
                        timerWalkingTest.Interval = Math.Abs(_deviceCurrent.period * 1000);
                        timerWalkingTest.Start();
                    }

                    _blnStartMeasure = true;
                }
                else
                {
                    telnetDSF = null;
                    ShowMsg(MessageBoxIcon.Error, MSG_ERR_CONNECT);
                }
            }
            catch
            {
                ShowMsg(MessageBoxIcon.Error, MSG_ERR_CONNECT);
            }
        }

        private void MeasureEnd(object sender, EventArgs e)
        {
            if (MeasureType == emMeasureType.WalkingTest)
            {
                timerWalkingTest.Stop();
            }

            Disconnect();
            //blnAlarm = false;
            _flagInsertDetail = false;
            _blnStartMeasure = false;
            this.IsWaitMode = true;
        }

        /// <summary>
        /// Meno      : Change mode measure 
        /// Create by : 2018/09/17 AKB Nguyen Thanh Tung
        /// </summary>
        /// <param name="intDev">DEVICE1/DEVICE2</param>
        /// <param name="isStart"></param>
        private void ModeMeasure(int intDev, bool isStart = true)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    ModeMeasure(intDev, isStart);
                }));
            } else
            {
                btnManagement.Enabled = !isStart;
                txtPeriod.ReadOnly = isStart;
                txtFailLevel.ReadOnly = isStart;
                txtPeriod2.ReadOnly = isStart;
                txtFailLevel2.ReadOnly = isStart;

                if (_deviceInfo[DEVICE1].active)
                {
                    txtPeriod.ReadOnly = isStart;
                    txtFailLevel.ReadOnly = isStart;
                    btnWalkingStart.Enabled = !isStart;
                    txtDev1AlarmValue.ReadOnly = isStart;
                    btnDevice1.Enabled = !isStart;
                    btnStopDevice1.Enabled = isStart && intDev == DEVICE1 && MeasureType == emMeasureType.AlarmTest;
                }

                if (_deviceInfo[DEVICE2].active)
                {
                    txtDev2AlarmValue.ReadOnly = isStart;
                    txtPeriod2.ReadOnly = isStart;
                    txtFailLevel2.ReadOnly = isStart;
                    btnDevice2.Enabled = !isStart;
                    btnStopDevice2.Enabled = isStart && intDev == DEVICE2 && MeasureType == emMeasureType.AlarmTest;
                    btnWalkingStart2.Enabled = !isStart;
                }
            }
        }
        
        private void ParseData2(string message)
        {
            message = Regex.Replace(message, @"\t|\n|\r", "");
            _message = string.IsNullOrEmpty(_message) ? message : _message + message;

            // Raw Data
            if (_message != "\"" && !_message.EndsWith("\"\"") && _message.EndsWith("\""))
            {
                lock (syncObj)
                {
                    lstSample.Add(GetDataFromSample(_message, true));
                }
                _message = string.Empty;
            }

            // Detail Data
            if (dtsCurrent.isFrist)
            {
                dtsCurrent.t = DateTime.Now;
                dtsCurrent.strSample = message;
                dtsCurrent.isFrist = false;

                if (dtsCurrent.strSample != "\"" && !dtsCurrent.strSample.EndsWith("\"\"") && dtsCurrent.strSample.EndsWith("\""))
                {
                    ProcessData(GetDataFromSample(dtsCurrent.strSample));
                    dtsCurrent.strSample = string.Empty;
                }
            }
            else
            {
                dtsCurrent.strSample += message;

                if (dtsCurrent.strSample != "\"" && !dtsCurrent.strSample.EndsWith("\"\"") && dtsCurrent.strSample.EndsWith("\""))
                {
                    var now = DateTime.Now;
                    var checkTime = now - dtsCurrent.t;
                    var checkMilliseconds = MeasureType == emMeasureType.AlarmTest ? _iSetting.AlarmTime : _iSetting.WalkingTime;
                    
                    if (checkTime.TotalMilliseconds >= checkMilliseconds)
                    {
                        dtsCurrent.t = now;
                        ProcessData(GetDataFromSample(dtsCurrent.strSample));
                        dtsCurrent.strSample = string.Empty;
                    }
                }
            }

            #region code old
            //message = Regex.Replace(message, @"\t|\n|\r", "");
            //if (message == "") return;

            //_message = string.IsNullOrEmpty(_message) ? ParseData(message) : _message + ParseData(message);

            //if (_message.EndsWith("\""))
            //{
            //    int x;
            //    if (graphDS.Samples == null) x = 0;
            //    else x = graphDS.Samples.Count;
            //    var addDataSample = new DataSample
            //    {
            //        deviceId = _deviceCurrent.deviceId,
            //        strSample = _message,
            //        t = DateTime.Now,
            //        isRaw = true,
            //    };

            //    lock (syncObj)
            //    {
            //        lstSample.Add(addDataSample);
            //    }

            //    if (MeasureType == emMeasureType.WalkingTest)
            //    {

            //        ProcessData(addDataSample, intDataIndex++);
            //        dtsCurrent = addDataSample;

            //        // Get Value From Sample
            //        var strSample = addDataSample.strSample;
            //        strSample = strSample.Replace("\"\"", ",");
            //        strSample = strSample.Replace("\"", "");

            //        var arrValue = !string.IsNullOrEmpty(strSample) ? strSample.Split(',') : null;

            //        if (arrValue != null && arrValue.Length > 0)
            //        {
            //            var temp = arrValue.Select(i => new MaxValue
            //            {
            //                datetime = addDataSample.t,
            //                maxValue = Math.Abs(clsCommon.CnvNullToDouble(i, 0) * 1000),
            //                maxValueShow = clsCommon.CnvNullToDouble(i, 0) * 1000,
            //                point = new cPoint() { x = x, y = (float)(clsCommon.CnvNullToDouble(i, 0) * 1000) }
            //            }).ToList();

            //            temp.AddRange(lstMaxValue);
            //            temp = temp.OrderByDescending(i => i.maxValue).Take(_deviceCurrent.samples).ToList();
            //            lstMaxValue = temp;
            //        }
            //    }

            //    _message = string.Empty;
            //}

            //if (MeasureType == emMeasureType.WalkingTest)
            //{
            //    return;
            //}

            //DataSample dts = new DataSample();
            //string strCurrentSample;
            //int index;

            //dts.t = DateTime.Now;
            //dts.strSample = message;

            //if (dtsCurrent.strSample == cstrNothing)
            //{
            //    dtsCurrent = dts;
            //}

            ////proccess the first received data
            //if (dtsCurrent.t == dts.t)
            //{
            //    if (!dtsCurrent.strSample.StartsWith("\""))
            //    {
            //        dtsCurrent.strSample = "\"" + dtsCurrent.strSample;
            //    }

            //    return;
            //}

            //var test = dts.t - dtsCurrent.t;
            //Console.WriteLine("Milliseconds: " + test.TotalMilliseconds);
            //if (test.TotalMilliseconds < 100)
            //{
            //    dtsCurrent.strSample += dts.strSample;
            //    Console.WriteLine("<100: " + dtsCurrent.strSample);
            //}
            //else
            //{
            //    strCurrentSample = dtsCurrent.strSample;

            //    if (ProcessData(dtsCurrent, intDataIndex++))
            //    {
            //        dtsCurrent.strSample = cstrNothing;
            //    }
            //    else
            //    {
            //        index = dts.strSample.IndexOf("\"");
            //        string strTemp = "";

            //        if (index == 0)
            //        {
            //            dtsCurrent.strSample = dtsCurrent.strSample + "\"";
            //            if (ProcessData(dtsCurrent, intDataIndex++)) dtsCurrent.strSample = cstrNothing;
            //        }
            //        else
            //        {
            //            strTemp = dts.strSample.Substring(0, index + 1);
            //            dtsCurrent.strSample = dtsCurrent.strSample + strTemp;
            //            ProcessData(dtsCurrent, intDataIndex++);
            //            if (dts.strSample.Length - index - 1 > 0)
            //            {
            //                dts.strSample = dts.strSample.Substring(index + 1, dts.strSample.Length - index - 1);
            //                if (!ProcessData(dts, intDataIndex++))
            //                {
            //                    dtsCurrent = dts;
            //                }
            //                else
            //                {
            //                    dtsCurrent.strSample = cstrNothing;
            //                }
            //            }
            //            else
            //            {
            //                dtsCurrent.strSample = cstrNothing;
            //            }
            //        }
            //    }
            //}
            #endregion code old
        }

        //This method is called when the client has received data from the server
        private void client_OnDataReceived(byte[] data, int bytesRead)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            string message = encoder.GetString(data, 0, bytesRead);
            ParseData2(message);
        }

        private void client_ProcessErrors()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    confirmReConnect();
                }));
            } else
            {
                confirmReConnect();
            }
        }

        private void confirmReConnect()
        {
            while (ComfirmMsgErr(COMFIRM_ERR_CONNECT_DEVICE))
            {
                Task.Delay(1000);

                try
                {
                    if (telnetDSF != null)
                    {
                        telnetDSF.Disconnect();
                        telnetDSF = new TelnetInterfaceDsf();
                    }
                    telnetDSF.OnDataReceived += new ClientHandlePacketData(client_OnDataReceived);
                    telnetDSF.ProcessErrors += client_ProcessErrors;
                    if (telnetDSF.ConnectToServer(_deviceCurrent.ipAddress, _deviceCurrent.port))
                    {
                        telnetDSF.Start();
                        if (MeasureType == emMeasureType.WalkingTest)
                        {
                            timerWalkingTest.Start();
                        }
                        return;
                    }
                }
                catch
                {
                }
            }

            MeasureEnd(null, null);
        }

        private void ProcessData(DataSample dts)
        {
            if (IsDeBugMode)
            {
                var strTime = dts.t.ToString(cstrDateTimeFormatMiliSecond);
                var index = dgvMeasure.Rows.Count;
                SetGridValue(new object[] { index, strTime, dts.strSample }, dgvMeasure);
                SetValueText(index.ToString(), txtCount);
            }
            
            if (_beforeActualDelegate != dts.actualDelegate)
            {
                var isMeasureUp = dts.actualDelegate > _beforeActualDelegate;

                if (isMeasureUp != _isMeasureUp)
                {
                    _isMeasureUp = isMeasureUp;
                    _MaxKey++;
                }

                _beforeActualDelegate = dts.actualDelegate;
            }

            var drawValue = DrawValueToGraph(dts);

            if (MeasureType == emMeasureType.AlarmTest)
            {
                SetValueText(drawValue.ToString("N0"), _deviceCurrent.deviceType == DEVICE1 ? txtDev1ActualValue : txtDev2ActualValue);
            }
            else if (MeasureType == emMeasureType.WalkingTest)
            {
                var max = new MaxValue
                {
                    key = _MaxKey,
                    dtSample =dts,
                    datetime = dts.t,
                    maxValue = Math.Abs(dts.actualDelegate),
                    maxValueShow = dts.actualDelegate,
                    point = new cPoint()
                    {
                        x = graphDS.Samples != null ? graphDS.Samples.Count - 1 : 0,
                        y = (float)dts.actualDelegate
                    }
                };

                lstMaxValue = lstMaxValue.Where(i => i.key != _MaxKey).ToList();
                lstMaxValue.Add(max);
                lstMaxValue = lstMaxValue.OrderByDescending(i => i.maxValue)
                                         .ThenByDescending(i => i.point.x)
                                         .Take(_deviceCurrent.samples)
                                         .ToList();

                lstMaxValue = lstMaxValue.OrderBy(i => i.point.x)
                                         .ThenBy(i => i.maxValue)
                                         .ToList();

                //// Get Value From Sample
                //var strSample = dts.strSample;
                //strSample = strSample.Replace("\"\"", ",");
                //strSample = strSample.Replace("\"", "");
                //strSample = strSample.Replace(" ", "");

                //var arrValue = !string.IsNullOrEmpty(strSample) ? strSample.Split(',') : null;

                //if (arrValue != null && arrValue.Length > 0)
                //{
                //    var temp = arrValue.Select(i => new MaxValue
                //    {
                //        dtSample = GetDataFromSample(dts.strSample),
                //        datetime = dts.t,
                //        maxValue = Math.Abs(clsCommon.CnvNullToDouble(i, 0) * _iSetting.RoundValue),
                //        maxValueShow = clsCommon.CnvNullToDouble(i, 0) * _iSetting.RoundValue,
                //        point = new cPoint()
                //        {
                //            x = graphDS.Samples != null ? graphDS.Samples.Count - 1 : 0,
                //            y = (float)(clsCommon.CnvNullToDouble(i, 0) * _iSetting.RoundValue)
                //        }
                //    }).ToList();

                //    temp.AddRange(lstMaxValue);
                //    temp = temp.OrderByDescending(i => i.maxValue).Take(_deviceCurrent.samples).ToList();
                //    lstMaxValue = temp;
                //}
            }
        }
        
        private string GetDeviceType(string strValue)
        {
            strValue = strValue.Replace(" ", "");

            if (strValue.Length == 5)
            {
                graphDS.maxValue = MAX_DEVICE_20K;
                return "20kV";
            }

            if (strValue.Length == 6)
            {
                int index = strValue.IndexOf(".");
                if (strValue.Length - index == 4)
                {
                    graphDS.maxValue = MAX_DEVICE_2K;
                    return "2kV";
                }
                else if (strValue.Length - index == 3)
                {
                    graphDS.maxValue = MAX_DEVICE_20K;
                    return "20kV";
                }
            }

            return DEVICE_ERROR;
        }

        private string GetDeviceType(string[] strValue)
        {
            string strRet = DEVICE_ERROR;
            if (strValue.Length > 0)
            {
                int i = 0;
                do
                {
                    strRet = GetDeviceType(strValue[i]);
                    i++;

                } while (strRet == DEVICE_ERROR && i < strValue.Length);
            }
            return strRet;
        }

        private double DrawValueToGraph(DataSample dts)
        {

            var absActualDelegate = Math.Abs(dts.actualDelegate);
            var dblValueGraph = dts.actualDelegate;
            dts.result = (int)emMeasureResult.Pass;

            try
            {   
                if (MeasureType == emMeasureType.AlarmTest)
                {
                    dts.result = (int)emMeasureResult.Normal;

                    if (_deviceCurrent.AlarmValue <= Math.Abs(dts.actualDelegate))
                    {
                        dts.result = (int)emMeasureResult.Alarm;
                        
                        if (_measureResult == (int)emMeasureResult.Normal)
                        {
                            _measureResult = dts.result;
                        }
                    }

                    // Actual Value > Alarm Value
                    if (absActualDelegate > graphDS.UpLimit)
                    {
                        SetValueLabel(dts.actualDelegate.ToString(), lblOverViewValue);
                        blnAlarm = true;
                        Blink();
                    }

                    if (graphDS.maxValue > 0 && absActualDelegate >= graphDS.maxValue)
                    {
                        dblValueGraph = (dts.actualDelegate / absActualDelegate) * graphDS.maxValue;
                    }
                }
                else
                {
                    graphDS.maxPoint = lstMaxValue.Select(i => i.point).ToArray();
                }
                
                lock (syncObj)
                {
                    lstSample.Add(dts);
                }
                
                AddPoint((float)dblValueGraph);

                if ((graphDS.Length - pdeGraph.starting_idx) >= (int)pdeGraph.endX)
                {
                    float dx = (pdeGraph.endX - pdeGraph.startX);
                    pdeGraph.SetStartingIdx((int)(graphDS.Length - dx - (Math.Abs(pdeGraph.MoveMouse) > 0 ? pdeGraph.MoveMouse - 1 : 0)));
                }

                RefreshGraph();

                return dts.actualDelegate;
            }
            catch
            {
                return 0;
            }
        }

        private void Disconnect()
        {
            if (telnetDSF == null) return;
            telnetDSF.Disconnect();
            telnetDSF = null;
        }

        private DataSample GetDataFromSample(string msg, bool isRaw = false)
        {
            double dblMax;
            double dblMin;
            double dblDelegate;
            string strValue;
            string[] arrValue;

            GetDataFromSample(msg, out arrValue, out strValue, out dblMax, out dblMin, out dblDelegate);

            var result = (int)emMeasureResult.Pass;

            if (MeasureType == emMeasureType.AlarmTest)
            {
                result = (int)emMeasureResult.Normal;

                if (_deviceCurrent.AlarmValue <= (int)dblDelegate)
                {
                    result = (int)emMeasureResult.Alarm;
                }
            }
            
            return new DataSample
            {
                deviceId = _deviceCurrent.deviceId,
                t = DateTime.Now,
                strSample = msg,
                actualValue = strValue,
                arrActualValue = arrValue,
                actualMaxValue = (int)dblMax,
                actualMinValue = (int)dblMin,
                actualDelegate = (int)dblDelegate,
                result = result,
                isRaw = isRaw,
            };
        }

        private void GetDataFromSample(string strSample, out string[] arrValue, out string strValue, out double dblMax, out double dblMin, out double dblDelegate)
        {
            // Default Value
            strValue = string.Empty;
            dblMax = 0;
            dblMin = 0;
            dblDelegate = 0;
            arrValue = new string[1] { "" };

            // Get Value From Sample
            strSample = strSample.Replace("\"\"", ",");
            strSample = strSample.Replace("\"", "");

            if (string.IsNullOrEmpty(strSample))
            {
                return;
            }

            arrValue = strSample.Split(',');

            if (lblDeviceType.Text == string.Empty || lblDeviceType.Text == DEVICE_ERROR)
            {
                SetValueLabel(GetDeviceType(arrValue), lblDeviceType);
            }

            double[] dblValue = arrValue.Select(n => CnvDeviceRstToDouble(n) ).ToArray();

            strValue = string.Join(",", dblValue);
            dblMax = dblValue.Max();
            dblMin = dblValue.Min();
            dblDelegate = Math.Abs(dblMax) >= Math.Abs(dblMin) ? dblMax : dblMin;
        }

        private double CnvDeviceRstToDouble(string strSample)
        {
            strSample = strSample.Replace(" ", "");

            if (strSample.ToUpper() == MAX_DEVICE_RETURN)
            {
                return graphDS.maxValue * (_isMeasureUp ? 1 : -1);
            } else
            {
                return clsCommon.CnvNullToDouble(strSample, 0) * _iSetting.RoundValue;
            }
        }
        
        private void ResetMeasure()
        {
            lblOverViewValue.Text = string.Empty;
            lblRstWalking.Text = "PASS";
            lblRstWalking.ForeColor = Color.Blue;
            lblRstWalking.BackColor = Color.Lime;
            lblAlarmTest.BackColor = alarmLabelColor;
            lblDeviceType.Text = string.Empty;
            txtDev1ActualValue.Text = string.Empty;
            txtDev2ActualValue.Text = string.Empty;
            txtAverage.Text = string.Empty;
            txtAverage2.Text = string.Empty;

            dgvMeasure.Rows.Clear();
            lstMaxValue.Clear();
            dtsCurrent = new DataSample() { isFrist = true };
            blnAlarm = false;
            _message = string.Empty;
            _isMeasureUp = true;
            _MaxKey = 0;
            _beforeActualDelegate = 0;
        }

        private void CallProcessData()
        {
            try
            {
                while (_flagInsertDetail || lstSample.Count > 0)
                {
                    if (lstSample.Count > 0)
                    {
                        var detail = lstSample[0];

                        _iSetting.MeasureProcess(detail);

                        lock (syncObj)
                        {
                            if (lstSample.Count > 0)
                            {
                                lstSample.RemoveAt(0);
                            }
                        }
                    }

                    Thread.Sleep(10);
                }

                _measureEndTime = dtsCurrent.t;

                if (MeasureType == emMeasureType.WalkingTest)
                {
                    SetValueText(lstMaxValue.Average(i => Math.Abs(i.maxValue)).ToString("N0"),
                                 _deviceCurrent.deviceType == DEVICE1 ? txtAverage : txtAverage2);

                    if ((lstMaxValue.Sum(i => i.maxValue) / lstMaxValue.Count) >= _deviceCurrent.failLevel)
                    {
                        _measureResult = (int)emMeasureResult.Fail;
                        SetValueLabel("FAIL", lblRstWalking);
                        lblRstWalking.ForeColor = Color.Yellow;
                        lblRstWalking.BackColor = Color.Red;
                        Application.DoEvents();
                    }
                }

                _iSetting.MeasureEnd();

                ModeMeasure(_deviceCurrent.deviceType, false);
                graphDS.Name += " - " + dtsCurrent.t.ToString(cstrDateTimeFomatShow);
                RefreshGraph();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                this.IsWaitMode = false;
            }            
        }

        #endregion Measure

        #region Graph

        private void GraphInit(string graphName = "GRAPH", int limit = 10)
        {
            pdeGraph.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;
            pdeGraph.SetPlayPanelInvisible();
            pdeGraph.drawAlarmLine = MeasureType == emMeasureType.AlarmTest;
            pdeGraph.drawMaxPoint = MeasureType == emMeasureType.WalkingTest;
            pdeGraph.DataSources.Clear();
            pdeGraph.DataSources.Add(new DataSource());
            graphDS = pdeGraph.DataSources[intDataSrcIndex];
            graphDS.Name = graphName;

            pdeGraph.SetDisplayRangeX(0, 60);
            pdeGraph.SetGridDistanceX(10);
            graphDS.OnRenderXAxisLabel += RenderXLabel;

            graphDS.AutoScaleY = false;
            graphDS.SetDisplayRangeY(-501, 501);
            graphDS.SetGridDistanceY(100);
            graphDS.OnRenderYAxisLabel += RenderYLabel;

            graphDS.UpLimit = limit;
            graphDS.DownLimit = -limit;
            
            UpdateColorSchemaMenu();
            LoadStatusForm();
            
            pdeGraph.StartingIndexOff = 0;
            pdeGraph.SetStartingIdx(0);
            AddPoint(0);

            RefreshGraph();            
        }

        private void LoadStatusForm()
        {
            if (string.IsNullOrEmpty(xAxis))
            {
                xAxis = btn60s.Name;
            }

            if (string.IsNullOrEmpty(yAxis))
            {
                yAxis = btn500V.Name;
            }

            if (pnlBottomTool.Controls.ContainsKey(xAxis))
            {
                ((Button)pnlBottomTool.Controls[xAxis]).PerformClick();
            }

            if (pnlBottomTool.Controls.ContainsKey(yAxis))
            {
                ((Button)pnlBottomTool.Controls[yAxis]).PerformClick();
            }
        }

        private void UpdateColorSchemaMenu()
        {
            Color[] cols = { Color.FromArgb(0,255,0),
                             Color.FromArgb(0,255,0),
                             Color.FromArgb(0,255,0),
                             Color.FromArgb(0,255,0),
                             Color.FromArgb(0,255,0) ,
                             Color.FromArgb(0,255,0),
                             Color.FromArgb(0,255,0) };

            for (int j = 0; j < pdeGraph.DataSources.Count; j++)
            {
                pdeGraph.DataSources[j].GraphColor = cols[j % 7];
            }

            pdeGraph.BackgroundColorTop = Color.FromArgb(0, 15, 33);
            pdeGraph.BackgroundColorBot = Color.FromArgb(0, 15, 33);
            pdeGraph.SolidGridColor = Color.FromArgb(0, 128, 0);
            pdeGraph.DashedGridColor = Color.FromArgb(0, 128, 0);
        }

        private string RenderXLabel(DataSource s, int idx)
        {
            return ((int)s.Samples[idx].x / 10).ToString();
        }

        private string RenderYLabel(DataSource s, float value)
        {
            return ((int)value).ToString();
        }

        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void RefreshGraphCallBack();
        private void RefreshGraph()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            try
            {
                if (pdeGraph.InvokeRequired)
                {
                    RefreshGraphCallBack d = new RefreshGraphCallBack(RefreshGraph);
                    this.Invoke(d, new object[] { });
                }
                else
                {
                    pdeGraph.Refresh();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("RefreshGraph: " + ex.Message);
            }
        }

        #endregion Graph

        #region Public Function

        /// <summary>
        /// Create by : 2018/09/17 AKB Nguyen Thanh Tung
        /// Meno      : Get Info Device
        /// </summary>
        public void LoadInfoDevice()
        {
            txtDev1ActualValue.Text = string.Empty;
            txtDev1AlarmValue.Text = string.Empty;
            txtDev2ActualValue.Text = string.Empty;
            txtDev2AlarmValue.Text = string.Empty;
            txtPeriod.Text = string.Empty;
            txtFailLevel.Text = string.Empty;
            txtAverage.Text = string.Empty;
            txtPeriod2.Text = string.Empty;
            txtFailLevel2.Text = string.Empty;
            txtAverage2.Text = string.Empty;

            btnDevice1.Enabled = false;
            btnStopDevice1.Enabled = false;
            btnDevice2.Enabled = false;
            btnStopDevice2.Enabled = false;

            btnWalkingStart.Enabled = false;
            btnWalkingStart2.Enabled = false;

            _deviceInfo = _iSetting.GetListDevice();

            for (var i = 0; i < _deviceInfo.Length; i++)
            {
                _deviceInfo[i].deviceType = i;
                _deviceInfo[i].active = _deviceInfo[i].active && !string.IsNullOrEmpty(_deviceInfo[i].ipAddress);

                switch (i)
                {
                    case DEVICE1:
                        txtDev1AlarmValue.Text = _deviceInfo[DEVICE1].AlarmValue > 0 ? _deviceInfo[DEVICE1].AlarmValue.ToString() : string.Empty;
                        txtDev1AlarmValue.ReadOnly = !_deviceInfo[DEVICE1].active;
                        txtPeriod.Text = _deviceInfo[DEVICE1].period > 0 ? _deviceInfo[DEVICE1].period.ToString() : string.Empty;
                        txtFailLevel.Text = _deviceInfo[DEVICE1].failLevel > 0 ? _deviceInfo[DEVICE1].failLevel.ToString() : string.Empty;
                        txtPeriod.ReadOnly = !_deviceInfo[DEVICE1].active;
                        txtFailLevel.ReadOnly = !_deviceInfo[DEVICE1].active;
                        btnDevice1.Enabled = _deviceInfo[DEVICE1].active;
                        btnWalkingStart.Enabled = _deviceInfo[DEVICE1].active;
                        break;
                    case DEVICE2:
                        txtDev2AlarmValue.ReadOnly = !_deviceInfo[DEVICE2].active;
                        txtDev2AlarmValue.Text = _deviceInfo[DEVICE2].AlarmValue > 0 ? _deviceInfo[DEVICE2].AlarmValue.ToString() : string.Empty;
                        txtPeriod2.Text = _deviceInfo[DEVICE2].period > 0 ? _deviceInfo[DEVICE2].period.ToString() : string.Empty;
                        txtFailLevel2.Text = _deviceInfo[DEVICE2].failLevel > 0 ? _deviceInfo[DEVICE2].failLevel.ToString() : string.Empty;
                        txtFailLevel2.ReadOnly = !_deviceInfo[DEVICE2].active;
                        txtPeriod2.ReadOnly = !_deviceInfo[DEVICE2].active;
                        btnDevice2.Enabled = _deviceInfo[DEVICE2].active;
                        btnWalkingStart2.Enabled = _deviceInfo[DEVICE2].active;
                        break;
                    default:
                        continue;
                }
            }
        }

        #endregion Public Function

        #region Private Function

        private void AddPoint(float value)
        {
            int x;
            if (graphDS.Samples == null) x = 0;
            else x = graphDS.Samples.Count;

            graphDS.AddPoint(x, value);

            if (Math.Abs(value) > graphDS.endY)
            {
                SetValueLabel(value.ToString(), lblOverViewValue);
            }
        }

        private async void Blink()
        {
            while (blnAlarm)
            {
                await Task.Delay(500);
                lblAlarmTest.BackColor = lblAlarmTest.BackColor == Color.Red ? Color.Green : Color.Red;
            }
            if (!blnAlarm)
            {
                lblAlarmTest.BackColor = alarmLabelColor;
                Application.DoEvents();
            }
        }
        
        /// <summary>
        /// Create by : 2018/09/17 AKB Nguyen Thanh Tung
        /// Meno      : Get input device 
        /// </summary>
        /// <param name="intDev">DEVICE1/DEVICE2</param>
        /// <returns></returns>
        private bool GetInputDevice(int intDev, bool isAlarmTestMode = true)
        {
            if (!Validate(intDev))
            {
                return false;
            }

            _deviceInfo[intDev].AlarmValue = clsCommon.CnvNullToInt((intDev == DEVICE1 ? txtDev1AlarmValue : txtDev2AlarmValue).Text);
            _deviceInfo[intDev].period = clsCommon.CnvNullToInt(intDev == DEVICE1 ? txtPeriod.Text : txtPeriod2.Text);
            _deviceInfo[intDev].failLevel = clsCommon.CnvNullToInt(intDev == DEVICE1 ? txtFailLevel.Text : txtFailLevel2.Text);
            _deviceCurrent = _deviceInfo[intDev];

            return true;
        }

        /// <summary>
        /// Create by : 2018/09/17 AKB Nguyen Thanh Tung
        /// Meno      : Validate input device  
        /// </summary>
        /// <param name="intDev">DEVICE1/DEVICE2</param>
        /// <returns></returns>
        private bool Validate(int intDev, bool isAlarmTestMode = true)
        {
            if (MeasureType == emMeasureType.AlarmTest)
            {
                var txtAlarmValue = (intDev == DEVICE1 ? txtDev1AlarmValue : txtDev2AlarmValue);

                if (!clsCommon.IsNumberAndThanZero(txtAlarmValue.Text))
                {
                    txtAlarmValue.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(MSG_ERR_INPUT_NUMBER, "Alarm Value"));
                }
            }
            else
            {
                if (!clsCommon.IsNumberAndThanZero(txtPeriod.Text))
                {
                    txtPeriod.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(MSG_ERR_INPUT_NUMBER, "Period"));
                }

                if (!clsCommon.IsNumberAndThanZero(txtFailLevel.Text))
                {
                    txtFailLevel.Focus();
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(MSG_ERR_INPUT_NUMBER, "Fail Level"));
                }
            }

            return true;
        }

        private void IntForm()
        {
            // Limit resize form
            this.MinimumSize = this.Size;
            this.MaximumSize = Screen.PrimaryScreen.Bounds.Size;
            
            pdeGraph.DoubleBuffering = true;
            alarmLabelColor = lblAlarmTest.BackColor;
            LoadInfoDevice();

            if (IsDeBugMode)
            {
                btnAddPoint.Visible = true;
                txtCount.Visible = true;
                txtTimeTest.Visible = true;
                dgvMeasure.Visible = true;
            }
                        
            // Add Event
            txtDev1ActualValue.KeyPress += onlyInputNumber;
            txtDev1AlarmValue.KeyPress += onlyInputNumber;
            txtDev2ActualValue.KeyPress += onlyInputNumber;
            txtDev2AlarmValue.KeyPress += onlyInputNumber;
            txtPeriod.KeyPress += onlyInputNumber;
            txtFailLevel.KeyPress += onlyInputNumber;
            
            btn60s.Click += xAxis_Click;
            btn90s.Click += xAxis_Click;
            btn120s.Click += xAxis_Click;
            btn180s.Click += xAxis_Click;

            btn500V.Click += yAxis_Click;
            btn1000V.Click += yAxis_Click;
            btn2000V.Click += yAxis_Click;
            btn5000V.Click += yAxis_Click;
            btn10000V.Click += yAxis_Click;
            btn15000V.Click += yAxis_Click;
            btn20000V.Click += yAxis_Click;

            btnStopDevice1.Click += MeasureEnd;
            btnStopDevice2.Click += MeasureEnd;
            timerWalkingTest.Tick += MeasureEnd;

            lblReset.Click += (sender, e) => { blnAlarm = false; };
            lblAlarmTest.Click += (sender, e) => { blnAlarm = false; };

            btnManagement.Click += _iSetting.ShowManagement;
            btnReport.Click += _iSetting.ShowMeasureManagement;

            // Int var
            lstSample = new List<DataSample>();
            lstMaxValue = new List<MaxValue>();
        }

        #endregion Private Function

    }
}
