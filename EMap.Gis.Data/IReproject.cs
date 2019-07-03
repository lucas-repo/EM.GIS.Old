using OSGeo.OSR;
using System;

namespace EMap.Gis.Data
{
    public interface IReproject:IDisposable
    {
        bool CanReproject { get; }
        SpatialReference SpatialReference { get; set; }
        void Reproject(SpatialReference destSpatialReference);
    }
}