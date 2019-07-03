using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    public interface ISymbolizer : IDisposable,ICloneable
    {
        void Draw(Image<Rgba32> image, Rectangle rectangle);
    }
}
