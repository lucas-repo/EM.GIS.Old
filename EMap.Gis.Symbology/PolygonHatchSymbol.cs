using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    //todo 暂未实现
    public class PolygonHatchSymbol : PolygonSymbol, IPolygonHatchSymbol
    {
        public PolygonHatchSymbol() : base(PolygonSymbolType.Hatch)
        {
            throw new NotImplementedException();
        }
        public override IBrush GetBrush()
        {
            return base.GetBrush();
        }
    }
}
