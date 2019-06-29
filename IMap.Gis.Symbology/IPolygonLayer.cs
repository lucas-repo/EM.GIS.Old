namespace IMap.Gis.Symbology
{
    public interface IPolygonLayer:IFeatureLayer
    {
        new IPolygonScheme Symbology { get; set; }
    }
}