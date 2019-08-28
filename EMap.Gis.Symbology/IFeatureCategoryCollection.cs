using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface IFeatureCategoryCollection:ICategoryCollection
    {
        new IFeatureCategory this[int index] { get; set; }
        new IFeatureScheme Parent { get; set; }
        new IEnumerator<IFeatureCategory> GetEnumerator();
    }
}