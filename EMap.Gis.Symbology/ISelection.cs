using EMap.Gis.Data;
using OSGeo.OGR;
using System;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface ISelection:IDisposable
    {
        Extent Extent { get; }
        List<Feature> Features { get; }
    }
}