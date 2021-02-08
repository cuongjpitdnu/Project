using BaseCommon.Core;
using DSF602.Model;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DSF602.View.GraphLayout
{
    public partial class MapLocation : BaseUserControl
    {
        public const int MAP_PADDING = 3;
        public const int COLUMN_COUNT = 8;

        public delegate void EventClick(object data, EventArgs e);
        public event EventClick OnSelectedSensor;
        public MapLocation()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);
            this.UpdateStyles();
            GeneralBlockMap();
        }

        private bool _allowRunningUpdate = false;
        public bool AllowRuningUpdate
        {
            get { return _allowRunningUpdate; }
            set
            {
                _allowRunningUpdate = value;
                foreach (Control ctr in plnMain.Controls)
                {
                    if (ctr.GetType() == typeof(BlockView))
                    {
                        ((BlockView)ctr).AllowRuningUpdate = value;
                    }
                }
            }
        }

        private void GeneralBlockMap()
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)(() =>
                {
                    DisplayAllBlock();
                }));
            }
            else
            {
                DisplayAllBlock();
            }
        }

        private void DisplayAllBlock()
        {
            var lstBlock = AppManager.ListBlock;
            var w_sensor = 136;
            var w_parent = 1088;
            var h_device = 171;
            plnMain.SuspendLayout();
            plnMain.Controls.Clear();

            for (var i = 0; i < lstBlock.Count; i++)
            {
                Block dvc = lstBlock[i];
                BlockView dvcView = new BlockView()
                {
                    Width = w_sensor * 8,
                    Location = new Point(0, i * h_device),
                };
                dvcView.DisplayBlockInfo(dvc);
                dvcView.DisplaySensorInfo();
                dvcView.OnSelectedItem += OnSensor_Selected;
                plnMain.Controls.Add(dvcView);
            }

            
            plnMain.Width = w_parent;
            plnMain.VerticalScroll.Maximum = 100;
            plnMain.VerticalScroll.SmallChange = 1;
            

            plnMain.PerformLayout();
            plnMain.ResumeLayout(false);

            plnMain.AutoScroll = true;
        }

        private void OnSensor_Selected(SensorInfo sender)
        {
            AppManager.ShowDialog<MeasureManagement>(sender, null);
        }

        private void label_Click(object sender, EventArgs e)
        {
            OnSelectedSensor?.Invoke(sender, e);
        }

        public void CalculateLocation()
        {
            this.SuspendLayout();
            plnMain.Location = new Point(Math.Abs(this.Size.Width - plnMain.Size.Width) / 2, Math.Abs(this.Size.Height - plnMain.Size.Height) / 2);
            this.PerformLayout();
            this.ResumeLayout(false);
        }

        private void plnMain_Scroll(object sender, ScrollEventArgs e)
        {
            plnMain.SuspendLayout();
            plnMain.VerticalScroll.Value = e.NewValue;
            plnMain.PerformLayout();
            plnMain.ResumeLayout();
        }
    }
}
