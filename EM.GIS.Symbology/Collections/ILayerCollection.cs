using EM.GIS.Data;
using System;
using System.Collections.Generic;

namespace EM.GIS.Symbology
{
    public interface ILayerCollection : IItemCollection<IGroup, ILayer>
    {
        /// <summary>
        /// Gets or sets the progress handler to report progress for time consuming actions.
        /// </summary>
        IProgressHandler ProgressHandler { get; set; }

        /// <summary>
        /// Adds the specified fileName to the map as a new layer.
        /// </summary>
        /// <param name="path">The string fileName to add as a layer.</param>
        /// <returns>An IMapLayer that is the layer handle for the specified file.</returns>
        ILayer AddLayer(string path);
        /// <summary>
        /// Adds the dataset specified to the file. Depending on whether this is a featureSet,
        /// Raster, or ImageData, this will return the appropriate layer for the map.
        /// </summary>
        /// <param name="dataSet">A dataset</param>
        /// <returns>The IMapLayer to add</returns>
        ILayer Add(IDataSet dataSet);
        /// <summary>
        /// This overload automatically constructs a new MapLayer from the specified
        /// feature layer with the default drawing characteristics and returns a valid
        /// IMapLayer which can be further cast into a PointLayer, MapLineLayer or
        /// a PolygonLayer, depending on the data that is passed in.
        /// </summary>
        /// <param name="featureSet">Any valid IFeatureSet that does not yet have drawing characteristics</param>
        /// <returns>A newly created valid implementation of FeatureLayer which at least gives a few more common
        /// drawing related methods and can also be cast into the appropriate Point, Line or Polygon layer.</returns>
        IFeatureLayer Add(IFeatureSet featureSet);
        /// <summary>
        /// Adds the specified raster as a new layer
        /// </summary>
        /// <param name="raster">The raster to add as a layer</param>
        /// <returns>the MapRasterLayer interface</returns>
        IRasterLayer Add(IRasterSet raster);
    }
}
