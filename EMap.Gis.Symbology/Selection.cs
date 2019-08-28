using EMap.Gis.Data;
using OSGeo.OGR;
using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public class Selection : Changeable, ISelection
    {
        public List<Feature> Features { get; }
        private Envelope _envelope;
        public Envelope Envelope
        {
            get
            {
                if (_envelope == null)
                {
                    _envelope = new Envelope();
                }
                for (int i = 0; i < Features.Count; i++)
                {
                    var feature = Features[i];
                    using (Geometry geometry = feature.GetGeometryRef())
                    {
                        using (Envelope tempEnvelope = new Envelope())
                        {
                            geometry.GetEnvelope(tempEnvelope); 
                            if (i == 0)
                            {
                                _envelope.MinX = tempEnvelope.MinX;
                                _envelope.MinY = tempEnvelope.MinY;
                                _envelope.MaxX = tempEnvelope.MaxX;
                                _envelope.MaxY = tempEnvelope.MaxY;
                            }
                            else
                            {
                                _envelope.ExpandToInclude(tempEnvelope);
                            }
                        }
                    }
                }
                return _envelope;
            }
        }

        public Selection()
        {
            Features = new List<Feature>();
        }
        protected override void Dispose(bool disposing)
        {
            if (_envelope != null)
            {
                _envelope.Dispose();
                _envelope = null;
            }
            if (Features.Count > 0)
            {
                foreach (var item in Features)
                {
                    item.Dispose();
                }
                Features.Clear();
            }
            base.Dispose(disposing);
        }
    }
}