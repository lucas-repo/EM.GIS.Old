using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
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
        IList<ILineSymbol> Symbols { get;}
        void DrawPath(Image<Rgba32> image, float scale, PointF[] points);
    }
}
