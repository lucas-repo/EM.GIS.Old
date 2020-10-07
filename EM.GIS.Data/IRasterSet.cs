using System.Collections;
using System.Collections.Generic;

namespace EM.GIS.Data
{
    /// <summary>
    /// 栅格数据集
    /// </summary>
    public interface IRasterSet:IDataSet
    {
        /// <summary>
        /// 栅格类型
        /// </summary>
        RasterType RasterType { get; }
        /// <summary>
        /// 波段
        /// </summary>
        IList<IRasterSet> Bands { get; }
        /// <summary>
        /// 波段数
        /// </summary>
        int BandCount { get; }
        /// <summary>
        /// 长度
        /// </summary>
        int Width { get; }
        /// <summary>
        /// 高度
        /// </summary>
        int Height { get; }
        /// <summary>
        /// 无数据值
        /// </summary>
        double NoDataValue { get; set; }
        /// <summary>
        /// 栅格范围
        /// </summary>
        IRasterBounds RasterBounds { get; set; }
        /// <summary>
        /// 获取统计
        /// </summary>
        /// <returns></returns>
        Statistics GetStatistics();
    }
}