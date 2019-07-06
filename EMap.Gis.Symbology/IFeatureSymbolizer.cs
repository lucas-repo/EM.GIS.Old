using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    public interface IFeatureSymbolizer:ISymbolizer
    {
        IFeatureSymbol FeatureSymbol { get; set; }
        ScaleMode ScaleMode { get; set; }
    }
}
