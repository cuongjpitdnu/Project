using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public partial class MaterialDropDownColorPicker : DropDownControl, IMaterialControl
    {
        [Browsable(false)]
        public int Depth { get; set; }
        [Browsable(false)]
        public MaterialSkinManager SkinManager { get { return MaterialSkinManager.Instance; } }
        [Browsable(false)]
        public MouseState MouseState { get; set; }

        public MaterialColorPicker ColorControl;// chuyển phạm vi biến từ private sang public - Hải 29/07/2020
        private Color _Color;
        private Rectangle ColorRect;
        public Color BackColor { get { return Parent == null ? SkinManager.BackgroundColor : Parent.BackColor; } set { } }
        public Color Color
        {
            get { return _Color; }
            set
            {
                _Color = value; ColorControl.Value = _Color;
            }
        }
        public MaterialDropDownColorPicker()
        {
            InitializeComponent();
            ColorControl = new MaterialColorPicker();
            Color = SkinManager.ColorScheme.AccentColor;
            ColorControl.onColorChanged += objDateControl_onDateChanged;

            InitializeDropDown(ColorControl);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            ColorRect = new Rectangle();
            ColorRect.Location = new Point(1, 1);
            ColorRect.Size = new Size((int)(Width - 18), (int)(Height * 0.8));

            e.Graphics.FillRectangle(new SolidBrush(Color), ColorRect);
        }

        void objDateControl_onDateChanged(Color newColor)
        {
            Color = newColor;
            this.BackColor = newColor;
            Invalidate();
        }
    }
}
