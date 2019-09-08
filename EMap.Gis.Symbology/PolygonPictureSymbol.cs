using System;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public class PolygonPictureSymbol : PolygonSymbol, IPolygonPictureSymbol
    {
        public PolygonPictureSymbol() : base(PolygonSymbolType.Picture)
        { }
        public Image Picture { get; set; }
        public float Angle { get; set; }
        public PointF Scale { get; set; } = new PointF(1, 1);
        public WrapMode WrapMode { get; set; }
        private Image GetPicture(Image srcImg)
        {
            Image destImg = null;
            if (srcImg != null)
            {
                destImg= new Bitmap((int)(Picture.Width * Scale.X), (int)(Picture.Height * Scale.Y));
                using (Graphics g = Graphics.FromImage(destImg))
                {
                    g.DrawImage(Picture, new Rectangle(0, 0, destImg.Width, destImg.Height), new Rectangle(0, 0, Picture.Width, Picture.Height), GraphicsUnit.Pixel);
                }
            }
            return destImg;
        }
        public override Brush GetBrush()
        {
            Brush brush = base.GetBrush();
            if (Picture == null) return brush;
            if (Scale.X == 0 || Scale.Y == 0) return brush;
            if (Scale.X * Picture.Width * Scale.Y * Picture.Height > 8000 * 8000) return brush; // The scaled image is too large, will cause memory exceptions.
            if (Picture != null)
            {
                using (Image scaledBitmap = GetPicture(Picture))
                {
                    brush = new TextureBrush(scaledBitmap, WrapMode);
                }
            }
            return brush;
        }
    }
}
