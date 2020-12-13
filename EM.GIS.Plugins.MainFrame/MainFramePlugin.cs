using AvalonDock;
using EM.GIS.Controls;
using EM.GIS.WPFControls;
using Fluent;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EM.GIS.Plugins.MainFrame
{
    public class MainFramePlugin : WpfPlugin
    {
        public override int Priority => -1000;
        public override void Activate()
        {
            if (App.Window != null && App.Window.Content is Grid grid && grid.Children.Count == 0 && App.Ribbon == null)
            {
                //添加行
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

                //添加ribbon
                RibbonHelper ribbonHelper = new RibbonHelper(App);
                DockHelper dockHelper = new DockHelper(App);
                Ribbon ribbon = ribbonHelper.GetRibbon();
                DockingManager dockingManager = dockHelper.GetDockingManager();
                StatusBar statusBar = ribbonHelper.GetStatusBar();
                grid.Children.Add(ribbon);
                grid.Children.Add(dockingManager);
                grid.Children.Add(statusBar);
                Grid.SetRow(dockingManager, 1);
                Grid.SetRow(statusBar,2);
                App.Ribbon = ribbon;
                App.DockingManager = dockingManager;
                App.StatusBar = statusBar;
            }
            base.Activate();
        }


        public override void Deactivate()
        {
            App.Map = null;
            App.Ribbon = null;
            App.StatusBar = null;
            App.DockingManager = null;
            base.Deactivate();
        }
    }
}
