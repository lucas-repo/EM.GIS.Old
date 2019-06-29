using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace IMap.Gis.Symbology
{
    internal class PolygonLayer : FeatureLayer, IPolygonLayer
    {
        public PolygonLayer(DataSource dataSource):base(dataSource)
        {
        }

        public new IPolygonScheme Symbology { get => base.Symbology as IPolygonScheme; set => base.Symbology = value; }

        public override Image<Rgba32> GetImage(Envelope envelope, Rectangle rectangle)
        {
            throw new System.NotImplementedException();
        }
    }
}