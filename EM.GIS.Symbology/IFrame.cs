using EM.GIS.Data;
using EM.GIS.Geometries;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace EM.GIS.Symbology
{
    public interface IFrame : IGroup
    {
        ILayerCollection DrawingLayers { get; }
        IExtent ViewExtents { get; set; }
    }
}