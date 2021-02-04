using GP40Common;
using GPCommon;
using GPMain;
using GPMain.Common;
using GPMain.Common.Helper;
using GPMain.Core;
using GPMain.Properties;
using GPMain.Views;
using GPMain.Views.Tree.Build;
using GPModels;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GP40Common.clsConst;

namespace GP40DrawTree
{
    public class ColorDrawHelper
    {
        public static SKColor FromColor(Color color)
        {
            return new SKColor(color.R, color.G, color.B);
        }

        public static SKColor FromColor(int R, int G, int B)
        {
            return FromColor(Color.FromArgb(R, G, B));
        }

        public static SKColor FromHtml(string hex)
        {
            return FromColor(ColorTranslator.FromHtml(hex));
        }

        public static Color FromHtmlToColor(string hex)
        {
            return ColorTranslator.FromHtml(hex);
        }
    }

    public class DrawTreeConfig
    {
        public string PathRoot { get; set; }
        public Hashtable DataMember { get; set; }
        public clsConst.ENUM_MEMBER_TEMPLATE TemplateMember { get; set; } = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;
        public SKColor BackgroudColor { get; set; } = new SKColor(250, 191, 98);
        public SKColor SelectedMemberColor { get; set; } = SKColors.RoyalBlue;
        public SKColor ChildLineColor { get; set; } = SKColors.Blue;
        public SKColor SpouseLineColor { get; set; } = SKColors.Red;
        public SKColor TextColor { get; set; } = SKColors.Black;
        public SKColor BorderColor { get; set; } = SKColors.Blue;
        public SKColor MaleBackColor { get; set; } = SKColors.GhostWhite;
        public SKColor FeMaleBackColor { get; set; } = SKColors.LightPink;
        public SKColor UnknowBackColor { get; set; } = SKColors.Magenta;
        public int MemberVerticalSpace { get; set; } = 30;
        public int MemberHorizonSpace { get; set; } = 30;
        public int NumberFrame { get; set; } = 1;
        public bool ShowImage { get; set; } = true;
        public bool ShowBirthDayDefaul { get; set; } = true;
        public bool ShowDeathDayLunarCalendar { get; set; } = true;
        public bool ShowFamilyLevel { get; set; } = true;
        public bool ShowGender { get; set; } = false;
        public string TypeTextShow { get; set; } = TextShow.Normal.ToString();
        public int MaxLevelInFamily { get; set; } = 20;
    }

    public enum TextShow
    {
        Normal, TurnRight, TurnLeft
    }

    public interface ISKControl
    {
        event EventHandler<SKCanvas> PaintSurfaceCanvas;
    }

    public class SKCustom : SKControl, ISKControl
    {
        public event EventHandler<SKCanvas> PaintSurfaceCanvas;

        public SKCustom() : base()
        {
            PaintSurface += (sender, e) => PaintSurfaceCanvas?.Invoke(sender, e.Surface.Canvas);
        }
    }

    public class SKGLCustom : SKGLControl, ISKControl
    {
        public event EventHandler<SKCanvas> PaintSurfaceCanvas;

        public SKGLCustom() : base()
        {
            this.VSync = true;
            PaintSurface += (sender, e) => PaintSurfaceCanvas?.Invoke(sender, e.Surface.Canvas);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.MakeCurrent();
            base.OnPaint(e);
        }
    }

    public class DrawTreeManager : DrawCommon
    {
        protected float minTop = 10;
        protected float minLeft = 10;
        protected bool blnMouseDown = false;
        protected Point pointMouseDown = new Point(0, 0);
        public clsFamilyMember SelectedMember   // property
        {
            get { return _memberSelected; }   // get method
            set
            {
                // set method
                _memberSelected = value;
                Invalidate();
            }
        }

        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
        public bool DrawToPrint { get; set; } = false;
        public bool DrawFirst { get; set; } = true;
        public int OffsetYCanvas { get; set; } = 0;

        public DrawTreeConfig Config { get; set; }
        protected clsFamilyMember RootInfo { get; set; }

        public event EventHandler<MouseEventArgs> MemberDoubleClick;
        public event EventHandler<clsFamilyMember> MemberRightClick;
        public event EventHandler MouseLeftClickCanvas;
        //public event EventHandler<int> DrawMember;

        protected clsFamilyMember GetDataMember(string intMemberId)
        {
            if (string.IsNullOrEmpty(intMemberId) || !Config.DataMember.ContainsKey(intMemberId))
            {
                return null;
            }
            var memberData = Config.DataMember[intMemberId] as clsFamilyMember;
            if (memberData != null)
            {
                memberData.TemplateType = Config.TemplateMember;
            }
            return memberData;
        }

        #region Public Func Control

