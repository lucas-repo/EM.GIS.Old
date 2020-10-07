using System.Collections.Generic;

namespace EM.GIS.Symbology
{
    public interface IPolygonCategoryCollection : IFeatureCategoryCollection
    {
        new IPolygonCategory this[int index] { get; set; }
        new IPolygonScheme Parent { get; set; }
        new IEnumerator<IPolygonCategory> GetEnumerator();
    }
}