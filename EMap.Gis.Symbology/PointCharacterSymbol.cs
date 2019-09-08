using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;

namespace EMap.Gis.Symbology
{
    public class PointCharacterSymbol : PointSymbol, IPointCharacterSymbol
    {
        public PointCharacterSymbol() : base(PointSymbolType.Character)
        { }

        public UnicodeCategory Category => char.GetUnicodeCategory(Character);

        public char Character { get; set; }
        public string FontFamilyName { get; set; }
      
        public FontStyle Style { get; set; }
        
        protected override void OnRandomize(Random generator)
        {
            Color = generator.NextColor();
            Opacity = generator.NextFloat();
            Character = (char)generator.Next(0, 255);
            int fontCount = FontFamily.Families.Count();
            FontFamilyName = FontFamily.Families[generator.Next(0, fontCount - 1)].Name;
            Style = generator.NextEnum<FontStyle>();
            base.OnRandomize(generator);
        }

        public override void DrawPoint(Graphics graphics, float scale, PointF point)
        {
            string text = new string(new[] { Character });
            float fontPointSize = Size.Height * scale;
            Font font = new Font(FontFamilyName, fontPointSize, Style);
            SizeF sizeF= graphics.MeasureString(text, font);
            float x = point.X - sizeF.Width / 2;
            float y = point.Y - sizeF.Height / 2;
            PointF location = new PointF(x, y);
            using (Brush brush = new SolidBrush(Color))
            {
                graphics.DrawString(text, font, brush, location); 
            }
            using (GraphicsPath graphicsPath = sizeF.ToPath())
            {
                DrawOutLine(graphics, scale, graphicsPath);
            }
        }
    }
}
