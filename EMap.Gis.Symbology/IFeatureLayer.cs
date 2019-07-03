using OSGeo.OGR;

namespace EMap.Gis.Symbology
{
    public interface IFeatureLayer:IBaseLayer
    {
        new IFeatureScheme Symbology { get; set; }
        DataSource DataSource { get; set; }
        Layer Layer { get;  }
    }
}
