using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    public interface IPointCategoryCollection: IFeatureCategoryCollection
    {
        new IPointCategory this[int index] { get; set; }
        new IPointScheme Parent { get; set; }
        new IEnumerator<IPointCategory> GetEnumerator();
    }
}
