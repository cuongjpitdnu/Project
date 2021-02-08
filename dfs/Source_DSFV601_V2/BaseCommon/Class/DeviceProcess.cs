using GraphLib;
using KeyCipher;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BaseCommon.clsConst;

namespace BaseCommon.Class
{
    public class DeviceProcess : BaseForm
    {
        #region const

        private const string MSG_EXIT_APP = "Measure is starting";
        private const string MSG_ERR_CONNECT = "Cannot connect to {0}, please check network or connection information!";
        private const string MSG_ERR_INPUT_NUMBER = "Please input {0} is a number than 0.";
        private const string COMFIRM_ERR_CONNECT_DEVICE = "Cannot connect to this device, please check network or connection information!\n Do you want re connect?";
        private const string DEVICE_ERROR = "DEVICE ERROR";
        private const string MSG_ERR_DEVICE_NOT_REGIS = "Device: {0} isn't registed!";

        private const int DEVICE1_INDEX = 1;
        private const int DEVICE2_INDEX = 2;
        private const int DEVICE3_INDEX = 3;
        private const int DEVICE4_INDEX = 4;
        private const int DEVICE5_INDEX = 5;

        #endregion

        #region variables

        private bool isThreadInsertRunning = true;
        private TelnetInterfaceDsf telnetDSF = null;
        public bool IsRunning { get; set; } = false;
        public GraphForm MainForm { get; set; }
        public DeviceInfo Device { get; set; }
        public emMeasureType MeasureType { get; set; } = emMeasureType.AlarmTest;

        //only one graph of device is showed
        public bool IsRenderForm { get; set; } = false;
        private string message = string.Empty;
        private readonly object syncObj = new object();
        private List<MaxValue> lstMaxValue = new List<MaxValue>();
        private List<DataSample> lstSample = new List<DataSample>();
        private DataSample dtsCurrent;
        private DateTime? measureEndTime;
        private bool isMeasureUp = true;
        private int maxKey = 0;
        private int beforeActualDelegate = 0;
        private int measureResult;
        public int maxValue { get; set; } = 0;
        private bool isFirstMax = false;
        private Thread threadInsert = null;
        private Thread thread;
        private System.Timers.Timer timerWalkingTest;
        public List<float> LstPoint { get; set; }

        public string StartDate { get; set; } = "";
        public string GraphTitle { get; set; } = "";

        private int _measureId;
        private string _pathFolderData;

        #endregion

        #region functions

        public DeviceProcess()
        {
            this.timerWalkingTest = new System.Timers.Timer();
            this.timerWalkingTest.Interval = 1000;
            this.timerWalkingTest.Elapsed += this.MeasureEnd;

            LstPoint = new List<float>();
        }


        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            try
            {
                maxValue = 0;
                Device.IsFirstAlarm = false;
                isFirstMax = false;
                isThreadInsertRunning = true;
                thread = new Thread(new ThreadStart(MeasureStart));
                thread.Start();
            }
            catch (Exception e)
            {
                thread.Abort();
                throw e;
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            MeasureEnd(null, null);
        }

