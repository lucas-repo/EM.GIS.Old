using System.Collections.Generic;

namespace EM.GIS.Symbology
{
    public class PolygonCategoryCollection :FeatureCategoryCollection, IPolygonCategoryCollection
    {

        public new IPolygonCategory this[int index] { get => base[index] as IPolygonCategory; set => base[index] = value; }

        public new IPolygonScheme Parent { get => base.Parent as IPolygonScheme; set => base.Parent = value; }

        public PolygonCategoryCollection()
        { }
        public PolygonCategoryCollection(IPolygonScheme parent) : base(parent)
        { }
        public new IEnumerator<IPolygonCategory> GetEnumerator()
        {
            foreach (var item in Items)
            {
                yield return item as IPolygonCategory;
            }
        }
    }
}