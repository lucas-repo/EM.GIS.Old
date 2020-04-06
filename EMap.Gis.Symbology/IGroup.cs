using System.Drawing;

namespace EMap.Gis.Symbology
{
    public interface IGroup : IBaseLayer
    {
        ILayerCollection Layers { get; }
    }
}