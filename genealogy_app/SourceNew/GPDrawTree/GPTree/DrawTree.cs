using GPCommon;
using GPConst;
using GPModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GPTree
{
    public class DrawTree : IDisposable
    {
        public static Dictionary<string, DrawMember> DrawFromMemberId(string memberId, TreeConfig config = null)
        {
            if (string.IsNullOrEmpty(memberId))
            {
                return null;
            }

            using (var tree = new DrawTree(config))
            {
                return tree.drawFromMemberId(memberId);
            }
        }

        public TreeConfig Config { get; set; }

        private Dictionary<string, DrawMember> dataTree;

        private DrawTree(TreeConfig config)
        {
            dataTree = new Dictionary<string, DrawMember>();
            Config = config ?? new TreeConfig();

            if (!Config.DataMember.HasValue())
            {
                Config.DataMember = new Dictionary<string, TMember>();
            }
        }

        private Dictionary<string, DrawMember> drawFromMemberId(string memberId)
        {
            ObjectHelper.FreeMemory(ref dataTree);
            dataTree = new Dictionary<string, DrawMember>();

            drawFromMemberId(memberId, Config.MinTop, Config.MinLeft);

            return dataTree;
        }

        private void drawFromMemberId(string memberId, float minTop, float minLeft)
        {
            var drawRootId = createDrawMember(memberId);

            if (!drawRootId.HasValue() || drawRootId.HasPosition)
            {
                return;
            }

            drawRootId.SetPositionY(minTop);
            drawRootId.MinTreeLeft = minLeft;

            if (!drawRootId.HasChild())
            {
                drawRootId.SetPositionX(minLeft);
                drawRootId.HasPosition = true;
                drawRootId.MinTreeLeft = drawRootId.Position.X + drawRootId.Size.Width + Config.MemberHorizonSpace;
                updateSpousePosition(memberId);
                //updateSpousePosition(ref drawRootId, enSpousePos);

                return;
            }

            drawRootId.MemberInfo.ListCHILDREN.ForEach(childId => drawFromMemberId(childId, minTop, minLeft));

            var firstChild = getDrawMember(drawRootId.MemberInfo.ListCHILDREN.First());
            var lastChild = getDrawMember(drawRootId.MemberInfo.ListCHILDREN.Last());

            drawRootId.SetPositionX((firstChild.Position.X + lastChild.Position.X + lastChild.Size.Width) / 2 - drawRootId.Size.Width / 2);
            updateSpousePosition(memberId);
            //updateSpousePosition(ref objRoot, enSpousePos);
            drawRootId.MaxTreeRight = drawRootId.MaxTreeRight < lastChild.MaxTreeRight ? lastChild.MaxTreeRight : drawRootId.MaxTreeRight;
            drawRootId.HasPosition = true;
        }

        private void updateSpousePosition(string memberId)
        {
            var drawMember = getDrawMember(memberId);

            if (drawMember == null || !drawMember.MemberInfo.ListSPOUSE.HasValue())
            {
                return;
            }

            var listSpouse = drawMember.MemberInfo.ListSPOUSE;
            int intSpouseSpace = Config.MemberHorizonSpace / 2;
            int j = 0;

            //if (objMember.lstSpouse.Count >= 2)
            //{
            //    //If the first spose is on the left
            //    if (enSpousePos == clsConst.ENUM_FIRSTSPOUSE_POSITION.LeftMember)
            //    {
            //        clsFamilyMember objSpouse = GetDataMember(objMember.lstSpouse[0]);
            //        objSpouse.miPosition.Y = objMember.miPosition.Y;

            //        objSpouse.miPosition.X = objMember.miPosition.X - (intSpouseSpace + objSpouse.Width);
            //        //objMember.miPosition.X = objMember.miPosition.X - intDelta;
            //        //If this spose outside the tree
            //        if (objSpouse.miPosition.X < objMember.miMinTreeLeft)
            //        {
            //            objSpouse.miPosition.X = objMember.miMinTreeLeft;
            //            objMember.miPosition.X = objSpouse.miPosition.X + objSpouse.Width + intSpouseSpace;
            //        }

            //        objMember.miMaxTreeRight = objMember.miPosition.X + objMember.Width + intSpouseSpace;
            //        objSpouse.miMinTreeLeft = objSpouse.miPosition.X;
            //        objSpouse.miMaxTreeRight = objMember.miMaxTreeRight; //objSpouse.miPosition.X + objSpouse.miSize.Width + intMemberHorizonSpace;

            //        //objMember.miMaxTreeRight = objSpouse.miMaxTreeRight;
            //        objMember.miMinTreeLeft = objSpouse.miPosition.X;
            //        objSpouse.blnPos = true;
            //        j = 1;
            //    }
            //}

            for (var i = j; i < listSpouse.Count; i++)
            {
                var drawSpouse = createDrawMember(listSpouse[i]);
                drawSpouse.SetPositionX(drawMember.Position.X + (i + 1 - j) * (intSpouseSpace + drawSpouse.Size.Width));
                drawSpouse.SetPositionY(drawMember.Position.Y);
                drawSpouse.MaxTreeRight = drawSpouse.Position.X + drawSpouse.Size.Width + intSpouseSpace;
                drawSpouse.HasPosition = true;

                drawMember.MaxTreeRight = drawSpouse.MaxTreeRight;
            }

        }

        private DrawMember getDrawMember(string memberId)
        {
            if (string.IsNullOrEmpty(memberId) || !dataTree.ContainsKey(memberId))
            {
                return null;
            }

            return dataTree[memberId];
        }

        private DrawMember createDrawMember(string memberId)
        {
            if (string.IsNullOrEmpty(memberId))
            {
                return null;
            }

            if (dataTree.ContainsKey(memberId))
            {
                return dataTree[memberId];
            }

            var drawMember = Config.DataMember.ContainsKey(memberId) ? new DrawMember(Config.DataMember[memberId]) : null;

            if (drawMember != null)
            {
                drawMember.ResetDataDraw();
                drawMember.ReDraw = true;
                drawMember.TextColor = Config.TextColor;
                drawMember.BorderColor = Config.BorderColor;
                drawMember.BackColor = drawMember.MemberInfo.Gender == (int)EmGender.FeMale ? Config.FeMaleBackColor : Config.MaleBackColor;
                //drawMember.TemplateType = Config.TemplateMember;
                //drawMember.FrameImagePath = clsConst.FramePath + Config.NumberFrame.ToString().PadLeft(2, '0') + ".png";

                dataTree.Add(memberId, drawMember);
                return dataTree[memberId];
            }

            return drawMember;
        }

        #region Disposable

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    ObjectHelper.FreeMemory(ref dataTree);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                Config = null;
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DrawTree()
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

        #endregion Disposable
    }
}