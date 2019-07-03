using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace EMap.Gis.Symbology
{
    public interface IPointPictureSymbol:IPointSymbol
    {
        Image<Rgba32> Image { get; set; }
        string ImageBase64 { get; set; }
        string ImagePath { get; set; }
    }
}