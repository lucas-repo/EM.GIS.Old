using System;
using System.Collections.Generic;
using System.Text;

namespace IMap.Gis.Symbology
{
    public interface IFeatureSymbolizer:ISymbolizer
    {
        IFeatureSymbol FeatureSymbol { get; set; }
    }
}
