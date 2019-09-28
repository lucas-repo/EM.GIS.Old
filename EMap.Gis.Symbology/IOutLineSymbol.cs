using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace EMap.Gis.Symbology
{
    public interface IOutlineSymbol:IFeatureSymbol
    {
        bool UseOutLine { get; set; }
        ILineSymbolizer OutLineSymbolizer { get; set; }
        void CopyOutLine(IOutlineSymbol outlineSymbol);
        void DrawOutLine(IImageProcessingContext context, float scale,IPath path);
    }
}