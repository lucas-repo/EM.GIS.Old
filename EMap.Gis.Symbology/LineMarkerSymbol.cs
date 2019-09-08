using System;
using System.Drawing;

namespace EMap.Gis.Symbology
{
    [Serializable]
    public class LineMarkerSymbol : LineCartographicSymbol, ILineMarkerSymbol
    {
        public IPointSymbolizer Marker { get; set; }
        public LineMarkerSymbol() : base(LineSymbolType.Marker)
        {
            Marker = new PointSymbolizer();
        }
        public override Pen ToPen(float scale)
        {
            Pen pen = base.ToPen(scale); 
            if (Marker != null)
            {
                int width = (int)Math.Ceiling(Marker.Size.Width);
                int height = (int)Math.Ceiling(Marker.Size.Height);
                Image image = new Bitmap(width, height);
                using (Graphics g = Graphics.FromImage(image))
                {
                    PointF point = new PointF(width / 2.0f, height / 2.0f);
                    Marker.DrawPoint(g, scale, point);
                }
                pen.Brush = new TextureBrush(image);//todo 待测试
            }
            return pen;
        }
    }
}
