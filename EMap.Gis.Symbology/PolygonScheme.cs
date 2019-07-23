using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class PolygonScheme : FeatureScheme, IPolygonScheme
    {
        public override ICategory CreateNewCategory(Rgba32 fillColor, float size)
        {
            PolygonCategory result = new PolygonCategory();
            if (EditorSettings.UseGradient)
            {
                result.Symbolizer = new PolygonSymbolizer(fillColor.Lighter(.2f), fillColor.Darker(.2f), EditorSettings.GradientAngle, GradientType.Linear, fillColor.Darker(.5f), 1);
            }
            else
            {
                if (EditorSettings.TemplateSymbolizer != null)
                {
                    result.Symbolizer = EditorSettings.TemplateSymbolizer.Copy() as IPolygonSymbolizer;
                    result.Symbolizer.Symbols[0].Color = fillColor;
                }
                else
                {
                    result.Symbolizer = new PolygonSymbolizer(fillColor, fillColor.Darker(.5f));
                }
            }
            return result;
        }

        public override IFeatureCategory CreateRandomCategory(string filterExpression)
        {
            PolygonCategory result = new PolygonCategory();
            Rgba32 fillColor = CreateRandomColor();
            if (EditorSettings.UseGradient)
            {
                result.Symbolizer = new PolygonSymbolizer(fillColor.Lighter(.2f), fillColor.Darker(.2f), EditorSettings.GradientAngle, GradientType.Linear, fillColor.Darker(.5f), 1);
            }
            else
            {
                result.Symbolizer = new PolygonSymbolizer(fillColor, fillColor.Darker(.5f));
            }

            result.FilterExpression = filterExpression;
            result.LegendText = filterExpression;
            return result;
        }

        public override void DrawCategory(int index, Image<Rgba32> image, Rectangle bounds)
        {
            throw new NotImplementedException();
        }

    }
}