        public void Draw(string rootId, clsConst.ENUM_MEMBER_TEMPLATE emTemplate)
        {
            this.RootInfo = GetDataMember(rootId);
            if (this.RootInfo != null)
            {
                Config.TemplateMember = emTemplate;

                Action<clsFamilyMember> updateMember = (objMember) =>
                {
                    if (objMember != null)
                    {
                        objMember.ResetDataDraw();
                        objMember.TypeTextShow = Config.TypeTextShow;
                        objMember.TemplateType = Config.TemplateMember;
                        objMember.BorderColor = Config.BorderColor;
                        objMember.BackColor = (string.IsNullOrEmpty(objMember.strBackColor) || objMember.UseDefaultColor) ?
                                              (objMember.Gender == clsConst.ENUM_GENDER.FMale ? Config.FeMaleBackColor :
                                               objMember.Gender == clsConst.ENUM_GENDER.Male ? Config.MaleBackColor : Config.UnknowBackColor) :
                                               ColorDrawHelper.FromHtml(objMember.strBackColor);
                        objMember.TextColor = (string.IsNullOrEmpty(objMember.strForeColor) || objMember.UseDefaultColor) ?
                                               Config.TextColor : ColorDrawHelper.FromHtml(objMember.strForeColor);
                        objMember.FrameImage = clsConst.FramePath + Config.NumberFrame.ToString().PadLeft(2, '0') + ".png";
                        objMember.showBirthDayDefault = Config.ShowBirthDayDefaul;
                        objMember.showDeathDayLunarCalander = Config.ShowDeathDayLunarCalendar;
                        objMember.showGender = Config.ShowGender;
                        objMember.showFamilyLevel = Config.ShowFamilyLevel;
                        objMember.ReDraw = true;
                        if (!string.IsNullOrEmpty(objMember.strBackColor))
                        {
                            objMember.DrawingMemberSVGFrame(true);
                        }
                    }
                };

                updateMember(_memberMale);
                updateMember(_memberFMale);
                updateMember(_memberUnknown);

                _memberMale.DrawingMemberSVGFrame(true);
                _memberFMale.DrawingMemberSVGFrame(true);
                _memberUnknown.DrawingMemberSVGFrame(true);

                _memberMale.DrawingMemberSVGAvatarGender(true);
                _memberFMale.DrawingMemberSVGAvatarGender(true);
                _memberUnknown.DrawingMemberSVGAvatarGender(true);

                // Reset Member Data
                Config.DataMember.Keys.Cast<string>().Select((keyMember) =>
                {
                    var objMember = (clsFamilyMember)Config.DataMember[keyMember];
                    updateMember(objMember);
                    return keyMember;
                }).ToList();

                BeginInvalidate();

                this.MakeTreeDrawWithSpouse(rootId, minTop, minLeft, clsConst.ENUM_FIRSTSPOUSE_POSITION.LeftMember);

                this.flZoomLevel = 1f;
                this.SelectedMember = null;
                this.ZoomLevelFit();
                this.CenteringRoot();

                EndInvalidate();
            }
        }

        float flZoomTemp;
        public bool ExportPdf(string pathSave, ProgressBarManager progressBar, InfoOptionExportPDF optionExportPDF)
        {
            int startOffsetX = 10;
            int startOffsetY = 20;
            int offsetUserCreateAndFamilyInfo = 50;
            try
            {
                GetBound();
                var dicInfo = CaculatorText(optionExportPDF, startOffsetY, offsetUserCreateAndFamilyInfo);
                float w = (float)dicInfo.GetValue("width");
                float h = (float)dicInfo.GetValue("height");

                using (var document = SKDocument.CreatePdf(pathSave))
                {
                    using (var canvas = document.BeginPage(w, h))
                    {
                        DrawTreeToCanvas(canvas, dicInfo, optionExportPDF, progressBar, startOffsetX, startOffsetY, w, h);
                    }
                    document.EndPage();
                  
                }
                progressBar.Percent = progressBar.fncCalculatePercent(++progressBar.count, progressBar.total);
                return true;
            }
            catch (Exception ex)
            {
                AppManager.LoggerApp.Error(typeof(DrawTreeManager), ex);
                return false;
            }
        }


        public bool ExportSVG(string pathSave, ProgressBarManager progressBar, InfoOptionExportPDF optionExportPDF)
        {
            int startOffsetX = 10;
            int startOffsetY = 20;
            int offsetUserCreateAndFamilyInfo = 50;
            try
            {
                GetBound();
                var dicInfo = CaculatorText(optionExportPDF, startOffsetY, offsetUserCreateAndFamilyInfo);
                float w = (float)dicInfo.GetValue("width");
                float h = (float)dicInfo.GetValue("height");
                using (var memory = new MemoryStream())
                {
                    using (var canvas = SKSvgCanvas.Create(SKRect.Create(w, h), memory))
                    {
                        DrawTreeToCanvas(canvas, dicInfo, optionExportPDF, progressBar, startOffsetX, startOffsetY, w, h);
                    }
                    var dataContentSvg = memory.ToArray();
                    File.WriteAllText(pathSave, Encoding.UTF8.GetString(dataContentSvg));
                }
                progressBar.Percent = progressBar.fncCalculatePercent(++progressBar.count, progressBar.total);
                return true;
            }
            catch (Exception ex)
            {
                AppManager.LoggerApp.Error(typeof(DrawTreeManager), ex);
                return false;
            }
        }

