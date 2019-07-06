using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface IScheme:ILegendItem
    {
        int NumCategories { get; }
        EditorSettings EditorSettings { get; set; }
        Statistics Statistics { get; }
        void AddCategory(ICategory category);
        ICategory GetCategory(int index);
        void ClearCategories();
        void InsertCategory(int index, ICategory category);
        void RemoveCategory(ICategory category);
        ICategory CreateNewCategory(Rgba32 fillColor, double size);
        List<double> Values { get; }
    }
}