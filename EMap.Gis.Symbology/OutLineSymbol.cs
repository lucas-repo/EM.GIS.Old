using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace EMap.Gis.Symbology
{
    public abstract class OutLineSymbol : FeatureSymbol, IOutlineSymbol
    {
        public bool UseOutLine { get; set; } = true;
        public ILineSymbolizer OutLineSymbolizer { get; set; }
        public OutLineSymbol()
        {
            OutLineSymbolizer = new LineSymbolizer();
        }

        public void CopyOutLine(IOutlineSymbol outlineSymbol)
        {
            UseOutLine = outlineSymbol.UseOutLine;
            OutLineSymbolizer = outlineSymbol.OutLineSymbolizer.Clone() as ILineSymbolizer;
        }

        public void DrawOutLine(IImageProcessingContext<Rgba32> context, float scale, PointF[] points)
        {
            if (UseOutLine && OutLineSymbolizer != null && points.Length > 1)
            {
                OutLineSymbolizer.DrawLine(context, scale, points);
            }
        }
    }
}