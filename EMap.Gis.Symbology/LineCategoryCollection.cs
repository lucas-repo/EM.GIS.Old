using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public class LineCategoryCollection : FeatureCategoryCollection, ILineCategoryCollection
    {
        public new ILineCategory this[int index] { get => base[index] as ILineCategory; set => base[index] = value; }

        public new ILineScheme Parent { get => base.Parent as ILineScheme; set => base.Parent = value; }

        public LineCategoryCollection()
        { }
        public LineCategoryCollection(ILineScheme parent) : base(parent)
        { }
        public new IEnumerator<ILineCategory> GetEnumerator()
        {
            foreach (var item in Items)
            {
                yield return item as ILineCategory;
            }
        }
    }
}