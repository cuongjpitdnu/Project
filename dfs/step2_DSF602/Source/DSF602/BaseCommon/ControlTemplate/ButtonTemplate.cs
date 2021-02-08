using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BaseCommon.ControlTemplate
{
    public partial class btn : Button
    {
        public enum emSize
        {
            Small,
            Medium,
            Large,
            Custom,
        }

        private emSize _typeSize;
        private Size[] _arrSize = new Size[] { new Size(60, 30), new Size(100, 40), new Size(160, 80) };

        [Description("Change size template"), Category("Custom")]
        public emSize TypeSize
        {
            get { return _typeSize; }
            set
            {
                _typeSize = value;

                if (_typeSize != emSize.Custom)
                {
                    this.Size = _arrSize[(int)value];
                }
            }
        }

        [Description("Add Shortcut"), Category("Custom")]
        public Keys Shortcut { get; set; }

        public btn()
        {
            InitializeComponent();

            this.TypeSize = emSize.Small;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void ButtonBase_Resize(object sender, System.EventArgs e)
        {
            if (this.Size == _arrSize[(int)emSize.Small])
            {
                _typeSize = emSize.Small;
            }
            else if (this.Size == _arrSize[(int)emSize.Medium])
            {
                _typeSize = emSize.Medium;
            }
            else if (this.Size == _arrSize[(int)emSize.Large])
            {
                _typeSize = emSize.Large;
            }
            else
            {
                _typeSize = emSize.Custom;
            }
        }
    }
}
