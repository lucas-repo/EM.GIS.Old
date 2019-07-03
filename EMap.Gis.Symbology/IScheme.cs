using System;
using System.Collections;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface IScheme:IDisposable
    {
        int Count { get; }
        ICategory this[int index] { get; set; }
        void AddCategory(ICategory category);
        void ClearCategories();
        void InsertCategory(int index, ICategory category);
        void RemoveCategory(ICategory category);
        IEnumerable<ICategory> GetCategories();
    }
}