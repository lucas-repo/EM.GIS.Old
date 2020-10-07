using EM.GIS.Data;
using OSGeo.GDAL;

namespace EM.GIS.Symbology
{
    public interface IRasterLayer : ILayer
    {
        new IRasterScheme Symbology { get; set; }
        new IRasterSet DataSet { get; set; }
    }
}
