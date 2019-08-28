using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace EMap.Gis.Symbology
{
    public abstract class PolygonSymbol : OutLineSymbol, IPolygonSymbol
    {
        public PolygonSymbolType PolygonSymbolType { get; }

        public RectangleF Bounds { get; set; }
        public PolygonSymbol(PolygonSymbolType polygonSymbolType)
        {
            PolygonSymbolType = polygonSymbolType;
        }

        public void DrawPolygon(IImageProcessingContext<Rgba32> context, float scale, Polygon polygon)
        {
            if (context == null || polygon == null )
            {
                return;
            }
            IBrush<Rgba32> brush = GetBrush();
            if (brush == null)
            {
                return;
            }
            //List<ILineSegment> lineSegments = new List<ILineSegment>();
            //foreach (var points in polygons)
            //{
            //    if (points.Length > 2)
            //    {
            //        ILineSegment lineSegment = new LinearLineSegment(points); 
            //        lineSegments.Add(lineSegment);
            //    }
            //}
            //if (lineSegments.Count == 0)
            //{
            //    return;
            //}
            //Polygon polygon = new Polygon(lineSegments); 
            context.Fill(brush, polygon); 
            foreach (var lineSegment in polygon.LineSegments)
            {
                DrawOutLine(context, scale, lineSegment.Flatten().ToArray());
            }
        }

        public virtual IBrush<Rgba32> GetBrush()
        {
            IBrush<Rgba32> brush = new SolidBrush<Rgba32>(Color);
            return brush;
        }
    }
}
