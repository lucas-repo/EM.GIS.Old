using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace EMap.Gis.Symbology
{
    public interface IOutlineSymbol:IFeatureSymbol
    {
        bool UseOutLine { get; set; }
        ILineSymbolizer OutLineSymbolizer { get; set; }
        void DrawPath(Image<Rgba32> image, float scale, PointF[] points);
        void CopyOutLine(IOutlineSymbol outlineSymbol);
    }
}