        private Dictionary<string, object> CaculatorText(InfoOptionExportPDF optionExportPDF, int startOffsetY, int offsetUserCreateAndFamilyInfo)
        {
            Dictionary<string, object> dicResult = new Dictionary<string, object>();
            int offsetLine = 10;
            float maxWidth = 10;

            #region Tính kích thước thông tin xuất file PDF
            //Thông tin dòng họ
            var familyInfo = AppManager.DBManager.GetTable<MFamilyInfo>().FirstOrDefault(x => x.Id == AppManager.LoginUser.FamilyId);

            //Tính kích thước thông tin ngày tạo
            SizeF dateCreateSize = new SizeF(0, 0);
            if (optionExportPDF.DateCreateSelect)
            {
                dateCreateSize = MeasureString(optionExportPDF.DateCreate, AppConst.FontUserCreateForExportPDF);
            }
            dicResult.Add("dateCreateSize", dateCreateSize);
            //Tính kích thước thông tin người lập
            SizeF userCreateSize = new SizeF(0, 0);
            string sUserCreate = "";
            if (optionExportPDF.UserCreateSelect)
            {
                sUserCreate = $"NGƯỜI LẬP: {optionExportPDF.UserCreate}";
                userCreateSize = MeasureString(sUserCreate, AppConst.FontUserCreateForExportPDF);
                userCreateSize = new SizeF(userCreateSize.Width * 2 / 3, userCreateSize.Height + offsetLine);
            }

            dicResult.Add("sUserCreate", sUserCreate);
            dicResult.Add("userCreateSize", userCreateSize);

            //Tính kích thước thông tin tên dòng họ
            SizeF familyNameSize = new SizeF(0, 0);
            string sFamilyName = "";
            if (optionExportPDF.FamilyNameSelect)
            {
                sFamilyName = $"GIA PHẢ DÒNG HỌ: {(optionExportPDF.FamilyName ?? "")}";
                familyNameSize = MeasureString(sFamilyName, AppConst.FontFamilyInfoForExportPDF);
                familyNameSize = new SizeF(familyNameSize.Width * 2 / 3, familyNameSize.Height + offsetLine);
            }
            maxWidth = familyNameSize.Width > maxWidth ? familyNameSize.Width : maxWidth;


            dicResult.Add("sFamilyName", sFamilyName);
            dicResult.Add("familyNameSize", familyNameSize);
            //Tính kích thước thông tin nguyên quán
            SizeF homeTowerSize = new SizeF(0, 0);
            string familyHomeTower = "";
            if (optionExportPDF.HomeTowerSelect)
            {
                string familyAnniversary = "NGÀY GIỖ TỔ: ";
                if (familyInfo != null)
                {
                    if (familyInfo.FamilyAnniversary != null)
                    {
                        LunarCalendar lunarCalendar = new LunarCalendar(familyInfo.FamilyAnniversary.Value);
                        string day = lunarCalendar.intLunarDay > 0 ? lunarCalendar.intLunarDay.ToString().PadLeft(2, '0') : "__";
                        string month = lunarCalendar.intLunarMonth > 0 ? lunarCalendar.intLunarMonth.ToString().PadLeft(2, '0') : "__";
                        familyAnniversary += $"{day}/{month}";
                    }
                }
                familyHomeTower = $"NGUYÊN QUÁN: {(optionExportPDF.HomeTower ?? "")},     {familyAnniversary}";
                homeTowerSize = MeasureString(familyHomeTower, AppConst.FontFamilyInfoForExportPDF);
                homeTowerSize = new SizeF(homeTowerSize.Width * 2 / 3, homeTowerSize.Height + offsetLine);
            }
            maxWidth = homeTowerSize.Width > maxWidth ? homeTowerSize.Width : maxWidth;

            dicResult.Add("familyHomeTower", familyHomeTower);
            dicResult.Add("homeTowerSize", homeTowerSize);
            //Tính kích thước thông tin thành viên là tổ phụ
            SizeF rootMemberSize = new SizeF(0, 0);
            string rootMemberName = "";
            if (optionExportPDF.RootMemberSelect)
            {
                using (var memberHelper = new MemberHelper())
                {
                    var rootMember = memberHelper.RootTMember;
                    if (rootMember != null)
                    {
                        rootMemberName = $"GIA ĐÌNH {(rootMember.Gender == (int)GPConst.EmGender.Male ? "ÔNG" : "BÀ")} {optionExportPDF.RootMember}";
                    }
                }
                rootMemberSize = MeasureString(rootMemberName, AppConst.FontFamilyInfoForExportPDF);
                rootMemberSize = new SizeF(rootMemberSize.Width * 2 / 3, rootMemberSize.Height + offsetLine);
            }
            maxWidth = rootMemberSize.Width > maxWidth ? rootMemberSize.Width : maxWidth;

            dicResult.Add("rootMemberName", rootMemberName);
            dicResult.Add("rootMemberSize", rootMemberSize);

            float dateCreateHeight = dateCreateSize.Height;
            float userCreateHeight = optionExportPDF.PositionUserCreate == AppConst.PositionUserCreate.Top_Right ? (dateCreateHeight > 0 ? 0 : userCreateSize.Height) : userCreateSize.Height;
            float familyNameHeight = familyNameSize.Height;
            float homeTowerHeight = homeTowerSize.Height;
            float rootMemberHeight = rootMemberSize.Height;

            if (optionExportPDF.PositionUserCreate == AppConst.PositionUserCreate.Top_Right || optionExportPDF.PositionUserCreate == AppConst.PositionUserCreate.Bottom_Right)
            {
                float temp = dateCreateSize.Width + userCreateSize.Width + 50;
                maxWidth = temp > maxWidth ? temp : maxWidth;
            }
            #endregion

            float w = (int)intMaxX + 10;
            w = maxWidth > w ? maxWidth : w;
            float h = (int)intMaxY + startOffsetY + dateCreateHeight + userCreateHeight + offsetUserCreateAndFamilyInfo + familyNameHeight + homeTowerHeight + rootMemberHeight + 50;
            dicResult.Add("width", w);
            dicResult.Add("height", h);
            return dicResult;
        }

        private void DrawText(SKCanvas canvas, string text, PointF point, Font font)
        {
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.TextSize = font.Size;
                paint.Color = ColorDrawHelper.FromColor(Color.Black);
                paint.Style = SKPaintStyle.StrokeAndFill;
                canvas.DrawText(text, point.X, point.Y, paint);
            }
        }

