using EM.GIS.Data;
using OSGeo.GDAL;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;

namespace EM.GIS.Symbology
{
    public class RasterLayer : Layer, IRasterLayer
    {
        public RasterLayer()
        {
        }
        public RasterLayer(IRasterSet rasterSet)
        {
            DataSet = rasterSet;
        }

        public new IRasterScheme Symbology { get => base.Symbology as IRasterScheme; set => base.Symbology = value; }

        public new IRasterSet DataSet { get => base.DataSet as IRasterSet; set => base.DataSet = value; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DataSet?.Dispose();
                DataSet = null;
                Symbology = null;
            }
            base.Dispose(disposing);
        }
        protected override void OnDraw(Graphics graphics, Rectangle rectangle, IExtent extent, bool selected = false, CancellationTokenSource cancellationTokenSource = null)
        {
            using (var bmp = DataSet.GetBitmap(extent, rectangle))
            {
                if (bmp != null)
                {
                    graphics.DrawImage(bmp, rectangle);
                }
            }
        }
    }
}