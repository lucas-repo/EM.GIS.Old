using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

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
        public override IPen ToPen(float scale)
        {
            IPen pen = ToPen(scale, 0);
            return pen;
        }
        public IPen ToPen(float scale, float angle)
        {
            IPen pen = null;
            if (Marker != null)
            {
                pen = base.ToPen(scale);
            }
            else
            {
                float width = GetWidth(scale);
                IBrush brush = null;
                if (Marker != null)
                {
                    SizeF size = Marker.Size;
                    int imgWidth = (int)Math.Ceiling(size.Width);
                    int imgHeight = (int)Math.Ceiling(size.Height);
                    Image<Rgba32> image = new Image<Rgba32>(imgWidth, imgHeight);
                    Rectangle rectangle = new Rectangle(0, 0, imgWidth, imgHeight);
                    image.Mutate(x =>
                    {
                        x.Rotate(angle);
                        Marker.DrawLegend(x, rectangle);
                    });
                    brush = new ImageBrush(image);
                }
                pen = new Pen(brush, width, Pattern);
            }
            return pen;
        }
        private float GetWidth(float scale)
        {
            float width = Width;
            if (Marker != null)
            {
                width = Marker.Size.Height;
            }
            width *= scale;
            return width;
        }
        public IPen ToPen(float scale, PointF startPoint, PointF endPoint)
        {
            IPen pen = null;
            if (Marker == null)
            {
                pen = base.ToPen(scale);
            }
            else
            {
                float width = GetWidth(scale);
                IBrush brush = null;
                SizeF size = Marker.Size;
                int imgWidth = (int)Math.Ceiling(size.Width);
                int imgHeight = (int)Math.Ceiling(size.Height);
                Image<Rgba32> image = new Image<Rgba32>(imgWidth, imgHeight);
                Rectangle rectangle = new Rectangle(0, 0, imgWidth, imgHeight);
                float angle = DrawingHelper.GetAngle(startPoint, endPoint, true);
                //foreach (var symbol in Marker.Symbols)
                //{
                //    symbol.Angle += angle;
                //}
                //AffineTransformBuilder ats = new AffineTransformBuilder().AppendRotationRadians(angle, new System.Numerics.Vector2(imgWidth / 2f, imgHeight / 2f));
                image.Mutate(x =>
                {
                    Marker.DrawLegend(x, rectangle);
                    x.Rotate(angle);
                });
                //foreach (var symbol in Marker.Symbols)
                //{
                //    symbol.Angle -= angle;
                //}
                brush = new ImageBrush(image) ;
                float[] dashPattern = ToDashPattern(Pattern, startPoint, endPoint);
                pen = new Pen(brush, width, dashPattern);
            }
            return pen;
        }
        public float[] ToDashPattern(float[] pattern, PointF startPoint, PointF endPoint)
        {
            float[] dashPattern = null;
            if (Marker == null)
            {
                dashPattern = pattern;
            }
            else
            {
                if (pattern != null)
                {
                    switch (pattern.Length)
                    {
                        case 0:
                            dashPattern = pattern;
                            break;
                        case 1:
                        default:
                            SizeF size = Marker.Size;
                            int imgWidth = (int)Math.Ceiling(size.Width);
                            int imgHeight = (int)Math.Ceiling(size.Height);
                            switch (pattern.Length)
                            {
                                case 1:
                                    float angle = DrawingHelper.GetAngle(startPoint, endPoint, false);
                                    float totalLength = Convert.ToSingle(DrawingHelper.Distance(startPoint.X, startPoint.Y, endPoint.X, endPoint.Y));
                                    float centerX = (startPoint.X + endPoint.X) / 2;
                                    float centerY = (startPoint.Y + endPoint.Y) / 2;
                                    float dy = Convert.ToSingle(imgWidth / 2.0 * Math.Sin(angle));
                                    float dx = Convert.ToSingle(-imgWidth / 2.0 * Math.Cos(angle));
                                    float x = centerX - dx;
                                    float y = centerY - dy;
                                    float firstLength = Convert.ToSingle(DrawingHelper.Distance(startPoint.X, startPoint.Y, x, y));
                                    float secondLength = imgWidth;
                                    float thirdLength = totalLength - firstLength - secondLength;
                                    if (thirdLength > 0)
                                    {
                                        dashPattern = new float[]
                                        {
                                    0,firstLength,secondLength,thirdLength
                                        };
                                    }
                                    break;
                                default:
                                    dashPattern = new float[pattern.Length];
                                    for (int i = 0; i < pattern.Length; i++)
                                    {
                                        if (i % 2 == 0)
                                        {
                                            dashPattern[i] = pattern[i] * imgWidth;
                                        }
                                        else
                                        {
                                            dashPattern[i] = pattern[i];
                                        }
                                        float item = pattern[i];
                                    }
                                    break;
                            }
                            break;
                    }
                }
            }
            return dashPattern;
        }
        public override void DrawLine(IImageProcessingContext context, float scale, IPath path)
        {
            if (Marker == null)
            {
                base.DrawLine(context, scale, path);
            }
            else
            {
                var simplePaths = path.Flatten();
                PointF[] points = new PointF[2];
                GraphicsOptions graphicsOptions = GraphicsOptions.Default;
                foreach (var simplePath in simplePaths)
                {
                    for (int i = 0; i < simplePath.Points.Count - 1; i++)
                    {
                        PointF point0 = simplePath.Points[i];
                        PointF point1 = simplePath.Points[i + 1];
                        IPen pen = ToPen(scale, point0, point1);
                        points[0] = point0;
                        points[1] = point1;
                        context.Draw(graphicsOptions, pen, points.ToPath());
                    }
                }
            }
        }
    }
}
