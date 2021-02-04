using SkiaSharp;
using System;
using System.Windows.Forms;

namespace GPMain.Common.Interface
{
    public interface ISKControl
    {
        event EventHandler<SKCanvas> PaintSurfaceCanvas;

        //event MouseEventHandler MouseClick;

        //event MouseEventHandler MouseDoubleClick;

        //event MouseEventHandler MouseDown;

        //event MouseEventHandler MouseMove;

        //event MouseEventHandler MouseUp;

        //event MouseEventHandler MouseWheel;

        //event EventHandler MouseLeave;
    }
}