using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class PointCategory : FeatureCategory, IPointCategory
    {
        public new IPointSymbolizer SelectionSymbolizer { get=>base.SelectionSymbolizer as IPointSymbolizer; set => base.SelectionSymbolizer=value; }
        public new IPointSymbolizer Symbolizer { get => base.Symbolizer as IPointSymbolizer; set => base.Symbolizer = value; }
        public PointCategory()
        {
            Symbolizer = new PointSymbolizer();
            SelectionSymbolizer = new PointSymbolizer(true);
        }
        public PointCategory(IPointSymbolizer pointSymbolizer)
        {
            Symbolizer = pointSymbolizer;
            SelectionSymbolizer = pointSymbolizer.Clone() as IPointSymbolizer;
            SelectionSymbolizer.Symbols[0].Color = Rgba32.Cyan;
        }

        public override void Draw(Image<Rgba32> image, Rectangle rectangle)
        {
            Symbolizer?.Draw(image, rectangle);
        }
    }
}
