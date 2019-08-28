using OSGeo.GDAL;

namespace EMap.Gis.Symbology
{
    public interface IRasterLayer : IBaseLayer
    {
        new IRasterScheme Symbology { get; set; }
        Dataset Dataset { get; set; }
        /// <summary>
        /// 仿射六参数：0:左上角x坐标，1:X轴分辨率，2:Y轴旋转角，3:左上角y坐标，4:X轴旋转角，5:Y轴分辨率.
        ///  x = Affine[0] + Affine[1] * col + Affine[2] * row;
        ///  y = Affine[3] + Affine[4] * col + Affine[5] * row;
        /// </summary>
        double[] Affine { get; set; }
        int RasterXSize { get; }
        int RasterYSize { get; }
    }
}
