using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using SixLabors.Shapes;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    internal class LineSymbolizer : FeatureSymbolizer, ILineSymbolizer
    {
        public IList<ILineSymbol> Strokes { get; set; } = new List<ILineSymbol>();
        public LineSymbolizer() : this(false)
        {
        }
        public LineSymbolizer(bool selected)
        {
            ILineSymbol lineSymbol = new LineSimpleSymbol();
            if (selected)
            {
                lineSymbol.Color = Rgba32.Cyan;
            }
            Strokes.Add(lineSymbol);
        }

        public void DrawPath(Image<Rgba32> image, float scale, PointF[] points)
        {
            foreach (var stroke in Strokes)
            {
                var p = stroke.ToPen(scale);
                stroke.DrawPath(image, scale, points);
            }
        }

        public override void Draw(Image<Rgba32> image, Rectangle rectangle)
        {
            PointF[] points = new PointF[]
            {
                new PointF(rectangle.X, rectangle.Y + (rectangle.Height / 2)),
                new PointF(rectangle.Right, rectangle.Y + (rectangle.Height / 2))
            };
            DrawPath(image, 1, points);
        }
    }
}