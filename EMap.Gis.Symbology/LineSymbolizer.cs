
using System.Drawing;



using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public class LineSymbolizer : FeatureSymbolizer, ILineSymbolizer
    {
        public Color Color
        {
            get
            {
                Color color = new Color();
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
        public LineSymbolizer(Color color)
        {
            var symbol = new LineSimpleSymbol(color);
            Symbols = new LineSymbolCollection
            {
                symbol
            };
        }
        public LineSymbolizer(Color color, float width)
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
                symbol= new LineSimpleSymbol(Color.Cyan);
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

        public override void DrawLegend(Graphics context, Rectangle rectangle)
        {
            PointF[] points = new PointF[]
            {
                new PointF(rectangle.X, rectangle.Y + (rectangle.Height / 2)),
                new PointF(rectangle.Right, rectangle.Y + (rectangle.Height / 2))
            };
            GraphicsPath graphicsPath= points.ToPath();
            DrawLine(context, 1, graphicsPath);
        }

        public void DrawLine(Graphics context, float scale, GraphicsPath path)
        {
            foreach (var symbol in Symbols)
            {
                symbol.DrawLine(context, scale, path);
            }
        }
    }
}