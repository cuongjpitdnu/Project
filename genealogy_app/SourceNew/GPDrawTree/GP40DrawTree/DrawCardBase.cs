using GPCommon;
using SkiaSharp;
using System;
using System.IO;
using System.Text;

namespace GP40Common
{
    public abstract class DrawCardBase : IDisposable
    {
        public string Id;
        public SKPoint Position;
        public SKSizeI Size;

        private MemoryStream _memoryStreamWrite = new MemoryStream();
        protected clsConst.ENUM_MEMBER_TEMPLATE enmTemplate;

        protected virtual void DrawText(SKCanvas canvas)
        {

        }

        protected virtual void DrawImage(SKCanvas canvas)
        {

        }

        protected virtual void DrawFrame(SKCanvas canvas)
        {

        }

        public SKPicture ToPicture(bool notUseCache = false)
        {
            var strFileName = clsCommon.GetMemberCardDataPath(enmTemplate) + Id + ".svg";
            var svgData = !notUseCache ? LoadCache(strFileName, true) : new SkiaSharp.Extended.Svg.SKSvg();
            byte[] dataContentSvg = null;
            if (svgData.Picture != null)
            {
                return svgData.Picture;
            }
            using (var memory = new MemoryStream())
            {
                using (SKCanvas canvas = SKSvgCanvas.Create(SKRect.Create(Size.Width, Size.Height), memory))
                {
                    DrawText(canvas);
                    DrawImage(canvas);
                }
                dataContentSvg = memory.ToArray();
                Encoding.UTF8.GetString(dataContentSvg).ToFileEncrypt(strFileName, clsConst.PASS_SVG);
            }
            using (var memory = new MemoryStream(dataContentSvg))
            {
                svgData.Load(memory);
            }
            return svgData.Picture;
        }

        public SKPicture ToPictureFrame(bool notUseCache = false)
        {
            var strFileName = clsCommon.GetMemberCardDataPath(enmTemplate) + "_frame.svg";
            var svgData = !notUseCache ? LoadCache(strFileName, false) : new SkiaSharp.Extended.Svg.SKSvg();
            byte[] dataContentSvg = null;

            if (svgData.Picture != null)
            {
                return svgData.Picture;
            }

            using (var memory = new MemoryStream())
            {
                using (SKCanvas canvas = SKSvgCanvas.Create(SKRect.Create(Size.Width, Size.Height), memory))
                {
                    DrawFrame(canvas);
                }

                dataContentSvg = memory.ToArray();
            }

            File.WriteAllText(strFileName, Encoding.UTF8.GetString(dataContentSvg));

            using (var memory = new MemoryStream(dataContentSvg))
            {
                svgData.Load(memory);
            }

            return svgData.Picture;
        }

        public static void DeleteCacheFrame()
        {
            var arr = Enum.GetNames(typeof(clsConst.ENUM_MEMBER_TEMPLATE));

            if (arr != null && arr.Length > 0)
            {
                foreach (var name in arr)
                {
                    var template = (clsConst.ENUM_MEMBER_TEMPLATE)Enum.Parse(typeof(clsConst.ENUM_MEMBER_TEMPLATE), name);
                    var pathDelete = clsCommon.GetMemberCardDataPath(template) + "_frame.svg";

                    if (File.Exists(pathDelete))
                    {
                        File.Delete(pathDelete);
                    }
                }
            }
        }

        #region Private Func

        private SkiaSharp.Extended.Svg.SKSvg LoadCache(string strFilePath, bool blnFileEncryped)
        {
            var svgData = new SkiaSharp.Extended.Svg.SKSvg();

            if (File.Exists(strFilePath))
            {
                var strContentSvg = blnFileEncryped
                                    ? EncryptHelper.ReadFileDecrypt(strFilePath, clsConst.PASS_SVG)
                                    : File.ReadAllText(strFilePath);

                using (var stream = ConvertHelper.CnvStringToStream(strContentSvg))
                {
                    svgData.Load(stream);
                }
            }

            return svgData;
        }

        #endregion Private Func

        #region IDisposable

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
        // ~DrawCardBase()
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

        #endregion IDisposable
    }
}
