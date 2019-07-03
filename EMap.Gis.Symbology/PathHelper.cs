using SixLabors.Primitives;
using SixLabors.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMap.Gis.Symbology
{
    public static class PathHelper
    {
        public static IPath ToPath(this RectangleF rectangle)
        {
            float xMin = rectangle.X;
            float yMin = rectangle.Y;
            float xMax = xMin + rectangle.Width;
            float yMax = yMin + rectangle.Height;
            IPath path = ExtentToPath(xMin, yMin, xMax, yMax);
            return path;
        }
        public static IPath ToPath(this Size size)
        {
            float xMin = -size.Width / 2;
            float yMin = -size.Height / 2;
            float xMax = xMin + size.Width;
            float yMax = yMin + size.Height;
            IPath path = ExtentToPath( xMin,  yMin,  xMax,  yMax);
            return path;
        }
        public static IPath ToPath(this SizeF size)
        {
            float xMin = -size.Width / 2;
            float yMin = -size.Height / 2;
            float xMax = xMin + size.Width;
            float yMax = yMin + size.Height;
            IPath path = ExtentToPath(xMin, yMin, xMax, yMax);
            return path;
        }
        public static IPath ExtentToPath(float xMin, float yMin, float xMax, float yMax)
        {
            PointF[] polyPoints = new PointF[]
            {
                new PointF(xMin,yMin),
                new PointF(xMin,yMax),
                new PointF(xMax,yMax),
                new PointF(xMax,yMin),
                new PointF(xMin,yMin)
            };
            ILineSegment lineSegment = new LinearLineSegment(polyPoints);
            IPath path = new Path(lineSegment);
            return path;
        }
        public static IPath ToPath(this PointF[] points)
        {
            IPath path = null;
            if (points == null || points.Length < 2)
            {
                return path;
            }
            ILineSegment lineSegment = new LinearLineSegment(points);
             path = new Path(lineSegment);
            return path;
        }
        public static EllipsePolygon ToEllipsePolygon(this SizeF scaledSize)
        {
            PointF upperLeft = new PointF(-scaledSize.Width / 2, -scaledSize.Height / 2);
            RectangleF destRect = new RectangleF(upperLeft, scaledSize);
            EllipsePolygon ellipsePolygon = new EllipsePolygon(upperLeft, scaledSize);
            return ellipsePolygon;
        }
        public static IPath ToRegularPolyPath(this SizeF scaledSize, int numSides)
        {
            PointF[] polyPoints = new PointF[numSides + 1];
            for (int i = 0; i <= numSides; i++)
            {
                double ang = i * (2 * Math.PI) / numSides;
                float x = Convert.ToSingle(Math.Cos(ang)) * scaledSize.Width / 2f;
                float y = Convert.ToSingle(Math.Sin(ang)) * scaledSize.Height / 2f;
                polyPoints[i] = new PointF(x, y);
            }
            IPath path = polyPoints.ToPath();
            return path;
        }
        public static IPath ToStarsPath(this SizeF scaledSize)
        {
            PointF[] polyPoints = new PointF[11];
            for (int i = 0; i <= 10; i++)
            {
                double ang = i * Math.PI / 5;
                float x = Convert.ToSingle(Math.Cos(ang)) * scaledSize.Width / 2f;
                float y = Convert.ToSingle(Math.Sin(ang)) * scaledSize.Height / 2f;
                if (i % 2 == 0)
                {
                    x /= 2; // the shorter radius points of the star
                    y /= 2;
                }
                polyPoints[i] = new PointF(x, y);
            }
            IPath path = polyPoints.ToPath();
            return path;
        }
    }
}
