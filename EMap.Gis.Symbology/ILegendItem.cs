using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
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
        ILegendItemCollection Items { get;  }
        bool LegendItemVisible { get; set; }
        string LegendText { get; set; }
        Size GetLegendSymbolSize();
        void DrawLegend(IImageProcessingContext context, Rectangle rectangle);
    }
}