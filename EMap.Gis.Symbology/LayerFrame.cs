using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.OGR;

namespace EMap.Gis.Symbology
{
    public class LayerFrame : Group, IFrame
    {
        public List<IBaseLayer> DrawingLayers { get; set; }
        private Envelope _viewExtents;
        public Envelope ViewExtents
        {
            get { return _viewExtents; }
            set { _viewExtents = value; }
        }
        public LayerFrame()
        {
            
        }
    }
}
