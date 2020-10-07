using EM.GIS.Data;
using EM.GIS.Geometries;
using EM.GIS.Projection;
using OSGeo.OGR;
using OSGeo.OSR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EM.GIS.Gdals
{
    public class GdalFeatureSet : FeatureSet
    {
        private DataSource _dataSource;

        public DataSource DataSource
        {
            get { return _dataSource; }
            private set 
            {
                if (_dataSource != null)
                {
                    _dataSource.Dispose();
                }
                _dataSource = value;
                if (_dataSource?.GetLayerCount() > 0)
                {
                    Layer = _dataSource.GetLayerByIndex(0);
                }
                else
                {
                    Layer = null;
                }
            }
        }

        private Layer _layer;
        public Layer Layer 
        {
            get { return _layer; }
            private set
            {
                if (_layer != value)
                {
                    if (_layer != null)
                    {
                        _layer.Dispose();
                    }
                    _layer = value;
                    FeatureDefn = _layer?.GetLayerDefn();
                }
            }
        }
        private FeatureDefn _featureDefn;

        public FeatureDefn FeatureDefn=> Layer.GetLayerDefn();

        public override IExtent Extent => Layer.GetExtent();
        public override string Filename { get => DataSource.; set => base.Filename = value; }
        public override ProjectionInfo Projection 
        {
            get
            {
                SpatialReference spatialReference = null;
                if (Layer != null)
                {
                    spatialReference = Layer.GetSpatialRef();
                }
                if (base.Projection == null)
                {
                    base.Projection = new GdalProjectionInfo(spatialReference);
                }
                else if (base.Projection is GdalProjectionInfo gdalProjectionInfo)
                {
                    if (!gdalProjectionInfo.SpatialReference.Equals(spatialReference))//待测试
                    {
                        gdalProjectionInfo.SpatialReference = spatialReference;
                    }
                }
                return base.Projection; ;
            }
            set
            {
                if (base.Projection != null)
                {
                    base.Projection.Dispose();
                }
                base.Projection = value;
            }
        }
        public override int FeatureCount => (int)Layer.GetFeatureCount(1);

        public override IGeometry SpatialFilter
        {
            get => Layer.GetSpatialFilter()?.ToGeometry();
            set
            {
                if (Layer != null)
                {
                    Layer.SetSpatialFilter(value?.ToGeometry());
                }
            }
        }
        private string _attributeFilter;
        public override string AttributeFilter 
        { 
            get => _attributeFilter;
            set
            {
                if (Layer != null)
                {
                    Layer.SetAttributeFilter(value);
                    _attributeFilter = value;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (FeatureDefn != null)
                    {
                        FeatureDefn.Dispose();
                        FeatureDefn = null;
                    }
                    if (_layer != null)
                    {
                        _layer.Dispose();
                        _layer = null;
                    }
                    if (DataSource != null)
                    {
                        DataSource.Dispose();
                        DataSource = null;
                    }
                }
            }
            base.Dispose(disposing);
        }
        public override void Save()
        {
            DataSource.FlushCache();
        }
        public override void SaveAs(string filename, bool overwrite)
        {
            if (string.IsNullOrEmpty(filename) || (File.Exists(filename) && !overwrite))
            {
                return;
            }
            using var driver = DataSource.GetDriver();
            using var ds = driver.CopyDataSource(DataSource, filename, null);
        }
        public override IFeature AddFeature(IGeometry geometry)
        {
            IFeature destFeature = AddFeature(geometry, null);
            return destFeature;
        }

        public override IFeature AddFeature(IGeometry geometry, Dictionary<string, object> attribute)
        {
            IFeature destFeature = null;
            if (geometry != null)
            {
                return destFeature;
            }
            var fieldCount = FeatureDefn.GetFieldCount();
            FeatureDefn featureDefn = new FeatureDefn(null);
            var destAttribute = new Dictionary<FieldDefn, object>();
            for (int i = 0; i < fieldCount; i++)
            {
                var fieldDefn = FeatureDefn.GetFieldDefn(i);
                featureDefn.AddFieldDefn(fieldDefn);
                var fieldName = fieldDefn.GetName();
                if (attribute.ContainsKey(fieldName))
                {
                    destAttribute.Add(fieldDefn, attribute[fieldName]);
                }
                else
                {
                    fieldDefn.Dispose();
                }
            }
            using GeomFieldDefn geomFieldDefn = new GeomFieldDefn(null, FeatureDefn.GetGeomType());
            featureDefn.AddGeomFieldDefn(geomFieldDefn);
            Feature feature = new Feature(featureDefn);
            foreach (var item in destAttribute)
            {
                var fieldDefn = item.Key;
                feature.SetField(fieldDefn, item.Value);
                fieldDefn.Dispose();
            }
            var ret = Layer.CreateFeature(feature);
            if (ret == 1)
            {
                destFeature = new GdalFeature(feature);
            }
            return destFeature;
        }

        public override IFeature GetFeature(int index)
        {
            var feature = Layer.GetFeature(index)?.ToFeature();
            return feature;
        }

        public override bool RemoveFeature(int index)
        {
            return Layer.DeleteFeature(index) == 1;
        }

        public override IEnumerable<IFeature> GetFeatures()
        {
            var feature = Layer.GetNextFeature()?.ToFeature();
            while (feature != null)
            {
                yield return feature;
                feature = Layer.GetNextFeature()?.ToFeature();
            }
        }
    }
}
