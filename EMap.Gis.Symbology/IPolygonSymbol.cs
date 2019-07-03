using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public interface IPolygonSymbol: IFeatureSymbol,IOutlineSymbol
    {
        PolygonSymbolType PolygonSymbolType { get; }
        RectangleF Bounds { get; set; }
        void Draw(Image<Rgba32> image, PointF[] polygon, float scale);
        IBrush<Rgba32> ToBrush();
    }
}
