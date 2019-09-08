using OSGeo.OGR;
using System.Data;
using System.Drawing;

namespace EMap.Gis.Symbology
{
    public interface IFeatureScheme : IScheme
    {
        new IFeatureCategoryCollection Categories { get; set; }
        new FeatureEditorSettings EditorSettings { get; set; }
        new IFeatureLayer Parent { get; set; }
        void CreateCategories(DataTable table);
        IFeatureCategory CreateRandomCategory(string filterExpression);
        void Draw(Graphics graphics, Envelope envelope, Rectangle rectangle);
    }
}
