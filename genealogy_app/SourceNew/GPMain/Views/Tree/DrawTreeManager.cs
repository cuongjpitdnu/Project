//using GPCommon;
//using GPMain.Common.Interface;
//using GPModels;
//using GPTree;
//using SkiaSharp;
//using SkiaSharp.Views.Desktop;
//using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Windows.Forms;

//namespace GPMain.Views.Tree
//{
//    public class DrawTreeManager : IDisposable
//    {
//        private const float MinZoomLevel = 0.2f;
//        private const float MaxZoomLevel = 2f;
//        private const float ZoomLevel = 0.05f;

//        private string rootId;
//        private float flZoomLevel = 1f;
//        private bool blnMouseDown = false;
//        private SKPoint translateDistance = new SKPoint(0, 0);
//        private SKPoint translateCurrentDistance = new SKPoint(0, 0);
//        private Point pointMouseDown = new Point(0, 0);
//        private Dictionary<string, DrawMember> dataTree;

//        public TreeConfig Config { get; set; }
//        public Control Control { get; set; }

//        public DrawTreeManager(TreeConfig config, string rootId)
//        {
//            Config = config ?? new TreeConfig();
//            Control = new Controls.SKControl();

//            this.rootId = rootId;

//            ((ISKControl)Control).PaintSurfaceCanvas += new EventHandler<SKCanvas>(PaintSurfaceCanvas);

//            //Control.MouseClick += new MouseEventHandler(skTree_MouseClick);
//            //Control.MouseDoubleClick += new MouseEventHandler(skTree_MouseDoubleClick);
//            Control.MouseDown += new MouseEventHandler(skTree_MouseDown);
//            Control.MouseLeave += new EventHandler(skTree_MouseLeave);
//            Control.MouseMove += new MouseEventHandler(skTree_MouseMove);
//            Control.MouseUp += new MouseEventHandler(skTree_MouseUp);
//            //Control.MouseWheel += new MouseEventHandler(skTree_MouseWheel);
//        }

//        public void Draw()
//        {
//            dataTree = DrawTree.DrawFromMemberId(this.rootId, Config);
//            Control.Invalidate();
//        }

//        public void CenteringMember(string memberId)
//        {
//            var objMember = GetDataMember(memberId);

//            if (objMember != null)
//            {
//                var x = -objMember.Position.X + (Control.Width / 2) / flZoomLevel - objMember.Size.Width;
//                var y = -objMember.Position.Y + (Control.Height / 2) / flZoomLevel - objMember.Size.Height;
//                translateDistance = new SKPoint(x, y);
//                translateCurrentDistance = translateDistance;
//                Control.Invalidate();
//            }
//        }

//        private DrawMember GetDataMember(string memberId)
//        {
//            if (string.IsNullOrEmpty(memberId) || !dataTree.HasValue() || !dataTree.ContainsKey(memberId))
//            {
//                return null;
//            }

//            return dataTree[memberId];
//        }

//        private void PaintSurfaceCanvas(object sender, SKCanvas canvas)
//        {
//            if (!dataTree.HasValue())
//            {
//                return;
//            }

//            canvas.Clear(Color.Aqua.ToSKColor());
//            canvas.Scale(flZoomLevel);
//            canvas.Translate(translateDistance);

//            var dataDraw = dataTree.Where(i => InCanvas(i.Value, canvas)).Select(i => i.Value).ToList();

//            dataDraw.ForEach(member =>
//            {
//                canvas.DrawPicture(member.DrawingMemberSVG(), member.Position);
//            });

//            //using (var paint = new SKPaint())
//            //{
//            //    paint.IsAntialias = true;
//            //    paint.IsStroke = false;
//            //    paint.TextSize = 14f;
//            //    paint.Typeface = SKTypeface.FromFamilyName("Arial");
//            //    paint.Color = Color.Red.ToSKColor();
//            //    paint.TextEncoding = SKTextEncoding.Utf8;

//            //    SKRect textBounds = new SKRect();
//            //    paint.MeasureText("Tesssssssssssssssssssss", ref textBounds);

//            //    // Calculate offsets to center the text on the screen
//            //    float xText = Control.ClientRectangle.X - textBounds.Location.X;
//            //    float yText = Control.ClientRectangle.Y - textBounds.Location.Y;


//            //    canvas.DrawText("Tesssssssssssssssssssss", xText, yText, paint);
//            //}

//            //canvas.Flush();
//            canvas.ResetMatrix();
//        }

//        private void skTree_MouseDown(object sender, MouseEventArgs e)
//        {
//            blnMouseDown = true;
//            pointMouseDown = new Point(e.X, e.Y);
//            // Set cursor as hourglass
//            Cursor.Current = Cursors.Hand;

//            //if (e.Button == MouseButtons.Right && MemberRightClick != null)
//            //{
//            //    var objMember = getMemberClicked(e);

//            //    if (objMember != null)
//            //    {
//            //        SelectedMember = objMember;
//            //        MemberRightClick(this, objMember);
//            //    }
//            //}
//        }

//        private void skTree_MouseLeave(object sender, EventArgs e)
//        {
//            blnMouseDown = false;
//            //Invalidate();
//            Control.Invalidate();

//            // Set cursor as default arrow
//            Cursor.Current = Cursors.Default;
//        }

//        private void skTree_MouseMove(object sender, MouseEventArgs e)
//        {
//            if (blnMouseDown)
//            {
//                translateDistance = new SKPoint(translateCurrentDistance.X + (e.X - pointMouseDown.X) / flZoomLevel, translateCurrentDistance.Y + (e.Y - pointMouseDown.Y) / flZoomLevel);
//                Control.Invalidate();
//                //SelectedMember = null;
//            }
//        }

//        private void skTree_MouseUp(object sender, MouseEventArgs e)
//        {
//            blnMouseDown = false;
//            pointMouseDown = new Point(e.X, e.Y);
//            translateCurrentDistance = translateDistance;

//            // Set cursor as default arrow
//            Cursor.Current = Cursors.Default;
//        }

//        private bool InCanvas(DrawMember objMember, SKCanvas canvas)
//        {
//            var up = objMember.Size.Height;
//            var down = -up;
//            var test = new SKRectI(AutoUp(canvas.DeviceClipBounds.Left, down),
//                                   AutoUp(canvas.DeviceClipBounds.Top, down),
//                                   AutoUp(canvas.DeviceClipBounds.Right, up),
//                                   AutoUp(canvas.DeviceClipBounds.Bottom, up));

//            return test.Contains((int)(objMember.Position.X + translateDistance.X), (int)(objMember.Position.Y + translateDistance.Y));
//        }

//        private int AutoUp(int input, int up) => (int)(Math.Ceiling((input + up) / flZoomLevel));

//        #region Dispose

//        private bool disposedValue;

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    // TODO: dispose managed state (managed objects)
//                    if (Control != null)
//                    {
//                        ((ISKControl)Control).PaintSurfaceCanvas -= new EventHandler<SKCanvas>(PaintSurfaceCanvas);
//                        Control.Dispose();
//                        Control = null;
//                    }
//                }

//                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
//                // TODO: set large fields to null
//                disposedValue = true;
//            }
//        }

//        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
//        // ~DrawTreeManager()
//        // {
//        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
//        //     Dispose(disposing: false);
//        // }

//        public void Dispose()
//        {
//            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
//            Dispose(disposing: true);
//            GC.SuppressFinalize(this);
//        }

//        #endregion Dispose
//    }
//}