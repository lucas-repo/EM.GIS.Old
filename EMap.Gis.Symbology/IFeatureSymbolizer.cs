namespace EMap.Gis.Symbology
{
    public interface IFeatureSymbolizer:ISymbolizer
    {
        ScaleMode ScaleMode { get; set; }
        double GetScale(MapArgs drawArgs);

        IFeatureSymbolCollection Symbols { get; set; }
    }
}
