using EM.GIS.Data;
using OSGeo.OGR;
using System.Drawing;

namespace EM.GIS.Symbology
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
        public PointLayer(IFeatureSet featureSet) : base(featureSet)
        {
            DefaultCategory = new PointCategory();
            Symbology = new PointScheme();
            Symbology.Categories.Add(DefaultCategory);
        }

        protected override void DrawGeometry(MapArgs drawArgs, IFeatureSymbolizer symbolizer, Geometry geometry)
        {
            int geometryCount = geometry.GetGeometryCount();
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
                    (symbolizer as IPointSymbolizer).DrawPoint(drawArgs.Device, scaleSize, point);
                }
            }
        }
    }
}