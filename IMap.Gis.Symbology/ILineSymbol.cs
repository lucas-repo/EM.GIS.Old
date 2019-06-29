using SixLabors.ImageSharp;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace IMap.Gis.Symbology
{
    public interface ILineSymbol : IFeatureSymbol
    {
        LineSymbolType StrokeStyle { get; }
        float Width { get; set; }
        void Draw(IImage image, PointF[] line, float scale);
    }
}
