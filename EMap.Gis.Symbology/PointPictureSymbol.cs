using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace EMap.Gis.Symbology
{
    public class PointPictureSymbol : PointSymbol, IPointPictureSymbol
    {
        private Image<Rgba32> _original;
        private Image<Rgba32> _image;
        public Image<Rgba32> Image
        {
            get
            {
                return _image;
            }
            set
            {
                if (_original != null && _original != value)
                {
                    _original.Dispose();
                    _original = null;
                }
                if (_image != null && _image != _original && _image != value)
                {
                    _image.Dispose();
                    _image = null;
                }
                _original = value;
                _image = MakeTransparent(value, Opacity);
            }
        }
        public string ImageBase64 { get; set; }
        public string ImagePath { get; set; }
        public override float Opacity
        {
            get { return base.Opacity; }
            set
            {
                if (_image != null && _image != _original)
                    _image.Dispose();
                _image = MakeTransparent(_original, value);
                base.Opacity = value;
            }
        }

        public PointPictureSymbol() : base(PointSymbolType.Picture)
        { }

        private static Image<Rgba32> MakeTransparent(Image<Rgba32> image, float opacity)
        {
            Image<Rgba32> destImage = null;
            if (image == null) return destImage;
            destImage = new Image<Rgba32>(image.Width, image.Height);
            destImage.Mutate(x => x.DrawImage(image, opacity));
            return destImage;
        }

        public override void DrawPoint(IImageProcessingContext context, float scale, PointF point)
        {
            float width = scale * Size.Width;
            float height = scale * Size.Height;
            float x = point.X - scale * Size.Width / 2;
            float y = point.Y - scale * Size.Height / 2;
            RectangleF rectangle = new RectangleF(x, y, width, height);
            if (Image != null)
            {
                PointF position = new PointF(Size.Width / 2, Size.Height / 2);
                AffineTransformBuilder affineTransformBuilder = new AffineTransformBuilder()
                    .AppendScale(scale)
                    .AppendTranslation(new PointF(rectangle.X, rectangle.Y));
                //using (Image<Rgba32> tmpImage = Image.Clone())
                //{
                //    tmpImage.Mutate(processing =>
                //    {
                //        processing.Transform(affineTransformBuilder);
                //        context.DrawImage(tmpImage, 1);
                //    });
                //}
                Image.Mutate(processing =>
                {
                    processing.Transform(affineTransformBuilder);
                    context.DrawImage(Image, 1);
                });
            }
            PointF[] points = rectangle.ToPoints();
            DrawOutLine(context, scale, points.ToPath());
        }
    }
}
