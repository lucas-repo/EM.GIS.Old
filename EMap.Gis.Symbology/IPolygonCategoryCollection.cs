using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface IPolygonCategoryCollection : IFeatureCategoryCollection
    {
        new IPolygonCategory this[int index] { get; set; }
        new IPolygonScheme Parent { get; set; }
        new IEnumerator<IPolygonCategory> GetEnumerator();
    }
}