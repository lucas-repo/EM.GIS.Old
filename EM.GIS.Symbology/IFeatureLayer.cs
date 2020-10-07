using EM.GIS.Data;
using OSGeo.OGR;
using System.Collections.Generic;
using System.Threading;

namespace EM.GIS.Symbology
{
    public interface IFeatureLayer: ILayer
    {
        new IFeatureScheme Symbology { get; set; }
        new IFeatureCategory DefaultCategory { get; set; }
        new IFeatureSet DataSet { get; set; }
        DataSource DataSource { get; set; }
        ISelection Selection { get; }
        Layer Layer { get;  }
        ILabelLayer LabelLayer { get; set; }
    }
}
