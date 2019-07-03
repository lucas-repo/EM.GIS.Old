using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class FeatureSymbolizer : Symbolizer, IFeatureSymbolizer
    {
        public IFeatureSymbol FeatureSymbol { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override void Draw(Image<Rgba32> image, Rectangle rectangle)
        {
            throw new System.NotImplementedException();
        }
    }
}