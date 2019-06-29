using System;
using System.Collections.Generic;
using System.Text;

namespace IMap.Gis.Symbology
{
    public interface IPolygonSymbolizer:IFeatureSymbolizer
    {
        IList<IPolygonSymbol> Patterns { get; set; }
    }
}
