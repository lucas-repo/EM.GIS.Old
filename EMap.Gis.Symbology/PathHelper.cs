using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public static class PathHelper
    {
        public static GraphicsPath ToPolygonPath(this RectangleF rectangle)
        {
            float xMin = rectangle.X;
            float yMin = rectangle.Y;
            float xMax = xMin + rectangle.Width;
            float yMax = yMin + rectangle.Height;
            GraphicsPath path = ToPath(xMin, yMin, xMax, yMax);
            return path;
        }
        public static PointF[] ToPoints(this RectangleF rectangle)
        {
            float xMin = rectangle.X;
            float yMin = rectangle.Y;
            float xMax = xMin + rectangle.Width;
            float yMax = yMin + rectangle.Height;
            PointF[] points = ToPoints(xMin, yMin, xMax, yMax);
            return points;
        }
        public static GraphicsPath ToPath(this RectangleF rectangle)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(rectangle);
            return graphicsPath;
        }
        public static GraphicsPath ToPath(this Rectangle rectangle)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddRectangle(rectangle);
            return graphicsPath;
        }
        public static GraphicsPath ToPath(this Size size)
        {
            float xMin = -size.Width / 2;
            float yMin = -size.Height / 2;
            float xMax = xMin + size.Width;
            float yMax = yMin + size.Height;
            GraphicsPath path = ToPath(xMin, yMin, xMax, yMax);
            return path;
        }
        public static PointF[] ToPoints(this SizeF size)
        {
            float xMin = -size.Width / 2;
            float yMin = -size.Height / 2;
            float xMax = xMin + size.Width;
            float yMax = yMin + size.Height;
            PointF[] points = ToPoints(xMin, yMin, xMax, yMax);
            return points;
        }
        public static GraphicsPath ToPath(this SizeF size)
        {
            float xMin = -size.Width / 2;
            float yMin = -size.Height / 2;
            float xMax = xMin + size.Width;
            float yMax = yMin + size.Height;
            GraphicsPath path = ToPath(xMin, yMin, xMax, yMax); 
            return path;
        }
        public static PointF[] ToPoints(float xMin, float yMin, float xMax, float yMax)
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
        public static GraphicsPath ToPath(float xMin, float yMin, float xMax, float yMax)
        {
            PointF[] polyPoints = ToPoints(xMin, yMin, xMax, yMax);
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(polyPoints);
            return path;
        }
        public static GraphicsPath ToPath(this PointF[] points)
        {
            GraphicsPath path = null;
            if (points == null || points.Length < 2)
            {
                return path;
            }
            path.AddLines(points); 
            return path;
        }
        
        public static GraphicsPath ToPath(this Point[] points)
        {
            GraphicsPath path = null;
            if (points == null || points.Length < 2)
            {
                return path;
            }
            path.AddLines(points);
            return path;
        }
       
        public static GraphicsPath ToEllipsePath(this RectangleF rectangleF)
        {
            PointF upperLeft = new PointF(-rectangleF.Width / 2, -rectangleF.Height / 2);
            RectangleF destRect = new RectangleF(upperLeft, rectangleF.Size);
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddEllipse(destRect);
            return graphicsPath;
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
        public static GraphicsPath ToRegularPath(this RectangleF rectangle, int numSides)//todo 待测试
        {
            PointF[] polyPoints = new PointF[numSides + 1];
            float centerX = rectangle.X + rectangle.Width / 2f;
            float centerY = rectangle.Y + rectangle.Height / 2f;
            for (int i = 0; i <= numSides; i++)
            {
                double ang = i * (2 * Math.PI) / numSides;
                float x = Convert.ToSingle(Math.Cos(ang)) * centerX;
                float y = Convert.ToSingle(Math.Sin(ang)) * centerY;
                polyPoints[i] = new PointF(x, y);
            }
            GraphicsPath graphics = new GraphicsPath();
            graphics.AddLines(polyPoints);
            return graphics;
        }
        public static GraphicsPath ToRegularPolyPath(this SizeF scaledSize, int numSides)
        {
            PointF[] polyPoints = ToRegularPoints(scaledSize, numSides);
            GraphicsPath path = polyPoints.ToPath();
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
        public static GraphicsPath ToStarsPath(this RectangleF rectangle)
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
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddLines(polyPoints);
            return graphicsPath;
        }
        public static GraphicsPath ToStarsPath(this SizeF scaledSize)
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
            GraphicsPath path = polyPoints.ToPath();
            return path;
        }
    }
}
