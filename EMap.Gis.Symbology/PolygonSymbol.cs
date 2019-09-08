using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace EMap.Gis.Symbology
{
    public abstract class PolygonSymbol : OutLineSymbol, IPolygonSymbol
    {
        public PolygonSymbolType PolygonSymbolType { get; }

        public RectangleF Bounds { get; set; }
        public PolygonSymbol(PolygonSymbolType polygonSymbolType)
        {
            PolygonSymbolType = polygonSymbolType;
        }

        public void DrawPolygon(Graphics graphics, float scale, GraphicsPath path)
        {
            if (graphics == null || path == null )
            {
                return;
            }
            using (Brush brush = GetBrush())
            {
                if (brush != null)
                {
                    graphics.FillPath(brush, path);
                }
            }
            DrawOutLine(graphics, scale, path);
        }

        public virtual Brush GetBrush()
        {
            Brush brush = new SolidBrush(Color);
            return brush;
        }
    }
}
