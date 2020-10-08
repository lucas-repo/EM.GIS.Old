using EM.GIS.Data;
using EM.GIS.Geometries;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace EM.GIS.Symbology
{
    public abstract class FeatureLayer : Layer, IFeatureLayer
    {
        public DataSource DataSource { get; set; }
        private Layer _layer;
        public Layer Layer
        {
            get
            {
                if (_layer == null)
                {
                    _layer = DataSource?.GetLayerByIndex(0);
                }
                return _layer;
            }
        }
        public override IExtent Extent
        {
            get
            {
                IExtent extent = null;
                if (Layer != null)
                {
                    using (Envelope envelope = new Envelope())
                    {
                        var ret = Layer.GetExtent(envelope, 0);
                        extent = envelope.ToExtent();
                    }
                }
                return extent;
            }
        }
        public new IFeatureScheme Symbology { get => base.Symbology as IFeatureScheme; set => base.Symbology = value; }
        public new IFeatureCategory DefaultCategory { get => base.DefaultCategory as IFeatureCategory; set => base.DefaultCategory = value; }
        public FeatureLayer(IFeatureSet featureSet)
        {
            DataSet = featureSet;
            DataSource = featureSet.DataSource;
            Selection = new Selection();
        }

        public ISelection Selection { get; }
        public ILabelLayer LabelLayer { get; set; }
        public new IFeatureSet DataSet { get => base.DataSet as IFeatureSet; set => base.DataSet = value; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Layer?.Dispose();
                DataSource?.Dispose();
                DataSource = null;
            }
            base.Dispose(disposing);
        }
        protected override void OnDraw(Graphics graphics, Rectangle rectangle, IExtent extent, bool selected = false, CancellationTokenSource cancellationTokenSource = null)
        {
            using (Geometry polygon = extent.ToGeometry())
            {
                Layer.SetSpatialFilter(polygon);
            }
            List<Feature> features = new List<Feature>();
            Feature feature = Layer.GetNextFeature();
            long featureCount = Layer.GetFeatureCount(0);
            long drawnFeatureCount = 0;
            int threshold = 65536;
            int totalPointCount = 0;
            int percent = 0;
            Action drawFeatuesAction = new Action(() =>
            {
                if (features.Count > 0)
                {
                    percent = (int)(drawnFeatureCount * 100 / featureCount);
                    ProgressHandler?.Progress(percent, "绘制要素中...");
                    MapArgs drawArgs = new MapArgs(rectangle, extent, graphics);
                    DrawFeatures(drawArgs, features, selected, ProgressHandler, cancellationTokenSource);
                    drawnFeatureCount += features.Count;
                    foreach (var item in features)
                    {
                        item.Dispose();
                    }
                    features.Clear();
                }
                totalPointCount = 0;
            });
            while (feature != null)
            {
                features.Add(feature);
                int pointCount = feature.GetPointCount();
                totalPointCount += pointCount;
                if (totalPointCount >= threshold)
                {
                    drawFeatuesAction();
                }
                feature = Layer.GetNextFeature();
            }
            if (totalPointCount > 0)
            {
                drawFeatuesAction();
            }
            ProgressHandler?.Progress(100, "绘制要素中...");
        }
        private Dictionary<Feature, IFeatureCategory> GetFeatureAndCategoryDic(List<Feature> features)
        {
            Dictionary<Feature, IFeatureCategory> featureCategoryDic = new Dictionary<Feature, IFeatureCategory>();
            using (DataTable dataTable = GetAttribute(features))
            {
                foreach (IFeatureCategory featureCategory in Symbology.Categories)
                {
                    DataRow[] rows = dataTable.Select(featureCategory.FilterExpression);
                    foreach (var row in rows)
                    {
                        int index = dataTable.Rows.IndexOf(row);
                        Feature feature = features[index];
                        if (!featureCategoryDic.ContainsKey(feature))
                        {
                            featureCategoryDic[feature] = featureCategory;
                        }
                    }
                }
            }
            var lastFeatures = features.Except(featureCategoryDic.Select(x => x.Key));
            foreach (var feature in lastFeatures)
            {
                featureCategoryDic[feature] = DefaultCategory;
            }
            return featureCategoryDic;
        }
        protected abstract void DrawGeometry(MapArgs drawArgs, IFeatureSymbolizer symbolizer, Geometry geometry);
        private void DrawFeatures(MapArgs drawArgs, List<Feature> features, bool selected, IProgressHandler progressHandler, CancellationTokenSource cancellationTokenSource)
        {
            if (drawArgs == null || features == null || cancellationTokenSource?.IsCancellationRequested == true)
            {
                return;
            }
            var featureCategoryDic = GetFeatureAndCategoryDic(features);
            foreach (var item in featureCategoryDic)
            {
                if (cancellationTokenSource?.IsCancellationRequested == true)
                {
                    return;
                }
                Feature feature = item.Key;
                var category = item.Value;
                var symbolizer = selected ? category.SelectionSymbolizer : category.Symbolizer;
                using (Geometry geometry = feature.GetGeometryRef())
                {
                    DrawGeometry(drawArgs, symbolizer, geometry);
                }
            }
        }

        public DataTable GetSchema()
        {
            DataTable dataTable = new DataTable();
            using (FeatureDefn featureDefn = Layer.GetLayerDefn())
            {
                int fieldCount = featureDefn.GetFieldCount();
                for (int i = 0; i < fieldCount; i++)
                {
                    using (FieldDefn fieldDefn = featureDefn.GetFieldDefn(i))
                    {
                        FieldType fieldType = fieldDefn.GetFieldType();
                        string name = fieldDefn.GetName();
                        Type type = null;
                        string fieldName1 = fieldDefn.GetNameRef();
                        switch (fieldType)
                        {
                            case FieldType.OFTBinary:
                                type = typeof(byte[]);
                                break;
                            case FieldType.OFTDate:
                                type = typeof(DateTime);
                                break;
                            case FieldType.OFTDateTime:
                                type = typeof(DateTime);
                                break;
                            case FieldType.OFTInteger:
                                type = typeof(int);
                                break;
                            case FieldType.OFTInteger64:
                                type = typeof(long);
                                break;
                            case FieldType.OFTInteger64List:
                                type = typeof(long[]);//todo待测试
                                break;
                            case FieldType.OFTIntegerList:
                                type = typeof(int[]);
                                break;
                            case FieldType.OFTReal:
                                type = typeof(double);
                                break;
                            case FieldType.OFTRealList:
                                type = typeof(double[]);
                                break;
                            case FieldType.OFTString:
                                type = typeof(string);
                                break;
                            case FieldType.OFTStringList:
                                type = typeof(string[]);
                                break;
                            case FieldType.OFTTime:
                                type = typeof(DateTime);
                                break;
                            case FieldType.OFTWideString:
                                type = typeof(string);
                                break;
                            case FieldType.OFTWideStringList:
                                type = typeof(string[]);
                                break;
                        }
                        DataColumn dataColumn = new DataColumn(name, type);
                        dataTable.Columns.Add(dataColumn);
                    }
                }
            }
            return dataTable;
        }
        public DataTable GetAttribute(List<Feature> features)
        {
            DataTable dataTable = GetSchema();
            if (features == null)
            {
                return dataTable;
            }
            foreach (var feature in features)
            {
                DataRow dataRow = dataTable.NewRow();
                dataTable.Rows.Add(dataRow);
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    DataColumn column = dataTable.Columns[i];
                    string name = column.ColumnName;
                    switch (column.DataType.Name)
                    {
                        case "Int32":
                            dataRow[column] = feature.GetFieldAsInteger(name);
                            break;
                        case "Int64":
                            dataRow[column] = feature.GetFieldAsInteger64(name);
                            break;
                        case "Double":
                            dataRow[column] = feature.GetFieldAsDouble(name);
                            break;
                        case "DateTime":
                            feature.GetFieldAsDateTime(name, out int pnYear, out int pnMonth, out int pnDay, out int pnHour, out int pnMinute, out float pfSecond, out int pnTZFlag);
                            DateTime dateTime = new DateTime(pnYear, pnMonth, pnDay, pnHour, pnMinute, (int)pfSecond);
                            dataRow[column] = dateTime;
                            break;
                        case "String":
                            dataRow[column] = feature.GetStringValue(i);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
            return dataTable;
        }

    }
}