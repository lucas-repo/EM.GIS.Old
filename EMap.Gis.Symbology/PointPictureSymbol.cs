
using System.Drawing;
using System.Drawing.Imaging;

namespace EMap.Gis.Symbology
{
    public class PointPictureSymbol : PointSymbol, IPointPictureSymbol
    {
        private Image _original;
        private Image _image;
        public Image Image
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

        private static Image MakeTransparent(Image image, float opacity)
        {
            if (image == null) return null;
            if (opacity == 1F) return image.Clone() as Image;

            Bitmap bmp = new Bitmap(image.Width, image.Height);
            Graphics g = Graphics.FromImage(bmp);
            float[][] ptsArray =
            {
                new float[] { 1, 0, 0, 0, 0 }, // R
                new float[] { 0, 1, 0, 0, 0 }, // G
                new float[] { 0, 0, 1, 0, 0 }, // B
                new[] { 0, 0, 0, opacity, 0 }, // A
                new float[] { 0, 0, 0, 0, 1 }
            };
            ColorMatrix clrMatrix = new ColorMatrix(ptsArray);
            ImageAttributes att = new ImageAttributes();
            att.SetColorMatrix(clrMatrix);
            g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, att);
            g.Dispose();
            return bmp;
        }

        public override void DrawPoint(Graphics graphics, float scale, PointF point)
        {
            float width = scale * Size.Width;
            float height = scale * Size.Height;
            float x = point.X - width / 2;
            float y = point.Y - height / 2;
            RectangleF rectangle = new RectangleF(x, y, width, height);
            if (_image != null)
            {
                graphics.DrawImage(_image, rectangle);
            }
            using (var path = rectangle.ToPath())
            {
                DrawOutLine(graphics, scale, path);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _image?.Dispose();
                _image = null;
                _original?.Dispose();
                _original = null;
            }
            base.Dispose(disposing);
        }
    }
}
