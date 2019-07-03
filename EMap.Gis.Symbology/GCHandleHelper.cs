using System;
using System.Runtime.InteropServices;

namespace EMap.Gis.Symbology
{
    public static class GCHandleHelper
    {
        public static IntPtr GetIntPtr(object obj)
        {
            GCHandle handle = GCHandle.Alloc(obj, GCHandleType.Pinned);
            IntPtr bufferPtr = handle.AddrOfPinnedObject();
            if (handle.IsAllocated) { handle.Free(); }
            return bufferPtr;
        }
    }
}