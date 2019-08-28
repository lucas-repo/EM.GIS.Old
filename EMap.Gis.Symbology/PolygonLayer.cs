using EMap.Gis.Data;
using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Diagnostics;

namespace EMap.Gis.Symbology
{
    public class PolygonLayer : FeatureLayer, IPolygonLayer
    {
        public new IPolygonScheme Symbology { get => base.Symbology as IPolygonScheme; set => base.Symbology = value; }
        public new IPolygonCategory DefaultCategory { get => base.DefaultCategory as IPolygonCategory; set => base.DefaultCategory = value; }

        public PolygonLayer(DataSource dataSource) : base(dataSource)
        {
            Symbology = new PolygonScheme();
            DefaultCategory = new PolygonCategory();
        }
        protected override void DrawGeometry(MapArgs drawArgs, IFeatureSymbolizer symbolizer, Geometry geometry)
        {
            IPolygonSymbolizer polygonSymbolizer = symbolizer as IPolygonSymbolizer;
            if (drawArgs == null || polygonSymbolizer == null || geometry == null)
            {
                return;
            }
            drawArgs.Image.Mutate(context =>
            {
                float scaleSize = (float)symbolizer.GetScale(drawArgs);
                var polygons = GetPolygons(drawArgs, geometry);
                foreach (var polygon in polygons)
                {
                    polygonSymbolizer.DrawPolygon(context, scaleSize, polygon);
                }
            });
        }
        private void DrawGeometry(MapArgs drawArgs, IImageProcessingContext<Rgba32> context, float scaleSize, IPolygonSymbolizer polygonSymbolizer, Geometry geometry)
        {
            int geoCount = geometry.GetGeometryCount();
            if (geoCount == 0)
            {
                int pointCount = geometry.GetPointCount();
                double[] coord = new double[2];
                PointF[] points = new PointF[pointCount];
                for (int j = 0; j < pointCount; j++)
                {
                    geometry.GetPoint_2D(j, coord);
                    PointF point = drawArgs.ProjToPixelPointF(coord);
                    points[j] = point;
                }
                polygonSymbolizer.DrawPolygon(context, scaleSize, points.ToPolygon());
            }
            else
            {
                for (int i = 0; i < geoCount; i++)
                {
                    using (var partGeo = geometry.GetGeometryRef(i))
                    {
                        DrawGeometry(drawArgs, context, scaleSize, polygonSymbolizer, partGeo);
                    }
                }
            }
        }
        private List<ILineSegment> GetLineSegments(MapArgs drawArgs, Geometry geometry)
        {
            List<ILineSegment> lineSegments = new List<ILineSegment>();
            int geoCount = geometry.GetGeometryCount();
            var geometryType = geometry.GetGeometryType();
            switch (geometryType)
            {
                case wkbGeometryType.wkbPolygon:
                case wkbGeometryType.wkbPolygon25D:
                case wkbGeometryType.wkbPolygonM:
                case wkbGeometryType.wkbPolygonZM:
                    for (int i = 0; i < geoCount; i++)
                    {
                        using (var partGeo = geometry.GetGeometryRef(i))
                        {
                            lineSegments.AddRange(GetLineSegments(drawArgs, partGeo));
                        }
                    }
                    break;
                case wkbGeometryType.wkbLineString:
                case wkbGeometryType.wkbLineString25D:
                case wkbGeometryType.wkbLineStringM:
                case wkbGeometryType.wkbLineStringZM:
                    int pointCount = geometry.GetPointCount();
                    double[] coord = new double[2];
                    PointF[] points = new PointF[pointCount];
                    for (int j = 0; j < pointCount; j++)
                    {
                        geometry.GetPoint_2D(j, coord);
                        PointF point = drawArgs.ProjToPixelPointF(coord);
                        points[j] = point;
                        Debug.WriteLine($"{point.X},{point.Y}");
                    }
                    ILineSegment lineSegment = new LinearLineSegment(points);
                    lineSegments.Add(lineSegment);
                    break;
                default:
                    throw new Exception("不支持的几何类型");
            }
            return lineSegments;
        }
        private List<Polygon> GetPolygons(MapArgs drawArgs, Geometry geometry)
        {
            List<Polygon> polygons = new List<Polygon>();
            int geoCount = geometry.GetGeometryCount();
            var geometryType = geometry.GetGeometryType();
            Polygon polygon;
            switch (geometryType)
            {
                case wkbGeometryType.wkbMultiPolygon:
                case wkbGeometryType.wkbMultiPolygon25D:
                case wkbGeometryType.wkbMultiPolygonM:
                case wkbGeometryType.wkbMultiPolygonZM:
                    for (int i = 0; i < geoCount; i++)
                    {
                        using (var partGeo = geometry.GetGeometryRef(i))
                        {
                            polygon = GetPolygon(drawArgs, partGeo);
                            polygons.Add(polygon);
                        }
                    }
                    break;
                case wkbGeometryType.wkbPolygon:
                case wkbGeometryType.wkbPolygon25D:
                case wkbGeometryType.wkbPolygonM:
                case wkbGeometryType.wkbPolygonZM:
                    polygon = GetPolygon(drawArgs, geometry);
                    polygons.Add(polygon);
                    break;
                default:
                    throw new Exception("不支持的几何类型");
            }
            return polygons;
        }
        private Polygon GetPolygon(MapArgs drawArgs, Geometry geometry)
        {
            List<ILineSegment> lineSegments = GetLineSegments(drawArgs, geometry);
            Polygon polygon = new Polygon(lineSegments);
            return polygon;
        }
    }
}