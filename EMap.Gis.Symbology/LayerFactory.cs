using OSGeo.GDAL;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EMap.Gis.Symbology
{
    public class LayerFactory : ILayerFactory
    {
        public virtual IBaseLayer OpenLayer(string dataPath)
        {
            IBaseLayer baseLayer = null;
            Dataset dataset = OpenDataset(dataPath);
            if (dataset != null)
            {
                baseLayer = new RasterLayer(dataset);
            }
            else
            {
                DataSource dataSource = OpenDataSource(dataPath);
                if (dataSource != null)
                {
                    if (dataSource.GetLayerCount() > 0)
                    {
                        wkbGeometryType wkbGeometryType = wkbGeometryType.wkbNone;
                        using (Layer layer = dataSource.GetLayerByIndex(0))
                        {
                            wkbGeometryType = layer.GetGeomType();
                        }
                        switch (wkbGeometryType)
                        {
                            case wkbGeometryType.wkbPoint:
                                baseLayer = new PointLayer(dataSource);
                                break;
                            case wkbGeometryType.wkbLineString:
                                baseLayer = new LineLayer(dataSource);
                                break;
                            case wkbGeometryType.wkbPolygon:
                                baseLayer = new PolygonLayer(dataSource);
                                break;
                        }
                    }
                }
            }
            return baseLayer;
        }
        public static Dataset OpenDataset(string dataPath, int update = 0)
        {
            Dataset dataset = null;
            try
            {
                dataset = Gdal.Open(dataPath, (Access)update); 
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            return dataset;
        }
        public static DataSource OpenDataSource(string dataPath,int update=0)
        {
            DataSource dataSource = null;
            try
            {
                dataSource = Ogr.Open(dataPath, update);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
            return dataSource;
        }
    }
}
