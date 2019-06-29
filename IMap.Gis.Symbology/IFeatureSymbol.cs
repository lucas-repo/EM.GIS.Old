using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMap.Gis.Symbology
{
    public interface IFeatureSymbol:ICloneable
    {
        Rgba32 Color { get; set; }
        IImage GetImage(Rectangle rectangle);
    }
}
