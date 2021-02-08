using BaseCommon;
using DSF602.Model;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BaseCommon.clsConst;
using Timer = System.Windows.Forms.Timer;

namespace DSF602.View.GraphLayout
{
    public partial class DisplaySensor : UserControl
    {
        public Label LableSensorName => lblSensorName;

        public int maxValue;

        private SensorInfo _sensor;

        private bool showFuntion;

        private Timer tmPos;
        private Timer tmNeg;
        private Timer tmIon;
        private double counterPos = 0;
        private double counterNeg = 0;
        private DateTime endtimePos = DateTime.Now;
        private DateTime endtimeNeg = DateTime.Now;
        private int lowValue = 0;
        private Color sensorColor = Color.Yellow;
        public DisplaySensor()
        {
            InitializeComponent();
            lblSensorName.Text = string.Empty;
            lblValueSensor.Text = string.Empty;
            lblPeak.Text = string.Empty;
            //lbAlarm.ForeColor = Color.White;
            lblValueSensor.ForeColor = Color.White;
            lblPeak.ForeColor = Color.White;
            lbDecayPositive.ForeColor = Color.White;
            lbDecayNegative.ForeColor = Color.White;
            lbIonCheck.ForeColor = Color.White;

            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            tmPos = new Timer();
            tmPos.Interval = 100;
            tmPos.Tick += TimerPositive_Tick;

            tmNeg = new Timer();
            tmNeg.Interval = 100;
            tmNeg.Tick += TimerNegative_Tick;

            tmIon = new Timer();
            tmIon.Interval = 100;
            tmIon.Tick += TimerIon_Tick;
        }

        private void TimerIon_Tick(object sender, EventArgs e)
        {
            tmIon.Stop();
            var frmMain = this.ParentForm as MainForm;
            if (frmMain != null && _sensor != null)
            {
                frmMain.Charge(_sensor.SensorId, MeasureState.StopIon);
            }
        }

        private void TimerNegative_Tick(object sender, EventArgs e)
        {
            //counterNeg += 0.1;
            if (_sensor != null && _sensor.MeasureState == MeasureState.Negative)
            {
                if (_sensor.ActualValue >= (-1 * _sensor.DecayLowerValue))
                {
                    StopTimerNegative(counterNeg > _sensor.DecayTimeCheck);
                }
                else
                {
                    _sensor.NegativePoint.Add(_sensor.ActualValue);
                    //lbDecayNagative.Text = "DECAY(-):" + string.Format("{0} sec", counterNeg.ToString("N1"));
                }
            }

        }

        private void StopTimerNegative(bool isAlarm)
        {
            tmNeg.Stop();
            var frmMain = this.ParentForm as MainForm;
            if (frmMain != null && _sensor != null)
            {
                if (isAlarm)
                {
                    SetFail();
                }
                frmMain.Charge(_sensor.SensorId, MeasureState.StopNegative, isAlarm);

                Task.Run(() =>
                {
                    Thread.Sleep(2000);

                    frmMain.Invoke(new MethodInvoker(() =>
                    {
                        frmMain.Charge(_sensor.SensorId, MeasureState.Ion);
                        tmIon.Interval = _sensor.IonTimeCheck * 1000 + 2500;
                        tmIon.Start();
                    }));
                });
            }
        }

        private void TimerPositive_Tick(object sender, EventArgs e)
        {
            //counterPos += 0.1;
            if (_sensor != null && _sensor.MeasureState == MeasureState.Positive)
            {
                if (_sensor.ActualValue <= _sensor.DecayLowerValue)
                {
                    StopTimerPos(counterPos > _sensor.DecayTimeCheck);
                }
                else
                {
                    _sensor.PositivePoint.Add(_sensor.ActualValue);
                    //lbDecayPositive.Text = "DECAY(+):" + string.Format("{0} sec", counterPos.ToString("N1"));
                }
            }
        }

        private void StopTimerPos(bool isAlarm)
        {
            tmPos.Stop();
            var frmMain = this.ParentForm as MainForm;
            if (frmMain != null && _sensor != null)
            {
                if (isAlarm)
                {
                    SetFail();
                }
                frmMain.Charge(_sensor.SensorId, MeasureState.StopPositive, isAlarm);

                endtimeNeg = DateTime.Now.AddSeconds(_sensor.DecayStopTime);
                frmMain.Charge(_sensor.SensorId, MeasureState.Negative);
            }
        }

