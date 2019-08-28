namespace EMap.Gis.Symbology
{
    public interface IPolygonLayer:IFeatureLayer
    {
        new IPolygonScheme Symbology { get; set; }
        new IPolygonCategory DefaultCategory { get; set; }
    }
}