using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using EM.GIS.Data;
using EM.GIS.Geometries;
using EM.GIS.Serialization;
using OSGeo.OGR;

namespace EM.GIS.Symbology
{
    /// <summary>
    /// This is a class that organizes a list of renderable layers into a single "view" which might be
    /// shared by multiple displays. For instance, if you have a map control and a print preview control,
    /// you could draw the same data frame property on both, just by passing the graphics object for each.
    /// Be sure to handle any scaling or translation that you require through the Transform property
    /// of the graphics object as it will ultimately be that scale which is used to back-calculate the
    /// appropriate pixel sizes for point-size, line-width and other non-georeferenced characteristics.
    /// </summary>
    public abstract class Frame : Group, IFrame
    {
        private bool _extentsChanged;
        private IExtent _viewExtents;
        private int _extentChangedSuspensionCount;
        public event EventHandler UpdateMap;
        public event EventHandler<ExtentArgs> ViewExtentsChanged;

        public bool ExtentsInitialized { get; set; }

        public ILayer SelectedLayer => throw new NotImplementedException();

        public SmoothingMode SmoothingMode { get; set; } = SmoothingMode.AntiAlias;
        public virtual IExtent ViewExtents
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

        public ILayerCollection DrawingLayers { get; }

        public Frame()
        {
            DrawingLayers = new LayerCollection();
        }
        /// <summary>
        /// Fires the ExtentsChanged event
        /// </summary>
        /// <param name="ext">The new extent.</param>
        protected virtual void OnExtentsChanged(IExtent ext)
        {
            if (_extentChangedSuspensionCount > 0) return;
            ViewExtentsChanged?.Invoke(this, new ExtentArgs(ext));
        }

    }
}
