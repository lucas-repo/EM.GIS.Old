using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Data
{
    public interface IDataSet: IDisposable, IReproject
    {
        Envelope Envelope { get; set; }
        string Filename { get; set; }
        string Name { get; set; }
    }
}
