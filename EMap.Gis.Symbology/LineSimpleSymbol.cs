using System;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    [Serializable]
    public class LineSimpleSymbol : LineSymbol, ILineSimpleSymbol
    {

        public LineSimpleSymbol() : this(LineSymbolType.Simple)
        { }
        protected LineSimpleSymbol(LineSymbolType lineSymbolType) : base(lineSymbolType)
        { }
        public LineSimpleSymbol(Rgba32 color) : base(color, LineSymbolType.Simple)
        {
        }
        public LineSimpleSymbol(Rgba32 color, float width) : base(color, width, LineSymbolType.Simple)
        {
        }
        public virtual DashStyle DashStyle { get; set; }

        public override IPen ToPen(float scale)
        {
            float width = scale * Width;
            IPen pen = null;
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