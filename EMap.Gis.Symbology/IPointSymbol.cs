using System.Drawing;

namespace EMap.Gis.Symbology
{
    public interface IPointSymbol: IFeatureSymbol,IOutlineSymbol
    {
        float Angle { get; set; }
        PointF Offset { get; set; }
        PointSymbolType PointSymbolType { get; }
        void DrawPoint(Graphics graphics, float scale, PointF point);
        SizeF Size { get; set; }
    }
}
