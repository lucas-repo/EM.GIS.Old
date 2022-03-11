using EM.GIS.Geometries;
using EM.GIS.Symbology;
using System.Drawing;

namespace EM.GIS.Plugins.WebMaps
{
    /// <summary>
    /// 在线地图图层
    /// </summary>
    public class WebMapLayer : Layer
    {
        protected override void OnDraw(Graphics graphics, Rectangle rectangle, IExtent extent, bool selected = false, Func<bool> cancelFunc = null, Action invalidateMapFrameAction = null)
        {
            throw new NotImplementedException();
        }
    }
}