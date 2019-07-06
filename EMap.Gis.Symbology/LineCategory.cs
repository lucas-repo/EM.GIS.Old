using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class LineCategory : FeatureCategory, ILineCategory
    {
        public new ILineSymbolizer SelectionSymbolizer { get => base.SelectionSymbolizer as ILineSymbolizer; set => base.SelectionSymbolizer = value; }
        public new ILineSymbolizer Symbolizer { get => base.Symbolizer as ILineSymbolizer; set => base.Symbolizer = value; }
        public LineCategory()
        {
            Symbolizer = new LineSymbolizer();
            SelectionSymbolizer = new LineSymbolizer(true); 
        }

        public override void Draw(Image<Rgba32> image, Rectangle rectangle)
        {
            Symbolizer?.Draw(image, rectangle);
        }
    }
}
