using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace GPMain.Common.Helper
{
    public class FileHepler
    {

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height, bool highQuality = false)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = highQuality ? CompositingQuality.HighQuality : CompositingQuality.HighSpeed;
                graphics.InterpolationMode = highQuality ? InterpolationMode.HighQualityBicubic : InterpolationMode.Bicubic;
                graphics.SmoothingMode = highQuality ? SmoothingMode.HighQuality : SmoothingMode.HighSpeed;
                graphics.PixelOffsetMode = highQuality ? PixelOffsetMode.HighQuality : PixelOffsetMode.HighSpeed;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Bitmap ResizeImage(Image image, int height, bool highQuality = false)
        {
            if (image == null || height < 1)
            {
                return null;
            }

            var ratio = (float)height / image.Height;
            var imgW = image.Width * ratio;
            var imgH = height;

            return ResizeImage(image, (int)imgW, imgH, highQuality);
        }

        public static void SaveImage(Image image, string saveAs, long levelCompression = 80L)
        {
            var typeImage = image != null && !string.IsNullOrEmpty(saveAs) ? Path.GetExtension(saveAs) : string.Empty;

            if (string.IsNullOrEmpty(typeImage))
            {
                return;
            }

            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, levelCompression);

            myEncoderParameters.Param[0] = myEncoderParameter;
            typeImage = typeImage.ToLower();

            if (typeImage.EndsWith("jpg"))
            {
                image.Save(saveAs, GetEncoder(ImageFormat.Jpeg), myEncoderParameters);
                return;
            }

            if (typeImage.EndsWith("png"))
            {
                image.Save(saveAs, GetEncoder(ImageFormat.Png), myEncoderParameters);
                return;
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        private static readonly string[] SizeSuffixes =
               { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        /// <summary>
        /// Size Display
        /// </summary>
        /// <param name="value">bytes 數值</param>
        /// <param name="decimalPlaces">小數位數</param>
        /// <returns></returns>
        public static string SizeSuffix(Int64 value, int decimalPlaces = 2)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + SizeSuffix(-value); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag]);
        }


        /// <summary>
        /// Create folder
        /// </summary>
        /// <param name="path"></param>
        public static void CreateFolder(string path)
        {
            fncDeleteFolder(path);
            fncCreateFolder(path);
        }

        public static bool isDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }
        public static bool fncDeleteFolder(string strFolderPath)
        {
            try
            {
                if (System.IO.Directory.Exists(strFolderPath))
                {
                    DirectoryInfo objDirInfo = new DirectoryInfo(strFolderPath);

                    // reset attribute before deleting
                    xSetFolderAttr(objDirInfo, FileAttributes.Normal);
                    objDirInfo.Delete(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
            }
        }
        private static bool xSetFolderAttr(System.IO.DirectoryInfo objDir, FileAttributes emAttr)
        {
            try
            {
                if (objDir.Exists)
                {
                    // set this folder's attribute
                    objDir.Attributes = FileAttributes.Normal;

                    // set file's attribute
                    foreach (FileInfo objFile in objDir.GetFiles())
                        objFile.Attributes = FileAttributes.Normal;

                    // set folder's attribute
                    foreach (DirectoryInfo objFolder in objDir.GetDirectories())
                        xSetFolderAttr(objFolder, emAttr);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
            }
        }
        public static bool fncCreateFolder(string strFolderPath, bool blnIsHidden = true)
        {
            System.IO.DirectoryInfo objDirInfo = null;

            try
            {
                objDirInfo = new System.IO.DirectoryInfo(strFolderPath);

                // check existance of temp folder
                if (!objDirInfo.Exists)
                {
                    // create folder
                    objDirInfo.Create();

                    // set hidden
                    if (blnIsHidden)
                        objDirInfo.Attributes = System.IO.FileAttributes.Hidden;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDirInfo = null;
            }
        }
    }
}
