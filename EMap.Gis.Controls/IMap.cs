using EMap.Gis.Data;
using EMap.Gis.Symbology;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace EMap.Gis.Controls
{
    public interface IMap : IProj
    {
        /// <summary>
        /// 视图范围
        /// </summary>
        Extent ViewExtent { get; set; }
        /// <summary>
        /// 视图边界
        /// </summary>
        Rectangle ViewBounds { get; set; }
        /// <summary>
        /// 是否在工作中
        /// </summary>
        bool IsBusy { get; set; }
        /// <summary>
        /// 图例
        /// </summary>
        ILegend Legend { get; set; }
        /// <summary>
        /// 重绘指定范围
        /// </summary>
        /// <param name="extent"></param>
        void Invalidate(Extent extent);
        /// <summary>
        /// 重绘整个地图
        /// </summary>
        void Invalidate();
        /// <summary>
        /// 缩放至最大范围
        /// </summary>
        void ZoomToMaxExtent();
        /// <summary>
        /// 地图框架
        /// </summary>
        IMapFrame MapFrame { get; set; }
        /// <summary>
        /// 添加多个图层
        /// </summary>
        /// <returns></returns>
        IList<IBaseLayer> AddLayers();
        /// <summary>
        /// 添加单个图层
        /// </summary>
        /// <returns></returns>
        IBaseLayer AddLayer();
        /// <summary>
        /// 图层
        /// </summary>
        ILayerCollection Layers { get; }
        /// <summary>
        /// 地图方法
        /// </summary>
        List<IMapTool> MapTools { get; }
        /// <summary>
        /// 激活地图方法
        /// </summary>
        /// <param name="function"></param>
        void ActivateMapFunction(IMapTool function);
        /// <summary>
        /// 使所有地图工具无效
        /// </summary>
        void DeactivateAllMapTools();

        #region 事件
        event EventHandler<GeoMouseArgs> GeoMouseMove;
        #endregion
    }
}
