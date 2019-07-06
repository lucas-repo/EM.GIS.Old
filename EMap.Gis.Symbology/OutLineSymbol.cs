using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
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

        public void DrawPath(Image<Rgba32> image, float scale, PointF[] points)
        {
            if (UseOutLine && OutLineSymbolizer != null)
            {
                OutLineSymbolizer.DrawPath(image,  scale, points);
            }
        }
    }
}