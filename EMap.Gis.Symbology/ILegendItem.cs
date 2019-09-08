using System;
using System.Drawing;

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
        void DrawLegend(Graphics graphics, Rectangle rectangle);
    }
}