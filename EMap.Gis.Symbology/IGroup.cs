using System.Drawing;

namespace EMap.Gis.Symbology
{
    public interface IGroup : IBaseLayer
    {
        Image Icon { get; set; }
        new ILayerCollection Items { get; set; }
        new IGroup Parent { get; set; }
    }
}