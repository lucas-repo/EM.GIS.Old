using OSGeo.OGR;
using System.Drawing;


namespace EMap.Gis.Symbology
{
    public interface IGetImage
    {
        Bitmap GetImage(Envelope envelope, Rectangle rectangle) ;
    }
}