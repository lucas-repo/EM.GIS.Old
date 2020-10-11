using EM.GIS.Geometries;
using System.Collections.Generic;

namespace EM.GIS.Data
{
    /// <summary>
    /// 矢量数据集
    /// </summary>
    public interface IFeatureSet : IDataSet, IGetFieldDefn
    {
        /// <summary>
        /// 要素个数
        /// </summary>
        int FeatureCount { get; }
        /// <summary>
        /// 要素类型
        /// </summary>
        FeatureType FeatureType { get; }
        /// <summary>
        /// 获取或设置空间过滤
        /// </summary>
        IGeometry SpatialFilter { get; set; }
        /// <summary>
        /// 获取或设置属性过滤
        /// </summary>
        string AttributeFilter { get; set; }
        /// <summary>
        /// 添加要素
        /// </summary>
        /// <param name="geometry"></param>
        /// <returns></returns>
        IFeature AddFeature(IGeometry geometry);
        /// <summary>
        /// 添加要素
        /// </summary>
        /// <param name="geometry"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        IFeature AddFeature(IGeometry geometry, Dictionary<string, object> attribute);
        /// <summary>
        /// 获取要素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IFeature GetFeature(int index);
        /// <summary>
        /// 获取要素集合(返回过滤后的要素)
        /// </summary>
        /// <returns></returns>
        IEnumerable<IFeature> GetFeatures();
        /// <summary>
        /// 移除要素
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        bool RemoveFeature(int index);
    }
}