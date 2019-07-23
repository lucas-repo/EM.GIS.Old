using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using SixLabors.Shapes;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    internal class LineSymbolizer : FeatureSymbolizer, ILineSymbolizer
    {
        public IList<ILineSymbol> Symbols { get; set; } = new List<ILineSymbol>();
        public Rgba32 Color
        {
            get
            {
                Rgba32 color = new Rgba32();
                if (Symbols.Count > 0)
                {
                    color = Symbols[Symbols.Count - 1].Color;
                }
                return color;
            }
            set
            {
                if (Symbols.Count > 0)
                {
                    Symbols[Symbols.Count - 1].Color = value;
                }
            }
        }
        public float Width
        {
            get
            {
                float width = 0;
                foreach (var item in Symbols)
                {
                    if (item.Width > width)
                    {
                        width = item.Width;
                    }
                }
                return width;
            }
            set
            {
                float width = Width;
                var ratio = value / width;
                foreach (var item in Symbols)
                {
                    Symbols[Symbols.Count - 1].Width *= ratio;
                }
            }
        }

        public LineSymbolizer() : this(false)
        {
        }
        public LineSymbolizer(Rgba32 color, float width)
        {
            var symbol = new LineSimpleSymbol(width, color);
            Symbols.Add(symbol);
        }
        public LineSymbolizer(bool selected)
        {
            ILineSymbol lineSymbol = new LineSimpleSymbol();
            if (selected)
            {
                lineSymbol.Color = Rgba32.Cyan;
            }
            Symbols.Add(lineSymbol);
        }

        public void DrawPath(Image<Rgba32> image, float scale, PointF[] points)
        {
            foreach (var stroke in Symbols)
            {
                var p = stroke.ToPen(scale);
                stroke.DrawPath(image, scale, points);
            }
        }

        public override void LegendSymbolPainted(Image<Rgba32> image, Rectangle rectangle)
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