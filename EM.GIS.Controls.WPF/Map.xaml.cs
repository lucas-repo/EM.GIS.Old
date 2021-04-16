﻿using EM.GIS.Controls;
using EM.GIS.Data;
using EM.GIS.Geometries;
using EM.GIS.Symbology;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EM.GIS.WPFControls
{
    /// <summary>
    /// Map.xaml 的交互逻辑
    /// </summary>
    public partial class Map : UserControl, IMap
    {
        public IFrame MapFrame { get; set; }
        public bool IsBusy { get; set; }
        private ILegend _legend;

        public ILegend Legend
        {
            get { return _legend; }
            set
            {
                _legend = value;
                _legend?.AddMapFrame(MapFrame);
            }
        }

        public IExtent ViewExtent { get => MapFrame.ViewExtent; set => MapFrame.ViewExtent = value; }

        public ILayerCollection Layers => MapFrame.Layers;

        public Rectangle ViewBound { get => MapFrame.ViewBound; set => MapFrame.ViewBound = value; }
        public List<ITool> MapTools { get; }
        public IExtent Extent { get => (MapFrame as IProj).Extent; set => (MapFrame as IProj).Extent = value; }
        public Rectangle Bound { get => MapFrame.Bound; set => MapFrame.Bound = value; }

        public event EventHandler<IGeoMouseEventArgs> GeoMouseMove;
        public Map()
        {
            InitializeComponent();
            MapFrame = new Symbology.Frame((int)ActualWidth, (int)ActualHeight)
            {
                Text="地图框"
            };
            MapFrame.BufferChanged += MapFrame_BufferChanged;
            MapFrame.ViewBoundChanged += MapFrame_ViewBoundChanged;
            var pan = new MapToolPan(this);
            var zoom = new MapToolZoom(this);
            ITool[] mapTools = { pan, zoom };
            MapTools = new List<ITool>();
            MapTools.AddRange(mapTools);
            foreach (var mapTool in MapTools)
            {
                mapTool.Activated += MapTool_Activated;
            }
            ActivateMapFunctionWithZoom(pan);
        }

        private void MapFrame_ViewBoundChanged(object? sender, EventArgs e)
        {
            Invalidate();
        }

        private void MapTool_Activated(object? sender, EventArgs e)
        {
            if (sender is IMapTool mapTool)
            {
                if (mapTool.Cursor != null)
                {
                    Cursor = new Cursor(mapTool.Cursor);
                }
                else if (mapTool is MapToolPan)
                {
                    Cursor = Cursors.SizeAll;
                }
                else
                {
                    if (Cursor != Cursors.Arrow)
                    {
                        Cursor = Cursors.Arrow;
                    }
                }
            }
        }

        private void MapFrame_BufferChanged(object? sender, EventArgs e)
        {
            Invalidate();
        }

        public IList<ILayer> AddLayers()
        {
            var layers = new List<ILayer>();
            OpenFileDialog dg = new OpenFileDialog()
            {
                Filter = "*.img,*.shp|*.img;*.shp",
                Multiselect = true
            };
            if (dg.ShowDialog(Window.GetWindow(this)).HasValue)
            {
                foreach (var fileName in dg.FileNames)
                {
                    var layer = Layers.AddLayer(fileName);
                    if (layer != null)
                    {
                        layers.Add(layer);
                    }
                }
            }
            return layers;
        }

        public void Invalidate(IExtent extent)
        {
            Invalidate();
        }

        public void Invalidate()
        {
            var action = new Action(() =>
            {
                InvalidateVisual();
                //UpdateLayout();
            });
            Dispatcher.BeginInvoke(action);
        }
        public void ZoomToMaxExtent()
        {
            MapFrame.ZoomToMaxExtent();
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (MapFrame?.BackBuffer is Bitmap)
            {
                if (Bound.Width <= 0 || Bound.Height <= 0)
                {
                    return;
                }
                BitmapSource bitmapSource = null;
                using (Bitmap bmp = new Bitmap(Bound.Width, Bound.Height))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        MapFrame.Draw(g, Bound);
                    }
                    bitmapSource = bmp.ToBitmapImage();
                }
                var rect = Bound.ToRect();
                double offsetX = (ActualWidth - Bound.Width) / 2.0;
                double offsetY = (ActualHeight - Bound.Height) / 2.0;
                Transform transform = new TranslateTransform(offsetX, offsetY);
                drawingContext.PushTransform(transform);
                drawingContext.DrawImage(bitmapSource, rect);
            }
            base.OnRender(drawingContext);
        }
        public ILayer AddLayer()
        {
            try
            {
                return Layers.AddLayer(DataFactory.Default.DriverFactory.OpenFile());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return null;
        }
        public void ActivateMapFunctionWithZoom(IMapTool function)
        {
            if (function == null)
            {
                return;
            }
            if (!(function is MapToolZoom))
            {
                var mapToolZoom = MapTools.FirstOrDefault(x => x is MapToolZoom);
                if (mapToolZoom != null)
                {
                    ActivateMapFunction(mapToolZoom);
                }
            }
            ActivateMapFunction(function);
        }
        public void ActivateMapFunction(ITool function)
        {
            if (function == null)
            {
                return;
            }
            if (!MapTools.Contains(function))
            {
                MapTools.Add(function);
            }

            foreach (var f in MapTools)
            {
                if ((f.MapToolMode & MapToolMode.AlwaysOn) == MapToolMode.AlwaysOn) continue;
                int test = (int)(f.MapToolMode & function.MapToolMode);
                if (test > 0) f.Deactivate();
            }
            function.Activate();

        }

        public void DeactivateAllMapTools()
        {
            foreach (var f in MapTools)
            {
                if ((f.MapToolMode & MapToolMode.AlwaysOn) != MapToolMode.AlwaysOn) f.Deactivate();
            }
        }
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            if (MapFrame != null && sizeInfo.NewSize.Width > 0 && sizeInfo.NewSize.Height > 0)
            {
                MapFrame.Resize((int)sizeInfo.NewSize.Width, (int)sizeInfo.NewSize.Height);
            }
            base.OnRenderSizeChanged(sizeInfo);
        }
        #region 鼠标事件
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            var args = new GeoMouseButtonEventArgs(e, this);
            foreach (IMapTool tool in MapTools.Where(_ => _.IsActivated))
            {
                tool.DoMouseDoubleClick(args);
                if (args.Handled) break;
            }
            base.OnMouseDoubleClick(e);
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            var args = new GeoMouseButtonEventArgs(e, this);
            foreach (IMapTool tool in MapTools.Where(_ => _.IsActivated))
            {
                tool.DoMouseDown(args);
                if (args.Handled) break;
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            var args = new GeoMouseEventArgs(e, this);
            foreach (IMapTool tool in MapTools.Where(_ => _.IsActivated))
            {
                tool.DoMouseMove(args);
                if (args.Handled) break;
            }
            GeoMouseMove?.Invoke(this, args);
            base.OnMouseMove(e);
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            var args = new GeoMouseButtonEventArgs(e, this);
            foreach (IMapTool tool in MapTools.Where(_ => _.IsActivated))
            {
                tool.DoMouseUp(args);
                if (args.Handled) break;
            }
            base.OnMouseUp(e);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            var args = new GeoMouseWheelEventArgs(e, this);
            foreach (IMapTool tool in MapTools.Where(_ => _.IsActivated))
            {
                tool.DoMouseWheel(args);
                if (args.Handled) break;
            }
            base.OnMouseWheel(e);
        }
        protected override void OnKeyUp(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            foreach (IMapTool tool in MapTools.Where(_ => _.IsActivated))
            {
                tool.DoKeyUp(e);
                if (e.Handled) break;
            }
            base.OnKeyUp(e);//todo 待完成
        }
        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            foreach (IMapTool tool in MapTools.Where(_ => _.IsActivated))
            {
                tool.DoKeyDown(e);
                if (e.Handled) break;
            }
            base.OnKeyDown(e);
        }
        #endregion
    }
}
