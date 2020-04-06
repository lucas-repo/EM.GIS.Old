using System.Drawing;

namespace EMap.Gis.Symbology
{
    public interface IPolygonGradientSymbol: IPolygonSymbol
    {
        double Angle { get; set; }
        Color[] Colors { get; set; }

        GradientType GradientType { get; set; }
        float[] Positions { get; set; }
    }
}