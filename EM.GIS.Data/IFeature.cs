using EM.GIS.Geometries;
using System;
using System.Collections.Generic;

namespace EM.GIS.Data
{
    /// <summary>
    /// 要素接口
    /// </summary>
    public interface IFeature:ICloneable,IDisposable
    {
        /// <summary>
        /// 几何体
        /// </summary>
        IGeometry Geometry { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        Dictionary<string, object> Attribute { get; set; }
    }
}