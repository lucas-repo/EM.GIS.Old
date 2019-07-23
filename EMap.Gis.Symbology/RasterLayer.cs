using OSGeo.GDAL;
using OSGeo.OGR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace EMap.Gis.Symbology
{
    public class RasterLayer : BaseLayer, IRasterLayer
    {
        private int _overview;
        private int _overviewCount;
        private ColorInterp _colorInterp;
        public Dataset Dataset { get; set; }
        public double[] Affine { get; set; }
        public int RasterXSize { get => Dataset.RasterXSize; }
        public int RasterYSize { get => Dataset.RasterYSize; }
        public Band[] Bands { get; }
        Band FirstBand { get => Bands.Length > 0 ? Bands[0] : null; }
        public int NumBands { get => Dataset.RasterCount; }
        public double NoDataValue { get; }
        public RasterLayer(Dataset dataset)
        {
            Dataset = dataset;
            Affine = new double[6];
            Dataset?.GetGeoTransform(Affine);
            Bands = new Band[Dataset.RasterCount];
            for (int i = 1; i <= Dataset.RasterCount; i++)
            {
                Band band = Dataset.GetRasterBand(i);
                Bands[i - 1] = band;
                if (i == 1)
                {
                    _overviewCount = band.GetOverviewCount();
                    _colorInterp = band.GetColorInterpretation();
                    band.GetNoDataValue(out double value, out int hasValue);
                    if (hasValue > 0)
                    {
                        NoDataValue = value;
                    }
                }
            }
        }

        public new IRasterScheme Symbology { get => base.Symbology as IRasterScheme; set => base.Symbology = value; }
        public override Image<Rgba32> GetImage(Envelope envelope, Rectangle rectangle)
        {
            Image<Rgba32> image = new Image<Rgba32>(rectangle.Width, rectangle.Height);
            DrawGraphics(image, envelope, rectangle);
            return image;
        }

        private void DrawGraphics(Image<Rgba32> image, Envelope envelope, Rectangle rectangle)
        {
            if (Bands.Length == 0)
            {
                return;
            }
            // Gets the scaling factor for converting from geographic to pixel coordinates
            double width = envelope.MaxX - envelope.MinX;
            double height = envelope.MaxY - envelope.MinY;
            double dx = rectangle.Width / width;
            double dy = rectangle.Height / height;

            double[] a = Affine;

            // calculate inverse
            double p = 1 / ((a[1] * a[5]) - (a[2] * a[4]));
            double[] aInv = new double[4];
            aInv[0] = a[5] * p;
            aInv[1] = -a[2] * p;
            aInv[2] = -a[4] * p;
            aInv[3] = a[1] * p;

            // estimate rectangle coordinates
            double tlx = ((envelope.MinX - a[0]) * aInv[0]) + ((envelope.MaxY - a[3]) * aInv[1]);
            double tly = ((envelope.MinX - a[0]) * aInv[2]) + ((envelope.MaxY - a[3]) * aInv[3]);
            double trx = ((envelope.MaxX - a[0]) * aInv[0]) + ((envelope.MaxY - a[3]) * aInv[1]);
            double trY = ((envelope.MaxX - a[0]) * aInv[2]) + ((envelope.MaxY - a[3]) * aInv[3]);
            double blx = ((envelope.MinX - a[0]) * aInv[0]) + ((envelope.MinY - a[3]) * aInv[1]);
            double bly = ((envelope.MinX - a[0]) * aInv[2]) + ((envelope.MinY - a[3]) * aInv[3]);
            double brx = ((envelope.MaxX - a[0]) * aInv[0]) + ((envelope.MinY - a[3]) * aInv[1]);
            double bry = ((envelope.MaxX - a[0]) * aInv[2]) + ((envelope.MinY - a[3]) * aInv[3]);

            // get absolute maximum and minimum coordinates to make a rectangle on projected coordinates
            // that overlaps all the visible area.
            double tLx = Math.Min(Math.Min(Math.Min(tlx, trx), blx), brx);
            double tLy = Math.Min(Math.Min(Math.Min(tly, trY), bly), bry);
            double bRx = Math.Max(Math.Max(Math.Max(tlx, trx), blx), brx);
            double bRy = Math.Max(Math.Max(Math.Max(tly, trY), bly), bry);

            // limit it to the available image
            // todo: why we compare NumColumns\Rows and X,Y coordinates??
            if (tLx > RasterXSize) tLx = RasterXSize;
            if (tLy > RasterYSize) tLy = RasterYSize;
            if (bRx > RasterXSize) bRx = RasterXSize;
            if (bRy > RasterYSize) bRy = RasterYSize;

            if (tLx < 0) tLx = 0;
            if (tLy < 0) tLy = 0;
            if (bRx < 0) bRx = 0;
            if (bRy < 0) bRy = 0;

            // gets the affine scaling factors.
            float m11 = Convert.ToSingle(a[1] * dx);
            float m22 = Convert.ToSingle(a[5] * -dy);
            float m21 = Convert.ToSingle(a[2] * dx);
            float m12 = Convert.ToSingle(a[4] * -dy);
            double l = a[0] - (.5 * (a[1] + a[2])); // Left of top left pixel
            double t = a[3] - (.5 * (a[4] + a[5])); // top of top left pixel
            float xShift = (float)((l - envelope.MinX) * dx);
            float yShift = (float)((envelope.MaxY - t) * dy);

            float xRatio = 1, yRatio = 1;
            if (_overviewCount > 0)
            {
                using (Band firstOverview = FirstBand.GetOverview(0))
                {
                    xRatio = (float)firstOverview.XSize / FirstBand.XSize;
                    yRatio = (float)firstOverview.YSize / FirstBand.YSize;
                }
            }
            if (m11 > xRatio || m22 > yRatio)
            {
                _overview = -1; // don't use overviews when zooming behind the max res.
            }
            else
            {
                // estimate the pyramids that we need.
                // when using unreferenced images m11 or m22 can be negative resulting on inf logarithm.
                // so the Math.abs
                _overview = (int)Math.Min(Math.Log(Math.Abs(1 / m11), 2), Math.Log(Math.Abs(1 / m22), 2));

                // limit it to the available pyramids
                _overview = Math.Min(_overview, _overviewCount - 1);

                // additional test but probably not needed
                if (_overview < 0)
                {
                    _overview = -1;
                }
            }

            var overviewPow = Math.Pow(2, _overview + 1);

            // witdh and height of the image
            var w = (bRx - tLx) / overviewPow;
            var h = (bRy - tLy) / overviewPow;
            AffineTransformBuilder affineTransformBuilder = new AffineTransformBuilder();
            Matrix3x2 matrix = new Matrix3x2(m11 * (float)overviewPow, m12 * (float)overviewPow, m21 * (float)overviewPow, m22 * (float)overviewPow, xShift, yShift);
            affineTransformBuilder.AppendMatrix(matrix);

            //image.Mutate(x => x.Transform(affineTransformBuilder));

            int blockXsize, blockYsize;

            // get the optimal block size to request gdal.
            // if the image is stored line by line then ask for a 100px stripe.
            if (_overview >= 0 && _overviewCount > 0)
            {
                using (var overview = FirstBand.GetOverview(_overview))
                {
                    overview.GetBlockSize(out blockXsize, out blockYsize);
                    if (blockYsize == 1)
                    {
                        blockYsize = Math.Min(100, overview.YSize);
                    }
                }
            }
            else
            {
                FirstBand.GetBlockSize(out blockXsize, out blockYsize);
                if (blockYsize == 1)
                {
                    blockYsize = Math.Min(100, FirstBand.YSize);
                }
            }

            int nbX, nbY;

            // limit the block size to the viewable image.
            if (w < blockXsize)
            {
                blockXsize = (int)Math.Ceiling(w);
                nbX = 1;
            }
            else if (w == blockXsize)
            {
                nbX = 1;
            }
            else
            {
                nbX = (int)Math.Ceiling(w / blockXsize);
            }

            if (h < blockYsize)
            {
                blockYsize = (int)Math.Ceiling(h);
                nbY = 1;
            }
            else if (h == blockYsize)
            {
                nbY = 1;
            }
            else
            {
                nbY = (int)Math.Ceiling(h / blockYsize);
            }

            for (var i = 0; i < nbX; i++)
            {
                for (var j = 0; j < nbY; j++)
                {
                    // The +1 is to remove the white stripes artifacts
                    double xOffsetD = (tLx / overviewPow) + (i * blockXsize);
                    double yOffsetD = (tLy / overviewPow) + (j * blockYsize);
                    int xOffsetI = (int)Math.Floor(xOffsetD);
                    int yOffsetI = (int)Math.Floor(yOffsetD);
                    int xSize = blockXsize + 1;
                    int ySize = blockYsize + 1;
                    using (Image<Rgba32> childImage = GetImage(xOffsetI, yOffsetI, xSize, ySize))
                    {
                        if (childImage != null)
                        {
                            childImage.Mutate(x => x.Transform(affineTransformBuilder));
                            Point location = new Point(xOffsetI, yOffsetI);
                            location= Point.Transform(location,matrix);
                            image.Mutate(x => x.DrawImage(childImage, location,1));
                        }
                    }
                }
            }
        }

        private Image<Rgba32> GetImage(int xOffset, int yOffset, int xSize, int ySize)
        {
            Image<Rgba32> result = null;
            Action action = new Action(() =>
            {
                switch (NumBands)
                {
                    case 0:
                        break;
                    case 1:
                    case 2:
                        result = ReadGrayIndex(xOffset, yOffset, xSize, ySize);
                        break;
                    case 3:
                        result = ReadRgb(xOffset, yOffset, xSize, ySize);
                        break;
                    default:
                        result = ReadArgb(xOffset, yOffset, xSize, ySize);
                        break;
                }
            });
            switch (_colorInterp)
            {
                case ColorInterp.GCI_PaletteIndex:
                    result = ReadPaletteBuffered(xOffset, yOffset, xSize, ySize);
                    break;
                case ColorInterp.GCI_GrayIndex:
                    result = ReadGrayIndex(xOffset, yOffset, xSize, ySize);
                    break;
                case ColorInterp.GCI_RedBand:
                    result = ReadRgb(xOffset, yOffset, xSize, ySize);
                    break;
                case ColorInterp.GCI_AlphaBand:
                    result = ReadArgb(xOffset, yOffset, xSize, ySize);
                    break;
                default:
                    action.Invoke();
                    break;
            }
            // data set disposed on disposing this image
            return result;
        }

        private Image<Rgba32> ReadGrayIndex(int xOffset, int yOffset, int xSize, int ySize)
        {
            Band firstBand;
            var disposeBand = false;
            if (_overview >= 0 && _overviewCount > 0)
            {
                firstBand = FirstBand.GetOverview(_overview);
                disposeBand = true;
            }
            else
            {
                firstBand = FirstBand;
            }
            int width, height;
            NormalizeSizeToBand(xOffset, yOffset, xSize, ySize, firstBand, out width, out height);
            byte[] rBuffer = ReadBand(firstBand, xOffset, yOffset, xSize, ySize, width, height);
            if (disposeBand)
            {
                firstBand.Dispose();
            }
            Image<Rgba32> result = GetImage(width, height, rBuffer, rBuffer, rBuffer);
            return result;
        }
        private Image<Rgba32> GetImage(int width, int height, byte[] rBuffer, byte[] gBuffer, byte[] bBuffer, byte[] aBuffer = null)
        {
            if (width <= 0 || height <= 0)
            {
                return null;
            }
            Image<Rgba32> result = new Image<Rgba32>(width, height);
            int bufferIndex = -1;
            if (aBuffer == null)
            {
                for (int row = 0; row < height; row++)
                {
                    Span<Rgba32> pixelRowSpan = result.GetPixelRowSpan(row);
                    bufferIndex = row * width;
                    for (int col = 0; col < width; col++)
                    {
                        byte bValue = bBuffer[bufferIndex];
                        byte gValue = gBuffer[bufferIndex];
                        byte rValue = rBuffer[bufferIndex];
                        byte aValue = 255;
                        if (rValue == NoDataValue)
                        {
                            aValue = 0;
                        }
                        pixelRowSpan[col] = new Rgba32(rValue, gValue, bValue, aValue);
                        bufferIndex++;
                    }
                }
            }
            else
            {
                for (int row = 0; row < height; row++)
                {
                    bufferIndex = row * width;
                    for (int col = 0; col < width; col++)
                    {
                        byte bValue = bBuffer[bufferIndex];
                        byte gValue = gBuffer[bufferIndex];
                        byte rValue = rBuffer[bufferIndex];
                        byte aValue = aBuffer[bufferIndex];
                        if (rValue == NoDataValue)
                        {
                            aValue = 0;
                        }
                        result[col, row] = new Rgba32(rValue, gValue, bValue, aValue);
                        bufferIndex++;
                    }
                }
            }
            return result;
        }
        private Image<Rgba32> ReadRgb(int xOffset, int yOffset, int xSize, int ySize)
        {
            if (Bands.Length < 3)
            {
                return null;
            }
            Band rBand;
            Band gBand;
            Band bBand;
            var disposeBand = false;
            if (_overview >= 0 && _overviewCount > 0)
            {
                rBand = Bands[0].GetOverview(_overview);
                gBand = Bands[1].GetOverview(_overview);
                bBand = Bands[2].GetOverview(_overview);
                disposeBand = true;
            }
            else
            {
                rBand = Bands[0];
                gBand = Bands[1];
                bBand = Bands[2];
            }

            int width, height;
            NormalizeSizeToBand(xOffset, yOffset, xSize, ySize, rBand, out width, out height);
            byte[] rBuffer = ReadBand(rBand, xOffset, yOffset, xSize, ySize, width, height);
            byte[] gBuffer = ReadBand(gBand, xOffset, yOffset, xSize, ySize, width, height);
            byte[] bBuffer = ReadBand(bBand, xOffset, yOffset, xSize, ySize, width, height);
            if (disposeBand)
            {
                rBand.Dispose();
                gBand.Dispose();
                bBand.Dispose();
            }
            Image<Rgba32> result = GetImage(width, height, rBuffer, gBuffer, bBuffer);
            return result;
        }

        private Image<Rgba32> ReadArgb(int xOffset, int yOffset, int xSize, int ySize)
        {
            if (Bands.Length < 4)
            {
                return null;
            }
            Band aBand;
            Band rBand;
            Band gBand;
            Band bBand;
            var disposeBand = false;
            if (_overview >= 0 && _overviewCount > 0)
            {
                aBand = Bands[0].GetOverview(_overview);
                rBand = Bands[1].GetOverview(_overview);
                gBand = Bands[2].GetOverview(_overview);
                bBand = Bands[3].GetOverview(_overview);
                disposeBand = true;
            }
            else
            {
                aBand = Bands[0];
                rBand = Bands[1];
                gBand = Bands[2];
                bBand = Bands[3];
            }

            int width, height;
            NormalizeSizeToBand(xOffset, yOffset, xSize, ySize, rBand, out width, out height);
            byte[] aBuffer = ReadBand(aBand, xOffset, yOffset, xSize, ySize, width, height);
            byte[] rBuffer = ReadBand(rBand, xOffset, yOffset, xSize, ySize, width, height);
            byte[] gBuffer = ReadBand(gBand, xOffset, yOffset, xSize, ySize, width, height);
            byte[] bBuffer = ReadBand(bBand, xOffset, yOffset, xSize, ySize, width, height);
            if (disposeBand)
            {
                aBand.Dispose();
                rBand.Dispose();
                gBand.Dispose();
                bBand.Dispose();
            }
            Image<Rgba32> result = GetImage(width, height, rBuffer, gBuffer, bBuffer, aBuffer);
            return result;
        }
        private Image<Rgba32> ReadPaletteBuffered(int xOffset, int yOffset, int xSize, int ySize)
        {
            ColorTable ct = FirstBand.GetRasterColorTable();
            if (ct == null)
            {
                return null;
            }

            if (ct.GetPaletteInterpretation() != PaletteInterp.GPI_RGB)
            {
                return null;
            }

            int count = ct.GetCount();
            byte[][] colorTable = new byte[ct.GetCount()][];
            for (int i = 0; i < count; i++)
            {
                using (ColorEntry ce = ct.GetColorEntry(i))
                {
                    colorTable[i] = new[] { (byte)ce.c4, (byte)ce.c1, (byte)ce.c2, (byte)ce.c3 };
                }
            }
            ct.Dispose();

            Band firstBand;
            bool disposeBand = false;
            if (_overview >= 0 && _overviewCount > 0)
            {
                firstBand = FirstBand.GetOverview(_overview);
                disposeBand = true;
            }
            else
            {
                firstBand = FirstBand;
            }

            int width, height;
            NormalizeSizeToBand(xOffset, yOffset, xSize, ySize, firstBand, out width, out height);
            byte[] indexBuffer = ReadBand(firstBand, xOffset, yOffset, xSize, ySize, width, height);
            if (disposeBand)
            {
                firstBand.Dispose();
            }
            byte[] rBuffer = new byte[indexBuffer.Length];
            byte[] gBuffer = new byte[indexBuffer.Length];
            byte[] bBuffer = new byte[indexBuffer.Length];
            byte[] aBuffer = new byte[indexBuffer.Length];
            for (int i = 0; i < indexBuffer.Length; i++)
            {
                int index = indexBuffer[i];
                aBuffer[i] = colorTable[index][0];
                rBuffer[i] = colorTable[index][1];
                gBuffer[i] = colorTable[index][2];
                bBuffer[i] = colorTable[index][3];
            }
            Image<Rgba32> result = GetImage(width, height, rBuffer, gBuffer, gBuffer, aBuffer);
            return result;
        }
        public static void NormalizeSizeToBand(int xOffset, int yOffset, int xSize, int ySize, Band band, out int width, out int height)
        {
            width = xSize;
            height = ySize;

            if (xOffset + width > band.XSize)
            {
                width = band.XSize - xOffset;
            }

            if (yOffset + height > band.YSize)
            {
                height = band.YSize - yOffset;
            }
        }
        public static byte[] ReadBand(Band band, int xOffset, int yOffset, int xSize, int ySize, int width, int height)
        {
            int length = width * height;
            byte[] buffer = new byte[length];
            DataType dataType = band.DataType;
            IntPtr bufferPtr = IntPtr.Zero;
            // Percentage truncation
            double minPercent = 0.5;
            double maxPercent = 0.5;
            band.GetMaximum(out double maxValue, out int hasvalue);
            band.GetMinimum(out double minValue, out hasvalue);
            double dValue = maxValue - minValue;
            double highValue = maxValue - dValue * maxPercent / 100;
            double lowValue = minValue + dValue * minPercent / 100;
            switch (dataType)
            {
                case DataType.GDT_Unknown:
                    throw new Exception("Unknown datatype");
                case DataType.GDT_Byte:
                    {
                        byte[] tmpBuffer = new byte[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        dValue = (byte)(highValue - lowValue);
                        for (int i = 0; i < length; i++)
                        {
                            byte value = tmpBuffer[i];
                            if (value >= highValue)
                            {
                                buffer[i] = 255;
                            }
                            else if (value <= lowValue)
                            {
                                buffer[i] = 0;
                            }
                            else
                            {
                                buffer[i] = (byte)((value - lowValue) / dValue * 255);
                            }
                        }
                    }
                    break;
                case DataType.GDT_UInt16:
                    {
                        ushort[] tmpBuffer = new ushort[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        dValue = (ushort)(highValue - lowValue);
                        for (int i = 0; i < length; i++)
                        {
                            ushort value = tmpBuffer[i];
                            if (value >= highValue)
                            {
                                buffer[i] = 255;
                            }
                            else if (value <= lowValue)
                            {
                                buffer[i] = 0;
                            }
                            else
                            {
                                buffer[i] = (byte)((value - lowValue) / dValue * 255);
                            }
                        }
                    }
                    break;
                case DataType.GDT_Int16:
                    {
                        short[] tmpBuffer = new short[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        dValue = (short)(highValue - lowValue);
                        for (int i = 0; i < length; i++)
                        {
                            short value = tmpBuffer[i];
                            if (value >= highValue)
                            {
                                buffer[i] = 255;
                            }
                            else if (value <= lowValue)
                            {
                                buffer[i] = 0;
                            }
                            else
                            {
                                buffer[i] = (byte)((value - lowValue) / dValue * 255);
                            }
                        }
                    }
                    break;
                case DataType.GDT_UInt32:
                    {
                        uint[] tmpBuffer = new uint[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        dValue = (uint)(highValue - lowValue);
                        for (int i = 0; i < length; i++)
                        {
                            uint value = tmpBuffer[i];
                            if (value >= highValue)
                            {
                                buffer[i] = 255;
                            }
                            else if (value <= lowValue)
                            {
                                buffer[i] = 0;
                            }
                            else
                            {
                                buffer[i] = (byte)((value - lowValue) / dValue * 255);
                            }
                        }
                    }
                    break;
                case DataType.GDT_Int32:
                    {
                        int[] tmpBuffer = new int[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        dValue = (int)(highValue - lowValue);
                        for (int i = 0; i < length; i++)
                        {
                            int value = tmpBuffer[i];
                            if (value >= highValue)
                            {
                                buffer[i] = 255;
                            }
                            else if (value <= lowValue)
                            {
                                buffer[i] = 0;
                            }
                            else
                            {
                                buffer[i] = (byte)((value - lowValue) / dValue * 255);
                            }
                        }
                    }
                    break;
                case DataType.GDT_Float32:
                    {
                        float[] tmpBuffer = new float[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        dValue = (float)(highValue - lowValue);
                        for (int i = 0; i < length; i++)
                        {
                            float value = tmpBuffer[i];
                            if (value >= highValue)
                            {
                                buffer[i] = 255;
                            }
                            else if (value <= lowValue)
                            {
                                buffer[i] = 0;
                            }
                            else
                            {
                                buffer[i] = (byte)((value - lowValue) / dValue * 255);
                            }
                        }
                    }
                    break;
                case DataType.GDT_Float64:
                    {
                        double[] tmpBuffer = new double[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        dValue = (double)(highValue - lowValue);
                        for (int i = 0; i < length; i++)
                        {
                            double value = tmpBuffer[i];
                            if (value >= highValue)
                            {
                                buffer[i] = 255;
                            }
                            else if (value <= lowValue)
                            {
                                buffer[i] = 0;
                            }
                            else
                            {
                                buffer[i] = (byte)((value - lowValue) / dValue * 255);
                            }
                        }
                    }
                    break;
                case DataType.GDT_CInt16:
                case DataType.GDT_CInt32:
                case DataType.GDT_CFloat32:
                case DataType.GDT_CFloat64:
                case DataType.GDT_TypeCount:
                    throw new NotImplementedException();
            }
            return buffer;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Dataset?.Dispose();
                Symbology?.Dispose();
                Dataset = null;
                Symbology = null;
            }
            base.Dispose(disposing);
        }
    }
}