using System;
using System.Collections.Generic;
using System.Drawing;

namespace EMap.Gis.Symbology
{
    public interface ILegendItem : IDescriptor, IChangeItem, IParentItem<ILegendItem>
    {
        /// <summary>
        /// Gets or sets a value indicating whether or not this legend item should be visible.
        /// This will not be altered unless the LegendSymbolMode is set to CheckBox.
        /// </summary>
        bool Checked { get; set; }

        /// <summary>
        /// Gets or sets a list of menu items that should appear for this layer.
        /// These are in addition to any that are supported by default.
        /// Handlers should be added to this list before it is retrieved.
        /// </summary>
        List<SymbologyMenuItem> ContextMenuItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this legend item is expanded.
        /// </summary>
        bool IsExpanded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this legend item is currently selected (and therefore drawn differently).
        /// </summary>
        bool IsSelected { get; set; }
        SymbolMode LegendSymbolMode { get; set; }
        LegendType LegendType { get; set; }
        bool IsVisible { get; set; }
        bool LegendItemVisible { get; set; }
        string LegendText { get; set; }
        Size GetLegendSymbolSize();
        void DrawLegend(Graphics context, Rectangle rectangle);
    }
}