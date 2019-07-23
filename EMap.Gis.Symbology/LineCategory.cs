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
        public LineCategory(ILineSymbolizer lineSymbolizer)
        {
            Symbolizer = lineSymbolizer;
            ILineSymbolizer select = lineSymbolizer.Clone() as ILineSymbolizer;
            SelectionSymbolizer = select;
            if (select.Symbols != null && select.Symbols.Count > 0)
            {
                var ss = select.Symbols[select.Symbols.Count - 1] as ILineSimpleSymbol;
                if (ss != null) ss.Color = Rgba32.Cyan;
            }
        }

        public override void LegendSymbolPainted(Image<Rgba32> image, Rectangle rectangle)
        {
            Symbolizer?.LegendSymbolPainted(image, rectangle);
        }
    }
}
