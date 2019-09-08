using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public class PointSimpleSymbol : PointSymbol, IPointSimpleSymbol
    {
        public PointSimpleSymbol() : base(PointSymbolType.Simple)
        {
            PointShape = PointShape.Ellipse;
        }
        public PointSimpleSymbol(Color color)
           : this()
        {
            Color = color;
        }
        public PointSimpleSymbol(Color color, PointShape shape) : this(color)
        {
            PointShape = shape;
        }

        public PointSimpleSymbol(Color color, PointShape shape, float size) : this(color, shape)
        {
            Size = new SizeF(size, size);
        }

        public PointShape PointShape { get; set; }

        public override void DrawPoint(Graphics graphics, float scale, PointF point)
        {
            if (scale == 0) return;
            if (Size.Width == 0 || Size.Height == 0) return;

            RectangleF rectangle = new RectangleF(point.X, point.Y, scale * Size.Width, scale * Size.Height);
            GraphicsPath path = null;
            switch (PointShape)
            {
                case PointShape.Diamond:
                    path = rectangle.ToRegularPath(4);
                    break;
                case PointShape.Ellipse:
                    path = rectangle.ToEllipsePath();
                    break;
                case PointShape.Hexagon:
                    path = rectangle.ToRegularPath(6);
                    break;
                case PointShape.Pentagon:
                    path = rectangle.ToRegularPath(5);
                    break;
                case PointShape.Rectangle:
                    path = rectangle.ToPath();
                    break;
                case PointShape.Star:
                    path = rectangle.ToStarsPath();
                    break;
                case PointShape.Triangle:
                    path = rectangle.ToRegularPath(3);
                    break;
            }
            using (Brush brush = new SolidBrush(Color))
            {
                graphics.FillPath(brush, path);
            }
            DrawOutLine(graphics, scale, path);
        }
    }
}
