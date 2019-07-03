using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    public interface IFeatureCategory : ICategory
    {
        string FilterExpression { get; set; }
        IFeatureSymbolizer SelectionSymbolizer { get; set; }

        IFeatureSymbolizer Symbolizer { get; set; }
    }
}
