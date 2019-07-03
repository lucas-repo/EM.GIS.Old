using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Shapes;

namespace EMap.Gis.Symbology
{
    public abstract class OutLineSymbol : FeatureSymbol, IOutlineSymbol
    {
        public bool UseOutLine { get; set; }
        public ILineSymbolizer OutLineSymbolizer { get; set; }

        public void CopyOutLine(IOutlineSymbol outlineSymbol)
        {
            UseOutLine = outlineSymbol.UseOutLine;
            OutLineSymbolizer = outlineSymbol.OutLineSymbolizer.Clone() as ILineSymbolizer;
        }

        public void DrawOutLine(Image<Rgba32> image, IPath path, double scale)
        {
            if (UseOutLine && OutLineSymbolizer != null)
            {
                OutLineSymbolizer.DrawPath(image, path, scale);
            }
        }
    }
}