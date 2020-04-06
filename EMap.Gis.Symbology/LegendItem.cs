using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;



namespace EMap.Gis.Symbology
{
    [Serializable]
    public abstract class LegendItem : Descriptor, ILegendItem
    {
        private Size _legendSymbolSize;

        public event EventHandler ItemChanged;
        public event EventHandler RemoveItem;

        public bool IsVisible { get; set; }
        public bool IsExpanded { get; set; }
        public bool IsSelected { get; set; }

        public bool LegendItemVisible { get; set; } = true;
        public string LegendText { get; set; }
        public ILegendItem Parent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is checked.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Checked { get; set; }
        public List<SymbologyMenuItem> ContextMenuItems { get; set; }
        public SymbolMode LegendSymbolMode { get ; set ; }
        public LegendType LegendType { get; set; }

        public LegendItem()
        {
            _legendSymbolSize = new Size(16, 16);
        }
        public LegendItem(ILegendItem parent) : this()
        {
            Parent = parent;
        }
        public Size GetLegendSymbolSize()
        {
            return _legendSymbolSize;
        }

        public virtual void DrawLegend(Graphics g, Rectangle rectangle)
        {
            throw new NotImplementedException();
        }
    }
}