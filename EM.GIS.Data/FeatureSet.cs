using EM.GIS.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.GIS.Data
{
    public abstract class FeatureSet : DataSet, IFeatureSet
    {
        public FeatureType FeatureType { get;protected set; }
        public abstract int FeatureCount { get; }
        public abstract IGeometry SpatialFilter { get; set; }
        public abstract string AttributeFilter { get; set; }

        public abstract IFeature AddFeature(IGeometry geometry);
        public abstract IFeature AddFeature(IGeometry geometry, Dictionary<string, object> attribute);
        public abstract IFeature GetFeature(int index);
        public abstract IEnumerable<IFeature> GetFeatures();
        public abstract bool RemoveFeature(int index);
    }
}
