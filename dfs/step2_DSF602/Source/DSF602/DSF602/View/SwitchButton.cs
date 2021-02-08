using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSF602.View
{
    public partial class SwitchButton : UserControl
    {
        public SwitchButton()
        {
            InitializeComponent();
        }

        private int _status = 0;
        public int Status
        {
            get { return _status; }
            set
            {
                _status = value;
                Invalidate();
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.X <= this.Width / 2)
            {
                // ON
            }
            else
            {
                // OFF
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.Clear(Color.Transparent);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            Pen p = new Pen(Color.White);
            g.DrawRectangle(p, new Rectangle(0, 0, this.Width, this.Height));

            p.Dispose();
        }
    }
}
