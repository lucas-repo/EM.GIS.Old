
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public interface IPolygonPictureSymbol:IPolygonSymbol
    {
        float Angle { get; set; }
        Image Picture { get; set; }
        PointF Scale { get; set; }
         WrapMode WrapMode { get; set; }

    }
}