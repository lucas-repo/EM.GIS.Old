using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace EMap.Gis.Symbology
{
    public class LineCartographicSymbol : LineSimpleSymbol, ILineCartographicSymbol
    {
        public float[] Pattern { get; set; } = new float[0];
        public List<ILineDecoration> Decorations { get;  } = new List<ILineDecoration>();
        public override DashStyle DashStyle
        {
            get => base.DashStyle;
            set
            {
                switch (DashStyle)
                {
                    case DashStyle.Dash:
                        Pattern = new float[] { 3f, 1f, 1f, 1f };
                        break;
                    case DashStyle.Dot:
                        Pattern = new float[] { 1f, 1f };
                        break;
                    case DashStyle.DashDot:
                        Pattern = new float[] { 3f, 1f, 1f, 1f };
                        break;
                    case DashStyle.DashDotDot:
                        Pattern = new float[] { 3f, 1f, 1f, 1f, 1f, 1f };
                        break;
                }
                base.DashStyle = value;
            }
        }
        public LineCartographicSymbol() : this(LineSymbolType.Cartographic)
        { }
        protected LineCartographicSymbol(LineSymbolType lineSymbolType) : base(lineSymbolType)
        {
        }
        public override IPen<Rgba32> ToPen(float scale)
        {
            float width = scale * Width;
            IPen<Rgba32> pen = new Pen<Rgba32>(Color, width, Pattern);
            return pen;
        }
    }
}
