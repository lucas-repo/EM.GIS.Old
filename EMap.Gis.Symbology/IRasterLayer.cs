using OSGeo.GDAL;

namespace EMap.Gis.Symbology
{
    public interface IRasterLayer:IBaseLayer
    {
        new IRasterScheme Symbology { get; set; }
        Dataset Dataset { get; set; }
        double[] Affine { get; set; }
         int RasterXSize { get; }
         int RasterYSize { get;  }
    }
}
