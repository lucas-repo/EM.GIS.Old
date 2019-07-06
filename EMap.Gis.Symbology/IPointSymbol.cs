using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
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
        void Draw(Image<Rgba32> image, float scale);
        SizeF Size { get; set; }
    }
}
