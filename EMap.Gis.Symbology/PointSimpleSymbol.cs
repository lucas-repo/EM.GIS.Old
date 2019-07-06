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
        public PointSimpleSymbol(Rgba32 color, PointShape shape):this(color)
        {
            PointShape = shape;
        }

        public PointSimpleSymbol(Rgba32 color, PointShape shape, float size) : this(color, shape)
        {
            Size = new SizeF(size, size);
        }

        public PointShape PointShape { get; set; }

        public override void Draw(Image<Rgba32> image, float scale)
        {
            if (scale == 0) return;
            if (Size.Width == 0 || Size.Height == 0) return;

            SizeF size = new SizeF(scale * Size.Width, scale * Size.Height);
            IPen<Rgba32> pen = new Pen<Rgba32>(Color, Size.Width);
            PointF[] points = null;
            IBrush<Rgba32> fillBrush = new SolidBrush<Rgba32>(Color);
            switch (PointShape)
            {
                case PointShape.Diamond:
                    points = size.ToRegularPoints(4);
                    break;
                case PointShape.Ellipse:
                    points = size.ToEllipsePoints();
                    break;
                case PointShape.Hexagon:
                    points = size.ToRegularPoints(6);
                    break;
                case PointShape.Pentagon:
                    points = size.ToRegularPoints(5);
                    break;
                case PointShape.Rectangle:
                    points = size.ToPoints();
                    break;
                case PointShape.Star:
                    points = size.ToStarsPoints();
                    break;
                case PointShape.Triangle:
                    points = size.ToRegularPoints(3);
                    break;
            }
            IPath path = points.ToPath();
            image.Mutate(x => x.Fill(Color, path));
            DrawPath(image, scale, points);
        }
    }
}
