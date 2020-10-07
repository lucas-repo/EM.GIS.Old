namespace EM.GIS.Symbology
{
    public interface IPointLayer:IFeatureLayer
    {
        new IPointScheme Symbology { get; set; }
        new IPointCategory DefaultCategory { get; set; }
    }
}