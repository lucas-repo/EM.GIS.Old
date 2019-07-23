using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class PointSymbolizer : FeatureSymbolizer, IPointSymbolizer
    {
        public IList<IPointSymbol> Symbols { get; } = new List<IPointSymbol>();
        public SizeF Size
        {
            get
            {
                SizeF size = new SizeF();
                foreach (var symbol in Symbols)
                {
                    SizeF bsize = symbol.Size;
                    size.Width = Math.Max(size.Width, bsize.Width);
                    size.Height = Math.Max(size.Height, bsize.Height);
                }
                return size;
            }
            set
            {
                SizeF oldSize = Size;
                float dX = value.Width / oldSize.Width;
                float dY = value.Height / oldSize.Height;
                foreach (var symbol in Symbols)
                {
                    var os = symbol.Size;
                    symbol.Size = new SizeF(os.Width * dX, os.Height * dY);
                }
            }
        }
        
        public PointSymbolizer()
        {
            Configure();
        }
        public PointSymbolizer(bool selected)
        {
            Configure();
            if (!selected) return;

            IPointSymbol pointSymbol = Symbols[0];
            if (pointSymbol != null)
            {
                pointSymbol.Color = Rgba32.Cyan;
            }
        }
        public PointSymbolizer(IPointSymbol symbol)
        {
            Symbols.Add(symbol);
        }
        public PointSymbolizer(IEnumerable<IPointSymbol> symbols)
        {
            foreach (var item in symbols)
            {
                Symbols.Add(item);
            }
        }

        public PointSymbolizer(Rgba32 color, PointShape shape, float size)
        {
            IPointSymbol ss = new PointSimpleSymbol(color, shape, size);
            Symbols.Add(ss);
        }

        private void Configure()
        {
            IPointSimpleSymbol ss = new PointSimpleSymbol
            {
                Color = SymbologyGlobal.RandomColor(),
                Opacity = 1F,
                PointShape = PointShape.Rectangle
            };
            Symbols.Add(ss);
        }

        public override void LegendSymbolPainted(Image<Rgba32> image, Rectangle rectangle)
        {
            float scaleH = rectangle.Width / Size.Width;
            float scaleV = rectangle.Height / Size.Height;
            float scale = Math.Min(scaleH, scaleV);
            float dx = rectangle.X + (rectangle.Width / 2);
            float dy = rectangle.Y + (rectangle.Height / 2);
            AffineTransformBuilder atb = new AffineTransformBuilder().AppendTranslation(new PointF(dx, dy));
            AffineTransformBuilder atbInverse = new AffineTransformBuilder().AppendTranslation(new PointF(-dx, -dy));
            image.Mutate(x => x.Transform(atb));
            Draw(image, scale);
            image.Mutate(x => x.Transform(atbInverse));
        }

        public void Draw(Image<Rgba32> image, float scale)
        {
            foreach (var symbol in Symbols)
            {
                symbol.Draw(image, scale);
            }
        }
    }
}