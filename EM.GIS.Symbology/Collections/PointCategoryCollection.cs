using System.Collections.Generic;

namespace EM.GIS.Symbology
{
    public class PointCategoryCollection : FeatureCategoryCollection,  IPointCategoryCollection
    {
        public new IPointCategory this[int index] { get => base[index] as IPointCategory; set => base[index] = value; }

        public new IPointScheme Parent { get => base.Parent as IPointScheme; set => base.Parent = value; }

        public PointCategoryCollection()
        { }
        public PointCategoryCollection(IPointScheme parent) : base(parent)
        { }
        public new IEnumerator<IPointCategory> GetEnumerator()
        {
            foreach (var item in Items)
            {
                yield return item as IPointCategory;
            }
        }
    }
}