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

namespace GP40Common
{
    public class clsFamilyMember
    {
        public int miID;       
       
        public SKPoint miPosition;
        private SKSizeI miSize;
        public float miMaxTreeRight;
        public float miMinTreeLeft;

        public clsConst.ENUM_GENDER Gender;
        public string FirstName;
        public string MilddeName;
        public string LastName;
        public string FullName;
        public List<int> lstSpouse;
        public List<int> lstChild;
        public List<int> lstParent;
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
        public bool blnPos = false;

        public float flZoom = 1;

        private SKColor _backColor = SKColors.GhostWhite;
        private SKColor _borderColor = SKColors.Blue;
        private SKColor _textColor = SKColors.Black;

        public SKColor BackColor
        {
            get { return _backColor; }   // get method
            set { 
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
               // DrawingMemberSVG(true);
            }   // set method
        }

        public SKColor TextColor
        {
            get { return _borderColor; }   // get method
            set
            {
                _textColor = value;
               // DrawingMemberSVG(true);
            }   // set method
        }
        public SKSurface InfoSurface   // property
        {
            get { return mskInfo; }   // get method
        }

        public int Width   // property
        {
            get { return Convert.ToInt32(miSize.Width*flZoom); }   // get method
        }

        public int Height   // property
        {
            get { return Convert.ToInt32(miSize.Height * flZoom); }   // get method
        }

        public SKRect SelectedBorder   // property
        {
            get {
                SKRect skBorder = new SKRect(miPosition.X - 5, miPosition.Y -5,
                   miPosition.X + this.Width + 5, miPosition.Y + this.Height + 5);
                return skBorder;
            }            
        }        

        public clsConst.ENUM_MEMBER_TEMPLATE TemplateType   // property
        {
            get { return enmTemplate; }   // get method 
            set {
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
            lstChild = new List<int>();
            lstSpouse = new List<int>();
            lstParent = new List<int>();
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
            this.FullName = "Phần Mềm QLGP " + miID.ToString();
            this.FullName = "PM QLGP ";
            this.BDayS = "06";
            this.BMonthS = "10";
            this.BYearS = "1981";
        }
                
        public void AddChild(int intChildID)
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

        public void xDraw2Canvas(SKCanvas canvas)
        {
            canvas.Clear(_backColor);

            canvas.Scale(flZoom);
            //SKRectI skRectCenter = new SKRectI(0, 0, 1, 1);
            SKRect skBorder = new SKRect(0, 0, miSize.Width, miSize.Height);
            SKRectI skRTl = new SKRectI(0, 0, miSize.Width, miSize.Height);
            SKRegion skRegion = new SKRegion(skRTl);

            SKPaint paint = new SKPaint();
            paint.TextSize = 14.0f;
            //paint.Color = new SKColor(0xE6, 0xB8, 0x9C);
            paint.Color = _borderColor;
            //paint.TextScaleX = 1.5f;
            paint.IsAntialias = true;
            paint.StrokeWidth = 3;
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

                //skBorder = new SKRect(ctrl.Left, ctrl.Top, ctrl.Left + picImage.Width, ctrl.Top + picImage.Height);
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

                if (!blnFileEncryped)
                {
                    clsCipher.EncryptString(strFilePath, strContentSvg);
                }
                return true;
            }
            return false;
        }

        public void DrawingMemberSVG(bool blnOverwrite = false)
        {
            SKImageInfo imageInfo = new SKImageInfo(miSize.Width, miSize.Height);
            mskInfo = SKSurface.Create(imageInfo);
            
            string strFileName = clsCommon.GetMemberCardDataPath(enmTemplate) + miID.ToString() + ".svg";

            if (!blnOverwrite)
            {
                if (LoadSVGMember(strFileName, true))
                {
                    mskInfo.Dispose();
                    mskInfo = null;
                    return;
                }
            }          

            FileStream fs = File.Create(strFileName);
            
            SKCanvas canvas = SKSvgCanvas.Create(SKRect.Create(miSize.Width, miSize.Height), fs);
            // draw on the canvas ...
            xDraw2Canvas(canvas);

            canvas.Dispose();

            fs.Flush();
            fs.Close();

            LoadSVGMember(strFileName, false);
            mskInfo.Dispose();
            mskInfo = null;
        }

        public void FreeSVGData()
        {
            if (miSvgData != null)
            {
                miSvgData = null;                
            }
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
            }else
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
                
                if (((Label) ctrl).TextAlign == ContentAlignment.MiddleCenter)
                {
                    //if (ctrl.Top < miPosition.Y) ctrl.Top = Convert.ToInt32(miPosition.Y + 20);
                    paint.TextAlign = SKTextAlign.Center;
                    
                    canvas.DrawText(strValue, ctrl.Left + ctrl.Width/2, ctrl.Bottom + ctrl.Margin.Top, paint);
                    
                }
                else
                {
                    paint.TextAlign = SKTextAlign.Left;
                    canvas.DrawText(strValue, ctrl.Left + ctrl.Margin.Left, ctrl.Bottom + ctrl.Margin.Top, paint);
                }
            }

        }
    }
}
