using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface IGroup : IBaseLayer
    {
        Image<Rgba32> Icon { get; set; }
        new ILayerCollection Items { get; set; }
        new IGroup Parent { get; set; }
    }
}