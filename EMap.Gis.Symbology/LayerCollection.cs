using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public class LayerCollection : LegendItemCollection, ILayerCollection
    {
        private IFrame _mapFrame;
        public IFrame MapFrame
        {
            get
            {
                return _mapFrame;
            }
            set
            {
                _mapFrame = value;
                foreach (IBaseLayer item in this)
                {
                    item.MapFrame = value;
                }
            }
        }

        public new  IGroup Parent { get => base.Parent as IGroup; set => base.Parent=value; }

        public new IBaseLayer this[int index] { get => base[index] as IBaseLayer; set => base[index]=value; }

        public LayerCollection(IFrame containerFrame):base(containerFrame)
        {
            _mapFrame = containerFrame;
        }
        public LayerCollection(IFrame frame, IGroup parent):base(parent)
        {
            _mapFrame = frame;
        }
        public LayerCollection()
        {
        }

        public new  IEnumerator<IBaseLayer> GetEnumerator()
        {
            foreach (var item in Items)
            {
                yield return item as IBaseLayer;
            }
        }
    }
}