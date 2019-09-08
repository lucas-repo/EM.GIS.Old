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
    public interface IPolygonSymbolizer:IFeatureSymbolizer
    {
        new IPolygonSymbolCollection Symbols { get; set; }
        void DrawPolygon(IImageProcessingContext<Rgba32> context, float scale, IPath path);
    }
}
