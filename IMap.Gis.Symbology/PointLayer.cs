using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace IMap.Gis.Symbology
{
    internal class PointLayer :FeatureLayer, IPointLayer
    {
        public PointLayer(DataSource dataSource):base(dataSource)
        {
        }

        public new  IPointScheme Symbology { get =>base.Symbology as IPointScheme; set => base.Symbology = value; }

        public override Image<Rgba32> GetImage(Envelope envelope, Rectangle rectangle)
        {
            throw new System.NotImplementedException();
        }
    }
}