        private SizeF MeasureString(string text, Font font)
        {
            text = string.IsNullOrEmpty(text) ? "" : text;
            var size = TextRenderer.MeasureText(text, font);
            return size;
        }

        private void DrawTreeToCanvas(SKCanvas canvas, Dictionary<string, object> dicInfo, InfoOptionExportPDF optionExportPDF, ProgressBarManager progressBar, int startOffsetX, int startOffsetY, float w, float h)
        {
            float offset = 0;
            canvas.Translate(0, startOffsetY);
            if (optionExportPDF.DateCreateSelect)//Lựa chọn vẽ thông tin ngày tạo
            {
                Point p = new Point(startOffsetX, startOffsetY);
                var dateCreateSize = (SizeF)dicInfo.GetValue("dateCreateSize");
                offset = startOffsetY + dateCreateSize.Height;
                DrawText(canvas, optionExportPDF.DateCreate, p, AppConst.FontUserCreateForExportPDF);
            }
            if (optionExportPDF.UserCreateSelect)//Lựa chọn vẽ thông tin người lập
            {
                PointF p = new PointF(startOffsetX, offset);
                SizeF userCreateSize = (SizeF)dicInfo.GetValue("userCreateSize");
                switch (optionExportPDF.PositionUserCreate)
                {
                    case AppConst.PositionUserCreate.Top_Left://Tên người lập nằm ở phía trên bên trái
                        p = new PointF(startOffsetX, offset);
                        offset += userCreateSize.Height;
                        break;
                    case AppConst.PositionUserCreate.Top_Right://Tên người lập nằm ở phía trên bên phải
                        p = new PointF(w - userCreateSize.Width - startOffsetX, startOffsetY);
                        break;
                    case AppConst.PositionUserCreate.Bottom_Left://Tên người lập nằm ở phía dưới bên trái
                        p = new PointF(startOffsetX, h - userCreateSize.Height);
                        break;
                    case AppConst.PositionUserCreate.Bottom_Right://Tên người lập nằm ở phía dưới bên phải
                        p = new PointF(w - userCreateSize.Width - startOffsetX, h - userCreateSize.Height);
                        break;
                }
                var sUserCreate = (string)dicInfo.GetValue("sUserCreate");
                DrawText(canvas, sUserCreate, p, AppConst.FontUserCreateForExportPDF);

            }
            offset += 50;
            if (optionExportPDF.FamilyNameSelect)//Lựa chọn vẽ thông tin tên dòng họ
            {
                string sFamilyName = (string)dicInfo.GetValue("sFamilyName");
                var familyNameSize = (SizeF)dicInfo.GetValue("familyNameSize");
                PointF p = new PointF((w - familyNameSize.Width) / 2, offset);
                DrawText(canvas, sFamilyName, p, AppConst.FontFamilyInfoForExportPDF);
                offset += familyNameSize.Height;
            }
            if (optionExportPDF.HomeTowerSelect)//Lựa chọn vẽ thông tin nguyên quán
            {
                string familyHomeTower = (string)dicInfo.GetValue("familyHomeTower");
                var homeTowerSize = (SizeF)dicInfo.GetValue("homeTowerSize");

                PointF p = new PointF((w - homeTowerSize.Width) / 2, offset);
                DrawText(canvas, familyHomeTower, p, AppConst.FontFamilyInfoForExportPDF);
                offset += homeTowerSize.Height;
            }
            if (optionExportPDF.RootMemberSelect)//Lựa chọn vẽ thông tin tên thành viên là tổ phụ
            {
                string rootMemberName = (string)dicInfo.GetValue("rootMemberName");
                var rootMemberSize = (SizeF)dicInfo.GetValue("rootMemberSize");

                PointF p = new PointF((w - rootMemberSize.Width) / 2, offset);
                DrawText(canvas, rootMemberName, p, AppConst.FontFamilyInfoForExportPDF);
                offset += rootMemberSize.Height;
            }

            flZoomTemp = flZoomLevel;
            flZoomLevel = 1;
            canvas.Translate(0, offset);
            DrawTree(canvas, true, progressBar, false, true);
            flZoomLevel = flZoomTemp;

        }

        public void CenteringRoot()
        {
            if (RootInfo != null)
            {
                CenteringMember(RootInfo.Id);
            }
        }

        public void CenteringMember(string intMember)
        {
            if (string.IsNullOrEmpty(intMember))
            {
                return;
            }

            var objMember = GetDataMember(intMember);

            if (objMember != null)
            {
                var x = -objMember.Position.X + (_controlTree.Width / 2) / flZoomLevel - objMember.Width;
                var y = OffsetYCanvas == 0 ? -objMember.Position.Y + (_controlTree.Height / 2) / flZoomLevel - objMember.Height : OffsetYCanvas;
                _pointTranslate = new SKPoint(x, y);
                _translateCurrentDistance = _pointTranslate;
                SelectedMember = objMember;
                Invalidate();
            }
        }

        protected bool _invalidateTrans;

        public void BeginInvalidate()
        {
            _invalidateTrans = true;
        }

        public void EndInvalidate()
        {
            _invalidateTrans = false;
            Invalidate();
        }

        public void Invalidate()
        {
            if (_invalidateTrans)
            {
                return;
            }

            _controlTree.Invalidate();
        }

        public void ZoomOut(int intNum = 1)
        {
            if (flZoomLevel < MaxZoomLevel)
            {
                intNum = intNum > 0 ? intNum : 1;
                flZoomLevel = flZoomLevel + (ZoomLevel * intNum);
                Invalidate();
            }
        }