        /// <summary>
        /// Measures the start.
        /// </summary>
        public void MeasureStart()
        {

            if (!validate())
            {
                MainForm.BeginInvoke((Action)(() =>
                {
                    if (MeasureType == emMeasureType.AlarmTest)
                    {
                        MainForm.DeviceCallBack(this);
                    }
                    else if (MeasureType == emMeasureType.WalkingTest)
                    {
                        MainForm.ResetWalkingTestMode(false);
                    }
                }));

                return;
            }

            GraphTitle = MainForm.GetAlarmTestTextValue() + "_" + Device.DeviceName + " : ";
            telnetDSF = new TelnetInterfaceDsf();
            measureResult = (int)emMeasureResult.Normal;

            if (MeasureType == emMeasureType.WalkingTest)
            {
                measureResult = (int)emMeasureResult.Pass;
                GraphTitle = MainForm.GetWalkingTestTextValue() + "_" + Device.DeviceName + " : ";
            }

            try
            {
                if (telnetDSF.ConnectToServer(Device.IpAddress, Device.Port))
                {
                    Device.MeasureType = this.MeasureType;

                    telnetDSF.OnDataReceived += new ClientHandlePacketData(client_OnDataReceived);
                    telnetDSF.ProcessErrors += new ClientHandleWhenHasErrors(client_ProcessErrors);

                    lstMaxValue.Clear();
                    dtsCurrent = new DataSample() { isFrist = true };
                    message = string.Empty;
                    isMeasureUp = true;
                    maxKey = 0;
                    beforeActualDelegate = 0;
                    MainForm.BeginInvoke((Action)(() =>
                    {
                        MainForm.ResetMeasure();
                    }));

                    MainForm.BeginInvoke((Action)(() =>
                    {
                        MainForm.ModeMeasure(Device.DeviceId, true);
                    }));

                    MainForm.BeginInvoke((Action)(() =>
                    {
                        StartDate = DateTime.Now.ToString(cstrDateTimeFomatShow);
                        MainForm.GraphInit(GraphTitle + " " + StartDate, MeasureType,
                                MeasureType == emMeasureType.WalkingTest ? Device.FailLevel : Device.AlarmValue);
                    }));

                    Response resStart = MainForm.iSetting.MeasureStart(Device, measureResult);
                    _pathFolderData = resStart.PathFolderData;
                    _measureId = resStart.MeasureId;

                    Device.MeasureId = _measureId;
                    Device.PathFolderData = _pathFolderData;

                    threadInsert = new Thread(new ThreadStart(CallProcessData));
                    threadInsert.Start();

                    telnetDSF.Start();

                    if (MeasureType == emMeasureType.WalkingTest)
                    {
                        timerWalkingTest.Interval = Math.Abs(Device.Period * 1000);
                        timerWalkingTest.Start();
                    }

                    IsRunning = true;
                }
                else
                {
                    telnetDSF = null;
                    ShowMsg(MessageBoxIcon.Error, string.Format(MSG_ERR_CONNECT, Device.DeviceName));
                }
            }
            catch (Exception e)
            {
                isThreadInsertRunning = false;
                if (threadInsert != null)
                {
                    threadInsert.Abort();
                }
                threadInsert = null;
                telnetDSF = null;
                ShowMsg(MessageBoxIcon.Error, string.Format(MSG_ERR_CONNECT, Device.DeviceName));
                throw e;
            }
            finally
            {
                MainForm.BeginInvoke((Action)(() =>
                {
                    if (MeasureType == emMeasureType.AlarmTest)
                    {
                        MainForm.DeviceCallBack(this);
                    }
                    else
                    {
                        MainForm.ResetWalkingTestMode(true);
                    }
                }));
            }
        }

        private void updateData()
        {
            lock (syncObj)
            {
                measureEndTime = dtsCurrent.t;

                if (MeasureType == emMeasureType.WalkingTest)
                {
                    MainForm.BeginInvoke((Action)(() =>
                    {
                        if (lstMaxValue.Count != 0)
                        {
                            MainForm.GetTxtAverage().Text = lstMaxValue.Average(i => Math.Abs(i.maxValue)).ToString("N0");
                        }
                    }));

                    if ((lstMaxValue.Sum(i => i.maxValue) / lstMaxValue.Count) >= Device.FailLevel)
                    {
                        measureResult = (int)emMeasureResult.Fail;

                        MainForm.BeginInvoke((Action)(() =>
                        {
                            SetValueLabel("FAIL", MainForm.GetLblRstWalking());
                        }));

                        MainForm.GetLblRstWalking().ForeColor = Color.Yellow;
                        MainForm.GetLblRstWalking().BackColor = Color.Red;
                        Application.DoEvents();
                    }
                }

                MainForm.iSetting.MeasureEnd(Device, lstMaxValue, measureResult, measureEndTime);
                MainForm.graphDS.Name = GraphTitle + " " + StartDate + " - " + dtsCurrent.t.ToString(cstrDateTimeFomatShow);
                MainForm.RefreshGraph();
            }
        }

