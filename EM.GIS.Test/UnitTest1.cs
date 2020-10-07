using EM.GIS.Data;
using EM.GIS.Serialization;
using EM.GIS.Symbology;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OSGeo.GDAL;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;

namespace EM.GIS.Test
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//todo 注册编码，注册gdal使用的GBK编码
            Gdal.AllRegister();
            Ogr.RegisterAll();
            // 为了支持中文路径，请添加下面这句代码  
            if (Encoding.Default.EncodingName == Encoding.UTF8.EncodingName && Encoding.Default.CodePage == Encoding.UTF8.CodePage)
            {
                Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            }
            else
            {
                Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "NO");
            }
            // 为了使属性表字段支持中文，请添加下面这句  
            Gdal.SetConfigOption("SHAPE_ENCODING", "");
        }
        /// <summary>
        /// 在指定的私有路径查找程序集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string privatePath = "Dependencies";
            Assembly assembly = null;
            string[] directoryNames = privatePath.Split(';');
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string extension = Path.GetExtension(args.RequestingAssembly.CodeBase);
            string directory = null;
            string[] arry = args.Name.Split(',');
            string name = arry[0];
            string path = null;
            foreach (var directoryName in directoryNames)
            {
                directory = Path.Combine(baseDirectory, directoryName);
                path = Path.Combine(directory, $"{name}{extension}");
                if (File.Exists(path))
                {
                    assembly = Assembly.LoadFrom(path);
                    if (assembly != null)
                    {
                        break;
                    }
                }
            }
            return assembly;
        }
        [TestMethod]
        public void TestLayerGetImage()
        {
            List<string> fileNames = new List<string>()
            {
                @"E:\LC\数据\line1.shp",
                //@"E:\LC\数据\polygon.shp" ,
                //@"E:\LC\数据\双流\2014年裁剪影像.img"
            };
            foreach (var fileName in fileNames)
            {
                using (var layer = LayerManager.Default.OpenLayer(fileName))
                {
                    if (layer is IFeatureLayer featureLayer)
                    {
                        var lineSymbolizer = featureLayer.DefaultCategory.Symbolizer as ILineSymbolizer;
                        if (lineSymbolizer != null)
                        {
                            var marker = new PointSymbolizer();
                            marker.Symbols[0] = new PointSimpleSymbol(Color.Red, PointShape.Triangle, 10)
                            {
                                UseOutLine = false
                            };
                            ILineSymbol symbol = new LineMarkerSymbol
                            {
                                Color = Color.Black,
                                Width = 3,
                                Marker = marker
                            };
                            lineSymbolizer.Symbols[0] = symbol;
                            lineSymbolizer.Symbols.Insert(0, new LineSimpleSymbol());
                        }
                    }

                    Rectangle rectangle = new Rectangle(0, 0, 256, 256);
                    using (var image = new Bitmap(rectangle.Width, rectangle.Height))
                    {
                        //var marker = new PointSymbolizer();
                        //marker.Symbols[0] = new PointSimpleSymbol(Color.Red, PointShape.Triangle, 10)
                        //{
                        //    UseOutLine = false
                        //};
                        //SizeF size = marker.Size;
                        //int imgWidth = (int)Math.Ceiling(size.Width);
                        //int imgHeight = (int)Math.Ceiling(size.Height);
                        ////Image markerImage = Image.Load(@"C:\Users\lc156\Desktop\arrow.png");
                        //Image markerImage = new Bitmap(imgWidth, imgHeight);
                        //marker.Symbols[0].Angle = -45;
                        //using (Graphics g = Graphics.FromImage(markerImage))
                        //{
                        //    marker.DrawLegend(g, new Rectangle(0, 0, imgWidth, imgHeight));
                        //}
                        //var brush = new TextureBrush(markerImage);
                        //float[] dashPattern = null;
                        //var pen = new Pen(brush, size.Height)
                        //{
                        //    DashPattern= dashPattern
                        //};
                        //using (Graphics g = Graphics.FromImage(image))
                        //{
                        //    var points = new PointF[] { new PointF(0, 255), new PointF(255, 0) };
                        //    //var points = new PointF[] { new PointF(0, 0), new PointF(255, 255) };
                        //    //var points = new PointF[] { new PointF(0, 128), new PointF(255, 128) };
                        //    g.DrawLines(pen, points);
                        //    g.DrawLines(new Pen(Color.Blue, 1), points);
                        //    //marker.DrawPoint(x, 1, new PointF(128, 128));
                        //    //lineSymbolizer.DrawLine(x, 1, new PointF[] { new PointF(0, 128), new PointF(128, 128) }.ToPath());
                        //}
                        var extent = layer.Extent.Copy();
                        ViewHelper.GetViewEnvelope(extent, rectangle);
                        using (Graphics g = Graphics.FromImage(image))
                        {
                            MapArgs mapArgs = new MapArgs(rectangle, extent, g);
                            layer.Draw(g, rectangle, extent);
                        }
                        image.Save(@"C:\Users\lc156\Desktop\tmp\123.png");
                    }
                }
            }
        }
    }
}
