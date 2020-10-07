using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace EM.GIS.Data
{
    /// <summary>
    /// This can be used as a component to work as a DataManager. This also provides the very important DefaultDataManager property,
    /// which is where the developer controls what DataManager should be used for their project.
    /// </summary>
    public class DataManager : IDataManager
    {
        [Browsable(false)]
        [ImportMany(AllowRecomposition = true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IEnumerable<IDriver> Drivers { get; set; }

        public IEnumerable<IVectorDriver> VectorDrivers => Drivers.Where(x => x is IVectorDriver).Select(x => x as IVectorDriver);

        public IEnumerable<IRasterDriver> RasterDrivers => Drivers.Where(x => x is IRasterDriver).Select(x => x as IRasterDriver);

        [Category("Handlers")]
        [Description("Gets or sets the object that implements the IProgressHandler interface for recieving status messages.")]
        public virtual IProgressHandler ProgressHandler { get; set; }

        private static IDataManager _default;
        /// <summary>
        /// 默认数据管理器
        /// </summary>
        public static IDataManager Default
        {
            get
            {
                return _default ?? (_default = new DataManager());
            }
            set
            {
                _default = value;
            }
        }
        public IRasterSet OpenRaster(string fileName)
        {
            IRasterSet dataSet = null;
            if (Drivers != null)
            {
                foreach (var driver in RasterDrivers)
                {
                    try
                    {
                        dataSet = driver.Open(fileName, true);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        try
                        {
                            dataSet = driver.Open(fileName, false);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                    if (dataSet != null)
                    {
                        break;
                    }
                }
            }
            return dataSet;
        }

        public IFeatureSet OpenVector(string fileName)
        {
            IFeatureSet dataSet = null;
            if (Drivers != null)
            {
                foreach (var driver in VectorDrivers)
                {
                    try
                    {
                        dataSet = driver.Open(fileName, true);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        try
                        {
                            dataSet = driver.Open(fileName, false);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                    if (dataSet != null)
                    {
                        break;
                    }
                }
            }
            return dataSet;
        }


        public IDataSet Open(string path)
        {
            IDataSet dataSet = null;
            if (Drivers != null)
            {
                foreach (var driver in Drivers)
                {
                    try
                    {
                        dataSet = driver.Open(path, true);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                        try
                        {
                            dataSet = driver.Open(path, false);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                    }
                    if (dataSet != null)
                    {
                        break;
                    }
                }
            }
            return dataSet;
        }

    }
}
