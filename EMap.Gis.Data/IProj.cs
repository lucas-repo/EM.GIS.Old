using OSGeo.OGR;
using SixLabors.Primitives;

namespace EMap.Gis.Data
{
    public interface IProj
    {
        Envelope Envelope { get; }
        Rectangle Rectangle { get; }
    }
}
