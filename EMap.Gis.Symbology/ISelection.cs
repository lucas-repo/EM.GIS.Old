using OSGeo.OGR;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface ISelection:IDisposable
    {
        Envelope Envelope { get; }
        List<Feature> Features { get; }
    }
}