using EM.GIS.Controls;
using EM.GIS.Symbology;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EM.GIS.WPFControls
{
    /// <summary>
    /// Legend.xaml 的交互逻辑
    /// </summary>
    public partial class Legend : TreeView, ILegend
    {
        public Legend()
        {
            InitializeComponent();
            LegendItems = new LayerCollection();
            ItemsSource = LegendItems;
        }

        public ILegendItemCollection LegendItems { get; }

        public void AddMapFrame(IFrame mapFrame)
        {
            LegendItems.Add(mapFrame);
        }
    }
}
