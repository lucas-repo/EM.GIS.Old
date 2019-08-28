using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public abstract class PointSymbol : OutLineSymbol, IPointSymbol
    {
        public float Angle { get; set; }
        public SizeF Size { get; set; }
        public PointF Offset { get; set; }
        public PointSymbolType PointSymbolType { get; }
        public PointSymbol(PointSymbolType pointSymbolType)
        {
            PointSymbolType = pointSymbolType;
        }
        public abstract void DrawPoint(IImageProcessingContext<Rgba32> context, float scale, PointF point);
    }
}
