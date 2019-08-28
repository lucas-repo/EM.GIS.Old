namespace EMap.Gis.Symbology
{
    public interface IPolygonScheme:IFeatureScheme
    {
        new IPolygonCategoryCollection Categories { get; set; }
    }
}