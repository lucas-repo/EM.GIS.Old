using EMap.Gis.Data;
using EMap.Gis.Resources;
using EMap.Gis.Serialization;
using EMap.Gis.Symbology;
using OSGeo.GDAL;
using OSGeo.OGR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EMap.Gis.Symbology
{
    public class MapFrame : Group, IMapFrame
    {
        private BackgroundWorker _bw;//后台绘制工作线程
        private object _lockObject = new object();
        private int _busyCount;

        private bool BwIsBusy
        {
            get => _busyCount > 0;
            set
            {
                lock (_lockObject)
                {
                    if (value)
                    {
                        _busyCount++;
                    }
                    else
                    {
                        _busyCount--;
                    }
                    if (_busyCount < 0)
                    {
                        _busyCount = 0;
                    }
                }
            }
        }
        private Rectangle _viewBounds;
        public Rectangle ViewBounds
        {
            get { return _viewBounds; }
            set 
            {
                if (_viewBounds == value) 
                { 
                    return; 
                }
                _viewBounds = value;
                OnViewBoundsChanged();
            }
        }
        public ProgressHandler ProgressHandler { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
        private int _width;
        public int Width => _width;
        private int _height;
        public int Height => _height;

        public ILayerCollection DrawingLayers { get; }
        private Color _backGround = Color.Transparent;
        public Color BackGround
        {
            get { return _backGround; }
            set { _backGround = value; OnBackGroundChanged(); }
        }

        public event EventHandler BufferChanged;
        private Image _backBuffer;
        public Image BackBuffer
        {
            get { return _backBuffer; }
            set
            {
                if (_backBuffer == value)
                {
                    return;
                }
                lock (_lockObject)
                {
                    _backBuffer?.Dispose();
                    _backBuffer = value;
                }
                OnBackBufferChanged();
            }
        }

        private void OnBackBufferChanged()
        {
            _viewBounds = new Rectangle(0, 0, _width, _height);
            OnBufferChanged();
        }

        private Extent _viewExtent = new Extent(-180, -90, 180, 90);

        public Extent ViewExtent
        {
            get { return _viewExtent; }
            set
            {
                if (value == null) return;
                Extent ext = value.Copy();
                ResetAspectRatio(ext);
                _viewExtent = ext;
                OnViewExtentChanged();
            }
        }

        private Rectangle _bounds;
        public Rectangle Bounds
        {
            get => _bounds;
            private set
            {
                if (_bounds != value)
                {
                    _bounds = value;
                }
            }
        }
        private int _isBusyIndex;
        public bool IsBusy
        {
            get
            {
                return _isBusyIndex > 0;
            }
            set
            {
                if (value) _isBusyIndex++;
                else _isBusyIndex--;
                if (_isBusyIndex <= 0)
                {
                    _isBusyIndex = 0;
                }
            }
        }

        public MapFrame(int width, int height)
        {
            _width = width;
            _height = height;
            _bounds = new Rectangle(0, 0, _width, _height);
            _bw = new BackgroundWorker()
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true 
            };
            _bw.DoWork += Bw_DoWork;
            _bw.ProgressChanged += Bw_ProgressChanged;
            _bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            _viewBounds = new Rectangle(0, 0, _width, _height);
            DrawingLayers = new LayerCollection();
            Layers.CollectionChanged += Layers_CollectionChanged;
        }
        protected void ResetAspectRatio(Extent newEnv)
        {
            // Aspect Ratio Handling
            if (newEnv == null) return;

            // It isn't exactly an exception, but rather just an indication not to do anything here.
            if (_height == 0 || _width == 0) return;

            double controlAspect = (double)_width / _height;
            double envelopeAspect = newEnv.Width / newEnv.Height;
            Coordinate center = newEnv.Center;

            if (controlAspect > envelopeAspect)
            {
                // The Control is proportionally wider than the envelope to display.
                // If the envelope is proportionately wider than the control, "reveal" more width without
                // changing height If the envelope is proportionately taller than the control,
                // "hide" width without changing height
                newEnv.SetCenter(center, newEnv.Height * controlAspect, newEnv.Height);
            }
            else
            {
                // The control is proportionally taller than the content is
                // If the envelope is proportionately wider than the control,
                // "hide" the extra height without changing width
                // If the envelope is proportionately taller than the control, "reveal" more height without changing width
                newEnv.SetCenter(center, newEnv.Width, newEnv.Width / controlAspect);
            }

        }
        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                _bw.RunWorkerAsync();
                return;
            }
            IsBusy = false;
            _busySet = false;
            ProgressHandler?.Invoke(0, string.Empty);
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressHandler?.Invoke( e.ProgressPercentage, Strings.DrawingLayer);
        }
        bool _busySet;
        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!_busySet)
            {
                _busySet = true;
                IsBusy = true;
            }
            var worker = sender as BackgroundWorker;
            if (worker != null)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                }
                else
                {
                    worker.ReportProgress(10);
                    /*DoWork(e);*/ //TODO 绘制buffer
                }
            }
        }

        bool firstLayerAdded;
        private void Layers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!firstLayerAdded)
            {
                if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems.Count > 0)
                {
                    firstLayerAdded = true;
                }
                if (firstLayerAdded)
                {
                    ViewExtent = Extent;
                    return;
                }
            }
            ResetBuffer();
        }

        private void OnViewExtentChanged()
        {
            ResetBuffer();
        }

        private void OnViewBoundsChanged()
        {
            OnBufferChanged();
        }

        private void OnBackGroundChanged()
        {
            ResetBuffer();
        }

        private void OnBufferChanged()
        {
            BufferChanged?.Invoke(this, new EventArgs());
        }
        protected override void OnDraw(Graphics graphics, Rectangle rectangle, Extent extent, bool selected = false, ProgressHandler progressHandler = null, CancellationTokenSource cancellationTokenSource = null)
        {
            base.OnDraw(graphics, rectangle, extent, selected, progressHandler, cancellationTokenSource);
            var visibleDrawingLayers = DrawingLayers?.Where(x => x.GetVisible(extent, rectangle));
            foreach (var layer in visibleDrawingLayers)
            {
                if (CancellationTokenSource?.IsCancellationRequested == true)
                {
                    break;
                }
                layer?.Draw(graphics, rectangle, extent, selected, progressHandler, cancellationTokenSource);
            }

            var featureLayers = GetFeatureLayers().Union(visibleDrawingLayers.Where(x => x is IFeatureLayer).Select(x => x as IFeatureLayer));
            var labelLayers = featureLayers.Where(x => x.LabelLayer?.GetVisible(extent, rectangle)==true).Select(x => x.LabelLayer);
            foreach (var layer in labelLayers)
            {
                if (CancellationTokenSource?.IsCancellationRequested == true)
                {
                    break;
                }
                layer.Draw(graphics, rectangle, extent, selected, ProgressHandler, CancellationTokenSource);
            }
        }
        public Point ProjToBuffer(Coordinate location)
        {
            if (_width == 0 || _height == 0) return new Point(0, 0);
            int x = (int)((location.X - ViewExtent.MinX) * (_width / ViewExtent.Width)) + ViewBounds.X;
            int y = (int)((ViewExtent.MaxY - location.Y) * (_height / ViewExtent.Height)) + ViewBounds.Y;
            return new Point(x, y);
        }
        public Rectangle ProjToBuffer(Extent extent)
        {
            Coordinate tl = new Coordinate(extent.MinX, extent.MaxY);
            Coordinate br = new Coordinate(extent.MaxX, extent.MinY);
            Point topLeft = ProjToBuffer(tl);
            Point bottomRight = ProjToBuffer(br);
            return new Rectangle(topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y);
        }

        /// <summary>
        /// 根据指定范围重置缓存图片
        /// </summary>
        /// <param name="extent"></param>
        public async Task ResetBuffer(Extent extent = null)
        {
            await Task.Run(() =>
             {
                 if (extent == null)
                 {
                     extent = ViewExtent;
                 }
                 Bitmap tmpBuffer = null;
                 if (Width > 0 && Height > 0)
                 {
                     tmpBuffer = new Bitmap(Width, Height);
                     #region 绘制MapFrame
                     Rectangle rectangle = ProjToBuffer(extent);
                     if (rectangle.Width * rectangle.Height != 0)
                     {
                         using (Graphics g = Graphics.FromImage(tmpBuffer))
                         {
                             using (Brush brush = new SolidBrush(BackGround))
                             {
                                 g.FillRectangle(brush, rectangle);
                             }

                             int count = 2;
                             var visibleLayers = Layers.Where(x => x.GetVisible(extent, rectangle));
                             for (int i = 0; i < count; i++)
                             {
                                 if (CancellationTokenSource?.IsCancellationRequested == true)
                                 {
                                     break;
                                 }
                                 bool selected = i == 1;
                                 Draw(g, rectangle, extent, selected, ProgressHandler, CancellationTokenSource);
                             }
                         }
                     }
                     #endregion
                 }
                 BackBuffer = tmpBuffer;
             });
        }
        private List<IFeatureLayer> GetFeatureLayers(IGroup mapGroup)
        {
            List<IFeatureLayer> mapFeatureLayers = new List<IFeatureLayer>();
            foreach (var layer in mapGroup.Layers)
            {
                if (layer is IGroup group)
                {
                    mapFeatureLayers.AddRange(GetFeatureLayers(group));
                }
                else if (layer is IFeatureLayer mapFeatureLayer)
                {
                    mapFeatureLayers.Add(mapFeatureLayer);
                }
            }
            return mapFeatureLayers;
        }
        public IFeatureLayer[] GetFeatureLayers()
        {
            List<IFeatureLayer> mapFeatureLayers = GetFeatureLayers(this);
            return mapFeatureLayers.ToArray();
        }

        public void Draw(Graphics g, Rectangle rectangle)
        {
            if (BackBuffer != null && g != null)
            {
                g.DrawImage(BackBuffer, rectangle.X, rectangle.Y);
            }
        }
        public IBaseLayer AddLayer(string fileName)
        {
            IBaseLayer layer = null;
            Dataset dataset = LayerFactory.OpenDataset(fileName);
            if (dataset != null)
            {
                layer = AddLayer(dataset);
            }
            else
            {
                DataSource dataSource = LayerFactory.OpenDataSource(fileName);
                layer = AddLayer(dataSource);
            }
            return layer;
        }

        public IFeatureLayer AddLayer(DataSource dataSource)
        {
            IFeatureLayer layer = null;
            if (dataSource != null)
            {
                if (dataSource.GetLayerCount() > 0)
                {
                    wkbGeometryType wkbGeometryType = wkbGeometryType.wkbNone;
                    using (Layer layerItem = dataSource.GetLayerByIndex(0))
                    {
                        wkbGeometryType = layerItem.GetGeomType();
                    }
                    switch (wkbGeometryType)
                    {
                        case wkbGeometryType.wkbPoint:
                            layer = new PointLayer(dataSource);
                            break;
                        case wkbGeometryType.wkbLineString:
                            layer = new LineLayer(dataSource);
                            break;
                        case wkbGeometryType.wkbPolygon:
                            layer = new PolygonLayer(dataSource);
                            break;
                    }
                    if (layer != null)
                    {
                        Layers.Add(layer);
                    }
                }
            }
            return layer;
        }

        public IRasterLayer AddLayer(Dataset dataset)
        {
            IRasterLayer layer = null;
            if (dataset != null)
            {
                layer = new RasterLayer(dataset);
                Layers.Add(layer);
            }
            return layer;
        }

        public void ResetExtents()
        {
            Extent env = BufferToProj(ViewBounds);
            ViewExtent = env;
        }
        public Extent BufferToProj(Rectangle rect)
        {
            Point tl = new Point(rect.X, rect.Y);
            Point br = new Point(rect.Right, rect.Bottom);

            Coordinate topLeft = BufferToProj(tl);
            Coordinate bottomRight = BufferToProj(br);
            return new Extent(topLeft.X, bottomRight.Y, bottomRight.X, topLeft.Y);
        }
        public Coordinate BufferToProj(Point position)
        {
            double x = Convert.ToDouble(position.X);
            double y = Convert.ToDouble(position.Y);
            x = (x * ViewExtent.Width / _width) + ViewExtent.MinX;
            y = ViewExtent.MaxY - (y * ViewExtent.Height / _height);

            return new Coordinate(x, y, 0.0);
        }

        public void Resize(int width, int height)
        {
            var diff = new Point
            {
                X = width - _width,
                Y = height - _height
            };
            var newView = new Rectangle(ViewBounds.X, ViewBounds.Y, ViewBounds.Width + diff.X, ViewBounds.Height + diff.Y);

            // Check for minimal size of view.
            if (newView.Width < 5) newView.Width = 5;
            if (newView.Height < 5) newView.Height = 5;

            ViewBounds = newView;
            ResetExtents();

            _width = width;
            _height = height; 
            Bounds = new Rectangle(0, 0, _width, _height);
        }

        public void ZoomToMaxExtent()
        {
            ViewExtent = GetMaxExtent(true);
        }
        public Extent GetMaxExtent(bool expand = false)
        {
            // to prevent exception when zoom to map with one layer with one point
            const double Eps = 1e-7;
            var maxExtent = Extent.Width < Eps || Extent.Height < Eps ? new Extent(Extent.MinX - Eps, Extent.MinY - Eps, Extent.MaxX + Eps, Extent.MaxY + Eps) : Extent;
            if (expand) maxExtent.ExpandBy(maxExtent.Width / 10, maxExtent.Height / 10);
            return maxExtent;
        }
    }
}
