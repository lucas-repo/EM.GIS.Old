using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Serialization
{
    /// <summary>
    /// 拷贝扩展
    /// </summary>
    public static class CopyExtension
    {
        /// <summary>
        /// 拷贝一份副本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <returns></returns>
        public static T Copy<T>(this T original)
           where T : class, ICloneable
        {
            return original?.Clone() as T;
        }
    }
}
