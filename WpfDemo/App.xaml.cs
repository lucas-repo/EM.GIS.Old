using OSGeo.GDAL;
using OSGeo.OGR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
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
        private Assembly GetAssembly(string directory, string assemblyName)
        {
            Assembly assembly = null;
            string path = Path.Combine(directory, assemblyName);
            if (File.Exists(path))
            {
                assembly = Assembly.LoadFrom(path);
            }
            if (assembly == null)
            {
                string[] directories = Directory.GetDirectories(directory);
                foreach (var item in directories)
                {
                    assembly = GetAssembly(item, assemblyName);
                    if (assembly != null)
                    {
                        break;
                    }
                }
            }
            return assembly;
        }
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string privatePath = "Dependencies";
            Assembly assembly = null;
            string[] directoryNames = privatePath.Split(';');
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string extension = Path.GetExtension(args.RequestingAssembly.CodeBase);
            string[] arry = args.Name.Split(',');
            string name = arry[0];
            string assemblyName = $"{name}{extension}";
            foreach (var directoryName in directoryNames)
            {
                string directory = Path.Combine(baseDirectory, directoryName);
                assembly = GetAssembly(directory, assemblyName);
                if (assembly != null)
                {
                    break;
                }
            }
            return assembly;
        }
    }
}
