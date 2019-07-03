using SixLabors.Primitives;
using System;
using System.Collections.Generic;

using System.Text;

namespace EMap.Gis.Symbology
{
    public interface IPointSymbolizer:IFeatureSymbolizer
    {
        IList<IPointSymbol> Symbols { get; set; }
        SizeF GetSize();
    }
}
