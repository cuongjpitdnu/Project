using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GP40Common;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace GP40Tree
{
    class clsTreeDraw
    {
        SKControl skTree;

        private bool blnMouseDown = false;
        private SKPoint translateDistance;
        private SKPoint translateCurrentDistance;
        private Point pointMouseDown = new Point(0, 0);
        private float intMinX, intMinY, intMaxX, intMaxY;
        private bool blnFit = false;
        //private clsConst.ENUM_MEMBER_TEMPLATE enTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MemberCardTemplFull;

        private float flZoomLevel = 1;
        private clsFamilyMember objSelectedMember;
        private SKColor _backColor = new SKColor(250, 191, 98);
        private SKColor _selectMemberColor = SKColors.RoyalBlue;
        private SKColor _childLineColor = SKColors.Blue;
        private SKColor _spouseLineColor = SKColors.Red;

        public SKColor BackColor
        {
            get { return _backColor; }   // get method
            set
            {
                _backColor = value;
                skTree.Invalidate();
            }   // set method
        }

        public SKColor SelectMemberColor
        {
            get { return _selectMemberColor; }   // get method
            set
            {
                _selectMemberColor = value;
                skTree.Invalidate();
            }   // set method
        }

        public SKColor ChildLineColor
        {
            get { return _childLineColor; }   // get method
            set
            {
                _childLineColor = value;
                skTree.Invalidate();
            }   // set method
        }

        public SKColor SpouseLineColor
        {
            get { return _spouseLineColor; }   // get method
            set
            {
                _spouseLineColor = value;
                skTree.Invalidate();
            }   // set method
        }

        Hashtable hasMember;

        public SKControl TreeDraw   // property
        {
            get { return skTree; }   // get method
            set { skTree = value; }    // set method
        }

        public float ZoomLevel   // property
        {
            get { return flZoomLevel; }   // get method
            set { // set method
                flZoomLevel = value;
                skTree.Invalidate();
            }
        }

        public clsFamilyMember SelectedMember   // property
        {
            get { return objSelectedMember; }   // get method
            set
            {
                // set method
                objSelectedMember = value;
                skTree.Invalidate();
            }
        }

        public Hashtable Family   // property
        {
            get { return hasMember; }   // get method
            set { hasMember = value; }   // set method
        }

        public clsTreeDraw()
        {
            skTree = new SKControl();

            skTree.BackColor =  Color.FromArgb(250, 191, 98) ;//System.Drawing.Color.White;
            skTree.Dock = System.Windows.Forms.DockStyle.Fill;
            skTree.Location = new System.Drawing.Point(0, 0);
            skTree.Name = "skTree";

            skTree.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.skTree_PaintSurface);
            skTree.MouseClick += new System.Windows.Forms.MouseEventHandler(this.skTree_MouseClick);
            skTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.skTree_MouseDown);
            skTree.MouseLeave += new System.EventHandler(this.skTree_MouseLeave);
            skTree.MouseMove += new System.Windows.Forms.MouseEventHandler(this.skTree_MouseMove);
            skTree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.skTree_MouseUp);
            hasMember = new Hashtable();
        }

        public void GetBound()
        {
            intMinX = float.MaxValue;
            intMaxX = float.MinValue;

            intMaxY = float.MinValue;
            intMinY = float.MaxValue;

            foreach (DictionaryEntry entry in hasMember)
            {
                clsFamilyMember objMember = (clsFamilyMember)entry.Value;

                if (intMinX > objMember.miPosition.X) intMinX = objMember.miPosition.X;
                if (intMinY > objMember.miPosition.Y) intMinY = objMember.miPosition.Y;

                if (intMaxX < objMember.miPosition.X + objMember.Width) intMaxX = objMember.miPosition.X + objMember.Width;
                if (intMaxY < objMember.miPosition.Y + objMember.Height) intMaxY = objMember.miPosition.Y + objMember.Height;
            }
        }

        public void SetFit()
        {
            blnFit = true;
            skTree.Invalidate();
        }

        private void SetFit(SKCanvas canvas)
        {
            GetBound();
            SKRectI skCanvasBound = canvas.DeviceClipBounds;
            float flZ = (skCanvasBound.Height)/(intMaxY - intMinY);
            flZ = flZ* (skCanvasBound.Width)/(intMaxX - intMinX);
            flZoomLevel = flZ;
        }

        private bool IsMemberClicked(clsFamilyMember objMember, float x, float y)
        {
            return objMember.CheckMemberClick(new SKPoint(x - translateCurrentDistance.X, y - translateCurrentDistance.Y));
        }

        private void skTree_MouseClick(object sender, MouseEventArgs e)
        {
            if (!blnMouseDown) return;

            var objMember = (from keyHas in hasMember.Keys.Cast<int>()
                          where IsMemberClicked((clsFamilyMember)hasMember[keyHas], e.X / flZoomLevel, e.Y / flZoomLevel)
                          select (clsFamilyMember)hasMember[keyHas]).FirstOrDefault();

            //var objMember = xFindMember(e.X / flZoomLevel, e.Y / flZoomLevel, hasMember);
            if (objMember!=null) SelectedMember = objMember;
        }

        //private clsFamilyMember xFindMember(float x, float y, Hashtable hasMember)
        //{
        //    if (hasMember == null) return null;
        //    //clsFamilyMember objMember = (clsFamilyMember)hasMember[0];
        //    foreach (DictionaryEntry entry in hasMember)
        //    {
        //        clsFamilyMember objMember = (clsFamilyMember)entry.Value;
        //        if (objMember.CheckMemberClick(new SKPoint(x - translateCurrentDistance.X, y - translateCurrentDistance.Y)))
        //        {
        //            return objMember;
        //        }
        //    }
        //    return null;
        //}

        private void skTree_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            //if (!mblnDraw) return;
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Save();
            canvas.Clear(_backColor);

            if (blnFit) SetFit(canvas);
            blnFit = false;

            canvas.Scale(flZoomLevel);
            canvas.Translate(translateDistance);

            DrawTree(canvas);

            if (SelectedMember != null)
            {
                //SKRect skBorder = new SKRect(SelectedMember.miPosition.X, SelectedMember.miPosition.Y,
                //SelectedMember.miPosition.X + SelectedMember.miSize.Width, SelectedMember.miPosition.Y + SelectedMember.miSize.Height);

                SKPaint paint = new SKPaint();
                paint.Color = _selectMemberColor;
                paint.Style = SKPaintStyle.Stroke;
                paint.StrokeWidth = 5;

                canvas.DrawRoundRect(SelectedMember.SelectedBorder, new SKSize(5, 5), paint);

                paint.Dispose();
            }

            canvas.ResetMatrix();
            //if (lblEnd.Text == "") lblEnd.Text = DateTime.Now.ToString();
        }

        private void skTree_MouseDown(object sender, MouseEventArgs e)
        {
            blnMouseDown = true;
            pointMouseDown = new Point(e.X, e.Y);
            // Set cursor as hourglass
            Cursor.Current = Cursors.Hand;
        }

        private void skTree_MouseLeave(object sender, EventArgs e)
        {
            blnMouseDown = false;

            // Set cursor as default arrow
            Cursor.Current = Cursors.Default;
        }

        private void skTree_MouseMove(object sender, MouseEventArgs e)
        {
            if (blnMouseDown)
            {
                translateDistance = new SKPoint(translateCurrentDistance.X + (e.X - pointMouseDown.X)/flZoomLevel, translateCurrentDistance.Y + (e.Y - pointMouseDown.Y)/flZoomLevel);
                //pointMouseDown = new Point(e.X, e.Y);
                SelectedMember = null;
                skTree.Invalidate();
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

        private void xDrawSpouseLine(SKCanvas canvas, clsFamilyMember objMember, clsFamilyMember objSpouse)
        {
            SKPoint objLeftPoint;
            SKPoint objRightPoint;

            SKPaint paint = new SKPaint();
            paint.Color = _spouseLineColor;
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

            clsFamilyMember objSpouse = (clsFamilyMember)hasMember[objMember.lstSpouse[0]];
            xDrawSpouseLine(canvas, objSpouse, objMember);

            for (int i = 1; i < objMember.lstSpouse.Count; i++)
            {
                clsFamilyMember objSpouse1 = (clsFamilyMember)hasMember[objMember.lstSpouse[i - 1]];
                clsFamilyMember objSpouse2 = (clsFamilyMember)hasMember[objMember.lstSpouse[i]];

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

        private void xDrawFatherChildLine(SKCanvas canvas, clsFamilyMember objFather, clsFamilyMember objChild)
        {
            SKPoint objFatherPoint = new SKPoint(objFather.miPosition.X + objFather.Width / 2, objFather.miPosition.Y + objFather.Height);
            SKPoint objFatherMiddlePoint = new SKPoint(objFatherPoint.X, objFatherPoint.Y + clsConst.MemberVerticalSpace / 2);
            SKPoint objChildPoint = new SKPoint(objChild.miPosition.X + objChild.Width / 2, objChild.miPosition.Y);
            SKPoint objChildMiddlePoint = new SKPoint(objChildPoint.X, objChildPoint.Y - clsConst.MemberVerticalSpace / 2);

            SKPaint paint = new SKPaint();
            paint.Color = _childLineColor;
            //paint.TextScaleX = 1.5f;
            paint.IsAntialias = true;
            paint.StrokeWidth = 2;
            paint.Style = SKPaintStyle.Stroke;
            canvas.DrawLine(objFatherPoint, objFatherMiddlePoint, paint);
            canvas.DrawLine(objChildMiddlePoint, objFatherMiddlePoint, paint);
            canvas.DrawLine(objChildMiddlePoint, objChildPoint, paint);
            paint.Dispose();

        }

        public void CenteringMember(int intMember)
        {
            clsFamilyMember objRoot = (clsFamilyMember)hasMember[intMember];

            if (objRoot == null) return;
            //translateDistance = new SKPoint(-translateCurrentDistance.X, -translateCurrentDistance.Y);

            //skTree.Invalidate();
            translateDistance = new SKPoint(-objRoot.miPosition.X + skTree.Width / 2 - objRoot.Width, 0);
            translateCurrentDistance = translateDistance;
            skTree.Invalidate();
        }

        public void CenteringRoot(int intRoot = 0)
        {
            CenteringMember(intRoot);
        }

        private bool InCanvas(object obj, SKCanvas canvas)
        {
            clsFamilyMember objMember = (clsFamilyMember)obj;
            var up = 500;
            var down = -up;
            var test = new SKRectI(AutoUp(canvas.DeviceClipBounds.Left, down),
                                   AutoUp(canvas.DeviceClipBounds.Top, down),
                                   AutoUp(canvas.DeviceClipBounds.Right, up),
                                   AutoUp(canvas.DeviceClipBounds.Bottom, up));

            return test.Contains((int)(objMember.miPosition.X + translateDistance.X), (int)(objMember.miPosition.Y + translateDistance.Y));
        }

        private bool InCanvasDrawLine(object obj, SKCanvas canvas)
        {
            clsFamilyMember objMember = (clsFamilyMember)obj;
            bool blnRet = canvas.DeviceClipBounds.Contains(10, (int)(objMember.miPosition.Y + translateDistance.Y + objMember.Height + clsConst.MemberVerticalSpace));
            //blnRet = blnRet | canvas.DeviceClipBounds.Contains(10, (int)(objMember.miPosition.Y + translateDistance.Y - objMember.miSize.Height/2));
            return blnRet;
        }

        private int AutoUp(int input, int up)
        {
            return (int)(Math.Ceiling((input + up) / flZoomLevel));
        }

        public void DrawTree(SKCanvas canvas)
        {


            SKPaint paint = new SKPaint();

            var dataRaw = from keyHas in hasMember.Keys.Cast<int>()
                          where InCanvas(hasMember[keyHas], canvas)
                          select (clsFamilyMember)hasMember[keyHas];

            var dataRawSpouseLine = from keyHas in hasMember.Keys.Cast<int>()
                          where InCanvasDrawLine(hasMember[keyHas], canvas)
                          select (clsFamilyMember)hasMember[keyHas];

            foreach (var objMember in dataRaw)
            {
                objMember.DrawingMemberSVG();
                canvas.DrawPicture(objMember.miSvgData.Picture, objMember.miPosition, paint);

                for (int i = 0; i < objMember.lstChild.Count; i++)
                {
                    xDrawFatherChildLine(canvas, objMember, (clsFamilyMember)hasMember[objMember.lstChild[i]]);
                }

                xDrawSpouseLine(canvas, objMember);
                objMember.FreeSVGData();
            }

            foreach (var objMember in dataRawSpouseLine)
            {
                //xDrawSpouseLine(canvas, objMember);
                for (int i = 0; i < objMember.lstChild.Count; i++)
                {
                    xDrawFatherChildLine(canvas, objMember, (clsFamilyMember)hasMember[objMember.lstChild[i]]);
                }

                xDrawSpouseLine(canvas, objMember);
            }

            paint.Dispose();
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
            clsFamilyMember objRoot = (clsFamilyMember)hasMember[intRootID];
            if (objRoot.blnPos) return;
            objRoot.miPosition.Y = intMinTop;

            if (objRoot.isNoChild())
            {
                objRoot.miPosition.X = intMinLeft;
                objRoot.miMaxTreeRight = objRoot.miPosition.X + objRoot.Width + clsConst.MemberHorizonSpace;
                //objRoot.miMinTreeLeft = intMinLeft;
                objRoot.blnPos = true;
                return;
            }

            int i = 0;
            MakeTreeDraw(objRoot.lstChild[i],
                intMinTop + objRoot.Height + clsConst.MemberVerticalSpace, intMinLeft);
            for (i = 1; i < objRoot.lstChild.Count; i++)
            {
                clsFamilyMember objPrevBrother = (clsFamilyMember)hasMember[objRoot.lstChild[i - 1]];
                MakeTreeDraw(objRoot.lstChild[i], objPrevBrother.miPosition.Y, objPrevBrother.miMaxTreeRight);
            }

            clsFamilyMember objFirstChild = (clsFamilyMember)hasMember[objRoot.lstChild[0]];
            clsFamilyMember objLastChild = (clsFamilyMember)hasMember[objRoot.lstChild[objRoot.lstChild.Count - 1]];
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

            int intSpouseSpace = clsConst.MemberHorizonSpace / 2;

            int j = 0;
            if (objMember.lstSpouse.Count >= 2)
            {
                //If the first spose is on the left
                if (enSpousePos == clsConst.ENUM_FIRSTSPOUSE_POSITION.LeftMember)
                {
                    clsFamilyMember objSpouse = (clsFamilyMember)hasMember[objMember.lstSpouse[0]];
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
                clsFamilyMember objSpouse = (clsFamilyMember)hasMember[objMember.lstSpouse[i]];
                objSpouse.miPosition.Y = objMember.miPosition.Y;
                objSpouse.miPosition.X = objMember.miPosition.X + (i + 1 - j) * (intSpouseSpace + objSpouse.Width);
                objSpouse.miMaxTreeRight = objSpouse.miPosition.X + objSpouse.Width + intSpouseSpace;
                objSpouse.blnPos = true;
                objMember.miMaxTreeRight = objSpouse.miMaxTreeRight;
            }

            return;
        }

        ///// <summary>
        ///// Make tree draw with spouse, root meaning top of Tree need to be maked
        ///// </summary>
        ///// <param name="intRootID"></param>
        ///// <param name="intMinTop"></param>
        ///// <param name="intMinLeft"></param>
        ///// <param name="hasMember"></param>
        ///// <param name="enSpousePos"></param>
        //public void MakeTreeDrawWithSpouseAlign(int intRootID, float intMinTop, float intMinLeft,
        //    Hashtable hasMember, clsConst.ENUM_FIRSTSPOUSE_POSITION enSpousePos = clsConst.ENUM_FIRSTSPOUSE_POSITION.RightMember)
        //{

        //    //Update position of root member
        //    clsFamilyMember objRoot = (clsFamilyMember)hasMember[intRootID];
        //    if (objRoot.blnPos) return;
        //    objRoot.miPosition.Y = intMinTop;
        //    objRoot.miMinTreeLeft = intMinLeft;

        //    //If root there is no child
        //    if (objRoot.isNoChild())
        //    {
        //        objRoot.miPosition.X = intMinLeft;
        //        objRoot.miMaxTreeRight = objRoot.miPosition.X + objRoot.miSize.Width + clsConst.MemberHorizonSpace;
        //        objRoot.blnPos = true;

        //        //calculate position for the root Spouse
        //        UpdateSpousePosition(ref objRoot, hasMember, enSpousePos);

        //        return;
        //    }

        //    //If root has children
        //    int i = 0;
        //    //Calculate position for the first child
        //    MakeTreeDrawWithSpouseAlign(objRoot.lstChild[i], intMinTop + objRoot.miSize.Height + clsConst.MemberVerticalSpace, objRoot.miMinTreeLeft, hasMember, enSpousePos);

        //    //Calculate position for the next children
        //    for (i = 1; i < objRoot.lstChild.Count; i++)
        //    {
        //        clsFamilyMember objPrevBrother = (clsFamilyMember)hasMember[objRoot.lstChild[i - 1]];
        //        MakeTreeDrawWithSpouseAlign(objRoot.lstChild[i], objPrevBrother.miPosition.Y, objPrevBrother.miMaxTreeRight, hasMember, enSpousePos);
        //    }

        //    clsFamilyMember objFirstChild = (clsFamilyMember)hasMember[objRoot.lstChild[0]];
        //    clsFamilyMember objLastChild = (clsFamilyMember)hasMember[objRoot.lstChild[objRoot.lstChild.Count - 1]];

        //    //Re-calculate position for the root after children position are fixed
        //    //objRoot.miPosition.X = (objFirstChild.miPosition.X + objLastChild.miPosition.X
        //    //    + objLastChild.miSize.Width) / 2 - objRoot.miSize.Width / 2;

        //    objRoot.miPosition.X = objFirstChild.miPosition.X;
        //    int intAlignDelta = (int) (xParentWidth(objRoot) - (objLastChild.miMaxTreeRight - objFirstChild.miPosition.X)) / 2;

        //    objRoot.miMinTreeLeft = objFirstChild.miPosition.X;
        //    //calculate position for the root Spouse
        //    UpdateSpousePosition(ref objRoot, hasMember, enSpousePos);

        //    if (objRoot.miMaxTreeRight < objLastChild.miMaxTreeRight)
        //    {
        //        objRoot.miMaxTreeRight = objLastChild.miMaxTreeRight;
        //    }
        //    objRoot.blnPos = true;
        //    return;
        //}

        private int xParentWidth(clsFamilyMember objFather)
        {
            int intParentCount = objFather.lstSpouse.Count + 1;
            int intWidth = intParentCount * objFather.Width + (intParentCount - 1) * clsConst.MemberHorizonSpace;
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
            clsFamilyMember objRoot = (clsFamilyMember)hasMember[intRootID];
            if (objRoot.blnPos) return;
            objRoot.miPosition.Y = intMinTop;
            objRoot.miMinTreeLeft = intMinLeft;

            int i = 0;

            if (objRoot.isNoChild())
            {
                objRoot.miPosition.X = intMinLeft;
                objRoot.miMaxTreeRight = objRoot.miPosition.X + objRoot.Width + clsConst.MemberHorizonSpace;
                objRoot.blnPos = true;
                UpdateSpousePosition(ref objRoot, enSpousePos);

                return;
            }

            MakeTreeDrawWithSpouse(objRoot.lstChild[i], intMinTop + objRoot.Height + clsConst.MemberVerticalSpace, objRoot.miMinTreeLeft, enSpousePos);

            for (i = 1; i < objRoot.lstChild.Count; i++)
            {
                clsFamilyMember objPrevBrother = (clsFamilyMember)hasMember[objRoot.lstChild[i - 1]];
                MakeTreeDrawWithSpouse(objRoot.lstChild[i], objPrevBrother.miPosition.Y, objPrevBrother.miMaxTreeRight, enSpousePos);
            }

            clsFamilyMember objFirstChild = (clsFamilyMember)hasMember[objRoot.lstChild[0]];
            clsFamilyMember objLastChild = (clsFamilyMember)hasMember[objRoot.lstChild[objRoot.lstChild.Count - 1]];
            objRoot.miPosition.X = (objFirstChild.miPosition.X + objLastChild.miPosition.X
                + objLastChild.Width) / 2 - objRoot.Width / 2;


            UpdateSpousePosition(ref objRoot, enSpousePos);

            if (objRoot.miMaxTreeRight < objLastChild.miMaxTreeRight)
            {
                objRoot.miMaxTreeRight = objLastChild.miMaxTreeRight;
            }
            objRoot.blnPos = true;
            return;
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
                objRoot.miMaxTreeRight = objRoot.miPosition.X + objRoot.Width + clsConst.MemberHorizonSpace;
                objRoot.blnPos = true;
                UpdateSpousePosition(ref objRoot, enSpousePos);
                return;
            }

        }
    }


}
