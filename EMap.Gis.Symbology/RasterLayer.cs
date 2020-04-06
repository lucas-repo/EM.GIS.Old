using EMap.Gis.Data;
using OSGeo.GDAL;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading;

namespace EMap.Gis.Symbology
{
    public class RasterLayer : BaseLayer, IRasterLayer
    {
        private int _overview;
        private int _overviewCount;
        private ColorInterp _colorInterp;
        public Dataset Dataset { get; set; }
        /// <summary>
        /// 仿射六参数：0:左上角x坐标，1:X轴分辨率，2:Y轴旋转角，3:左上角y坐标，4:X轴旋转角，5:Y轴分辨率.
        ///  x = Affine[0] + Affine[1] * col + Affine[2] * row;
        ///  y = Affine[3] + Affine[4] * col + Affine[5] * row;
        /// </summary>
        public double[] Affine { get; set; }
        public int RasterXSize { get => Dataset.RasterXSize; }
        public int RasterYSize { get => Dataset.RasterYSize; }
        public Band[] Bands { get; private set; }
        Band FirstBand { get => Bands.Length > 0 ? Bands[0] : null; }
        public int NumBands { get => Dataset.RasterCount; }
        public double NoDataValue { get; private set; }
        public RasterLayer()
        {
        }
        public RasterLayer(string fileName)
        {
            Dataset dataset = LayerFactory.OpenDataset(fileName);
            Configure(dataset);
        }

        public RasterLayer(Dataset dataset)
        {
            Configure(dataset);
        }
        private void Configure(Dataset dataset)
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
        private Extent _extent;
        public override Extent Extent
        {
            get
            {
                if (_extent == null)
                {
                    _extent = new Extent();
                }
                _extent.MinX = GetXMin();
                _extent.MinY = GetYMin();
                _extent.MaxX = GetXMax();
                _extent.MaxY = GetYMax();
                return _extent;
            }
        }
        private double GetXMin()
        {
            double xMin = double.MaxValue;
            double[] affine = Affine; // in case this is an overridden property
            double nr = RasterYSize;
            double nc = RasterXSize;

            // Because these coefficients can be negative, we can't make assumptions about what corner is furthest left.
            if (affine[0] < xMin) xMin = affine[0]; // TopLeft;
            if (affine[0] + (nc * affine[1]) < xMin) xMin = affine[0] + (nc * affine[1]); // TopRight;
            if (affine[0] + (nr * affine[2]) < xMin) xMin = affine[0] + (nr * affine[2]); // BottomLeft;
            if (affine[0] + (nc * affine[1]) + (nr * affine[2]) < xMin) xMin = affine[0] + (nc * affine[1]) + (nr * affine[2]); // BottomRight

            // the coordinate thus far is the center of the cell. The actual left is half a cell further left.
            xMin = xMin - (Math.Abs(affine[1]) / 2) - (Math.Abs(affine[2]) / 2);
            return xMin;
        }
        private double GetYMax()
        {
            double yMax = double.MinValue;
            double[] affine = Affine; // in case this is an overridden property
            double nr = RasterYSize;
            double nc = RasterXSize;

            // Because these coefficients can be negative, we can't make assumptions about what corner is furthest left.
            if (affine[3] > yMax) yMax = affine[3]; // TopLeft;
            if (affine[3] + (nc * affine[4]) > yMax) yMax = affine[3] + (nc * affine[4]); // TopRight;
            if (affine[3] + (nr * affine[5]) > yMax) yMax = affine[3] + (nr * affine[5]); // BottomLeft;
            if (affine[3] + (nc * affine[4]) + (nr * affine[5]) > yMax) yMax = affine[3] + (nc * affine[4]) + (nr * affine[5]); // BottomRight

            // the value thus far is at the center of the cell. Return a value half a cell further
            return yMax + (Math.Abs(affine[4]) / 2) + (Math.Abs(affine[5]) / 2);
        }
        public double GetXMax()
        {
            double width = (RasterXSize * Math.Abs(Affine[1])) + (RasterYSize * Math.Abs(Affine[2]));
            double xMax = GetXMin() + width;
            return xMax;
        }
        public double GetYMin()
        {
            double height = (Math.Abs(Affine[4]) * RasterXSize) + (Math.Abs(Affine[5]) * RasterYSize);
            double yMin = GetYMax() - height;
            return yMin;
        }


