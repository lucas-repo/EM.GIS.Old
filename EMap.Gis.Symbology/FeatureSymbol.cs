using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public abstract class FeatureSymbol : Descriptor, IFeatureSymbol
    {
        private Rgba32 _color { get; set; }
        public virtual Rgba32 Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }
        public virtual float Opacity
        {
            get
            {
                return _color.GetOpacity();
            }
            set
            {
                _color = Color.ToTransparent(value);
            }
        }
    }
}
