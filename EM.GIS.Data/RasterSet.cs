using EM.GIS.Geometries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;

namespace EM.GIS.Data
{
    public abstract class RasterSet : DataSet, IRasterSet
    {
        public int NumRows { get; protected set; }
        public int NumColumns { get; protected set; }
        public IList<IRasterSet> Bands { get;  }
        public abstract int ByteSize { get; }
        [Category("Statistics")]
        [Description("Gets or sets the maximum data value, not counting no-data values in the grid.")]
        public virtual double Maximum { get; protected set; }

        [Category("Statistics")]
        [Description("Gets or sets the mean of the non-NoData values in this grid. If the data is not InRam, then the GetStatistics method must be called before these values will be correct.")]
        public virtual double Mean { get; protected set; }
        /// <summary>
        /// Gets or sets the minimum data value that is not classified as a no data value in this raster.
        /// </summary>
        [Category("Statistics")]
        [Description("Gets or sets the minimum data value that is not classified as a no data value in this raster.")]
        public virtual double Minimum { get; protected set; }
        [Category("Statistics")]
        [Description("Gets or sets the standard deviation of all the Non-nodata cells. If the data is not InRam, then you will have to first call the GetStatistics() method to get meaningful values.")]
        public virtual double StdDeviation { get; protected set; }

        [Category("Data")]
        [Description("Gets or sets a  double showing the no-data value for this raster.")]
        public virtual double NoDataValue { get; set; }
        public IRasterBounds Bounds { get; set; }
        public int PixelSpace { get; set; }
        public int LineSpace { get; set; }
        public override IExtent Extent
        {
            get => Bounds?.Extent;
            set
            {
                if (Bounds != null) Bounds.Extent = value;
            }
        }

        public RasterType RasterType { get; set; }

        public int BandCount => Bands.Count;

        public int Width { get; }

        public int Height { get; }

        public IRasterBounds RasterBounds { get; set; }

        public virtual void GetStatistics()
        { }

        public virtual Bitmap GetBitmap()
        {
            return null;
        }

        public  Bitmap GetBitmap(IExtent envelope, Size size)
        {
            return GetBitmap(envelope, new Rectangle(new Point(0, 0), size));
        }

        public virtual Bitmap GetBitmap(IExtent envelope, Rectangle window)
        {
            return null;
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Color[] CategoryColors()
        {
            return null;
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string[] CategoryNames()
        {
            return null;
        }

        Statistics IRasterSet.GetStatistics()
        {
            throw new NotImplementedException();
        }
    }
}
