using EMap.Gis.Data;
using OSGeo.OGR;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public class LineLayer : FeatureLayer, ILineLayer
    {
        public new ILineScheme Symbology
        {
            get => base.Symbology as ILineScheme;
            set => base.Symbology = value;
        }
        public new ILineCategory DefaultCategory
        {
            get => base.DefaultCategory as ILineCategory;
            set => base.DefaultCategory = value;
        }
        public LineLayer(DataSource dataSource) : base(dataSource)
        {
            DefaultCategory = new LineCategory();
            Symbology = new LineScheme();
            Symbology.Categories.Add(DefaultCategory);
        }

        protected override void DrawGeometry(MapArgs drawArgs, IFeatureSymbolizer symbolizer, Geometry geometry)
        {
            if (drawArgs == null || !(symbolizer is ILineSymbolizer lineSymbolizer) || geometry == null)
            {
                return;
            }
            float scaleSize = (float)symbolizer.GetScale(drawArgs);
            using (GraphicsPath path = new GraphicsPath())
            {
                GetLines(drawArgs, geometry, path);
                lineSymbolizer.DrawLine(drawArgs.Device, scaleSize, path);
            }
        }
        private void GetLines(MapArgs drawArgs, Geometry geometry, GraphicsPath path)
        {
            int geoCount = geometry.GetGeometryCount();
            var geometryType = geometry.GetGeometryType();
            switch (geometryType)
            {
                case wkbGeometryType.wkbMultiLineString:
                case wkbGeometryType.wkbMultiLineString25D:
                case wkbGeometryType.wkbMultiLineStringM:
                case wkbGeometryType.wkbMultiLineStringZM:
                    for (int i = 0; i < geoCount; i++)
                    {
                        using (var partGeo = geometry.GetGeometryRef(i))
                        {
                            path.StartFigure();
                            GetLines(drawArgs, partGeo, path);
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