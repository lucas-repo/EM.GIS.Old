using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface IPointSymbolCollection:IFeatureSymbolCollection 
    {
        new IPointSymbol this[int index] { get; set; }
        new IPointSymbolizer Parent { get; set; }
        new IEnumerator<IPointSymbol> GetEnumerator();
    }
}