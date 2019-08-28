using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface ICategoryCollection : ILegendItemCollection
    {
        new ICategory this[int index] { get; set; }
        new IScheme Parent { get; set; }
        new IEnumerator<ICategory> GetEnumerator();
    }
}