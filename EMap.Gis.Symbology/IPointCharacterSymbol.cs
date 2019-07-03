using SixLabors.Fonts;
using System.Globalization;

namespace EMap.Gis.Symbology
{
    public interface IPointCharacterSymbol:IPointSymbol
    {
        UnicodeCategory Category { get; }

        char Character { get; set; }

        string FontFamilyName { get; set; }

        FontStyle Style { get; set; }

        string ToString();
    }
}