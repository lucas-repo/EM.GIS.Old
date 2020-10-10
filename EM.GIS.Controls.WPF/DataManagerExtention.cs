using EM.GIS.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.GIS.WpfControls
{
    /// <summary>
    /// Extend the data manager with some convenient dialog spawning options.
    /// </summary>
    public static class DataManagerExt
    {
        /// <summary>
        /// This opens a file, but populates the dialog filter with only vector formats.
        /// </summary>
        /// <param name="self">this</param>
        /// <returns>An IFeatureSet with the data from the file specified in a dialog, or null if nothing load.</returns>
        public static IFeatureSet OpenVector(this IDataFactory self)
        {
            IFeatureSet featureSet = null;
            var ofd = new OpenFileDialog { Filter = self.VectorReadFilter };

            var ret = ofd.ShowDialog();
            if (ret.HasValue && ret.Value)
            {
                featureSet = self.Open(ofd.FileName) as IFeatureSet;
            }
            return featureSet;
        }

        /// <summary>
        /// This uses an open dialog filter with only vector extensions but where multi-select is
        /// enabled, hence allowing multiple vectors to be returned in this list.
        /// </summary>
        /// <param name="self">this</param>
        /// <returns>The enumerable or vectors.</returns>
        public static IEnumerable<IFeatureSet> OpenVectors(this IDataFactory self)
        {
            var ofd = new OpenFileDialog { Filter = self.VectorReadFilter, Multiselect = true };
            var ret = ofd.ShowDialog();
            if (!ret.HasValue || !ret.Value)
            {
                yield break;
            }
            foreach (var name in ofd.FileNames)
            {
                var ds = self.OpenVector(name);
                if (ds != null) yield return ds;
            }
        }

        /// <summary>
        /// This launches an open file dialog and attempts to load the specified file.
        /// </summary>
        /// <param name="self">this</param>
        /// <returns>An IDataSet with the data from the file specified in an open file dialog</returns>
        public static IDataSet OpenFile(this IDataFactory self)
        {
            var ofd = new OpenFileDialog { Filter = self.DialogReadFilter };
            var ret = ofd.ShowDialog();
            if (!ret.HasValue || !ret.Value)
            { return null; }
            return self.Open(ofd.FileName);
        }

        /// <summary>
        /// This launches an open file dialog that allows loading of several files at once
        /// and returns the datasets in a list.
        /// </summary>
        /// <param name="self">this</param>
        /// <returns>An enumerable of all the files that were opened.</returns>
        public static IEnumerable<IDataSet> OpenFiles(this IDataFactory self)
        {
            var ofd = new OpenFileDialog { Multiselect = true, Filter = self.DialogReadFilter };
            var ret = ofd.ShowDialog();
            if (!ret.HasValue|| !ret.Value) yield break;

            var filterparts = ofd.Filter.Split('|');
            var pos = (ofd.FilterIndex - 1) * 2;
            int index = filterparts[pos].IndexOf(" - ", StringComparison.Ordinal);
            var filterName = index > 0 ? filterparts[pos].Remove(index) : string.Empty; // provider entries contain a -, entries without - aren't specific providers but lists that contain endings more than one provider can open

            foreach (var name in ofd.FileNames)
            {
                var ds = self.Open(name,  filterName);
                if (ds != null) yield return ds;
            }

        }

        /// <summary>
        /// This opens a file, but populates the dialog filter with only raster formats.
        /// </summary>
        /// <param name="self">this</param>
        /// <returns>An IRaster with the data from the file specified in an open file dialog</returns>
        public static IRasterSet OpenRaster(this IDataFactory self)
        {
            var ofd = new OpenFileDialog { Filter = self.RasterReadFilter };
            var ret = ofd.ShowDialog();
            if (!ret.HasValue || !ret.Value) return null;
            return self.Open(ofd.FileName) as IRasterSet;
        }

        /// <summary>
        /// This uses an open dialog filter with only raster extensions but where multi-select is
        /// enabled, hence allowing multiple rasters to be returned in this list.
        /// </summary>
        /// <param name="self">this</param>
        /// <returns>An enumerable or rasters.</returns>
        public static IEnumerable<IRasterSet> OpenRasters(this IDataFactory self)
        {
            var ofd = new OpenFileDialog { Filter = self.RasterReadFilter, Multiselect = true };
            var ret = ofd.ShowDialog();
            if (!ret.HasValue || !ret.Value) yield break;
            foreach (var name in ofd.FileNames)
            {
                var ds = self.OpenRaster(name);
                if (ds != null) yield return ds;
            }
        }
    }
}
