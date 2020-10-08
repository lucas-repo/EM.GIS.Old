using EM.GIS.Data;
using OSGeo.OGR;
using System;
using System.Collections.Generic;

namespace EM.GIS.Symbology
{
    public interface ISelection:IDisposable
    {
        IExtent IExtent { get; }
        List<Feature> Features { get; }
    }
}