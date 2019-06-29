namespace IMap.Gis.Symbology
{
    public interface IPointLayer:IFeatureLayer
    {
        new IPointScheme Symbology { get; set; }
    }
}