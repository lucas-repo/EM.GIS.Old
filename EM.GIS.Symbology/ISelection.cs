using EM.GIS.Data;
using EM.GIS.Geometries;
using System;
using System.Collections.Generic;

namespace EM.GIS.Symbology
{
    /// <summary>
    /// 选择接口
    /// </summary>
    public interface ISelection
    {
        /// <summary>
        /// 范围
        /// </summary>
        IExtent IExtent { get; }
        /// <summary>
        /// 选择改变事件
        /// </summary>
        event EventHandler SelectionChanged;
    }
}