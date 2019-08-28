using EMap.Gis.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public abstract class FeatureSymbolizer : Symbolizer, IFeatureSymbolizer
    {
        public ScaleMode ScaleMode { get; set; } = ScaleMode.Symbolic;

        public IFeatureSymbolCollection Symbols { get;  set; }

        public double GetScale(MapArgs drawArgs)
        {
            if (ScaleMode == ScaleMode.Geographic)
            {
                return drawArgs.Rectangle.Width / drawArgs.Envelope.Width();
            }
            return 1;
        }
    }
}