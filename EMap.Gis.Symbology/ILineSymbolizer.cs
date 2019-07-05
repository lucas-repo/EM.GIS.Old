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
        IList<ILineSymbol> Strokes { get; set; }
        void DrawPath(Image<Rgba32> image, IPath path, double scale);
    }
}
