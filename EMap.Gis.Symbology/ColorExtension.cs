using SixLabors.ImageSharp.PixelFormats;
using System;

namespace EMap.Gis.Symbology
{
    public static class ColorExtension
    {
        public static float GetOpacity(this Rgba32 rgba32)
        {
            return rgba32.A / 255F;
        }
        public static Rgba32 ToTransparent(this Rgba32 rgba32, float opacity)
        {
            int a = Convert.ToInt32(opacity * 255);
            if (a > 255) a = 255;
            if (a < 0) a = 0;
            return new Rgba32(rgba32.R, rgba32.G, rgba32.B, a);
        }
    }
}
