using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace EM.GIS.Symbology
{
    public class CategoryCollection : LegendItemCollection, ICategoryCollection
    {
       public new ICategory this[int index] { get => base[index] as ICategory; set => base[index]=value; }

        public new IScheme Parent { get => base.Parent as IScheme; set => base.Parent = value; }

        public CategoryCollection()
        { }
        public CategoryCollection(IScheme parent) : base(parent)
        { }
        public new IEnumerator<ICategory> GetEnumerator()
        {
            foreach (var item in Items)
            {
                yield return item as ICategory;
            }
        }
    }
}