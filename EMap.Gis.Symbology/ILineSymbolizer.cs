using System.Drawing;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public interface ILineSymbolizer:IFeatureSymbolizer
    {
        Color Color { get; set; }
        float Width { get; set; }
        new ILineSymbolCollection Symbols { get; set; }
        void DrawLine(Graphics graphics, float scale, GraphicsPath path);
    }
}
