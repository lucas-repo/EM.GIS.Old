namespace EMap.Gis.Symbology
{
    public interface IPointSimpleSymbol : IPointSymbol
    {
        PointShape PointShape { get; set; }
    }
}