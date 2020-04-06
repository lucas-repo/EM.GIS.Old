using EMap.Gis.Data;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public interface IFrame : IGroup
    {
        List<IBaseLayer> DrawingLayers { get; set; }
        Extent ViewExtents { get; set; }
    }
}