        public void ZoomIn(int intNum = 1)
        {
            if (flZoomLevel > MinZoomLevel)
            {
                intNum = intNum > 0 ? intNum : 1;
                flZoomLevel = flZoomLevel - (ZoomLevel * intNum);
                Invalidate();
            }
        }

        #endregion Public Func Control

        #region Event SKTree

        protected void skTree_PaintSurface(object sender, SKCanvas canvas)
        {
            canvas.Clear(Config.BackgroudColor);
            if (RootInfo == null)
            {
                return;
            }

            //if (blnFit)
            //{
            //    SetFit(canvas);
            //    blnFit = false;
            //}

            canvas.Scale(flZoomLevel);
            if (DrawToPrint && DrawFirst)
            {
                _translateCurrentDistance = _pointTranslate = new SKPoint(0, 0);
                DrawFirst = false;
            }
            canvas.Translate(_pointTranslate);
            DrawTree(canvas, false);

            if (SelectedMember != null)
            {
                using (SKPaint paint = new SKPaint())
                {
                    paint.Color = Config.SelectedMemberColor;
                    paint.Style = SKPaintStyle.Stroke;
                    paint.StrokeWidth = 5;
                    paint.IsAntialias = true;
                    canvas.DrawRoundRect(SelectedMember.SelectedBorder, new SKSize(0, 0), paint);
                }
            }
            canvas.ResetMatrix();
        }

        protected void skTree_MouseClick(object sender, MouseEventArgs e)
        {
            if (!blnMouseDown) return;
            var objMember = getMemberClicked(e);
            if (objMember != null) SelectedMember = objMember;
        }

