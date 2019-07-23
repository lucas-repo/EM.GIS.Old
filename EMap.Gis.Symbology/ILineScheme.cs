namespace EMap.Gis.Symbology
{
    public interface ILineScheme:IFeatureScheme
    {
        new CategoryCollection<ILineCategory> Categories { get; }
    }
}