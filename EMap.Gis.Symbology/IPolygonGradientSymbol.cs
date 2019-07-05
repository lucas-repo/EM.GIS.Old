using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public interface IPolygonGradientSymbol: IPolygonSymbol
    {
        double Angle { get; set; }
        Rgba32[] Colors { get; set; }

        GradientType GradientType { get; set; }
        float[] Positions { get; set; }
    }
}