using EM.GIS.Data;
using EM.GIS.Geometries;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EM.GIS.Gdals
{
    public static class OgrExtension
    {

        public static ICoordinate ToCoordinate(this IEnumerable<double> array)
        {
            ICoordinate coordinate = null;
            if (array == null)
            {
                return coordinate;
            }
            var count = array.Count();
            if (count >= 0)
            {
                coordinate = new Coordinate();
                for (int i = 0; i < coordinate.MaxPossibleOrdinates && i < count; i++)
                {
                    coordinate[i] = array.ElementAt(i);
                }
            }
            return coordinate;
        }
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
        public static void SetCenter(this Envelope envelope, double[] center, double width, double height)
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
        public static OSGeo.OGR.Geometry ToGeometry(this Envelope envelope)
        {
            var ring = new OSGeo.OGR.Geometry(wkbGeometryType.wkbLinearRing);
            ring.AddPoint_2D(envelope.MinX, envelope.MinY);
            ring.AddPoint_2D(envelope.MaxX, envelope.MinY);
            ring.AddPoint_2D(envelope.MaxX, envelope.MaxY);
            ring.AddPoint_2D(envelope.MinX, envelope.MaxY);
            ring.CloseRings();
            var polygon = new OSGeo.OGR.Geometry(wkbGeometryType.wkbPolygon);
            polygon.AddGeometry(ring);
            return polygon;
        }
        public static IExtent ToExtent(this Envelope envelope)
        {
            IExtent extent = new Extent(envelope.MinX, envelope.MinY, envelope.MaxX, envelope.MaxY);
            return extent;
        }
        /// <summary>
        /// 转Envelope
        /// </summary>
        /// <returns></returns>
        public static Envelope ToEnvelope(this IExtent extent)
        {
            Envelope envelope = new Envelope();
            if (!double.IsNaN(extent.MinX))
            {
                envelope.MinX = extent.MinX;
                envelope.MinY = extent.MinY;
                envelope.MaxX = extent.MaxX;
                envelope.MaxY = extent.MaxY;
            }
            return envelope;
        }
        /// <summary>
        /// 是否包含范围
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static bool Contains(this IExtent extent, Envelope env)
        {
            if (Equals(env, null)) throw new ArgumentNullException(nameof(env));

            return extent.Contains(env.MinX, env.MaxX, env.MinY, env.MaxY);
        }
        /// <summary>
        /// 是否相交
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static bool Intersects(this IExtent extent, Envelope env)
        {
            if (Equals(env, null)) throw new ArgumentNullException(nameof(env));

            return extent.Intersects(env.MinX, env.MaxX, env.MinY, env.MaxY);
        }
        /// <summary>
        /// 是否包含于
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="env"></param>
        /// <returns></returns>
        public static bool Within(this IExtent extent, Envelope env)
        {
            if (Equals(env, null)) throw new ArgumentNullException(nameof(env));

            return extent.Within(env.MinX, env.MaxX, env.MinY, env.MaxY);
        }
        public static OSGeo.OGR.Geometry ToGeometry(this Extent envelope)
        {
            var ring = new OSGeo.OGR.Geometry(wkbGeometryType.wkbLinearRing);
            ring.AddPoint_2D(envelope.MinX, envelope.MinY);
            ring.AddPoint_2D(envelope.MaxX, envelope.MinY);
            ring.AddPoint_2D(envelope.MaxX, envelope.MaxY);
            ring.AddPoint_2D(envelope.MinX, envelope.MaxY);
            ring.CloseRings();
            var polygon = new OSGeo.OGR.Geometry(wkbGeometryType.wkbPolygon);
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
                using (var geometry = feature.GetGeometryRef())
                {
                    geometry.GetPointCount(ref count);
                }
            }
            return count;
        }
        public static void GetPointCount(this OSGeo.OGR.Geometry geometry, ref int count)
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
                    envelope.MinX = double.NaN;
                    envelope.MaxX = double.NaN;
                    envelope.MinY = double.NaN;
                    envelope.MaxY = double.NaN;
                }
            }
        }
        #region 要素操作
        public static IFeature ToFeature(this OSGeo.OGR.Feature feature)
        {
            IFeature destFeature = null;
            if (feature != null)
            {
                destFeature = new GdalFeature(feature);
            }
            return destFeature;
        }
        public static OSGeo.OGR.Feature ToFeature(this IFeature feature)
        {
            OSGeo.OGR.Feature destFeature = null;
            if (feature is GdalFeature gdalFeature)
            {
                destFeature = gdalFeature.Feature;
            }
            return destFeature;
        }
        public static object GetFieldValue(this Feature feature,int index)
        {
            if (feature == null || index < 0 || index >= feature.GetFieldCount())
            {
                throw new Exception("参数设置错误");
            }
            using var fieldDefn = feature.GetFieldDefnRef(index);
            object value;
            switch (fieldDefn.GetFieldType())
            {
                case FieldType.OFTString:
                    value = feature.GetFieldAsString(index);
                    break;
                case FieldType.OFTInteger:
                    value = feature.GetFieldAsInteger(index);
                    break;
                case FieldType.OFTInteger64:
                    value = feature.GetFieldAsInteger64(index);
                    break;
                case FieldType.OFTReal:
                    value = feature.GetFieldAsDouble(index);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return value;
        }
        public static object GetFieldValue(this Feature feature, string fieldName)
        {
            if (feature == null || string.IsNullOrEmpty(fieldName))
            {
                throw new Exception("参数设置错误");
            }
            using var fieldDefn = feature.GetFieldDefnRef(fieldName);
            object value;
            switch (fieldDefn.GetFieldType())
            {
                case FieldType.OFTString:
                    value = feature.GetFieldAsString(fieldName);
                    break;
                case FieldType.OFTInteger:
                    value = feature.GetFieldAsInteger(fieldName);
                    break;
                case FieldType.OFTInteger64:
                    value = feature.GetFieldAsInteger64(fieldName);
                    break;
                case FieldType.OFTReal:
                    value = feature.GetFieldAsDouble(fieldName);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return value;
        }
        public static void SetField(this Feature feature, FieldDefn fieldDefn, object value)
        {
            if (feature == null || fieldDefn == null)
            {
                return;
            }
            var fieldName = fieldDefn.GetName();
            var fieldType = fieldDefn.GetFieldType();
            if (!DBNull.Value.Equals(value))
            {
                switch (fieldType)
                {
                    case FieldType.OFTString:
                        feature.SetField(fieldName, value.ToString());
                        break;
                    case FieldType.OFTInteger:
                        if (value is int intValue)
                        {
                            feature.SetField(fieldName, intValue);
                        }
                        break;
                    case FieldType.OFTReal:
                        if (value is double doubleValue)
                        {
                            feature.SetField(fieldName, doubleValue);
                        }
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        #endregion
    }
}