        private void SetFail()
        {
            if (_sensor != null)
            {
                _sensor.Result_Measure = (int)clsConst.emMeasureResult.Fail;
                _sensor.Alarm = true;
            }
        }

        public void SetSensorName(SensorInfo sensor)
        {
            //_sensor = sensor;
            //if (lblSensorName.InvokeRequired)
            //{
            //    lblSensorName.BeginInvoke((MethodInvoker)(() =>
            //    {
            //        FillData();
            //    }));
            //}
            //else
            //{
            //    if (sensor.Alarm)
            //    {
            //        Task.Run(async () =>
            //        {
            //            lblSensorName.BackColor = Color.Red;
            //            await Task.Delay(1000);
            //            lblSensorName.BackColor = Color.Yellow;
            //            await Task.Delay(1000);
            //        });
            //        Application.DoEvents();
            //    }

            //    FillData();
            //}
            _sensor = sensor;
            if (sensor.Alarm)
            {
                Console.WriteLine(DateTime.Now.ToString("HH:mm:ss ffff"));
                lblSensorName.BackColor = lblSensorName.BackColor == Color.Red ? Color.Yellow : Color.Red;
                //sensorColor = Color.Red;
            }
            else
            {
                lblSensorName.BackColor = Color.Yellow;
                //sensorColor = Color.Yellow;
            }
            
            FillData();
        }


        private void FillData()
        {
            if (_sensor == null) return;
            lblSensorName.Text = _sensor.GraphName;

            //if (_sensor.AlarmStatus)
            //{
            //    btnON.BackColor = Color.Yellow;
            //    btnON.FlatAppearance.BorderColor = Color.Yellow;
            //    btnON.ForeColor = Color.Black;
            //    btnON.FlatAppearance.MouseOverBackColor = Color.Yellow;
            //    btnON.FlatAppearance.MouseDownBackColor = Color.Yellow;

            //    btnOFF.BackColor = Color.Transparent;
            //    btnOFF.FlatAppearance.BorderColor = Color.White;
            //    btnOFF.ForeColor = Color.LightGray;
            //    btnOFF.FlatAppearance.MouseOverBackColor = Color.Transparent;
            //    btnOFF.FlatAppearance.MouseDownBackColor = Color.Transparent;
            //}
            //else
            //{
            //    btnOFF.BackColor = Color.FromArgb(0, 32, 96);
            //    btnOFF.FlatAppearance.BorderColor = Color.Yellow;
            //    btnOFF.ForeColor = Color.LightGray;
            //    btnOFF.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 32, 96);
            //    btnOFF.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 32, 96);

            //    btnON.BackColor = Color.Transparent;
            //    btnON.FlatAppearance.BorderColor = Color.White;
            //    btnON.ForeColor = Color.LightGray;
            //    btnON.FlatAppearance.MouseOverBackColor = Color.Transparent;
            //    btnON.FlatAppearance.MouseDownBackColor = Color.Transparent;
            //}

            if (!showFuntion || _sensor.MeasureType != clsConst.MeasureMode_Decay)
            {
                pnDecay.Visible = false;
                pnFunction.Visible = false;
                lblPeak.Visible = true;
            }
            else
            {
                pnDecay.Visible = true;
                pnFunction.Visible = true;
                lblPeak.Visible = false;
            }

            if (_sensor.ActualValue >= _sensor.DecayUpperValue && _sensor.MeasureState == MeasureState.Positive)
            {
                tmPos.Start();
            }

            if (_sensor.ActualValue <= (-1 * _sensor.DecayUpperValue) && _sensor.MeasureState == MeasureState.Negative)
            {
                tmNeg.Start();
            }

            if (endtimePos < DateTime.Now && _sensor.MeasureState == MeasureState.Positive)
            {
                StopTimerPos(true);
            }
            if (endtimeNeg < DateTime.Now && _sensor.MeasureState == MeasureState.Negative)
            {
                StopTimerNegative(true);
            }
        }


