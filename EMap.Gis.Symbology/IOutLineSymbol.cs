using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Shapes;

namespace EMap.Gis.Symbology
{
    public interface IOutlineSymbol:IFeatureSymbol
    {
        bool UseOutLine { get; set; }
        ILineSymbolizer OutLineSymbolizer { get; set; }
        void DrawOutLine(Image<Rgba32> image, IPath path, double scaleWidth);
        void CopyOutLine(IOutlineSymbol outlineSymbol);
    }
}