using GP40Common;
using GP40Main.Utility;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
        public int MemberVerticalSpace { get; set; } = 30;
        public int MemberHorizonSpace { get; set; } = 30;
        public int NumberFrame { get; set; } = 1;
    }

    public class SKGLCustom : SKGLControl
    {
        public SKGLCustom() : base()
        {
            this.VSync = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.MakeCurrent();
            base.OnPaint(e);
        }
    }

    public class DrawTreeManager : IDisposable
    {
        private const float MinZoomLevel = 0.2f;
        private const float MaxZoomLevel = 2f;
        private const float ZoomLevel = 0.05f;

        private bool blnMouseDown = false;
        private SKPoint translateDistance;
        private SKPoint translateCurrentDistance;
        private Point pointMouseDown = new Point(0, 0);
        private float intMinX, intMinY, intMaxX, intMaxY;
        private bool blnFit = false;
        private float flZoomLevel = 1f;
        private SKGLCustom _skTree;

        private clsFamilyMember objSelectedMember;
        private clsFamilyMember objMale = new clsFamilyMember() { miID = clsConst.ENUM_GENDER.Male.ToString(), Gender = clsConst.ENUM_GENDER.Male };
        private clsFamilyMember objFMale = new clsFamilyMember() { miID = clsConst.ENUM_GENDER.FMale.ToString(), Gender = clsConst.ENUM_GENDER.FMale };
        private clsFamilyMember objUnknown = new clsFamilyMember() { miID = clsConst.ENUM_GENDER.Unknown.ToString(), Gender = clsConst.ENUM_GENDER.Unknown };

        private bool disposedValue;

        public clsFamilyMember SelectedMember   // property
        {
            get { return objSelectedMember; }   // get method
            set
            {
                // set method
                objSelectedMember = value;
                Invalidate();
            }
        }

        public SKGLCustom Tree => _skTree;
        public DrawTreeConfig Config { get; set; }
        public clsFamilyMember RootInfo { get; private set; }

        public event EventHandler<MouseEventArgs> MemberDoubleClick;
        public event EventHandler<clsFamilyMember> MemberRightClick;

        public DrawTreeManager(DrawTreeConfig config, SKGLCustom skTree = null)
        {
            if (string.IsNullOrEmpty(config.PathRoot))
            {
                config.PathRoot = Application.StartupPath + @"\";
            }

            if (config.DataMember == null)
            {
                config.DataMember = new Hashtable();
            }

            Config = config;

            _skTree = skTree != null ? skTree : new SKGLCustom();
            _skTree.PaintSurface += new EventHandler<SKPaintGLSurfaceEventArgs>(skTree_PaintSurface);
            _skTree.MouseClick += new MouseEventHandler(skTree_MouseClick);
            _skTree.MouseDoubleClick += new MouseEventHandler(skTree_MouseDoubleClick);
            _skTree.MouseDown += new MouseEventHandler(skTree_MouseDown);
            _skTree.MouseLeave += new EventHandler(skTree_MouseLeave);
            _skTree.MouseMove += new MouseEventHandler(skTree_MouseMove);
            _skTree.MouseUp += new MouseEventHandler(skTree_MouseUp);
            _skTree.MouseWheel += new MouseEventHandler(skTree_MouseWheel);
        }

        private clsFamilyMember GetDataMember(string intMemberId)
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
                        objMember.TemplateType = Config.TemplateMember;
                        objMember.TextColor = Config.TextColor;
                        objMember.BorderColor = Config.BorderColor;
                        objMember.BackColor = objMember.Gender == clsConst.ENUM_GENDER.FMale ? Config.FeMaleBackColor : Config.MaleBackColor;
                        objMember.FrameImage = clsConst.FramePath + Config.NumberFrame.ToString().PadLeft(2, '0') + ".png";
                        objMember.ReDraw = true;
                    }
                };

                updateMember(objMale);
                updateMember(objFMale);
                updateMember(objUnknown);

                objMale.DrawingMemberSVGFrame(true);
                objFMale.DrawingMemberSVGFrame(true);
                objUnknown.DrawingMemberSVGFrame(true);

                objMale.DrawingMemberSVGAvatarGender(true);
                objFMale.DrawingMemberSVGAvatarGender(true);
                objUnknown.DrawingMemberSVGAvatarGender(true);

                // Reset Member Data
                Config.DataMember.Keys.Cast<string>().Select((keyMember) =>
                {
                    var objMember = (clsFamilyMember)Config.DataMember[keyMember];
                    updateMember(objMember);
                    return keyMember;
                }).ToList();

                BeginInvalidate();

                this.MakeTreeDrawWithSpouse(rootId, 10, 10, clsConst.ENUM_FIRSTSPOUSE_POSITION.LeftMember);

                this.flZoomLevel = 1f;
                this.SelectedMember = null;
                this.ZoomLevelFit();
                this.CenteringRoot();

                EndInvalidate();
            }
        }

        public void ExportPdf()
        {
            float dpi = 300;

            // create the document
            using (var stream = SKFileWStream.OpenStream("FamilyTree.pdf"))
            {

                using (var document = SKDocument.CreatePdf(stream, dpi))
                {
                    // get the canvas from the page
                    using (var canvas = document.BeginPage(46.8f * dpi, 33.1f * dpi))
                    {
                        DrawTree(canvas);
                    }

                    document.EndPage();
                }
            }
        }

        public void CenteringRoot()
        {
            if (RootInfo != null)
            {
                CenteringMember(RootInfo.miID);
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
                var x = -objMember.miPosition.X + (_skTree.Width / 2) / flZoomLevel - objMember.Width;
                var y = -objMember.miPosition.Y + (_skTree.Height / 2) / flZoomLevel - objMember.Height;
                translateDistance = new SKPoint(x, y);
                translateCurrentDistance = translateDistance;
                Invalidate();
            }
        }

        private bool _invalidateTrans;

        public void BeginInvalidate() {
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

            _skTree.Invalidate();
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

        private void skTree_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            //canvas.Save();
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
            canvas.Translate(translateDistance);

            DrawTree(canvas);

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

        private void skTree_MouseClick(object sender, MouseEventArgs e)
        {
            if (!blnMouseDown) return;
            var objMember = getMemberClicked(e);
            if (objMember != null) SelectedMember = objMember;
        }

        private void skTree_MouseDoubleClick(object sender, MouseEventArgs e)
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

        private void skTree_MouseDown(object sender, MouseEventArgs e)
        {
            blnMouseDown = true;
            pointMouseDown = new Point(e.X, e.Y);
            // Set cursor as hourglass
            Cursor.Current = Cursors.Hand;

            if (e.Button == MouseButtons.Right && MemberRightClick != null)
            {
                var objMember = getMemberClicked(e);

                if (objMember != null)
                {
                    SelectedMember = objMember;
                    MemberRightClick(this, objMember);
                }
            }
        }

        private void skTree_MouseLeave(object sender, EventArgs e)
        {
            blnMouseDown = false;
            Invalidate();

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void skTree_MouseMove(object sender, MouseEventArgs e)
        {
            if (blnMouseDown)
            {
                translateDistance = new SKPoint(translateCurrentDistance.X + (e.X - pointMouseDown.X) / flZoomLevel, translateCurrentDistance.Y + (e.Y - pointMouseDown.Y) / flZoomLevel);
                SelectedMember = null;
            }
        }

        private void skTree_MouseUp(object sender, MouseEventArgs e)
        {
            blnMouseDown = false;
            pointMouseDown = new Point(e.X, e.Y);
            translateCurrentDistance = translateDistance;

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void skTree_MouseWheel(object sender, MouseEventArgs e)
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

            CenteringMember(SelectedMember?.miID);
            EndInvalidate();
        }

        #endregion Event SKTree

        private clsFamilyMember getMemberClicked(MouseEventArgs e)
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

                if (intMinX > objMember.miPosition.X) intMinX = objMember.miPosition.X;
                if (intMinY > objMember.miPosition.Y) intMinY = objMember.miPosition.Y;

                if (intMaxX < objMember.miPosition.X + objMember.Width) intMaxX = objMember.miPosition.X + objMember.Width;
                if (intMaxY < objMember.miPosition.Y + objMember.Height) intMaxY = objMember.miPosition.Y + objMember.Height;

                return objMember;
            }).ToList();
        }

        private void ZoomLevelFit()
        {
            GetBound();
            float flZ = (_skTree.Height) / (intMaxY - intMinY);
            flZ = flZ * (_skTree.Width) / (intMaxX - intMinX);
            flZoomLevel = flZ < 0.3f ? 0.3f : flZ;
            Invalidate();
        }

        public void SetFit()
        {
            blnFit = true;
            Invalidate();
        }

        private void SetFit(SKCanvas canvas)
        {
            GetBound();
            SKRectI skCanvasBound = canvas.DeviceClipBounds;
            float flZ = (skCanvasBound.Height) / (intMaxY - intMinY);
            flZ = flZ * (skCanvasBound.Width) / (intMaxX - intMinX);
            flZoomLevel = flZ < 0.3f ? 0.3f : flZ;
        }

        private void xDrawSpouseLine(SKCanvas canvas, clsFamilyMember objMember, clsFamilyMember objSpouse)
        {
            SKPoint objLeftPoint;
            SKPoint objRightPoint;

            SKPaint paint = new SKPaint();
            paint.Color = Config.SpouseLineColor;
            //paint.TextScaleX = 1.5f;
            paint.IsAntialias = true;
            paint.StrokeWidth = 2;
            paint.Style = SKPaintStyle.Stroke;

            if (objMember.miPosition.X < objSpouse.miPosition.X)
            {
                objLeftPoint = new SKPoint(objSpouse.miPosition.X, objSpouse.miPosition.Y + objSpouse.Height / 2);
                objRightPoint = new SKPoint(objMember.miPosition.X + objMember.Width, objMember.miPosition.Y + objMember.Height / 2);
                canvas.DrawLine(objLeftPoint, objRightPoint, paint);
            }
            else
            {
                objLeftPoint = new SKPoint(objMember.miPosition.X, objMember.miPosition.Y + objMember.Height / 2);
                objRightPoint = new SKPoint(objSpouse.miPosition.X + objSpouse.Width, objSpouse.miPosition.Y + objSpouse.Height / 2);
                canvas.DrawLine(objLeftPoint, objRightPoint, paint);
            }
            paint.Dispose();
        }

        private void xDrawSpouseLine(SKCanvas canvas, clsFamilyMember objMember)
        {
            if (objMember.lstSpouse.Count <= 0) return;

            clsFamilyMember objSpouse = GetDataMember(objMember.lstSpouse[0]);
            xDrawSpouseLine(canvas, objSpouse, objMember);

            for (int i = 1; i < objMember.lstSpouse.Count; i++)
            {
                clsFamilyMember objSpouse1 = GetDataMember(objMember.lstSpouse[i - 1]);
                clsFamilyMember objSpouse2 = GetDataMember(objMember.lstSpouse[i]);

                if (objMember.miPosition.X < objSpouse2.miPosition.X && objMember.miPosition.X > objSpouse1.miPosition.X)
                {
                    xDrawSpouseLine(canvas, objMember, objSpouse2);
                }
                else
                {
                    xDrawSpouseLine(canvas, objSpouse1, objSpouse2);
                }
            }
        }

        private SKPaint _paintLine;

        private SKPaint PainLine
        {
            get
            {
                if (_paintLine == null)
                {
                    _paintLine = new SKPaint();
                    _paintLine.Color = Config.ChildLineColor;
                    _paintLine.IsAntialias = true;
                    _paintLine.StrokeWidth = 2;
                    _paintLine.Style = SKPaintStyle.Stroke;
                    _paintLine.FilterQuality = SKFilterQuality.Low;
                }

                return _paintLine;
            }
        }

        private void xDrawFatherChildLine(SKCanvas canvas, clsFamilyMember objFather, clsFamilyMember objChild, bool notCheck = false)
        {
            if (objFather == null)
            {
                return;
            }

            SKPoint objFatherPoint = new SKPoint(objFather.miPosition.X + objFather.Width / 2, objFather.miPosition.Y + objFather.Height);
            SKPoint objFatherMiddlePoint = new SKPoint(objFatherPoint.X, objFatherPoint.Y + Config.MemberVerticalSpace / 2);

            if (notCheck)
            {
                canvas.DrawLine(objFatherPoint, objFatherMiddlePoint, PainLine);
            }
            else
            {
                if (InCanvasDrawLine(objFatherPoint, objFatherMiddlePoint, canvas))
                {
                    canvas.DrawLine(objFatherPoint, objFatherMiddlePoint, PainLine);
                }
            }

            if (objChild == null)
            {
                return;
            }

            SKPoint objChildPoint = new SKPoint(objChild.miPosition.X + objChild.Width / 2, objChild.miPosition.Y);
            SKPoint objChildMiddlePoint = new SKPoint(objChildPoint.X, objChildPoint.Y - Config.MemberVerticalSpace / 2);

            if (notCheck)
            {
                canvas.DrawLine(objChildMiddlePoint, objFatherMiddlePoint, PainLine);
                canvas.DrawLine(objChildMiddlePoint, objChildPoint, PainLine);
                return;
            }

            if (InCanvasDrawLine(objChildMiddlePoint, objFatherMiddlePoint, canvas))
            {
                canvas.DrawLine(objChildMiddlePoint, objFatherMiddlePoint, PainLine);
            }

            if (InCanvasDrawLine(objChildMiddlePoint, objChildPoint, canvas))
            {
                canvas.DrawLine(objChildMiddlePoint, objChildPoint, PainLine);
            }
        }

        private bool InCanvas(object obj, SKCanvas canvas)
        {
            clsFamilyMember objMember = (clsFamilyMember)obj;
            var up = objMember.miSize.Height;
            var down = -up;
            var test = new SKRectI(AutoUp(canvas.DeviceClipBounds.Left, down),
                                   AutoUp(canvas.DeviceClipBounds.Top, down),
                                   AutoUp(canvas.DeviceClipBounds.Right, up),
                                   AutoUp(canvas.DeviceClipBounds.Bottom, up));

            return test.Contains((int)(objMember.miPosition.X + translateDistance.X), (int)(objMember.miPosition.Y + translateDistance.Y));
        }

        private bool InCanvasDrawLine(SKPoint startLine, SKPoint endLine, SKCanvas canvas)
        {
            var width = (int) Math.Ceiling(Math.Abs(endLine.X - startLine.X));
            var height = (int)Math.Ceiling(Math.Abs(endLine.Y - startLine.Y));
            var up = width > height ? width : height;
            var down = -up;
            var test = new SKRectI(AutoUp(canvas.DeviceClipBounds.Left, down),
                                   AutoUp(canvas.DeviceClipBounds.Top, down),
                                   AutoUp(canvas.DeviceClipBounds.Right, up),
                                   AutoUp(canvas.DeviceClipBounds.Bottom, up));

            return test.Contains((int)(startLine.X + translateDistance.X), (int)(startLine.Y + translateDistance.Y))
                   || test.Contains((int)(endLine.X + translateDistance.X), (int)(endLine.Y + translateDistance.Y));
        }

        private bool InCanvasDrawLine(object obj, SKCanvas canvas)
        {
            clsFamilyMember objMember = (clsFamilyMember)obj;
            bool blnRet = canvas.DeviceClipBounds.Contains(10, (int)(objMember.miPosition.Y + translateDistance.Y + objMember.Height + Config.MemberVerticalSpace));
            //blnRet = blnRet | canvas.DeviceClipBounds.Contains(10, (int)(objMember.miPosition.Y + translateDistance.Y - objMember.miSize.Height/2));
            return blnRet;
        }

        private int AutoUp(int input, int up)
        {
            return (int)(Math.Ceiling((input + up) / flZoomLevel));
        }

        public void DrawTree(SKCanvas canvas)
        {
            //using (SKPaint paint = new SKPaint())
            {

                var dataRaw = (from keyHas in Config.DataMember.Keys.Cast<string>()
                               where InCanvas(Config.DataMember[keyHas], canvas)
                               select (clsFamilyMember)Config.DataMember[keyHas]).ToList();

                var dataRawSpouseLine = (from keyHas in Config.DataMember.Keys.Cast<string>()
                                         //where InCanvasDrawLine(Config.DataMember[keyHas], canvas)
                                         select (clsFamilyMember)Config.DataMember[keyHas]);

                Func<clsConst.ENUM_GENDER, clsFamilyMember> getTemplate = (gender) =>
                {
                    if (gender == clsConst.ENUM_GENDER.Male)
                    {
                        return objMale;
                    }

                    if (gender == clsConst.ENUM_GENDER.FMale)
                    {
                        return objFMale;
                    }

                    return objUnknown;
                };

                dataRaw.ForEach(objMember =>
                {
                    objMember.TemplateType = Config.TemplateMember;

                    var templateMember = getTemplate(objMember.Gender);
                    canvas.DrawPicture(templateMember.miSvgData.Picture, objMember.miPosition);

                    if (flZoomLevel != MinZoomLevel)
                    {
                        if (objMember.ReDraw || objMember.miSvgData.IsNotHasValue())
                        {
                            objMember.FreeSVGData();
                            objMember.DrawingMemberSVG(objMember.ReDraw);
                        }

                        canvas.DrawPicture(objMember.miSvgData.Picture, objMember.miPosition);
                    }
                    else
                    {
                        canvas.DrawPicture(templateMember.miSvgAvatar.Picture, objMember.miPosition);
                    }

                    for (int i = 0; i < objMember.lstChild.Count; i++)
                    {
                        xDrawFatherChildLine(canvas, objMember, (clsFamilyMember)Config.DataMember[objMember.lstChild[i]], true);
                    }

                    xDrawSpouseLine(canvas, objMember);
                    //objMember.FreeSVGData();
                });

                dataRawSpouseLine.Where(i => !dataRaw.Any(j => j.miID == i.miID)).Select(objMember =>
                {
                    objMember.TemplateType = Config.TemplateMember;

                    //xDrawSpouseLine(canvas, objMember);
                    for (int i = 0; i < objMember.lstChild.Count; i++)
                    {
                        xDrawFatherChildLine(canvas, objMember, (clsFamilyMember)Config.DataMember[objMember.lstChild[i]]);
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
            objRoot.miPosition.Y = intMinTop;

            if (objRoot.isNoChild())
            {
                objRoot.miPosition.X = intMinLeft;
                objRoot.miMaxTreeRight = objRoot.miPosition.X + objRoot.Width + Config.MemberHorizonSpace;
                //objRoot.miMinTreeLeft = intMinLeft;
                objRoot.blnPos = true;
                return;
            }

            int i = 0;
            MakeTreeDraw(objRoot.lstChild[i], intMinTop + objRoot.Height + Config.MemberVerticalSpace, intMinLeft);

            for (i = 1; i < objRoot.lstChild.Count; i++)
            {
                clsFamilyMember objPrevBrother = (clsFamilyMember)Config.DataMember[objRoot.lstChild[i - 1]];
                MakeTreeDraw(objRoot.lstChild[i], objPrevBrother.miPosition.Y, objPrevBrother.miMaxTreeRight);
            }

            clsFamilyMember objFirstChild = (clsFamilyMember)Config.DataMember[objRoot.lstChild[0]];
            clsFamilyMember objLastChild = (clsFamilyMember)Config.DataMember[objRoot.lstChild[objRoot.lstChild.Count - 1]];
            objRoot.miPosition.X = (objFirstChild.miPosition.X + objLastChild.miPosition.X
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
        public void UpdateSpousePosition(ref clsFamilyMember objMember,
            clsConst.ENUM_FIRSTSPOUSE_POSITION enSpousePos = clsConst.ENUM_FIRSTSPOUSE_POSITION.RightMember)
        {
            if (objMember == null) return;

            int intSpouseSpace = Config.MemberHorizonSpace / 2;

            int j = 0;
            if (objMember.lstSpouse.Count >= 2)
            {
                //If the first spose is on the left
                if (enSpousePos == clsConst.ENUM_FIRSTSPOUSE_POSITION.LeftMember)
                {
                    clsFamilyMember objSpouse = GetDataMember(objMember.lstSpouse[0]);
                    objSpouse.miPosition.Y = objMember.miPosition.Y;

                    objSpouse.miPosition.X = objMember.miPosition.X - (intSpouseSpace + objSpouse.Width);
                    //objMember.miPosition.X = objMember.miPosition.X - intDelta;
                    //If this spose outside the tree
                    if (objSpouse.miPosition.X < objMember.miMinTreeLeft)
                    {
                        objSpouse.miPosition.X = objMember.miMinTreeLeft;
                        objMember.miPosition.X = objSpouse.miPosition.X + objSpouse.Width + intSpouseSpace;
                    }

                    objMember.miMaxTreeRight = objMember.miPosition.X + objMember.Width + intSpouseSpace;
                    objSpouse.miMinTreeLeft = objSpouse.miPosition.X;
                    objSpouse.miMaxTreeRight = objMember.miMaxTreeRight; //objSpouse.miPosition.X + objSpouse.miSize.Width + intMemberHorizonSpace;

                    //objMember.miMaxTreeRight = objSpouse.miMaxTreeRight;
                    objMember.miMinTreeLeft = objSpouse.miPosition.X;
                    objSpouse.blnPos = true;
                    j = 1;
                }
            }

            int i;

            for (i = j; i < objMember.lstSpouse.Count; i++)
            {
                clsFamilyMember objSpouse = GetDataMember(objMember.lstSpouse[i]);
                objSpouse.miPosition.Y = objMember.miPosition.Y;
                objSpouse.miPosition.X = objMember.miPosition.X + (i + 1 - j) * (intSpouseSpace + objSpouse.Width);
                objSpouse.miMaxTreeRight = objSpouse.miPosition.X + objSpouse.Width + intSpouseSpace;
                objSpouse.blnPos = true;
                objMember.miMaxTreeRight = objSpouse.miMaxTreeRight;
            }

            return;
        }
        private bool IsMemberClicked(clsFamilyMember objMember, float x, float y)
        {
            return objMember.CheckMemberClick(new SKPoint(x - translateCurrentDistance.X, y - translateCurrentDistance.Y));
        }

        private int xParentWidth(clsFamilyMember objFather)
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
        public void MakeTreeDrawWithSpouse(string intRootID, float intMinTop, float intMinLeft, clsConst.ENUM_FIRSTSPOUSE_POSITION enSpousePos = clsConst.ENUM_FIRSTSPOUSE_POSITION.RightMember)
        {
            clsFamilyMember objRoot = GetDataMember(intRootID);
            if (objRoot.blnPos) return;
            objRoot.miPosition.Y = intMinTop;
            objRoot.miMinTreeLeft = intMinLeft;

            int i = 0;

            if (objRoot.isNoChild())
            {
                objRoot.miPosition.X = intMinLeft;
                objRoot.miMaxTreeRight = objRoot.miPosition.X + objRoot.Width + Config.MemberHorizonSpace;
                objRoot.blnPos = true;
                UpdateSpousePosition(ref objRoot, enSpousePos);

                return;
            }

            MakeTreeDrawWithSpouse(objRoot.lstChild[i], intMinTop + objRoot.Height + Config.MemberVerticalSpace, objRoot.miMinTreeLeft, enSpousePos);

            for (i = 1; i < objRoot.lstChild.Count; i++)
            {
                clsFamilyMember objPrevBrother = GetDataMember(objRoot.lstChild[i - 1]);
                if (objPrevBrother == null) continue;
                MakeTreeDrawWithSpouse(objRoot.lstChild[i], objPrevBrother.miPosition.Y, objPrevBrother.miMaxTreeRight, enSpousePos);
            }

            clsFamilyMember objFirstChild = GetDataMember(objRoot.lstChild[0]);
            clsFamilyMember objLastChild = GetDataMember(objRoot.lstChild[objRoot.lstChild.Count - 1]);

            objRoot.miPosition.X = (objFirstChild.miPosition.X + objLastChild.miPosition.X
                + objLastChild.Width) / 2 - objRoot.Width / 2;

            UpdateSpousePosition(ref objRoot, enSpousePos);

            if (objRoot.miMaxTreeRight < objLastChild.miMaxTreeRight)
            {
                objRoot.miMaxTreeRight = objLastChild.miMaxTreeRight;
            }
            objRoot.blnPos = true;
        }

        public void MakeTreeDrawinBox(int intRootID, float intMinTop, float intMinLeft,
            Hashtable hasMember, clsConst.ENUM_FIRSTSPOUSE_POSITION enSpousePos = clsConst.ENUM_FIRSTSPOUSE_POSITION.RightMember)
        {
            clsFamilyMember objRoot = (clsFamilyMember)hasMember[intRootID];
            if (objRoot.blnPos) return;
            objRoot.miPosition.Y = intMinTop;
            objRoot.miMinTreeLeft = intMinLeft;

            int i = 0;

            if (objRoot.isNoChild())
            {
                objRoot.miPosition.X = intMinLeft;
                objRoot.miMaxTreeRight = objRoot.miPosition.X + objRoot.Width + Config.MemberHorizonSpace;
                objRoot.blnPos = true;
                UpdateSpousePosition(ref objRoot, enSpousePos);
                return;
            }
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
        // ~DrawTreeManager()
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