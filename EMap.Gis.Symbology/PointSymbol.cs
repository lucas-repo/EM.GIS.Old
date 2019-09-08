using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;



namespace EMap.Gis.Symbology
{
    public abstract class PointSymbol : OutLineSymbol, IPointSymbol
    {
        public float Angle { get; set; }
        public SizeF Size { get; set; }
        public PointF Offset { get; set; }
        public PointSymbolType PointSymbolType { get; }
        public PointSymbol(PointSymbolType pointSymbolType)
        {
            PointSymbolType = pointSymbolType;
        }
        public abstract void DrawPoint(Graphics context, float scale, PointF point);
    }
}
