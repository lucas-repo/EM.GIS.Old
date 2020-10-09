using System.Collections.Generic;
using System.Collections.Specialized;

namespace EM.GIS.Symbology
{
    /// <summary>
    /// 元素集合接口（带父元素）
    /// </summary>
    /// <typeparam name="TParent">父元素类型</typeparam>
    /// <typeparam name="TChild">元素类型</typeparam>
    public interface IItemCollection<out TParent,  TChild> : IItemCollection<TChild>, IParentItem<TParent>, IEnumerable<TChild>
    {
    }
    /// <summary>
    /// 元素集合
    /// </summary>
    /// <typeparam name="TChild">元素类型</typeparam>
    public interface IItemCollection< TChild> : IList<TChild>, IEnumerable<TChild>,  IMoveable, INotifyCollectionChanged
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        TChild this[int index]
        {
            get;
            set;
        }

        //
        // 摘要:
        //     Determines the index of a specific item in the System.Collections.Generic.IList`1.
        //
        // 参数:
        //   item:
        //     The object to locate in the System.Collections.Generic.IList`1.
        //
        // 返回结果:
        //     The index of item if found in the list; otherwise, -1.
        int IndexOf(TChild item);

        //
        // 摘要:
        //     Inserts an item to the System.Collections.Generic.IList`1 at the specified index.
        //
        // 参数:
        //   index:
        //     The zero-based index at which item should be inserted.
        //
        //   item:
        //     The object to insert into the System.Collections.Generic.IList`1.
        //
        // 异常:
        //   TChild:System.ArgumentOutOfRangeException:
        //     index is not a valid index in the System.Collections.Generic.IList`1.
        //
        //   TChild:System.NotSupportedException:
        //     The System.Collections.Generic.IList`1 is read-only.
        void Insert(int index, TChild item);

        //
        // 摘要:
        //     Removes the System.Collections.Generic.IList`1 item at the specified index.
        //
        // 参数:
        //   index:
        //     The zero-based index of the item to remove.
        //
        // 异常:
        //   TChild:System.ArgumentOutOfRangeException:
        //     index is not a valid index in the System.Collections.Generic.IList`1.
        //
        //   TChild:System.NotSupportedException:
        //     The System.Collections.Generic.IList`1 is read-only.
        void RemoveAt(int index);
        //
        // 摘要:
        //     Gets the number of elements contained in the System.Collections.Generic.ICollection`1.
        //
        // 返回结果:
        //     The number of elements contained in the System.Collections.Generic.ICollection`1.
        int Count
        {
            get;
        }

        //
        // 摘要:
        //     Gets a value indicating whether the System.Collections.Generic.ICollection`1
        //     is read-only.
        //
        // 返回结果:
        //     true if the System.Collections.Generic.ICollection`1 is read-only; otherwise,
        //     false.
        bool IsReadOnly
        {
            get;
        }

        //
        // 摘要:
        //     Adds an item to the System.Collections.Generic.ICollection`1.
        //
        // 参数:
        //   item:
        //     The object to add to the System.Collections.Generic.ICollection`1.
        //
        // 异常:
        //   TChild:System.NotSupportedException:
        //     The System.Collections.Generic.ICollection`1 is read-only.
        void Add(TChild item);

        //
        // 摘要:
        //     Removes all items from the System.Collections.Generic.ICollection`1.
        //
        // 异常:
        //   TChild:System.NotSupportedException:
        //     The System.Collections.Generic.ICollection`1 is read-only.
        void Clear();

        //
        // 摘要:
        //     Determines whether the System.Collections.Generic.ICollection`1 contains a specific
        //     value.
        //
        // 参数:
        //   item:
        //     The object to locate in the System.Collections.Generic.ICollection`1.
        //
        // 返回结果:
        //     true if item is found in the System.Collections.Generic.ICollection`1; otherwise,
        //     false.
        bool Contains(TChild item);

        //
        // 摘要:
        //     Copies the elements of the System.Collections.Generic.ICollection`1 to an System.Array,
        //     starting at a particular System.Array index.
        //
        // 参数:
        //   array:
        //     The one-dimensional System.Array that is the destination of the elements copied
        //     from System.Collections.Generic.ICollection`1. The System.Array must have zero-based
        //     indexing.
        //
        //   arrayIndex:
        //     The zero-based index in array at which copying begins.
        //
        // 异常:
        //   TChild:System.ArgumentNullException:
        //     array is null.
        //
        //   TChild:System.ArgumentOutOfRangeException:
        //     arrayIndex is less than 0.
        //
        //   TChild:System.ArgumentException:
        //     The number of elements in the source System.Collections.Generic.ICollection`1
        //     is greater than the available space from arrayIndex to the end of the destination
        //     array.
        void CopyTo(TChild[] array, int arrayIndex);

        //
        // 摘要:
        //     Removes the first occurrence of a specific object from the System.Collections.Generic.ICollection`1.
        //
        // 参数:
        //   item:
        //     The object to remove from the System.Collections.Generic.ICollection`1.
        //
        // 返回结果:
        //     true if item was successfully removed from the System.Collections.Generic.ICollection`1;
        //     otherwise, false. This method also returns false if item is not found in the
        //     original System.Collections.Generic.ICollection`1.
        //
        // 异常:
        //   TChild:System.NotSupportedException:
        //     The System.Collections.Generic.ICollection`1 is read-only.
        bool Remove(TChild item);
    }
}