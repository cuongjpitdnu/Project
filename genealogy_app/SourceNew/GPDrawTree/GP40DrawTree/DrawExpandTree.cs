using GP40Common;
using GPCommon;
using GPMain.Common.Helper;
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
    public class DrawTreeExpand : DrawTreeManager
    {
        public DrawTreeExpand(DrawTreeConfig config, bool useGPU = false)
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

            bool fatherInClan = objFather.InRootTree;

            SKPoint objFatherPoint = new SKPoint(objFather.Position.X + objFather.Width / 2, objFather.Position.Y + objFather.Height);
            SKPoint objFatherMiddlePoint = new SKPoint(objFatherPoint.X, objFatherPoint.Y + Config.MemberVerticalSpace / 2);

            var lstParent = objChild.lstParent;
            if (lstParent == null || lstParent.Count == 0)
            {
                return;
            }

            if (lstParent.Count > 1)
            {
                if (fatherInClan)
                {
                    return;
                }
                clsFamilyMember objMother = GetDataMember(lstParent.FirstOrDefault(x => x != objFather.Id));
                if (objMother.Position.X > objFather.Position.X)
                {
                    float pTemp = objFather.Position.X + objFather.Width + Config.MemberHorizonSpace / 2;
                    objFatherPoint = new SKPoint(pTemp, objMother.Position.Y + objMother.Height / 2);
                    objFatherMiddlePoint = new SKPoint(pTemp, objFatherPoint.Y + objMother.Height / 2 + Config.MemberVerticalSpace / 2);
                    //var objTemp = GetDataMember(objMother.lstSpouse[1]);
                    //float pTemp = (objMother.Position.X + objFather.Position.X + objFather.Width) / 2;
                    //if (objTemp.minLeftInFamily > 0)
                    //{
                    //    pTemp = pTemp >= objTemp.minLeftInFamily ? objTemp.minLeftInFamily : pTemp;
                    //}
                    //if (objMother.minLeftInFamily > 0)
                    //{
                    //    pTemp = pTemp > objMother.minLeftInFamily ? objMother.minLeftInFamily : pTemp;
                    //}
                    //objFatherPoint = new SKPoint(pTemp, objFather.Position.Y + objFather.Height / 2);
                    //objFatherMiddlePoint = new SKPoint(pTemp, objFatherPoint.Y + objFather.Height / 2 + Config.MemberVerticalSpace / 2);
                }
                else
                {
                    float pTemp = objFather.Position.X - Config.MemberHorizonSpace / 2;
                    objFatherPoint = new SKPoint(pTemp, objMother.Position.Y + objMother.Height / 2);
                    objFatherMiddlePoint = new SKPoint(pTemp, objFatherPoint.Y + objMother.Height / 2 + Config.MemberVerticalSpace / 2);
                    #region Code Hide
                    //var lstChild = objMother.lstChild;
                    //int idxSpouse = objMother.lstSpouse.IndexOf(objFather.Id);
                    //if (idxSpouse == 0)
                    //{
                    //    float pTemp = (objMother.Position.X + objFather.Position.X + objFather.Width) / 2;
                    //    objFatherPoint = new SKPoint(pTemp, objFather.Position.Y + objFather.Height / 2);
                    //    objFatherMiddlePoint = new SKPoint(pTemp, objFatherPoint.Y + objFather.Height / 2 + Config.MemberVerticalSpace / 2);
                    //}
                    //else if (idxSpouse == 1)
                    //{
                    //    var objTemp = GetDataMember(objMother.lstSpouse[idxSpouse - 1]);
                    //    float pTemp = (objMother.Position.X + objFather.Position.X + objFather.Width) / 2;
                    //    if (objTemp.maxRightInFamily > 0)
                    //    {
                    //        pTemp = pTemp <= objTemp.maxRightInFamily ? objTemp.maxRightInFamily : pTemp;
                    //    }
                    //    if (objMother.maxRightInFamily > 0)
                    //    {
                    //        pTemp = pTemp <= objMother.maxRightInFamily ? objMother.maxRightInFamily : pTemp;
                    //    }
                    //    objFatherPoint = new SKPoint(pTemp, objFather.Position.Y + objFather.Height / 2);
                    //    objFatherMiddlePoint = new SKPoint(pTemp, objFatherPoint.Y + objFather.Height / 2 + Config.MemberVerticalSpace / 2);
                    //}
                    //else
                    //{
                    //    var objTemp = GetDataMember(objMother.lstSpouse[idxSpouse - 1]);
                    //    float pTemp = (objTemp.Position.X + objFather.Position.X + objFather.Width) / 2;
                    //    if (objTemp.maxRightInFamily > 0)
                    //    {
                    //        pTemp = pTemp <= objTemp.maxRightInFamily ? objTemp.maxRightInFamily : pTemp;
                    //    }
                    //    objFatherPoint = new SKPoint(pTemp, objFather.Position.Y + objFather.Height / 2);
                    //    objFatherMiddlePoint = new SKPoint(pTemp, objFatherPoint.Y + objFather.Height / 2 + Config.MemberVerticalSpace / 2);
                    //}
                    #endregion
                }
            }
            else
            {
                objFatherPoint = new SKPoint(objFather.Position.X + objFather.Width / 2, objFather.Position.Y + objFather.Height);
                if (objFather.lstSpouse == null || objFather.lstSpouse.Count == 0)
                {
                    objFatherMiddlePoint = new SKPoint(objFatherPoint.X, objFatherPoint.Y + Config.MemberVerticalSpace / 2);
                }
                else
                {
                    objFatherMiddlePoint = new SKPoint(objFatherPoint.X, objFatherPoint.Y + Config.MemberVerticalSpace / 4);
                }
            }
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
            int intSpouseSpace = Config.MemberHorizonSpace;
            //List<string> lstHasMother = new List<string>();
            int j = 0;
            if (objMember.lstSpouse.Count >= 2)
            {
                //If the first spose is on the left
                if (enSpousePos == clsConst.ENUM_FIRSTSPOUSE_POSITION.LeftMember)
                {
                    clsFamilyMember objSpouse = GetDataMember(objMember.lstSpouse[0]);
                    objSpouse.Position.Y = objMember.Position.Y;
                    var lstChild = objSpouse.lstChild;
                    if (lstChild.Count > 0)
                    {
                        //lstHasMother.AddRange(objSpouse.lstChild);
                        clsFamilyMember firstChild = GetDataMember(lstChild[0]);
                        clsFamilyMember lastChild = firstChild;
                        if (lstChild.Count > 1)
                        {
                            lastChild = GetDataMember(lstChild[lstChild.Count - 1]);
                        }
                        if (firstChild == null || lastChild == null)
                        {
                            objSpouse.Position.X = objMember.Position.X;
                            objMember.Position.X = objSpouse.Position.X + objSpouse.Width + Config.MemberHorizonSpace;
                        }
                        else
                        {
                            objSpouse.Position.X = (firstChild.Position.X + lastChild.Position.X + firstChild.Width) / 2 - objSpouse.Width / 2;
                            objMember.Position.X = (objSpouse.Position.X + objSpouse.Width + intSpouseSpace + 1) >= objMember.Position.X ? (objSpouse.Position.X + objSpouse.Width + intSpouseSpace) : objMember.Position.X;
                            //objSpouse.minLeftInFamily = firstChild.Position.X + firstChild.Width / 2 - 10;
                            //objSpouse.maxRightInFamily = lastChild.Position.X + lastChild.Width / 2 + 10;
                        }
                    }
                    else
                    {
                        objSpouse.Position.X = objMember.Position.X - (intSpouseSpace + objSpouse.Width);
                        //objSpouse.minLeftInFamily = objSpouse.maxRightInFamily = 0;
                    }
                    objMember.miMaxTreeRight = objMember.Position.X + objMember.Width + intSpouseSpace;
                    objSpouse.miMinTreeLeft = objSpouse.Position.X;
                    objSpouse.miMaxTreeRight = objMember.miMaxTreeRight;
                    objMember.miMinTreeLeft = objSpouse.Position.X;
                    objSpouse.blnPos = true;
                    j = 1;
                }
            }
            int i;
            clsFamilyMember temp = objMember;
            for (i = j; i < objMember.lstSpouse.Count; i++)
            {
                clsFamilyMember objSpouse = GetDataMember(objMember.lstSpouse[i]);
                float spousePosition = 0;
                var lstChild = objSpouse.lstChild;
                if (!(lstChild == null || lstChild.Count == 0) && j == 1)
                {
                    //lstHasMother.AddRange(objSpouse.lstChild);
                    clsFamilyMember firstChild = GetDataMember(lstChild[0]);
                    clsFamilyMember lastChild = firstChild;
                    if (lstChild.Count > 1)
                    {
                        lastChild = GetDataMember(lstChild[lstChild.Count - 1]);
                    }
                    if (firstChild != null && lastChild != null)
                    {
                        spousePosition = (firstChild.Position.X + lastChild.Position.X + lastChild.Width) / 2 - objSpouse.Width / 2;
                        //objSpouse.minLeftInFamily = firstChild.Position.X + firstChild.Width / 2 - 10;
                        //objSpouse.maxRightInFamily = lastChild.Position.X + lastChild.Width / 2 + 10;
                    }
                }
                objSpouse.Position.Y = objMember.Position.Y;
                if (spousePosition > 0)
                {
                    spousePosition = spousePosition <= temp.Position.X + temp.Width + intSpouseSpace + 1 ? temp.Position.X + temp.Width + intSpouseSpace : spousePosition;
                    objSpouse.Position.X = spousePosition;
                }
                else
                {
                    objSpouse.Position.X = temp.Position.X + temp.Width + intSpouseSpace;
                    //objSpouse.minLeftInFamily = objSpouse.maxRightInFamily = 0;
                }

                objSpouse.miMaxTreeRight = objSpouse.Position.X + objSpouse.Width + intSpouseSpace;
                objSpouse.blnPos = true;
                objMember.miMaxTreeRight = objSpouse.miMaxTreeRight;

                temp = objSpouse;
            }
            //var lstChildHasOneParent = objMember.lstChild.Where(x => !lstHasMother.Contains(x)).ToList();
            //if (lstChildHasOneParent.Count > 0)
            //{
            //    clsFamilyMember firstChild = GetDataMember(lstChildHasOneParent[0]);
            //    clsFamilyMember lastChild = firstChild;
            //    if (lstChildHasOneParent.Count > 1)
            //    {
            //        lastChild = GetDataMember(lstChildHasOneParent[lstChildHasOneParent.Count - 1]);
            //    }
            //    objMember.minLeftInFamily = firstChild.Position.X + firstChild.Width / 2 - 10;
            //    objMember.maxRightInFamily = lastChild.Position.X + lastChild.Width / 2 + 10;
            //}
            //else
            //{
            //    objMember.minLeftInFamily = objMember.maxRightInFamily = 0;
            //}
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
            using (var memberHelper = new MemberHelper())
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
                objRoot.lstChild = memberHelper.UpdateListChildren(intRootID);
                if (objRoot.lstChild.Count == 1 && objRoot.lstSpouse != null && objRoot.lstSpouse.Count == 1)
                {
                    var clsChild = GetDataMember(objRoot.lstChild[0]);
                    if (objRoot.miMinTreeLeft > minLeft && (clsChild.lstSpouse == null || clsChild.lstSpouse.Count == 0))
                    {
                        MakeTreeDrawWithSpouse(objRoot.lstChild[0], intMinTop + objRoot.Height + Config.MemberVerticalSpace, objRoot.miMinTreeLeft + objRoot.Width / 2 + Config.MemberHorizonSpace / 2, enSpousePos);
                    }
                    else
                    {
                        MakeTreeDrawWithSpouse(objRoot.lstChild[0], intMinTop + objRoot.Height + Config.MemberVerticalSpace, objRoot.miMinTreeLeft, enSpousePos);
                    }
                }
                else
                {
                    MakeTreeDrawWithSpouse(objRoot.lstChild[0], intMinTop + objRoot.Height + Config.MemberVerticalSpace, objRoot.miMinTreeLeft, enSpousePos);
                }

                for (i = 1; i < objRoot.lstChild.Count; i++)
                {
                    clsFamilyMember objPrevBrother = GetDataMember(objRoot.lstChild[i - 1]);
                    if (objPrevBrother == null) continue;
                    MakeTreeDrawWithSpouse(objRoot.lstChild[i], objPrevBrother.Position.Y, objPrevBrother.miMaxTreeRight, enSpousePos);
                }
                /******************************Tính tạo độ cha theo tọa độ các con*********************************/
                clsFamilyMember objFirstChild = GetDataMember(objRoot.lstChild[0]);
                clsFamilyMember objLastChild = objFirstChild;
                if (objRoot.lstChild.Count > 1)
                {
                    objLastChild = GetDataMember(objRoot.lstChild[objRoot.lstChild.Count - 1]);
                }
                if (objRoot.lstSpouse == null || objRoot.lstSpouse.Count == 0)//Cha không có vợ hoặc có hơn 1 vợ
                {
                    clsFamilyMember objTemp1 = objFirstChild;
                    if (!(objFirstChild.lstSpouse == null || objFirstChild.lstSpouse.Count <= 1))
                    {
                        objTemp1 = GetDataMember(objFirstChild.lstSpouse[0]);
                    }
                    clsFamilyMember objTemp2 = objLastChild;
                    if (!(objLastChild.lstSpouse == null || objLastChild.lstSpouse.Count == 0))
                    {
                        objTemp2 = GetDataMember(objLastChild.lstSpouse[objLastChild.lstSpouse.Count - 1]);
                    }
                    objRoot.Position.X = (objTemp1.Position.X + objTemp2.Position.X + objTemp2.Width) / 2 - objRoot.Width / 2;
                }
                else if (objRoot.lstSpouse.Count > 1)
                {
                    objRoot.Position.X = (objFirstChild.Position.X + objLastChild.Position.X + objLastChild.Width) / 2 - objRoot.Width / 2;
                }
                else//Cha có 1 vợ
                {
                    clsFamilyMember objTemp1 = objFirstChild;
                    if (!(objFirstChild.lstSpouse == null || objFirstChild.lstSpouse.Count <= 1))
                    {
                        objTemp1 = GetDataMember(objFirstChild.lstSpouse[0]);
                    }
                    clsFamilyMember objTemp2 = objLastChild;
                    if (!(objLastChild.lstSpouse == null || objLastChild.lstSpouse.Count == 0))
                    {
                        objTemp2 = GetDataMember(objLastChild.lstSpouse[objLastChild.lstSpouse.Count - 1]);
                    }
                    float objRoot_XAxisPos = (objTemp1.Position.X + objTemp2.Position.X + objTemp2.Width) / 2 - (objTemp1.Width + Config.MemberHorizonSpace / 2);//Tọa độ trục X của chồng/cha
                    objRoot_XAxisPos = objRoot_XAxisPos < intMinLeft ? intMinLeft : objRoot_XAxisPos;
                    objRoot.Position.X = objRoot_XAxisPos;
                    //var lstParent = objRoot.lstParent;
                    //if (lstParent == null || lstParent.Count == 0)
                    //{
                    //    objRoot.Position.X = objRoot_XAxisPos;
                    //}
                    //else
                    //{
                    //    foreach (string parent in lstParent)
                    //    {
                    //        var objParent = GetDataMember(parent);
                    //        if (objParent == null) continue;
                    //        if (!objParent.InRootTree) continue;
                    //        var lstTemp = memberHelper.UpdateListChildren(objParent.Id);
                    //        int indexTemp = lstTemp.IndexOf(objRoot.Id);
                    //        if (indexTemp == 0)
                    //        {
                    //            objRoot.Position.X = objRoot_XAxisPos;
                    //            break;
                    //        }
                    //        else if (indexTemp > 0)
                    //        {
                    //            var objPrevBrother = GetDataMember(lstTemp[indexTemp - 1]);
                    //            objRoot_XAxisPos = objRoot_XAxisPos < objPrevBrother.miMaxTreeRight ? objPrevBrother.miMaxTreeRight : objRoot_XAxisPos;
                    //            objRoot.Position.X = objRoot_XAxisPos;
                    //            break;
                    //        }
                    //        else
                    //        {
                    //            objRoot.Position.X = objRoot_XAxisPos;
                    //            break;
                    //        }
                    //    }
                    //}
                }

                /****************************************END********************************************/
                /*Tính tọa độ của vợ và tính lại tọa độ của chồng trong trường hợp có vợ nằm ở bên trái*/
                UpdateSpousePosition(ref objRoot, enSpousePos);
                /****************************************END********************************************/
                if (objRoot.miMaxTreeRight < objLastChild.miMaxTreeRight)
                {
                    objRoot.miMaxTreeRight = objLastChild.miMaxTreeRight;
                }
                objRoot.blnPos = true;
            }
        }
    }
}