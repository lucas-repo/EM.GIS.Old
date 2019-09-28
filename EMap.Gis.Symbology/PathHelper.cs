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
        public static PointF[] ToPoints(this RectangleF rectangle)
        {
            float xMin = rectangle.X;
            float yMin = rectangle.Y;
            float xMax = xMin + rectangle.Width;
            float yMax = yMin + rectangle.Height;
            PointF[] points = ExtentToPoints(xMin, yMin, xMax, yMax);
            return points;
        }
        public static IPath ToPath(this Size size)
        {
            float xMin = -size.Width / 2;
            float yMin = -size.Height / 2;
            float xMax = xMin + size.Width;
            float yMax = yMin + size.Height;
            IPath path = ExtentToPath(xMin, yMin, xMax, yMax);
            return path;
        }
        public static PointF[] ToPoints(this SizeF size)
        {
            float xMin = -size.Width / 2;
            float yMin = -size.Height / 2;
            float xMax = xMin + size.Width;
            float yMax = yMin + size.Height;
            PointF[] points = ExtentToPoints(xMin, yMin, xMax, yMax);
            return points;
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
        public static PointF[] ExtentToPoints(float xMin, float yMin, float xMax, float yMax)
        {
            PointF[] polyPoints = new PointF[]
            {
                new PointF(xMin,yMin),
                new PointF(xMin,yMax),
                new PointF(xMax,yMax),
                new PointF(xMax,yMin),
                new PointF(xMin,yMin)
            };
            return polyPoints;
        }
        public static IPath ExtentToPath(float xMin, float yMin, float xMax, float yMax)
        {
            PointF[] polyPoints = ExtentToPoints(xMin, yMin, xMax, yMax);
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
        public static Polygon ToPolygon(this PointF[] points)
        {
            Polygon polygon = null;
            if (points == null || points.Length < 3)
            {
                return polygon;
            }
            ILineSegment lineSegment = new LinearLineSegment(points);
            polygon = new Polygon(lineSegment);
            return polygon;
        }
        public static IPath ToPath(this Point[] points)
        {
            PointF[] pointFs = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                pointFs[i] = new PointF(points[i].X, points[i].Y);
            }
            IPath path = pointFs.ToPath();
            return path;
        }
        public static PointF[] ToEllipsePoints(this SizeF scaledSize)
        {
            PointF upperLeft = new PointF(-scaledSize.Width / 2, -scaledSize.Height / 2);
            RectangleF destRect = new RectangleF(upperLeft, scaledSize);
            EllipsePolygon ellipsePolygon = new EllipsePolygon(upperLeft, scaledSize);
            var pathes = ellipsePolygon.Flatten();
            List<PointF> points = new List<PointF>();
            foreach (var path in pathes)
            {
                foreach (var point in path.Points)
                {
                    points.Add(point);
                }
            }
            return points.ToArray();
        }
        public static PointF[] ToEllipsePoints(this RectangleF rectangle)
        {
            PointF upperLeft = new PointF(rectangle.X - rectangle.Width / 2, rectangle.Y - rectangle.Height / 2);
            EllipsePolygon ellipsePolygon = new EllipsePolygon(upperLeft.X, upperLeft.Y, rectangle.Width, rectangle.Height);
            var pathes = ellipsePolygon.Flatten();
            List<PointF> points = new List<PointF>();
            foreach (var path in pathes)
            {
                foreach (var point in path.Points)
                {
                    points.Add(point);
                }
            }
            return points.ToArray();
        }
        public static EllipsePolygon ToEllipsePolygon(this SizeF scaledSize)
        {
            PointF upperLeft = new PointF(-scaledSize.Width / 2, -scaledSize.Height / 2);
            RectangleF destRect = new RectangleF(upperLeft, scaledSize);
            EllipsePolygon ellipsePolygon = new EllipsePolygon(upperLeft, scaledSize);
            return ellipsePolygon;
        }
        public static PointF[] ToRegularPoints(this SizeF scaledSize, int numSides)
        {
            PointF[] polyPoints = new PointF[numSides + 1];
            for (int i = 0; i <= numSides; i++)
            {
                double ang = i * (2 * Math.PI) / numSides;
                float x = Convert.ToSingle(Math.Cos(ang)) * scaledSize.Width / 2f;//todo 待测试
                float y = Convert.ToSingle(Math.Sin(ang)) * scaledSize.Height / 2f;
                polyPoints[i] = new PointF(x, y);
            }
            return polyPoints;
        }
        public static PointF[] ToRegularPoints(this RectangleF rectangle, int numSides)//todo 待测试
        {
            PointF[] polyPoints = new PointF[numSides + 1];
            float centerX = rectangle.X + rectangle.Width / 2f;
            float centerY = rectangle.Y + rectangle.Height / 2f;
            for (int i = 0; i <= numSides; i++)
            {
                double ang = i * (2 * Math.PI) / numSides;
                float x = centerX + Convert.ToSingle(Math.Cos(ang)) * rectangle.Width / 2f;
                float y = centerY + Convert.ToSingle(Math.Sin(ang)) * rectangle.Height / 2f;
                polyPoints[i] = new PointF(x, y);
            }
            return polyPoints;
        }
        public static IPath ToRegularPolyPath(this SizeF scaledSize, int numSides)
        {
            PointF[] polyPoints = ToRegularPoints(scaledSize, numSides);
            IPath path = polyPoints.ToPath();
            return path;
        }
        public static PointF[] ToStarsPoints(this SizeF scaledSize)
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
            return polyPoints;
        }
        public static PointF[] ToStarsPoints(this RectangleF rectangle)
        {
            PointF[] polyPoints = new PointF[11];
            float centerX = rectangle.X + rectangle.Width / 2f;
            float centerY = rectangle.Y + rectangle.Height / 2f;
            for (int i = 0; i <= 10; i++)
            {
                double ang = i * Math.PI / 5;
                float x = Convert.ToSingle(Math.Cos(ang)) * centerX;
                float y = Convert.ToSingle(Math.Sin(ang)) * centerY;
                if (i % 2 == 0)
                {
                    x /= 2; // the shorter radius points of the star
                    y /= 2;
                }
                polyPoints[i] = new PointF(x, y);
            }
            return polyPoints;
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
