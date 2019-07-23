using SixLabors.Fonts;
using SixLabors.Primitives;
using SixLabors.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    public static class DrawingExtension
    {
        public static RectangleF MeasureText(this string text, Font fnt)
        {
            RendererOptions rendererOptions = new RendererOptions(fnt);
            IPathCollection paths = TextBuilder.GenerateGlyphs(text, rendererOptions);
            return paths.Bounds;
        }
    }
}
