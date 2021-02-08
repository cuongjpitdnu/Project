using BaseCommon.Class;
using BaseCommon.Properties;
using GraphLib;
using KeyCipher;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BaseCommon.clsConst;

namespace BaseCommon
{
    public partial class GraphForm : BaseForm
    {
        #region Const

        private const string DEVICE_ERROR = "DEVICE ERROR";
        private const string MSG_EXIT_APP = "Measure is starting";

        private const string MSG_VALIDATE_ERR = "Machine is not activated!";

        private const int DEVICE1_INDEX = 1;
        private const int DEVICE2_INDEX = 2;
        private const int DEVICE3_INDEX = 3;
        private const int DEVICE4_INDEX = 4;
        private const int DEVICE5_INDEX = 5;

        private Color ALARM_COLOR_ENABLE = Color.FromArgb(255, 255, 128);
        private Color ALARM_COLOR_DISABLE = Color.Gray;
        private Color ALARM_COLOR_FAIL = Color.Red;
        private Color ALARM_COLOR_PASS = Color.Green;
        private Color ALARM_COLOR_RUNNING = Color.CornflowerBlue;

        private bool isWalkingTestRun = false;

        #endregion

        #region Variables

        public DataSource graphDS { get; set; }
        public IGraphForm iSetting { get; set; }
        public List<string> lstMac { get; set; }
        private List<DeviceInfo> lstDeviceInfo = new List<DeviceInfo>();
        private int intDataSrcIndex = 0;
        private Button currentXButton;
        private Button currentYButton;
        public List<DeviceProcess> lstThread { get; set; }
        private DeviceProcess deviceProcess1;
        private DeviceProcess deviceProcess2;
        private DeviceProcess deviceProcess3;
        private DeviceProcess deviceProcess4;
        private DeviceProcess deviceProcess5;
        private bool isDeviceAlarm1 = false;
        private bool isDeviceAlarm2 = false;
        private bool isDeviceAlarm3 = false;
        private bool isDeviceAlarm4 = false;
        private bool isDeviceAlarm5 = false;

