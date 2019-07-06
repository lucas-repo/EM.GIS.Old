using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Data
{
    public interface ICancelProgressHandler : IProgressHandler
    {
        /// <summary>
        /// Gets a value indicating whether the progress handler has been notified that the running process should be cancelled.
        /// </summary>
        bool Cancel { get; }
    }
}
