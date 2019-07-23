using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class PolygonCategory : FeatureCategory, IPolygonCategory
    {
        public new IPolygonSymbolizer SelectionSymbolizer { get => base.SelectionSymbolizer as IPolygonSymbolizer; set => base.SelectionSymbolizer = value; }
        public new IPolygonSymbolizer Symbolizer { get => base.Symbolizer as IPolygonSymbolizer; set => base.Symbolizer = value; }
        public PolygonCategory()
        {
            Symbolizer = new PolygonSymbolizer();
            SelectionSymbolizer = new PolygonSymbolizer(true);
        }

        public override void LegendSymbolPainted(Image<Rgba32> image, Rectangle rectangle)
        {
            Symbolizer?.LegendSymbolPainted(image, rectangle);
        }
    }
}
