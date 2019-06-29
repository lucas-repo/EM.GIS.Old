using SixLabors.ImageSharp;
using SixLabors.Primitives;

namespace IMap.Gis.Symbology
{
    public interface IPolygonSymbol: IFeatureSymbol
    {
        PolygonSymbolType PatternType { get; }
        RectangleF Bounds { get; set; }
        ILineSymbolizer Outline { get; set; }
        bool UseOutline { get; set; }
        void Draw(IImage image, PointF[] polygon, float scale);
    }
}
