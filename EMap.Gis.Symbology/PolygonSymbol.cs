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

        public void DrawPolygon(IImageProcessingContext context, float scale, IPath path)
        {
            if (context == null || path == null)
            {
                return;
            }
            IBrush brush = GetBrush();
            if (brush == null)
            {
                return;
            }
            context.Fill(brush, path);
            DrawOutLine(context, scale, path);
        }

        public virtual IBrush GetBrush()
        {
            IBrush brush = new SolidBrush(Color);
            return brush;
        }
    }
}
