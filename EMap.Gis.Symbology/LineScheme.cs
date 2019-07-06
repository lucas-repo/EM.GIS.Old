using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    public class LineScheme:FeatureScheme,ILineScheme
    {
        public new CategoryCollection<ILineCategory> Categories { get; }

        public LineScheme()
        {
            Categories = new CategoryCollection<ILineCategory>(this);
            LineCategory category = new LineCategory();
            Categories.Add(category);
        }

        public override ICategory CreateNewCategory(Rgba32 fillColor, float size)
        {
            ILineSymbolizer ls = EditorSettings.TemplateSymbolizer.Clone() as ILineSymbolizer;
            if (ls != null)
            {
                ls.Color = fillColor;
                ls.Width = size;
            }
            else
            {
                ls = new LineSymbolizer(fillColor, size);
            }

            return new LineCategory(ls);
        }

        public override IFeatureCategory CreateRandomCategory(string filterExpression)
        {
            LineCategory result = new LineCategory();
            var fillColor = CreateRandomColor();
            result.Symbolizer = new LineSymbolizer(fillColor, 2);
            result.FilterExpression = filterExpression;
            result.LegendText = filterExpression;
            return result;
        }

        public override IEnumerable<IFeatureCategory> GetCategories()
        {
            return Categories;
        }

        public override void DrawCategory(int index, SixLabors.ImageSharp.Image<Rgba32> image, Rectangle bounds)
        {
            Categories[index].Symbolizer.Draw(image, bounds);
        }
        
    }
}