        #endregion

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
                    this.Invoke(new Action(() =>
                    {
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

        #endregion

        #region Events

        public GraphForm(IGraphForm iSetting, bool blnIsDebug = false)
        {
            lstThread = new List<DeviceProcess>();
            this.iSetting = iSetting;
            this.iSetting.FormSetting = this;
            IsDeBugMode = blnIsDebug;

            InitializeComponent();
            IntForm();
        }

        private void GraphForm_Load(object sender, EventArgs e)
        {

            var keyEnvrip = Cipher.EncryptText("BFEBFBFF000906EB8CEC4BD28C43/9J47NX2/CNWS20094A018U/");


            GraphInit();
            this.Hide();
            var keyEncripted = clsCommon.readKeyFromRegistry();

            try
            {
                var rsValidate = clsCommon.ValidateMachineAndDevices(lstDeviceInfo, keyEncripted);

                if (!rsValidate)
                {
                    using (var lis = new LicenseForm(iSetting, this))
                    {
                        var rs = lis.ShowDialog();
                        if (rs == DialogResult.Cancel)
                        {
                            this.Close();
                            Environment.Exit(0);
                        }
                    }
                }
                else
                {
                    //var keyDecripted = Cipher.DecryptText(keyEncripted);
                    //var lstKeyMacServer = keyDecripted.Split(clsConst.KEY_CHAR_SPLIT_SERVER).ToList();
                    //lstKeyMacServer.RemoveAt(0);
                    //lstMac = lstKeyMacServer;

                    this.Show();
                    //InitThread();
                }
            }
            catch (Exception)
            {
            }
        }

        private void GraphForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var isThreadRunning = lstThread.Any(i => i.IsRunning == true);
            if (isThreadRunning)
            {
                e.Cancel = !ShowMsg(MessageBoxIcon.Warning, MSG_EXIT_APP);
                return;
            }

            foreach (var process in lstThread)
            {
                if (process.IsRunning)
                {
                    process.Stop();
                }
            }


            if (iSetting != null)
            {
                iSetting.StopInsertDB();
                iSetting.Dispose();
                iSetting = null;
            }
        }

        private void lblDevice1_Click(object sender, EventArgs e)
        {
            if (isWalkingTestRun) return;

            if (deviceProcess1 == null)
            {
                deviceProcess1 = new DeviceProcess();
                deviceProcess1.MainForm = this;
                deviceProcess1.Device = lstDeviceInfo[0];
                deviceProcess1.MeasureType = emMeasureType.AlarmTest;
                deviceProcess1.IsRenderForm = true;
                lstThread.Add(deviceProcess1);
            }

            //Draw old graph
            GraphInit(deviceProcess1.GraphTitle + " " + deviceProcess1.StartDate, deviceProcess1.MeasureType,
                                deviceProcess1.MeasureType == emMeasureType.WalkingTest ? deviceProcess1.Device.FailLevel : deviceProcess1.Device.AlarmValue);
            AddPoints(deviceProcess1.LstPoint);

            setRenderThread(DEVICE1_INDEX);
            if (deviceProcess1.IsRunning)
            {
                return;
            }
            deviceProcess1.Start();
            txtDev1AlarmValue.ReadOnly = true;
            btnDevice1Stop.Enabled = true;
        }

        private void setRenderThread(int deviceIndex)
        {
            foreach (var thread in lstThread)
            {
                thread.IsRenderForm = false;
            }

            switch (deviceIndex)
            {
                case DEVICE1_INDEX:
                    lblOverViewValue.Text = deviceProcess1.maxValue + "";
                    deviceProcess1.IsRenderForm = true;
                    lblDevice1.BackColor = ALARM_COLOR_RUNNING;
                    lblDevice2.BackColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice3.BackColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice4.BackColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice5.BackColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    break;

                case DEVICE2_INDEX:
                    lblOverViewValue.Text = deviceProcess2.maxValue + "";
                    deviceProcess2.IsRenderForm = true;
                    lblDevice1.BackColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice2.BackColor = ALARM_COLOR_RUNNING;
                    lblDevice3.BackColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice4.BackColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice5.BackColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    break;

                case DEVICE3_INDEX:
                    lblOverViewValue.Text = deviceProcess3.maxValue + "";
                    deviceProcess3.IsRenderForm = true;
                    lblDevice1.BackColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice2.BackColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice3.BackColor = ALARM_COLOR_RUNNING;
                    lblDevice4.BackColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice5.BackColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    break;

                case DEVICE4_INDEX:
                    lblOverViewValue.Text = deviceProcess4.maxValue + "";
                    deviceProcess4.IsRenderForm = true;
                    lblDevice1.BackColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice2.BackColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice3.BackColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice4.BackColor = ALARM_COLOR_RUNNING; 
                    lblDevice5.BackColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    break;

                case DEVICE5_INDEX:
                    lblOverViewValue.Text = deviceProcess5.maxValue + "";
                    deviceProcess5.IsRenderForm = true;
                    lblDevice1.BackColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice2.BackColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice3.BackColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice4.BackColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice5.BackColor = ALARM_COLOR_RUNNING; 
                    break;

                default:
                    break;
            }
        }

        private void lblDevice2_Click(object sender, EventArgs e)
        {
            if (isWalkingTestRun) return;

            if (deviceProcess2 == null)
            {
                deviceProcess2 = new DeviceProcess();
                deviceProcess2.MainForm = this;
                deviceProcess2.Device = lstDeviceInfo[1];
                deviceProcess2.MeasureType = emMeasureType.AlarmTest;
                deviceProcess2.IsRenderForm = true;
                lstThread.Add(deviceProcess2);
            }

            //Draw old graph
            GraphInit(deviceProcess2.GraphTitle + " " + deviceProcess2.StartDate, deviceProcess2.MeasureType,
                                deviceProcess2.MeasureType == emMeasureType.WalkingTest ? deviceProcess2.Device.FailLevel : deviceProcess2.Device.AlarmValue);
            AddPoints(deviceProcess2.LstPoint);

            setRenderThread(DEVICE2_INDEX);

            if (deviceProcess2.IsRunning)
            {
                return;
            }

            deviceProcess2.Start();
            txtDev2AlarmValue.ReadOnly = true;
            btnDevice2Stop.Enabled = true;
        }

        private void lblDevice3_Click(object sender, EventArgs e)
        {
            if (isWalkingTestRun) return;

            if (deviceProcess3 == null)
            {
                deviceProcess3 = new DeviceProcess();
                deviceProcess3.MainForm = this;
                deviceProcess3.Device = lstDeviceInfo[2];
                deviceProcess3.MeasureType = emMeasureType.AlarmTest;
                deviceProcess3.IsRenderForm = true;
                lstThread.Add(deviceProcess3);
            }

            //Draw old graph
            GraphInit(deviceProcess3.GraphTitle + " " + deviceProcess3.StartDate, deviceProcess3.MeasureType,
                                deviceProcess3.MeasureType == emMeasureType.WalkingTest ? deviceProcess3.Device.FailLevel : deviceProcess3.Device.AlarmValue);
            AddPoints(deviceProcess3.LstPoint);

            setRenderThread(DEVICE3_INDEX);

            if (deviceProcess3.IsRunning)
            {
                return;
            }

            deviceProcess3.Start();
            txtDev3AlarmValue.ReadOnly = true;
            btnDevice3Stop.Enabled = true;
        }

        private void lblDevice4_Click(object sender, EventArgs e)
        {
            if (isWalkingTestRun) return;

            if (deviceProcess4 == null)
            {
                deviceProcess4 = new DeviceProcess();
                deviceProcess4.MainForm = this;
                deviceProcess4.Device = lstDeviceInfo[3];
                deviceProcess4.MeasureType = emMeasureType.AlarmTest;
                deviceProcess4.IsRenderForm = true;
                lstThread.Add(deviceProcess4);
            }

            //Draw old graph
            GraphInit(deviceProcess4.GraphTitle + " " + deviceProcess4.StartDate, deviceProcess4.MeasureType,
                                deviceProcess4.MeasureType == emMeasureType.WalkingTest ? deviceProcess4.Device.FailLevel : deviceProcess4.Device.AlarmValue);
            AddPoints(deviceProcess4.LstPoint);

            setRenderThread(DEVICE4_INDEX);

            if (deviceProcess4.IsRunning)
            {
                return;
            }
            deviceProcess4.Start();
            txtDev4AlarmValue.ReadOnly = true;
            btnDevice4Stop.Enabled = true;
        }

        private void lblDevice5_Click(object sender, EventArgs e)
        {
            if (isWalkingTestRun) return;

            if (deviceProcess5 == null)
            {
                deviceProcess5 = new DeviceProcess();
                deviceProcess5.MainForm = this;
                deviceProcess5.Device = lstDeviceInfo[4];
                deviceProcess5.MeasureType = emMeasureType.AlarmTest;
                deviceProcess5.IsRenderForm = true;
                lstThread.Add(deviceProcess5);
            }

            //Draw old graph
            GraphInit(deviceProcess5.GraphTitle + " " + deviceProcess5.StartDate, deviceProcess5.MeasureType,
                                deviceProcess5.MeasureType == emMeasureType.WalkingTest ? deviceProcess5.Device.FailLevel : deviceProcess5.Device.AlarmValue);
            AddPoints(deviceProcess5.LstPoint);

            setRenderThread(DEVICE5_INDEX);

            if (deviceProcess5.IsRunning)
            {
                setRenderThread(DEVICE5_INDEX);
                return;
            }

            deviceProcess5.Start();
            txtDev5AlarmValue.ReadOnly = true;
            btnDevice5Stop.Enabled = true;
        }

        private void btnWalkingStart_Click(object sender, EventArgs e)
        {
            var index = lstDeviceInfo.FirstOrDefault(i => i.WalkingMode == true).OrdinalDisplay;
            processWalkingTest(lstDeviceInfo[index - 1]);
        }

        private void processWalkingTest(DeviceInfo device)
        {
            //var processThread = lstThread.FirstOrDefault(i => i.Device.DeviceId == device.DeviceId);

            //if (processThread != null && processThread.IsRunning)
            //{
            //    if (ComfirmMsg(string.Format("Device: {0} is running in Alarm Test Mode! Do you want to run in Walking Test Mode?", device.DeviceName), ""))
            //    {
            //        processThread.Stop();
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}

            btnWalkingDevice.Enabled = false;
            txtFailLevel.ReadOnly = true;
            txtPeriod.ReadOnly = true;

            switch (device.OrdinalDisplay)
            {
                case DEVICE1_INDEX:

                    if (deviceProcess1 == null)
                    {
                        deviceProcess1 = new DeviceProcess();
                    }

                    deviceProcess1.Device = device;
                    deviceProcess1.IsRenderForm = true;
                    deviceProcess1.MainForm = this;
                    deviceProcess1.MeasureType = emMeasureType.WalkingTest;
                    txtDev1AlarmValue.ReadOnly = true;
                    btnDevice1Stop.Enabled = false;
                    lblDevice1.Enabled = false;
                    lblDevice1.BackColor = ALARM_COLOR_DISABLE;
                    txtDev1AlarmValue.ForeColor = Color.Red;
                    lstThread.Add(deviceProcess1);
                    setRenderThread(DEVICE1_INDEX);
                    lblDevice1.BackColor = ALARM_COLOR_DISABLE;
                    deviceProcess1.Start();
                    break;

                case DEVICE2_INDEX:
                    if (deviceProcess2 == null)
                    {
                        deviceProcess2 = new DeviceProcess();
                    }
                    deviceProcess2.Device = device;
                    deviceProcess2.IsRenderForm = true;
                    deviceProcess2.MainForm = this;
                    deviceProcess2.MeasureType = emMeasureType.WalkingTest;
                    txtDev2AlarmValue.ReadOnly = true;
                    btnDevice2Stop.Enabled = false;
                    lblDevice2.Enabled = false;
                    lblDevice2.BackColor = ALARM_COLOR_DISABLE;
                    txtDev2AlarmValue.ForeColor = Color.Red;
                    lstThread.Add(deviceProcess2);
                    setRenderThread(DEVICE2_INDEX);
                    lblDevice2.BackColor = ALARM_COLOR_DISABLE;
                    deviceProcess2.Start();
                    break;

                case DEVICE3_INDEX:
                    if (deviceProcess3 == null)
                    {
                        deviceProcess3 = new DeviceProcess();
                    }
                    deviceProcess3.Device = device;
                    deviceProcess3.IsRenderForm = true;
                    deviceProcess3.MainForm = this;
                    deviceProcess3.MeasureType = emMeasureType.WalkingTest;
                    txtDev3AlarmValue.ReadOnly = true;
                    btnDevice3Stop.Enabled = false;
                    lblDevice3.Enabled = false;
                    lblDevice3.BackColor = ALARM_COLOR_DISABLE;
                    txtDev3AlarmValue.ForeColor = Color.Red;
                    lstThread.Add(deviceProcess3);
                    setRenderThread(DEVICE3_INDEX);
                    lblDevice3.BackColor = ALARM_COLOR_DISABLE;
                    deviceProcess3.Start();
                    break;

                case DEVICE4_INDEX:
                    if (deviceProcess4 == null)
                    {
                        deviceProcess4 = new DeviceProcess();
                    }
                    deviceProcess4.Device = device;
                    deviceProcess4.IsRenderForm = true;
                    deviceProcess4.MainForm = this;
                    deviceProcess4.MeasureType = emMeasureType.WalkingTest;
                    txtDev4AlarmValue.ReadOnly = true;
                    btnDevice4Stop.Enabled = false;
                    lblDevice4.Enabled = false;
                    lblDevice4.BackColor = ALARM_COLOR_DISABLE;
                    txtDev4AlarmValue.ForeColor = Color.Red;
                    lstThread.Add(deviceProcess4);
                    setRenderThread(DEVICE4_INDEX);
                    lblDevice4.BackColor = ALARM_COLOR_DISABLE;
                    deviceProcess4.Start();
                    break;

                case DEVICE5_INDEX:
                    if (deviceProcess5 == null)
                    {
                        deviceProcess5 = new DeviceProcess();
                    }
                    deviceProcess5.Device = device;
                    deviceProcess5.IsRenderForm = true;
                    deviceProcess5.MainForm = this;
                    deviceProcess5.MeasureType = emMeasureType.WalkingTest;
                    txtDev5AlarmValue.ReadOnly = true;
                    btnDevice5Stop.Enabled = false;
                    lblDevice5.Enabled = false;
                    lblDevice5.BackColor = ALARM_COLOR_DISABLE;
                    txtDev5AlarmValue.ForeColor = Color.Red;
                    lstThread.Add(deviceProcess5);
                    setRenderThread(DEVICE5_INDEX);
                    lblDevice5.BackColor = ALARM_COLOR_DISABLE;
                    deviceProcess5.Start();
                    break;

                default:
                    break;
            }
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
            if (yAxisValue < 500) yAxisValue = yAxisValue * iSetting.RoundValue;
            yAxisValue++;
            graphDS.SetDisplayRangeY(-yAxisValue, yAxisValue);

            if (yAxisValue >= 5000) graphDS.SetGridDistanceY(1000);
            else graphDS.SetGridDistanceY(100);

            RefreshGraph();
        }

        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            //var randomTest = new Random();

            //GraphInit("Test", 60);

            //var cnn = 0;
            //var type = 1;
            //MeasureType = emMeasureType.WalkingTest;
            //GetInputDevice(0);
            //ResetMeasure();
            //ModeMeasure(currentDevice.DeviceId);

            //_blnStartMeasure = true;
            //_flagInsertDetail = true;
            //_iSetting.MeasureType = MeasureType;
            //_iSetting.DeviceCurrent = currentDevice;
            //_iSetting.MeasureStart();
            //if (MeasureType == emMeasureType.WalkingTest)
            //{
            //    timerWalkingTest.Interval = Math.Abs(currentDevice.Period * 1000);
            //    timerWalkingTest.Start();
            //}
            //_threadInsert = new Thread(new ThreadStart(CallProcessData));
            //_threadInsert.Start();
            //for (var i = 0; i < clsCommon.CnvNullToInt(txtTimeTest.Text); i++)
            //{
            //    //if (cnn == 6)
            //    //{
            //    //    cnn = 0;
            //    //    type = -type;
            //    //}
            //    //float intValue = (float)(type * 59) / 1000;
            //    //float intValue = (float)randomTest.Next(-100, 100) / 1000;
            //    //float intValue = cnn < 6 && cnn > 2 ? (float)(type * 60) / 1000 : (float)(type * 59) / 1000;
            //    //float intValue = cnn < 5 && cnn > 3 ? (float)randomTest.Next(-100, 100) / 1000 : (float)60/1000;
            //    //var dts = new DataSample { t = DateTime.Now, strSample = intValue.ToString() };
            //    //DrawValueToGraph(dts);

            //    int intValue = i < 2 || i > 5 ? 1 : 2000;

            //    ParseData2("\"" + intValue + "\"");
            //    cnn++;
            //    //Application.DoEvents();
            //    Thread.Sleep(100);
            //}

            ////_blnStartMeasure = false;
            ////_flagInsertDetail = false;

            ////RefreshGraph();

            //MeasureEnd(null, null);
        }

