using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

namespace EMap.Gis.Symbology
{
    public class PointCharacterSymbol : PointSymbol, IPointCharacterSymbol
    {
        public PointCharacterSymbol() : base(PointSymbolType.Character)
        { }

        public UnicodeCategory Category => char.GetUnicodeCategory(Character);

        public char Character { get; set; }
        public string FontFamilyName { get; set; }
        [NonSerialized]
        private FontFamily _fontFamily;
        public FontFamily FontFamily
        {
            get
            {
                if (_fontFamily?.Name != FontFamilyName)
                {
                    _fontFamily = SystemFonts.Find(FontFamilyName);
                }
                return _fontFamily;
            }
            set
            {
                FontFamilyName = value?.Name;
                _fontFamily = value;
            }
        }

        public FontStyle Style { get; set; }
        public static RectangleF MeasureText(string text, Font fnt)
        {
            RendererOptions rendererOptions = new RendererOptions(fnt);
            IPathCollection paths = TextBuilder.GenerateGlyphs(text, rendererOptions);
            return paths.Bounds;
        }
        public override void Draw(Image<Rgba32> image, float scale)
        {
            string text = new string(new[] { Character });
            float fontPointSize = Size.Height * scale;
            Font font = new Font(FontFamily, fontPointSize, Style);
            RectangleF bounds = MeasureText(text, font);
            float x = -bounds.Width / 2;
            float y = -bounds.Height / 2;
            PointF location = new PointF(x, y);
            image.Mutate(p => p.DrawText(text, font, Color, location));
            IPath path= bounds.ToPath();
            DrawOutLine(image, path, scale);
        }
        protected override void OnRandomize(Random generator)
        {
            Color = generator.NextColor();
            Opacity = generator.NextFloat();
            Character = (char)generator.Next(0, 255);
            int fontCount = SystemFonts.Families.Count();
            FontFamilyName = SystemFonts.Families.ElementAt(generator.Next(0, fontCount - 1)).Name;
            Style = generator.NextEnum<FontStyle>();
            base.OnRandomize(generator);
        }
    }
}
