using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;

namespace WpfDemo
{
    public class Initializer
    {
        public void Initialize()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//todo 注册编码，注册gdal使用的GBK编码
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
                string[] directories = Directory.GetDirectories(directory,"*.dll", SearchOption.TopDirectoryOnly);
                foreach (var item in directories)
                {
                    assembly = Assembly.LoadFrom(item);  
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
            string privatePath = "Libs";
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
