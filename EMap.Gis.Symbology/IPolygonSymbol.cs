
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public interface IPolygonSymbol: IFeatureSymbol,IOutlineSymbol
    {
        PolygonSymbolType PolygonSymbolType { get; }
        RectangleF Bounds { get; set; }
        void DrawPolygon(Graphics graphics, float scale, GraphicsPath path);
        Brush GetBrush();
    }
}
