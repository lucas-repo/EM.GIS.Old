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

        public PolygonSymbolizer(bool selected)
        {
            IPolygonSymbol polygonSymbol = new PolygonSimpleSymbol();
            if (selected)
            {
                polygonSymbol.Color = Rgba32.Cyan;
            }
            Symbols.Add(polygonSymbol);
        }
        public override void Draw(Image<Rgba32> image, Rectangle rectangle)
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