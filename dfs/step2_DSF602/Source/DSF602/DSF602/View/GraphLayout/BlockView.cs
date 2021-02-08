using System;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DSF602.Model;
using BaseCommon;
using DSF602.Language;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace DSF602.View.GraphLayout
{
    public partial class BlockView : UserControl
    {
        public delegate void OnClickDelegate(SensorInfo sender);
        public OnClickDelegate OnSelectedItem;

        string sensorTitle;
        string prodTitle;
        string statusTitle;

        private Font f;
        public BlockView()
        {
            InitializeComponent();
            f = new Font(lbDeviceTitle.Font, FontStyle.Bold);
            foreach (Control control in Controls)
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }

            GetTitle();
        }

        private bool _allowRunningUpdate = false;
        public bool AllowRuningUpdate
        {
            get { return _allowRunningUpdate; }
            set { _allowRunningUpdate = value;
                foreach (Control ctr in pnSensor.Controls)
                {
                    if (ctr.GetType() == typeof(SensorItemView))
                    {
                        ((SensorItemView)ctr).AllowRuningUpdate = value;
                    }
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AppManager.OnLanguageChanged += OnLanguageChanged;
            AppManager.OnSensorInfoRefresh += OnSensorInfoRefresh;
            AppManager.OnSettingChanged += OnSettingChanged;
        }

        private void OnSettingChanged(object sender, EventArgs e)
        {
            ObjChanged obj = sender as ObjChanged;
            if (obj.ListBlockIdChanged.Contains(BlockInfo.BlockId))
            {
                Block blUpdate = AppManager.ListBlock.FirstOrDefault(bl => bl.BlockId == BlockInfo.BlockId);
                DisplayBlockInfo(blUpdate);
            }
        }

        private void GetTitle()
        {
            sensorTitle = LanguageHelper.GetValueOf("BLOCK_SENSORS");
            prodTitle = LanguageHelper.GetValueOf("BLOCK_PRODUCT");
            statusTitle = LanguageHelper.GetValueOf("BLOCK_STATUS");
        }

        private void OnSensorInfoRefresh()
        {
            if (!AllowRuningUpdate) return;

            Block bl = AppManager.ListBlock.FirstOrDefault(i => i.BlockId == BlockInfo.BlockId);
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    DisplayBlockInfo(bl);
                }));
            }
            else
            {
                DisplayBlockInfo(bl);
            }
        }

        private void OnLanguageChanged(object sender, EventArgs e)
        {
            GetTitle();
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    DisplayBlockInfo(BlockInfo);
                    //DisplaySensorInfo();
                }));
            }
            else
            {
                DisplayBlockInfo(BlockInfo);
                //DisplaySensorInfo();
            }
        }

        private Model.Block BlockInfo;

        public void DisplayBlockInfo(Block bl)
        {
            BlockInfo = bl;
            if (BlockInfo.Active == clsConst.NOT_ACTIVE) return;
            lbDeviceTitle.Update();
        }

        public void DisplaySensorInfo()
        {
            if (BlockInfo == null) return;
            var lstSensor = AppManager.ListSensor.Where(s => s.OfBlock == BlockInfo.BlockId).ToList();
            var w_sensor = 133;
            pnSensor.Controls.Clear();
            for (var j = 0; j < lstSensor.Count; j++)
            {
                SensorInfo inf = lstSensor[j];
                var objPanelContent = new SensorItemView()
                {
                    Info = inf,
                    Location = new Point(j * w_sensor, 0),
                    Anchor = (((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right)
                };

                pnSensor.Controls.Add(objPanelContent);

                //event click label
                objPanelContent.OnClick += new EventHandler((sender, e) =>
                {
                    SensorInfo itemSelected = sender as SensorInfo;
                    OnSelectedItem?.Invoke(itemSelected);
                });
            }
        }

        private void lbDeviceTitle_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(lbDeviceTitle.BackColor);

            var lstSensor = AppManager.ListSensor.Where(s => s.OfBlock == BlockInfo.BlockId).ToList();
            int countRunning = lstSensor.Where(i => i.Active == clsConst.ACTIVE && i.Alarm == false).Count();
            string dvcName = BlockInfo.BlockName;
            string sensorCount = lstSensor.Count + " " + sensorTitle;
            string prodName = prodTitle + ": DSF602";
            string status = statusTitle + ": " + countRunning * 100 / lstSensor.Count + "%";
            if (BlockInfo.Active == clsConst.NOT_ACTIVE)
            {
                status = statusTitle + ": 0%";
            }
            string strTitle = string.Format("{0} | {1} | {2} | {3}", BlockInfo.BlockName, sensorCount, prodName, status);
            //Font f = new Font(lbDeviceTitle.Font, FontStyle.Bold);
            var sz = g.MeasureString(strTitle, f);

            using (var pen = new Pen(Color.Blue, 1))
            {
                Size recSize = new Size((int)sz.Width + 25, (int)sz.Height + 5);
                Point recPoint = new Point((int)(lbDeviceTitle.Width - recSize.Width) / 2, (int)(lbDeviceTitle.Height - recSize.Height) / 2);
                Rectangle arc = new Rectangle(recPoint, recSize);

                using (GraphicsPath path = RoundedRect(arc, 7))
                {
                    g.DrawPath(pen, path);
                }
            }

            using (var br = new SolidBrush(Color.Black))
            {
                var p = new PointF((lbDeviceTitle.Width - sz.Width) / 2, (lbDeviceTitle.Height - sz.Height) / 2);
                string str1 = string.Format("{0} | {1} | {2} | ", BlockInfo.BlockName, sensorCount, prodName);
                var mes = g.MeasureString(str1, f);

                g.DrawString(str1, f, br, p);
                br.Color = Color.FromArgb(63, 110, 194);
                g.DrawString(status, f, br, new PointF(p.X + mes.Width, p.Y));
            }
                
        }

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
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
    }
}
