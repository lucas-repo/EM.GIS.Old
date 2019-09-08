using System;
using System.Collections.Generic;
using System.Linq;
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

        public void DrawPolygon(IImageProcessingContext<Rgba32> context, float scale, IPath path)
        {
            if (context == null || path == null )
            {
                return;
            }
            IBrush<Rgba32> brush = GetBrush();
            if (brush == null)
            {
                return;
            }
            context.Fill(brush, path);
            var simplePaths = path.Flatten();
            foreach (var simplePath in simplePaths)
            {
                DrawOutLine(context, scale, simplePath.Points.ToArray());
            }
        }

        public virtual IBrush<Rgba32> GetBrush()
        {
            IBrush<Rgba32> brush = new SolidBrush<Rgba32>(Color);
            return brush;
        }
    }
}
