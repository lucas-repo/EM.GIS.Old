namespace EMap.Gis.Data
{
    /// <summary>
    /// Supports bounding in the M Dimension
    /// </summary>
    public interface IExtentM
    {
        /// <summary>
        /// Gets or sets the minimum M value
        /// </summary>
        double MinM { get; set; }

        /// <summary>
        /// Gets or sets the maximum M value
        /// </summary>
        double MaxM { get; set; }
    }
}