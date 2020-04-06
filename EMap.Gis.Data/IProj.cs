using System.Drawing;

namespace EMap.Gis.Data
{
    public interface IProj
    {
        /// <summary>
        /// 范围
        /// </summary>
        Extent Extent { get; }
        /// <summary>
        /// 边界
        /// </summary>
        Rectangle Bounds { get; }
    }
}