        public TextBox GettxtFailLevel()
        {
            return this.txtFailLevel;
        }

        public TextBox GettxtPeriod()
        {
            return this.txtPeriod;
        }

        public TextBox GettxtDev1AlarmValue()
        {
            return this.txtDev1AlarmValue;
        }

        public TextBox GettxtDev2AlarmValue()
        {
            return this.txtDev2AlarmValue;
        }

        public TextBox GettxtDev3AlarmValue()
        {
            return this.txtDev3AlarmValue;
        }

        public TextBox GettxtDev4AlarmValue()
        {
            return this.txtDev4AlarmValue;
        }

        public TextBox GettxtDev5AlarmValue()
        {
            return this.txtDev5AlarmValue;
        }

        public TextBox GettxtDev1ActualValue()
        {
            return this.txtDev1ActualValue;
        }

        public TextBox GettxtDev2ActualValue()
        {
            return this.txtDev2ActualValue;
        }

        public TextBox GettxtDev3ActualValue()
        {
            return this.txtDev3ActualValue;
        }

        public TextBox GettxtDev4ActualValue()
        {
            return this.txtDev4ActualValue;
        }

        public TextBox GettxtDev5ActualValue()
        {
            return this.txtDev5ActualValue;
        }

        public Label GetlblOverViewValue()
        {
            return this.lblOverViewValue;
        }

