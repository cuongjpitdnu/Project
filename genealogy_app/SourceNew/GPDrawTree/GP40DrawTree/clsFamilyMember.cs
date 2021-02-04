using GP40DrawTree;
using GPCommon;
using GPMain.Common;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GP40Common
{
    public class clsFamilyMember : DrawCardBase
    {
        //public string Id;

        //public SKPoint Position;
        //public SKSizeI Size;

        public float miMaxTreeRight;
        public float miMinTreeLeft;
        public float minLeftInFamily = 0;
        public float maxRightInFamily = 0;

        private clsConst.ENUM_GENDER _gender;

        public clsConst.ENUM_GENDER Gender
        {
            get
            {
                return _gender;
            }
            set
            {
                _gender = value;
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
        public string LevelInFamily;

        public string strBackColor;
        public string strForeColor;
        public bool UseDefaultColor = false;
        public bool ShowStepChild = false;

        public bool showBirthDayDefault = true;
        public bool showDeathDayLunarCalander = true;
        public bool showGender = false;
        public bool showFamilyLevel = true;
        public string TypeTextShow { get; set; } = TextShow.Normal.ToString();
        public bool InRootTree { get; set; } = false;

        public string FLevel = "";
        public int intFLevel = 1;
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
            get { return Convert.ToInt32(Size.Width * flZoom); }   // get method
        }

        public int Height   // property
        {
            get { return Convert.ToInt32(Size.Height * flZoom); }   // get method
        }

        public SKRect SelectedBorder   // property
        {
            get
            {
                SKRect skBorder = new SKRect(Position.X - 5, Position.Y - 5,
                   Position.X + this.Width + 5, Position.Y + this.Height + 5);
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
                    switch (Enum.Parse(typeof(TextShow), TypeTextShow))
                    {
                        case TextShow.TurnRight: ucMemberInfo = clsCommon.mcTemplVeryShortTurnRight; break;
                        case TextShow.TurnLeft: ucMemberInfo = clsCommon.mcTemplVeryShortTurnLeft; break;
                        default: ucMemberInfo = clsCommon.mcTemplVeryShort; break;
                    }
                }
                else if (enmTemplate == clsConst.ENUM_MEMBER_TEMPLATE.MCTTall)
                {
                    ucMemberInfo = clsCommon.mcTemplTall;
                }
                else if (enmTemplate == clsConst.ENUM_MEMBER_TEMPLATE.MCTInput)
                {
                    ucMemberInfo = clsCommon.mcTemplInput;
                }

                Size.Height = ucMemberInfo.Height;
                Size.Width = ucMemberInfo.Width;

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
            Position = new SKPoint(0, 0);
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
            this.FullName = "Phần Mềm QLGP " + Id;
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
            SKRect rect = new SKRect(Position.X, Position.Y, Position.X + Size.Width, Position.Y + Size.Height);
            return rect.Contains(skP);
        }

        protected override void DrawFrame(SKCanvas canvas)
        {
            canvas.Scale(flZoom);

            SKRect skBorder = new SKRect(0, 0, Size.Width, Size.Height);

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
                    using (SKBitmap frameBitmap = SKBitmap.Decode(FrameImage).Resize(new SKSizeI(Size.Width, Size.Height), SKFilterQuality.Medium))
                    {
                        canvas.DrawBitmap(frameBitmap, 0, 0);
                    }
                }
            }
        }

        protected override void DrawImage(SKCanvas canvas)
        {
            var ctrl = ucMemberInfo.Controls.Find(clsConst.picImage, false).FirstOrDefault();

            if (ctrl == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(Image) || !File.Exists(Image))
            {
                Image = clsConst.AvartaPath + "noavatar.jpg";

                if (_gender == clsConst.ENUM_GENDER.Male)
                {
                    Image = clsConst.AvartaPath + "male.png";
                }

                if (_gender == clsConst.ENUM_GENDER.FMale)
                {
                    Image = clsConst.AvartaPath + "female.png";
                }
            }

            if (!string.IsNullOrEmpty(Image) && File.Exists(Image))
            {
                using (SKBitmap picImage = SKBitmap.Decode(Image).Resize(new SKSizeI(ctrl.Width, ctrl.Height), SKFilterQuality.Medium))
                {
                    canvas.Scale(flZoom);
                    canvas.DrawBitmap(picImage, ctrl.Left, ctrl.Top + 10);
                }
            }
        }

        protected override void DrawText(SKCanvas canvas)
        {
            canvas.Scale(flZoom);

            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.TextSize = 14.0f;
                paint.Color = _textColor;
                paint.Style = SKPaintStyle.StrokeAndFill;
                if (TemplateType == clsConst.ENUM_MEMBER_TEMPLATE.MCTVeryShort)
                {
                    switch (Enum.Parse(typeof(TextShow), TypeTextShow))
                    {
                        case TextShow.TurnRight: canvas.RotateDegrees(90); break;
                        case TextShow.TurnLeft: canvas.RotateDegrees(-90); break;
                    }
                }

                xDrawText(clsConst.lblFullName, canvas, $"{(ShowStepChild ? AppConst.StepChild : FullName)}{(showGender ? (Gender == clsConst.ENUM_GENDER.Male ? "(Nam)" : (Gender == clsConst.ENUM_GENDER.FMale ? "(Nữ)" : "(Không rõ)")) : "")}", paint);
                xDrawText(clsConst.lblBirthDate, canvas, xMakeDate(BYearS, BMonthS, BDayS, showBirthDayDefault), paint);
                xDrawText(clsConst.lblDeadDate, canvas, showDeathDayLunarCalander ? xMakeDate(DYearM, DMonthM, DDayM) : xMakeDate(DYearS, DMonthS, DDayS), paint);
                xDrawText(clsConst.lblLevel, canvas, showFamilyLevel ? $"Đời thứ {this.LevelInFamily}" : "", paint);

                if (TemplateType == clsConst.ENUM_MEMBER_TEMPLATE.MCTVeryShort)
                {
                    switch (Enum.Parse(typeof(TextShow), TypeTextShow))
                    {
                        case TextShow.TurnRight: canvas.RotateDegrees(-90); break;
                        case TextShow.TurnLeft: canvas.RotateDegrees(90); break;
                    }
                }
            }
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
                var strContentSvg = blnFileEncryped
                                    ? EncryptHelper.ReadFileDecrypt(strFilePath, clsConst.PASS_SVG)
                                    : File.ReadAllText(strFilePath);

                using (var stream = ConvertHelper.CnvStringToStream(strContentSvg))
                {
                    miSvgData = new SkiaSharp.Extended.Svg.SKSvg();
                    miSvgData.Load(stream);
                }

                return true;
            }

            return false;
        }

        private MemoryStream _memoryStreamWrite = new MemoryStream();

        public void DrawingMemberSVGFrame(bool blnOverwrite = false)
        {
            var strFileName = clsCommon.GetMemberCardDataPath(enmTemplate) + Id + "_frame.svg";

            if (blnOverwrite || !File.Exists(strFileName))
            {
                using (var memory = new MemoryStream())
                {
                    using (SKCanvas canvas = SKSvgCanvas.Create(SKRect.Create(Size.Width, Size.Height), memory))
                    {
                        // draw on the canvas ...
                        DrawFrame(canvas);
                    }

                    var dataContentSvg = memory.ToArray();
                    File.WriteAllText(strFileName, Encoding.UTF8.GetString(dataContentSvg));
                }
            }

            LoadSVGMember(strFileName, false);
        }

        public void DrawingMemberSVGAvatarGender(bool blnOverwrite = false)
        {
            var strFileName = clsCommon.GetMemberCardDataPath(enmTemplate) + Id + "_avatar.svg";

            if (!blnOverwrite)
            {
                if (LoadSVGMember(strFileName, false))
                {
                    return;
                }
            }

            _memoryStreamWrite.SetLength(0);

            using (SKCanvas canvas = SKSvgCanvas.Create(SKRect.Create(Size.Width, Size.Height), _memoryStreamWrite))
            {
                // draw on the canvas ...
                DrawImage(canvas);
            }

            var dataContentSvg = _memoryStreamWrite.ToArray();
            miSvgAvatar = new SkiaSharp.Extended.Svg.SKSvg();
            miSvgAvatar.Load(new MemoryStream(dataContentSvg));
        }

        public void DrawingMemberSVG(bool blnOverwrite = false)
        {
            var strFileName = clsCommon.GetMemberCardDataPath(enmTemplate) + Id + ".svg";

            if (!blnOverwrite)
            {
                if (LoadSVGMember(strFileName, true))
                {
                    return;
                }
            }

            _memoryStreamWrite.SetLength(0);

            using (SKCanvas canvas = SKSvgCanvas.Create(SKRect.Create(Size.Width, Size.Height), _memoryStreamWrite))
            {
                // draw on the canvas ...
                DrawText(canvas);
                DrawImage(canvas);
            }

            var dataContentSvg = _memoryStreamWrite.ToArray();
            miSvgData = new SkiaSharp.Extended.Svg.SKSvg();
            miSvgData.Load(new MemoryStream(dataContentSvg));

            Encoding.UTF8.GetString(dataContentSvg).ToFileEncrypt(strFileName, clsConst.PASS_SVG);
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

        private string xMakeDate(string strYear, string strMonth, string strDay, bool showDefault = true)
        {
            if (showDefault)
            {
                return strDay + "-" + strMonth + "-" + strYear;
            }
            else
            {
                if (int.TryParse(strDay, out int d) || int.TryParse(strMonth, out int m) || int.TryParse(strYear, out int y))
                {
                    return strDay + "-" + strMonth + "-" + strYear;
                }
                else
                {
                    return "Không rõ";
                }
            }
        }

        private void xDrawTextCenter(string strControlName, SKCanvas canvas, string strValue, SKPaint paint)
        {
            Control ctrl = ucMemberInfo.Controls.Find(strControlName, false).FirstOrDefault();
            if (ctrl == null) return;

            paint.TextSize = ctrl.Font.Size;
            paint.Typeface = SKTypeface.FromFamilyName(ctrl.Font.Name, SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
            canvas.DrawText(strValue, Size.Width / 2, ctrl.Top, paint);
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

                    if (TypeTextShow == TextShow.TurnRight.ToString() && TemplateType == clsConst.ENUM_MEMBER_TEMPLATE.MCTVeryShort)
                    {
                        canvas.DrawText(strValue, ctrl.Width / 2, -ctrl.Left, paint);
                    }
                    else if (TypeTextShow == TextShow.TurnLeft.ToString() && TemplateType == clsConst.ENUM_MEMBER_TEMPLATE.MCTVeryShort)
                    {
                        canvas.DrawText(strValue, -ctrl.Width / 2, ctrl.Left + 20, paint);
                    }
                    else if (TypeTextShow == TextShow.Normal.ToString() || TemplateType != clsConst.ENUM_MEMBER_TEMPLATE.MCTVeryShort)
                    {
                        canvas.DrawText(string.IsNullOrEmpty(strValue) ? "" : strValue, ctrl.Left + ctrl.Width / 2, ctrl.Bottom, paint);
                    }
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
            Size = new SKSizeI();
            Position = new SKPoint();
            miMaxTreeRight = 0f;
            miMaxTreeRight = 0f;
            flZoom = 1f;
            blnPos = false;
            FreeSVGData();
        }
    }
}