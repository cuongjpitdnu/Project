using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace GP40Common
{
    public class clsFamilyMember
    {
        public string miID;

        public SKPoint miPosition;
        public SKSizeI miSize;
        public float miMaxTreeRight;
        public float miMinTreeLeft;

        private clsConst.ENUM_GENDER _gender;

        public clsConst.ENUM_GENDER Gender {
            get {

                return _gender;
            }
            set
            {
                _gender = value;

                if (_gender == clsConst.ENUM_GENDER.Male)
                {
                    Image = clsConst.AvartaPath + "male.png";
                }
                else if (_gender == clsConst.ENUM_GENDER.FMale)
                {
                    Image = clsConst.AvartaPath + "female.png";
                }
                else
                {
                    Image = clsConst.AvartaPath + "noavatar.jpg";
                }
            }
        }


        public string FirstName;
        public string MilddeName;
        public string LastName;
        public string FullName;
        public List<string> lstSpouse;
        public List<string> lstChild;
        public List<string> lstParent;
        public string Image;
        public string FrameImage;
        public string HomeTown;
        public string BYearS;
        public string BMonthS;
        public string BDayS;
        public string BYearM;
        public string BMonthM;
        public string BDayM;

        public string DYearS;
        public string DMonthS;
        public string DDayS;
        public string DYearM;
        public string DMonthM;
        public string DDayM;

        public string FLevel = "";
        public int intFLevel = 1;
        private clsConst.ENUM_MEMBER_TEMPLATE enmTemplate;
        SKSurface mskInfo = null;
        UserControl ucMemberInfo;
        public SkiaSharp.Extended.Svg.SKSvg miSvgData;
        public SkiaSharp.Extended.Svg.SKSvg miSvgAvatar;
        public bool blnPos = false;

        public float flZoom = 1;

        private SKColor _backColor = SKColors.GhostWhite;
        private SKColor _borderColor = SKColors.Blue;
        private SKColor _textColor = SKColors.Black;

        public bool ReDraw { get; set; } = false;

        public SKColor BackColor
        {
            get { return _backColor; }   // get method
            set
            {
                _backColor = value;
                //DrawingMemberSVG(true);
            }   // set method
        }

        public SKColor BorderColor
        {
            get { return _borderColor; }   // get method
            set
            {
                _borderColor = value;
                //DrawingMemberSVG(true);
            }   // set method
        }

        public SKColor TextColor
        {
            get { return _textColor; }   // get method
            set
            {
                _textColor = value;
                //DrawingMemberSVG(true);
            }   // set method
        }
        public SKSurface InfoSurface   // property
        {
            get { return mskInfo; }   // get method
        }

        public int Width   // property
        {
            get { return Convert.ToInt32(miSize.Width * flZoom); }   // get method
        }

        public int Height   // property
        {
            get { return Convert.ToInt32(miSize.Height * flZoom); }   // get method
        }

        public SKRect SelectedBorder   // property
        {
            get
            {
                SKRect skBorder = new SKRect(miPosition.X - 5, miPosition.Y - 5,
                   miPosition.X + this.Width + 5, miPosition.Y + this.Height + 5);
                return skBorder;
            }
        }

        public clsConst.ENUM_MEMBER_TEMPLATE TemplateType   // property
        {
            get { return enmTemplate; }   // get method
            set
            {
                enmTemplate = value;

                if (enmTemplate == clsConst.ENUM_MEMBER_TEMPLATE.MCTFull)
                {
                    ucMemberInfo = clsCommon.mcTemplFull;
                }
                else if (enmTemplate == clsConst.ENUM_MEMBER_TEMPLATE.MCTShort)
                {
                    ucMemberInfo = clsCommon.mcTemplShort;
                }
                else if (enmTemplate == clsConst.ENUM_MEMBER_TEMPLATE.MCTVeryShort)
                {
                    ucMemberInfo = clsCommon.mcTemplVeryShort;
                }
                else if (enmTemplate == clsConst.ENUM_MEMBER_TEMPLATE.MCTTall)
                {
                    ucMemberInfo = clsCommon.mcTemplTall;
                }
                else if (enmTemplate == clsConst.ENUM_MEMBER_TEMPLATE.MCTInput)
                {
                    ucMemberInfo = clsCommon.mcTemplInput;
                }

                miSize.Height = ucMemberInfo.Height;
                miSize.Width = ucMemberInfo.Width;

                //SKImageInfo imageInfo = new SKImageInfo(miSize.Width, miSize.Height);
                //mskInfo = SKSurface.Create(imageInfo);
                // allocate memory
                //draw faster
                //var memory = Marshal.AllocCoTaskMem(imageInfo.BytesSize);
                // construct a surface around the existing memory
                //mskInfo = SKSurface.Create(imageInfo, memory, imageInfo.RowBytes);
            }
        }

        public clsFamilyMember()
        {
            miPosition = new SKPoint(0, 0);
            lstChild = new List<string>();
            lstSpouse = new List<string>();
            lstParent = new List<string>();
            blnPos = false;
        }

        public clsFamilyMember(clsConst.ENUM_MEMBER_TEMPLATE enTemplate)
        {
            InitMemberInfo(enTemplate);
        }

        public void InitMemberInfo(clsConst.ENUM_MEMBER_TEMPLATE enTemplate, clsConst.ENUM_GENDER enGender = clsConst.ENUM_GENDER.Male)
        {
            this.FrameImage = clsConst.FramePath + "01.png";
            this.Gender = enGender;

            this.Image = clsConst.AvartaPath + "noavatar.jpg";
            //this.TemplateType = enTemplate;
            if (Gender == clsConst.ENUM_GENDER.Unknown)
                this.Image = clsConst.AvartaPath + "noavatar.jpg";
            else if (Gender == clsConst.ENUM_GENDER.FMale)
            {
                this.Image = clsConst.AvartaPath + "female.png";
                this.BackColor = SKColors.LightPink;
            }
            else if (Gender == clsConst.ENUM_GENDER.Male)
                this.Image = clsConst.AvartaPath + "male.png";

            this.TemplateType = enTemplate;
            this.FullName = "Phần Mềm QLGP " + miID;
            //this.FullName = "PM QLGP ";
            this.BDayS = "06";
            this.BMonthS = "10";
            this.BYearS = "1981";
        }

        public void AddChild(string intChildID)
        {
            lstChild.Add(intChildID);
        }

        public bool isNoChild()
        {
            return lstChild.Count == 0;
        }

        public bool CheckMemberClick(SKPoint skP)
        {
            SKRect rect = new SKRect(miPosition.X, miPosition.Y, miPosition.X + miSize.Width, miPosition.Y + miSize.Height);
            return rect.Contains(skP);
        }

        private void drawCanvasFrame(SKCanvas canvas)
        {
            canvas.Scale(flZoom);

            SKRect skBorder = new SKRect(0, 0, miSize.Width, miSize.Height);

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.StrokeWidth = 3;
                paint.Style = SKPaintStyle.StrokeAndFill;
                paint.Color = _backColor;
                canvas.DrawRoundRect(skBorder, new SKSize(1, 1), paint);

                paint.Color = _borderColor;
                paint.Style = SKPaintStyle.Stroke;
                canvas.DrawRoundRect(skBorder, new SKSize(1, 1), paint);

                if (!string.IsNullOrEmpty(FrameImage) && File.Exists(FrameImage))
                {
                    //Draw Frame
                    using (SKBitmap frameBitmap = SKBitmap.Decode(FrameImage).Resize(new SKSizeI(miSize.Width, miSize.Height), SKFilterQuality.Medium))
                    {
                        canvas.DrawBitmap(frameBitmap, 0, 0);
                    }
                }
            }
        }

        private void drawCanvasAvatar(SKCanvas canvas)
        {
            canvas.Scale(flZoom);
            var ctrl = ucMemberInfo.Controls.Find(clsConst.picImage, false).FirstOrDefault();

            if (ctrl != null && !string.IsNullOrEmpty(Image) && File.Exists(Image))
            {
                using (SKBitmap picImage = SKBitmap.Decode(Image).Resize(new SKSizeI(ctrl.Width, ctrl.Height), SKFilterQuality.Medium))
                {
                    canvas.DrawBitmap(picImage, ctrl.Left, ctrl.Top + 10);
                }
            }
        }

        private void drawCanvasTextInfo(SKCanvas canvas)
        {
            canvas.Scale(flZoom);

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.TextSize = 14.0f;
                paint.Color = _textColor;
                paint.Style = SKPaintStyle.StrokeAndFill;

                xDrawText(clsConst.lblFullName, canvas, FullName, paint);
                xDrawText(clsConst.lblBirthDate, canvas, xMakeDate(BYearS, BMonthS, BDayS), paint);
                xDrawText(clsConst.lblDeadDate, canvas, xMakeDate(DYearM, DMonthM, DDayM), paint);
                xDrawText(clsConst.lblLevel, canvas, this.FLevel, paint);
            }
        }

        public void xDraw2Canvas(SKCanvas canvas)
        {
            canvas.Scale(flZoom);

            //canvas.Clear(SKColors.Transparent);
            //canvas.Clear(_backColor);

            //SKRectI skRectCenter = new SKRectI(0, 0, 1, 1);
            SKRect skBorder = new SKRect(0, 0, miSize.Width, miSize.Height);
            SKRectI skRTl = new SKRectI(0, 0, miSize.Width, miSize.Height);
            SKRegion skRegion = new SKRegion(skRTl);

            SKPaint paint = new SKPaint();
            paint.TextSize = 14.0f;
            //paint.Color = new SKColor(0xE6, 0xB8, 0x9C);

            //paint.TextScaleX = 1.5f;
            paint.IsAntialias = true;
            paint.StrokeWidth = 3;
            paint.Style = SKPaintStyle.StrokeAndFill;
            paint.Color = _backColor;
            canvas.DrawRoundRect(skBorder, new SKSize(1, 1), paint);

            paint.Color = _borderColor;
            paint.Style = SKPaintStyle.Stroke;

            //paint.TextAlign = SKTextAlign.Center;
            //paint.Typeface = SKTypeface.FromFamilyName("Arial",
            //SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);

            //if (enmTemplate != clsConst.ENUM_MEMBER_TEMPLATE.MemberCardTemplFull)
            {
                //canvas.DrawRegion(skRegion, paint);
                canvas.DrawRoundRect(skBorder, new SKSize(1, 1), paint);
            }

            Control ctrl = ucMemberInfo.Controls.Find(clsConst.picFrame, false).FirstOrDefault();
            if (ctrl != null)
            {
                //Control picFrame = ctrls[0];
                //Draw Frame
                //SKBitmap frameBitmap = SKBitmap.Decode(FrameImage);
                //SKImage frameImage = SKImage.FromEncodedData(FrameImage);
                //SKRect skRectDst = new SKRect(0, 0, miWidth, miHeight);
                //canvas.DrawImage(frameImage, 0, 0, paint);
                //canvas.DrawBitmapNinePatch(frameImage, skRectCenter, skRectDst);

                //frameImage.Dispose();
            }

            ctrl = ucMemberInfo.Controls.Find(clsConst.picImage, false).FirstOrDefault();
            if (ctrl != null)
            {
                SKImage picImage = SKImage.FromEncodedData(Image);
                canvas.DrawImage(picImage, ctrl.Left, ctrl.Top, paint);

                skBorder = new SKRect(ctrl.Left, ctrl.Top, ctrl.Left + picImage.Width, ctrl.Top + picImage.Height);
                //canvas.DrawRoundRect(skBorder, new SKSize(1, 1), paint);
                picImage.Dispose();
            }

            paint.Color = _textColor;
            paint.Style = SKPaintStyle.StrokeAndFill;
            //xDrawTextCenter(clsConst.lblFullName, canvas, FullName, paint);
            //xDrawTextCenter(clsConst.lblBirthDate, canvas, xMakeDate(BYearS, BMonthS, BDayS), paint);
            //xDrawTextCenter(clsConst.lblDeadDate, canvas, xMakeDate(DYearM, DMonthM, DDayM), paint);
            //xDrawTextCenter(clsConst.lblLevel, canvas, this.FLevel, paint);

            xDrawText(clsConst.lblFullName, canvas, FullName, paint);
            xDrawText(clsConst.lblBirthDate, canvas, xMakeDate(BYearS, BMonthS, BDayS), paint);
            xDrawText(clsConst.lblDeadDate, canvas, xMakeDate(DYearM, DMonthM, DDayM), paint);
            xDrawText(clsConst.lblLevel, canvas, this.FLevel, paint);

            paint.Dispose();
        }

        public void DrawingInputMember()
        {
            if (mskInfo == null)
            {
                SKImageInfo imageInfo = new SKImageInfo(miSize.Width, miSize.Height);
                mskInfo = SKSurface.Create(imageInfo);
            }

            SKCanvas canvas = mskInfo.Canvas;

            xDraw2Canvas(canvas);
            canvas.ResetMatrix();
        }

        public void DrawingInputSVGMember()
        {
            if (mskInfo == null)
            {
                SKImageInfo imageInfo = new SKImageInfo(miSize.Width, miSize.Height);
                mskInfo = SKSurface.Create(imageInfo);
            }

            Byte[] dataContentSvg = null;

            using (var memoryStreamWrite = new MemoryStream())
            {
                using (SKCanvas canvas = SKSvgCanvas.Create(SKRect.Create(miSize.Width, miSize.Height), memoryStreamWrite))
                {
                    // draw on the canvas ...
                    xDraw2Canvas(canvas);
                }

                dataContentSvg = memoryStreamWrite.ToArray();
            }

            using (var memoryStreamReader = new MemoryStream(dataContentSvg))
            {
                miSvgData = new SkiaSharp.Extended.Svg.SKSvg();
                miSvgData.Load(memoryStreamReader);
            }

            mskInfo.Dispose();
            mskInfo = null;
        }

        /// <summary>
        /// Load SVG member from file, which file is encrypted or not.
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="blnFileEncryped"></param>
        /// <returns></returns>
        private bool LoadSVGMember(string strFilePath, bool blnFileEncryped)
        {
            if (File.Exists(strFilePath))
            {
                string strContentSvg = string.Empty;

                if (blnFileEncryped)
                {
                    strContentSvg = clsCipher.DecryptString(strFilePath);
                }
                else
                {
                    strContentSvg = File.ReadAllText(strFilePath);
                }

                using (var stream = clsCipher.GenerateStreamFromString(strContentSvg))
                {
                    miSvgData = new SkiaSharp.Extended.Svg.SKSvg();
                    miSvgData.Load(stream);
                }

                //if (!blnFileEncryped)
                //{
                //    clsCipher.EncryptString(strFilePath, strContentSvg);
                //}
                return true;
            }
            return false;
        }

        private MemoryStream _memoryStreamWrite = new MemoryStream();

        public void DrawingMemberSVGFrame(bool blnOverwrite = false)
        {
            var strFileName = clsCommon.GetMemberCardDataPath(enmTemplate) + miID + "_frame.svg";

            if (blnOverwrite || !File.Exists(strFileName))
            {
                using (var memory = new MemoryStream())
                {
                    using (SKCanvas canvas = SKSvgCanvas.Create(SKRect.Create(miSize.Width, miSize.Height), memory))
                    {
                        // draw on the canvas ...
                        drawCanvasFrame(canvas);
                    }

                    var dataContentSvg = memory.ToArray();
                    File.WriteAllText(strFileName, Encoding.UTF8.GetString(dataContentSvg));
                }
            }

            LoadSVGMember(strFileName, false);
        }

        public void DrawingMemberSVGAvatarGender(bool blnOverwrite = false)
        {

            var strFileName = clsCommon.GetMemberCardDataPath(enmTemplate) + miID + "_avatar.svg";

            if (!blnOverwrite)
            {
                if (LoadSVGMember(strFileName, true))
                {
                    return;
                }
            }

            _memoryStreamWrite.SetLength(0);

            using (SKCanvas canvas = SKSvgCanvas.Create(SKRect.Create(miSize.Width, miSize.Height), _memoryStreamWrite))
            {
                // draw on the canvas ...
                drawCanvasAvatar(canvas);
            }

            var dataContentSvg = _memoryStreamWrite.ToArray();
            miSvgAvatar = new SkiaSharp.Extended.Svg.SKSvg();
            miSvgAvatar.Load(new MemoryStream(dataContentSvg));

            clsCipher.EncryptString(strFileName, Encoding.UTF8.GetString(dataContentSvg));
        }

        public void DrawingMemberSVG(bool blnOverwrite = false)
        {

            var strFileName = clsCommon.GetMemberCardDataPath(enmTemplate) + miID + ".svg";

            if (!blnOverwrite)
            {
                if (LoadSVGMember(strFileName, true))
                {
                    return;
                }
            }

            _memoryStreamWrite.SetLength(0);

            using (SKCanvas canvas = SKSvgCanvas.Create(SKRect.Create(miSize.Width, miSize.Height), _memoryStreamWrite))
            {
                // draw on the canvas ...
                drawCanvasAvatar(canvas);
                drawCanvasTextInfo(canvas);
            }

            var dataContentSvg = _memoryStreamWrite.ToArray();
            miSvgData = new SkiaSharp.Extended.Svg.SKSvg();
            miSvgData.Load(new MemoryStream(dataContentSvg));

            clsCipher.EncryptString(strFileName, Encoding.UTF8.GetString(dataContentSvg));
        }

        public void FreeSVGData()
        {
            if (miSvgData != null)
            {
                miSvgData = null;
            }

            if (miSvgAvatar != null)
            {
                miSvgAvatar = null;
            }

            if (_memoryStreamWrite != null)
            {
                _memoryStreamWrite.SetLength(0);
            }

            this.ReDraw = false;
        }

        private string xMakeDate(string strYear, string strMonth, string strDay)
        {
            return strDay + "-" + strMonth + "-" + strYear;
        }

        private void xDrawTextCenter(string strControlName, SKCanvas canvas, string strValue, SKPaint paint)
        {
            Control ctrl = ucMemberInfo.Controls.Find(strControlName, false).FirstOrDefault();
            if (ctrl == null) return;

            paint.TextSize = ctrl.Font.Size;
            paint.Typeface = SKTypeface.FromFamilyName(ctrl.Font.Name, SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
            canvas.DrawText(strValue, miSize.Width / 2, ctrl.Top, paint);
        }

        private void xDrawText(string strControlName, SKCanvas canvas, string strValue, SKPaint paint)
        {
            Control ctrl = ucMemberInfo.Controls.Find(strControlName, false).FirstOrDefault();
            if (ctrl == null) return;

            Font controlFont = ctrl.Font;
            paint.TextSize = ctrl.Font.Size;

            if (controlFont.Bold)
            {
                paint.Typeface = SKTypeface.FromFamilyName(ctrl.Font.Name,
                SKFontStyleWeight.Bold,
                SKFontStyleWidth.Normal,
                SKFontStyleSlant.Upright);

                if (controlFont.Italic)
                {
                    paint.Typeface = SKTypeface.FromFamilyName(ctrl.Font.Name,
                    SKFontStyleWeight.Bold,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Italic);
                }
            }
            else
            {
                paint.Typeface = SKTypeface.FromFamilyName(ctrl.Font.Name,
                    SKFontStyleWeight.Normal,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Upright);

                if (controlFont.Italic)
                    paint.Typeface = SKTypeface.FromFamilyName(ctrl.Font.Name,
                    SKFontStyleWeight.Normal,
                    SKFontStyleWidth.Normal,
                    SKFontStyleSlant.Italic);
            }

            if (ctrl is Label)
            {

                if (((Label)ctrl).TextAlign == ContentAlignment.MiddleCenter)
                {
                    //if (ctrl.Top < miPosition.Y) ctrl.Top = Convert.ToInt32(miPosition.Y + 20);
                    paint.TextAlign = SKTextAlign.Center;

                    canvas.DrawText(strValue, ctrl.Left + ctrl.Width / 2, ctrl.Bottom, paint);

                }
                else
                {
                    paint.TextAlign = SKTextAlign.Left;
                    SKPoint ctrlPoint = new SKPoint(ctrl.Location.X + ctrl.Padding.Left + ctrl.Margin.Left + paint.TextSize,
                                                    ctrl.Location.Y + ctrl.Padding.Top + ctrl.Margin.Top + paint.TextSize);

                    canvas.DrawText(strValue, ctrlPoint, paint);
                    //canvas.DrawText(strValue, new SKPoint(ctrlPoint.X, ctrlPoint.Y), paint);
                }
            }

        }

        /// <summary>
        /// Add by: 2020.06.01 AKB Nguyen Thanh Tung
        /// </summary>
        public void ResetDataDraw()
        {
            miSize = new SKSizeI();
            miPosition = new SKPoint();
            miMaxTreeRight = 0f;
            miMaxTreeRight = 0f;
            flZoom = 1f;
            blnPos = false;
            FreeSVGData();
        }
    }
}
