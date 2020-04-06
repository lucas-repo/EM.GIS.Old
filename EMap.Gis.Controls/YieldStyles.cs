using System;

namespace EMap.Gis.Controls
{
    [Flags]
    public enum YieldStyles
    {
        None=0,
        LeftButton=1,
        RightButton =2,
        Scroll = 4,
        Keyboard = 8,
        AlwaysOn = 16
    }
}