using System;

namespace EMap.Gis.Symbology
{
    public interface IBaseLayer : IGetImage,IDisposable
    {
        IScheme Symbology { get; set; }
    }
}
