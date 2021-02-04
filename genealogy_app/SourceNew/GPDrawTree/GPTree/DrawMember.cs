using GPCommon;
using GPModels;
using SkiaSharp;
using System;
using System.IO;

namespace GPTree
{
    public class DrawMember : IDisposable
    {
        private bool disposedValue;

        public string Id => MemberInfo.Id;
        public bool ReDraw { get; set; } = false;
        public float flZoom = 1;

        public TMember MemberInfo { get; set; }
        public SKPoint Position { get; set; }
        public SKSizeI Size { get; set; }
        public SKColor BackColor { get; set; }
        public SKColor BorderColor { get; set; }
        public SKColor TextColor { get; set; }
        public string TextFontName { get; set; }
        public string FrameImagePath { get; set; }
        public SKFilterQuality Quality { get; set; }
        public bool HasPosition { get; set; }
        public float MinTreeLeft { get; set; }
        public float MaxTreeRight { get; set; }

        public DrawMember(TMember member)
        {
            MemberInfo = member ?? new TMember();
            Position = new SKPoint(0, 0);
            Size = new SKSizeI(50, 50);
            Quality = SKFilterQuality.Medium;
        }

        internal bool HasChild()
        {
            return MemberInfo.ListCHILDREN.HasValue();
        }

        public void SetPositionX(float x)
        {
            Position = new SKPoint(x, Position.Y);
        }

        public void SetPositionY(float y)
        {
            Position = new SKPoint(Position.X, y);
        }

        public void ResetDataDraw()
        {

        }

        private void drawFrame(SKCanvas canvas)
        {
            canvas.Scale(flZoom);

            SKRect skBorder = SKRect.Create(Size);

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                //paint.StrokeWidth = 3;
                paint.Style = SKPaintStyle.StrokeAndFill;
                paint.Color = BackColor;
                canvas.DrawRoundRect(skBorder, new SKSize(1, 1), paint);

                paint.Color = BorderColor;
                paint.Style = SKPaintStyle.Stroke;
                canvas.DrawRoundRect(skBorder, new SKSize(1, 1), paint);
            }

            if (!string.IsNullOrEmpty(FrameImagePath) && File.Exists(FrameImagePath))
            {
                //Draw Frame
                using (SKBitmap frameBitmap = SKBitmap.Decode(FrameImagePath).Resize(Size, Quality))
                {
                    canvas.DrawBitmap(frameBitmap, 0, 0);
                }
            }
        }

        private void drawTextInfo(SKCanvas canvas)
        {

            canvas.Scale(flZoom);

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.IsStroke = false;
                paint.TextSize = 14f;
                paint.Typeface = SKTypeface.FromFamilyName("Arial");
                paint.TextEncoding = SKTextEncoding.Utf8;

                SKRect textBounds = new SKRect();
                paint.MeasureText("test", ref textBounds);

                // Calculate offsets to center the text on the screen
                float xText = 0 - textBounds.Location.X;
                float yText = 0 - textBounds.Location.Y;

                canvas.DrawText("test", xText, yText, paint);
            }
        }

        public SKPicture DrawingMemberSVG()
        {
            //var strFileName = clsCommon.GetMemberCardDataPath(enmTemplate) + miID + ".svg";

            //if (!blnOverwrite)
            //{
            //    if (LoadSVGMember(strFileName, true))
            //    {
            //        return;
            //    }
            //}

            byte[] dataContentSvg;

            using (var memory = new MemoryStream())
            {
                using (var canvas = SKSvgCanvas.Create(SKRect.Create(Size), memory))
                {
                    //// draw on the canvas ...
                    //drawCanvasAvatar(canvas);
                    //drawCanvasTextInfo(canvas);
                    drawFrame(canvas);
                    drawTextInfo(canvas);
                }

                dataContentSvg = memory.ToArray();
                //EncryptHelper.EncryptString(strFileName, Encoding.UTF8.GetString(dataContentSvg));
            }

            var svgData = new SkiaSharp.Extended.Svg.SKSvg();
            svgData.Load(new MemoryStream(dataContentSvg));

            return svgData.Picture;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DrawMember()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}