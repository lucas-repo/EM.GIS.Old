using System;
using System.Collections.Generic;
using System.Text;

namespace IMap.Gis.Symbology
{
    public interface ILineSymbolizer:IFeatureSymbolizer
    {
        IList<ILineSymbol> Strokes { get; set; }
    }
}
