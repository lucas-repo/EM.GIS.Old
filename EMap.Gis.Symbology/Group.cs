using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class Group : BaseLayer, IGroup
    {
        public Image<Rgba32> Icon { get; set; }
        public new ILayerCollection Items { get => base.Items as ILayerCollection; set => base.Items = value; }

        public new IGroup Parent { get => base.Parent as IGroup; set => base.Parent = value; }
        private Envelope _extents;
        public override Envelope Extents
        {
            get
            {
                if (_extents == null)
                {
                    _extents = new Envelope();
                }
                for (int i = 0; i < Items.Count; i++)
                {
                    var layer = Items[i];
                    if (i == 0)
                    {
                        _extents.MinX = layer.Extents.MinX;
                        _extents.MinY = layer.Extents.MinY;
                        _extents.MaxX = layer.Extents.MaxX;
                        _extents.MaxY = layer.Extents.MaxY;
                    }
                    else
                    {
                        _extents.MinX = Math.Min(_extents.MinX ,layer.Extents.MinX);
                        _extents.MinY = Math.Min(_extents.MinY, layer.Extents.MinY);
                        _extents.MaxX = Math.Max(_extents.MaxX, layer.Extents.MaxX);
                        _extents.MaxY = Math.Max(_extents.MaxY, layer.Extents.MaxY);
                    }
                }
                return _extents;
            }
        }

        public override void ResetBuffer(Rectangle rectangle, Envelope envelope, bool selected, ProgressHandler progressHandler, CancellationTokenSource cancellationTokenSource)
        {
            throw new NotImplementedException();
        }

        public Group()
        {
            Items = new LayerCollection(MapFrame, this);
        }
        public Group(IFrame frame) : this()
        {
            MapFrame = frame;
        }
    }
}