        public void DisplayValueSensor(int valueSensor)
        {
            if (maxValue < Math.Abs(valueSensor))
            {
                maxValue = Math.Abs(valueSensor);
            }

            if (lblValueSensor.InvokeRequired)
            {
                lblValueSensor.BeginInvoke((MethodInvoker)(() =>
                {
                    //lblValueSensor.ForeColor = Color.FromArgb(0, 255, 0);
                    lblValueSensor.Text = string.Format("VALUE: {0}", clsCommon.FormatValue(valueSensor));

                    //lblPeak.ForeColor = Color.FromArgb(0, 255, 0);
                    lblPeak.Text = string.Format("PEAK: {0} ", maxValue.ToString());

                }));
            }
            else
            {
                //lblValueSensor.ForeColor = Color.FromArgb(0, 255, 0);
                lblValueSensor.Text = string.Format("VALUE: {0}", clsCommon.FormatValue(valueSensor));

                //lblPeak.ForeColor = Color.FromArgb(0, 255, 0);
                lblPeak.Text = string.Format("PEAK: {0}", maxValue.ToString());
            }
        }

        public void DisplayDecayValue(double decayValue, clsConst.MeasureState measureState)
        {
            if (measureState == MeasureState.Positive)
            {
                counterPos = decayValue;
                SetDecayLabel(decayValue.ToString("#,##0.00"), lbDecayPositive);
            }
            else if (measureState == MeasureState.Negative)
            {
                counterNeg = decayValue;
                SetDecayLabel(decayValue.ToString("#,##0.00"), lbDecayNegative);
            }else if (measureState == MeasureState.StopIon)
            {
                SetDecayLabel(((int)decayValue).ToString(), lbIonCheck);
                var frmMain = this.ParentForm as MainForm;
                if (frmMain != null && _sensor != null)
                {
                    frmMain.Charge(_sensor.SensorId, MeasureState.None);
                }
            }
        }

        private void SetDecayLabel(string val, Label lb)
        {
            if (lb.InvokeRequired)
            {
                lb.BeginInvoke((MethodInvoker)(() =>
                {
                    lb.Text = string.Format("" + lb.Tag, val);
                }));
            }
            else
            {
                lb.Text = string.Format("" + lb.Tag, val);
            }
        }


        public void ResetDisplayValueSensor()
        {
            if (lblValueSensor.InvokeRequired)
            {
                lblValueSensor.BeginInvoke((MethodInvoker)(() =>
                {
                    lblValueSensor.Text = string.Empty;
                    lblPeak.Text = string.Empty;
                }));
            }
            else
            {
                lblValueSensor.Text = string.Empty;
                lblPeak.Text = string.Empty;
            }
        }

        private void btnCharge_Click(object sender, EventArgs e)
        {
            if (_sensor == null) return;
            var frmMain = this.ParentForm as MainForm;
            if (frmMain != null)
            {
                if (frmMain.isDecayRuning)
                {
                    frmMain.ShowMsg(MessageBoxIcon.Warning, "Has one device is running. Please waiting!");
                    return;
                }
            }
            else
            {
                return;
            }

            SettingParam pr = new SettingParam()
            {
                UpVal = _sensor.DecayUpperValue,
                LowVal = _sensor.DecayLowerValue,
                DecayTimeCheck = _sensor.DecayTimeCheck,
                StopDecayTime = _sensor.DecayStopTime,
                IonBalanceCheck = _sensor.IonValueCheck,
                IonStopTimeCheck = _sensor.IonTimeCheck
            };
            var dlgResult = AppManager.ShowDialog<ChargingConfirmPopup>(pr);
            if (dlgResult == null) return;

            // Reset timer
            tmPos.Stop();
            tmNeg.Stop();
            counterPos = 0;
            counterNeg = 0;
            endtimePos = DateTime.Now.AddSeconds(_sensor.DecayStopTime);
            SetDecayLabel("0", lbDecayPositive);
            SetDecayLabel("0", lbDecayNegative);
            SetDecayLabel("0", lbIonCheck);
            

            if (_sensor.PositivePoint == null)
            {
                _sensor.PositivePoint = new System.Collections.Generic.List<int>();
            }
            if (_sensor.NegativePoint == null)
            {
                _sensor.NegativePoint = new System.Collections.Generic.List<int>();
            }

            _sensor.PositivePoint.Clear();
            _sensor.NegativePoint.Clear();
            _sensor.GraphData?.Samples?.Clear();
            _sensor.Alarm = false;

            SensorInfo sensorDB = AppManager.ListSensor.Find(i => i.SensorId == _sensor.SensorId);

            if (sensorDB != null)
            {
                sensorDB.PositivePoint.Clear();
                sensorDB.NegativePoint.Clear();
                sensorDB.GraphData?.Samples?.Clear();
            }
            
            // Charge
            if (frmMain != null && _sensor != null)
            {
                frmMain.Charge(_sensor.SensorId, MeasureState.Positive, false);
            }
        }

