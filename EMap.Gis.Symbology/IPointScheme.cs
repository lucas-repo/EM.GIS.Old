namespace EMap.Gis.Symbology
{
    public interface IPointScheme : IFeatureScheme
    {
        new IPointCategoryCollection Categories { get; set; }
    }
}