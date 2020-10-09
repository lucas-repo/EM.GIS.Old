﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace EM.GIS.Symbology
{
    /// <summary>
    /// 图例元素集合
    /// </summary>
    public class LegendItemCollection:ItemCollection<ILegendItem, ILegendItem>, ILegendItemCollection
    {
        public new ILegendItem Parent
        {
            get => base.Parent;
            set
            {
                if (base.Parent != value)
                {
                    base.Parent = value;
                    foreach (var item in this)
                    {
                        if (!item.Parent.Equals(Parent))
                        {
                            item.Parent = Parent;
                        }
                    }
                }
            }
        }
        public LegendItemCollection() 
        {
        }
        public LegendItemCollection(ILegendItem parent) : base(parent)
        { }
    }
}