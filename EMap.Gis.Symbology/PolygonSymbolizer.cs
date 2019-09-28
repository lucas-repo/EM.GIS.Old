using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    internal class PolygonSymbolizer : FeatureSymbolizer, IPolygonSymbolizer
    {
        public new IPolygonSymbolCollection Symbols { get => base.Symbols as IPolygonSymbolCollection; set => base.Symbols = value; }
      
        public PolygonSymbolizer()
        {
            Symbols = new PolygonSymbolCollection();
            PolygonSimpleSymbol polygonSymbol = new PolygonSimpleSymbol();
            Symbols.Add(polygonSymbol);
        }

        public PolygonSymbolizer(bool selected) 
        {
            Symbols = new PolygonSymbolCollection();
            IPolygonSymbol polygonSymbol = new PolygonSimpleSymbol();
            if (selected)
            {
                polygonSymbol.Color = Rgba32.Cyan;
            }
            Symbols.Add(polygonSymbol);
        }
        public override void DrawLegend(IImageProcessingContext context, Rectangle rectangle)
        {
            PointF[] points = new PointF[]
            {
                new PointF(rectangle.Left,rectangle.Top),
                new PointF(rectangle.Left,rectangle.Bottom),
                new PointF(rectangle.Right,rectangle.Bottom),
                new PointF(rectangle.Right,rectangle.Top),
                new PointF(rectangle.Left,rectangle.Top)
            };
            DrawPolygon(context, 1, points.ToPolygon());
        }
        public void DrawPolygon(IImageProcessingContext context, float scale, IPath path)
        {
            foreach (var symbol in Symbols)
            {
                symbol.DrawPolygon(context, scale, path);
            }
        }
    }
}