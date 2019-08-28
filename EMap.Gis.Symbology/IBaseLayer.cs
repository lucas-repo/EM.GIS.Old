using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.Threading;

namespace EMap.Gis.Symbology
{
    public interface IBaseLayer : ILegendItem, IDisposable
    {
        Image<Rgba32> BufferImgage { get; set; }
        Envelope BufferEnvelope { get; }
        IScheme Symbology { get; set; }
        ICategory DefaultCategory { get; set; }
        IFrame MapFrame { get; set; }
        Envelope Extents { get; }
        //OSGeo.OSR.SpatialReference SpatialReference { get; }
        void DrawReagion(Image<Rgba32> image, Rectangle rectangle, Envelope envelope, bool selected,ProgressHandler progressHandler, CancellationTokenSource cancellationTokenSource);
        void ResetBuffer(Rectangle rectangle, Envelope envelope, bool selected, ProgressHandler progressHandler, CancellationTokenSource cancellationTokenSource);
    }
}
