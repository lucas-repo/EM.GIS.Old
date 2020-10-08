namespace EM.GIS.Symbology
{
    public interface IPolygonLayer:IFeatureLayer
    {
        new IPolygonScheme Symbology { get; set; }
        new IPolygonCategory DefaultCategory { get; set; }
    }
}