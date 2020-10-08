using EM.GIS.Data;

namespace EM.GIS.Symbology
{
    public interface IRasterLayer : ILayer
    {
        new IRasterScheme Symbology { get; set; }
        new IRasterCategory DefaultCategory { get; set; }
        new IRasterSet DataSet { get; set; }
    }
}
