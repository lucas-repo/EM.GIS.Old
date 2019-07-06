using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;

using System.Text;

namespace EMap.Gis.Symbology
{
    public interface IPointSymbolizer:IFeatureSymbolizer
    {
        IList<IPointSymbol> Symbols { get; }
        SizeF Size { get; set; }
        void Draw(Image<Rgba32> image, float scale);
    }
}
