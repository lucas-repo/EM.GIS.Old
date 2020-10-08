using EM.GIS.Data;

namespace EM.GIS.Symbology
{
    /// <summary>
    /// 要素图层集合
    /// </summary>
    public interface IFeatureLayer: ILayer
    {
        new IFeatureScheme Symbology { get; set; }
        new IFeatureCategory DefaultCategory { get; set; }
        new IFeatureSet DataSet { get; set; }
        ISelection Selection { get; }
        /// <summary>
        /// 标注图层
        /// </summary>
        ILabelLayer LabelLayer { get; set; }
    }
}
