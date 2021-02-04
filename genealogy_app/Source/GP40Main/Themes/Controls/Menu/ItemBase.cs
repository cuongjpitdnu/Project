using GP40DrawTree;
using System;
using System.Windows.Forms;

namespace GP40Main.Themes.Controls
{
    public delegate void EventUserControlDelagate(DrawTreeConfig config);

    /// <summary>
    /// Meno        : Menu Item Base
    /// Create by   : 2020.07.30 AKB Nguyen Thanh Tung
    /// </summary>
    public class ItemBase : UserControl
    {
        private bool _runningReload;

        public event EventUserControlDelagate ChangeData;

        public DrawTreeConfig Config { get; private set; }

        public ItemBase()
        {
            Config = new DrawTreeConfig();
        }

        public ItemBase(DrawTreeConfig config = null)
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor, false);

            this.Load += (sender, e) => ReloadConfig(config);
        }

        protected void ChangeData_Event()
        {
            if (_runningReload)
            {
                return;
            }

            SetConfig();

            if (ChangeData != null)
            {
                this.BeginInvoke(ChangeData, new object[] { Config });
            }
        }

        protected virtual void SetConfig()
        {
        }

        protected virtual void SetDisplayUI()
        {
        }

        public void ReloadConfig(DrawTreeConfig config)
        {
            _runningReload = true;
            Config = config ?? new DrawTreeConfig();
            SetDisplayUI();
            Refresh();
            _runningReload = false;
        }
    }
}