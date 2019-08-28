using OSGeo.OGR;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface IFrame: IGroup
    {
        List<IBaseLayer> DrawingLayers { get; set; }
        Envelope ViewExtents { get; set; }
    }
}