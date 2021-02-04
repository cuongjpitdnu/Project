using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GP40Common;
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace GP40Tree
{
    class clsFInputTree
    {
        clsFamilyMember objFather, objMother;
        clsFamilyMember objMemberLeft, objMemberSpouse;
        List<clsFamilyMember> objChild;
        clsFamilyMember objSelectedMember;

        SKControl skTree;

        int intHMargin;
        int intMainMemberHeight;
        int intOtherMemberHeight;
        float flZoom = 0.8f;

        public SKControl InputTree   // property
        {
            get { return skTree; }   // get method
            set { skTree = value; }    // set method
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

        public clsFInputTree()
        {
            skTree = new SKControl();
            this.skTree.BackColor =System.Drawing.Color.White;
            this.skTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skTree.Location = new System.Drawing.Point(0, 0);
            this.skTree.Name = "skTree";

            skTree.PaintSurface += new System.EventHandler<SkiaSharp.Views.Desktop.SKPaintSurfaceEventArgs>(this.skTree_PaintSurface);
            skTree.Resize += new System.EventHandler(this.skTree_Resize);
            skTree.MouseClick += new System.Windows.Forms.MouseEventHandler(this.skTree_MouseClick);
            skTree.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.skTree_MouseDoubleClick);

            objFather = new clsFamilyMember();           
            objFather.InitMemberInfo(clsConst.ENUM_MEMBER_TEMPLATE.MCTInput, clsConst.ENUM_GENDER.Male);

            objMother = new clsFamilyMember();            
            objMother.InitMemberInfo(clsConst.ENUM_MEMBER_TEMPLATE.MCTInput, clsConst.ENUM_GENDER.FMale);

            objMemberLeft = new clsFamilyMember();          
            objMemberLeft.InitMemberInfo(clsConst.ENUM_MEMBER_TEMPLATE.MCTInput, clsConst.ENUM_GENDER.Male);

            objMemberSpouse = new clsFamilyMember();           
            objMemberSpouse.InitMemberInfo(clsConst.ENUM_MEMBER_TEMPLATE.MCTInput, clsConst.ENUM_GENDER.FMale);

            objMemberLeft.DrawingInputMember();
            objFather.flZoom = 0.8f;
            objFather.DrawingInputMember();
            objMother.flZoom = 0.8f;
            objMother.DrawingInputMember();
            objMemberSpouse.DrawingInputMember();            
        }      

        private void skTree_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKCanvas canvas = e.Surface.Canvas;
            DrawInputTree(canvas);
        }

        private void skTree_Resize(object sender, System.EventArgs e)
        {
            
        }

        private void xSetPosition(SKCanvas canvas)
        {
            SKRectI devRect = canvas.DeviceClipBounds;

            intHMargin = Convert.ToInt32(devRect.Height * 0.05);
            int intCenterX = devRect.Width / 2;
            objFather.miPosition.X = intCenterX - 5 - objFather.Width;
            objFather.miPosition.Y = intHMargin;

            objMother.miPosition.X = intCenterX + 5;
            objMother.miPosition.Y = intHMargin;

            objMemberLeft.miPosition.X = intCenterX - 5 - objMemberLeft.Width;
            objMemberLeft.miPosition.Y = 2 * intHMargin + objFather.Height;

            objMemberSpouse.miPosition.X = intCenterX + 5;
            objMemberSpouse.miPosition.Y = 2 * intHMargin + objFather.Height;

        }

        public void DrawInputTree(SKCanvas canvas)
        {
            canvas.Clear();
            
            xSetPosition(canvas);

            //SKRectI devRect = canvas.DeviceClipBounds;
            //int intCenterX = devRect.Width / 2;
            //intHMargin = Convert.ToInt32(devRect.Height* 0.05);
            //intMainMemberHeight = Convert.ToInt32(devRect.Height * 0.3);
            //intOtherMemberHeight = Convert.ToInt32(devRect.Height * 0.2);

            SKPaint paint = new SKPaint();

            objFather.InfoSurface.Draw(canvas, objFather.miPosition.X, objFather.miPosition.Y, paint);
            objMother.InfoSurface.Draw(canvas, objMother.miPosition.X, objMother.miPosition.Y, paint);

            objMemberLeft.InfoSurface.Draw(canvas, objMemberLeft.miPosition.X, objMemberLeft.miPosition.Y, paint);
            objMemberSpouse.InfoSurface.Draw(canvas, objMemberSpouse.miPosition.X, objMemberSpouse.miPosition.Y, paint);


            if (SelectedMember != null)
            {
                //SKRect skBorder = new SKRect(SelectedMember.miPosition.X, SelectedMember.miPosition.Y,
                //SelectedMember.miPosition.X + SelectedMember.miSize.Width, SelectedMember.miPosition.Y + SelectedMember.miSize.Height);

                SKPaint paintS = new SKPaint();
                paintS.Color = SKColors.Red;
                paintS.Style = SKPaintStyle.Stroke;
                paintS.StrokeWidth = 5;

                canvas.DrawRoundRect(SelectedMember.SelectedBorder, new SKSize(5, 5), paintS);

                paintS.Dispose();
            }

            paint.Dispose();
        }

        private void skTree_MouseClick(object sender, MouseEventArgs e)
        {
            xFindSlectMember(e.X, e.Y);
        }

        private void skTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            xFindSlectMember(e.X, e.Y);
            frmMemberInfo frmInput = new frmMemberInfo();
            
            frmInput.Show();
        }

        private void xFindSlectMember(int x, int y)
        {
            if (objFather.CheckMemberClick(new SKPoint(x, y)))
            {
                SelectedMember = objFather;
                return;
            }

            if (objMother.CheckMemberClick(new SKPoint(x, y)))
            {
                SelectedMember = objMother;
                return;
            }

            if (objMemberLeft.CheckMemberClick(new SKPoint(x, y)))
            {
                SelectedMember = objMemberLeft;
                return;
            }

            if (objMemberSpouse.CheckMemberClick(new SKPoint(x, y)))
            {
                SelectedMember = objMemberSpouse;
                return;
            }
        }

    }
}
