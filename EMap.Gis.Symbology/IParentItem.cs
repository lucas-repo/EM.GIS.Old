namespace EMap.Gis.Symbology
{
    public interface IParentItem<T>
    {
        T Parent { get; set; }
    }
}