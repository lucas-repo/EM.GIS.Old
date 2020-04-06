using EMap.Gis.Data;
using System;
using System.Drawing;
using System.Threading;

namespace EMap.Gis.Symbology
{
    public interface IBaseLayer :  IDisposable,IDynamicVisibility
    {
        bool Name { get; set; }
        bool AliasName { get; set; }
        bool IsVisible { get; set; }
        Extent Extent { get;  }
        IScheme Symbology { get; set; }
        ICategory DefaultCategory { get; set; }
        bool GetVisible(Extent extent,Rectangle rectangle);
        void Draw(Graphics graphics,Rectangle rectangle, Extent extent, bool selected=false, ProgressHandler progressHandler = null, CancellationTokenSource cancellationTokenSource = null);
    }
}
