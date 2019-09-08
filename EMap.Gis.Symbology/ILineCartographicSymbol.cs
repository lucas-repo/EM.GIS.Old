using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public interface ILineCartographicSymbol:ILineSimpleSymbol
    {
        float[] CompoundArray { get; set; }
        bool[] CompoundButtons { get; set; }
        bool[] DashButtons { get; set; }
        DashCap DashCap { get; set; }
        LineCap EndCap { get; set; }
        LineJoinType JoinType { get; set; }
        float Offset { get; set; }
        LineCap StartCap { get; set; }
        float[] DashPattern { get; set; }
        IList<ILineDecoration> Decorations { get; }
    }
}