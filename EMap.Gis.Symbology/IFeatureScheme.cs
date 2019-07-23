using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EMap.Gis.Symbology
{
    public interface IFeatureScheme : IScheme
    {
        new FeatureEditorSettings EditorSettings { get; set; }
        void CreateCategories(DataTable table);
        IEnumerable<IFeatureCategory> GetCategories();
        IFeatureCategory CreateRandomCategory(string filterExpression);

    }
}
