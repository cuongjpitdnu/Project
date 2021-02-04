using GP40Common;
using SkiaSharp;
using System;
using System.Windows.Forms;

namespace GP40DrawTree
{
    public class DrawCommon : IDisposable
    {
        protected Control _controlTree;
        protected SKPoint _pointTranslate;
        protected SKPoint _translateCurrentDistance;

        public SKPoint _pointOffset;
        public SKPoint _pointCurrent;
        protected float flZoomLevel = 1f;
        protected float MinZoomLevel = 0.2f;
        protected float MaxZoomLevel = 2f;
        protected float ZoomLevel = 0.05f;

        protected float intMinX;
        protected float intMinY;
        protected float intMaxX;
        protected float intMaxY;

        protected clsFamilyMember _memberSelected;
        protected clsFamilyMember _memberMale = new clsFamilyMember() { Id = clsConst.ENUM_GENDER.Male.ToString(), Gender = clsConst.ENUM_GENDER.Male };
        protected clsFamilyMember _memberFMale = new clsFamilyMember() { Id = clsConst.ENUM_GENDER.FMale.ToString(), Gender = clsConst.ENUM_GENDER.FMale };
        protected clsFamilyMember _memberUnknown = new clsFamilyMember() { Id = clsConst.ENUM_GENDER.Unknown.ToString(), Gender = clsConst.ENUM_GENDER.Unknown };

        public Control Tree => _controlTree;

        protected clsFamilyMember getTemplate(clsConst.ENUM_GENDER gender)
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
        }

        protected void CreateTree(bool useGpu)
        {
            if (useGpu)
            {
                _controlTree = new SKGLCustom();
            }
            else
            {
                _controlTree = new SKCustom();
            }
            _controlTree.Dock = DockStyle.Fill;
        }

        protected void calculateZoomLevel(int intNum)
        {
            var condition = intNum > 0 ? flZoomLevel < MaxZoomLevel : flZoomLevel > MinZoomLevel;
            flZoomLevel = flZoomLevel + (ZoomLevel * intNum);
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
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DrawListMember()
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