using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public abstract class FeatureSymbol : Descriptor, IFeatureSymbol
    {
        public virtual Rgba32 Color { get; set; }
        public virtual float Opacity { get; set; } = 1;
    }
}
