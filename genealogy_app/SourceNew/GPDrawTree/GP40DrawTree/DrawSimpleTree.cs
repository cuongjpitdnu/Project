using GP40Common;
using GPCommon;
using GPMain.Core;
using GPMain.Views.Tree.Build;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GP40Common.clsConst;

namespace GP40DrawTree
{
    public class DrawSimpleTree : DrawTreeManager
    {
        public DrawSimpleTree(DrawTreeConfig config, bool useGPU = false)
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

            CreateTree(useGPU);

            ((ISKControl)_controlTree).PaintSurfaceCanvas += new EventHandler<SKCanvas>(skTree_PaintSurface);
            _controlTree.MouseClick += new MouseEventHandler(skTree_MouseClick);
            _controlTree.MouseDoubleClick += new MouseEventHandler(skTree_MouseDoubleClick);
            _controlTree.MouseDown += new MouseEventHandler(skTree_MouseDown);
            _controlTree.MouseLeave += new EventHandler(skTree_MouseLeave);
            _controlTree.MouseMove += new MouseEventHandler(skTree_MouseMove);
            _controlTree.MouseUp += new MouseEventHandler(skTree_MouseUp);
            _controlTree.MouseWheel += new MouseEventHandler(skTree_MouseWheel);
        }
        protected override void xDrawFatherChildLine(SKCanvas canvas, clsFamilyMember objFather, clsFamilyMember objChild, bool notCheck = false, bool drawAllLine = false)
        {
            if (objFather == null)
            {
                return;
            }
            if (objChild == null)
            {
                return;
            }
            if (!objFather.InRootTree) return;
            SKPoint objFatherPoint = new SKPoint(objFather.Position.X + objFather.Width / 2, objFather.Position.Y + objFather.Height);
            SKPoint objFatherMiddlePoint = new SKPoint(objFatherPoint.X, objFatherPoint.Y + Config.MemberVerticalSpace / 2);

            if (DrawToPrint)
            {
                if (objFatherPoint.X >= MaxWidth || objFatherPoint.Y >= MaxHeight)
                {
                    return;
                }
                objFatherMiddlePoint.X = objFatherMiddlePoint.X > MaxWidth ? MaxWidth : objFatherMiddlePoint.X;
                objFatherMiddlePoint.Y = objFatherMiddlePoint.Y > MaxHeight ? MaxHeight : objFatherMiddlePoint.Y;
            }

            if (notCheck)
            {
                canvas.DrawLine(objFatherPoint, objFatherMiddlePoint, PainLine);
            }
            else
            {
                if (InCanvasDrawLine(objFatherPoint, objFatherMiddlePoint, canvas, drawAllLine))
                {
                    canvas.DrawLine(objFatherPoint, objFatherMiddlePoint, PainLine);
                }
            }

            if (objChild == null)
            {
                return;
            }

            SKPoint objChildPoint = new SKPoint(objChild.Position.X + objChild.Width / 2, objChild.Position.Y);
            SKPoint objChildMiddlePoint = new SKPoint(objChildPoint.X, objFatherMiddlePoint.Y);

            float flTempX = objChildMiddlePoint.X;
            if (DrawToPrint)
            {
                objChildMiddlePoint.X = objChildMiddlePoint.X > MaxWidth ? MaxWidth : objChildMiddlePoint.X;
                objChildPoint.Y = objChildPoint.Y > MaxHeight ? MaxHeight : objChildPoint.Y;
            }
            if (notCheck)
            {
                canvas.DrawLine(objChildMiddlePoint, objFatherMiddlePoint, PainLine);
                if (DrawToPrint && flTempX > MaxWidth)
                {
                    return;
                }
                canvas.DrawLine(objChildMiddlePoint, objChildPoint, PainLine);
                return;
            }

            if (InCanvasDrawLine(objChildMiddlePoint, objFatherMiddlePoint, canvas, drawAllLine))
            {
                canvas.DrawLine(objChildMiddlePoint, objFatherMiddlePoint, PainLine);
            }
            if (DrawToPrint && flTempX > MaxWidth)
            {
                return;
            }
            if (InCanvasDrawLine(objChildMiddlePoint, objChildPoint, canvas, drawAllLine))
            {
                canvas.DrawLine(objChildMiddlePoint, objChildPoint, PainLine);
            }
        }


        /// <summary>
        /// Update Spouse Position of a member based on that member position is calculated
        /// </summary>
        /// <param name="objMember"></param>
        /// <param name="hasMember"></param>
        /// <param name="enSpousePos"></param>
        protected override void UpdateSpousePosition(ref clsFamilyMember objMember, clsConst.ENUM_FIRSTSPOUSE_POSITION enSpousePos = clsConst.ENUM_FIRSTSPOUSE_POSITION.RightMember)
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

