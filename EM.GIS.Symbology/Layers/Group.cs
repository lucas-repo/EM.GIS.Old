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

        public IEnumerable<ILayer> GetLayers(Func<ILayer, bool> func = null)
        {
            if (func == null)
            {
                foreach (ILayer item in Items)
                {
                    yield return item;
                }
            }
            else
            {
                foreach (ILayer item in Items)
                {
                    if (func(item))
                    {
                        yield return item;
                    }
                }
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
            throw new NotImplementedException();
        }

        public void AddLayer(ILayer layer, int? index = null)
        {
            throw new NotImplementedException();
        }

        public ILayer AddLayer(string filename, int? index = null)
        {
            throw new NotImplementedException();
        }

        public ILayer AddLayer(IDataSet dataSet, int? index = null)
        {
            throw new NotImplementedException();
        }

        public IFeatureLayer AddLayer(IFeatureSet dataSet, int? index = null)
        {
            throw new NotImplementedException();
        }

        public IRasterLayer AddLayer(IRasterSet dataSet, int? index = null)
        {
            throw new NotImplementedException();
        }

        public override IExtent Extent
        {
            get
            {
                IExtent extent = new Extent();
                for (int i = 0; i < Layers.Count; i++)
                {
                    var layer = Layers[i];
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
                }
                return extent;
            }
        }

    }
}
