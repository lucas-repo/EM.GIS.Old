using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public abstract class LegendItem : Descriptor, ILegendItem
    {
        private Size _legendSymbolSize;
        public bool IsVisible { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsSelected { get; set; }

        public virtual IEnumerable<ILegendItem> LegendItems { get; } = null;

        public bool LegendItemVisible { get; set; } = true;
        public string LegendText { get; set; }
        public ILegendItem Parent { get; set; }

        public LegendItem()
        {
            _legendSymbolSize = new Size(16, 16);
        }
        public Size GetLegendSymbolSize()
        {
            return _legendSymbolSize;
        }

        public virtual void Draw(Image<Rgba32> image, Rectangle rectangle)
        { }
    }
}