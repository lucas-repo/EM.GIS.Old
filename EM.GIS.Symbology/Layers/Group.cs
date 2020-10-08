using EM.GIS.Data;
using EM.GIS.Geometries;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;


namespace EM.GIS.Symbology
{
    public class Group : Layer, IGroup
    {
        public int LayerCount => GetLayers().Count();
        public Group()
        {
        }
        protected override void OnDraw(Graphics graphics, Rectangle rectangle, IExtent extent, bool selected = false, CancellationTokenSource cancellationTokenSource = null)
        {
            var visibleLayers = GetLayers().Where(x => x.GetVisible(extent, rectangle));
            foreach (var layer in visibleLayers)
            {
                layer?.Draw(graphics, rectangle, extent, selected, cancellationTokenSource);
            }
        }

        public ILayer GetLayer(int index)
        {
            ILayer layer = null;
            if (index >= 0 && index < LayerCount)
            {
                layer = GetLayers().ElementAt(index);
            }
            return layer;
        }

        public IEnumerable<ILayer> GetLayers()
        {
            foreach (ILayer item in Items)
            {
                yield return item;
            }
        }

        public IEnumerable<IFeatureLayer> GetAllFeatureLayers()
        {
            return GetAllLayers<IFeatureLayer>(GetLayers());
        }
        private IEnumerable<T> GetAllLayers<T>(IEnumerable<ILayer> layers) where T : ILayer
        {
            foreach (var layer in layers)
            {
                if (layer is IGroup group)
                {
                    foreach (var item in GetAllLayers<T>(group.GetLayers()))
                    {
                        yield return item;
                    }
                }
                else if(layer is T t)
                {
                    yield return t;
                }
            }
        }
        public IEnumerable<IRasterLayer> GetAllRasterLayers()
        {
            return GetAllLayers<IRasterLayer>(GetLayers());
        }

        public bool AddLayer(ILayer layer, int? index = null)
        {
            bool ret = false;
            if (layer == null)
            {
                return ret;
            }
            if (index.HasValue)
            {
                if (index.Value < 0 || index.Value > Items.Count)
                {
                    return ret;
                }
                Items.Insert(index.Value, layer);
            }
            else
            {
                Items.Add(layer);
            }
            if (layer.ProgressHandler == null && ProgressHandler != null)
            {
                layer.ProgressHandler = ProgressHandler;
            }
            ret = true;
            return ret;
        }

        public ILayer AddLayer(string filename, int? index = null)
        {
            IDataSet dataSet = DataManager.Default.Open(filename);
            return AddLayer(dataSet, index);
        }

        public ILayer AddLayer(IDataSet dataSet, int? index = null)
        {
            ILayer layer = null;
            if (dataSet is IFeatureSet featureSet)
            {
                layer = AddLayer(featureSet, index);
            }
            else if (dataSet is IRasterSet rasterSet)
            {
                layer = AddLayer(rasterSet, index);
            }
            return layer;
        }

        public IFeatureLayer AddLayer(IFeatureSet featureSet, int? index = null)
        {
            IFeatureLayer featureLayer = null;
            if (featureSet == null) return null;

            featureSet.ProgressHandler = ProgressHandler;
            if (featureSet.FeatureType == FeatureType.Point || featureSet.FeatureType == FeatureType.MultiPoint)
            {
                featureLayer = new PointLayer(featureSet);
            }
            else if (featureSet.FeatureType == FeatureType.Line)
            {
                featureLayer = new LineLayer(featureSet);
            }
            else if (featureSet.FeatureType == FeatureType.Polygon)
            {
                featureLayer = new PolygonLayer(featureSet);
            }

            if (featureLayer != null)
            {
                if (AddLayer(featureLayer, index))
                {
                    if (featureSet.ProgressHandler == null && ProgressHandler != null)
                    {
                        featureSet.ProgressHandler = ProgressHandler;
                    }
                }
            }
            return featureLayer;
        }

        public IRasterLayer AddLayer(IRasterSet rasterSet, int? index = null)
        {
            IRasterLayer rasterLayer = null;
            if (rasterSet != null)
            {
                rasterSet.ProgressHandler = ProgressHandler;
                rasterLayer = new RasterLayer(rasterSet);
                if (AddLayer(rasterLayer, index))
                {
                    if (rasterSet.ProgressHandler == null && ProgressHandler != null)
                    {
                        rasterSet.ProgressHandler = ProgressHandler;
                    }
                }
            }
            return rasterLayer;
        }

        public IEnumerable<ILayer> GetAllLayers()
        {
            return GetAllLayers<ILayer>(GetLayers());
        }

        public override IExtent Extent
        {
            get
            {
                IExtent extent = new Extent();
                int i = 0;
                foreach (var layer in GetAllLayers())
                {
                    if (i == 0)
                    {
                        extent.MinX = layer.Extent.MinX;
                        extent.MinY = layer.Extent.MinY;
                        extent.MaxX = layer.Extent.MaxX;
                        extent.MaxY = layer.Extent.MaxY;
                    }
                    else
                    {
                        extent.MinX = Math.Min(extent.MinX, layer.Extent.MinX);
                        extent.MinY = Math.Min(extent.MinY, layer.Extent.MinY);
                        extent.MaxX = Math.Max(extent.MaxX, layer.Extent.MaxX);
                        extent.MaxY = Math.Max(extent.MaxY, layer.Extent.MaxY);
                    }
                    i++;
                }
                return extent;
            }
        }

    }
}
