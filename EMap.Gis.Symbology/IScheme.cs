using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface IScheme:ILegendItem
    {
        EditorSettings EditorSettings { get; set; }
        Statistics Statistics { get; }
        void DrawCategory(int index, Image<Rgba32> image, Rectangle bounds);
        ICategory CreateNewCategory(Rgba32 fillColor, float size);
        List<double> Values { get; }
    }
}