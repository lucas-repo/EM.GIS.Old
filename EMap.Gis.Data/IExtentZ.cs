namespace EMap.Gis.Data
{
    /// <summary>
    /// The Extent interface for Z dimension extent bounds.
    /// </summary>
    public interface IExtentZ
    {
        /// <summary>
        /// Gets or sets the minimum in the Z dimension (usually the bottom).
        /// </summary>
        double MinZ { get; set; }

        /// <summary>
        /// Gets or sets the maximum in the Z dimension (usually the top).
        /// </summary>
        double MaxZ { get; set; }
    }
}