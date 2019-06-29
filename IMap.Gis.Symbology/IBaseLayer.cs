using System;

namespace IMap.Gis.Symbology
{
    public interface IBaseLayer : IGetImage,IDisposable
    {
        IScheme Symbology { get; set; }
    }
}
