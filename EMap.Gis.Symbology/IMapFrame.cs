using EMap.Gis.Data;
using EMap.Gis.Symbology;
using OSGeo.GDAL;
using OSGeo.OGR;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace EMap.Gis.Symbology
{
    public interface IMapFrame: IGroup, IBaseLayer, IProj
    {
        /// <summary>
        /// 地图框是否忙于绘制
        /// </summary>
        bool IsBusy { get; set; }
        /// <summary>
        /// 临时绘制图层
        /// </summary>
        ILayerCollection DrawingLayers { get; }
        /// <summary>
        /// 背景颜色
        /// </summary>
        Color BackGround { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        int Width { get;  }
        /// <summary>
        /// 高度
        /// </summary>
        int Height { get; }
        /// <summary>
        /// 后台缓存图片，用以获取缓存图片
        /// </summary>
        Image BackBuffer { get; set; }
        /// <summary>
        /// 视图边界
        /// </summary>
        Rectangle ViewBounds { get; set; }
        /// <summary>
        /// 视图范围
        /// </summary>
        Extent ViewExtent { get; set; }
        /// <summary>
        /// 进度委托
        /// </summary>
        ProgressHandler ProgressHandler { get; set; }
        /// <summary>
        /// 取消标记源
        /// </summary>
        CancellationTokenSource CancellationTokenSource { get; set; }
        /// <summary>
        /// 缓存图片改变事件
        /// </summary>
        event EventHandler BufferChanged;

        /// <summary>
        /// 重绘缓存
        /// </summary>
        /// <param name="extent"></param>
        Task ResetBuffer(Extent extent=null);
        /// <summary>
        /// 获取所有矢量图层
        /// </summary>
        /// <returns></returns>
        IFeatureLayer[] GetFeatureLayers();
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rectangle"></param>
        void Draw(Graphics g,Rectangle rectangle);
        /// <summary>
        /// 添加图层
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        IBaseLayer AddLayer(string fileName);
        /// <summary>
        /// 添加图层
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        IFeatureLayer AddLayer(DataSource dataSource);
        /// <summary>
        /// 添加图层
        /// </summary>
        /// <param name="dataset"></param>
        /// <returns></returns>
        IRasterLayer AddLayer(Dataset dataset);
        /// <summary>
        /// 根据视图范围重设范围
        /// </summary>
        void ResetExtents();
        /// <summary>
        /// 重设地图大小
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void Resize(int width, int height);
        /// <summary>
        /// 居中至最大范围
        /// </summary>
        void ZoomToMaxExtent();
    }
}