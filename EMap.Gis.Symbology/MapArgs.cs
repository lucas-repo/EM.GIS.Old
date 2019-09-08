using EMap.Gis.Data;
using OSGeo.OGR;
using System.Drawing;

namespace EMap.Gis.Symbology
{
    public class MapArgs : Disposable, IProj
    {
        public Graphics Device { get; }
        public Envelope Envelope { get ; }
        public Rectangle Rectangle { get; }
        public double Dx { get; }
        public double Dy { get; }
        public MapArgs(Envelope envelope, Rectangle rectangle)
        {
            Envelope = envelope;
            Rectangle = rectangle;
            double worldWidth = envelope.Width();
            double worldHeight = envelope.Height();
            Dx = rectangle.Width != 0 ? worldWidth / rectangle.Width : 0;
            Dy = rectangle.Height != 0 ? worldHeight / rectangle.Height : 0;
        }
        public MapArgs( Envelope envelope, Rectangle rectangle, Graphics g):this(envelope, rectangle)
        {
            Device = g;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Device?.Dispose();
                Envelope?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
