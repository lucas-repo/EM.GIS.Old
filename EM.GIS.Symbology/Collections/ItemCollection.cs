using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace EM.GIS.Symbology
{
    [Serializable]
    public abstract class ItemCollection<TParent, TChild> :  IItemCollection<TParent, TChild>
    {
        protected ObservableCollection<TChild> Items { get; }
        public TChild this[int index] { get => Items[index]; set => Items[index]=value; }
        [NonSerialized]
        private TParent _parent;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public TParent Parent { get => _parent; set => _parent = value; }

        public int Count => Items.Count;

        public bool IsReadOnly => false;
        public ItemCollection()
        {
            Items = new ObservableCollection<TChild>();
            Items.CollectionChanged += Items_CollectionChanged;
        }
        public ItemCollection(TParent parent):this()
        {
            _parent = parent;
        }
        private void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Action setOldItemsAction = new Action(() =>
            {
                foreach (var item in e.OldItems)
                {
                    if (item is IParentItem<ILegendItem> t)
                    {
                        t.Parent = null;
                    }
                }
            });
            Action setNewItemsAction = new Action(() =>
            {
                foreach (var item in e.NewItems)
                {
                    if (item is IParentItem<TParent> t)
                    {
                        t.Parent = Parent;
                    }
                }
            });
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    setNewItemsAction.Invoke();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    setOldItemsAction.Invoke();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    setOldItemsAction.Invoke();
                    setNewItemsAction.Invoke();
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    setOldItemsAction.Invoke();
                    break;
            }
            CollectionChanged?.Invoke(this, e);
        }
     

        public virtual void Add(TChild item)
        {
            Items.Add(item);
        }

        public virtual void Clear()
        {
            Items.Clear();
        }

        public virtual bool Contains(TChild item)
        {
            return Items.Contains(item);
        }

        public virtual void CopyTo(TChild[] array, int arrayIndex)
        {
            Items.CopyTo(array, arrayIndex);
        }

        public virtual IEnumerator<TChild> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public virtual int IndexOf(TChild item)
        {
            return Items.IndexOf(item);
        }

        public virtual void Insert(int index, TChild item)
        {
            Items.Insert(index, item);
        }

        public void Move(int oldIndex, int newIndex)
        {
            Items.Move(oldIndex, newIndex);
        }

        public virtual bool Remove(TChild item)
        {
            return Items.Remove(item);
        }

        public virtual void RemoveAt(int index)
        {
            Items.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Items.GetEnumerator();
        }
    }
}