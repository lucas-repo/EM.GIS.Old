using System;

namespace EMap.Gis.Symbology
{
    public interface IDescriptor : IRandomizable, ICloneable, IMatchable
    {
        void CopyProperties(object other);
    }
}