using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace EM.GIS.Gdals
{
    public static class GdalExtentions
    {
        /// <summary>
        /// Opens the given file.
        /// </summary>
        /// <param name="fileName">File that gets opened.</param>
        /// <returns>Opened file as data set.</returns>
        public static Dataset Open(string fileName)
        {
            Dataset dataset = null;
            if (File.Exists(fileName))
            {
                try
                {
                    dataset = Gdal.Open(fileName, Access.GA_Update);
                }
                catch
                {
                    try
                    {
                        dataset = Gdal.Open(fileName, Access.GA_ReadOnly);
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine($"打开“{fileName}”失败，{ex.Message}");
                    }
                }
            }
            return dataset;
        }

        public static int CreateOverview(this Dataset dataset, string resampling = "NEAREST", int[] overviewlist = null)
        {
            int value = -1;
            if (dataset == null || dataset.RasterCount <= 0)
            {
                return value;
            }

            if (overviewlist == null)
            {
                List<int> intList = new List<int>();
                int width = dataset.RasterXSize;
                int height = dataset.RasterYSize;
                int k = 1;
                while (width > 256 && height > 256)
                {
                    k *= 2;
                    intList.Add(k);
                    width /= 2;
                    height /= 2;
                }

                overviewlist = intList.ToArray();
            }

            value = dataset.BuildOverviews(resampling, overviewlist);
            return value;
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
        public static byte[] ReadBand(Band band, int xOffset, int yOffset, int width, int height)
        {
            int length = width * height;
            byte[] buffer = new byte[length];
            DataType dataType = band.DataType;
            // Percentage truncation
            double minPercent = 0.5;
            double maxPercent = 0.5;
            band.GetMaximum(out double maxValue, out _);
            band.GetMinimum(out double minValue, out _);
            double dValue = maxValue - minValue;
            double highValue = maxValue - dValue * maxPercent / 100;
            double lowValue = minValue + dValue * minPercent / 100;
            double dValuePercent = highValue - lowValue;
            IntPtr bufferPtr;
            switch (dataType)
            {
                case DataType.GDT_Unknown:
                    throw new Exception("Unknown datatype");
                case DataType.GDT_Byte:
                    {
                        byte[] tmpBuffer = new byte[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        for (int i = 0; i < length; i++)
                        {
                            var value = tmpBuffer[i];
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
                                buffer[i] = (byte)((value - lowValue) / dValuePercent * 255);
                            }
                        }
                    }
                    break;
                case DataType.GDT_UInt16:
                    {
                        ushort[] tmpBuffer = new ushort[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        for (int i = 0; i < length; i++)
                        {
                            var value = tmpBuffer[i];
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
                                buffer[i] = (byte)((value - lowValue) / dValuePercent * 255);
                            }
                        }
                    }
                    break;
                case DataType.GDT_Int16:
                    {
                        short[] tmpBuffer = new short[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        for (int i = 0; i < length; i++)
                        {
                            var value = tmpBuffer[i];
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
                                buffer[i] = (byte)((value - lowValue) / dValuePercent * 255);
                            }
                        }
                    }
                    break;
                case DataType.GDT_UInt32:
                    {
                        uint[] tmpBuffer = new uint[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        for (int i = 0; i < length; i++)
                        {
                            var value = tmpBuffer[i];
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
                                buffer[i] = (byte)((value - lowValue) / dValuePercent * 255);
                            }
                        }
                    }
                    break;
                case DataType.GDT_Int32:
                    {
                        int[] tmpBuffer = new int[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        for (int i = 0; i < length; i++)
                        {
                            var value = tmpBuffer[i];
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
                                buffer[i] = (byte)((value - lowValue) / dValuePercent * 255);
                            }
                        }
                    }
                    break;
                case DataType.GDT_Float32:
                    {
                        float[] tmpBuffer = new float[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        for (int i = 0; i < length; i++)
                        {
                            var value = tmpBuffer[i];
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
                                buffer[i] = (byte)((value - lowValue) / dValuePercent * 255);
                            }
                        }
                    }
                    break;
                case DataType.GDT_Float64:
                    {
                        double[] tmpBuffer = new double[length];
                        bufferPtr = GCHandleHelper.GetIntPtr(tmpBuffer);
                        band.ReadRaster(xOffset, yOffset, width, height, bufferPtr, width, height, dataType, 0, 0);
                        for (int i = 0; i < length; i++)
                        {
                            var value = tmpBuffer[i];
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
                                buffer[i] = (byte)((value - lowValue) / dValuePercent * 255);
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
    }
}
