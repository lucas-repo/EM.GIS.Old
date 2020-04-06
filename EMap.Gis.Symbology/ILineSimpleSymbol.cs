using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public interface ILineSimpleSymbol:ILineSymbol
    {
        DashStyle DashStyle { get; set; }
    }
}
