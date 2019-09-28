using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    [Serializable]
    public abstract class LegendItem : Descriptor, ILegendItem
    {
        private Size _legendSymbolSize;
        public bool IsVisible { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsSelected { get; set; }

        public bool LegendItemVisible { get; set; } = true;
        public string LegendText { get; set; }
        public ILegendItem Parent { get; set; }
        public ILegendItemCollection Items { get; set; }

        public LegendItem()
        {
            _legendSymbolSize = new Size(16, 16);
        }
        public LegendItem(ILegendItem parent) : this()
        {
            Parent = parent;
        }
        public Size GetLegendSymbolSize()
        {
            return _legendSymbolSize;
        }

        public virtual void DrawLegend(IImageProcessingContext context, Rectangle rectangle)
        {
            throw new NotImplementedException();
        }
    }
}