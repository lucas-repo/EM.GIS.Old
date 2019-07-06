using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    public static class CloneableEm
    {
        public static T Copy<T>(this T original)
            where T : class, ICloneable
        {
            return original?.Clone() as T;
        }
    }
}
