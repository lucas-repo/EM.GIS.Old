using System.Collections.Generic;
using System.Collections.Specialized;

namespace EM.GIS.Symbology
{
    /// <summary>
    /// 元素集合（带父元素）
    /// </summary>
    /// <typeparam name="TParent">父元素类型</typeparam>
    /// <typeparam name="TChild">元素类型</typeparam>
    public interface IItemCollection<TParent, TChild> : IItemCollection<TChild>, IParentItem<TParent>
    {
    }
    /// <summary>
    /// 元素集合
    /// </summary>
    /// <typeparam name="TChild">元素类型</typeparam>
    public interface IItemCollection<TChild> : IList<TChild>, ICollection<TChild>, IMoveable, INotifyCollectionChanged
    {
    }
}