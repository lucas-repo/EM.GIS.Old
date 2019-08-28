using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using SixLabors.Shapes;

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
                Color = new Rgba32(Color.R, Color.G, Color.B, (byte)(val * 255));
            }
        }
        protected LineSymbol(LineSymbolType lineSymbolType)
        {
            LineSymbolType = lineSymbolType;
        }
        protected LineSymbol(Rgba32 color, LineSymbolType lineSymbolType) : base(color)
        {
            LineSymbolType = lineSymbolType;
        }
        protected LineSymbol(Rgba32 color, float width, LineSymbolType lineSymbolType) : base(color)
        {
            LineSymbolType = lineSymbolType;
            Width = width;
        }

        public virtual IPen<Rgba32> ToPen(float scale)
        {
            float width = scale * Width;
            IPen<Rgba32> pen = new Pen<Rgba32>(Color, width);
            return pen;
        }
        public void DrawLine(IImageProcessingContext<Rgba32> context, float scale, PointF[] points)
        {
            IPen<Rgba32> pen = ToPen(scale);
            context.DrawLines(pen, points);
        }
    }
}
