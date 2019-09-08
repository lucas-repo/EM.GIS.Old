using System;
using System.Drawing;

namespace EMap.Gis.Symbology
{
    public static class ColorExtension
    {
        public static float GetOpacity(this Color rgba32)
        {
            return rgba32.A / 255F;
        }
        public static Color ToTransparent(this Color rgba32, float opacity)
        {
            int a = Convert.ToInt32(opacity * 255);
            if (a > 255) a = 255;
            if (a < 0) a = 0;
            return Color.FromArgb(rgba32.R, rgba32.G, rgba32.B, a);
        }
    }
}
