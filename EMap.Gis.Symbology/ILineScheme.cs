namespace EMap.Gis.Symbology
{
    public interface ILineScheme:IFeatureScheme
    {
         new ILineCategoryCollection Categories { get; set; }
    }
}