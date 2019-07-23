using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    public static class ColorExt
    {
        /// <summary>
        /// Creates a color with the same hue and saturation but that is slightly brighter than this color.
        /// </summary>
        /// <param name="self">The starting color</param>
        /// <param name="brightness">The floating point value of brightness to add to this color.
        /// If the combined result is greater than 1, the result will equal one.</param>
        /// <returns>A color lighter than this color</returns>
        public static Rgba32 Lighter(this Rgba32 self, float brightness)
        {
            float b = brightness + self.GetBrightness();
            if (b < 0F) b = 0F;
            if (b > 1F) b = 1F;
            Rgba32 color = SymbologyGlobal.ColorFromHsl(self.GetHue(), self.GetSaturation(), b);
            color.A = self.A;
            return color;
        }

        /// <summary>
        /// Creates a color with the same hue and saturation but that is slightly darker than this color.
        /// </summary>
        /// <param name="self">The starting color</param>
        /// <param name="brightness">The floating point value of brightness to add to this color.</param>
        /// if the combined result is less than 0, the result will equal 0.
        /// <returns>A color darker than this color.</returns>
        public static Rgba32 Darker(this Rgba32 self, float brightness)
        {
            float b = self.GetBrightness() - brightness;
            if (b < 0F) b = 0F;
            if (b > 1F) b = 1F;
            Rgba32 color = SymbologyGlobal.ColorFromHsl(self.GetHue(), self.GetSaturation(), b);
            color.A = self.A;
            return color;
        }

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

        /// <summary>
        /// Returns an equivalent version of this color that is fully opaque (having an alpha value of 255)
        /// </summary>
        /// <param name="self">The transparent color</param>
        /// <returns>The new Rgba32</returns>
        public static Rgba32 ToOpaque(this Rgba32 self)
        {
            return new Rgba32( self.R, self.G, self.B);
        }

        /// <summary>
        /// uses a linear ramp to extrapolate the midpoint between the specified color and the new color
        /// as defined by ARGB values independantly
        /// </summary>
        /// <param name="self">This color</param>
        /// <param name="other">The color to blend with this color</param>
        /// <returns>A color that is midway between this color and the specified color</returns>
        public static Rgba32 BlendWith(this Rgba32 self, Rgba32 other)
        {
            int a = (self.A + other.A) / 2;
            int b = (self.B + other.B) / 2;
            int g = (self.G + other.G) / 2;
            int r = (self.R + other.R) / 2;
            return new Rgba32( r, g, b, a);
        }
    }
}
