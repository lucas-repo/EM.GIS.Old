using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;

namespace EMap.Gis.Symbology
{
    public interface ILineSimpleSymbol:ILineSymbol
    {
        DashStyle DashStyle { get; set; }
    }
}
