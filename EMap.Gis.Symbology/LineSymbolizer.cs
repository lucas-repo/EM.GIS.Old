using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;
using System;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public class LineSymbolizer : FeatureSymbolizer, ILineSymbolizer
    {
        public Rgba32 Color
        {
            get
            {
                Rgba32 color = new Rgba32();
                if (Symbols.Count > 0)
                {
                    color = Symbols[Symbols.Count - 1].Color;
                }
                return color;
            }
            set
            {
                if (Symbols.Count > 0)
                {
                    Symbols[Symbols.Count - 1].Color = value;
                }
            }
        }
        public float Width
        {
            get
            {
                float width = 0;
                foreach (var item in Symbols)
                {
                    if (item.Width > width)
                    {
                        width = item.Width;
                    }
                }
                return width;
            }
            set
            {
                float width = Width;
                var ratio = value / width;
                foreach (var item in Symbols)
                {
                    Symbols[Symbols.Count - 1].Width *= ratio;
                }
            }
        }

        public new ILineSymbolCollection Symbols { get => base.Symbols as ILineSymbolCollection; set => base.Symbols = value; }

        public LineSymbolizer()
        {
            var symbol = new LineSimpleSymbol();
            Symbols = new LineSymbolCollection
            {
                symbol
            };
        }
        public LineSymbolizer(Rgba32 color)
        {
            var symbol = new LineSimpleSymbol(color);
            Symbols = new LineSymbolCollection
            {
                symbol
            };
        }
        public LineSymbolizer(Rgba32 color, float width)
        {
            var symbol = new LineSimpleSymbol(color, width);
            Symbols = new LineSymbolCollection
            {
                symbol
            };
        }
        public LineSymbolizer(bool selected)
        {
            ILineSymbol symbol = null;
            if (selected)
            {
                symbol= new LineSimpleSymbol(Rgba32.Cyan);
            }
            else
            {
                symbol = new LineSimpleSymbol();
            }
            Symbols = new LineSymbolCollection
            {
                symbol
            };
        }

        public override void DrawLegend(IImageProcessingContext<Rgba32> context, Rectangle rectangle)
        {
            PointF[] points = new PointF[]
            {
                new PointF(rectangle.X, rectangle.Y + (rectangle.Height / 2)),
                new PointF(rectangle.Right, rectangle.Y + (rectangle.Height / 2))
            };
            DrawLine(context, 1, points);
        }

        public void DrawLine(IImageProcessingContext<Rgba32> context, float scale, PointF[] points)
        {
            foreach (var symbol in Symbols)
            {
                symbol.DrawLine(context, scale, points);
            }
        }
    }
}