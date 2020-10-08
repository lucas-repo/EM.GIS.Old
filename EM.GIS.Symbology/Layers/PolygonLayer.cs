using EM.GIS.Data;
using OSGeo.OGR;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EM.GIS.Symbology
{
    public class PolygonLayer : FeatureLayer, IPolygonLayer
    {
        public new IPolygonScheme Symbology { get => base.Symbology as IPolygonScheme; set => base.Symbology = value; }
        public new IPolygonCategory DefaultCategory { get => base.DefaultCategory as IPolygonCategory; set => base.DefaultCategory = value; }

        public PolygonLayer(IFeatureSet featureSet) : base(featureSet)
        {
            Symbology = new PolygonScheme();
            DefaultCategory = new PolygonCategory();
        }
        protected override void DrawGeometry(MapArgs drawArgs, IFeatureSymbolizer symbolizer, Geometry geometry)
        {
            if (drawArgs == null || !(symbolizer is IPolygonSymbolizer polygonSymbolizer) || geometry == null)
            {
                return;
            }
            float scaleSize = (float)symbolizer.GetScale(drawArgs);
            GraphicsPath path = new  GraphicsPath();
            GetPolygons(drawArgs, geometry, path);
            polygonSymbolizer.DrawPolygon(drawArgs.Device, scaleSize, path);
        }
        private void DrawGeometry(MapArgs drawArgs, Graphics context, float scaleSize, IPolygonSymbolizer polygonSymbolizer, Geometry geometry)
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
                polygonSymbolizer.DrawPolygon(context, scaleSize, points.ToPath());
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
        private void GetPolygons(MapArgs drawArgs, Geometry geometry, GraphicsPath path)
        {
            int geoCount = geometry.GetGeometryCount();
            var geometryType = geometry.GetGeometryType();
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
                            GetPolygons(drawArgs, partGeo, path);
                        }
                    }
                    break;
                case wkbGeometryType.wkbPolygon:
                case wkbGeometryType.wkbPolygon25D:
                case wkbGeometryType.wkbPolygonM:
                case wkbGeometryType.wkbPolygonZM:
                    for (int i = 0; i < geoCount; i++)
                    {
                        using (var partGeo = geometry.GetGeometryRef(i))
                        {
                            path.StartFigure();
                            GetPolygons(drawArgs, partGeo, path);
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
                    }
                    path.AddLines(points);
                    break;
                default:
                    throw new Exception("不支持的几何类型");
            }
        }
    }
}