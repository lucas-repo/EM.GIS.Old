using OSGeo.OGR;
using System;
using System.Drawing;
using System.Threading;

namespace EMap.Gis.Symbology
{
    public interface IBaseLayer : ILegendItem, IDisposable
    {
        Image BufferImgage { get; set; }
        Envelope BufferEnvelope { get; }
        IScheme Symbology { get; set; }
        ICategory DefaultCategory { get; set; }
        IFrame MapFrame { get; set; }
        Envelope Extents { get; }
        //OSGeo.OSR.SpatialReference SpatialReference { get; }
        void DrawReagion(Image image, Rectangle rectangle, Envelope envelope, bool selected,ProgressHandler progressHandler, CancellationTokenSource cancellationTokenSource);
        void ResetBuffer(Rectangle rectangle, Envelope envelope, bool selected, ProgressHandler progressHandler, CancellationTokenSource cancellationTokenSource);
    }
}
