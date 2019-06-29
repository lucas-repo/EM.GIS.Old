using SixLabors.ImageSharp;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;

using System.Numerics;
using System.Text;

namespace IMap.Gis.Symbology
{
    public interface IPointSymbol: IFeatureSymbol
    {
        float Angle { get; set; }
        PointF Offset { get; set; }
        SizeF Size { get; set; }
        PointSymbolType SymbolType { get; }
        ILineSymbolizer LineSymbolizer { get; set; }
        void Draw(IImage image, float scale);
    }
}
