using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public abstract class PolygonSymbol : OutLineSymbol, IPolygonSymbol
    {
        public PolygonSymbolType PolygonSymbolType { get; }

        public RectangleF Bounds { get; set; }
        public PolygonSymbol(PolygonSymbolType polygonSymbolType)
        {
            PolygonSymbolType = polygonSymbolType;
        }

        public void Draw(Image<Rgba32> image, PointF[] polygon, float scale)
        {
            IBrush<Rgba32> brush = ToBrush();
            image.Mutate(x => x.FillPolygon(brush, polygon));
        }

        public virtual IBrush<Rgba32> ToBrush()
        {
            IBrush<Rgba32> brush = new SolidBrush<Rgba32>(Color);
            return brush;
        }
    }
}
