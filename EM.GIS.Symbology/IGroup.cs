using System.Drawing;

namespace EM.GIS.Symbology
{
    public interface IGroup : ILayer
    {
        ILayerCollection Layers { get; }
    }
}