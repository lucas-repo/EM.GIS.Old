using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface IPolygonSymbol: IFeatureSymbol,IOutlineSymbol
    {
        PolygonSymbolType PolygonSymbolType { get; }
        RectangleF Bounds { get; set; }
        void DrawPolygon(IImageProcessingContext context, float scale, IPath path);
        IBrush GetBrush();
    }
}
