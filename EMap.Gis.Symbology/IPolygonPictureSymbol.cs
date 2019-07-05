using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public interface IPolygonPictureSymbol:IPolygonSymbol
    {
        float Angle { get; set; }
        Image<Rgba32> Picture { get; set; }
        PointF Scale { get; set; }
        
    }
}