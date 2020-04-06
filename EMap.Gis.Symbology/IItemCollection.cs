using System.Collections.Generic;
using System.Collections.Specialized;

namespace EMap.Gis.Symbology
{
    public interface IItemCollection<TParent, TChild> : IList<TChild>, ICollection<TChild>, IParentItem<TParent>, IMoveable, INotifyCollectionChanged
    {
    }
}