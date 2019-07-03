using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    internal class LineLayer :FeatureLayer, ILineLayer
    {
        public LineLayer(DataSource dataSource):base(dataSource)
        {
        }

        public new ILineScheme Symbology { get =>base.Symbology as ILineScheme; set => base.Symbology=value; }

        public override Image<Rgba32> GetImage(Envelope envelope, Rectangle rectangle)
        {
            throw new System.NotImplementedException();
        }
    }
}