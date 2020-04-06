using EMap.Gis.Data;
using System;
using System.Drawing;
using System.Threading;

namespace EMap.Gis.Symbology
{
    public interface IBaseLayer :  IDisposable,IDynamicVisibility
    {
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
        Extent Extent { get;  }
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
        bool GetVisible(Extent extent,Rectangle rectangle);
        void Draw(Graphics graphics,Rectangle rectangle, Extent extent, bool selected=false, ProgressHandler progressHandler = null, CancellationTokenSource cancellationTokenSource = null);
    }
}
