using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Data
{
    public interface IFeature:IDisposable, ICloneable, IComparable<IFeature>
    {
    }
}
