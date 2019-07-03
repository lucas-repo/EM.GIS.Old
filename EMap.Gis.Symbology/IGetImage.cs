using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public interface IGetImage
    {
        Image<Rgba32> GetImage(Envelope envelope, Rectangle rectangle) ;
    }
}