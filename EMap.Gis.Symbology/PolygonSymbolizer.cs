using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    internal class PolygonSymbolizer :FeatureSymbolizer, IPolygonSymbolizer
    {
        public IList<IPolygonSymbol> Symbols { get; } = new List<IPolygonSymbol>();

        public PolygonSymbolizer():this(false)
        {
        }
        public PolygonSymbolizer(Rgba32 fillColor, Rgba32 outlineColor)
           : this(fillColor, outlineColor, 1)
        {
        }
        public PolygonSymbolizer(Rgba32 fillColor, Rgba32 outlineColor, float outlineWidth)
        {
            var symbol = new PolygonSimpleSymbol(fillColor);
            symbol.OutLineSymbolizer = new LineSymbolizer(outlineColor, outlineWidth);
            Symbols.Add(symbol);
        }
        public PolygonSymbolizer(Rgba32 startColor, Rgba32 endColor, double angle, GradientType style, Rgba32 outlineColor, float outlineWidth)
            : this(startColor, endColor, angle, style)
        {
            Symbols[0].OutLineSymbolizer = new LineSymbolizer(outlineColor, outlineWidth);
        }
        public PolygonSymbolizer(Rgba32 startColor, Rgba32 endColor, double angle, GradientType style)
        {
            IPolygonSymbol symbol = new PolygonGradientSymbol(startColor, endColor, angle, style);
            Symbols.Add(symbol);
        }
        public PolygonSymbolizer(bool selected)
        {
            IPolygonSymbol polygonSymbol = new PolygonSimpleSymbol();
            if (selected)
            {
                polygonSymbol.Color = Rgba32.Cyan;
            }
            Symbols.Add(polygonSymbol);
        }
        public override void LegendSymbolPainted(Image<Rgba32> image, Rectangle rectangle)
        {
            PointF[] points = new PointF[]
            {
                new PointF(rectangle.Left,rectangle.Top),
                new PointF(rectangle.Left,rectangle.Bottom),
                new PointF(rectangle.Right,rectangle.Bottom),
                new PointF(rectangle.Right,rectangle.Top),
                new PointF(rectangle.Left,rectangle.Top)
            };
            foreach (var symbol in Symbols)
            {
                symbol.FillPath(image, points);
            }
            foreach (var symbol in Symbols)
            {
                symbol.DrawPath(image,1, points);
            }
        }
        
    }
}