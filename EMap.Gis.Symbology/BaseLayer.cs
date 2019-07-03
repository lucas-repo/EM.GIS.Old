using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public abstract class BaseLayer :Disposable, IBaseLayer
    {
        public IScheme Symbology { get; set; }

        public abstract Image<Rgba32> GetImage(Envelope envelope, Rectangle rectangle) ;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Symbology?.Dispose();
                Symbology = null;
            }
            base.Dispose(disposing);
        }
        
    }
}