        /// <summary>
        /// Validates device and pc.
        /// </summary>
        /// <returns></returns>
        private bool validate()
        {

            //var lstMac = MainForm?.lstMac;
            //if (lstMac == null)
            //{
            //    var keyEncripted = clsCommon.readKeyFromRegistry();
            //    var keyDecripted = !string.IsNullOrEmpty(keyEncripted) ? Cipher.DecryptText(keyEncripted) : keyEncripted;
            //    var lstKeyMacServer = keyDecripted.Split(clsConst.KEY_CHAR_SPLIT_SERVER).ToList();
            //    lstKeyMacServer.RemoveAt(0);
            //    lstMac = lstKeyMacServer;
            //}

            //if (string.IsNullOrEmpty(Device.MacAddress) || !lstMac.Contains(Device.MacAddress))
            //{
            //    return ShowMsg(MessageBoxIcon.Warning, string.Format(MSG_ERR_DEVICE_NOT_REGIS, Device.DeviceName));
            //}

            var txtAlarmValue = MainForm.GettxtDev1AlarmValue();

            if (MeasureType == emMeasureType.AlarmTest)
            {
                switch (Device.OrdinalDisplay)
                {
                    case DEVICE1_INDEX:
                        txtAlarmValue = MainForm.GettxtDev1AlarmValue();
                        break;

                    case DEVICE2_INDEX:
                        txtAlarmValue = MainForm.GettxtDev2AlarmValue();
                        break;

                    case DEVICE3_INDEX:
                        txtAlarmValue = MainForm.GettxtDev3AlarmValue();
                        break;

                    case DEVICE4_INDEX:
                        txtAlarmValue = MainForm.GettxtDev4AlarmValue();
                        break;

                    case DEVICE5_INDEX:
                        txtAlarmValue = MainForm.GettxtDev5AlarmValue();
                        break;

                    default:
                        break;
                }
                
                if (!clsCommon.IsNumberAndThanZero(txtAlarmValue.Text))
                {
                    MainForm.BeginInvoke((Action)(() =>
                    {
                        txtAlarmValue.Focus();
                    }));

                    return ShowMsg(MessageBoxIcon.Warning, string.Format(MSG_ERR_INPUT_NUMBER, "Alarm Value"));
                }

                Device.AlarmValue = clsCommon.CnvNullToInt(txtAlarmValue.Text);
            }
            else
            {
                if (!clsCommon.IsNumberAndThanZero(MainForm.GettxtFailLevel().Text))
                {
                    MainForm.BeginInvoke((Action)(() =>
                    {
                        MainForm.GettxtFailLevel().Focus();
                    }));
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(MSG_ERR_INPUT_NUMBER, "Fail Level"));
                }

                if (!clsCommon.IsNumberAndThanZero(MainForm.GettxtPeriod().Text))
                {
                    MainForm.BeginInvoke((Action)(() =>
                    {
                        MainForm.GettxtPeriod().Focus();
                    }));
                    return ShowMsg(MessageBoxIcon.Warning, string.Format(MSG_ERR_INPUT_NUMBER, "Period"));
                }

                Device.AlarmValue = clsCommon.CnvNullToInt(txtAlarmValue.Text);
                Device.FailLevel = clsCommon.CnvNullToInt(MainForm.GettxtFailLevel().Text);
                Device.Period = clsCommon.CnvNullToInt(MainForm.GettxtPeriod().Text);
            }

            return true;
        }

