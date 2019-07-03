using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface ILineCartographicSymbol:ILineSimpleSymbol
    {
        float[] Pattern { get; set; }
        List<ILineDecoration> Decorations { get; }
    }
}