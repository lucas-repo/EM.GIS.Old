using System.Collections.Generic;

namespace EM.GIS.Symbology
{
    public abstract class FeatureCategoryCollection : CategoryCollection, IFeatureCategoryCollection
    {
        public new IFeatureCategory this[int index] { get => base[index] as IFeatureCategory; set => base[index] = value; }

        public new IFeatureScheme Parent { get => base.Parent as IFeatureScheme; set => base.Parent = value; }

        public FeatureCategoryCollection()
        { }
        public FeatureCategoryCollection(IFeatureScheme parent) : base(parent)
        { }
        public new IEnumerator<IFeatureCategory> GetEnumerator()
        {
            foreach (var item in Items)
            {
                yield return item as IFeatureCategory;
            }
        }
    }
}