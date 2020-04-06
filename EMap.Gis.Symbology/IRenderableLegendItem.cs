namespace EMap.Gis.Symbology
{
    /// <summary>
    /// Items with this setup can both be organized as an item,
    /// and feature the elemental control methods and properties
    /// around drawing. Layers, MapFrames, groups etc can fall in this
    /// category.
    /// </summary>
    public interface IRenderableLegendItem : IRenderable, ILegendItem
    {
    }
}