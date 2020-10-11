using EM.GIS.Data;
using EM.GIS.Geometries;
using System;
using System.Collections.Generic;

namespace EM.GIS.Symbology
{
    public interface ISelection:IDisposable
    {
        IExtent IExtent { get; }
        List<IFeature> Features { get; }
    }
}