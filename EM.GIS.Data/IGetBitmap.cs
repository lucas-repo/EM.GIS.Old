using EM.GIS.Geometries;
using System.Drawing;

namespace EM.GIS.Data
{
    public interface IGetBitmap
    {
        /// <summary>
        /// Attempts to create a bitmap for the entire image. This may cause memory exceptions.
        /// </summary>
        /// <returns>
        /// A Bitmap of the image.
        /// </returns>
        Bitmap GetBitmap();

        /// <summary>
        /// The geographic envelope gives the region that the image should be created for.
        /// The window gives the corresponding pixel dimensions for the image, so that
        /// images matching the resolution of the screen can be used.
        /// </summary>
        /// <param name="envelope">
        /// The geographic extents to retrieve data for
        /// </param>
        /// <param name="size">
        /// The rectangle that defines the size of the drawing area in pixels
        /// </param>
        /// <returns>
        /// A bitmap captured from the main image
        /// </returns>
        Bitmap GetBitmap(IExtent envelope, Size size);

        /// <summary>
        /// The geographic envelope gives the region that the image should be created for.
        /// The window gives the corresponding pixel dimensions for the image, so that
        /// images matching the resolution of the screen can be used.
        /// </summary>
        /// <param name="envelope">
        /// The geographic extents to retrieve data for
        /// </param>
        /// <param name="window">
        /// The rectangle that defines the size of the drawing area in pixels
        /// </param>
        /// <returns>
        /// A bitmap captured from the main image
        /// </returns>
        Bitmap GetBitmap(IExtent envelope, Rectangle window);
    }
}