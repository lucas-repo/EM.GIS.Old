using System;
using System.Collections.Generic;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;

namespace EMap.Gis.Symbology
{
    public class LineMarkerSymbol : LineCartographicSymbol, ILineMarkerSymbol
    {
        public IPointSymbolizer Marker { get; set; }
        public LineMarkerSymbol() : base(LineSymbolType.Marker)
        {
            Marker = new PointSymbolizer();
        }
        public override IPen<Rgba32> ToPen(float scale)
        {
            float width = scale * Width;
            IPen<Rgba32> pen = null; 
            if (Marker == null)
            {
                pen = new Pen<Rgba32>(Color, width, Pattern);
            }
            else
            {
                SizeF size = Marker.GetSize();
                int imgWidth = (int)Math.Ceiling(size.Width);
                int imgHeight= (int)Math.Ceiling(size.Height);
                Image<Rgba32> image = new Image<Rgba32>(imgWidth,imgHeight);
                Rectangle rectangle = new Rectangle(0, 0, imgWidth, imgHeight);
                Marker.Draw(image, rectangle);
                IBrush<Rgba32> brush = new ImageBrush<Rgba32>(image);
                pen = new Pen<Rgba32>(brush, width, Pattern);
            }
            return pen;
        }
    }
}
