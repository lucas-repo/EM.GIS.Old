using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public abstract class FeatureCategory : Category, IFeatureCategory
    {
        public string FilterExpression { get; set; }
        private IFeatureSymbolizer _selectionSymbolizer;
        public IFeatureSymbolizer SelectionSymbolizer
        {
            get => _selectionSymbolizer;
            set
            {
                value.Parent = this;
                _selectionSymbolizer = value;
            } 
        }
        private IFeatureSymbolizer _symbolizer;
        public IFeatureSymbolizer Symbolizer
        {
            get => _symbolizer;
            set
            {
                value.Parent = this;
                _symbolizer = value;
            }
        }
        public override void LegendSymbolPainted(Image<Rgba32> image, Rectangle rectangle)
        {
            Symbolizer.LegendSymbolPainted(image, rectangle);
        }
    }
}