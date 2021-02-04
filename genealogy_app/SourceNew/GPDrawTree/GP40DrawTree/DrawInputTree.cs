using GP40Common;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GP40DrawTree
{
    public class DrawInputTree : DrawCommon
    {

        clsFamilyMember objFather, objMother;
        clsFamilyMember objMemberLeft, objMemberSpouse;

        public DrawInputTree(bool useGPU = false)
        {
            CreateTree(useGPU);

            objFather = new clsFamilyMember();
            objFather.InitMemberInfo(clsConst.ENUM_MEMBER_TEMPLATE.MCTInput, clsConst.ENUM_GENDER.Male);

            objMother = new clsFamilyMember();
            objMother.InitMemberInfo(clsConst.ENUM_MEMBER_TEMPLATE.MCTInput, clsConst.ENUM_GENDER.FMale);

            objMemberLeft = new clsFamilyMember();
            objMemberLeft.InitMemberInfo(clsConst.ENUM_MEMBER_TEMPLATE.MCTInput, clsConst.ENUM_GENDER.Male);

            objMemberSpouse = new clsFamilyMember();
            objMemberSpouse.InitMemberInfo(clsConst.ENUM_MEMBER_TEMPLATE.MCTInput, clsConst.ENUM_GENDER.FMale);

            //objFather.flZoom = 0.8f;
            //objMother.flZoom = 0.8f;

            _memberMale.TemplateType = clsConst.ENUM_MEMBER_TEMPLATE.MCTInput;
            _memberFMale.TemplateType = clsConst.ENUM_MEMBER_TEMPLATE.MCTInput;
            _memberUnknown.TemplateType = clsConst.ENUM_MEMBER_TEMPLATE.MCTInput;

            _memberMale.BorderColor = Color.FromArgb(12, 0, 0, 0).ToSKColor();
            _memberFMale.BorderColor = Color.FromArgb(12, 0, 0, 0).ToSKColor();
            _memberUnknown.BorderColor = Color.FromArgb(12, 0, 0, 0).ToSKColor();

            _memberMale.DrawingMemberSVGFrame(true);
            _memberFMale.DrawingMemberSVGFrame(true);
            _memberUnknown.DrawingMemberSVGFrame(true);

            _memberMale.DrawingMemberSVGAvatarGender(true);
            _memberFMale.DrawingMemberSVGAvatarGender(true);
            _memberUnknown.DrawingMemberSVGAvatarGender(true);

            ((ISKControl)_controlTree).PaintSurfaceCanvas += new EventHandler<SKCanvas>(controlTree_PaintSurface);
        }

        private void controlTree_PaintSurface(object sender, SKCanvas canvas)
        {
            canvas.Clear(SKColors.Transparent);
            
            objFather.DrawingMemberSVG();
            objMother.DrawingMemberSVG();
            objMemberLeft.DrawingMemberSVG();
            objMemberSpouse.DrawingMemberSVG();

            setPosition(canvas);

            canvas.Scale(0.8f);
            canvas.Translate(_pointTranslate);
            canvas.DrawPicture(getTemplate(objFather.Gender).miSvgData.Picture, objFather.Position);
            canvas.DrawPicture(getTemplate(objMother.Gender).miSvgData.Picture, objMother.Position);
            canvas.DrawPicture(objFather.miSvgData.Picture, objFather.Position.X, objFather.Position.Y);
            canvas.DrawPicture(objMother.miSvgData.Picture, objMother.Position.X, objMother.Position.Y);
            canvas.ResetMatrix();

            canvas.Scale(1f);
            canvas.Translate(_pointTranslate);
            canvas.DrawPicture(getTemplate(objMemberLeft.Gender).miSvgData.Picture, objMemberLeft.Position);
            canvas.DrawPicture(getTemplate(objMemberSpouse.Gender).miSvgData.Picture, objMemberSpouse.Position);
            canvas.DrawPicture(objMemberLeft.miSvgData.Picture, objMemberLeft.Position.X, objMemberLeft.Position.Y);
            canvas.DrawPicture(objMemberSpouse.miSvgData.Picture, objMemberSpouse.Position.X, objMemberSpouse.Position.Y);
            canvas.ResetMatrix();
        }

        private void setPosition(SKCanvas canvas)
        {
            SKRectI devRect = canvas.DeviceClipBounds;

            //objFather.miPosition.X = devRect.MidX - (float)objFather.Width;
            //objMother.miPosition.X = devRect.MidX;

            //objMemberLeft.miPosition.X = devRect.MidX - (float)objMemberLeft.Width;
            //objMemberLeft.miPosition.Y = objFather.Height;
            //objMemberSpouse.miPosition.X = devRect.MidX;
            //objMemberSpouse.miPosition.Y = objMother.Height;


            var intHMargin = Convert.ToInt32(devRect.Height * 0.05);
            int intCenterX = devRect.Width / 2;
            
            objMother.Position.X = intCenterX / 0.8f + 5;
            objMother.Position.Y = intHMargin;

            objFather.Position.X = objMother.Position.X - 10 - objFather.Width;
            objFather.Position.Y = intHMargin;

            objMemberLeft.Position.X = devRect.MidX - 5 - objMemberLeft.Width;
            objMemberLeft.Position.Y = objMother.Position.Y + objMother.Height * 0.8f + 5;

            objMemberSpouse.Position.X = devRect.MidX + 5;
            objMemberSpouse.Position.Y = objMother.Position.Y + objMother.Height * 0.8f + 5;
        }
    }
}
