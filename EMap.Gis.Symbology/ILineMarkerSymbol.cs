namespace EMap.Gis.Symbology
{
    public interface ILineMarkerSymbol : ILineCartographicSymbol
    {
        IPointSymbolizer Marker { get; set; }
    }
}