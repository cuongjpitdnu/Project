using GPMain.Common.Interface;
using SkiaSharp;
using System;
using System.Windows.Forms;

namespace GPMain.Views.Controls
{
    public class SKGLControl : SkiaSharp.Views.Desktop.SKGLControl, ISKControl
    {
        public event EventHandler<SKCanvas> PaintSurfaceCanvas;

        public SKGLControl() : base()
        {
            //this.VSync = true;
            this.PaintSurface += (sender, e) => PaintSurfaceCanvas?.Invoke(sender, e.Surface.Canvas);
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    this.MakeCurrent();
        //    base.OnPaint(e);
        //}
    }
}