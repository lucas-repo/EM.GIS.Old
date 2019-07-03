namespace EMap.Gis.Symbology
{
    public interface IPointLayer:IFeatureLayer
    {
        new IPointScheme Symbology { get; set; }
    }
}