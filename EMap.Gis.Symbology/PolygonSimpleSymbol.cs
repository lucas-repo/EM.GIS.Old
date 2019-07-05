using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class PolygonSimpleSymbol : PolygonSymbol, IPolygonSimpleSymbol
    {
        public PolygonSimpleSymbol() : base(PolygonSymbolType.Simple)
        { }
    }
}
