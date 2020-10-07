using EM.GIS.Data;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.GIS.Gdals
{
    public static class FeatureExtensions
    {
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
        public static object GetFieldValue(this Feature feature, int index)
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
