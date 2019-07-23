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
            throw new NotImplementedException();
        }

        public override IFeatureCategory CreateRandomCategory(string filterExpression)
        {
            throw new NotImplementedException();
        }

        public override void DrawCategory(int index, Image<Rgba32> image, Rectangle bounds)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IFeatureCategory> GetCategories()
        {
            throw new NotImplementedException();
        }

    }
}
