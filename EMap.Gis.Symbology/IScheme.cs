using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
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
        void DrawCategory(int index, IImageProcessingContext context, Rectangle bounds);
        ICategory CreateNewCategory(Rgba32 fillColor, float size);
        List<double> Values { get; }
        void Move(int oldIndex, int newIndex);
    }
}