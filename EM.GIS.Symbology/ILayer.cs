using EM.GIS.Data;
using EM.GIS.Geometries;
using OSGeo.OSR;
using System;
using System.Drawing;
using System.Threading;

namespace EM.GIS.Symbology
{
    public interface ILayer :  IDisposable,IDynamicVisibility
    {
        IDataSet DataSet { get; set; }
        /// <summary>
        /// Gets or sets the progress handler
        /// </summary>
        IProgressHandler ProgressHandler { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 别名
        /// </summary>
        string AliasName { get; set; }
        /// <summary>
        /// 是否可见
        /// </summary>
        bool IsVisible { get; set; }
        /// <summary>
        /// 范围
        /// </summary>
        IExtent Extent { get;  }
        /// <summary>
        /// 空间参考
        /// </summary>
        SpatialReference SpatialReference { get; }
        /// <summary>
        /// 符号
        /// </summary>
        IScheme Symbology { get; set; }
        /// <summary>
        /// 默认符号
        /// </summary>
        ICategory DefaultCategory { get; set; }
        /// <summary>
        /// 在指定范围是否可见
        /// </summary>
        /// <param name="extent"></param>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        bool GetVisible(IExtent extent,Rectangle rectangle);
        /// <summary>
        /// 绘制图层到画布
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="rectangle"></param>
        /// <param name="extent"></param>
        /// <param name="selected"></param>
        /// <param name="cancellationTokenSource"></param>
        void Draw(Graphics graphics,Rectangle rectangle, IExtent extent, bool selected=false,  CancellationTokenSource cancellationTokenSource = null);
    }
}
