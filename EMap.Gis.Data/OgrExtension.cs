using OSGeo.OGR;
using System;

namespace EMap.Gis.Data
{
    public static class OgrExtension
    {
        public static double Width(this Envelope envelope)
        {
            return envelope != null ? envelope.MaxX - envelope.MinX : 0;
        }
        public static double Height(this Envelope envelope)
        {
            return envelope != null ? envelope.MaxY - envelope.MinY : 0;
        }
        public static Geometry ToGeometry(this Envelope envelope)
        {
            Geometry ring = new Geometry(wkbGeometryType.wkbLinearRing);
            ring.AddPoint_2D(envelope.MinX, envelope.MinY);
            ring.AddPoint_2D(envelope.MaxX, envelope.MinY);
            ring.AddPoint_2D(envelope.MaxX, envelope.MaxY);
            ring.AddPoint_2D(envelope.MinX, envelope.MaxY);
            ring.CloseRings();
            Geometry polygon = new Geometry(wkbGeometryType.wkbPolygon);
            polygon.AddGeometry(ring);
            return polygon;
        }
        public static void ExpandToInclude(this Envelope srcEnvelope, Envelope destEnvelope)
        {
            if (srcEnvelope == null || destEnvelope == null)
            {
                return;
            }
            srcEnvelope.MinX = Math.Min(srcEnvelope.MinX, destEnvelope.MinX);
            srcEnvelope.MinY = Math.Min(srcEnvelope.MinY, destEnvelope.MinY);
            srcEnvelope.MaxX = Math.Max(srcEnvelope.MaxX, destEnvelope.MaxX);
            srcEnvelope.MaxY = Math.Min(srcEnvelope.MaxY, destEnvelope.MaxY);
        }
        public static int GetPointCount(this Feature feature)
        {
            int count = 0;
            if (feature != null)
            {
                using (Geometry geometry = feature.GetGeometryRef())
                {
                    geometry.GetPointCount(ref count);
                }
            }
            return count;
        }
        public static void GetPointCount(this Geometry geometry, ref int count)
        {
            if (geometry != null)
            {
                int geoCount = geometry.GetGeometryCount();
                if (geoCount > 0)
                {
                    for (int i = 0; i < geoCount; i++)
                    {
                        using (var childGeo = geometry.GetGeometryRef(i))
                        {
                            childGeo.GetPointCount(ref count);
                        }
                    }
                }
                else
                {
                    count += geometry.GetPointCount();
                }
            }
        }
    }
}
