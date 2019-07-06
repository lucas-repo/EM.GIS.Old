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
        public float Width { get; set ; }
        public override float Opacity
        {
            get => base.Opacity;
            set
            {
                float val = value;
                if (val > 1) val = 1F;
                if (val < 0) val = 0F;
                Color =new Rgba32(Color.R, Color.G, Color.B, (byte)(val * 255));
            }
        }
        protected LineSymbol(LineSymbolType lineSymbolType)
        {
            LineSymbolType = lineSymbolType;
        }

        public  void DrawPath(Image<Rgba32> image, float scale, PointF[] points)
        {
            IPen<Rgba32> pen = ToPen(scale); 
            image.Mutate(x => x.DrawLines(pen, points));
        }
        public virtual IPen<Rgba32> ToPen(float scale)
        {
            float width = scale * Width;
            IPen<Rgba32> pen = new Pen<Rgba32>(Color, width);
            return pen;
        }
    }
}
