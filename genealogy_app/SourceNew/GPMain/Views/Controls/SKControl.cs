using GPMain.Common.Interface;
using SkiaSharp;
using System;

namespace GPMain.Views.Controls
{
    public class SKControl : SkiaSharp.Views.Desktop.SKControl, ISKControl
    {
        public event EventHandler<SKCanvas> PaintSurfaceCanvas;

        public SKControl() : base()
        {
            PaintSurface += (sender, e) => PaintSurfaceCanvas?.Invoke(sender, e.Surface.Canvas);
        }
    }
}