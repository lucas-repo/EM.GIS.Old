using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using EMap.Gis.Data;
using EMap.Gis.Serialization;
using OSGeo.OGR;

namespace EMap.Gis.Symbology
{
    public abstract class Frame : Group, IFrame
    {
        private bool _extentsChanged;
        private Extent _viewExtents;
        private int _extentChangedSuspensionCount;
        public List<IBaseLayer> DrawingLayers { get; set; }
        public event EventHandler UpdateMap;
        public event EventHandler<ExtentArgs> ViewExtentsChanged;

        public bool ExtentsInitialized { get; set; }

        public IBaseLayer SelectedLayer => throw new NotImplementedException();

        public SmoothingMode SmoothingMode { get; set; } = SmoothingMode.AntiAlias;
        public virtual Extent ViewExtents
        {
            get
            {
                return _viewExtents ?? (_viewExtents = Extent != null ? Extent.Copy() : new Extent(-180, -90, 180, 90));
            }

            set
            {
                _viewExtents = value;
                _extentsChanged = true;
                if (_extentChangedSuspensionCount == 0)
                {
                    OnExtentsChanged(_viewExtents);
                }
            }
        }

        public Frame()
        {
            DrawingLayers = new List<IBaseLayer>();
        }
        /// <summary>
        /// Fires the ExtentsChanged event
        /// </summary>
        /// <param name="ext">The new extent.</param>
        protected virtual void OnExtentsChanged(Extent ext)
        {
            if (_extentChangedSuspensionCount > 0) return;
            ViewExtentsChanged?.Invoke(this, new ExtentArgs(ext));
        }

    }
}
