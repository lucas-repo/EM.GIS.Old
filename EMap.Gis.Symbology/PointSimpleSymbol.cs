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

        public PointShape PointShape { get; set; }

        public override void Draw(Image<Rgba32> image, float scale)
        {
            if (scale == 0) return;
            if (Size.Width == 0 || Size.Height == 0) return;

            SizeF size = new SizeF(scale * Size.Width, scale * Size.Height);
            IPen<Rgba32> pen = new Pen<Rgba32>(Color, Size.Width);
            IPath path = null;
            IBrush<Rgba32> fillBrush = new SolidBrush<Rgba32>(Color);
            switch (PointShape)
            {
                case PointShape.Diamond:
                    path = size.ToRegularPolyPath(4);
                    break;
                case PointShape.Ellipse:
                    path = size.ToEllipsePolygon();
                    break;
                case PointShape.Hexagon:
                    path = size.ToRegularPolyPath(6);
                    break;
                case PointShape.Pentagon:
                    path = size.ToRegularPolyPath(5);
                    break;
                case PointShape.Rectangle:
                    path = size.ToPath();
                    break;
                case PointShape.Star:
                    path = size.ToStarsPath();
                    break;
                case PointShape.Triangle:
                    path = size.ToRegularPolyPath(3);
                    break;
            }
            image.Mutate(x => x.Fill(Color, path));
            DrawOutLine(image, path, scale);
        }


    }
}
