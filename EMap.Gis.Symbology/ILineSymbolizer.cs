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
    public interface ILineSymbolizer:IFeatureSymbolizer
    {
        Rgba32 Color { get; set; }
        float Width { get; set; }
        new ILineSymbolCollection Symbols { get; set; }
        void DrawLine(IImageProcessingContext context, float scale, IPath path);
    }
}
