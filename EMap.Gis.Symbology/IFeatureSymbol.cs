using System.Drawing;

namespace EMap.Gis.Symbology
{
    public interface IFeatureSymbol: ISymbol
    {
        Color Color { get; set; }
        float Opacity { get; set; }
    }
}