        /// <summary>
        /// process data.
        /// </summary>
        private void CallProcessData()
        {
            try
            {
                while (isThreadInsertRunning || lstSample.Count > 0)
                {
                    if (lstSample.Count > 0)
                    {
                        var detail = lstSample[0];
                        MainForm.iSetting.MeasureProcess(detail, Device);

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

                updateData();
            }
            finally
            {
                MainForm.IsWaitMode = false;
            }
        }

        private void client_OnDataReceived(byte[] data, int bytesRead)
        {
            ASCIIEncoding encoder = new ASCIIEncoding();
            string message = encoder.GetString(data, 0, bytesRead);
            ParseData2(message);
        }

        private void ParseData2(string message)
        {
            message = Regex.Replace(message, @"\t|\n|\r", "");
            this.message = string.IsNullOrEmpty(this.message) ? message : this.message + message;

            // Raw Data
            if (this.message != "\"" && !this.message.EndsWith("\"\"") && this.message.EndsWith("\""))
            {
                lock (syncObj)
                {
                    lstSample.Add(GetDataFromSample(this.message, true));
                }
                this.message = string.Empty;
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
                    var checkMilliseconds = MeasureType == emMeasureType.AlarmTest ? MainForm.iSetting.AlarmTime : MainForm.iSetting.WalkingTime;

                    if (checkTime.TotalMilliseconds >= checkMilliseconds)
                    {
                        dtsCurrent.t = now;
                        ProcessData(GetDataFromSample(dtsCurrent.strSample));
                        dtsCurrent.strSample = string.Empty;
                    }
                }
            }
        }

        private void ProcessData(DataSample dts)
        {
            if (beforeActualDelegate != dts.actualDelegate)
            {
                var isMeasureUp = dts.actualDelegate > beforeActualDelegate;

                if (isMeasureUp != this.isMeasureUp)
                {
                    this.isMeasureUp = isMeasureUp;
                    maxKey++;
                }

                beforeActualDelegate = dts.actualDelegate;
            }

            var drawValue = DrawValueToGraph(dts);

            if (MeasureType == emMeasureType.AlarmTest)
            {
                switch (Device.OrdinalDisplay)
                {
                    case DEVICE1_INDEX:
                        MainForm.BeginInvoke((Action)(() =>
                        {
                            SetValueText(drawValue.ToString("N0"), MainForm.GettxtDev1ActualValue());
                        }));
                        break;

                    case DEVICE2_INDEX:
                        MainForm.BeginInvoke((Action)(() =>
                        {
                            SetValueText(drawValue.ToString("N0"), MainForm.GettxtDev2ActualValue());
                        }));
                        break;

                    case DEVICE3_INDEX:
                        MainForm.BeginInvoke((Action)(() =>
                        {
                            SetValueText(drawValue.ToString("N0"), MainForm.GettxtDev3ActualValue());
                        }));
                        break;

                    case DEVICE4_INDEX:
                        MainForm.BeginInvoke((Action)(() =>
                        {
                            SetValueText(drawValue.ToString("N0"), MainForm.GettxtDev4ActualValue());
                        }));
                        break;

                    case DEVICE5_INDEX:
                        MainForm.BeginInvoke((Action)(() =>
                        {
                            SetValueText(drawValue.ToString("N0"), MainForm.GettxtDev5ActualValue());
                        }));
                        break;

                    default:
                        break;
                }
            }
            else if (MeasureType == emMeasureType.WalkingTest)
            {
                var max = new MaxValue
                {
                    key = maxKey,
                    dtSample = dts,
                    datetime = dts.t,
                    maxValue = Math.Abs(dts.actualDelegate),
                    maxValueShow = dts.actualDelegate,
                    point = new cPoint()
                    {
                        x = MainForm.graphDS.Samples != null ? MainForm.graphDS.Samples.Count - 1 : 0,
                        y = (float)dts.actualDelegate
                    }
                };

                lstMaxValue = lstMaxValue.Where(i => i.key != maxKey).ToList();
                lstMaxValue.Add(max);
                lstMaxValue = lstMaxValue.OrderByDescending(i => i.maxValue)
                                         .ThenByDescending(i => i.point.x)
                                         .Take(Device.Samples)
                                         .ToList();

                lstMaxValue = lstMaxValue.OrderBy(i => i.point.x)
                                         .ThenBy(i => i.maxValue)
                                         .ToList();
            }
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

                    if (Device.AlarmValue <= Math.Abs(dts.actualDelegate))
                    {
                        dts.result = (int)emMeasureResult.Alarm;

                        if (measureResult == (int)emMeasureResult.Normal)
                        {
                            measureResult = dts.result;
                        }
                    }

                    // Actual Value > Alarm Value
                    if (absActualDelegate > MainForm.graphDS.UpLimit)
                    {
                        if (!isFirstMax)
                        {
                            maxValue = dts.actualDelegate;
                            isFirstMax = true;
                        }
                        else
                        {
                            if (Math.Abs(dts.actualDelegate) >= Math.Abs(maxValue))
                            {
                                maxValue = dts.actualDelegate;
                            }
                        }

                        if (IsRenderForm)
                        {
                            MainForm.BeginInvoke((Action)(() =>
                            {
                                SetValueLabel(maxValue.ToString(), MainForm.GetlblOverViewValue());
                            }));
                        }

                        MainForm.BeginInvoke((Action)(() =>
                        {
                            MainForm.SetAlarmFlag(Device);
                            MainForm.Blink(Device);
                        }));
                    }

                    if (MainForm.graphDS.maxValue > 0 && absActualDelegate >= MainForm.graphDS.maxValue)
                    {
                        dblValueGraph = (dts.actualDelegate / absActualDelegate) * MainForm.graphDS.maxValue;
                    }
                }
                else
                {
                    MainForm.graphDS.maxPoint = lstMaxValue.Select(i => i.point).ToArray();
                }

                lock (syncObj)
                {
                    lstSample.Add(dts);
                }

                LstPoint.Add((float)dblValueGraph);

                if (IsRenderForm)
                {
                    MainForm.BeginInvoke((Action)(() =>
                    {
                        MainForm.AddPoint((float)dblValueGraph);
                    }));

                    if ((MainForm.graphDS.Length - MainForm.GetPdeGraph().starting_idx) >= (int)MainForm.GetPdeGraph().endX)
                    {
                        float dx = (MainForm.GetPdeGraph().endX - MainForm.GetPdeGraph().startX);
                        MainForm.GetPdeGraph().SetStartingIdx((int)(MainForm.graphDS.Length - dx - (Math.Abs(MainForm.GetPdeGraph().MoveMouse) > 0 ? MainForm.GetPdeGraph().MoveMouse - 1 : 0)));
                    }

                    MainForm.RefreshGraph();
                }

                //if ((MainForm.graphDS.Length - MainForm.GetPdeGraph().starting_idx) >= (int)MainForm.GetPdeGraph().endX)
                //{
                //    float dx = (MainForm.GetPdeGraph().endX - MainForm.GetPdeGraph().startX);
                //    MainForm.GetPdeGraph().SetStartingIdx((int)(MainForm.graphDS.Length - dx - (Math.Abs(MainForm.GetPdeGraph().MoveMouse) > 0 ? MainForm.GetPdeGraph().MoveMouse - 1 : 0)));
                //}

                //MainForm.RefreshGraph();

                return dts.actualDelegate;
            }
            catch (Exception ex)
            {
                return 0;
            }
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

