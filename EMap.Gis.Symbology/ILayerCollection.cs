using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    public interface ILayerCollection:ILegendItemCollection
    {
        IFrame MapFrame { get; set; }

        new IBaseLayer this[int index] { get; set; }
        new IGroup Parent { get; set; }
        new IEnumerator<IBaseLayer> GetEnumerator();
    }
}
