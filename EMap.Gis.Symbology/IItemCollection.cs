using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface IItemCollection<TParent, TChild> : IList<TChild>, ICollection<TChild>, IParentItem<TParent>, IMoveable
    {
    }
}