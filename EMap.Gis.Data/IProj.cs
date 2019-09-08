using OSGeo.OGR;
using System.Drawing;

namespace EMap.Gis.Data
{
    public interface IProj
    {
        Envelope Envelope { get; }
        Rectangle Rectangle { get; }
    }
}
