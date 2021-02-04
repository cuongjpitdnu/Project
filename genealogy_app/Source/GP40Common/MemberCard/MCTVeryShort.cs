using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkiaSharp;
using System.IO;
using System.Net.Http;

namespace GP40Common
{
    public partial class MemberCardTemplVeryShort : UserControl
    {
        public string mstrFullName;
        public int miTop;
        public int miLeft;
        public int miWidth;
        public int miHeight;
        public string strColor;
        public string strFramePath = @".\Docs\frames\";
        public string strFrameFile;
        public int miFrameIndex;
        SKSurface mskInfo = null;

        public SKSurface InfoSurface   // property
        {
            get { return mskInfo; }   // get method          
        }

        public int FrameIndex   // property
        {
            get { return miFrameIndex; }   // get method
            set
            {                           // set method
                miFrameIndex = value;
                strFrameFile = strFramePath + miFrameIndex.ToString("0#") + ".png";
                //picFrame.SizeMode = PictureBoxSizeMode.AutoSize;
                //picFrame.Image = Image.FromFile(strFrameFile);

                
                //intWidth = picFrame.Width;
                //intHeight = picFrame.Height;
                miWidth = 227;
                miHeight = 304;

                //picFrame.Image = null;
                //var context = GRContext.Create(GRBackend.OpenGL);

                SKImageInfo imageInfo = new SKImageInfo(miWidth, miHeight);
                mskInfo = SKSurface.Create(imageInfo);
                //mskInfo = SKSurface.Create(context,false,imageInfo);

            }   
        }

        public MemberCardTemplVeryShort(int iTop = 0, int iLeft = 0)
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.DoubleBuffer |
              ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint,
              true);
            this.UpdateStyles();

            miTop = iTop;
            miLeft = iLeft; 
        }        

        public void MakeMember()
        {
            if (mskInfo == null) return;
            if (mskInfo.Handle.ToInt64() <= 0) return;            

            SKCanvas canvas = mskInfo.Canvas;
            
            //Draw Frame
            SKBitmap frameImage = SKBitmap.Decode(strFrameFile);
            SKRectI skRectCenter = new SKRectI(0, 0, 9, 9);
            SKRect skRectDst = new SKRect(0, 0,   miWidth, miHeight);
            canvas.DrawBitmapNinePatch(frameImage, skRectCenter, skRectDst);

            SKBitmap memberImage = SKBitmap.Decode(@".\Docs\noavatar.jpg");
            int mImageLeft = miWidth/2- memberImage.Width/2;
            int mImageTop = 15;
            int mImageRight = mImageLeft + memberImage.Width;
            int mImageBottom = mImageTop + memberImage.Height;

            skRectDst = new SKRect(mImageLeft, mImageTop, mImageRight, mImageBottom);
            canvas.DrawBitmapNinePatch(memberImage, skRectCenter, skRectDst);
                                    
            SKColor colorPredefined = SKColors.Red;
            using (SKPaint paint = new SKPaint())
            {
                paint.TextSize = 14.0f;                
                paint.Color = new SKColor(0xE6, 0xB8, 0x9C);
                //paint.TextScaleX = 1.5f;
                paint.IsAntialias = true;
                paint.StrokeWidth = 1;
                paint.Style = SKPaintStyle.Stroke;
                paint.TextAlign = SKTextAlign.Center;
                paint.Typeface = SKTypeface.FromFamilyName(
                "Arial",
                SKFontStyleWeight.Bold,
                SKFontStyleWidth.Normal,
                SKFontStyleSlant.Italic);

                canvas.DrawText(miFrameIndex.ToString(), miWidth / 2, 30, paint);
                canvas.DrawText(mstrFullName, miWidth/2, mImageBottom + 10, paint);
                canvas.DrawText(mstrFullName, miWidth / 2, mImageBottom + 50, paint);
                canvas.DrawText(mstrFullName, miWidth / 2, mImageBottom + 90, paint);
            }            
        }
    }
}