        public void ModeMeasure(int deviceId, bool isRunning = true)
        {
            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new Action(() =>
            //    {
            //        ModeMeasure(intDev, isStart);
            //    }));
            //}
            //else
            //{
            //    btnManagement.Enabled = !isStart;
            //    //txtPeriod.ReadOnly = isStart;
            //    //txtFailLevel.ReadOnly = isStart;
            //}

            var process = lstThread.FirstOrDefault(i => i.Device.DeviceId == deviceId);
            if(process != null)
            {
                process.IsRunning = isRunning;
            }

            var isEnable = lstThread.Any(i => i.IsRunning);
            btnManagement.Enabled = !isEnable;
        }

        public Label GetlblDeviceType()
        {
            return this.lblDeviceType;
        }

        public string GetDeviceType(string strValue, DataSource graphDS)
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

        public string GetDeviceType(string[] strValue, DataSource graphDS)
        {
            string strRet = DEVICE_ERROR;
            if (strValue.Length > 0)
            {
                int i = 0;
                do
                {
                    strRet = GetDeviceType(strValue[i], graphDS);
                    i++;

                } while (strRet == DEVICE_ERROR && i < strValue.Length);
            }
            return strRet;
        }

        public void ResetMeasure()
        {
            lblOverViewValue.Text = string.Empty;
            lblRstWalking.Text = "PASS";
            lblRstWalking.ForeColor = Color.Blue;
            lblRstWalking.BackColor = Color.Lime;
            lblDeviceType.Text = string.Empty;
            txtDev1ActualValue.Text = string.Empty;
            txtDev2ActualValue.Text = string.Empty;
            txtDev3ActualValue.Text = string.Empty;
            txtDev4ActualValue.Text = string.Empty;
            txtDev5ActualValue.Text = string.Empty;
            txtAverage.Text = string.Empty;

            dgvMeasure.Rows.Clear();
        }

        public TextBox GetTxtAverage()
        {
            return txtAverage;
        }

        public Label GetLblRstWalking()
        {
            return lblRstWalking;
        }

        private async void btnDevice1Stop_Click(object sender, EventArgs e)
        {
            deviceProcess1.LstPoint.Clear();
            isDeviceAlarm1 = false;
            btnDevice1Stop.Enabled = false;
            txtDev1AlarmValue.ReadOnly = false;
            txtDev1AlarmValue.ForeColor = Color.Red;
            deviceProcess1.Stop();

            Application.DoEvents();
            txtDev1ActualValue.Text = string.Empty;

            await Task.Delay(500);
            lblDevice1.BackColor = ALARM_COLOR_ENABLE;
            Application.DoEvents();
        }

        private async void btnDevice2Stop_Click(object sender, EventArgs e)
        {
            deviceProcess2.LstPoint.Clear();
            isDeviceAlarm2 = false;
            btnDevice2Stop.Enabled = false;
            txtDev2AlarmValue.ReadOnly = false;
            txtDev2AlarmValue.ForeColor = Color.Red;
            deviceProcess2.Stop();

            Application.DoEvents();
            txtDev2ActualValue.Text = string.Empty;

            await Task.Delay(500);
            lblDevice2.BackColor = ALARM_COLOR_ENABLE;
            Application.DoEvents();
        }

        private async void btnDevice3Stop_Click(object sender, EventArgs e)
        {
            deviceProcess3.LstPoint.Clear();
            isDeviceAlarm3 = false;
            btnDevice3Stop.Enabled = false;
            txtDev3AlarmValue.ReadOnly = false;
            txtDev3AlarmValue.ForeColor = Color.Red;
            deviceProcess3.Stop();

            Application.DoEvents();
            txtDev3ActualValue.Text = string.Empty;

            await Task.Delay(500);
            lblDevice3.BackColor = ALARM_COLOR_ENABLE;
            Application.DoEvents();
        }

        private async void btnDevice4Stop_Click(object sender, EventArgs e)
        {
            deviceProcess4.LstPoint.Clear();
            isDeviceAlarm4 = false;
            btnDevice4Stop.Enabled = false;
            txtDev4AlarmValue.ReadOnly = false;
            txtDev4AlarmValue.ForeColor = Color.Red;
            deviceProcess4.Stop();

            Application.DoEvents();
            txtDev4ActualValue.Text = string.Empty;

            await Task.Delay(500);
            lblDevice4.BackColor = ALARM_COLOR_ENABLE;
            Application.DoEvents();
        }

        private async void btnDevice5Stop_Click(object sender, EventArgs e)
        {
            deviceProcess5.LstPoint.Clear();
            isDeviceAlarm5 = false;
            btnDevice5Stop.Enabled = false;
            txtDev5AlarmValue.ReadOnly = false;
            txtDev5AlarmValue.ForeColor = Color.Red;
            deviceProcess5.Stop();

            Application.DoEvents();
            txtDev5ActualValue.Text = string.Empty;

            await Task.Delay(500);
            lblDevice5.BackColor = ALARM_COLOR_ENABLE;
            Application.DoEvents();
        }

        private void lblReset_Click(object sender, EventArgs e)
        {
            isDeviceAlarm1 = false;
            isDeviceAlarm2 = false;
            isDeviceAlarm3 = false;
            isDeviceAlarm4 = false;
            isDeviceAlarm5 = false;
            //var renderThread = lstThread.FirstOrDefault(i => i.IsRunning && i.IsRenderForm);
            //if (renderThread == null) return;

            //switch (renderThread.Device.OrdinalDisplay)
            //{
            //    case DEVICE1_INDEX:
            //        isDeviceAlarm1 = false;
            //        await Task.Delay(500);
            //        lblDevice1.BackColor = ALARM_COLOR_RUNNING;
            //        Application.DoEvents();
            //        break;

            //    case DEVICE2_INDEX:
            //        isDeviceAlarm2 = false;
            //        await Task.Delay(500);
            //        lblDevice2.BackColor = ALARM_COLOR_RUNNING;
            //        Application.DoEvents();
            //        break;

            //    case DEVICE3_INDEX:
            //        isDeviceAlarm3 = false;
            //        await Task.Delay(500);
            //        lblDevice3.BackColor = ALARM_COLOR_RUNNING;
            //        Application.DoEvents();
            //        break;

            //    case DEVICE4_INDEX:
            //        isDeviceAlarm4 = false;
            //        await Task.Delay(500);
            //        lblDevice4.BackColor = ALARM_COLOR_RUNNING;
            //        Application.DoEvents();
            //        break;

            //    case DEVICE5_INDEX:
            //        isDeviceAlarm5 = false;
            //        await Task.Delay(500);
            //        lblDevice5.BackColor = ALARM_COLOR_RUNNING;
            //        Application.DoEvents();
            //        break;

            //    default:
            //        break;
            //}
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            var rs = new AboutForm().ShowDialog();
            if (rs == DialogResult.OK)
            {
                foreach (var process in lstThread)
                {
                    if (process.IsRunning)
                    {
                        process.Stop();
                    }
                }

                this.Close();
                this.Dispose();
            }
        }

        #endregion

        #region Graph

        public void GraphInit(string graphName = "GRAPH", emMeasureType measureType = emMeasureType.AlarmTest, int limit = 10)
        {
            pdeGraph.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;
            pdeGraph.SetPlayPanelInvisible();
            pdeGraph.drawAlarmLine = measureType == emMeasureType.AlarmTest;
            pdeGraph.drawMaxPoint = measureType == emMeasureType.WalkingTest;
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

        delegate void RefreshGraphCallBack();
        public void RefreshGraph()
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

        #region Functions

        public void ResetWalkingTestMode(bool isRunning = false)
        {
            btnWalkingDevice.Enabled = true;
            txtFailLevel.ReadOnly = false;
            txtPeriod.ReadOnly = false;

            isWalkingTestRun = isRunning;
        }

        public void DeviceCallBack(DeviceProcess process)
        {
            if (process.MeasureType == emMeasureType.WalkingTest)
            {
                btnWalkingDevice.Enabled = true;
                txtFailLevel.ReadOnly = false;
                txtPeriod.ReadOnly = false;

                if(process.IsRunning && process.IsRenderForm)
                {
                    isWalkingTestRun = true;
                }
                else
                {
                    isWalkingTestRun = false;
                }

                return;
            }

            var isExistProcess = lstThread.Any(i => i.Device.DeviceId == process.Device.DeviceId);

            if (!isExistProcess)
            {
                lstThread.Add(process);
            }
            else
            {
                lstThread.FirstOrDefault(i => i.Device.DeviceId == process.Device.DeviceId).IsRunning = process.IsRunning;
            }

            //set button stop enable/disable
            btnDevice1Stop.Enabled = deviceProcess1 != null && deviceProcess1.IsRunning && !lstDeviceInfo[0].WalkingMode;
            btnDevice2Stop.Enabled = deviceProcess2 != null && deviceProcess2.IsRunning && !lstDeviceInfo[1].WalkingMode;
            btnDevice3Stop.Enabled = deviceProcess3 != null && deviceProcess3.IsRunning && !lstDeviceInfo[2].WalkingMode;
            btnDevice4Stop.Enabled = deviceProcess4 != null && deviceProcess4.IsRunning && !lstDeviceInfo[3].WalkingMode;
            btnDevice5Stop.Enabled = deviceProcess5 != null && deviceProcess5.IsRunning && !lstDeviceInfo[4].WalkingMode;

            var firstSuccessDevice = lstThread.FirstOrDefault(i => i.IsRunning && i.IsRenderForm);

            if (firstSuccessDevice == null)
            {
                lblDevice1.BackColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                lblDevice2.BackColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                lblDevice3.BackColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                lblDevice4.BackColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                lblDevice5.BackColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;

                txtDev1AlarmValue.ForeColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? Color.Red : Color.Black;
                txtDev1AlarmValue.ReadOnly = (deviceProcess1 != null && deviceProcess1.IsRunning) ? true : !(lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode);

                txtDev2AlarmValue.ForeColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? Color.Red : Color.Black;
                txtDev2AlarmValue.ReadOnly = (deviceProcess2 != null && deviceProcess2.IsRunning) ? true : !(lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode);

                txtDev3AlarmValue.ForeColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? Color.Red : Color.Black;
                txtDev3AlarmValue.ReadOnly = (deviceProcess3 != null && deviceProcess3.IsRunning) ? true : !(lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode);

                txtDev4AlarmValue.ForeColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? Color.Red : Color.Black;
                txtDev4AlarmValue.ReadOnly = (deviceProcess4 != null && deviceProcess4.IsRunning) ? true : !(lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode);

                txtDev5AlarmValue.ForeColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? Color.Red : Color.Black;
                txtDev5AlarmValue.ReadOnly = (deviceProcess5 != null && deviceProcess5.IsRunning) ? true : !(lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode);

                Application.DoEvents();
                return;
            }
            var index = firstSuccessDevice.Device.OrdinalDisplay;

            switch (index)
            {
                case DEVICE1_INDEX:
                    deviceProcess1.IsRenderForm = true;
                    lblDevice1.BackColor = ALARM_COLOR_RUNNING;

                    lblDevice2.BackColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice3.BackColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice4.BackColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice5.BackColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;

                    txtDev1AlarmValue.ReadOnly = true;

                    txtDev2AlarmValue.ForeColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? Color.Red : Color.Black;
                    txtDev3AlarmValue.ForeColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? Color.Red : Color.Black;
                    txtDev4AlarmValue.ForeColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? Color.Red : Color.Black;
                    txtDev5AlarmValue.ForeColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? Color.Red : Color.Black;
                    break;

                case DEVICE2_INDEX:
                    deviceProcess2.IsRenderForm = true;
                    lblDevice2.BackColor = ALARM_COLOR_RUNNING;
                    txtDev2AlarmValue.ReadOnly = true;

                    lblDevice1.BackColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice3.BackColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice4.BackColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice5.BackColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;

                    txtDev1AlarmValue.ForeColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? Color.Red : Color.Black;
                    txtDev3AlarmValue.ForeColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? Color.Red : Color.Black;
                    txtDev4AlarmValue.ForeColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? Color.Red : Color.Black;
                    txtDev5AlarmValue.ForeColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? Color.Red : Color.Black;
                    break;

                case DEVICE3_INDEX:
                    deviceProcess3.IsRenderForm = true;
                    lblDevice3.BackColor = ALARM_COLOR_RUNNING;

                    lblDevice1.BackColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice2.BackColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice4.BackColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice5.BackColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;

                    txtDev3AlarmValue.ReadOnly = true;

                    txtDev1AlarmValue.ForeColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? Color.Red : Color.Black;
                    txtDev2AlarmValue.ForeColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? Color.Red : Color.Black;
                    txtDev4AlarmValue.ForeColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? Color.Red : Color.Black;
                    txtDev5AlarmValue.ForeColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? Color.Red : Color.Black;
                    break;

                case DEVICE4_INDEX:
                    deviceProcess4.IsRenderForm = true;
                    lblDevice4.BackColor = ALARM_COLOR_RUNNING;

                    lblDevice1.BackColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice2.BackColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice3.BackColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice5.BackColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;

                    txtDev4AlarmValue.ReadOnly = true;

                    txtDev1AlarmValue.ForeColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? Color.Red : Color.Black;
                    txtDev2AlarmValue.ForeColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? Color.Red : Color.Black;
                    txtDev3AlarmValue.ForeColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? Color.Red : Color.Black;
                    txtDev5AlarmValue.ForeColor = lstDeviceInfo[4].Active && !lstDeviceInfo[4].WalkingMode ? Color.Red : Color.Black;
                    break;

                case DEVICE5_INDEX:
                    deviceProcess5.IsRenderForm = true;
                    lblDevice5.BackColor = ALARM_COLOR_RUNNING;

                    lblDevice1.BackColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice2.BackColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice3.BackColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                    lblDevice4.BackColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;

                    txtDev5AlarmValue.ReadOnly = true;

                    txtDev1AlarmValue.ForeColor = lstDeviceInfo[0].Active && !lstDeviceInfo[0].WalkingMode ? Color.Red : Color.Black;
                    txtDev2AlarmValue.ForeColor = lstDeviceInfo[1].Active && !lstDeviceInfo[1].WalkingMode ? Color.Red : Color.Black;
                    txtDev3AlarmValue.ForeColor = lstDeviceInfo[2].Active && !lstDeviceInfo[2].WalkingMode ? Color.Red : Color.Black;
                    txtDev4AlarmValue.ForeColor = lstDeviceInfo[3].Active && !lstDeviceInfo[3].WalkingMode ? Color.Red : Color.Black;
                    break;

                default:
                    break;
            }   
        }

        public void LoadInfoDevice()
        {
            txtDev1ActualValue.Text = string.Empty;
            txtDev1AlarmValue.Text = string.Empty;
            txtDev2ActualValue.Text = string.Empty;
            txtDev2AlarmValue.Text = string.Empty;
            txtDev3ActualValue.Text = string.Empty;
            txtDev3AlarmValue.Text = string.Empty;
            txtDev4ActualValue.Text = string.Empty;
            txtDev4AlarmValue.Text = string.Empty;
            txtDev5ActualValue.Text = string.Empty;
            txtDev5AlarmValue.Text = string.Empty;
            txtPeriod.Text = string.Empty;
            txtFailLevel.Text = string.Empty;
            txtAverage.Text = string.Empty;
            txtPeriod.ReadOnly = false;
            txtFailLevel.ReadOnly = false;

            lblDevice1.Enabled = false;
            lblDevice2.Enabled = false;
            lblDevice3.Enabled = false;
            lblDevice4.Enabled = false;
            lblDevice5.Enabled = false;
            btnDevice1Stop.Enabled = false;
            btnDevice2Stop.Enabled = false;
            btnDevice3Stop.Enabled = false;
            btnDevice4Stop.Enabled = false;
            btnDevice5Stop.Enabled = false;
            btnWalkingDevice.Enabled = false;

            lstDeviceInfo = iSetting.GetListDevice();
            DeviceInfo walkingTestDevice = null;

            foreach (var device in lstDeviceInfo)
            {
                var index = device.OrdinalDisplay;
                if (device.WalkingMode) walkingTestDevice = device;
                device.Active = device.Active && !string.IsNullOrEmpty(device.IpAddress);

                bool isActivate = device.Active && !device.WalkingMode;
                switch (index)
                {
                    case DEVICE1_INDEX:
                        lblDevice1.Text = device.DeviceName + "";
                        lblDevice1.BackColor = isActivate ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                        txtDev1AlarmValue.Text = device.AlarmValue > 0 ? device.AlarmValue.ToString() : string.Empty;
                        txtDev1AlarmValue.ReadOnly = !isActivate;
                        lblDevice1.Enabled = isActivate;

                        if(deviceProcess1 != null)
                        {
                            deviceProcess1.Device = device;
                        }

                        break;

                    case DEVICE2_INDEX:
                        lblDevice2.Text = device.DeviceName + "";
                        lblDevice2.BackColor = isActivate ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                        txtDev2AlarmValue.ReadOnly = !isActivate;
                        txtDev2AlarmValue.Text = device.AlarmValue > 0 ? device.AlarmValue.ToString() : string.Empty;
                        lblDevice2.Enabled = isActivate;

                        if (deviceProcess2 != null)
                        {
                            deviceProcess2.Device = device;
                        }
                        break;

                    case DEVICE3_INDEX:
                        lblDevice3.Text = device.DeviceName + "";
                        lblDevice3.BackColor = isActivate ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                        txtDev3AlarmValue.ReadOnly = !isActivate;
                        txtDev3AlarmValue.Text = device.AlarmValue > 0 ? device.AlarmValue.ToString() : string.Empty;
                        lblDevice3.Enabled = isActivate;

                        if (deviceProcess3 != null)
                        {
                            deviceProcess3.Device = device;
                        }
                        break;

                    case DEVICE4_INDEX:
                        lblDevice4.Text = device.DeviceName + "";
                        lblDevice4.BackColor = isActivate ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                        txtDev4AlarmValue.ReadOnly = !isActivate;
                        txtDev4AlarmValue.Text = device.AlarmValue > 0 ? device.AlarmValue.ToString() : string.Empty;
                        lblDevice4.Enabled = isActivate;

                        if (deviceProcess4 != null)
                        {
                            deviceProcess4.Device = device;
                        }
                        break;

                    case DEVICE5_INDEX:
                        lblDevice5.Text = device.DeviceName + "";
                        lblDevice5.BackColor = isActivate ? ALARM_COLOR_ENABLE : ALARM_COLOR_DISABLE;
                        txtDev5AlarmValue.ReadOnly = !isActivate;
                        txtDev5AlarmValue.Text = device.AlarmValue > 0 ? device.AlarmValue.ToString() : string.Empty;
                        lblDevice5.Enabled = isActivate;

                        if (deviceProcess5 != null)
                        {
                            deviceProcess5.Device = device;
                        }
                        break;

                    default:
                        break;
                }
            }

            //binding walking test mode
            if (walkingTestDevice == null)
            {
                btnWalkingDevice.Text = "Device";
                txtPeriod.ReadOnly = true;
                txtFailLevel.ReadOnly = true;

                return;
            }

            if (walkingTestDevice.Active) btnWalkingDevice.Enabled = true;
            btnWalkingDevice.Text = walkingTestDevice?.DeviceName + "";
            txtPeriod.Text = walkingTestDevice?.Period > 0 ? walkingTestDevice?.Period.ToString() : string.Empty;
            txtFailLevel.Text = walkingTestDevice?.FailLevel > 0 ? walkingTestDevice?.FailLevel.ToString() : string.Empty;
        }

        public GraphLib.PlotterDisplayEx GetPdeGraph()
        {
            return this.pdeGraph;
        }

        public void AddPoints(List<float> lstPoint)
        {
            foreach(float point in lstPoint)
            {
                AddPoint(point);
            }
        }

        public void AddPoint(float value)
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

        public string GetAlarmTestTextValue()
        {
            return lblAlarmTest.Text.Trim();
        }

        public string GetWalkingTestTextValue()
        {
            return lblWalkingTest.Text.Trim();
        }

        public void SetAlarmFlag(DeviceInfo device, bool flag = true)
        {
            switch (device.OrdinalDisplay)
            {
                case DEVICE1_INDEX:
                    isDeviceAlarm1 = flag;
                    break;

                case DEVICE2_INDEX:
                    isDeviceAlarm2 = flag;
                    break;

                case DEVICE3_INDEX:
                    isDeviceAlarm3 = flag;
                    break;

                case DEVICE4_INDEX:
                    isDeviceAlarm4 = flag;
                    break;

                case DEVICE5_INDEX:
                    isDeviceAlarm5 = flag;
                    break;

                default:
                    break;
            }
        }

        private void SetColorAlarmLable(bool isDeviceAlarm, Label lblDevice, DeviceProcess deviceRender, int DeviceIndex)
        {
            if (isDeviceAlarm)
            {
                lblDevice.BackColor = lblDevice.BackColor == Color.Red ? Color.Green : Color.Red;
            }
            else if (lblDevice.BackColor != ALARM_COLOR_DISABLE)
            {
                lblDevice.BackColor = deviceRender != null && deviceRender.Device.DeviceId == DeviceIndex ? ALARM_COLOR_RUNNING : ALARM_COLOR_ENABLE;
            }
        }

        public async void Blink(DeviceInfo device)
        {
            DeviceProcess deviceRender = null;

            while (isDeviceAlarm1 || isDeviceAlarm2 || isDeviceAlarm3 || isDeviceAlarm4 || isDeviceAlarm5)
            {
                deviceRender = lstThread.FirstOrDefault(i => i.IsRunning && i.IsRenderForm);

                await Task.Delay(500);
                SetColorAlarmLable(isDeviceAlarm1, lblDevice1, deviceRender, DEVICE1_INDEX);
                SetColorAlarmLable(isDeviceAlarm2, lblDevice2, deviceRender, DEVICE2_INDEX);
                SetColorAlarmLable(isDeviceAlarm3, lblDevice3, deviceRender, DEVICE3_INDEX);
                SetColorAlarmLable(isDeviceAlarm4, lblDevice4, deviceRender, DEVICE4_INDEX);
                SetColorAlarmLable(isDeviceAlarm5, lblDevice5, deviceRender, DEVICE5_INDEX);
            }

            deviceRender = lstThread.FirstOrDefault(i => i.IsRunning && i.IsRenderForm);

            SetColorAlarmLable(false, lblDevice1, deviceRender, DEVICE1_INDEX);
            SetColorAlarmLable(false, lblDevice2, deviceRender, DEVICE2_INDEX);
            SetColorAlarmLable(false, lblDevice3, deviceRender, DEVICE3_INDEX);
            SetColorAlarmLable(false, lblDevice4, deviceRender, DEVICE4_INDEX);
            SetColorAlarmLable(false, lblDevice5, deviceRender, DEVICE5_INDEX);

            Application.DoEvents();

            //while (isDeviceAlarm1)
            //{
            //    await Task.Delay(10);
            //    lblDevice1.BackColor = lblDevice1.BackColor == Color.Red ? Color.Green : Color.Red;
            //}

            //while (isDeviceAlarm2)
            //{
            //    await Task.Delay(10);
            //    lblDevice2.BackColor = lblDevice2.BackColor == Color.Red ? Color.Green : Color.Red;
            //}

            //while (isDeviceAlarm3)
            //{
            //    await Task.Delay(10);
            //    lblDevice3.BackColor = lblDevice3.BackColor == Color.Red ? Color.Green : Color.Red;
            //}

            //while (isDeviceAlarm4)
            //{
            //    await Task.Delay(10);
            //    lblDevice4.BackColor = lblDevice4.BackColor == Color.Red ? Color.Green : Color.Red;
            //}

            //while (isDeviceAlarm5)
            //{
            //    await Task.Delay(10);
            //    lblDevice5.BackColor = lblDevice5.BackColor == Color.Red ? Color.Green : Color.Red;
            //}
        }

        private void InitThread()
        {
            var lstDevice = lstDeviceInfo.Where(i => i.Active).ToList();

            foreach (var device in lstDevice)
            {
                switch (device.OrdinalDisplay)
                {
                    case DEVICE1_INDEX:
                        deviceProcess1 = new DeviceProcess();
                        deviceProcess1.MainForm = this;
                        deviceProcess1.Device = device;
                        deviceProcess1.IsRenderForm = false;
                        deviceProcess1.Start();
                        break;

                    case DEVICE2_INDEX:
                        deviceProcess2 = new DeviceProcess();
                        deviceProcess2.MainForm = this;
                        deviceProcess2.Device = device;
                        deviceProcess2.IsRenderForm = false;
                        deviceProcess2.Start();
                        break;

                    case DEVICE3_INDEX:
                        deviceProcess3 = new DeviceProcess();
                        deviceProcess3.MainForm = this;
                        deviceProcess3.Device = device;
                        deviceProcess3.IsRenderForm = false;
                        deviceProcess3.Start();
                        break;

                    case DEVICE4_INDEX:
                        deviceProcess4 = new DeviceProcess();
                        deviceProcess4.MainForm = this;
                        deviceProcess4.Device = device;
                        deviceProcess4.IsRenderForm = false;
                        deviceProcess4.Start();
                        break;

                    case DEVICE5_INDEX:
                        deviceProcess5 = new DeviceProcess();
                        deviceProcess5.MainForm = this;
                        deviceProcess5.Device = device;
                        deviceProcess5.IsRenderForm = false;
                        deviceProcess5.Start();
                        break;

                    default:
                        break;
                }
            }
        }

        private void IntForm()
        {
            // Limit resize form
            this.MinimumSize = this.Size;
            this.MaximumSize = Screen.PrimaryScreen.Bounds.Size;

            pdeGraph.DoubleBuffering = true;
            LoadInfoDevice();

            if (IsDeBugMode)
            {
                btnAddPoint.Visible = true;
                txtCount.Visible = true;
                txtTimeTest.Visible = true;
                dgvMeasure.Visible = true;
            }

            // Add Event
            txtDev1AlarmValue.KeyPress += onlyInputNumber;
            txtDev2AlarmValue.KeyPress += onlyInputNumber;
            txtDev3AlarmValue.KeyPress += onlyInputNumber;
            txtDev4AlarmValue.KeyPress += onlyInputNumber;
            txtDev5AlarmValue.KeyPress += onlyInputNumber;
            txtPeriod.KeyPress += onlyInputNumber;
            txtFailLevel.KeyPress += onlyInputNumber;

            btn60s.Click += xAxis_Click;
            btn90s.Click += xAxis_Click;
            btn120s.Click += xAxis_Click;
            btn180s.Click += xAxis_Click;
            btn360s.Click += xAxis_Click;
            btn480s.Click += xAxis_Click;
            btn600s.Click += xAxis_Click;

            btn500V.Click += yAxis_Click;
            btn1000V.Click += yAxis_Click;
            btn2000V.Click += yAxis_Click;
            btn5000V.Click += yAxis_Click;
            btn10000V.Click += yAxis_Click;
            btn15000V.Click += yAxis_Click;
            btn20000V.Click += yAxis_Click;

            btnManagement.Click += iSetting.ShowManagement;
            btnReport.Click += iSetting.ShowMeasureManagement;
            btnAbout.Click += btnAbout_Click;
        }

        #endregion Private Function

        private void GraphForm_Resize(object sender, EventArgs e)
        {
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel2.Height = tableLayoutPanel1.Height - tableLayoutPanel1.Padding.Top - tableLayoutPanel2.Margin.Top;
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
        }
    }
}
