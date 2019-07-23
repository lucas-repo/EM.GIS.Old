using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class PolygonGradientSymbol : PolygonSymbol, IPolygonGradientSymbol
    {
        public PolygonGradientSymbol() : base(PolygonSymbolType.Gradient)
        { }
        public PolygonGradientSymbol(Rgba32 startColor, Rgba32 endColor, double angle, GradientType style):this()
        {
            Colors = new  Rgba32[2];
            Colors[0] = startColor;
            Colors[1] = endColor;
            Positions = new[] { 0F, 1F };
            Angle = angle;
            GradientType = style;
        }
        public double Angle { get; set; }
        public Rgba32[] Colors { get; set; }
        public GradientType GradientType { get; set; }
        public float[] Positions { get; set; }
        public override IBrush<Rgba32> GetBrush()
        {
            IBrush < Rgba32 > brush= base.GetBrush();
            RectangleF bounds = Bounds;
            if (bounds.IsEmpty|| bounds.Width == 0 || bounds.Height == 0)
            {
                return brush;
            }
            ColorStop<Rgba32>[] colorStops = new ColorStop<Rgba32>[Colors.Length];
            for (int i = 0; i < Colors.Length; i++)
            {
                colorStops[i] = new ColorStop<Rgba32>(Positions[i], Colors[i]);
            }
            switch (GradientType)
            {
                case GradientType.Circular:
                    {
                        float radius = bounds.Width / 2;
                        Point center = new Point((int)(bounds.X + bounds.Width / 2), (int)(bounds.Y + bounds.Height / 2));
                        brush = new RadialGradientBrush<Rgba32>(center, radius, GradientRepetitionMode.None, colorStops);
                    }
                    break;
                //case GradientType.Contour:
                //    break;
                case GradientType.Linear:
                    Point start = new Point((int)bounds.X, (int)(bounds.Y - bounds.Height / 2));
                    Point end = new Point((int)(bounds.X + bounds.Width), (int)(bounds.Y - bounds.Height / 2));
                    brush = new LinearGradientBrush<Rgba32>(start, end, GradientRepetitionMode.None, colorStops);
                    break;
                case GradientType.Rectangular:
                    {
                        Point center = new Point((int)(bounds.X + bounds.Width / 2), (int)(bounds.Y + bounds.Height / 2));
                        Point referenceAxisEnd = new Point((int)bounds.X, center.Y);
                        float axisRatio = bounds.Width / bounds.Height;
                        brush = new EllipticGradientBrush<Rgba32>(center, referenceAxisEnd, axisRatio, GradientRepetitionMode.None, colorStops);
                    }
                    break;
            }
            return brush;
        }
    }
}
