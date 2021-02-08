using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BaseCommon.clsConst;
using DSF602.Model;
using BaseCommon;
using System.Runtime.Remoting.Channels;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using DSF602.Language;
using System.Drawing.Drawing2D;
using System.Reflection;
//using System.Windows.Media;

namespace DSF602.View.GraphLayout
{
    public partial class SensorItemView : UserControl
    {
        public EventHandler OnClick;
        private string alarm;
        private string running;
        private string voltage;
        private string nouse;
        private string iontitle;

        public SensorItemView()
        {
            InitializeComponent();
            foreach (Control control in Controls)
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }
            GetTitle();

        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AppManager.OnSensorInfoRefresh += OnSensorInfoChanged;
            AppManager.OnLanguageChanged += OnLanguageChanged;
            AppManager.OnSettingChanged += OnSettingChanged;
        }

        private void OnSettingChanged(object sender, EventArgs e)
        {
            ObjChanged obj = sender as ObjChanged;
            if (obj.ListSensorIdChanged.Contains(Info.SensorId))
            {
                SensorInfo sensorUpdate = AppManager.ListSensor.FirstOrDefault(s => s.SensorId == Info.SensorId);
                Info = sensorUpdate;
            }
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            GetTitle();
        }

        private void GetTitle()
        {
            alarm = LanguageHelper.GetValueOf("SENSOR_ALARM");
            running = LanguageHelper.GetValueOf("SENSOR_RUNNING");
            voltage = LanguageHelper.GetValueOf("MEASURE_MODE_VOLT");
            nouse = LanguageHelper.GetValueOf("SENSOR_NOUSE");
            iontitle = LanguageHelper.GetValueOf("MEASURE_MODE_ION");
        }

        private void OnSensorInfoChanged()
        {
            Info = AppManager.ListSensor.FirstOrDefault(i => i.SensorId == Info.SensorId);
        }

        private SensorInfo _info;
        public SensorInfo Info
        {
            get { return _info; }
            set
            {
                _info = value;
                if (_info != null)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            pnMain.Refresh();
                        }));
                    }
                    else
                    {
                        pnMain.Refresh();
                    }
                }
            }
        }

        private void SensorItemView_Click(object sender, EventArgs e)
        {
            OnClick?.Invoke(Info, e);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                }
                return cp;
            }
        }

        public bool AllowRuningUpdate
        {
            get; set;
        }

        private void pnMain_Paint(object sender, PaintEventArgs e)
        {
            if (!AllowRuningUpdate) return;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(pnMain.BackColor);

            Block bl = AppManager.ListBlock.FirstOrDefault(i => i.BlockId == _info.OfBlock);
            Brush brBlack = new SolidBrush(Color.Black);
            Brush brRed = new SolidBrush(Color.Red);
            Brush brWhite = new SolidBrush(Color.White);
            Brush brStatusRunning = new SolidBrush(Color.FromArgb(112, 173, 71));

            Rectangle recStatus = new Rectangle(-1, 100, pnMain.Width, 27);
            Rectangle recName = new Rectangle(18, -1, 84, 22);
            recName.X = (pnMain.Width - recName.Width) / 2;

            Pen p = new Pen(Color.Black);

            StringFormat fCenter = new StringFormat();
            fCenter.LineAlignment = StringAlignment.Center;
            fCenter.Alignment = StringAlignment.Center;

            StringFormat fRight = new StringFormat();
            fRight.LineAlignment = StringAlignment.Center;
            fRight.Alignment = StringAlignment.Far;

            StringFormat fLeft = new StringFormat();
            fLeft.LineAlignment = StringAlignment.Center;
            fLeft.Alignment = StringAlignment.Near;

            if (_info.Active == clsConst.ACTIVE && AppManager.ActualListSensorIdActive.Contains(_info.SensorId))
            {
                var strVal = "0 V";
                if (_info.MeasureType != clsConst.MeasureMode_Decay)
                {
                    strVal = clsCommon.FormatValue(_info.ActualValue) + "V";
                }
                else
                {
                    strVal = clsCommon.FormatValue((int)_info.ActualIBMax) + "V";
                }
                

                string titleValue = "";
                if (_info.MeasureType == clsConst.MeasureMode_Volt)
                {
                    titleValue = voltage + ":";
                }
                else // if (_info.MeasureType == clsConst.MeasureMode_Ion)
                {
                    titleValue = iontitle + ":";
                }

                int yVal = 90;
                int xVal = pnMain.Width / 2;
                if (_info.Alarm)
                {
                    // Alarm status
                    g.FillRectangle(brRed, recName);
                    g.DrawString(_info.SensorName, this.Font, brWhite, xVal, recName.Height / 2, fCenter);

                    g.DrawString(titleValue, this.Font, brRed, xVal, yVal, fRight);
                    g.DrawString(strVal, this.Font, brRed, xVal, yVal, fLeft);

                    if (_info.MeasureType == clsConst.MeasureMode_Decay)
                    {
                        g.DrawString(string.Format("DECAY(-): {0} s", _info.ActualDecayNegativeTime.ToString("#,##0.00")), this.Font, brRed, xVal, yVal - 20, fCenter);
                        g.DrawString(string.Format("DECAY(+): {0} s", _info.ActualDecayPositiveTime.ToString("#,##0.00")), this.Font, brRed, xVal, yVal - 40, fCenter);
                    }

                    g.FillRectangle(brRed, recStatus);
                    g.DrawString(alarm, this.Font, brWhite, xVal, recStatus.Y + recStatus.Height / 2, fCenter);
                }
                else
                {
                    // Running status
                    g.FillRectangle(new SolidBrush(Color.FromArgb(91, 155, 213)), recName);
                    g.DrawString(_info.SensorName, this.Font, brWhite, xVal, recName.Height / 2, fCenter);

                    g.DrawString(titleValue, this.Font, brBlack, xVal, yVal, fRight);
                    g.DrawString(strVal, this.Font, brBlack, xVal, yVal, fLeft);

                    if (_info.MeasureType == clsConst.MeasureMode_Decay)
                    {
                        g.DrawString(string.Format("DECAY(-): {0} s", _info.ActualDecayNegativeTime.ToString("#,##0.00")), this.Font, brBlack, xVal, yVal - 20, fCenter);
                        g.DrawString(string.Format("DECAY(+): {0} s", _info.ActualDecayPositiveTime.ToString("#,##0.00")), this.Font, brBlack, xVal, yVal - 40, fCenter);
                    }

                    g.FillRectangle(brStatusRunning, recStatus);
                    g.DrawString(running, this.Font, brWhite, xVal, recStatus.Y + recStatus.Height / 2, fCenter);
                }
            }
            else
            {
                // No-use status
                g.DrawRectangle(p, recName);
                g.DrawString(_info.SensorName, this.Font, brBlack, pnMain.Width / 2, recName.Height / 2, fCenter);

                g.DrawRectangle(p, recStatus);
                g.DrawString(nouse, this.Font, brBlack, pnMain.Width / 2, recStatus.Y + recStatus.Height / 2, fCenter);

            }
            g.DrawRectangle(p, 0, 0, pnMain.Width - p.Width, pnMain.Height - p.Width);

            // Dispose
            brBlack.Dispose();
            brRed.Dispose();
            brWhite.Dispose();
            brStatusRunning.Dispose();
            fCenter.Dispose();
            fRight.Dispose();
            fLeft.Dispose();
            p.Dispose();
        }
    }
}
