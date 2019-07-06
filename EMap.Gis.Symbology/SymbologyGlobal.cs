using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;

namespace EMap.Gis.Symbology
{
    public static class SymbologyGlobal
    {
        #region Fields

        private static readonly Random DefaultRandom = new Random();

        #endregion

        #region Methods

        public static Rgba32 ColorFromHsl(double hue, double saturation, double brightness)
        {
            double normalizedHue = hue / 360;

            double red, green, blue;

            if (brightness == 0)
            {
                red = green = blue = 0;
            }
            else
            {
                if (saturation == 0)
                {
                    red = green = blue = brightness;
                }
                else
                {
                    double temp2;
                    if (brightness <= 0.5)
                    {
                        temp2 = brightness * (1.0 + saturation);
                    }
                    else
                    {
                        temp2 = brightness + saturation - (brightness * saturation);
                    }

                    double temp1 = (2.0 * brightness) - temp2;

                    double[] temp3 = { normalizedHue + (1.0 / 3.0), normalizedHue, normalizedHue - (1.0 / 3.0) };
                    double[] color = { 0, 0, 0 };
                    for (int i = 0; i < 3; i++)
                    {
                        if (temp3[i] < 0) temp3[i] += 1.0;

                        if (temp3[i] > 1) temp3[i] -= 1.0;

                        if (6.0 * temp3[i] < 1.0)
                        {
                            color[i] = temp1 + ((temp2 - temp1) * temp3[i] * 6.0);
                        }
                        else if (2.0 * temp3[i] < 1.0)
                        {
                            color[i] = temp2;
                        }
                        else if (3.0 * temp3[i] < 2.0)
                        {
                            color[i] = temp1 + ((temp2 - temp1) * ((2.0 / 3.0) - temp3[i]) * 6.0);
                        }
                        else
                        {
                            color[i] = temp1;
                        }
                    }

                    red = color[0];
                    green = color[1];
                    blue = color[2];
                }
            }

            if (red > 1) red = 1;
            if (red < 0) red = 0;
            if (green > 1) green = 1;
            if (green < 0) green = 0;
            if (blue > 1) blue = 1;
            if (blue < 0) blue = 0;

            return new Rgba32((int)(255 * red), (int)(255 * green), (int)(255 * blue));
        }
        public static IImageProcessingContext<TPixel> DrawLines<TPixel>(this IImageProcessingContext<TPixel> source, IPen<TPixel> pen, float x0, float y0, float x1, float y1) where TPixel : struct, IPixel<TPixel>
        {
            return source.DrawLines(pen, new PointF(x0, y0), new PointF(x1, y1));
        }
        /// <summary>
        /// Draws a rectangle with ever so slightly rounded edges. Good for selection borders.
        /// </summary>
        /// <param name="g">The Graphics object</param>
        /// <param name="pen">The pen to draw with</param>
        /// <param name="rect">The rectangle to draw to.</param>
        public static void DrawRoundedRectangle(Image<Rgba32> image, Pen<Rgba32> pen, Rectangle rect)
        {
            int l = rect.Left;
            int r = rect.Right;
            int t = rect.Top;
            int b = rect.Bottom;
            image.Mutate(x => x
            .DrawLines(pen, l + 1, t, r - 1, t)
            .DrawLines(pen, l, t + 1, l, b - 1)
            .DrawLines(pen, r, t + 1, r, b - 1)
            .DrawLines(pen, l, t + 2, l + 2, t)
            .DrawLines(pen, r - 2, t, r, t + 2)
            .DrawLines(pen, l, b - 2, l + 2, b)
            .DrawLines(pen, r, b - 2, r - 2, b));
        }

