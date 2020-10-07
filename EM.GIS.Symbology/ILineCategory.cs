namespace EM.GIS.Symbology
{
    public interface ILineCategory : IFeatureCategory
    {
        new ILineSymbolizer Symbolizer { get; set; }
        new ILineSymbolizer SelectionSymbolizer { get; set; }
    }
}