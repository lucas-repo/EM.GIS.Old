using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    public interface IFeatureSymbol:IDescriptor
    {
        Rgba32 Color { get; set; }
        float Opacity { get; set; }
    }
}
