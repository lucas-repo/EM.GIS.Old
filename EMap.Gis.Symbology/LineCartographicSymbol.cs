using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace EMap.Gis.Symbology
{
    [Serializable]
    public class LineCartographicSymbol : LineSimpleSymbol, ILineCartographicSymbol
    {
        protected static readonly float[] DashDotPattern = new float[4]
        {
                3f,
                1f,
                1f,
                1f
        };
        protected static readonly float[] DashDotDotPattern = new float[6]
        {
            3f,
            1f,
            1f,
            1f,
            1f,
            1f
        };
        protected static readonly float[] DottedPattern = new float[2]
        {
            1f,
            1f
        };
        protected static readonly float[] DashedPattern = new float[2]
        {
            3f,
            1f
        };
        protected static readonly float[] EmptyPattern = new float[0];
        public float[] Pattern { get; set; } = EmptyPattern;
        public List<ILineDecoration> Decorations { get; } = new List<ILineDecoration>();
        public override DashStyle DashStyle
        {
            get => base.DashStyle;
            set
            {
                switch (DashStyle)
                {
                    case DashStyle.Dash:
                        Pattern = DashedPattern;
                        break;
                    case DashStyle.Dot:
                        Pattern = DottedPattern;
                        break;
                    case DashStyle.DashDot:
                        Pattern = DashDotPattern;
                        break;
                    case DashStyle.DashDotDot:
                        Pattern = DashDotDotPattern;
                        break;
                    case DashStyle.Solid:
                    case DashStyle.Custom:
                        Pattern = EmptyPattern;
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
        public override IPen ToPen(float scale)
        {
            float width = scale * Width;
            IPen pen = new Pen(Color, width,Pattern); 
            return pen;
        }
    }
}
