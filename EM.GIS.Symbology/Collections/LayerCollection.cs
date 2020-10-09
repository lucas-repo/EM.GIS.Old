﻿using EM.GIS.Data;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EM.GIS.Symbology
{
    public class LayerCollection : ItemCollection<IGroup, ILayer>, ILayerCollection
    {
        ILayer IList<ILayer>.this[int index] { get => this[index] as ILayer; set => this[index] = value; }

        public IProgressHandler ProgressHandler { get; set; }
        IGroup IParentItem<IGroup>.Parent { get => Parent as IGroup; set => Parent = value; }

        public ILayer Add(IDataSet dataSet)
        {
            ILayer layer = null;
            //var ss = dataSet as ISelfLoadSet;
            //if (ss != null) return Add(ss);

            if (dataSet is IFeatureSet fs)
            {
                layer = Add(fs);
            }
            else if (dataSet is IRasterSet r)
            {
                layer = Add(r);
            }
            return layer;

            //var id = dataSet as IImageData;
            //return id != null ? Add(id) : null;
        }

        public IFeatureLayer Add(IFeatureSet featureSet)
        {
            IFeatureLayer res = null;
            if (featureSet == null) return null;

            featureSet.ProgressHandler = ProgressHandler;
            if (featureSet.FeatureType == FeatureType.Point || featureSet.FeatureType == FeatureType.MultiPoint)
            {
                res = new PointLayer(featureSet);
            }
            else if (featureSet.FeatureType == FeatureType.Line)
            {
                res = new LineLayer(featureSet);
            }
            else if (featureSet.FeatureType == FeatureType.Polygon)
            {
                res = new PolygonLayer(featureSet);
            }

            if (res != null)
            {
                base.Add(res);
                res.ProgressHandler = ProgressHandler;
            }

            return res;
        }

        public IRasterLayer Add(IRasterSet raster)
        {
            IRasterLayer rasterLayer = null;
            if (raster != null)
            {
                raster.ProgressHandler = ProgressHandler;
                rasterLayer = new RasterLayer(raster);
                Add(rasterLayer);
            }
            return rasterLayer;
        }

        public ILayer AddLayer(string path)
        {
            IDataSet dataSet = DataManager.Default.Open(path);
            return Add(dataSet);
        }

        IEnumerator<ILayer> IEnumerable<ILayer>.GetEnumerator()
        {
            foreach (var item in this)
            {
                yield return item as ILayer;
            }
        }
    }
}