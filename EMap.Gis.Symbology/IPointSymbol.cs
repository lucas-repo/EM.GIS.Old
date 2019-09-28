using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;

using System.Numerics;
using System.Text;

namespace EMap.Gis.Symbology
{
    public interface IPointSymbol: IFeatureSymbol,IOutlineSymbol
    {
        float Angle { get; set; }
        PointF Offset { get; set; }
        PointSymbolType PointSymbolType { get; }
        void DrawPoint(IImageProcessingContext context, float scale, PointF point);
        SizeF Size { get; set; }
    }
}
