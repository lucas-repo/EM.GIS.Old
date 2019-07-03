using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public abstract class Symbolizer : CopyBase, ISymbolizer
    {
        public abstract void Draw(Image<Rgba32> image, Rectangle rectangle);
    }
}