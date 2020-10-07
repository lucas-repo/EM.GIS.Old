using EM.GIS.Data;
using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EM.GIS.Gdals
{
    /// <summary>
    /// GdalRasterProvider
    /// </summary>
    public class GdalRasterProvider 
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GdalRasterProvider"/> class.
        /// </summary>
        public GdalRasterProvider()
        {
            // Add ourself in for these extensions, unless another provider is registered for them.
            //string[] extensions = { ".tif", ".tiff", ".adf", ".img", ".jpg" };
            //foreach (string extension in extensions)
            //{
            //    if (!DataManager.Default.PreferredProviders.ContainsKey(extension))
            //    {
            //        DataManager.Default.PreferredProviders.Add(extension, this);
            //    }
            //}
        }

        #endregion

        #region Properties

        // This function checks if a GeoTiff dataset should be interpreted as a one-band raster
        // or as an image. Returns true if the dataset is a valid one-band raster.

        /// <summary>
        /// Gets the description of the raster.
        /// </summary>
        public string Description => "GDAL Integer Raster";

        /// <summary>
        /// Gets the dialog filter to use when opening a file.
        /// </summary>
        public string DialogReadFilter => "GDAL Rasters|*.asc;*.adf;*.bil;*.gen;*.thf;*.blx;*.xlb;*.bt;*.dt0;*.dt1;*.dt2;*.tif;*.dem;*.ter;*.mem;*.img;*.nc";

        /// <summary>
        /// Gets the dialog filter to use when saving to a file.
        /// </summary>
        public string DialogWriteFilter => "AAIGrid|*.asc;*.adf|DTED|*.dt0;*.dt1;*.dt2|GTiff|*.tif;*.tiff|TERRAGEN|*.ter|GenBin|*.bil|netCDF|*.nc|Imagine|*.img|GFF|*.gff|Terragen|*.ter";

        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        public string Name => "GDAL Raster Provider";

        /// <summary>
        /// Gets or sets the progress handler that gets updated with progress information.
        /// </summary>
        public IProgressHandler ProgressHandler { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Opens the specified file.
        /// </summary>
        /// <param name="fileName">Name of the file that gets opened.</param>
        /// <returns>The file as IRasterSet.</returns>
        public IRasterSet Open(string fileName)
        {
            IRasterSet result = null;
            var dataset = GdalExtentions.Open(fileName);
            if (dataset != null)
            {
                // Single band rasters are easy, just return the band as the raster.
                // TODO: make a more complicated raster structure with individual bands.
                result = GetBand(fileName, dataset);

                // If we opened the dataset but did not find a raster to return, close the dataset
                if (result == null)
                {
                    dataset.Dispose();
                }
            }

            return result;
        }

        private static IRasterSet GetBand(string fileName, Dataset dataset)
        {
            IRasterSet result = null;
            Band band = dataset?.GetRasterBand(1);
            if (band == null)
            {
                return null;
            }
            switch (band.DataType)
            {
                case DataType.GDT_Byte:
                    result = new GdalRasterSet<byte>(fileName, dataset);
                    break;
                case DataType.GDT_CFloat32:
                case DataType.GDT_CFloat64:
                case DataType.GDT_CInt16:
                case DataType.GDT_CInt32: break;
                case DataType.GDT_Float32:
                    result = new GdalRasterSet<float>(fileName, dataset);
                    break;
                case DataType.GDT_Float64:
                    result = new GdalRasterSet<double>(fileName, dataset);
                    break;
                case DataType.GDT_Int16:
                    result = new GdalRasterSet<short>(fileName, dataset);
                    break;
                case DataType.GDT_UInt16:
                case DataType.GDT_Int32:
                    result = new GdalRasterSet<int>(fileName, dataset);
                    break;
                case DataType.GDT_TypeCount: break;

                case DataType.GDT_UInt32:
                    result = new GdalRasterSet<long>(fileName, dataset);
                    break;
                case DataType.GDT_Unknown: break;
                default: break;
            }

            return result;
        }
        private static string GetDriverCode(string fileExtension)
        {
            if (string.IsNullOrEmpty(fileExtension))
            {
                return null;
            }

            switch (fileExtension.ToLower())
            {
                case ".asc": return "AAIGrid";

                case ".adf": return "AAIGrid";

                case ".tiff":
                case ".tif": return "GTiff";

                case ".img": return "HFA";

                case ".gff": return "GFF";

                case ".dt0":
                case ".dt1":
                case ".dt2": return "DTED";

                case ".ter": return "Terragen";

                case ".nc": return "netCDF";

                default: return null;
            }
        }

        private static IRasterSet WrapDataSetInRaster(string name, Type dataType, Dataset dataset)
        {
            // todo: what about UInt32?
            if (dataType == typeof(int) || dataType == typeof(ushort))
            {
                return new GdalRasterSet<int>(name, dataset);
            }

            if (dataType == typeof(short))
            {
                return new GdalRasterSet<short>(name, dataset);
            }

            if (dataType == typeof(float))
            {
                return new GdalRasterSet<float>(name, dataset);
            }

            if (dataType == typeof(double))
            {
                return new GdalRasterSet<double>(name, dataset);
            }

            if (dataType == typeof(byte))
            {
                return new GdalRasterSet<byte>(name, dataset);
            }

            // It was an unsupported type.
            if (dataset != null)
            {
                dataset.Dispose();
                if (File.Exists(name)) File.Delete(name);
            }

            return null;
        }

        #endregion
    }
}