                    objSpouse.Position.Y = objMember.Position.Y;

                    objSpouse.Position.X = objMember.Position.X - (intSpouseSpace + objSpouse.Width);
                    //If this spose outside the tree
                    if (objSpouse.Position.X < objMember.miMinTreeLeft)
                    {
                        objSpouse.Position.X = objMember.miMinTreeLeft;

                        objMember.Position.X = objSpouse.Position.X + objSpouse.Width + intSpouseSpace;
                    }

                    objMember.miMaxTreeRight = objMember.Position.X + objMember.Width + intSpouseSpace;
                    objSpouse.miMinTreeLeft = objSpouse.Position.X;
                    objSpouse.miMaxTreeRight = objMember.miMaxTreeRight; //objSpouse.miPosition.X + objSpouse.miSize.Width + intMemberHorizonSpace;

                    //objMember.miMaxTreeRight = objSpouse.miMaxTreeRight;
                    objMember.miMinTreeLeft = objSpouse.Position.X;
                    objSpouse.blnPos = true;
                    j = 1;
                }
            }

            int i;
            for (i = j; i < objMember.lstSpouse.Count; i++)
            {
                clsFamilyMember objSpouse = GetDataMember(objMember.lstSpouse[i]);
                objSpouse.Position.Y = objMember.Position.Y;
                objSpouse.Position.X = objMember.Position.X + (i + 1 - j) * (intSpouseSpace + objSpouse.Width);
                objSpouse.miMaxTreeRight = objSpouse.Position.X + objSpouse.Width + intSpouseSpace;
                objSpouse.blnPos = true;
                if (i == objMember.lstSpouse.Count - 1)
                {
                    objSpouse.miMaxTreeRight = objSpouse.miMaxTreeRight + intSpouseSpace;
                }
                objMember.miMaxTreeRight = objSpouse.miMaxTreeRight;
                //objMember.miMaxTreeRight = objSpouse.miMaxTreeRight;
            }

            return;
        }

        /// <summary>
        /// MakeTreeDrawWithSpouse, calculate the position of member in tree include spouses
        /// </summary>
        /// <param name="intRootID"></param>
        /// <param name="intMinTop"></param>
        /// <param name="intMinLeft"></param>
        /// <param name="hasMember"></param>
        /// <param name="enSpousePos"></param>
        protected override void MakeTreeDrawWithSpouse(string intRootID, float intMinTop, float intMinLeft, clsConst.ENUM_FIRSTSPOUSE_POSITION enSpousePos = clsConst.ENUM_FIRSTSPOUSE_POSITION.RightMember)
        {
            clsFamilyMember objRoot = GetDataMember(intRootID);
            if (objRoot == null) return;
            if (objRoot.blnPos) return;
            objRoot.Position.Y = intMinTop;
            objRoot.miMinTreeLeft = intMinLeft;

            int i = 0;

            if (objRoot.isNoChild())
            {
                objRoot.Position.X = intMinLeft;
                objRoot.miMaxTreeRight = objRoot.Position.X + objRoot.Width + Config.MemberHorizonSpace;
                objRoot.blnPos = true;
                UpdateSpousePosition(ref objRoot, enSpousePos);
                return;
            }
            MakeTreeDrawWithSpouse(objRoot.lstChild[i], intMinTop + objRoot.Height + Config.MemberVerticalSpace, objRoot.miMinTreeLeft, enSpousePos);

            for (i = 1; i < objRoot.lstChild.Count; i++)
            {
                clsFamilyMember objPrevBrother = GetDataMember(objRoot.lstChild[i - 1]);
                if (objPrevBrother == null) continue;
                MakeTreeDrawWithSpouse(objRoot.lstChild[i], objPrevBrother.Position.Y, objPrevBrother.miMaxTreeRight, enSpousePos);
            }

            clsFamilyMember objFirstChild = GetDataMember(objRoot.lstChild[0]);
            clsFamilyMember objLastChild = GetDataMember(objRoot.lstChild[objRoot.lstChild.Count - 1]);

            objRoot.Position.X = (objFirstChild.Position.X + objLastChild.Position.X + objLastChild.Width) / 2 - objRoot.Width / 2;

            UpdateSpousePosition(ref objRoot, enSpousePos);

            if (objRoot.miMaxTreeRight < objLastChild.miMaxTreeRight)
            {
                objRoot.miMaxTreeRight = objLastChild.miMaxTreeRight;
            }
            objRoot.blnPos = true;
        }
    }
}