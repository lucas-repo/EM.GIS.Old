using OSGeo.OGR;
using System.Drawing;

namespace EMap.Gis.Symbology
{
    public interface IGetImage
    {
        Image GetImage(Envelope envelope, Rectangle rectangle) ;
    }
}