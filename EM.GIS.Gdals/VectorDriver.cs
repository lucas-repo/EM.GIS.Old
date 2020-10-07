using EM.GIS.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EM.GIS.Gdals
{
    /// <summary>
    /// 矢量数据驱动类
    /// </summary>
    public class VectorDriver : Driver, IVectorDriver
    {
        public override bool CopyFiles(string srcFileName, string destFileName)
        {
            bool ret = false;
            using var ds = OSGeo.OGR.Ogr.Open(srcFileName, 0);
            if (ds != null)
            {
                using var driver = ds.GetDriver();
                using var destDs = driver.CopyDataSource(ds, destFileName, null);
                ret = destDs != null;
            }
            return ret;
        }

        IFeatureSet IVectorDriver.Open(string fileName, bool update)
        {
            IFeatureSet featureSet = null;
            var ds = OSGeo.OGR.Ogr.Open(fileName, 0);
            if (ds != null)
            {
                using var driver = ds.GetDriver();
                switch (driver.name)
                {
                    case "ESRI Shapefile":
                        if (ds.GetLayerCount() > 0)
                        {
                            var layer = ds.GetLayerByIndex(0);
                            switch (layer.GetGeomType())
                            {
                                case OSGeo.OGR.wkbGeometryType.wkbPoint:
                                    featureSet = new PointShapeFile();
                                    break;
                                case OSGeo.OGR.wkbGeometryType.wkbLineString:
                                    break;
                                case OSGeo.OGR.wkbGeometryType.wkbPolygon:
                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            return featureSet;
        }
        public override IDataSet Open(string fileName, bool update)
        {
            return (this as IVectorDriver).Open(fileName, update);
        }
        public override bool Delete(string fileName)
        {
            bool ret = false;
            using var ds = OSGeo.OGR.Ogr.Open(fileName, 0);
            if (ds != null)
            {
                using var driver = ds.GetDriver();
                ret = driver.DeleteDataSource(fileName) == 1;
            }
            return ret;
        }
        public override bool Rename(string srcFileName, string destFileName)
        {
            bool ret = false;
            using var ds = OSGeo.OGR.Ogr.Open(srcFileName, 0);
            if (ds != null)
            {
                using var driver = ds.GetDriver();
                using var destDs = driver.CopyDataSource(ds, destFileName, null);
                if (destDs != null)
                {
                    ret = driver.DeleteDataSource(srcFileName) == 1;
                }
            }
            return ret;
        }
    }
}
