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
    [Serializable]
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

        public virtual IPen ToPen(float scale)
        {
            float width = scale * Width;
            IPen pen = new Pen(Color, width);
            return pen;
        }
        public virtual void DrawLine(IImageProcessingContext context, float scale, IPath path)
        {
            IPen pen = ToPen(scale); 
            context.Draw(pen, path);
        }
    }
}
