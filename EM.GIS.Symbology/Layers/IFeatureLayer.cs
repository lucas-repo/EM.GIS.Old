using EM.GIS.Data;

namespace EM.GIS.Symbology
{
    /// <summary>
    /// 要素图层集合
    /// </summary>
    public interface IFeatureLayer: ILayer
    {
        /// <summary>
        /// 数据集
        /// </summary>
        new IFeatureSet DataSet { get; set; }
        /// <summary>
        /// 选择器
        /// </summary>
        ISelection Selection { get; }
        /// <summary>
        /// 标注图层
        /// </summary>
        ILabelLayer LabelLayer { get; set; }
    }
}
