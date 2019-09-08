
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    internal class PolygonSymbolizer : FeatureSymbolizer, IPolygonSymbolizer
    {
        public new IPolygonSymbolCollection Symbols { get => base.Symbols as IPolygonSymbolCollection; set => base.Symbols = value; }
      
        public PolygonSymbolizer()
        {
            Symbols = new PolygonSymbolCollection();
            PolygonSimpleSymbol polygonSymbol = new PolygonSimpleSymbol();
            Symbols.Add(polygonSymbol);
        }

        public PolygonSymbolizer(bool selected) 
        {
            Symbols = new PolygonSymbolCollection();
            IPolygonSymbol polygonSymbol = new PolygonSimpleSymbol();
            if (selected)
            {
                polygonSymbol.Color = Color.Cyan;
            }
            Symbols.Add(polygonSymbol);
        }
        public override void DrawLegend(Graphics graphics, Rectangle rectangle)
        {
            PointF[] points = new PointF[]
            {
                new PointF(rectangle.Left,rectangle.Top),
                new PointF(rectangle.Left,rectangle.Bottom),
                new PointF(rectangle.Right,rectangle.Bottom),
                new PointF(rectangle.Right,rectangle.Top),
                new PointF(rectangle.Left,rectangle.Top)
            };
            using (GraphicsPath path = rectangle.ToPath())
            {
                DrawPolygon(graphics, 1, path);
            }
        }
        public void DrawPolygon(Graphics graphics, float scale, GraphicsPath path)
        {
            foreach (var symbol in Symbols)
            {
                symbol.DrawPolygon(graphics, scale, path);
            }
        }
    }
}