        /// <summary>
        /// Obtains a system.Drawing.Rectangle based on the two points, using them as
        /// opposite extremes for the rectangle.
        /// </summary>
        /// <param name="a">one corner point of the rectangle.</param>
        /// <param name="b">The opposing corner of the rectangle.</param>
        /// <returns>A System.Draing.Rectangle</returns>
        public static Rectangle GetRectangle(Point a, Point b)
        {
            int x = Math.Min(a.X, b.X);
            int y = Math.Min(a.Y, b.Y);
            int w = Math.Abs(a.X - b.X);
            int h = Math.Abs(a.Y - b.Y);
            return new Rectangle(x, y, w, h);
        }
        public static float GetBrightness(this Rgba32 rgba32)
        {
            float num = rgba32.R / 255f;
            float num2 = rgba32.G / 255f;
            float num3 = rgba32.B / 255f;
            float num4 = num;
            float num5 = num;
            if (num2 > num4)
            {
                num4 = num2;
            }
            if (num3 > num4)
            {
                num4 = num3;
            }
            if (num2 < num5)
            {
                num5 = num2;
            }
            if (num3 < num5)
            {
                num5 = num3;
            }
            return (num4 + num5) / 2f;
        }
        public static float GetHue(this Rgba32 rgba32)
        {
            if (rgba32.R == rgba32.G && rgba32.G == rgba32.B)
            {
                return 0f;
            }
            float num = rgba32.R / 255f;
            float num2 = rgba32.G / 255f;
            float num3 = rgba32.B / 255f;
            float num4 = 0f;
            float num5 = num;
            float num6 = num;
            if (num2 > num5)
            {
                num5 = num2;
            }
            if (num3 > num5)
            {
                num5 = num3;
            }
            if (num2 < num6)
            {
                num6 = num2;
            }
            if (num3 < num6)
            {
                num6 = num3;
            }
            float num7 = num5 - num6;
            if (num == num5)
            {
                num4 = (num2 - num3) / num7;
            }
            else if (num2 == num5)
            {
                num4 = 2f + (num3 - num) / num7;
            }
            else if (num3 == num5)
            {
                num4 = 4f + (num - num2) / num7;
            }
            num4 *= 60f;
            if (num4 < 0f)
            {
                num4 += 360f;
            }
            return num4;
        }
        public static float GetSaturation(this Rgba32 rgba32)
        {
            float num = rgba32.R / 255f;
            float num2 = rgba32.G / 255f;
            float num3 = rgba32.B / 255f;
            float result = 0f;
            float num4 = num;
            float num5 = num;
            if (num2 > num4)
            {
                num4 = num2;
            }
            if (num3 > num4)
            {
                num4 = num3;
            }
            if (num2 < num5)
            {
                num5 = num2;
            }
            if (num3 < num5)
            {
                num5 = num3;
            }
            if (num4 != num5)
            {
                float num6 = (num4 + num5) / 2f;
                result = ((!((double)num6 <= 0.5)) ? ((num4 - num5) / (2f - num4 - num5)) : ((num4 - num5) / (num4 + num5)));
            }
            return result;
        }
        /// <summary>
        /// Gets a cool Highlight brush for highlighting things.
        /// </summary>
        /// <param name="box">The rectangle in the box</param>
        /// <param name="selectionHighlight">The color to use for the higlight</param>
        /// <returns>The highlight brush.</returns>
        public static IBrush<Rgba32> HighlightBrush(Rectangle box, Rgba32 selectionHighlight)
        {
            float med = selectionHighlight.GetBrightness();
            float bright = med + 0.05f;
            if (bright > 1f) bright = 1f;
            float dark = med - 0.05f;
            if (dark < 0f) dark = 0f;
            Rgba32 brtCol = ColorFromHsl(selectionHighlight.GetHue(), selectionHighlight.GetSaturation(), bright);
            Rgba32 drkCol = ColorFromHsl(selectionHighlight.GetHue(), selectionHighlight.GetSaturation(), dark);
            Point p1 = new Point(box.X, box.Y - box.Height / 2);
            Point p2 = new Point(box.X+box.Width, box.Y - box.Height / 2);
            ColorStop<Rgba32>[] colorStops = new ColorStop<Rgba32>[2]
            {
                new ColorStop<Rgba32>(0,brtCol),
                new ColorStop<Rgba32>(0.5f,drkCol)
            };
            return new LinearGradientBrush<Rgba32>(p1, p2,  GradientRepetitionMode.None, colorStops);
        }

        /// <summary>
        /// Returns a completely random opaque color.
        /// </summary>
        /// <returns>A random color.</returns>
        public static Rgba32 RandomColor()
        {
            return new Rgba32(DefaultRandom.Next(0, 255), DefaultRandom.Next(0, 255), DefaultRandom.Next(0, 255));
        }

        /// <summary>
        /// This allows the creation of a transparent color with the specified opacity.
        /// </summary>
        /// <param name="opacity">A float ranging from 0 for transparent to 1 for opaque</param>
        /// <returns>A Rgba32</returns>
        public static Rgba32 RandomDarkColor(float opacity)
        {
            int alpha = Convert.ToInt32(opacity * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            Rgba32 rgba32 = ColorFromHsl(DefaultRandom.Next(0, 360), (double)DefaultRandom.Next(0, 255) / 256, (double)DefaultRandom.Next(0, 123) / 256);
            rgba32.A = (byte)alpha;
            return rgba32;
        }

        /// <summary>
        /// This allows the creation of a transparent color with the specified opacity.
        /// </summary>
        /// <param name="opacity">A float ranging from 0 for transparent to 1 for opaque</param>
        /// <returns>A Rgba32</returns>
        public static Rgba32 RandomLightColor(float opacity)
        {
            int alpha = Convert.ToInt32(opacity * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            Rgba32 rgba32 = ColorFromHsl(DefaultRandom.Next(0, 360), (double)DefaultRandom.Next(0, 255) / 256, (double)DefaultRandom.Next(123, 255) / 256);
            rgba32.A = (byte)alpha;
            return rgba32;
        }

        /// <summary>
        /// This allows the creation of a transparent color with the specified opacity.
        /// </summary>
        /// <param name="opacity">A float ranging from 0 for transparent to 1 for opaque</param>
        /// <returns>A Rgba32</returns>
        public static Rgba32 RandomTranslucent(float opacity)
        {
            int alpha = Convert.ToInt32(opacity * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            return new Rgba32(DefaultRandom.Next(0, 255), DefaultRandom.Next(0, 255), DefaultRandom.Next(0, 255), alpha);
        }

        #endregion
    }
}