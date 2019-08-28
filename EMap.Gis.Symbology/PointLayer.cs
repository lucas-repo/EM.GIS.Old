using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using EMap.Gis.Data;

namespace EMap.Gis.Symbology
{
    public class PointLayer : FeatureLayer, IPointLayer
    {
        public new IPointScheme Symbology
        {
            get => base.Symbology as IPointScheme;
            set => base.Symbology = value;
        }
        public new IPointCategory DefaultCategory
        {
            get => base.DefaultCategory as IPointCategory;
            set => base.DefaultCategory = value;
        }
        public PointLayer(DataSource dataSource) : base(dataSource)
        {
            DefaultCategory = new PointCategory();
            Symbology = new PointScheme();
            Symbology.Categories.Add(DefaultCategory);
        }

        protected override void DrawGeometry(MapArgs drawArgs, IFeatureSymbolizer symbolizer, Geometry geometry)
        {
            int geometryCount = geometry.GetGeometryCount();
            drawArgs.Image.Mutate(context =>
            {
                for (int i = 0; i < geometryCount; i++)
                {
                    Geometry partGeo = geometry.GetGeometryRef(i);
                    int pointCount = partGeo.GetPointCount();
                    double[] coord = new double[2];
                    float scaleSize = (float)symbolizer.GetScale(drawArgs);
                    for (int j = 0; j < pointCount; j++)
                    {
                        partGeo.GetPoint_2D(j, coord);
                        PointF point = drawArgs.ProjToPixelPointF(coord);
                        (symbolizer as IPointSymbolizer).DrawPoint(context, scaleSize, point);
                    }
                }
            });
        }
    }
}