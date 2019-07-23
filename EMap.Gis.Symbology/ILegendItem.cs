using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EMap.Gis.Symbology
{
    public interface ILegendItem : IDescriptor,IDisposable
    {
        bool IsVisible { get; set; }
        bool IsExpanded { get; set; }
        bool IsSelected { get; set; }
        ILegendItem Parent { get; set; }
        SymbolMode LegendSymbolMode { get; }
        ObservableCollection<ILegendItem> LegendItems { get; }
        bool LegendItemVisible { get; set; }
        string LegendText { get; set; }
        Size GetLegendSymbolSize();
        void LegendSymbolPainted(Image<Rgba32> image, Rectangle rectangle);
        void PrintLegendItem(Image<Rgba32> image, Font font, Rgba32 fontColor, SizeF maxExtent);
    }
}