        public void DisplayChargeButton(bool isShow)
        {
            showFuntion = isShow;

            //if (showFuntion)
            //{
            //    pnFunction.Dock = DockStyle.Right;
            //}
            //if (pnFunction.Visible)
            //{
            //    pnFunction.Location = new Point(this.Width - pnFunction.Width, 0);
            //}
        }

        private void lblSensorName_Paint(object sender, PaintEventArgs e)
        {
            if (_sensor == null) return;
            Graphics g = e.Graphics;
            g.Clear(Color.Transparent);
            g.SmoothingMode = SmoothingMode.HighQuality;
            
            using (SolidBrush b = new SolidBrush(lblSensorName.BackColor))
            {
                Rectangle rec = lblSensorName.ClientRectangle;
                rec.Height = rec.Height - 2;
                g.FillPath(b, RoundedRect(rec, 10));
            }
                
            using (SolidBrush b = new SolidBrush(Color.Black))
            {
                PointF p = new PointF()
                {
                    X = lblSensorName.Width / 2,
                    Y = lblSensorName.Height / 2,
                };
                g.DrawString(lblSensorName.Text, lblSensorName.Font, b, p, new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
        }

        public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void btnData_Click(object sender, EventArgs e)
        {
            AppManager.ShowDialog<MeasureManagement>(_sensor, null);
        }

        private void btnON_Click(object sender, EventArgs e)
        {
            if (_sensor == null || _sensor.AlarmStatus) return;
            _sensor.AlarmStatus = !_sensor.AlarmStatus;
        }

        private void btnOFF_Click(object sender, EventArgs e)
        {
            if (_sensor == null || !_sensor.AlarmStatus) return;
            _sensor.AlarmStatus = !_sensor.AlarmStatus;
        }

        //private void myPanel1_Paint(object sender, PaintEventArgs e)
        //{
        //    Graphics g = e.Graphics;
        //    g.Clear(Color.Transparent);
        //    g.SmoothingMode = SmoothingMode.HighQuality;

        //    Pen p = new Pen(Color.Yellow);
        //    var rec = new Rectangle(0, 0, myPanel1.Width - 1, myPanel1.Height - 1);
        //    g.DrawPath(p, GetRoundPath(rec, 15));
        //    p.Dispose();
        //}

        GraphicsPath GetRoundPath(RectangleF Rect, int radius)
        {
            float r2 = radius / 2f;
            GraphicsPath GraphPath = new GraphicsPath();
            GraphPath.AddArc(Rect.X, Rect.Y, radius, radius, 180, 90);
            GraphPath.AddLine(Rect.X + r2, Rect.Y, Rect.Width - r2, Rect.Y);
            GraphPath.AddArc(Rect.X + Rect.Width - radius, Rect.Y, radius, radius, 270, 90);
            GraphPath.AddLine(Rect.Width, Rect.Y + r2, Rect.Width, Rect.Height - r2);
            GraphPath.AddArc(Rect.X + Rect.Width - radius,
                             Rect.Y + Rect.Height - radius, radius, radius, 0, 90);
            GraphPath.AddLine(Rect.Width - r2, Rect.Height, Rect.X + r2, Rect.Height);
            GraphPath.AddArc(Rect.X, Rect.Y + Rect.Height - radius, radius, radius, 90, 90);
            GraphPath.AddLine(Rect.X, Rect.Height - r2, Rect.X, Rect.Y + r2);
            GraphPath.CloseFigure();
            return GraphPath;
        }
    }
}
