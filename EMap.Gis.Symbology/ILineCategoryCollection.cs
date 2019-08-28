using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface ILineCategoryCollection : IFeatureCategoryCollection
    {
        new ILineCategory this[int index] { get; set; }
        new ILineScheme Parent { get; set; }
        new IEnumerator<ILineCategory> GetEnumerator();
    }
}