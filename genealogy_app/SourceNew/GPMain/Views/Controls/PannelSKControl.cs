using GP40DrawTree;
using SkiaSharp;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace GPMain.Views.Controls
{
    public partial class PannelSKControl : Panel
    {
        Control controlSK = null;
        public event EventHandler<SKCanvas> PaintSurfaceCanvas;

        public PannelSKControl(bool useGpu = false)
        {
            InitializeComponent();

            if (useGpu)
            {
                controlSK = new SKGLCustom();
            }
            else
            {
                controlSK = new SKCustom();
            }

            ((ISKControl)controlSK).PaintSurfaceCanvas += PaintSurfaceCanvas;

            this.Controls.Add(controlSK);
            controlSK.Dock = DockStyle.Fill;
        }

        public PannelSKControl(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }
    }
}
