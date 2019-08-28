using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public interface IFeatureScheme : IScheme
    {
        new IFeatureCategoryCollection Categories { get; set; }
        new FeatureEditorSettings EditorSettings { get; set; }
        new IFeatureLayer Parent { get; set; }
        void CreateCategories(DataTable table);
        IFeatureCategory CreateRandomCategory(string filterExpression);
        void Draw(IImageProcessingContext<Rgba32> context, Envelope envelope, Rectangle rectangle);
    }
}
