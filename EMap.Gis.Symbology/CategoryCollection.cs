using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace EMap.Gis.Symbology
{
    public class CategoryCollection<T>: ObservableCollection<T>where T:ICategory
    {
        private IScheme _scheme;
        public IScheme Scheme
        {
            get
            {
                return _scheme;
            }
            set
            {
                _scheme = value;
                UpdateItemParentPointers();
            }
        }
        public CategoryCollection(IScheme scheme)
        {
            Scheme = scheme;
            CollectionChanged += CategoryCollection_CollectionChanged;
        }

        private void CategoryCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Action setOldItemsAction = new Action(() =>
              {
                  foreach (var item in e.OldItems)
                  {
                      if (item is T t)
                      {
                          t.Parent = null;
                      }
                  }
              });
            Action setNewItemsAction = new Action(() =>
              {
                  foreach (var item in e.NewItems)
                  {
                      if (item is T t)
                      {
                          t.Parent = Scheme;
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
        }

        private void UpdateItemParentPointers()
        {
            foreach (var item in this)
            {
                if (_scheme == null)
                {
                    item.Parent = null;
                }
                else
                {
                    item.Parent = Scheme;
                }
            }
        }
    }
}