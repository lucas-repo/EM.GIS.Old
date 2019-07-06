namespace EMap.Gis.Symbology
{
    public interface IPointScheme : IFeatureScheme
    {
        new CategoryCollection<IPointCategory> Categories { get; }
    }
}