using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace EMap.Gis.Symbology
{
    public class PointSimpleSymbol : PointSymbol, IPointSimpleSymbol
    {
        public PointSimpleSymbol() : base(PointSymbolType.Simple)
        {
            PointShape = PointShape.Ellipse;
        }
        public PointSimpleSymbol(Rgba32 color)
           : this()
        {
            Color = color;
        }
        public PointSimpleSymbol(Rgba32 color, PointShape shape) : this(color)
        {
            PointShape = shape;
        }

        public PointSimpleSymbol(Rgba32 color, PointShape shape, float size) : this(color, shape)
        {
            Size = new SizeF(size, size);
        }

        public PointShape PointShape { get; set; }

        public override void DrawPoint(IImageProcessingContext<Rgba32> context, float scale, PointF point)
        {
            if (scale == 0) return;
            if (Size.Width == 0 || Size.Height == 0) return;

            RectangleF rectangle = new RectangleF(point.X, point.Y, scale * Size.Width, scale * Size.Height);
            PointF[] points = null;
            switch (PointShape)
            {
                case PointShape.Diamond:
                    points = rectangle.ToRegularPoints(4);
                    break;
                case PointShape.Ellipse:
                    points = rectangle.ToEllipsePoints();
                    break;
                case PointShape.Hexagon:
                    points = rectangle.ToRegularPoints(6);
                    break;
                case PointShape.Pentagon:
                    points = rectangle.ToRegularPoints(5);
                    break;
                case PointShape.Rectangle:
                    points = rectangle.ToPoints();
                    break;
                case PointShape.Star:
                    points = rectangle.ToStarsPoints();
                    break;
                case PointShape.Triangle:
                    points = rectangle.ToRegularPoints(3);
                    break;
            }
            IPath path = points.ToPath();
            context.Fill(Color, path);
            DrawOutLine(context, scale, points);
        }
    }
}
