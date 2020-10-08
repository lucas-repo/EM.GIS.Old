using EM.GIS.Geometries;
using EM.GIS.Projection;
using EM.GIS.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace EM.GIS.Data
{
    /// <summary>
    /// DataSet
    /// </summary>
    [Serializable]
    public abstract class DataSet : BaseCopy, IDataSet
    {

        #region Properties

        /// <summary>
        /// Gets or sets the extent for the dataset. Usages to Envelope were replaced
        /// as they required an explicit using to DotSpatial.Topology which is not
        /// as intuitive. Extent.ToEnvelope() and new Extent(myEnvelope) convert them.
        /// This is designed to be a virtual member to be overridden by subclasses,
        /// and should not be called directly by the constructor of inheriting classes.
        /// </summary>
        public virtual IExtent Extent { get; }

        private string _filename;
        /// <summary>
        /// Gets or sets the file name of a file based data set. The file name should be the absolute path including
        /// the file extension. For data sets coming from a database or a web service, the Filename property is NULL.
        /// </summary>
        public string Filename
        {
            get => _filename;
            set
            {
                _filename = value;
                if (File.Exists(Filename))
                {
                    RelativeFilename = FilePathUtils.RelativePathTo(Filename);
                }
                else
                {
                    RelativeFilename = Filename;
                }
            }
        }

        /// <summary>
        /// Gets or sets the string name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the progress handler to use for internal actions taken by this dataset.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual IProgressHandler ProgressHandler { get; set; }

        public bool IsDisposed { get; private set; }

        #endregion

        public virtual string RelativeFilename { get; protected set; }

        public virtual bool CanReproject { get; }

        public virtual ProjectionInfo Projection { get; protected set; }
        public virtual ITransformation Transformation { get; set; }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }
                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                IsDisposed = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~DataSet()
        // {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }

        public virtual void Save() { }
        public virtual void SaveAs(string filename, bool overwrite) { }

        #endregion

    }
}

