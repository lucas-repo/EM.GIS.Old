using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class PolygonPictureSymbol : PolygonSymbol, IPolygonPictureSymbol
    {
        public PolygonPictureSymbol() : base(PolygonSymbolType.Picture)
        { }
        public Image<Rgba32> Picture { get; set; }
        public float Angle { get; set; }
        public PointF Scale { get; set; } = new PointF(1, 1);
        private Image<Rgba32> GetPicture(Image<Rgba32> srcImg)
        {
            Image<Rgba32> destImg = null;
            if (srcImg != null)
            {
                destImg = srcImg.Clone();
                destImg.Mutate(x => x.Resize((int)Math.Ceiling(Picture.Width * Scale.X), (int)Math.Ceiling(Picture.Height * Scale.Y)).Rotate(Angle));
            }
            return destImg;
        }
        public override IBrush<Rgba32> GetBrush()
        {
            IBrush<Rgba32> brush = base.GetBrush();
            if (Picture == null) return brush;
            if (Scale.X == 0 || Scale.Y == 0) return brush;
            if (Scale.X * Picture.Width * Scale.Y * Picture.Height > 8000 * 8000) return brush; // The scaled image is too large, will cause memory exceptions.
            if (Picture != null)
            {
                Image<Rgba32> scaledBitmap = GetPicture(Picture);
                brush = new ImageBrush<Rgba32>(scaledBitmap);
            }
            return brush;
        }
    }
}
