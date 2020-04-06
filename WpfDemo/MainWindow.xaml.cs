using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentRendered += MainWindow_ContentRendered;
            map.GeoMouseMove += Map_GeoMouseMove;
        }

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            map.MapFrame.ProgressHandler = ProgressHandler;
        }

        private void ProgressHandler(int percent, string message)
        {
            var action = new Action(() =>
            {
                progressBar.Value = percent;
                progressTBlock.Text = message;
            });
            Dispatcher.BeginInvoke(action);
        }
        private void Map_GeoMouseMove(object sender, EMap.Gis.Controls.GeoMouseArgs e)
        {
            coordTBlock.Text = $"{e.GeographicLocation.X},{e.GeographicLocation.Y}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            map.AddLayers();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            map.ZoomToMaxExtent();
        }
    }
}
