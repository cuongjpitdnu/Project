using BaseCommon;
using BaseCommon.Core;
using DSF602.Language;
using DSF602.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSF602.View
{
    public partial class MapDevices : BaseForm
    {
        public MapDevices()
        {
            InitializeComponent();
            this.Text = LanguageHelper.GetValueOf("MAIN_BTN_DEVICE");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CreateSensorMap();
        }

        private void CreateSensorMap()
        {
            var w_sensor = plnMain.Width / 9;
            var h_sensor = (int)(w_sensor * 0.3125);
            var h_header = (int)(h_sensor * 1.2);
            var pad = 5;
            
            this.SuspendLayout();
            plnMain.Controls.Clear();

            // Header
            Label lbCommon = new Label()
            {
                Text = LanguageHelper.GetValueOf("MAP_HEADER"),
                BackColor = Color.FromArgb(30, 144, 255),
                ForeColor = Color.White,
                Size = new Size() { Width = w_sensor - pad, Height = h_header - pad },
                Location = new Point(0, 0),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter
            };
            plnMain.Controls.Add(lbCommon);

            for (int i = 0; i < 8; i++)
            {
                Label lbHeader = new Label()
                {
                    Text = string.Format("#{0}", i + 1),
                    BackColor = Color.FromArgb(30, 144, 255),
                    ForeColor = Color.White,
                    Size = new Size() { Width = w_sensor - pad, Height = h_header - pad },
                    Location = new Point((i + 1) * w_sensor, 0),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                plnMain.Controls.Add(lbHeader);
            }

            var LstBlock = AppManager.ListBlock;
            for (var i = 0; i < LstBlock.Count; i++)
            {
                Block dvc = LstBlock[i];
                // Block info
                BlockLabel lbBlock = new BlockLabel()
                {
                    Text = dvc.BlockName,
                    BackColor = dvc.Active == clsConst.ACTIVE ? Color.White : Color.LightGray,
                    ForeColor = dvc.Active == clsConst.ACTIVE ? Color.Black : Color.White,
                    Size = new Size() { Width = w_sensor - pad, Height = h_sensor - pad },
                    Location = new Point(0, i * h_sensor + h_header),
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleCenter,
                    BorderStyle = BorderStyle.FixedSingle,
                    Tag = dvc
                };
                plnMain.Controls.Add(lbBlock);

                // List sensor

                var subListSensor = AppManager.ListSensor.Where(s => s.OfBlock == dvc.BlockId).ToList();
                for (int j = 0; j < subListSensor.Count; j++)
                {
                    SensorInfo item = subListSensor[j];
                    Color backColor = Color.FromArgb(112, 173, 71);
                    if (item.Active == clsConst.ACTIVE && dvc.Active == clsConst.ACTIVE)
                    {
                        if (item.Alarm)
                        {
                            backColor = Color.Red;
                        }
                    }
                    else
                    {
                        backColor = Color.LightGray;
                    }

                    SensorLabel lbl = new SensorLabel()
                    {
                        AutoSize = false,
                        Size = new Size() { Width = w_sensor - 5, Height = h_sensor - 5 },
                        BackColor = backColor,
                        //Margin = new System.Windows.Forms.Padding(5),
                        Text = item.SensorName,
                        TextAlign = ContentAlignment.MiddleCenter,
                        ForeColor = Color.White,
                        Location = new Point((j + 1) * w_sensor, i * h_sensor + h_header),
                        BorderStyle = BorderStyle.FixedSingle,
                        Tag = item
                    };
                    lbl.Click += Lbl_Click;
                    plnMain.Controls.Add(lbl);
                }
            }

            plnMain.Width = w_sensor * 8 + 25;
            plnMain.HorizontalScroll.Maximum = 1000;
            plnMain.AutoScroll = false;
            plnMain.VerticalScroll.Visible = false;
            plnMain.AutoScroll = true;
            plnMain.Width = w_sensor * 9;
            plnMain.Height = h_sensor * 10 + h_header;
            var pLoc = new Point(Math.Abs(this.Size.Width - plnMain.Size.Width) / 2, Math.Abs(this.Size.Height - plnMain.Size.Height) / 2);
            plnMain.Location = new Point(pLoc.X - 5, pLoc.Y - 20);
            //plnMain.PerformLayout();
            this.ResumeLayout(false);
        }

        private void Lbl_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            SensorInfo info = lbl.Tag as SensorInfo;
            AppManager.OnSensorSelected?.Invoke(info, e);
        }
    }

    public class SensorLabel : Label
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AppManager.OnSensorInfoRefresh += OnSensorInfoRefresh;
            AppManager.OnSettingChanged += OnSettingChanged;
        }

        private void OnSettingChanged(object sender, EventArgs e)
        {
            ObjChanged obj = sender as ObjChanged;
            SensorInfo info = this.Tag as SensorInfo;
            if (obj.ListSensorIdChanged.Contains(info.SensorId))
            {
                SensorInfo sensorUpdate = AppManager.ListSensor.FirstOrDefault(i => i.SensorId == info.SensorId);
                this.Tag = sensorUpdate;
                DrawValue(sensorUpdate);
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            AppManager.OnSensorInfoRefresh -= OnSensorInfoRefresh;
            AppManager.OnSettingChanged -= OnSettingChanged;
        }

        private void OnSensorInfoRefresh()
        {
            if (!this.IsHandleCreated) return;
            var inf = this.Tag as SensorInfo;
            inf = AppManager.ListSensor.FirstOrDefault(i => i.SensorId == inf.SensorId);
            this.Tag = inf;
            try
            {
                DrawValue(inf);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void DrawValue(SensorInfo inf)
        {
            var oldInfo = this.Tag as SensorInfo;
            if (inf.SensorId != oldInfo.SensorId) return;

            var dvc = AppManager.ListBlock.FirstOrDefault(b => b.BlockId == oldInfo.OfBlock);

            Color backColor = Color.LightGray;
            if (inf.Active == clsConst.ACTIVE && dvc != null && dvc.Active == clsConst.ACTIVE)
            {
                if (inf.Alarm)
                {
                    backColor = Color.Red;
                }
                else
                {
                    backColor = Color.FromArgb(112, 173, 71);
                }
            }

            this.Tag = inf;
            this.Text = inf.SensorName;
            BackColor = backColor;
        }
    }

    public class BlockLabel : Label
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AppManager.OnSensorInfoRefresh += OnInfoRefresh;
            AppManager.OnSettingChanged += OnSettingChanged;
        }

        private void OnInfoRefresh()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    RefreshBlockInfo();
                }));
            }
            else
            {
                RefreshBlockInfo();
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            AppManager.OnSensorInfoRefresh -= OnInfoRefresh;
            AppManager.OnSettingChanged -= OnSettingChanged;
        }

        private void OnSettingChanged(object sender, EventArgs e)
        {
            ObjChanged obj = sender as ObjChanged;
            Block bl = this.Tag as Block;
            if (obj.ListBlockIdChanged.Contains(bl.BlockId))
            {
                Block blUpdate = AppManager.ListBlock.FirstOrDefault(b => b.BlockId == bl.BlockId);
                this.Tag = blUpdate;
                RefreshBlockInfo();
            }
        }

        private void RefreshBlockInfo()
        {
            if (!this.IsHandleCreated) return;

            Block ollBlock = this.Tag as Block;
            Block bl = AppManager.ListBlock.FirstOrDefault(i => i.BlockId == ollBlock.BlockId);

            Text = bl.BlockName;
            BackColor = bl.Active == clsConst.ACTIVE ? Color.White : Color.LightGray;
            ForeColor = bl.Active == clsConst.ACTIVE ? Color.Black : Color.White;
        }
    }
}
