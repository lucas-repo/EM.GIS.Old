using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    public interface ILineSymbol : IFeatureSymbol
    {
        float Width { get; set; }
        LineSymbolType LineSymbolType { get; }
        void DrawLine(IImageProcessingContext context, float scale, IPath path);
        IPen ToPen(float scale);
    }
}
