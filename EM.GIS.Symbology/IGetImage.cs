using OSGeo.OGR;
using System.Drawing;


namespace EM.GIS.Symbology
{
    public interface IGetImage
    {
        Bitmap GetImage(Envelope envelope, Rectangle rectangle) ;
    }
}