using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    public interface IFeatureSymbolizer:ISymbolizer
    {
        ScaleMode ScaleMode { get; set; }
        double GetScale(MapArgs drawArgs);

        IFeatureSymbolCollection Symbols { get; set; }
    }
}