                if (Device.AlarmValue <= (int)dblDelegate)
                {
                    result = (int)emMeasureResult.Alarm;
                }
            }

            return new DataSample
            {
                deviceId = Device.DeviceId,
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
            string arrString = MainForm.GetDeviceType(arrValue, MainForm.graphDS);

            if (this.IsRunning && this.IsRenderForm)
            {
                MainForm.BeginInvoke((Action)(() =>
                {
                    SetValueLabel(arrString, MainForm.GetlblDeviceType());
                }));
            }

            double[] dblValue = arrValue.Select(n => CnvDeviceRstToDouble(n)).ToArray();

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
                return MainForm.graphDS.maxValue * (isMeasureUp ? 1 : -1);
            }
            else
            {
                return clsCommon.CnvNullToDouble(strSample, 0) * MainForm.iSetting.RoundValue;
            }
        }

        private void client_ProcessErrors()
        {
            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new Action(() =>
            //    {
            //        confirmReConnect();
            //    }));
            //}
            //else
            //{
            //    confirmReConnect();
            //}
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
                    if (telnetDSF.ConnectToServer(Device.IpAddress, Device.Port))
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

        public void MeasureEnd(object sender, EventArgs e)
        {
            isThreadInsertRunning = false;
            IsRunning = false;

            if (MeasureType == emMeasureType.WalkingTest)
            {
                if (timerWalkingTest != null)
                {
                    timerWalkingTest.Stop();
                }

                MainForm.BeginInvoke((Action)(() =>
                {
                    MainForm.DeviceCallBack(this);
                }));
            }

            MainForm.BeginInvoke((Action)(() =>
            {
                MainForm.ModeMeasure(Device.DeviceId, false);
            }));

            Disconnect();
        }

        private void Disconnect()
        {
            try
            {
                if (telnetDSF == null) return;
                telnetDSF.Disconnect();
                telnetDSF = null;

                if (thread != null)
                {
                    thread.Abort();
                }
                thread = null;
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