        protected void skTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!blnMouseDown || e.Button != MouseButtons.Left)
            {
                return;
            }

            var objMember = getMemberClicked(e);

            if (objMember != null && MemberDoubleClick != null)
            {
                MemberDoubleClick(objMember, e);
            }
        }

        protected void skTree_MouseDown(object sender, MouseEventArgs e)
        {
            blnMouseDown = true;
            pointMouseDown = new Point(e.X, e.Y);
            // Set cursor as hourglass
            Cursor.Current = Cursors.Hand;
            var objMember = getMemberClicked(e);
            if (e.Button == MouseButtons.Right && MemberRightClick != null)
            {
                if (objMember != null)
                {
                    SelectedMember = objMember;
                    MemberRightClick(this, objMember);
                }
            }
            else if (e.Button == MouseButtons.Left && MouseLeftClickCanvas != null)
            {
                if (objMember == null)
                {
                    MouseLeftClickCanvas?.Invoke(sender, e);
                }
            }
        }

        protected void skTree_MouseLeave(object sender, EventArgs e)
        {
            blnMouseDown = false;
            Invalidate();

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }
        protected void skTree_MouseMove(object sender, MouseEventArgs e)
        {
            if (blnMouseDown)
            {
                _pointTranslate = new SKPoint(_translateCurrentDistance.X + (e.X - pointMouseDown.X) / flZoomLevel, _translateCurrentDistance.Y + (e.Y - pointMouseDown.Y) / flZoomLevel);
                SelectedMember = null;
            }
        }

        protected void skTree_MouseUp(object sender, MouseEventArgs e)
        {

            blnMouseDown = false;
            //pointMouseDown = new Point(e.X, e.Y);
            _translateCurrentDistance = _pointTranslate;

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        protected void skTree_MouseWheel(object sender, MouseEventArgs e)
        {
            BeginInvalidate();

            if (e.Delta > 0)
            {
                ZoomOut();
            }
            else
            {
                ZoomIn();
            }

            CenteringMember(SelectedMember?.Id);
            EndInvalidate();
        }

        #endregion Event SKTree

        protected clsFamilyMember getMemberClicked(MouseEventArgs e)
        {
            var objMember = (from keyHas in Config.DataMember.Keys.Cast<string>()
                             where IsMemberClicked((clsFamilyMember)Config.DataMember[keyHas], e.X / flZoomLevel, e.Y / flZoomLevel)
                             select (clsFamilyMember)Config.DataMember[keyHas]).FirstOrDefault();

            return objMember;
        }

        public void GetBound()
        {
            intMinX = float.MaxValue;
            intMaxX = float.MinValue;

            intMaxY = float.MinValue;
            intMinY = float.MaxValue;

            Config.DataMember.Values.Cast<clsFamilyMember>().Select(objMember =>
            {
                objMember.TemplateType = Config.TemplateMember;

                if (intMinX > objMember.Position.X) intMinX = objMember.Position.X;
                if (intMinY > objMember.Position.Y) intMinY = objMember.Position.Y;

                if (intMaxX < objMember.Position.X + objMember.Width) intMaxX = objMember.Position.X + objMember.Width;
                if (intMaxY < objMember.Position.Y + objMember.Height) intMaxY = objMember.Position.Y + objMember.Height;

                return objMember;
            }).ToList();
        }
        protected void ZoomLevelFit()
        {
            GetBound();
            float flZH = (_controlTree.Height) / (intMaxY - intMinY + 100);
            float flZW = (_controlTree.Width) / (intMaxX - intMinX + 100);
            flZH = flZH > flZW ? flZW : flZH;
            flZoomLevel = flZH < 0.3f ? 0.3f : flZH > MaxZoomLevel ? MaxZoomLevel : flZH;
            Invalidate();
        }

        public void SetFit()
        {
            //blnFit = true;
            Invalidate();
        }

        protected void SetFit(SKCanvas canvas)
        {
            GetBound();
            SKRectI skCanvasBound = canvas.DeviceClipBounds;
            float flZ = (skCanvasBound.Height) / (intMaxY - intMinY);
            flZ = flZ * (skCanvasBound.Width) / (intMaxX - intMinX);
            flZoomLevel = flZ < 0.3f ? 0.3f : flZ > MaxZoomLevel ? MaxZoomLevel : flZ;
        }

        protected void xDrawSpouseLine(SKCanvas canvas, clsFamilyMember objMember, clsFamilyMember objSpouse)
        {
            SKPoint objLeftPoint;
            SKPoint objRightPoint;

            SKPaint paint = new SKPaint();
            paint.Color = Config.SpouseLineColor;
            //paint.TextScaleX = 1.5f;
            paint.IsAntialias = true;
            paint.StrokeWidth = 2;
            paint.Style = SKPaintStyle.Stroke;

            if (objMember.Position.X < objSpouse.Position.X)
            {
                objLeftPoint = new SKPoint(objSpouse.Position.X, objSpouse.Position.Y + objSpouse.Height / 2);
                objRightPoint = new SKPoint(objMember.Position.X + objMember.Width, objMember.Position.Y + objMember.Height / 2);
            }
            else
            {
                objLeftPoint = new SKPoint(objMember.Position.X, objMember.Position.Y + objMember.Height / 2);
                objRightPoint = new SKPoint(objSpouse.Position.X + objSpouse.Width, objSpouse.Position.Y + objSpouse.Height / 2);
            }
            if (DrawToPrint)
            {
                if (objRightPoint.X > MaxWidth)
                {
                    return;
                }
                objLeftPoint.X = objLeftPoint.X > MaxWidth ? MaxWidth : objLeftPoint.X;
            }
            canvas.DrawLine(objLeftPoint, objRightPoint, paint);
            paint.Dispose();
        }

        protected void xDrawSpouseLine(SKCanvas canvas, clsFamilyMember objMember)
        {
            if (objMember.lstSpouse.Count <= 0) return;

            clsFamilyMember objSpouse = GetDataMember(objMember.lstSpouse[0]);
            xDrawSpouseLine(canvas, objSpouse, objMember);

            for (int i = 1; i < objMember.lstSpouse.Count; i++)
            {
                clsFamilyMember objSpouse1 = GetDataMember(objMember.lstSpouse[i - 1]);
                clsFamilyMember objSpouse2 = GetDataMember(objMember.lstSpouse[i]);

                if (objMember.Position.X < objSpouse2.Position.X && objMember.Position.X > objSpouse1.Position.X)
                {
                    xDrawSpouseLine(canvas, objMember, objSpouse2);
                }
                else
                {
                    xDrawSpouseLine(canvas, objSpouse1, objSpouse2);
                }
            }
        }

        protected SKPaint _paintLine;

        protected SKPaint PainLine
        {
            get
            {
                _paintLine = new SKPaint();
                _paintLine.Color = Config.ChildLineColor;
                _paintLine.IsAntialias = true;
                _paintLine.StrokeWidth = 2;
                _paintLine.Style = SKPaintStyle.Stroke;
                _paintLine.FilterQuality = SKFilterQuality.Low;

                return _paintLine;
            }
        }

        protected virtual void xDrawFatherChildLine(SKCanvas canvas, clsFamilyMember objFather, clsFamilyMember objChild, bool notCheck = false, bool drawAllLine = false)
        {

        }
        protected bool InCanvas(object obj, SKCanvas canvas, bool drawToPrint = false)
        {
            clsFamilyMember objMember = (clsFamilyMember)obj;
            var up = objMember.Size.Height;
            var down = -up;
            var test = new SKRectI(AutoUp(canvas.DeviceClipBounds.Left, down),
                                   AutoUp(canvas.DeviceClipBounds.Top, down),
                                   AutoUp(canvas.DeviceClipBounds.Right, up),
                                   AutoUp(canvas.DeviceClipBounds.Bottom, up));

            bool inCanvas = drawToPrint || test.Contains((int)(objMember.Position.X + _pointTranslate.X), (int)(objMember.Position.Y + _pointTranslate.Y));
            bool inPage = true;
            if (DrawToPrint)
            {
                inPage = inCanvas ? objMember.Position.X + objMember.Width <= MaxWidth && objMember.Position.Y + objMember.Height <= MaxHeight : inPage;
            }
            return inCanvas && inPage;
        }

        protected bool InCanvasDrawLine(SKPoint startLine, SKPoint endLine, SKCanvas canvas, bool drawAll = false)
        {
            var width = (int)Math.Ceiling(Math.Abs(endLine.X - startLine.X));
            var height = (int)Math.Ceiling(Math.Abs(endLine.Y - startLine.Y));
            var up = width > height ? width : height;
            var down = -up;
            var test = new SKRectI(AutoUp(canvas.DeviceClipBounds.Left, down),
                                   AutoUp(canvas.DeviceClipBounds.Top, down),
                                   AutoUp(canvas.DeviceClipBounds.Right, up),
                                   AutoUp(canvas.DeviceClipBounds.Bottom, up));

            return (test.Contains((int)(startLine.X + _pointTranslate.X), (int)(startLine.Y + _pointTranslate.Y))
                   || test.Contains((int)(endLine.X + _pointTranslate.X), (int)(endLine.Y + _pointTranslate.Y))) || drawAll;
        }

        protected bool InCanvasDrawLine(object obj, SKCanvas canvas)
        {
            clsFamilyMember objMember = (clsFamilyMember)obj;
            bool blnRet = canvas.DeviceClipBounds.Contains(10, (int)(objMember.Position.Y + _pointTranslate.Y + objMember.Height + Config.MemberVerticalSpace));
            //blnRet = blnRet | canvas.DeviceClipBounds.Contains(10, (int)(objMember.miPosition.Y + translateDistance.Y - objMember.miSize.Height/2));
            return blnRet;
        }

        protected bool InGraphicDrawLine(SKPoint startLine, SKPoint endLine, Rectangle drawRegion)
        {
            return drawRegion.Contains((int)(startLine.X + _pointTranslate.X), (int)(startLine.Y + _pointTranslate.Y))
                   || drawRegion.Contains((int)(endLine.X + _pointTranslate.X), (int)(endLine.Y + _pointTranslate.Y));
        }

        protected int AutoUp(int input, int up)
        {
            return (int)(Math.Ceiling((input + up) / flZoomLevel));
        }

        public void DrawTree(SKCanvas canvas, bool drawAll = false, ProgressBarManager progressBar = null, bool drawToPrint = false, bool byPassReDraw = false)
        {
            //using (SKPaint paint = new SKPaint())
            using var memberHelper = new MemberHelper();

            flZoomLevel = drawToPrint ? 1 : flZoomLevel;
            {
                var dataRaw = (from keyHas in Config.DataMember.Keys.Cast<string>()
                               where (drawAll || InCanvas(Config.DataMember[keyHas], canvas, drawToPrint))
                               select (clsFamilyMember)Config.DataMember[keyHas]).ToList();

                dataRaw = dataRaw.Where(m => int.Parse(m.LevelInFamily) <= Config.MaxLevelInFamily).ToList();

                if (byPassReDraw)
                {
                    dataRaw.ForEach(dat => { dat.ReDraw = false; });
                }

                int totalProgressExportPDF = dataRaw.Count + 1;
                var dataRawSpouseLine = (from keyHas in Config.DataMember.Keys.Cast<string>()
                                             //where (drawAll || InCanvasDrawLine(Config.DataMember[keyHas], canvas))
                                         select (clsFamilyMember)Config.DataMember[keyHas]);

                Func<clsConst.ENUM_GENDER, clsFamilyMember> getTemplate = (gender) =>
                {
                    if (gender == clsConst.ENUM_GENDER.Male)
                    {
                        return _memberMale;
                    }
                    if (gender == clsConst.ENUM_GENDER.FMale)
                    {
                        return _memberFMale;
                    }
                    return _memberUnknown;
                };
                var rootMember = memberHelper.RootTMember;
                if (rootMember == null)
                {
                    return;
                }
                string rootID = rootMember.Id;
                int cnt = 0;
                Stopwatch sw = new Stopwatch();
                dataRaw.ForEach(objMember =>
                {
                    sw.Restart();
                    objMember.TemplateType = Config.TemplateMember;

                    var templateMember = getTemplate(objMember.Gender);
                    if (!string.IsNullOrEmpty(objMember.strBackColor) && !objMember.UseDefaultColor)
                    {
                        canvas.DrawPicture(objMember.miSvgData.Picture, objMember.Position);
                    }
                    else
                    {
                        canvas.DrawPicture(templateMember.miSvgData.Picture, objMember.Position);
                    }

                    var tt1 = sw.ElapsedMilliseconds;

                    if (flZoomLevel > MinZoomLevel)
                    {
                        canvas.DrawPicture(objMember.ToPicture(objMember.ReDraw), objMember.Position);
                        objMember.ReDraw = false;
                    }
                    else
                    {
                        canvas.DrawPicture(templateMember.miSvgAvatar.Picture, objMember.Position);
                    }

                    var tt2 = sw.ElapsedMilliseconds;

                    // for (int i = 0; i < objMember.lstChild.Count; i++)
                    foreach (string childID in objMember.lstChild)
                    {
                        xDrawFatherChildLine(canvas, objMember, (clsFamilyMember)Config.DataMember[childID], false, true);
                    }

                    var tt3 = sw.ElapsedMilliseconds;

                    xDrawSpouseLine(canvas, objMember);

                    var tt4 = sw.ElapsedMilliseconds;

                    if (progressBar != null)
                    {
                        progressBar.Percent = progressBar.fncCalculatePercent2(++cnt, totalProgressExportPDF);
                    }
                    var tt5 = sw.ElapsedMilliseconds;
                    sw.Stop();
                    var tt6 = sw.ElapsedMilliseconds;
                });

                dataRawSpouseLine.Where(i => !dataRaw.Any(j => j.Id == i.Id)).Select(objMember =>
                {
                    objMember.TemplateType = Config.TemplateMember;

                    //xDrawSpouseLine(canvas, objMember);
                    //for (int i = 0; i < objMember.lstChild.Count; i++)
                    foreach (string childID in objMember.lstChild)
                    {
                        xDrawFatherChildLine(canvas, objMember, (clsFamilyMember)Config.DataMember[childID]);
                    }

                    //xDrawSpouseLine(canvas, objMember);

                    return objMember;
                }).ToList();

                //foreach (var objMember in dataRaw)
                //{
                //    objMember.TemplateType = Config.TemplateMember;
                //    objMember.DrawingMemberSVG(objMember.ReDraw);
                //    canvas.DrawPicture(objMember.miSvgData.Picture, objMember.miPosition, paint);

                //    for (int i = 0; i < objMember.lstChild.Count; i++)
                //    {
                //        xDrawFatherChildLine(canvas, objMember, (clsFamilyMember)Config.DataMember[objMember.lstChild[i]]);
                //    }

                //    xDrawSpouseLine(canvas, objMember);
                //    objMember.FreeSVGData();
                //}

                //foreach (var objMember in dataRawSpouseLine)
                //{
                //    objMember.TemplateType = Config.TemplateMember;
                //    xDrawSpouseLine(canvas, objMember);
                //    for (int i = 0; i < objMember.lstChild.Count; i++)
                //    {
                //        xDrawFatherChildLine(canvas, objMember, (clsFamilyMember)Config.DataMember[objMember.lstChild[i]]);
                //    }

                //    xDrawSpouseLine(canvas, objMember);
                //}
            }

        }




        /// <summary>
        /// MakeTreeDraw, calculate the position of member in tree with no spouse drawing
        /// </summary>
        /// <param name="intRootID"></param>
        /// <param name="intMinTop"></param>
        /// <param name="intMinLeft"></param>
        /// <param name="hasMember"></param>
        public void MakeTreeDraw(string intRootID, float intMinTop, float intMinLeft)
        {
            clsFamilyMember objRoot = (clsFamilyMember)Config.DataMember[intRootID];
            if (objRoot.blnPos) return;
            objRoot.Position.Y = intMinTop;

            if (objRoot.isNoChild())
            {
                objRoot.Position.X = intMinLeft;
                objRoot.miMaxTreeRight = objRoot.Position.X + objRoot.Width + Config.MemberHorizonSpace;
                //objRoot.miMinTreeLeft = intMinLeft;
                objRoot.blnPos = true;
                return;
            }

            int i = 0;
            MakeTreeDraw(objRoot.lstChild[i], intMinTop + objRoot.Height + Config.MemberVerticalSpace, intMinLeft);

            for (i = 1; i < objRoot.lstChild.Count; i++)
            {
                clsFamilyMember objPrevBrother = (clsFamilyMember)Config.DataMember[objRoot.lstChild[i - 1]];
                MakeTreeDraw(objRoot.lstChild[i], objPrevBrother.Position.Y, objPrevBrother.miMaxTreeRight);
            }

            clsFamilyMember objFirstChild = (clsFamilyMember)Config.DataMember[objRoot.lstChild[0]];
            clsFamilyMember objLastChild = (clsFamilyMember)Config.DataMember[objRoot.lstChild[objRoot.lstChild.Count - 1]];
            objRoot.Position.X = (objFirstChild.Position.X + objLastChild.Position.X
                + objLastChild.Width) / 2 - objRoot.Width / 2;
            //objRoot.miMaxTreeRight = objLastChild.miPosition.X + objLastChild.miSize.Width + intMemberHorizonSpace;
            objRoot.miMaxTreeRight = objLastChild.miMaxTreeRight;
            //objRoot.miMinTreeLeft = objFirstChild.miMinTreeLeft;
            objRoot.blnPos = true;
            return;
        }

        /// <summary>
        /// Update Spouse Position of a member based on that member position is calculated
        /// </summary>
        /// <param name="objMember"></param>
        /// <param name="hasMember"></param>
        /// <param name="enSpousePos"></param>
        protected virtual void UpdateSpousePosition(ref clsFamilyMember objMember, clsConst.ENUM_FIRSTSPOUSE_POSITION enSpousePos = clsConst.ENUM_FIRSTSPOUSE_POSITION.RightMember)
        {

        }
        public bool IsMemberClicked(clsFamilyMember objMember, float x, float y)
        {
            return objMember.CheckMemberClick(new SKPoint(x - _translateCurrentDistance.X, y - _translateCurrentDistance.Y));
        }

        protected int xParentWidth(clsFamilyMember objFather)
        {
            int intParentCount = objFather.lstSpouse.Count + 1;
            int intWidth = intParentCount * objFather.Width + (intParentCount - 1) * Config.MemberHorizonSpace;
            return intWidth;
        }

        /// <summary>
        /// MakeTreeDrawWithSpouse, calculate the position of member in tree include spouses
        /// </summary>
        /// <param name="intRootID"></param>
        /// <param name="intMinTop"></param>
        /// <param name="intMinLeft"></param>
        /// <param name="hasMember"></param>
        /// <param name="enSpousePos"></param>
        protected virtual void MakeTreeDrawWithSpouse(string intRootID, float intMinTop, float intMinLeft, clsConst.ENUM_FIRSTSPOUSE_POSITION enSpousePos = clsConst.ENUM_FIRSTSPOUSE_POSITION.RightMember)
        {

        }

        public void MakeTreeDrawinBox(int intRootID, float intMinTop, float intMinLeft, Hashtable hasMember, clsConst.ENUM_FIRSTSPOUSE_POSITION enSpousePos = clsConst.ENUM_FIRSTSPOUSE_POSITION.RightMember)
        {
            clsFamilyMember objRoot = (clsFamilyMember)hasMember[intRootID];
            if (objRoot.blnPos) return;
            objRoot.Position.Y = intMinTop;
            objRoot.miMinTreeLeft = intMinLeft;

            if (objRoot.isNoChild())
            {
                objRoot.Position.X = intMinLeft;
                objRoot.miMaxTreeRight = objRoot.Position.X + objRoot.Width + Config.MemberHorizonSpace;
                objRoot.blnPos = true;
                UpdateSpousePosition(ref objRoot, enSpousePos);
                return;
            }
        }
    }
}