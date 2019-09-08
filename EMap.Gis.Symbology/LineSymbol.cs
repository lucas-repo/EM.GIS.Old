using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace EMap.Gis.Symbology
{
    public abstract class LineSymbol : FeatureSymbol, ILineSymbol
    {
        public LineSymbolType LineSymbolType { get; }
        public float Width { get; set; } = 1;
        public override float Opacity
        {
            get => base.Opacity;
            set
            {
                float val = value;
                if (val > 1) val = 1F;
                if (val < 0) val = 0F;
                Color = Color.FromArgb((byte)(val * 255),Color.R, Color.G, Color.B);
            }
        }
        protected LineSymbol(LineSymbolType lineSymbolType)
        {
            LineSymbolType = lineSymbolType;
        }
        protected LineSymbol(Color color, LineSymbolType lineSymbolType) : base(color)
        {
            LineSymbolType = lineSymbolType;
        }
        protected LineSymbol(Color color, float width, LineSymbolType lineSymbolType) : base(color)
        {
            LineSymbolType = lineSymbolType;
            Width = width;
        }

        public virtual Pen ToPen(float scale)
        {
            float width = scale * Width;
            Pen pen = new Pen(Color, width);
            return pen;
        }
        public void DrawLine(Graphics graphics, float scale, GraphicsPath path)
        {
            using (Pen pen = ToPen(scale))
            {
                graphics.DrawPath(pen, path); 
            }
        }
    }
}
