namespace EMap.Gis.Symbology
{
    public interface IPointCategory : IFeatureCategory
    {
        new IPointSymbolizer SelectionSymbolizer { get; set; }
        new IPointSymbolizer Symbolizer { get; set; }
    }
}