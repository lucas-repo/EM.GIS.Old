using System.Drawing;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public interface IOutlineSymbol:IFeatureSymbol
    {
        bool UseOutLine { get; set; }
        ILineSymbolizer OutLineSymbolizer { get; set; }
        void CopyOutLine(IOutlineSymbol outlineSymbol);
        void DrawOutLine(Graphics graphics, float scale, GraphicsPath path);
    }
}