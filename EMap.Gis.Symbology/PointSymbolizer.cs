using System.Collections.Generic;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class PointSymbolizer : FeatureSymbolizer, IPointSymbolizer
    {
        public IList<IPointSymbol> Symbols { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public SizeF GetSize()
        {
            throw new System.NotImplementedException();
        }
    }
}