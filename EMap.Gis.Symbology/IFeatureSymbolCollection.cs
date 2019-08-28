using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface IFeatureSymbolCollection: ISymbolCollection
    {
        new IFeatureSymbol this[int index] { get;set; }
        new IFeatureSymbolizer Parent { get; set; }
        new IEnumerator<IFeatureSymbol> GetEnumerator();
    }
}