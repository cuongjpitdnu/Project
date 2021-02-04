using System.Collections.Generic;
using System.Drawing;

namespace GP40Common
{
    public class RectangleComparer : IComparer<Rectangle>
    {
        public int Compare(Rectangle x, Rectangle y)
        {
            return x.Y.CompareTo(y.Y);
        }
    }
}
