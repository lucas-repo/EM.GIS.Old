using EM.GIS.Data;
using System.Collections.Generic;

namespace EM.GIS.Symbology
{
    /// <summary>
    /// 要素选择器
    /// </summary>
    public interface IFeatureSelection:ISelection,IItemCollection<IFeature>
    {
    }
}