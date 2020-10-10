using System.Collections.Generic;

namespace EM.GIS.Symbology
{
    public interface IFeatureCategoryCollection:ICategoryCollection
    {
        #region 需要重写的部分
        /// <summary>
        /// 获取或设置分类
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        new IFeatureCategory this[int index] { get; set; }
        /// <summary>
        /// 获取枚举器
        /// </summary>
        /// <returns></returns>
        new IEnumerator<IFeatureCategory> GetEnumerator();
        #endregion
    }
}