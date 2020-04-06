using System.Drawing;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public interface IPolygonSymbolizer:IFeatureSymbolizer
    {
        new IPolygonSymbolCollection Symbols { get; set; }
        void DrawPolygon(Graphics context, float scale, GraphicsPath path);
    }
}
