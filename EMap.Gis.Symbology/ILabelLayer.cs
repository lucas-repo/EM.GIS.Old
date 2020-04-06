using EMap.Gis.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMap.Gis.Symbology
{
    /// <summary>
    /// Interface for LabelLayer.
    /// </summary>
    public interface ILabelLayer : IBaseLayer
    {
        #region Properties

        /// <summary>
        /// Gets or sets an optional layer to link this layer to. If this is specified, then drawing will
        /// be associated with this layer.
        /// </summary>
        IFeatureLayer FeatureLayer { get; set; }

        new ILabelCategory DefaultCategory { get; set; }
        /// <summary>
        /// Gets or sets the symbology
        /// </summary>
        new ILabelScheme Symbology { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the current selection, reverting the geometries back to their normal colors.
        /// </summary>
        void ClearSelection();

        /// <summary>
        /// Expression creates the labels based on the given string expression. Field names in
        /// square brackets will be replaced by the values for those fields in the FeatureSet.
        /// </summary>
        void CreateLabels();

        /// <summary>
        /// Invalidates any cached content for this layer.
        /// </summary>
        void Invalidate();

        /// <summary>
        /// Highlights the values from a specified region. This will not unselect any members,
        /// so if you want to select a new region instead of an old one, first use ClearSelection.
        /// This is the default selection that only tests the anchor point, not the entire label.
        /// </summary>
        /// <param name="region">An Envelope showing a 3D selection box for intersection testing.</param>
        /// <returns>True if any members were added to the current selection.</returns>
        bool Select(Extent region);

        #endregion
    }
}
