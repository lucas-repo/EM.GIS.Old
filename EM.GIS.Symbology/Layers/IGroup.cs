using EM.GIS.Data;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace EM.GIS.Symbology
{
    /// <summary>
    /// 分组
    /// </summary>
    public interface IGroup : ILayer
    {
        /// <summary>
        /// 图层个数
        /// </summary>
        int LayerCount { get; }
        /// <summary>
        /// 获取图层
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        ILayer GetLayer(int index);
        /// <summary>
        /// 获取图层集合
        /// </summary>
        /// <param name="func">过滤方法</param>
        /// <returns></returns>
        IEnumerable<ILayer> GetLayers(Func<ILayer, bool> func = null);
        /// <summary>
        /// 获取所有要素图层集合（包含子分组）
        /// </summary>
        /// <returns></returns>
        IEnumerable<IFeatureLayer> GetAllFeatureLayers();
        /// <summary>
        /// 获取所有栅格图层集合（包含子分组）
        /// </summary>
        /// <returns></returns>
        IEnumerable<IRasterLayer> GetAllRasterLayers();
        /// <summary>
        /// 添加图层
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="index"></param>
        public void AddLayer(ILayer layer, int? index = null);
        /// <summary>
        /// 添加图层
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="index"></param>
        public ILayer AddLayer(string filename, int? index = null);
        /// <summary>
        /// 添加图层
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public ILayer AddLayer(IDataSet dataSet, int? index = null);
        /// <summary>
        /// 添加图层
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public IFeatureLayer AddLayer(IFeatureSet dataSet, int? index = null);
        /// <summary>
        /// 添加图层
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public IRasterLayer AddLayer(IRasterSet dataSet, int? index = null);
    }
}