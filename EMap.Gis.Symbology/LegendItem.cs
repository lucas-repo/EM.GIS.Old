using System.Collections.Generic;
using System.Collections.ObjectModel;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public abstract class LegendItem : Descriptor, ILegendItem
    {
        private Size _legendSymbolSize;
        public bool IsVisible { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsSelected { get; set; }

        public ObservableCollection<ILegendItem> LegendItems { get; } = new ObservableCollection<ILegendItem>();

        public bool LegendItemVisible { get; set; } = true;
        public string LegendText { get; set; }
        public ILegendItem Parent { get; set; }
        public virtual SymbolMode LegendSymbolMode { get; }

        public LegendItem()
        {
            _legendSymbolSize = new Size(16, 16); 
        }
        public Size GetLegendSymbolSize()
        {
            return _legendSymbolSize;
        }

        public virtual void LegendSymbolPainted(Image<Rgba32> image, Rectangle rectangle)
        { }

        public void PrintLegendItem(Image<Rgba32> image, Font font, Rgba32 fontColor, SizeF maxExtent)
        {
            string text = LegendText;
            if (text == null)
            {
                if (Parent?.LegendItems.Count == 1)
                {
                    text = Parent.LegendText;
                }
            }
            var txtBound = "Sample text".MeasureText( font);

            float h = txtBound.Height;
            float x = 0;
            bool drawBox = false;
            if (LegendSymbolMode == SymbolMode.Symbol)
            {
                drawBox = true;
                x = (h * 2) + 4;
            }
            float w = maxExtent.Width - x;
            PointF position = new PointF(x, 2);
            image.Mutate(p => p.DrawText(text, font, fontColor, position));
            if (drawBox) LegendSymbolPainted(image, new Rectangle(2, 2, (int)x - 4, (int)h));
        }
    }
}