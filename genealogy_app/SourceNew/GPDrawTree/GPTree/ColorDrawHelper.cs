using SkiaSharp;
using System.Drawing;

namespace GPTree
{
    public class ColorDrawHelper
    {
        public static SKColor FromColor(Color color)
        {
            return new SKColor(color.R, color.G, color.B);
        }

        public static SKColor FromColor(int R, int G, int B)
        {
            return FromColor(Color.FromArgb(R, G, B));
        }

        public static SKColor FromHtml(string hex)
        {
            return FromColor(ColorTranslator.FromHtml(hex));
        }

        public static Color FromHtmlToColor(string hex)
        {
            return ColorTranslator.FromHtml(hex);
        }
    }
}
