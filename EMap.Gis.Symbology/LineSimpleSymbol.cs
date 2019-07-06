using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class LineSimpleSymbol : LineSymbol, ILineSimpleSymbol
    {

        public LineSimpleSymbol() : this(LineSymbolType.Simple)
        { }
        protected LineSimpleSymbol(LineSymbolType lineSymbolType) : base(lineSymbolType)
        { }
        public LineSimpleSymbol(float width, Rgba32 color) : this()
        {
            Width = width;
            Color = color;
        }
        public virtual DashStyle DashStyle { get; set; }
        
        public override IPen<Rgba32> ToPen(float scale)
        {
            float width = scale * Width;
            IPen<Rgba32> pen =null; 
            switch (DashStyle)
            {
                case DashStyle.Solid:
                    pen = Pens.Solid(Color, width);
                    break;
                case DashStyle.Dash:
                    pen = Pens.Dash(Color, width);
                    break;
                case DashStyle.Dot:
                    pen = Pens.Dot(Color, width);
                    break;
                case DashStyle.DashDot:
                    pen = Pens.DashDot(Color, width);
                    break;
                case DashStyle.DashDotDot:
                    pen = Pens.DashDotDot(Color, width);
                    break;
            }
            return pen;
        }
        protected override void OnRandomize(Random generator)
        {
            Color = generator.NextColor();
            Opacity = generator.NextFloat();
            Width = generator.NextFloat(10);
            DashStyle = generator.NextEnum<DashStyle>();
            base.OnRandomize(generator);
        }
    }
}