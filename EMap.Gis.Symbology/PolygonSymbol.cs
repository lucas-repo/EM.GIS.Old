using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

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

        public void DrawPath(Image<Rgba32> image, PointF[] points, float scale)
        {
            if (UseOutLine)
            {
                IPath path = points.ToPath();
                OutLineSymbolizer.DrawPath(image, path, scale);
            }
        }

        public void FillPath(Image<Rgba32> image, PointF[] points)
        {
            IBrush<Rgba32> brush = GetBrush();
            if (brush != null)
            {
                image.Mutate(x => x.FillPolygon(brush, points));
            }
        }

        public virtual IBrush<Rgba32> GetBrush()
        {
            IBrush<Rgba32> brush = new SolidBrush<Rgba32>(Color);
            return brush;
        }
    }
}
