using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Data
{
    public static class OgrExtension
    {
        public static double Width(this Envelope envelope)
        {
            return envelope != null ? envelope.MaxX - envelope.MinX : 0;
        }
        public static double[] Center(this Envelope envelope)
        {
            double[] center = null;
            if (envelope != null)
            {
                center = new double[2];
                center[0] = (envelope.MinX + envelope.MaxX) / 2;
                center[1] = (envelope.MinY + envelope.MaxY) / 2;
            }
            return center;
        }
        public static void SetCenter(this Envelope envelope,double[] center, double width, double height)
        {
            if (Equals(center, null)) throw new ArgumentNullException(nameof(center));

            envelope.SetCenter(center[0], center[1], width, height);
        }
        public static void SetCenter(this Envelope envelope, double centerX, double centerY, double width, double height)
        {
            envelope.MinX = centerX - (width / 2);
            envelope.MaxX = centerX + (width / 2);
            envelope.MinY = centerY - (height / 2);
            envelope.MaxY = centerY + (height / 2);
        }
        public static Envelope Copy(this Envelope envelope)
        {
            Envelope copy = null;
            if (envelope != null)
            {
                copy = new Envelope()
                {
                    MinX = envelope.MinX,
                    MaxX = envelope.MaxX,
                    MinY = envelope.MinY,
                    MaxY = envelope.MaxY
                };
            }
            return copy;
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
        public static Extent ToExtent(this Envelope envelope)
        {
            Extent extent = new Extent(envelope.MinX, envelope.MinY, envelope.MaxX, envelope.MaxY);
            return extent;
        }
        public static Geometry ToGeometry(this Extent envelope)
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
        public static void ExpandBy(this Envelope envelope, double deltaX, double deltaY)
        {
            if (envelope != null)
            {
                envelope.MinX -= deltaX;
                envelope.MaxX += deltaX;
                envelope.MinY -= deltaY;
                envelope.MaxY += deltaY;
                if (envelope.MinX > envelope.MaxX || envelope.MinY > envelope.MaxY)
                {
                    envelope.MinX= double.NaN;
                    envelope.MaxX = double.NaN;
                    envelope.MinY = double.NaN;
                    envelope.MaxY = double.NaN;
                }
            }
        }
    }
}
