using EMap.Gis.Data;
using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

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
            int geometryCount = geometry.GetGeometryCount();
            drawArgs.Image.Mutate(context =>
            {
                ILineSymbolizer lineSymbolizer = symbolizer as ILineSymbolizer;
                for (int i = 0; i < geometryCount; i++)
                {
                    Geometry partGeo = geometry.GetGeometryRef(i);
                    int pointCount = partGeo.GetPointCount();
                    double[] coord = new double[2];
                    List<PointF> points = new List<PointF>();
                    float scaleSize = (float)symbolizer.GetScale(drawArgs);
                    for (int j = 0; j < pointCount; j++)
                    {
                        partGeo.GetPoint_2D(j, coord);
                        PointF point = drawArgs.ProjToPixelPointF(coord);
                        points.Add(point);
                    }
                    lineSymbolizer.DrawLine(context, scaleSize, points.ToArray());
                }
            });
        }

    }
}