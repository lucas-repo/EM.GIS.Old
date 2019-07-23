using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class PointScheme : FeatureScheme, IPointScheme
    {

        public PointScheme()
        {
            PointCategory category = new PointCategory();
            LegendItems.Add(category);
        }

        public override ICategory CreateNewCategory(Rgba32 fillColor, float size)
        {
            IPointSymbolizer ps = EditorSettings.TemplateSymbolizer.Clone() as IPointSymbolizer ?? new PointSymbolizer(fillColor, PointShape.Ellipse, size);
            ps.Symbols[0].Color = fillColor;
            SizeF oSize = ps.Size;
            float rat = size / Math.Max(oSize.Width, oSize.Height);
            ps.Size=new SizeF(rat * oSize.Width, rat * oSize.Height);
            return new PointCategory(ps);
        }

        public override IFeatureCategory CreateRandomCategory(string filterExpression)
        {
            PointCategory result = new PointCategory();
            var fillColor = CreateRandomColor();
            result.Symbolizer = new PointSymbolizer(fillColor, PointShape.Ellipse, 10);
            result.FilterExpression = filterExpression;
            result.LegendText = filterExpression;
            return result;
        }

    }
}
