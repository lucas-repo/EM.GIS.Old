namespace EM.GIS.Symbology
{
    public interface ILineLayer:IFeatureLayer
    {
        new ILineScheme Symbology { get; set; }
    }
}