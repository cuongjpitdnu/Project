using GP40Common;
using GPCommon;
using GPMain.Views.Tree;
using GPModels;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GP40DrawTree
{
    public class DrawListMember : DrawCommon
    {
        private List<clsFamilyMember> _dataDraw = null;
        private List<clsFamilyMember> _dataDrawTemp = null;

        private int _numberColumn = 4;
        private int _padding = 5;
        private string _memberSeletedId;

        public event EventHandler<MouseEventArgs> MemberDoubleClick;
        public event EventHandler<clsFamilyMember> MemberRightClick;

        public DrawListMember(bool useGPU = false)
        {
            CreateTree(useGPU);

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
            _controlTree.MouseWheel += new MouseEventHandler(controlTree_MouseWheel);
            _controlTree.MouseDoubleClick += new MouseEventHandler(controlTree_MouseDoubleClick);
            _controlTree.MouseClick += new MouseEventHandler(controlTree_MouseClick);
        }

        private void controlTree_MouseClick(object sender, MouseEventArgs e)
        {
            var objMember = getMemberClicked(e);
            _memberSeletedId = objMember?.Id + "";

            if (e.Button == MouseButtons.Right && MemberRightClick.HasValue())
            {
                if (objMember.HasValue())
                {
                    MemberRightClick(this, objMember);
                }
            }

            _controlTree.Invalidate();
        }

        #region Event

        private void controlTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            var objMember = getMemberClicked(e);

            if (objMember != null && MemberDoubleClick != null)
            {
                MemberDoubleClick(objMember, e);
            }
        }

        private void controlTree_MouseWheel(object sender, MouseEventArgs e)
        {

            if (!_dataDraw.HasValue())
            {
                return;
            }

            var maxY = (intMaxY + _padding) * flZoomLevel;

            if (maxY < _controlTree.Height)
            {
                return;
            }

            var newTranslate = new SKPoint(_pointTranslate.X, _pointTranslate.Y + (e.Delta / flZoomLevel));

            maxY = -(maxY - _controlTree.Height) / flZoomLevel;
            newTranslate.Y = newTranslate.Y < 0 ? newTranslate.Y : 0;
            newTranslate.Y = maxY > newTranslate.Y ? maxY : newTranslate.Y;

            _pointTranslate = newTranslate;
            _controlTree.Invalidate();
        }

        private void controlTree_PaintSurface(object sender, SKCanvas canvas)
        {
            canvas.Clear(SKColors.Transparent);
            canvas.Scale(flZoomLevel);
            canvas.Translate(_pointTranslate);

            if (!_dataDraw.HasValue())
            {
                return;
            }

            if (_dataDrawTemp != null && _dataDrawTemp.Count > 0)
            {
                _dataDrawTemp.ForEach(member =>
                {
                    member.FreeSVGData();
                    member = null;
                });

                _dataDrawTemp.Clear();

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            _dataDrawTemp = _dataDraw.Where(i => isInCanvas(i, canvas)).ToList();

            _dataDrawTemp.ForEach(member =>
            {
                canvas.DrawPicture(getTemplate(member.Gender).miSvgData.Picture, member.Position);
                canvas.DrawPicture(member.ToPicture(member.ReDraw), member.Position);
                member.ReDraw = false;

                if (member.Id == _memberSeletedId)
                {
                    canvas.DrawRoundRect(member.Position.X, member.Position.Y, member.Size.Width, member.Height, 1, 1, new SKPaint() {
                        IsAntialias = true,
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = 3,
                        Color = Color.FromArgb(192, 192, 192).ToSKColor()
                    });
                }
            });

            canvas.ResetMatrix();
        }

        #endregion Event

        public void Load(List<TMember> data)
        {

            _pointTranslate = new SKPoint(0, 0);
            //_numberColumn = _controlTree.Width / (clsCommon.mcTemplInput.Width + _padding) + 1;
            _dataDraw = getData(data);
            var widthTree = (clsCommon.mcTemplInput.Width + _padding) * _numberColumn + _padding;
            flZoomLevel = _controlTree.Width < widthTree ? ((float)_controlTree.Width / widthTree) : 1f;
            _controlTree.Invalidate();
        }

        private bool isMemberClicked(clsFamilyMember objMember, float x, float y)
        {
            return objMember.CheckMemberClick(new SKPoint(x - _pointTranslate.X, y - _pointTranslate.Y));
        }

        private bool isInCanvas(clsFamilyMember objMember, SKCanvas canvas)
        {
            if (objMember == null)
            {
                return false;
            }

            var up = objMember.Size.Height;
            var down = -up;
            var test = new SKRectI(autoUp(canvas.DeviceClipBounds.Left, down),
                                   autoUp(canvas.DeviceClipBounds.Top, down),
                                   autoUp(canvas.DeviceClipBounds.Right, up),
                                   autoUp(canvas.DeviceClipBounds.Bottom, up));

            return test.Contains((int)(objMember.Position.X + _pointTranslate.X), (int)(objMember.Position.Y + _pointTranslate.Y));
        }

        private int autoUp(int input, int up)
        {
            return (int)(Math.Ceiling((input + up) / flZoomLevel));
        }

        private clsFamilyMember getMemberClicked(MouseEventArgs e)
        {
            return _dataDraw.FirstOrDefault(member => isMemberClicked(member, e.X / flZoomLevel, e.Y / flZoomLevel));
        }

        private List<clsFamilyMember> getData(List<TMember> data)
        {

            if (!data.HasValue())
            {
                return null;
            }

            var intX = _padding;
            var intY = _padding;

            var numberHoz = 0;

            intMinX = 0;
            intMaxX = 0;

            intMaxY = 0;
            intMinY = 0;

            var cnn = 0;

            return data.Select(member =>
            {
                var obj = member.ToDataDraw(clsConst.ENUM_MEMBER_TEMPLATE.MCTInput);

                obj.ReDraw = true;
                obj.FullName += " (" + (++cnn).ToString() + ")";
                obj.Position = new SKPoint(intX, intY);
                intX += obj.Size.Width + _padding;
                numberHoz++;

                if (numberHoz == _numberColumn)
                {
                    numberHoz = 0;
                    intX = _padding;
                    intY += obj.Size.Height + _padding;
                }

                intMinX = intMinX > obj.Position.X ? obj.Position.X : intMinX;
                intMinY = intMinY > obj.Position.Y ? obj.Position.Y : intMinY;

                intMaxX = intMaxX < obj.Position.X + obj.Width ? obj.Position.X + obj.Width : intMaxX;
                intMaxY = intMaxY < obj.Position.Y + obj.Height ? obj.Position.Y + obj.Height : intMaxY;

                return obj;
            }).ToList();
        }
    }
}