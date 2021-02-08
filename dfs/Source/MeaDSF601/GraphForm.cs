namespace MeaDSF601
{
    using GraphLib;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class GraphForm : BaseForm
    {

        private const string MSG_ERR_CONNECT = "Cannot connect to this device, please check network or connection information!";
        private const string MSG_ERR_CONNECT_DB = "Cannot connect to database, please check network or connection information!";
        private const string MSG_ERR_HAS_DB = "Has error in insert data to database";
        private const string COMFIRM_ERR_CONNECT_DB = "Cannot connect to this device, please check network or connection information!\n Do you want re connect";
        private const string FOMAT_MSG_INPUT_NUMBER = "Please input {0} is a number than 0";
        private const string cstrNothing = "NOTHING";
        private const int DEVICE1 = 0;
        private const int DEVICE2 = 1;

        private struct DataSample
        {
            public DateTime t;
            public string strSample;

            public int deviceId;
            public int measureId;
            public string actualValue;
            public int actualMaxValue;
            public int actualMinValue;
            public int actualDelegate;
            public int result;
            public bool isRaw;
        }

        private struct DeviceInfo
        {
            public int deviceId;
            public int deviceType;
            public string ipAddress;
            public int port;
            public int AlarmValue;
            public int period;
            public int failLevel;
            public int samples;
            public bool active;
        }

        private struct MaxValue
        {
            public DateTime datetime;
            public double maxValue;
            public double maxValueShow;
            public cPoint point;
        }

        Random rd;
        TelnetInterfaceDsf telnetDSF = null;
        int intDataSrcIndex = 0;
        DataSource graphDS;
        int intDataIndex;

        Button currentXButton;
        Button currentYButton;
        bool blnAlarm = false;
        Color alarmLabelColor;

        List<DataSample> lstSample;
        DataSample dtsCurrent;

        private bool _blnStartMeasure = false;
        private bool _hasErrDB = false;
        private string _nameErrFileDetail = string.Empty;
        private string _nameErrFileLimit = string.Empty;
        private string _nameErrFileRaw = string.Empty;
        private int _numberRecordWriteDetail = 0;
        private int _numberRecordWriteLimit = 0;
        private int _numberRecordWriteRaw = 0;

        private int _measureId;
        private int _measureResult;
        private clsDBUltity _DBUltity = new clsDBUltity(true);
        private DeviceInfo[] _deviceInfo = new DeviceInfo[2];
        private DeviceInfo _deviceCurrent;
        private clsDBUltity.emMeasureType _MeasureTest;

        List<MaxValue> lstMaxValue = new List<MaxValue>();

        private string _message = string.Empty;
        private object syncObj1 = new object();
        //private object syncObj2 = new object();
        //private object syncObj3 = new object();

        private bool _flagInsertDetail = false;

        //List<DataSample> _lstSampleRaw = new List<DataSample>();
        //List<DataSample> _lstSampleDetal = new List<DataSample>();

        #region Event Form

        public GraphForm()
        {
            InitializeComponent();

#if DEBUG
            btnAddPoint.Visible = true;
            txtCount.Visible = true;
            txtTimeTest.Visible = true;
            dgvMeasure.Visible = true;
#endif

            this.Text = "Normal Graph";

            // Limit resize form
            this.MinimumSize = this.Size;
            this.MaximumSize = Screen.PrimaryScreen.Bounds.Size;
            
            txtDev1ActualValue.KeyPress += onlyInputNumber;
            txtDev1AlarmValue.KeyPress += onlyInputNumber;
            txtDev2ActualValue.KeyPress += onlyInputNumber;
            txtDev2AlarmValue.KeyPress += onlyInputNumber;
            txtPeriod.KeyPress += onlyInputNumber;
            txtFailLevel.KeyPress += onlyInputNumber;

            btn30s.Click += xAxis_Click;
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

            pdeGraph.DoubleBuffering = true;
            alarmLabelColor = lblAlarmTest.BackColor;
            GetInfoDevice();
        }

        private void GraphForm_Load(object sender, EventArgs e)
        {
            dtsCurrent = new DataSample();
            dtsCurrent.strSample = cstrNothing;
            rd = new Random();
            lstSample = new List<DataSample>();
            GraphInit();
        }
        
        private void GraphForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_blnStartMeasure)
            {
                e.Cancel = !Common.ComfirmMsg("Measure is starting. You will sure exit app?");
            }

            this.Hide();
            Disconnect();
            _flagInsertDetail = false;

            Thread.Sleep(3000);

            if (_DBUltity != null)
            {
                _DBUltity.Dispose();
                _DBUltity = null;
            }
        }

        private void GraphForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void btnDevice1_Click(object sender, EventArgs e)
        {
            _MeasureTest = clsDBUltity.emMeasureType.AlarmTest;
            MeasureStart(DEVICE1);
        }

        private void btnDevice2_Click(object sender, EventArgs e)
        {
            _MeasureTest = clsDBUltity.emMeasureType.AlarmTest;
            MeasureStart(DEVICE2);
        }

        private void btnStopDevice1_Click(object sender, EventArgs e)
        {
            MeasureEnd();
        }

        private void btnStopDevice2_Click(object sender, EventArgs e)
        {
            MeasureEnd();
        }

        private void btnWalkingStart_Click(object sender, EventArgs e)
        {
            _MeasureTest = clsDBUltity.emMeasureType.WalkingTest;
            timerWalkingTest.Interval = Common.CnvNullToInt(txtPeriod.Text, 0) * 1000;
            MeasureStart(DEVICE1);
        }
        
        private void btnWalkingStart2_Click(object sender, EventArgs e)
        {
            _MeasureTest = clsDBUltity.emMeasureType.WalkingTest;
            timerWalkingTest.Interval = Common.CnvNullToInt(txtPeriod2.Text, 0) * 1000;
            MeasureStart(DEVICE2);
        }

        private void lblReset_Click(object sender, EventArgs e)
        {
            blnAlarm = false;
            lblAlarmTest.BackColor = alarmLabelColor;
            Application.DoEvents();
        }
        
        private void xAxis_Click(object sender, EventArgs e)
        {
            if (currentXButton != null) currentXButton.BackColor = Button.DefaultBackColor;

            Button btnBox = (Button)sender;

            btnBox.BackColor = Color.LightBlue;
            currentXButton = btnBox;

            int xAxisValue = Convert.ToInt32(btnBox.Text.Replace("s", ""));

            pdeGraph.SetDisplayRangeX(0, xAxisValue);
            pdeGraph.SetGridDistanceX(10);
            pdeGraph.Refresh();
            //pdeGraph.Invalidate();           
        }

        private void yAxis_Click(object sender, EventArgs e)
        {
            if (currentYButton != null) currentYButton.BackColor = Button.DefaultBackColor;

            Button btnBox = (Button)sender;

            btnBox.BackColor = Color.LightBlue;
            currentYButton = btnBox;

            int yAxisValue = Convert.ToInt32(btnBox.Text.Replace("V", "").Replace("k", ""));
            if (yAxisValue < 500) yAxisValue = yAxisValue * 1000;
            yAxisValue++;
            graphDS.SetDisplayRangeY(-yAxisValue, yAxisValue);

            if (yAxisValue >= 5000) graphDS.SetGridDistanceY(1000);
            else graphDS.SetGridDistanceY(100);

            pdeGraph.Refresh();

        }

        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            var randomTest = new Random();

            GraphInit();
            pdeGraph.lockMouseMove = false;
            graphDS.UpLimit = 60;
            graphDS.DownLimit = -graphDS.UpLimit;
            AddPoint(0);
            var cnn = 0;
            var type = 1;
            for (var i = 0; i < Common.CnvNullToInt(txtTimeTest.Text); i++)
            {
                if (cnn == 6)
                {
                    cnn = 0;
                    type = -type;
                }
                //float intValue = (float)(type * 59) / 1000;
                //float intValue = (float)randomTest.Next(-100, 100) / 1000;
                float intValue = cnn < 6 && cnn > 2 ? (float)(type * 60) / 1000 : (float)(type * 59) / 1000;
                //float intValue = cnn < 5 && cnn > 3 ? (float)randomTest.Next(-100, 100) / 1000 : (float)60/1000;
                var dts = new DataSample { t = DateTime.Now, strSample = intValue.ToString() };
                DrawValueToGraph(dts);
                cnn++;
                Application.DoEvents();
                Thread.Sleep(100);
            }
            
            //AddPoint(intValue);

            //if (graphDS.Length > pdeGraph.currEndX)
            //{
            //    pdeGraph.UpdateStartingIdx(intValue);
            //}

            //pdeGraph.Refresh();
        }

        private void btnManagement_Click(object sender, EventArgs e)
        {
            using (var frmManagement = new Management())
            {
                frmManagement.ShowDialog();
                if (frmManagement.ChangeSetting)
                {
                    GetInfoDevice();
                }
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            using (var frmMM = new MeasureManagement())
            {
                frmMM.ShowDialog();
            }
        }

        private void timerWalkingTest_Tick(object sender, EventArgs e)
        {
            MeasureEnd();
            timerWalkingTest.Stop();
        }

        #endregion Event Form

        #region Measure

        private void MeasureStart(int intDev)
        {
            if (!GetInputDevice(intDev))
            {
                return;
            }
            
            telnetDSF = new TelnetInterfaceDsf();
            
            try
            {
                if (telnetDSF.ConnectToServer(_deviceCurrent.ipAddress, _deviceCurrent.port))
                {
                    telnetDSF.OnDataReceived += new ClientHandlePacketData(client_OnDataReceived);
                    telnetDSF.ProcessErrors += new ClientHandleWhenHasErrors(client_ProcessErrors);

                    var GraphTitle = lblAlarmTest.Text.Trim();
                    _measureResult = (int)clsDBUltity.emMeasureResult.Normal;

                    if (_MeasureTest == clsDBUltity.emMeasureType.WalkingTest)
                    {
                        _measureResult = (int)clsDBUltity.emMeasureResult.Pass;
                        GraphTitle = lblWalkingTest.Text.Trim();
                    }

                    //Insert measure information
                    _measureId = _DBUltity.InsertMeasure(_deviceCurrent.deviceId,
                                                         (int)_MeasureTest,
                                                         _deviceCurrent.AlarmValue,
                                                         _deviceCurrent.period,
                                                         _deviceCurrent.failLevel,
                                                         _deviceCurrent.samples,
                                                         DateTime.Now);

                    if (_measureId < 1)
                    {
                        Common.ShowMsg(MessageBoxIcon.Error, MSG_ERR_CONNECT_DB);
                        return;
                    }

                    ResetMeasure();

                    GraphInit(GraphTitle + " " + DateTime.Now.ToString("yyyy MMM,dd HH:mm:ss"));
                    pdeGraph.lockMouseMove = false;
                    pdeGraph.drawAlarmLine = _MeasureTest == clsDBUltity.emMeasureType.AlarmTest;
                    pdeGraph.drawMaxPoint = _MeasureTest == clsDBUltity.emMeasureType.WalkingTest;
                    graphDS.UpLimit = _MeasureTest == clsDBUltity.emMeasureType.WalkingTest ? _deviceCurrent.failLevel : _deviceCurrent.AlarmValue;
                    graphDS.DownLimit = -graphDS.UpLimit;
                    dgvMeasure.Rows.Clear();
                    ModeMeasure(_deviceCurrent.deviceType);

                    _flagInsertDetail = true;
                    var threadInsert = new Thread(new ThreadStart(CallInsertData));
                    threadInsert.Start();
                    telnetDSF.Start();
                    
                    if (_MeasureTest == clsDBUltity.emMeasureType.WalkingTest)
                    {
                        timerWalkingTest.Start();
                    }

                    _blnStartMeasure = true;
                } else
                {
                    telnetDSF = null;
                    Common.ShowMsg(MessageBoxIcon.Error, MSG_ERR_CONNECT);
                }
            }
            catch
            {
                Common.ShowMsg(MessageBoxIcon.Error, MSG_ERR_CONNECT);
                return;
            }
        }

        private void MeasureEnd()
        {
            _flagInsertDetail = false;
            _blnStartMeasure = false;
            Disconnect();
            lblReset_Click(lblReset, new EventArgs());

            using (var db = new clsDBUltity(true))
            {
                if (_MeasureTest == clsDBUltity.emMeasureType.WalkingTest)
                {
                    foreach (MaxValue obj in lstMaxValue)
                    {
                        if (!_hasErrDB)
                        {
                            if (!db.InsertMeasureDetailLimit(_deviceCurrent.deviceId, _measureId, obj.datetime, string.Empty, 0, 0, (int)obj.maxValueShow, (int)clsDBUltity.emMeasureResult.Pass))
                            {
                                _hasErrDB = true;
                                _nameErrFileLimit = Common.MEASURE_DETAIL_LIMIT_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                                string content = db.GetInsertMeasureDetailLimitSql(_deviceCurrent.deviceId, _measureId, obj.datetime, string.Empty, 0, 0, (int)obj.maxValueShow, (int)clsDBUltity.emMeasureResult.Pass);
                                WriteFileErrors(_nameErrFileLimit, content);
                                //WriteFileErrors(_nameErrFileDetail, _DBUltity.LastSqlQuery);
                                _numberRecordWriteLimit++;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(_nameErrFileLimit) || _numberRecordWriteDetail == Common.MAX_RECORD_FILE_ERR)
                            {
                                _numberRecordWriteLimit = 0;
                                _nameErrFileLimit = Common.MEASURE_DETAIL_LIMIT_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                            }
                            string content = db.GetInsertMeasureDetailLimitSql(_deviceCurrent.deviceId, _measureId, obj.datetime, string.Empty, 0, 0, (int)obj.maxValueShow, (int)clsDBUltity.emMeasureResult.Pass);
                            WriteFileErrors(_nameErrFileLimit, content);
                            _numberRecordWriteLimit++;
                        }
                    }

                    if ((lstMaxValue.Sum(i => i.maxValue) / lstMaxValue.Count) >= _deviceCurrent.failLevel)
                    {
                        _measureResult = (int)clsDBUltity.emMeasureResult.Pass;
                        lblRstWalking.Text = "FAIL";
                        lblRstWalking.ForeColor = Color.Yellow;
                        lblRstWalking.BackColor = Color.Red;
                        Application.DoEvents();
                    }
                }

                if (!db.UpdateEndTimeMeasure(_measureId, dtsCurrent.t, _measureResult))
                {
                    _hasErrDB = true;
                    WriteFileErrors(Common.MEASURE_NAME_FILE, db.GetUpdateEndTimeMeasure(_measureId, dtsCurrent.t, _measureResult));
                }
            }
            
            ModeMeasure(_deviceCurrent.deviceType, false);
            graphDS.Name += " - " + DateTime.Now.ToString("yyyy MMM,dd HH:mm:ss");
            pdeGraph.lockMouseMove = false;
            pdeGraph.Refresh();
            
            if (_hasErrDB)
            {
                Common.ShowMsg(MessageBoxIcon.Error, MSG_ERR_HAS_DB);
            }
        }

        /// <summary>
        /// Meno      : Change mode measure 
        /// Create by : 2018/09/17 AKB Nguyen Thanh Tung
        /// </summary>
        /// <param name="intDev">DEVICE1/DEVICE2</param>
        /// <param name="isStart"></param>
        private void ModeMeasure(int intDev, bool isStart = true)
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
                btnStopDevice1.Enabled = isStart && intDev == DEVICE1 && _MeasureTest == clsDBUltity.emMeasureType.AlarmTest;
            }

            if (_deviceInfo[DEVICE2].active)
            {
                txtDev2AlarmValue.ReadOnly = isStart;
                txtPeriod2.ReadOnly = isStart;
                txtFailLevel2.ReadOnly = isStart;
                btnDevice2.Enabled = !isStart;
                btnStopDevice2.Enabled = isStart && intDev == DEVICE2 && _MeasureTest == clsDBUltity.emMeasureType.AlarmTest;
                btnWalkingStart2.Enabled = !isStart;
            }
        }

        private string ParseData(string message)
        {
            if (!message.StartsWith("\""))
            {
                message = "\"" + message;
            }

            if (!message.EndsWith("\""))
            {
                message = message + "\"";
            }

            if (message.StartsWith("\"\""))
            {
                message = message.TrimStart('"');
            }

            if (message.EndsWith("\"\""))
            {
                message = message.TrimEnd('"');
            }
            
            return message;
        }

        private void ParseData2(string message)
        {

            message = Regex.Replace(message, @"\t|\n|\r", "");
            if (message == "") return;

            _message = string.IsNullOrEmpty(_message) ? ParseData(message) : _message + ParseData(message);

            if (_message.EndsWith("\""))
            {
                int x;
                if (graphDS.Samples == null) x = 0;
                else x = graphDS.Samples.Count;
                var addDataSample = new DataSample
                {
                    deviceId = _deviceCurrent.deviceId,
                    measureId = _measureId,
                    strSample = _message,
                    t = DateTime.Now,
                    isRaw = true,
                };

                lock (syncObj1)
                {
                    lstSample.Add(addDataSample);
                }

                if (_MeasureTest == clsDBUltity.emMeasureType.WalkingTest)
                {

                    ProcessData(addDataSample, intDataIndex++);
                    dtsCurrent = addDataSample;

                    // Get Value From Sample
                    var strSample = addDataSample.strSample;
                    strSample = strSample.Replace("\"\"", ",");
                    strSample = strSample.Replace("\"", "");

                    var arrValue = !string.IsNullOrEmpty(strSample) ? strSample.Split(',') : null;

                    if (arrValue != null && arrValue.Length > 0)
                    {
                        var temp = arrValue.Select(i => new MaxValue
                        {
                            datetime = addDataSample.t,
                            maxValue = Math.Abs(Common.CnvNullToDouble(i, 0) * Common.RoundValue),
                            maxValueShow = Common.CnvNullToDouble(i, 0) * Common.RoundValue,
                            point = new cPoint() { x = x, y = (float)(Common.CnvNullToDouble(i, 0) * Common.RoundValue) }
                        }).ToList();

                        temp.AddRange(lstMaxValue);
                        temp = temp.OrderByDescending(i => i.maxValue).Take(_deviceCurrent.samples).ToList();
                        lstMaxValue = temp;
                    }
                }

                _message = string.Empty;
            }

            if (_MeasureTest == clsDBUltity.emMeasureType.WalkingTest)
            {
                return;
            }

            DataSample dts = new DataSample();
            string strCurrentSample;
            int index;

            dts.t = DateTime.Now;
            dts.strSample = message;

            if (dtsCurrent.strSample == cstrNothing)
            {
                dtsCurrent = dts;
            }
            
            //proccess the first received data
            if (dtsCurrent.t == dts.t)
            {
                if (!dtsCurrent.strSample.StartsWith("\""))
                {
                    dtsCurrent.strSample = "\"" + dtsCurrent.strSample;
                }

                return;
            }

            //if (dtsCurrent.t.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond) == dts.t.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond))
            //{
                //dtsCurrent.strSample += dts.strSample;
            //}
            if ((dts.t - dtsCurrent.t).TotalMilliseconds < 100)
            {
                dtsCurrent.strSample += dts.strSample;
            }
            else
            {
                strCurrentSample = dtsCurrent.strSample;

                if (ProcessData(dtsCurrent, intDataIndex++))
                {
                    dtsCurrent.strSample = cstrNothing;
                }
                else
                {
                    index = dts.strSample.IndexOf("\"");
                    string strTemp = "";

                    if (index == 0)
                    {
                        dtsCurrent.strSample = dtsCurrent.strSample + "\"";
                        if (ProcessData(dtsCurrent, intDataIndex++)) dtsCurrent.strSample = cstrNothing;
                    }
                    else
                    {
                        strTemp = dts.strSample.Substring(0, index + 1);
                        dtsCurrent.strSample = dtsCurrent.strSample + strTemp;
                        ProcessData(dtsCurrent, intDataIndex++);
                        if (dts.strSample.Length - index - 1 > 0)
                        {
                            dts.strSample = dts.strSample.Substring(index + 1, dts.strSample.Length - index - 1);
                            if (!ProcessData(dts, intDataIndex++))
                            {
                                dtsCurrent = dts;
                            }
                            else
                            {
                                dtsCurrent.strSample = cstrNothing;
                            }
                        }
                        else
                        {
                            dtsCurrent.strSample = cstrNothing;
                        }
                    }
                }
            }
        }

        //This method is called when the client has received data from the server
        private void client_OnDataReceived(byte[] data, int bytesRead)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            string message = encoder.GetString(data, 0, bytesRead);
            
            //SetValueText(message, txtDev1ActualValue);
            //ParseData(message);
            ParseData2(message);

            //Console.WriteLine("Received a message: " + intDataIndex.ToString() + "," + strTime + "," + message);
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
            while (Common.ComfirmMsgErr(COMFIRM_ERR_CONNECT_DB))
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
                        if (_MeasureTest == clsDBUltity.emMeasureType.WalkingTest)
                        {
                            timerWalkingTest.Start();
                        }
                        return;
                    }
                }
                catch (Exception ex)
                {
                }
            }

            MeasureEnd();

            if (_MeasureTest == clsDBUltity.emMeasureType.WalkingTest)
            {
                timerWalkingTest.Stop();
            }
        }

        private bool ProcessData(DataSample dts, int index)
        {

            if (!dts.strSample.EndsWith("\"")) return false;

            if (dts.strSample.EndsWith("\"\""))
            {
                dts.strSample = dts.strSample.TrimEnd('\"');
            }

            string strTime = dts.t.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond);
            //SetGridValue(index, strTime, dts.strSample.Replace("\"\"", ",").Replace("\"", ""));
            //Add Data Sample to grid
            SetGridValue(new object[] { index, strTime, dts.strSample }, dgvMeasure);

            SetValueText(index.ToString(), txtCount);

            //Draw to graph and set actual value
            SetValueText(DrawValueToGraph(dts).ToString("N0"), txtDev1ActualValue);

            return true;
        }

        private string GetDeviceType(string strValue)
        {
            strValue = strValue.Replace(" ", "");

            if (strValue.Length == 5) return "20kV";

            if (strValue.Length == 6)
            {
                int index = strValue.IndexOf(".");
                if (strValue.Length - index == 4) return "2kV";
                if (strValue.Length - index == 3) return "20kV";
            }

            return "DEVICE ERROR";
        }

        private string GetDeviceType(string[] strValue)
        {
            string strRet = "DEVICE ERROR";
            if (strValue.Length > 0)
            {
                int i = 0;
                do
                {
                    strRet = GetDeviceType(strValue[i]);
                    i++;

                } while (strRet == "DEVICE ERROR" && i < strValue.Length);
            }
            return strRet;
        }

        private double DrawValueToGraph(DataSample dts)
        {
            double dblMax;
            double dblMin;
            double dblDelegate;
            double dblValueGraph;
            string strValue;
            string[] arrValue;

            try
            {

                GetDataFromSample(dts.strSample,out arrValue, out strValue, out dblMax, out dblMin, out dblDelegate);
                SetValueLabel(GetDeviceType(arrValue), lblDeviceType);
                dblValueGraph = dblDelegate;
                var result = (int)clsDBUltity.emMeasureResult.Pass;
                if (_MeasureTest == clsDBUltity.emMeasureType.AlarmTest)
                {
                    result = (int)clsDBUltity.emMeasureResult.Normal;

                    if (_deviceCurrent.AlarmValue <= (int)dblDelegate)
                    {
                        result = (int)clsDBUltity.emMeasureResult.Alarm;
                        _measureResult = result;
                    }
                    // Actual Value > Alarm Value
                    if (Math.Abs(dblDelegate) > graphDS.UpLimit)
                    {
                        dblValueGraph = (dblDelegate / Math.Abs(dblDelegate)) * graphDS.UpLimit;
                        SetValueLabel(dblDelegate.ToString(), lblOverViewValue);
                        blnAlarm = true;
                        Blink();
                    }
                } else
                {
                    graphDS.maxPoint = lstMaxValue.Select(i => i.point).ToArray();
                }

                lock (syncObj1)
                {
                    lstSample.Add(new DataSample
                    {
                        deviceId = _deviceCurrent.deviceId,
                        measureId = _measureId,
                        t = dts.t,
                        actualValue = strValue,
                        actualMaxValue = (int)dblMax,
                        actualMinValue = (int)dblMin,
                        actualDelegate = (int)dblDelegate,
                        result = result,
                        isRaw = false,
                    });
                }

                //InsertMeasureDetail(dts.t, strValue, (int)dblMax, (int)dblMin, (int)dblDelegate, result);

                // Actual Value > Value Screen
                //if ((graphDS.Length - pdeGraph.StartingIndexOff + pdeGraph.MoveMouse) > pdeGraph.endX)
                ////+ (pdeGraph.MoveMouse > 0 ? 0 : Math.Abs(pdeGraph.MoveMouse)))
                //{
                //    float dx = (pdeGraph.endX - pdeGraph.startX);
                //    //int dxMod = graphDS.Length % (int)dx;

                //    pdeGraph.SetStartingIdx((int)(graphDS.Length - dx - pdeGraph.StartingIndexOff - 1));

                //}
                //else
                //{
                //    //pdeGraph.UpdateStartingIdx(-1);
                //}
                AddPoint((float)dblValueGraph);
                
                if ((graphDS.Length - pdeGraph.starting_idx) >= (int)pdeGraph.endX)
                {
                    float dx = (pdeGraph.endX - pdeGraph.startX);
                    //int dxMod = graphDS.Length % (int)dx;

                    //pdeGraph.SetStartingIdx((int)(graphDS.Length - dx - pdeGraph.StartingIndexOff - 1));
                    //pdeGraph.starting_idx = (int)Math.Ceiling(graphDS.Length - dx - pdeGraph.StartingIndexOff - 1);
                    pdeGraph.SetStartingIdx((int)(graphDS.Length - dx - (Math.Abs(pdeGraph.MoveMouse) > 0 ? pdeGraph.MoveMouse - 1 : 0)));
                }

                RefreshGraph();

                return dblDelegate;

                #region Code Old
                //string strSample = dts.strSample;
                //strSample = strSample.Replace("\"\"", ",");
                //strSample = strSample.Replace("\"", "");

                //string[] strValue = strSample.Split(',');
                //SetValueLabel(GetDeviceType(strValue), lblDeviceType);

                //double[] dblValue = strValue.Select(n => Common.CnvNullToDouble(n, 0) * Common.RoundValue).ToArray();
                //double dblMax = dblValue.Max();
                //double dblMin = dblValue.Min();
                //var dbRstValue = Math.Abs(dblMax) >= Math.Abs(dblMin) ? dblMax : dblMin;
                //_DBUltity.InsertMeasureDetail(_deviceCurrent.deviceId, _measureId, dts.t, string.Join(",", dblValue), (int)dblMax, (int)dblMin, (int)dbRstValue);

                ////Temporary code to mutiple 1000 to see
                ////dblMax = dblMax * Common.RoundValue;
                ////dblMin = dblMin * Common.RoundValue;

                //if (dblMin < graphDS.DownLimit || dblMax > graphDS.UpLimit)
                //{
                //    blnAlarm = true;
                //    Blink();
                //    if (dblMin < graphDS.DownLimit)
                //        AddPoint((float)dblMin);
                //    else
                //    {
                //        AddPoint((float)dblMax);
                //    }
                //}
                //else
                //{
                //    var dbRstVlue = Math.Abs(dblMax) >= Math.Abs(dblMin) ? dblMax : dblMin;
                //    AddPoint((float)dbRstVlue);
                //}

                //if (graphDS.Length > pdeGraph.endX)
                //{
                //    float dx = (pdeGraph.endX - pdeGraph.startX);
                //    int dxMod = graphDS.Length % (int)dx;

                //    pdeGraph.SetStartingIdx((int)(graphDS.Length - dx));
                //}

                //RefreshGraph();
                ////panelGraph.Refresh();

                //return dblMax;
                #endregion Code Old
            }
            catch (Exception ex)
            {
                //MessageBox.Show(dts.strSample);
                //MessageBox.Show(ex.ToString());
                return 0;
            }
        }

        private void Disconnect()
        {
            if (telnetDSF == null) return;
            //if (telnetDSF.IsConnected)
            //{

            //    telnetDSF.Disconnect();
            //}
            telnetDSF.Disconnect();
            //telnetDSF = null;
        }

        private void GetDataFromSample(string strSample,out string[] arrValue, out string strValue, out double dblMax, out double dblMin, out double dblDelegate)
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
            double[] dblValue = arrValue.Select(n => Common.CnvNullToDouble(n.Trim(), 0) * Common.RoundValue).ToArray();

            strValue = string.Join(",", dblValue);
            dblMax = dblValue.Max();
            dblMin = dblValue.Min();
            dblDelegate = Math.Abs(dblMax) >= Math.Abs(dblMin) ? dblMax : dblMin;
        }
        
        private void WriteFileErrors(string fileName, string content)
        {
            string path = Common.PathDataErrors + @"\" + _measureId;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path += @"\" + _measureId + "_" + fileName + @".txt";

            if (!File.Exists(path))
            {
                using (var sw = File.CreateText(path))
                {
                    sw.WriteLine(content);
                    sw.Close();
                }
            } else
            {
                using (var sw = new StreamWriter(path, true))
                {
                    sw.WriteLine(content);
                    sw.Close();
                }
            }
        }

        private void ResetMeasure()
        {
            lstMaxValue.Clear();
            blnAlarm = false;
            intDataIndex = 0;
            lblTestDeviceType.Text = string.Empty;
            lblOverViewValue.Text = string.Empty;
            lblRstWalking.Text = "PASS";
            lblRstWalking.ForeColor = Color.Blue;
            lblRstWalking.BackColor = Color.Lime;
            lblAlarmTest.BackColor = alarmLabelColor;
            dtsCurrent = new DataSample();
            dtsCurrent.strSample = cstrNothing;

            _hasErrDB = false;
            _nameErrFileDetail = string.Empty;
            _nameErrFileLimit = string.Empty;
            _nameErrFileRaw = string.Empty;
            _numberRecordWriteDetail = 0;
            _numberRecordWriteLimit = 0;
            _numberRecordWriteRaw = 0;
        }

        #endregion Measure

        #region Graph

        private void GraphInit(string graphName = "GRAPH")
        {
            //this.SuspendLayout();

            pdeGraph.DataSources.Clear();
            pdeGraph.SetDisplayRangeX(0, 30);
            pdeGraph.SetGridDistanceX(10);
            
            pdeGraph.DataSources.Add(new DataSource());

            graphDS = pdeGraph.DataSources[intDataSrcIndex];

            graphDS.Name = graphName;
            graphDS.OnRenderXAxisLabel += RenderXLabel;

            //pdeGraph.DataSources[j].Length = 1;
            //pdeGraph.Refresh();

            pdeGraph.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;
            pdeGraph.SetPlayPanelInvisible();
            graphDS.AutoScaleY = false;
            graphDS.SetDisplayRangeY(-501, 501);
            graphDS.SetGridDistanceY(100);
            graphDS.OnRenderYAxisLabel = RenderYLabel;
            graphDS.UpLimit = 10;
            graphDS.DownLimit = -10;
            //CalcSinusFunction_0(pdeGraph.DataSources[j], j);
            pdeGraph.StartingIndexOff = 0;
            pdeGraph.SetStartingIdx(0);
            AddPoint(0);

            UpdateColorSchemaMenu();
            RefreshGraph();
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

        private String RenderXLabel(DataSource s, int idx)
        {
            if (s.AutoScaleX)
            {
                //if (idx % 2 == 0)
                {
                    int Value = (int)(s.Samples[idx].x);
                    return "" + Value;
                }
            }
            else
            {
                //int Value = (int)(s.Samples[idx].x / 500);
                int Value = (int)(s.Samples[idx].x);
                String Label = "" + Value;
                return Label;
            }
        }

        private String RenderYLabel(DataSource s, float value)
        {
            return String.Format("{0:0.0}", value);
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

        #region InsertDB 

        private void CallInsertData()
        {
            using (var db = new clsDBUltity(true))
            {
                while (_flagInsertDetail || lstSample.Count > 0)
                {
                    if (lstSample.Count > 0)
                    {
                        var detail = lstSample[0];
                    
                        if (detail.isRaw)
                        {
                            InsertMeasureRaw(detail, db);
                        }
                        else
                        {
                            if (detail.result == (int)clsDBUltity.emMeasureResult.Alarm)
                            {
                                InsertMeasureLimit(detail, db);
                            }

                            InsertMeasureDetail(detail, db);
                        }

                        lock (syncObj1)
                        {
                            lstSample.RemoveAt(0);
                        }
                    }
                    Thread.Sleep(33);
                }
            }
        }
        
        private void InsertMeasureDetail(DataSample data, clsDBUltity db)
        {
            if (string.IsNullOrEmpty(data.actualValue))
            {
                return;
            }

            if (!_hasErrDB)
            {
                if (!db.InsertMeasureDetail(data.deviceId, data.measureId, data.t, data.actualValue, data.actualMaxValue, data.actualMinValue, data.actualDelegate, data.result))
                {
                    _hasErrDB = true;
                    _nameErrFileDetail = Common.MEASURE_DETAIL_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                    string content = db.GetInsertMeasureDetailSql(data.deviceId, data.measureId, data.t, data.actualValue, data.actualMaxValue, data.actualMinValue, data.actualDelegate, data.result);
                    //WriteFileErrors(_nameErrFileDetail, _DBUltityDetail.LastSqlQuery);
                    WriteFileErrors(_nameErrFileDetail, content);
                    _numberRecordWriteDetail++;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_nameErrFileDetail) || _numberRecordWriteDetail == Common.MAX_RECORD_FILE_ERR)
                {
                    _numberRecordWriteDetail = 0;
                    _nameErrFileDetail = Common.MEASURE_DETAIL_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                }
                string content = db.GetInsertMeasureDetailSql(data.deviceId, data.measureId, data.t, data.actualValue, data.actualMaxValue, data.actualMinValue, data.actualDelegate, data.result);
                WriteFileErrors(_nameErrFileDetail, content);
                _numberRecordWriteDetail++;
            }
        }

        private void InsertMeasureRaw(DataSample data, clsDBUltity db)
        {
            if (string.IsNullOrEmpty(data.strSample))
            {
                return;
            }

            double dblMax;
            double dblMin;
            double dblDelegate;
            string strValue;
            string[] arrValue;
            var result = (int)clsDBUltity.emMeasureResult.Pass;

            GetDataFromSample(data.strSample, out arrValue, out strValue, out dblMax, out dblMin, out dblDelegate);

            if (_MeasureTest == clsDBUltity.emMeasureType.AlarmTest)
            {
                result = _deviceCurrent.AlarmValue > (int)dblDelegate ? (int)clsDBUltity.emMeasureResult.Normal : (int)clsDBUltity.emMeasureResult.Alarm;
            }
            //else
            //{
            //    if (arrValue.Length > 0)
            //    {
            //        var temp = arrValue.Select(i => new MaxValue
            //        {
            //            datetime = data.t,
            //            maxValue = Math.Abs(Common.CnvNullToDouble(i, 0) * Common.RoundValue),
            //            maxValueShow = Common.CnvNullToDouble(i, 0) * Common.RoundValue,
            //        }).ToList();

            //        temp.AddRange(lstMaxValue);
            //        temp = temp.OrderByDescending(i => i.maxValue).Take(_deviceCurrent.samples).ToList();
            //        lstMaxValue = temp;
            //    }
            //}

            if (!_hasErrDB)
            {
                if (!db.InsertMeasureDetailRaw(data.deviceId, data.measureId, data.t, strValue, (int)dblMax, (int)dblMin, (int)dblDelegate, result))
                {
                    _hasErrDB = true;
                    _nameErrFileRaw = Common.MEASURE_DETAIL_RAW_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                    string content = db.GetInsertMeasureDetailRawSql(data.deviceId, data.measureId, data.t, strValue, (int)dblMax, (int)dblMin, (int)dblDelegate, result);
                    WriteFileErrors(_nameErrFileRaw, content);
                    _numberRecordWriteRaw++;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_nameErrFileRaw) || _numberRecordWriteRaw == Common.MAX_RECORD_FILE_ERR)
                {
                    _numberRecordWriteRaw = 0;
                    _nameErrFileRaw = Common.MEASURE_DETAIL_RAW_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                }
                string content = db.GetInsertMeasureDetailRawSql(data.deviceId, data.measureId, data.t, strValue, (int)dblMax, (int)dblMin, (int)dblDelegate, result);
                WriteFileErrors(_nameErrFileRaw, content);
                _numberRecordWriteRaw++;
            }
        }

        private void InsertMeasureLimit(DataSample data, clsDBUltity db)
        {
            if (string.IsNullOrEmpty(data.actualValue))
            {
                return;
            }

            if (!_hasErrDB)
            {
                if (!db.InsertMeasureDetailLimit(data.deviceId, data.measureId, data.t, data.actualValue, data.actualMaxValue, data.actualMinValue, data.actualDelegate, data.result))
                {
                    _hasErrDB = true;
                    _nameErrFileLimit = Common.MEASURE_DETAIL_LIMIT_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                    string content = db.GetInsertMeasureDetailLimitSql(data.deviceId, data.measureId, data.t, data.actualValue, data.actualMaxValue, data.actualMinValue, data.actualDelegate, data.result);
                    WriteFileErrors(_nameErrFileLimit, content);
                    _numberRecordWriteLimit++;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_nameErrFileLimit) || _numberRecordWriteLimit == Common.MAX_RECORD_FILE_ERR)
                {
                    _numberRecordWriteLimit = 0;
                    _nameErrFileLimit = Common.MEASURE_DETAIL_LIMIT_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatNoMiliSecond2);
                }
                string content = db.GetInsertMeasureDetailLimitSql(data.deviceId, data.measureId, data.t, data.actualValue, data.actualMaxValue, data.actualMinValue, data.actualDelegate, data.result);
                WriteFileErrors(_nameErrFileLimit, content);
                _numberRecordWriteLimit++;
            }
        }

        #endregion

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

            //pdeGraph.Refresh();
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
        /// Meno      : Get Info Device
        /// </summary>
        private void GetInfoDevice()
        {
            txtDev1ActualValue.Text = string.Empty;
            txtDev1AlarmValue.Text = string.Empty;
            txtDev2ActualValue.Text = string.Empty;
            txtDev2AlarmValue.Text = string.Empty;
            txtPeriod.Text = string.Empty;
            txtFailLevel.Text = string.Empty;

            btnDevice1.Enabled = false;
            btnStopDevice1.Enabled = false;
            btnDevice2.Enabled = false;
            btnStopDevice2.Enabled = false;

            btnWalkingStart.Enabled = false;
            btnWalkingStart2.Enabled = false;
            
            using (var objDb = new clsDBUltity())
            {
                var deviceInfo = objDb.GetDeviceList(false);

                if (Common.TableIsNullOrEmpty(deviceInfo))
                {
                    return;
                }

                foreach (DataRow row in deviceInfo.Rows)
                {
                    var deviceActive = Common.CnvNullToInt(row["active"]);
                    var deviceType = Common.CnvNullToInt(row["device_type"]);

                    if (deviceType == (int)clsDBUltity.emDeviceType.Device1)
                    {
                        _deviceInfo[DEVICE1].deviceId = Common.CnvNullToInt(row["device_id"]);
                        _deviceInfo[DEVICE1].deviceType = DEVICE1;
                        _deviceInfo[DEVICE1].ipAddress = Common.CnvNullToString(row["ip_address"]).Trim();
                        _deviceInfo[DEVICE1].port = Common.CnvNullToInt(row["port"]);
                        _deviceInfo[DEVICE1].AlarmValue = Common.CnvNullToInt(row["alarm_value"]);
                        _deviceInfo[DEVICE1].period = Common.CnvNullToInt(row["period"]);
                        _deviceInfo[DEVICE1].failLevel = Common.CnvNullToInt(row["fail_level"]);
                        _deviceInfo[DEVICE1].samples = Common.CnvNullToInt(row["samples"]);
                        _deviceInfo[DEVICE1].active = deviceActive > 0 && !string.IsNullOrEmpty(_deviceInfo[DEVICE1].ipAddress);

                        _deviceInfo[DEVICE2].period = _deviceInfo[DEVICE1].period;
                        _deviceInfo[DEVICE2].failLevel = _deviceInfo[DEVICE1].failLevel;

                        txtDev1AlarmValue.Text = _deviceInfo[DEVICE1].AlarmValue > 0 ? _deviceInfo[DEVICE1].AlarmValue.ToString() : string.Empty;
                        txtDev1AlarmValue.ReadOnly = !_deviceInfo[DEVICE1].active;
                        txtPeriod.Text = _deviceInfo[DEVICE1].period > 0 ? _deviceInfo[DEVICE1].period.ToString() : string.Empty;
                        txtFailLevel.Text = _deviceInfo[DEVICE1].failLevel > 0 ? _deviceInfo[DEVICE1].failLevel.ToString() : string.Empty;
                        txtPeriod.ReadOnly = !_deviceInfo[DEVICE1].active;
                        txtFailLevel.ReadOnly = !_deviceInfo[DEVICE1].active;
                        btnDevice1.Enabled = _deviceInfo[DEVICE1].active;
                        btnWalkingStart.Enabled = _deviceInfo[DEVICE1].active;
                    }

                    if (deviceType == (int)clsDBUltity.emDeviceType.Device2)
                    {
                        _deviceInfo[DEVICE2].deviceId = Common.CnvNullToInt(row["device_id"]);
                        _deviceInfo[DEVICE2].deviceType = DEVICE2;
                        _deviceInfo[DEVICE2].ipAddress = Common.CnvNullToString(row["ip_address"]).Trim();
                        _deviceInfo[DEVICE2].port = Common.CnvNullToInt(row["port"]);
                        _deviceInfo[DEVICE2].AlarmValue = Common.CnvNullToInt(row["alarm_value"]);
                        _deviceInfo[DEVICE2].samples = Common.CnvNullToInt(row["samples"]);
                        _deviceInfo[DEVICE2].active = deviceActive > 0 && !string.IsNullOrEmpty(_deviceInfo[DEVICE2].ipAddress);

                        txtDev2AlarmValue.ReadOnly = !_deviceInfo[DEVICE2].active;
                        txtDev2AlarmValue.Text = _deviceInfo[DEVICE2].AlarmValue > 0 ? _deviceInfo[DEVICE2].AlarmValue.ToString() : string.Empty;
                        txtPeriod2.Text = _deviceInfo[DEVICE2].period > 0 ? _deviceInfo[DEVICE2].period.ToString() : string.Empty;
                        txtFailLevel2.Text = _deviceInfo[DEVICE2].failLevel > 0 ? _deviceInfo[DEVICE2].failLevel.ToString() : string.Empty;
                        txtFailLevel2.ReadOnly = !_deviceInfo[DEVICE2].active;
                        txtPeriod2.ReadOnly = !_deviceInfo[DEVICE2].active;
                        btnDevice2.Enabled = _deviceInfo[DEVICE2].active;
                        btnWalkingStart2.Enabled = _deviceInfo[DEVICE2].active;
                    }
                }
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

            _deviceInfo[intDev].AlarmValue = Common.CnvNullToInt((intDev == DEVICE1 ? txtDev1AlarmValue : txtDev2AlarmValue).Text);
            _deviceInfo[intDev].period = Common.CnvNullToInt(intDev == DEVICE1 ? txtPeriod.Text : txtPeriod2.Text);
            _deviceInfo[intDev].failLevel = Common.CnvNullToInt(intDev == DEVICE1 ? txtFailLevel.Text : txtFailLevel2.Text);
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
            if (_MeasureTest == clsDBUltity.emMeasureType.AlarmTest)
            {
                var txtAlarmValue = (intDev == DEVICE1 ? txtDev1AlarmValue : txtDev2AlarmValue);

                if (!Common.IsNumberAndThanZero(txtAlarmValue.Text))
                {
                    txtAlarmValue.Focus();
                    return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Alarm Value"));
                }
            } else
            {
                if (!Common.IsNumberAndThanZero(txtPeriod.Text))
                {
                    txtPeriod.Focus();
                    return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Period"));
                }

                if (!Common.IsNumberAndThanZero(txtFailLevel.Text))
                {
                    txtFailLevel.Focus();
                    return Common.ShowMsg(MessageBoxIcon.Warning, string.Format(FOMAT_MSG_INPUT_NUMBER, "Fail Level"));
                }
            }

            return true;
        }

        #endregion Private Function

        #region Remove Function

        //private void InsertMeasureRaw(string message, DateTime samplesTime)
        //{
        //    double dblMax;
        //    double dblMin;
        //    double dblDelegate;
        //    string strValue;
        //    string[] arrValue;
        //    var result = (int)clsDBUltity.emMeasureResult.Pass;

        //    GetDataFromSample(message, out arrValue, out strValue, out dblMax, out dblMin, out dblDelegate);

        //    if (_MeasureTest == clsDBUltity.emMeasureType.AlarmTest)
        //    {
        //        result = _deviceCurrent.AlarmValue > (int)dblDelegate ? (int)clsDBUltity.emMeasureResult.Normal : (int)clsDBUltity.emMeasureResult.Alarm;
        //    }
        //    else
        //    {
        //        if (lstMaxValue.Count < _deviceCurrent.samples)
        //        {
        //            var temp = arrValue.Select(i => Math.Abs(Common.CnvNullToDouble(i, 0) * Common.RoundValue)).ToArray();
        //            temp = temp.OrderByDescending(i => i).Take(5).ToArray();
        //            foreach (double max in temp)
        //            {
        //                lstMaxValue.Add(new MaxValue { datetime = samplesTime, maxvalue = max });
        //            }
        //        }
        //        else
        //        {
        //            var temp = new List<string>();
        //            temp.AddRange(arrValue);
        //            temp.AddRange(lstMaxValue.Select(i => i.maxvalue.ToString()).ToArray());
        //            var temp2 = arrValue.Select(i => Math.Abs(Common.CnvNullToDouble(i, 0) * Common.RoundValue)).ToArray();
        //            temp2 = temp2.OrderByDescending(i => i).Take(5).ToArray();
        //            lstMaxValue.Clear();
        //            foreach (double max in temp2)
        //            {
        //                lstMaxValue.Add(new MaxValue { datetime = samplesTime, maxvalue = max });
        //            }
        //        }
        //    }

        //    var taskInsertRaw = new Thread(() => {

        //        if (!_hasErrDB)
        //        {
        //            if (!_DBUltityRaw.InsertMeasureDetailRaw(_deviceCurrent.deviceId, _measureId, samplesTime, strValue, (int)dblMax, (int)dblMin, (int)dblDelegate, result))
        //            {
        //                _hasErrDB = true;
        //                lock (syncObj3)
        //                {
        //                    _nameErrFileRaw = Common.MEASURE_DETAIL_RAW_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatMiliSecond);
        //                    string content = _DBUltityRaw.GetInsertMeasureDetailRawSql(_deviceCurrent.deviceId, _measureId, samplesTime, strValue, (int)dblMax, (int)dblMin, (int)dblDelegate, result);
        //                    WriteFileErrors(_nameErrFileRaw, content);
        //                    //WriteFileErrors(_nameErrFileRaw, _DBUltityRaw.LastSqlQuery);
        //                    _numberRecordWriteRaw++;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            lock (syncObj3)
        //            {
        //                if (string.IsNullOrEmpty(_nameErrFileRaw) || _numberRecordWriteRaw == Common.MAX_RECORD_FILE_ERR)
        //                {
        //                    _numberRecordWriteRaw = 0;
        //                    _nameErrFileRaw = Common.MEASURE_DETAIL_RAW_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatMiliSecond);
        //                }
        //                string content = _DBUltityRaw.GetInsertMeasureDetailRawSql(_deviceCurrent.deviceId, _measureId, samplesTime, strValue, (int)dblMax, (int)dblMin, (int)dblDelegate, result);
        //                WriteFileErrors(_nameErrFileRaw, content);
        //                _numberRecordWriteRaw++;
        //            }
        //        }

        //        Thread.Sleep(1000);
        //    });

        //    taskInsertRaw.Start();
        //}

        //private void InsertMeasureDetail(DateTime samplesTime, string actualValue, int actualMaxValue, int actualMinValue, int actualDelegate, int result)
        //{
        //    var taskInsertDetail = new Thread(() => {

        //        try
        //        {
        //            if (result == (int)clsDBUltity.emMeasureResult.Alarm)
        //            {
        //                InsertMeasureLimit(samplesTime, actualValue, actualMaxValue, actualMinValue, actualDelegate, result);
        //            }

        //            if (!_hasErrDB)
        //            {
        //                if (!_DBUltityDetail.InsertMeasureDetail(_deviceCurrent.deviceId, _measureId, samplesTime, actualValue, actualMaxValue, actualMinValue, actualDelegate, result))
        //                {
        //                    _hasErrDB = true;
        //                    lock (syncObj1)
        //                    {
        //                        _nameErrFileDetail = Common.MEASURE_DETAIL_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatMiliSecond);
        //                        string content = _DBUltityDetail.GetInsertMeasureDetailSql(_deviceCurrent.deviceId, _measureId, samplesTime, actualValue, actualMaxValue, actualMinValue, actualDelegate, result);
        //                        //WriteFileErrors(_nameErrFileDetail, _DBUltityDetail.LastSqlQuery);
        //                        WriteFileErrors(_nameErrFileDetail, content);
        //                        _numberRecordWriteDetail++;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                lock (syncObj1)
        //                {
        //                    if (string.IsNullOrEmpty(_nameErrFileDetail) || _numberRecordWriteDetail == Common.MAX_RECORD_FILE_ERR)
        //                    {
        //                        _numberRecordWriteDetail = 0;
        //                        _nameErrFileDetail = Common.MEASURE_DETAIL_NAME_FILE + DateTime.Now.ToString(clsDBUltity.cstrDateTimeFormatMiliSecond);
        //                    }
        //                    string content = _DBUltityDetail.GetInsertMeasureDetailSql(_deviceCurrent.deviceId, _measureId, samplesTime, actualValue, actualMaxValue, actualMinValue, actualDelegate, result);
        //                    WriteFileErrors(_nameErrFileDetail, content);
        //                    _numberRecordWriteDetail++;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("InsertMeasureDetail" + ex.Message);
        //        }

        //        Thread.Sleep(100);
        //    });

        //    taskInsertDetail.Start();
        //}

        //private void ParseData(string message)
        //{
        //    message = Regex.Replace(message, @"\t|\n|\r", "");
        //    if (message == "") return;

        //    DataSample dts = new DataSample();
        //    dts.t = DateTime.Now;
        //    dts.strSample = message;
        //    lstSample.Add(dts);

        //    while (lstSample.Count > 0)
        //    {
        //        dts = lstSample[0];
        //        char lastChar = dts.strSample[dts.strSample.Length - 1];

        //        if (lastChar == '\"')
        //        {
        //            ProcessData(dts, intDataIndex++);
        //            lstSample.RemoveAt(0);
        //        }
        //        else
        //        {

        //            string strTemp = "";
        //            int index = dts.strSample.LastIndexOf("\"");

        //            if (index >= 0)
        //            {
        //                if (index == 0)
        //                {
        //                    strTemp = dts.strSample;
        //                }
        //                else
        //                {
        //                    strTemp = dts.strSample.Substring(index, dts.strSample.Length - index);
        //                    dts.strSample = dts.strSample.Substring(0, index);
        //                    ProcessData(dts, intDataIndex++);
        //                }

        //                lstSample.RemoveAt(0);
        //                if (lstSample.Count >= 1)
        //                {
        //                    DataSample dtsTemp = lstSample[0];
        //                    dtsTemp.strSample = strTemp + dtsTemp.strSample;
        //                    lstSample[0] = dtsTemp;
        //                }
        //                else
        //                {
        //                    //strTemp is exceptinal value, need to process or not?
        //                }
        //            }
        //        }
        //    }
        //}

        //protected void CalcSinusFunction_0(DataSource src, int idx)
        //{
        //    for (int i = 0; i < src.Length; i++)
        //    {
        //        src.Samples[i].setX(i);

        //        src.Samples[i].setY(rd.Next(-1000, 1000) / src.Length);

        //        //(float)(((float)200 * Math.Sin((idx + 1) * (i + 1.0) * 48 / src.Length)));
        //    }
        //}

        #endregion Remove Function

        private void lblAlarmTest_Click(object sender, EventArgs e)
        {
            blnAlarm = false;
        }
    }
}
