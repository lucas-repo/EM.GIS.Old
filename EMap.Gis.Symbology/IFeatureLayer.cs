using OSGeo.OGR;
using System.Collections.Generic;
using System.Threading;

namespace EMap.Gis.Symbology
{
    public interface IFeatureLayer: IBaseLayer
    {
        new IFeatureScheme Symbology { get; set; }
        new IFeatureCategory DefaultCategory { get; set; }
        DataSource DataSource { get; set; }
        ISelection Selection { get; }
        Layer Layer { get;  }
    }
}
