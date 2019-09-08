using System;
using System.Drawing;

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

        public virtual void DrawLegend(Graphics graphics, Rectangle rectangle)
        {
            throw new NotImplementedException();
        }
    }
}