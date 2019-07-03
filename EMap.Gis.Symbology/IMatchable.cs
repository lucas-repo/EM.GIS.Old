using System.Collections.Generic;

namespace EMap.Gis.Symbology
{
    public interface IMatchable
    {
        bool Matches(IMatchable other, out List<string> mismatchedProperties);
    }
}