        private Bitmap GetImage(int xOffset, int yOffset, int xSize, int ySize)
        {
            Bitmap result = null;
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

        private Bitmap ReadGrayIndex(int xOffset, int yOffset, int xSize, int ySize)
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
            Bitmap result = GetImage(width, height, rBuffer, rBuffer, rBuffer);
            return result;
        }
        private unsafe Bitmap GetImage(int width, int height, byte[] rBuffer, byte[] gBuffer, byte[] bBuffer, byte[] aBuffer = null)
        {
            if (width <= 0 || height <= 0)
            {
                return null;
            }
            Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData bData = result.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* scan0 = (byte*)bData.Scan0;
            int stride = bData.Stride;
            int dWidth = stride - width * 4;
            int ptrIndex = -1;
            int bufferIndex = -1;
            if (aBuffer == null)
            {
                for (int row = 0; row < height; row++)
                {
                    ptrIndex = row * stride;
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
                        scan0[ptrIndex] = bValue;
                        scan0[ptrIndex + 1] = gValue;
                        scan0[ptrIndex + 2] = rValue;
                        scan0[ptrIndex + 3] = aValue;
                        ptrIndex += 4;
                        bufferIndex++;
                    }
                }
            }
            else
            {
                for (int row = 0; row < height; row++)
                {
                    ptrIndex = row * stride;
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
                        scan0[ptrIndex] = bValue;
                        scan0[ptrIndex + 1] = gValue;
                        scan0[ptrIndex + 2] = rValue;
                        scan0[ptrIndex + 3] = aValue;
                        ptrIndex += 4;
                        bufferIndex++;
                    }
                }
            }
            result.UnlockBits(bData);
            return result;
        }
        private Bitmap ReadRgb(int xOffset, int yOffset, int xSize, int ySize)
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
            Bitmap result = GetImage(width, height, rBuffer, gBuffer, bBuffer);
            return result;
        }

        private Bitmap ReadArgb(int xOffset, int yOffset, int xSize, int ySize)
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
            Bitmap result = GetImage(width, height, rBuffer, gBuffer, bBuffer, aBuffer);
            return result;
        }
        private Bitmap ReadPaletteBuffered(int xOffset, int yOffset, int xSize, int ySize)
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
            Bitmap result = GetImage(width, height, rBuffer, gBuffer, gBuffer, aBuffer);
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
                if (Bands != null)
                {
                    foreach (var band in Bands)
                    {
                        band?.Dispose();
                    }
                }
                Dataset?.Dispose();
                Dataset = null;
                Symbology = null;
            }
            base.Dispose(disposing);
        }
        protected override void OnDraw(Graphics graphics, Rectangle rectangle, Extent extent, bool selected = false, ProgressHandler progressHandler = null, CancellationTokenSource cancellationTokenSource = null)
        {
            if (Bands.Length == 0)
            {
                return;
            }
            if (selected || cancellationTokenSource?.IsCancellationRequested == true)
            {
                return;
            }
            var layerIsDrawing = $"{Name} 绘制中...";
            progressHandler?.Invoke(5, layerIsDrawing);

            // Gets the scaling factor for converting from geographic to pixel coordinates
            double width = extent.MaxX - extent.MinX;//范围宽
            double height = extent.MaxY - extent.MinY;//范围高
            double dx = rectangle.Width / width;//x分辨率倒数
            double dy = rectangle.Height / height;//y分辨率倒数

            double[] a = Affine;

            // calculate inverse
            double p = 1 / ((a[1] * a[5]) - (a[2] * a[4]));
            double[] aInv = new double[4];
            aInv[0] = a[5] * p;
            aInv[1] = -a[2] * p;
            aInv[2] = -a[4] * p;
            aInv[3] = a[1] * p;

            // estimate rectangle coordinates
            double tlx = ((extent.MinX - a[0]) * aInv[0]) + ((extent.MaxY - a[3]) * aInv[1]);
            double tly = ((extent.MinX - a[0]) * aInv[2]) + ((extent.MaxY - a[3]) * aInv[3]);
            double trx = ((extent.MaxX - a[0]) * aInv[0]) + ((extent.MaxY - a[3]) * aInv[1]);
            double trY = ((extent.MaxX - a[0]) * aInv[2]) + ((extent.MaxY - a[3]) * aInv[3]);
            double blx = ((extent.MinX - a[0]) * aInv[0]) + ((extent.MinY - a[3]) * aInv[1]);
            double bly = ((extent.MinX - a[0]) * aInv[2]) + ((extent.MinY - a[3]) * aInv[3]);
            double brx = ((extent.MaxX - a[0]) * aInv[0]) + ((extent.MinY - a[3]) * aInv[1]);
            double bry = ((extent.MaxX - a[0]) * aInv[2]) + ((extent.MinY - a[3]) * aInv[3]);

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

            progressHandler?.Invoke(10, layerIsDrawing);

            // gets the affine scaling factors.
            float m11 = Convert.ToSingle(a[1] * dx);//x缩放值
            float m22 = Convert.ToSingle(a[5] * -dy);//y缩放值
            float m21 = Convert.ToSingle(a[2] * dx);//旋转值
            float m12 = Convert.ToSingle(a[4] * -dy);//旋转值
            //double l = a[0] - (.5 * (a[1] + a[2])); // Left of top left pixel
            //double t = a[3] - (.5 * (a[4] + a[5])); // top of top left pixel
            double l = a[0]; // Left of top left pixel
            double t = a[3]; // top of top left pixel
            float xShift = (float)((l - extent.MinX) * dx);
            float yShift = (float)((extent.MaxY - t) * dy);

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

            progressHandler?.Invoke(15, layerIsDrawing);

            var overviewPow = Math.Pow(2, _overview + 1);

            // witdh and height of the image
            var w = (bRx - tLx) / overviewPow;
            var h = (bRy - tLy) / overviewPow;

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
            Matrix originMatrix = graphics.Transform;
            using (var matrix = new Matrix(m11 * (float)overviewPow, m12 * (float)overviewPow, m21 * (float)overviewPow, m22 * (float)overviewPow, xShift, yShift))
            {
                graphics.Transform = matrix;
            }

            progressHandler?.Invoke(20, $"{Name} 绘制中...");
            var increment = 70.0 / nbX / nbY;
            double progressPercent = 20;
            for (var i = 0; i < nbX; i++)
            {
                for (var j = 0; j < nbY; j++)
                {
                    double xOffsetD = (tLx / overviewPow) + (i * blockXsize);
                    double yOffsetD = (tLy / overviewPow) + (j * blockYsize);
                    int xOffsetI = (int)Math.Floor(xOffsetD);
                    int yOffsetI = (int)Math.Floor(yOffsetD);
                    // The +1 is to remove the white stripes artifacts
                    int xSize = blockXsize + 1;
                    int ySize = blockYsize + 1;
                    try
                    {
                        using (Bitmap childImage = GetImage(xOffsetI, yOffsetI, xSize, ySize))
                        {
                            if (childImage != null)
                            {
                                graphics.DrawImage(childImage, xOffsetI, yOffsetI);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine($"获取图片失败：{e.Message}");
                    }
                    progressPercent += increment;
                    progressHandler?.Invoke((int)progressPercent, $"{Name} 绘制中...");
                }
            }
            graphics.Transform = originMatrix;
            progressHandler?.Invoke(99, $"{Name} 绘制中...");
        }
    }
}