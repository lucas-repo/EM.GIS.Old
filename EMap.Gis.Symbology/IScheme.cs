
using System.Drawing;


using System;
using System.Collections;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface IScheme : ILegendItem
    {
        ICategoryCollection Categories { get; set; }
        EditorSettings EditorSettings { get; set; }
        Statistics Statistics { get; }
        void DrawCategory(int index, Graphics context, Rectangle bounds);
        ICategory CreateNewCategory(Color fillColor, float size);
        List<double> Values { get; }
        void Move(int oldIndex, int newIndex);
    }
}