using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSF602.View
{
    public class MyPanel : System.Windows.Forms.Panel
    {
        public MyPanel()
        {
            if (!DesignMode)
            {
                DoubleBuffered = true;
                Initialize();
            }
        }
        private void Initialize()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
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
    }
}
