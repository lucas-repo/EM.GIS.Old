using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface ILegendItem : IDescriptor, IParentItem<ILegendItem>,IDisposable
    {
        bool IsVisible { get; set; }
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
        IEnumerable<ILegendItem> LegendItems { get; }
        bool LegendItemVisible { get; set; }
        string LegendText { get; set; }
        Size GetLegendSymbolSize();
        void Draw(Image<Rgba32> image, Rectangle rectangle);
    }
}