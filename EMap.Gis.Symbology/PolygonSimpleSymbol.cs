using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;



namespace EMap.Gis.Symbology
{
    public class PolygonSimpleSymbol : PolygonSymbol, IPolygonSimpleSymbol
    {
        public PolygonSimpleSymbol() : base(PolygonSymbolType.Simple)
        { }
    }
}
