using EMap.Gis.Data;
using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    public class MapArgs : Disposable, IProj
    {
        private Image<Rgba32> _image;
        public Image<Rgba32> Image { get => _image; }
        private Envelope _envelope;
        public Envelope Envelope { get => _envelope; }
        public Rectangle Rectangle { get; }
        public double Dx { get; }
        public double Dy { get; }
        public MapArgs(Image<Rgba32> image, Envelope envelope, Rectangle rectangle)
        {
            _image = image;
            _envelope = envelope;
            Rectangle = rectangle;
            double worldWidth = envelope.Width();
            double worldHeight = envelope.Height();
            Dx = rectangle.Width != 0 ? worldWidth / rectangle.Width : 0;
            Dy = rectangle.Height != 0 ? worldHeight / rectangle.Height : 0;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _image?.Dispose();
                _image = null;
                _envelope?.Dispose();
                _envelope = null;
            }
            base.Dispose(disposing);
        }
    }
}
