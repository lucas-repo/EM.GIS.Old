
using System.Drawing;
using System.Drawing.Drawing2D;

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

        public void DrawOutLine(Graphics graphics, float scale, GraphicsPath path)
        {
            if (UseOutLine && OutLineSymbolizer != null && path!=null)
            {
                OutLineSymbolizer.DrawLine(graphics, scale, path);
            }
        }
    }
}