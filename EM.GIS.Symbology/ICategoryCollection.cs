using System.Collections.Generic;

namespace EM.GIS.Symbology
{
    public interface ICategoryCollection : ILegendItemCollection
    {
        new ICategory this[int index] { get; set; }
        new IScheme Parent { get; set; }
        new IEnumerator<ICategory> GetEnumerator();
    }
}