using EM.GIS.Controls;
using EM.GIS.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.GIS.Gdals
{
    /// <summary>
    /// gdal扩展
    /// </summary>
    public class GdalPlugin : Plugin
    {
        public override int Priority => -10000;
        public override void Activate()
        {
            DataFactory.Default.GeometryFactory = new GdalGeometryFactory();
            base.Activate();
        }
        public override void Deactivate()
        {
            DataFactory.Default.GeometryFactory = null;
            base.Deactivate();
        }
    }
}
